import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router, RouterModule, ActivatedRoute } from '@angular/router';
import { TeamService } from '../../../core/services/team.service';
import { TeamGroup, UpdateTeamGroup } from '../../../core/models/team.model';
import { HeaderComponent } from '../../../shared/components/header/header.component';
import { SidebarComponent } from '../../../shared/components/sidebar/sidebar.component';

@Component({
  selector: 'app-edit-team-group',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, RouterModule, HeaderComponent, SidebarComponent],
  templateUrl: './edit-team-group.component.html',
  styleUrls: ['./edit-team-group.component.scss']
})
export class EditTeamGroupComponent implements OnInit {
  editGroupForm!: FormGroup;
  teamGroup: TeamGroup | null = null;
  loading = true;
  submitting = false;
  error = '';
  success = '';
  teamId = '';
  groupId = '';

  constructor(
    private fb: FormBuilder,
    private route: ActivatedRoute,
    private router: Router,
    private teamService: TeamService
  ) {}

  ngOnInit(): void {
    this.teamId = this.route.snapshot.paramMap.get('teamId') || '';
    this.groupId = this.route.snapshot.paramMap.get('groupId') || '';

    this.initForm();

    if (this.groupId) {
      this.loadGroupDetails();
    }
  }

  initForm(): void {
    this.editGroupForm = this.fb.group({
      groupTag: ['', [Validators.required, Validators.maxLength(50)]],
      groupType: ['', [Validators.required, Validators.maxLength(100)]],
      isActive: [true]
    });
  }

  loadGroupDetails(): void {
    this.loading = true;
    this.teamService.getTeamGroupById(this.groupId).subscribe({
      next: (group) => {
        this.teamGroup = group;
        this.populateForm(group);
        this.loading = false;
      },
      error: (err) => {
        this.error = err.error?.message || err.message || 'Failed to load team group details';
        this.loading = false;
        console.error('Error loading team group:', err);
      }
    });
  }

  populateForm(group: TeamGroup): void {
    this.editGroupForm.patchValue({
      groupTag: group.groupTag,
      groupType: group.groupType,
      isActive: group.isActive
    });
  }

  onSubmit(): void {
    if (this.editGroupForm.invalid) {
      this.markFormGroupTouched(this.editGroupForm);
      return;
    }

    const formValue = this.editGroupForm.value;
    const updateData: UpdateTeamGroup = {
      teamGroupId: this.groupId,
      groupTag: formValue.groupTag,
      groupType: formValue.groupType,
      isActive: formValue.isActive
    };

    this.submitting = true;
    this.error = '';
    this.success = '';

    this.teamService.updateTeamGroup(this.groupId, updateData).subscribe({
      next: (updatedGroup) => {
        this.submitting = false;
        this.success = 'Team group updated successfully!';
        
        setTimeout(() => {
          this.goBack();
        }, 1500);
      },
      error: (err) => {
        this.submitting = false;
        this.error = err.error?.message || err.message || 'Failed to update team group. Please try again.';
        console.error('Error updating team group:', err);
      }
    });
  }

  goBack(): void {
    this.router.navigate(['/teams', this.teamId, 'groups', this.groupId]);
  }

  private markFormGroupTouched(formGroup: FormGroup): void {
    Object.keys(formGroup.controls).forEach(key => {
      const control = formGroup.get(key);
      control?.markAsTouched();
    });
  }

  isFieldInvalid(fieldName: string): boolean {
    const field = this.editGroupForm.get(fieldName);
    return !!(field && field.invalid && (field.dirty || field.touched));
  }

  getFieldError(fieldName: string): string {
    const field = this.editGroupForm.get(fieldName);
    
    if (field?.hasError('required')) {
      return `${this.getFieldLabel(fieldName)} is required`;
    }
    
    if (field?.hasError('maxlength')) {
      const maxLength = field.errors?.['maxlength'].requiredLength;
      return `${this.getFieldLabel(fieldName)} must not exceed ${maxLength} characters`;
    }
    
    return '';
  }

  private getFieldLabel(fieldName: string): string {
    const labels: { [key: string]: string } = {
      'groupTag': 'Group Tag',
      'groupType': 'Group Type'
    };
    return labels[fieldName] || fieldName;
  }
}

