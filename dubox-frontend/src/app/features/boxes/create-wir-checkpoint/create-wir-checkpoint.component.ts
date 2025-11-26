import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { WIRService } from '../../../core/services/wir.service';
import { CreateWIRCheckpointRequest, WIRRecord } from '../../../core/models/wir.model';
import { HeaderComponent } from '../../../shared/components/header/header.component';
import { SidebarComponent } from '../../../shared/components/sidebar/sidebar.component';

@Component({
  selector: 'app-create-wir-checkpoint',
  standalone: true,
  imports: [CommonModule, RouterModule, ReactiveFormsModule, HeaderComponent, SidebarComponent],
  templateUrl: './create-wir-checkpoint.component.html',
  styleUrls: ['./create-wir-checkpoint.component.scss']
})
export class CreateWIRCheckpointComponent implements OnInit {
  projectId!: string;
  boxId!: string;
  activityId!: string;
  wirRecord: WIRRecord | null = null;
  
  checkpointForm!: FormGroup;
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
    
    this.initForm();
    this.loadWIRRecord();
  }

  private initForm(): void {
    this.checkpointForm = this.fb.group({
      boxActivityId: [''], // Hidden field
      wirName: ['', [Validators.maxLength(200)]],
      wirDescription: ['', [Validators.maxLength(500)]],
      attachmentPath: ['', [Validators.maxLength(500)]],
      comments: ['', [Validators.maxLength(1000)]]
    });
  }

  loadWIRRecord(): void {
    this.loading = true;
    this.error = '';
    
    // Get WIR records for this activity
    this.wirService.getWIRRecordsByActivity(this.activityId).subscribe({
      next: (wirs) => {
        if (wirs && wirs.length > 0) {
          // Get the rejected WIR record
          this.wirRecord = wirs.find(w => w.status === 'Rejected') || wirs.find(w => w.status === 'Pending') || wirs[0];
          
          if (this.wirRecord) {
            // Set hidden field
            this.checkpointForm.patchValue({
              boxActivityId: this.activityId
            });
            
            // Pre-fill form with suggested values
            this.checkpointForm.patchValue({
              wirName: `${this.wirRecord.wirCode} - ${this.wirRecord.activityName}`,
              wirDescription: `WIR checkpoint for ${this.wirRecord.activityName}`
            });
          }
        } else {
          this.error = 'No WIR record found for this activity.';
        }
        this.loading = false;
      },
      error: (err) => {
        this.error = 'Failed to load WIR record';
        this.loading = false;
        console.error('Error loading WIR:', err);
      }
    });
  }

  onSubmit(): void {
    if (this.checkpointForm.invalid || !this.wirRecord) {
      this.markFormGroupTouched(this.checkpointForm);
      return;
    }

    this.submitting = true;
    this.error = '';

    const formValue = this.checkpointForm.value;
    const request: CreateWIRCheckpointRequest = {
      boxActivityId: formValue.boxActivityId, // From hidden field
      wirNumber: this.wirRecord.wirCode || '', // From WIRRecord
      wirName: formValue.wirName?.trim() || undefined,
      wirDescription: formValue.wirDescription?.trim() || undefined,
      attachmentPath: formValue.attachmentPath?.trim() || undefined,
      comments: formValue.comments?.trim() || undefined
    };

    this.wirService.createWIRCheckpoint(request).subscribe({
      next: (checkpoint) => {
        this.submitting = false;
        // Navigate to add checklist items page
        this.router.navigate([
          '/projects',
          this.projectId,
          'boxes',
          this.boxId,
          'activities',
          this.activityId,
          'wir-checkpoints',
          checkpoint.wirId,
          'add-checklist-items'
        ]);
      },
      error: (err) => {
        this.submitting = false;
        this.error = err.error?.message || err.message || 'Failed to create WIR checkpoint';
        console.error('Error creating WIR checkpoint:', err);
      }
    });
  }

  goBack(): void {
    this.router.navigate(['/projects', this.projectId, 'boxes', this.boxId]);
  }

  private markFormGroupTouched(formGroup: FormGroup): void {
    Object.keys(formGroup.controls).forEach(key => {
      const control = formGroup.get(key);
      control?.markAsTouched();
    });
  }
}

