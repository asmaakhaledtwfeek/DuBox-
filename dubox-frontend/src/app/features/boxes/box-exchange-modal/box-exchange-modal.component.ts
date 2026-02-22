import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { trigger, transition, style, animate } from '@angular/animations';
import { ProjectService } from '../../../core/services/project.service';
import { BoxService } from '../../../core/services/box.service';
import { Box } from '../../../core/models/box.model';

@Component({
  selector: 'app-box-exchange-modal',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './box-exchange-modal.component.html',
  styleUrls: ['./box-exchange-modal.component.scss'],
  animations: [
    trigger('fadeIn', [
      transition(':enter', [
        style({ opacity: 0 }),
        animate('200ms ease-out', style({ opacity: 1 }))
      ]),
      transition(':leave', [
        animate('150ms ease-in', style({ opacity: 0 }))
      ])
    ]),
    trigger('slideIn', [
      transition(':enter', [
        style({ transform: 'translateY(20px)', opacity: 0 }),
        animate('250ms ease-out', style({ transform: 'translateY(0)', opacity: 1 }))
      ]),
      transition(':leave', [
        animate('200ms ease-in', style({ transform: 'translateY(20px)', opacity: 0 }))
      ])
    ])
  ]
})
export class BoxExchangeModalComponent implements OnInit {
  @Input() isOpen = false;
  @Input() boxId!: string;
  @Input() projectId!: string;
  @Input() currentBuilding?: string;
  @Input() currentFloor?: string;
  @Input() currentBoxTag?: string;
  @Output() closeModal = new EventEmitter<void>();
  @Output() submitRequest = new EventEmitter<{
    newBuildingNumber: string | null;
    newFloor: string | null;
    requestReason: string;
    targetBoxId: string | null;
  }>();

  exchangeForm = {
    newBuildingNumber: null as string | null,
    newFloor: null as string | null,
    requestReason: '',
    targetBoxId: null as string | null
  };

  projectBuildings: any[] = [];
  projectLevels: any[] = [];
  loadingProjectConfig = false;

  // Box selection for exchange
  availableBoxes: Box[] = [];
  loadingBoxes = false;
  noBoxesFound = false;
  boxesErrorMessage = '';

  constructor(
    private projectService: ProjectService,
    private boxService: BoxService
  ) {}

  /** Buildings excluding current - for dropdown options */
  get availableBuildings(): any[] {
    if (!this.currentBuilding) return this.projectBuildings;
    const current = this.currentBuilding.trim().toUpperCase();
    return this.projectBuildings.filter(b => (b.buildingCode || '').trim().toUpperCase() !== current);
  }

  /** Levels excluding current - for dropdown options */
  get availableLevels(): any[] {
    if (!this.currentFloor) return this.projectLevels;
    const current = this.currentFloor.trim().toUpperCase();
    return this.projectLevels.filter(l => (l.levelCode || '').trim().toUpperCase() !== current);
  }

  /** Display building as name OR code (not both) */
  getBuildingDisplay(building: any): string {
    const name = building?.buildingName?.trim();
    const code = building?.buildingCode?.trim() || '';
    return name || code || '';
  }

  /** Display level as name OR code (not both) */
  getLevelDisplay(level: any): string {
    const name = level?.levelName?.trim();
    const code = level?.levelCode?.trim() || '';
    return name || code || '';
  }

  /** Get display label for selected building code */
  getSelectedBuildingDisplay(): string {
    const code = this.exchangeForm.newBuildingNumber;
    if (!code) return '';
    const b = this.projectBuildings.find(x => (x.buildingCode || '').trim() === code?.trim());
    return b ? this.getBuildingDisplay(b) : code;
  }

  /** Get display label for selected level code */
  getSelectedLevelDisplay(): string {
    const code = this.exchangeForm.newFloor;
    if (!code) return '';
    const l = this.projectLevels.find(x => (x.levelCode || '').trim() === code?.trim());
    return l ? this.getLevelDisplay(l) : code;
  }

  ngOnInit(): void {
    if (this.projectId) {
      this.loadProjectConfiguration();
    }
  }

  loadProjectConfiguration(): void {
    if (this.loadingProjectConfig) return;
    
    this.loadingProjectConfig = true;
    this.projectService.getProjectConfiguration(this.projectId).subscribe({
      next: (config) => {
        this.projectBuildings = config.buildings || [];
        this.projectLevels = config.levels || [];
        this.loadingProjectConfig = false;
      },
      error: (error) => {
        console.error('Error loading project configuration:', error);
        this.loadingProjectConfig = false;
      }
    });
  }

  /**
   * Load boxes when building or floor selection changes
   */
  onBuildingOrFloorChange(): void {
    // Reset box selection
    this.exchangeForm.targetBoxId = null;
    this.availableBoxes = [];
    this.noBoxesFound = false;
    this.boxesErrorMessage = '';

    // Check if at least one field is selected
    if (!this.exchangeForm.newBuildingNumber && !this.exchangeForm.newFloor) {
      return;
    }

    // Load boxes with same box type in selected building/floor
    this.loadBoxesForExchange();
  }

  /**
   * Load eligible boxes for exchange
   */
  loadBoxesForExchange(): void {
    if (!this.projectId || !this.boxId) return;

    this.loadingBoxes = true;
    this.noBoxesFound = false;
    this.boxesErrorMessage = '';

    const targetBuilding = this.exchangeForm.newBuildingNumber || this.currentBuilding;
    const targetFloor = this.exchangeForm.newFloor || this.currentFloor;

    this.boxService.getBoxesForExchange(
      this.projectId,
      this.boxId,
      targetBuilding,
      targetFloor
    ).subscribe({
      next: (boxes) => {
        this.availableBoxes = boxes;
        this.loadingBoxes = false;
        
        if (boxes.length === 0) {
          this.noBoxesFound = true;
          this.boxesErrorMessage = `No boxes with the same box type found in ${targetBuilding || 'the selected building'} - ${targetFloor || 'the selected floor'}.`;
        }
      },
      error: (error) => {
        console.error('Error loading boxes for exchange:', error);
        this.loadingBoxes = false;
        this.noBoxesFound = true;
        this.boxesErrorMessage = error?.error?.error || 'Failed to load boxes. Please try again.';
      }
    });
  }

  hasExchangeChanges(): boolean {
    return !!(this.exchangeForm.newBuildingNumber || this.exchangeForm.newFloor);
  }

  canSubmit(): boolean {
    // User must select building/floor, provide reason, and select a target box
    return this.hasExchangeChanges() 
      && !!this.exchangeForm.requestReason?.trim()
      && !!this.exchangeForm.targetBoxId;
  }

  onSubmit(): void {
    if (!this.canSubmit()) return;
    this.submitRequest.emit({
      newBuildingNumber: this.exchangeForm.newBuildingNumber,
      newFloor: this.exchangeForm.newFloor,
      requestReason: this.exchangeForm.requestReason,
      targetBoxId: this.exchangeForm.targetBoxId
    });
  }

  onClose(): void {
    this.resetForm();
    this.closeModal.emit();
  }

  resetForm(): void {
    this.exchangeForm = {
      newBuildingNumber: null,
      newFloor: null,
      requestReason: '',
      targetBoxId: null
    };
    this.availableBoxes = [];
    this.noBoxesFound = false;
    this.boxesErrorMessage = '';
  }

  onBackdropClick(event: MouseEvent): void {
    if (event.target === event.currentTarget) {
      this.onClose();
    }
  }
}
