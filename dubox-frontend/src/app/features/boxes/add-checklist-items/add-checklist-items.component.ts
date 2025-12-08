import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { FormBuilder, FormGroup, FormArray, Validators, ReactiveFormsModule } from '@angular/forms';
import { WIRService } from '../../../core/services/wir.service';
import { WIRCheckpoint, AddChecklistItemsRequest, PredefinedChecklistItem } from '../../../core/models/wir.model';
import { HeaderComponent } from '../../../shared/components/header/header.component';
import { SidebarComponent } from '../../../shared/components/sidebar/sidebar.component';

@Component({
  selector: 'app-add-checklist-items',
  standalone: true,
  imports: [CommonModule, RouterModule, ReactiveFormsModule, HeaderComponent, SidebarComponent],
  templateUrl: './add-checklist-items.component.html',
  styleUrls: ['./add-checklist-items.component.scss']
})
export class AddChecklistItemsComponent implements OnInit {
  projectId!: string;
  boxId!: string;
  activityId!: string;
  wirId!: string;
  
  wirCheckpoint: WIRCheckpoint | null = null;
  checklistForm!: FormGroup;
  loading = true;
  error = '';
  submitting = false;
  
  // Predefined checklist items
  predefinedChecklistItems: PredefinedChecklistItem[] = [];
  availablePredefinedItems: PredefinedChecklistItem[] = [];
  loadingPredefinedItems = false;
  selectedPredefinedItemIds: string[] = [];

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private fb: FormBuilder,
    private wirService: WIRService
  ) {}

  ngOnInit(): void {
    this.projectId = this.route.snapshot.params['projectId'];
    this.boxId = this.route.snapshot.params['boxId'];
    this.activityId = this.route.snapshot.params['activityId'];
    this.wirId = this.route.snapshot.params['wirId'];
    
    this.initForm();
    this.loadWIRCheckpoint();
    this.loadPredefinedChecklistItems();
  }

  private initForm(): void {
    this.checklistForm = this.fb.group({});
  }

  loadWIRCheckpoint(): void {
    this.loading = true;
    this.error = '';
    
    this.wirService.getWIRCheckpointById(this.wirId).subscribe({
      next: (checkpoint) => {
        this.wirCheckpoint = checkpoint;
        this.updateAvailablePredefinedItems();
        this.loading = false;
      },
      error: (err) => {
        this.error = 'Failed to load WIR checkpoint';
        this.loading = false;
        console.error('Error loading WIR checkpoint:', err);
      }
    });
  }

  private loadPredefinedChecklistItems(): void {
    this.loadingPredefinedItems = true;
    this.wirService.getPredefinedChecklistItems().subscribe({
      next: (items) => {
        this.predefinedChecklistItems = items;
        this.updateAvailablePredefinedItems();
        this.loadingPredefinedItems = false;
      },
      error: (err) => {
        console.error('Error loading predefined checklist items:', err);
        this.loadingPredefinedItems = false;
      }
    });
  }

  private updateAvailablePredefinedItems(): void {
    if (!this.wirCheckpoint || !this.predefinedChecklistItems.length) {
      this.availablePredefinedItems = [...this.predefinedChecklistItems];
      return;
    }

    // Get IDs of items already added to this checkpoint
    const addedPredefinedIds = new Set(
      (this.wirCheckpoint.checklistItems || [])
        .map(item => item.predefinedItemId)
        .filter(id => id != null) as string[]
    );

    // Filter out items that are already added
    this.availablePredefinedItems = this.predefinedChecklistItems.filter(
      item => !addedPredefinedIds.has(item.predefinedItemId)
    );
  }

  togglePredefinedItemSelection(itemId: string): void {
    const index = this.selectedPredefinedItemIds.indexOf(itemId);
    if (index > -1) {
      this.selectedPredefinedItemIds.splice(index, 1);
    } else {
      this.selectedPredefinedItemIds.push(itemId);
    }
  }

  isPredefinedItemSelected(itemId: string): boolean {
    return this.selectedPredefinedItemIds.includes(itemId);
  }

  onSubmit(): void {
    if (!this.wirCheckpoint || this.selectedPredefinedItemIds.length === 0) {
      this.error = 'Please select at least one predefined checklist item to add.';
      return;
    }

    this.submitting = true;
    this.error = '';

    const request: AddChecklistItemsRequest = {
      wirId: this.wirId,
      predefinedItemIds: this.selectedPredefinedItemIds
    };

    this.wirService.addChecklistItems(request).subscribe({
      next: (updatedCheckpoint) => {
        this.submitting = false;
        this.router.navigate([
          '/projects',
          this.projectId,
          'boxes',
          this.boxId,
          'activities',
          this.activityId,
          'qa-qc'
        ], { queryParams: { checklistAdded: 'true' } });
      },
      error: (err) => {
        this.submitting = false;
        this.error = err.error?.message || err.message || 'Failed to add checklist items';
        console.error('Error adding checklist items:', err);
      }
    });
  }

  goBack(): void {
    this.router.navigate(['/projects', this.projectId, 'boxes', this.boxId]);
  }

  private markFormGroupTouched(formGroup: FormGroup): void {
    Object.keys(formGroup.controls).forEach(key => {
      const control = formGroup.get(key);
      if (control instanceof FormArray) {
        control.controls.forEach(item => {
          if (item instanceof FormGroup) {
            this.markFormGroupTouched(item);
          } else {
            item.markAsTouched();
          }
        });
      } else if (control instanceof FormGroup) {
        this.markFormGroupTouched(control);
      } else {
        control?.markAsTouched();
      }
    });
  }
}

