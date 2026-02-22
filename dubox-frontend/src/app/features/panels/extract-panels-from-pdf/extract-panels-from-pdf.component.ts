import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
import { MatTooltipModule } from '@angular/material/tooltip';
import {
  PanelExtractionService,
  ReviewPanelTypeDto,
  ReviewBoxPanelDto,
  PanelExtractionReviewDto
} from '../../../core/services/panel-extraction.service';
import { BoxService } from '../../../core/services/box.service';
import { Box } from '../../../core/models/box.model';

export interface BoxTagMapping {
  boxTag: string;
  boxId?: string;
  newBoxName?: string;
}

@Component({
  selector: 'app-extract-panels-from-pdf',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    MatProgressSpinnerModule,
    MatSnackBarModule,
    MatTooltipModule
  ],
  templateUrl: './extract-panels-from-pdf.component.html',
  styleUrls: ['./extract-panels-from-pdf.component.scss']
})
export class ExtractPanelsFromPdfComponent implements OnInit {
  projectId!: string;
  boxId?: string;
  selectedFile?: File;
  selectedBoxTagsFile?: File;
  loading = false;
  extractionResult?: PanelExtractionReviewDto;
  existingBoxes: Box[] = [];
  boxTagMappings: BoxTagMapping[] = [];

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private panelExtractionService: PanelExtractionService,
    private boxService: BoxService,
    private snackBar: MatSnackBar
  ) {}

  ngOnInit(): void {
    this.projectId = this.route.snapshot.paramMap.get('projectId') || '';
    this.boxId = this.route.snapshot.queryParamMap.get('boxId') || undefined;
    this.loadExistingBoxes();
  }

  loadExistingBoxes(): void {
    this.boxService.getBoxesByProject(this.projectId).subscribe({
      next: (boxes) => {
        this.existingBoxes = boxes;
      },
      error: (error) => {
        console.error('Error loading boxes:', error);
      }
    });
  }

  onFileSelected(event: Event, isBoxTagsFile: boolean = false): void {
    const input = event.target as HTMLInputElement;
    if (input.files && input.files[0]) {
      const file = input.files[0];
      
      // Validate file type
      if (file.type !== 'application/pdf') {
        this.snackBar.open('Please select a PDF file', 'Close', { duration: 3000 });
        return;
      }

      // Validate file size (10MB limit)
      if (file.size > 10 * 1024 * 1024) {
        this.snackBar.open('File size must be less than 10MB', 'Close', { duration: 3000 });
        return;
      }

      if (isBoxTagsFile) {
        this.selectedBoxTagsFile = file;
      } else {
        this.selectedFile = file;
      }
    }
  }

  onDrop(event: DragEvent): void {
    event.preventDefault();
    event.stopPropagation();
    
    if (event.dataTransfer?.files && event.dataTransfer.files[0]) {
      const file = event.dataTransfer.files[0];
      
      if (file.type !== 'application/pdf') {
        this.snackBar.open('Please select a PDF file', 'Close', { duration: 3000 });
        return;
      }

      if (file.size > 10 * 1024 * 1024) {
        this.snackBar.open('File size must be less than 10MB', 'Close', { duration: 3000 });
        return;
      }

      this.selectedFile = file;
    }
  }

  onDragOver(event: DragEvent): void {
    event.preventDefault();
    event.stopPropagation();
  }

  extractPanels(): void {
    if (!this.selectedFile) {
      this.snackBar.open('Please select a PDF file first', 'Close', { duration: 3000 });
      return;
    }

    this.loading = true;
    this.extractionResult = undefined;

    this.panelExtractionService.extractPanelsFromPdf(
      this.projectId,
      this.selectedFile,
      this.boxId,
      this.selectedBoxTagsFile
    ).subscribe({
      next: (response) => {
        this.loading = false;
        if (response.isSuccess && response.data) {
          this.extractionResult = response.data;
          
          // Initialize box tag mappings
          if (this.extractionResult?.boxTags && this.extractionResult.boxTags.length > 0) {
            this.boxTagMappings = this.extractionResult.boxTags.map(tag => ({
              boxTag: tag,
              boxId: undefined,
              newBoxName: undefined
            }));
          }
          
          this.snackBar.open('Panel data extracted successfully! Please review and confirm.', 'Close', { duration: 5000 });
        } else {
          this.snackBar.open(response.error || 'Extraction failed', 'Close', { duration: 5000 });
        }
      },
      error: (error) => {
        this.loading = false;
        console.error('Extraction error:', error);
        this.snackBar.open('Error extracting panel data: ' + (error.error?.error || error.message), 'Close', { duration: 5000 });
      }
    });
  }

  confirmExtraction(): void {
    if (!this.extractionResult) {
      return;
    }

    this.loading = true;

    // Show info about unmapped tags (optional - user can choose to map only some)
    if (this.boxTagMappings && this.boxTagMappings.length > 0) {
      const mappedCount = this.boxTagMappings.filter(m => m.boxId).length;
      const unmappedCount = this.boxTagMappings.length - mappedCount;
      if (unmappedCount > 0) {
        console.log(`Processing ${mappedCount} mapped box tags, skipping ${unmappedCount} unmapped tags`);
      }
    }

    this.panelExtractionService.confirmPanelExtraction(
      this.projectId,
      {
        boxId: this.boxId,
        panelTypes: this.extractionResult.panelTypes,
        boxPanels: this.extractionResult.boxPanels,
        boxTagMappings: this.boxTagMappings
      }
    ).subscribe({
      next: (response) => {
        this.loading = false;
        if (response.isSuccess) {
          const result = response.data;
          this.snackBar.open(
            `Success! Created ${result.panelTypesCreated} panel types, updated ${result.panelTypesUpdated}, created ${result.boxPanelsCreated} box panels.`,
            'Close',
            { duration: 7000 }
          );
          this.router.navigate(['/projects', this.projectId]);
        } else {
          this.snackBar.open(response.error || 'Confirmation failed', 'Close', { duration: 5000 });
        }
      },
      error: (error) => {
        this.loading = false;
        console.error('Confirmation error:', error);
        this.snackBar.open('Error saving panel data: ' + (error.error?.error || error.message), 'Close', { duration: 5000 });
      }
    });
  }

  cancel(): void {
    this.router.navigate(['/projects', this.projectId]);
  }

  parseEmbeds(embedsJson?: string): any[] {
    if (!embedsJson) {
      return [];
    }
    try {
      return JSON.parse(embedsJson);
    } catch {
      return [];
    }
  }

  formatEmbedsTooltip(embedsJson?: string): string {
    const embeds = this.parseEmbeds(embedsJson);
    if (!embeds || embeds.length === 0) {
      return 'No embeds';
    }
    return embeds.map((e: any) => `${e.symbol}: ${e.description} (Qty: ${e.qty})`).join('\n');
  }

  getNewPanelCount(): number {
    return this.extractionResult?.panelTypes.filter(p => p.isNew).length || 0;
  }

  getExistingPanelCount(): number {
    return this.extractionResult?.panelTypes.filter(p => !p.isNew).length || 0;
  }

  onBoxMappingChange(mapping: BoxTagMapping, event: any): void {
    const value = event.target.value;
    if (value && value !== 'undefined') {
      mapping.boxId = value;
    } else {
      mapping.boxId = undefined;
    }
    mapping.newBoxName = undefined; // Not used anymore
  }

  getMappedCount(): number {
    return this.boxTagMappings.filter(m => m.boxId).length;
  }

  getUnmappedCount(): number {
    return this.boxTagMappings.length - this.getMappedCount();
  }
}

