import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { FormBuilder, FormGroup, FormArray, Validators, ReactiveFormsModule } from '@angular/forms';
import { WIRService } from '../../../core/services/wir.service';
import { WIRCheckpoint, AddChecklistItemsRequest, WIR_CHECKLIST_TEMPLATES } from '../../../core/models/wir.model';
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
  }

  private initForm(): void {
    this.checklistForm = this.fb.group({
      checklistItems: this.fb.array([])
    });
  }

  get checklistItems(): FormArray {
    return this.checklistForm.get('checklistItems') as FormArray;
  }

  loadWIRCheckpoint(): void {
    this.loading = true;
    this.error = '';
    
    this.wirService.getWIRCheckpointById(this.wirId).subscribe({
      next: (checkpoint) => {
        this.wirCheckpoint = checkpoint;
        this.loadChecklistTemplate();
        this.loading = false;
      },
      error: (err) => {
        this.error = 'Failed to load WIR checkpoint';
        this.loading = false;
        console.error('Error loading WIR checkpoint:', err);
      }
    });
  }

  loadChecklistTemplate(): void {
    if (!this.wirCheckpoint) return;

    // Clear existing items
    this.checklistItems.clear();
    
    // Always start with one empty item by default
    this.addChecklistItem();
  }

  addChecklistItem(): void {
    const newItem = this.createChecklistItemGroup();
    this.checklistItems.push(newItem);
    this.updateSequenceNumbers();
  }

  removeChecklistItem(index: number): void {
    this.checklistItems.removeAt(index);
    this.updateSequenceNumbers();
  }

  private createChecklistItemGroup(templateItem?: any): FormGroup {
    return this.fb.group({
      checkpointDescription: [templateItem?.checkpointDescription || '', Validators.required],
      referenceDocument: [templateItem?.referenceDocument || ''],
      sequence: [templateItem?.sequence || (this.checklistItems.length + 1), Validators.required]
    });
  }

  private updateSequenceNumbers(): void {
    // Update sequence numbers to be sequential (1, 2, 3, ...)
    this.checklistItems.controls.forEach((control, index) => {
      control.get('sequence')?.setValue(index + 1, { emitEvent: false });
    });
  }

  onSubmit(): void {
    if (this.checklistForm.invalid || !this.wirCheckpoint) {
      this.markFormGroupTouched(this.checklistForm);
      return;
    }

    this.submitting = true;
    this.error = '';

    const request: AddChecklistItemsRequest = {
      wirId: this.wirId,
      items: this.checklistItems.value.map((item: any) => ({
        checkpointDescription: item.checkpointDescription.trim(),
        referenceDocument: item.referenceDocument?.trim() || undefined,
        sequence: item.sequence
      }))
    };

    this.wirService.addChecklistItems(request).subscribe({
      next: (updatedCheckpoint) => {
        this.submitting = false;
        alert('Checklist items added successfully!');
        // Navigate to review page (qa-qc-checklist with review step)
        this.router.navigate([
          '/projects',
          this.projectId,
          'boxes',
          this.boxId,
          'activities',
          this.activityId,
          'qa-qc'
        ]);
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

