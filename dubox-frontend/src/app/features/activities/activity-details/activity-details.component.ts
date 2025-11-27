import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { BoxService } from '../../../core/services/box.service';
import { BoxActivity } from '../../../core/models/box.model';
import { HeaderComponent } from '../../../shared/components/header/header.component';
import { SidebarComponent } from '../../../shared/components/sidebar/sidebar.component';
import { UpdateProgressModalComponent } from '../update-progress-modal/update-progress-modal.component';
import { ProgressUpdateService } from '../../../core/services/progress-update.service';
import { BoxActivityDetail, ProgressUpdate } from '../../../core/models/progress-update.model';
import { TeamService } from '../../../core/services/team.service';
import { Team, TeamMember } from '../../../core/models/team.model';

@Component({
  selector: 'app-activity-details',
  standalone: true,
  imports: [CommonModule, RouterModule, ReactiveFormsModule, HeaderComponent, SidebarComponent, UpdateProgressModalComponent],
  templateUrl: './activity-details.component.html',
  styleUrls: ['./activity-details.component.scss']
})
export class ActivityDetailsComponent implements OnInit {
  activity: BoxActivity | null = null;
  activityDetail: BoxActivityDetail | null = null;
  progressHistory: ProgressUpdate[] = [];
  activityId!: string;
  projectId!: string;
  boxId!: string;
  loading = true;
  error = '';
  isModalOpen = false;

  // Modal states
  isUpdateStatusModalOpen = false;
  isAssignTeamModalOpen = false;
  isIssueMaterialModalOpen = false;
  isSetScheduleModalOpen = false;

  // Form groups
  statusForm!: FormGroup;
  assignTeamForm!: FormGroup;
  issueMaterialForm!: FormGroup;
  scheduleForm!: FormGroup;

  // Loading states
  isUpdatingStatus = false;
  isAssigningTeam = false;
  isIssuingMaterial = false;
  isSettingSchedule = false;

  // Error messages
  scheduleError = '';
  scheduleSuccess = false;
  assignTeamSuccess = false;

  // Data for dropdowns
  availableTeams: Team[] = [];
  availableMembers: TeamMember[] = [];
  availableMaterials: any[] = [];

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private fb: FormBuilder,
    private boxService: BoxService,
    private progressUpdateService: ProgressUpdateService,
    private teamService: TeamService
  ) {}

  ngOnInit(): void {
    this.projectId = this.route.snapshot.params['projectId'];
    this.boxId = this.route.snapshot.params['boxId'];
    this.activityId = this.route.snapshot.params['activityId'];
    
    if (!this.projectId || !this.boxId || !this.activityId) {
      this.error = 'Missing required parameters';
      this.loading = false;
      return;
    }
    
    this.initForms();
    this.loadActivity();
    this.loadDropdownData();
  }

  initForms(): void {
    this.statusForm = this.fb.group({
      status: ['', Validators.required],
      notes: ['']
    });

    this.assignTeamForm = this.fb.group({
      teamId: ['', Validators.required],
      memberId: ['']
    });

    this.issueMaterialForm = this.fb.group({
      materialId: ['', Validators.required],
      quantity: ['', [Validators.required, Validators.min(0.01)]],
      notes: ['']
    });

    this.scheduleForm = this.fb.group({
      plannedStartDate: ['', Validators.required],
      duration: ['', [Validators.required, Validators.min(1)]],
      notes: ['']
    });
  }

  loadDropdownData(): void {
    // Load teams from database
    this.teamService.getTeams().subscribe({
      next: (teams) => {
        this.availableTeams = teams.filter(team => team.isActive);
        console.log('✅ Teams loaded:', this.availableTeams);
      },
      error: (err) => {
        console.error('❌ Error loading teams:', err);
        this.availableTeams = [];
      }
    });

    // TODO: Load materials from services
    this.availableMaterials = [
      { id: '1', name: 'Cement', unit: 'bags' },
      { id: '2', name: 'Steel Bars', unit: 'tons' },
      { id: '3', name: 'Concrete', unit: 'm³' }
    ];
  }

  /**
   * Load team members when a team is selected
   */
  loadTeamMembers(teamId: string): void {
    if (!teamId) {
      this.availableMembers = [];
      return;
    }

    this.teamService.getTeamMembers(teamId).subscribe({
      next: (teamMembersData) => {
        this.availableMembers = teamMembersData.members.filter(member => member.isActive !== false);
        console.log('✅ Team members loaded:', this.availableMembers);
      },
      error: (err) => {
        console.error('❌ Error loading team members:', err);
        this.availableMembers = [];
      }
    });
  }

  loadActivity(): void {
    this.loading = true;
    this.error = '';
    
    // Load activity details
    this.progressUpdateService.getActivityDetails(this.activityId).subscribe({
      next: (activityDetail) => {
        this.activityDetail = activityDetail;
        this.activity = this.convertToBoxActivity(activityDetail);
        this.loading = false;
        console.log('✅ Activity loaded:', activityDetail);
        
        // Load progress history
        this.loadProgressHistory();
      },
      error: (err) => {
        // Fallback to old service if new endpoint not available
        this.boxService.getActivityDetails(this.activityId).subscribe({
          next: (activity) => {
            this.activity = activity;
            this.loading = false;
            console.log('✅ Activity loaded (fallback):', activity);
          },
          error: (err) => {
            this.error = err.message || 'Failed to load activity details';
            this.loading = false;
            console.error('❌ Error loading activity:', err);
          }
        });
      }
    });
  }

  loadProgressHistory(): void {
    this.progressUpdateService.getProgressUpdatesByActivity(this.activityId).subscribe({
      next: (history) => {
        this.progressHistory = history.sort((a, b) => 
          new Date(b.updateDate || b.createdDate || '').getTime() - 
          new Date(a.updateDate || a.createdDate || '').getTime()
        );
        console.log('✅ Progress history loaded:', this.progressHistory.length, 'updates');
      },
      error: (err) => {
        console.warn('⚠️ Could not load progress history:', err);
        // Not critical, just log the warning
      }
    });
  }

  convertToBoxActivity(detail: BoxActivityDetail): BoxActivity {
    return {
      id: detail.boxActivityId,
      boxId: detail.boxId,
      name: detail.activityName,
      description: detail.workDescription,
      status: detail.status as any,
      sequence: detail.sequence,
      assignedTo: detail.teamName || detail.assignedMemberName,
      plannedDuration: detail.duration || 0,
      weightPercentage: detail.progressPercentage,
      plannedStartDate: detail.plannedStartDate,
      actualStartDate: detail.actualStartDate,
      plannedEndDate: detail.plannedEndDate,
      actualEndDate: detail.actualEndDate
    };
  }

  openProgressModal(): void {
    if (this.activityDetail) {
      this.isModalOpen = true;
    }
  }

  closeProgressModal(): void {
    this.isModalOpen = false;
  }

  onProgressUpdated(response: any): void {
    // Reload activity data to reflect the updated progress
    this.loadActivity();
    
    // Show success message
    document.dispatchEvent(new CustomEvent('app-toast', {
      detail: { 
        message: response.wirCreated 
          ? `Progress updated successfully! WIR ${response.wirCode} has been automatically created for QC inspection.`
          : 'Progress updated successfully!', 
        type: 'success' 
      }
    }));
  }

  goBack(): void {
    this.router.navigate(['/projects', this.projectId, 'boxes', this.boxId]);
  }

  getStatusClass(status: string): string {
    const statusMap: Record<string, string> = {
      'NotStarted': 'badge-secondary',
      'InProgress': 'badge-warning',
      'Completed': 'badge-success',
      'Approved': 'badge-primary',
      'Rejected': 'badge-danger',
      'OnHold': 'badge-warning'
    };
    return statusMap[status] || 'badge-secondary';
  }

  formatDate(date?: Date): string {
    if (!date) return 'Not set';
    return new Date(date).toLocaleDateString('en-US', { 
      year: 'numeric', 
      month: 'short', 
      day: 'numeric' 
    });
  }

  getProgressColor(): string {
    if (!this.activity) return 'var(--secondary-color)';
    const progress = this.activity.weightPercentage;
    if (progress < 25) return 'var(--danger-color)';
    if (progress < 75) return 'var(--warning-color)';
    return 'var(--success-color)';
  }

  formatDateTime(date?: Date | string): string {
    if (!date) return 'Not set';
    return new Date(date).toLocaleString('en-US', {
      year: 'numeric',
      month: 'short',
      day: 'numeric',
      hour: '2-digit',
      minute: '2-digit'
    });
  }

  getStatusLabel(status: string): string {
    const statusMap: Record<string, string> = {
      'NotStarted': 'Not Started',
      'InProgress': 'In Progress',
      'Completed': 'Completed',
      'Approved': 'Approved',
      'Rejected': 'Rejected',
      'OnHold': 'On Hold',
      'Delayed': 'Delayed'
    };
    return statusMap[status] || status;
  }

  isWIRCheckpoint(): boolean {
    return this.activityDetail?.isWIRCheckpoint || false;
  }

  // Update Status Modal
  openUpdateStatusModal(): void {
    if (this.activity) {
      this.statusForm.patchValue({
        status: this.activity.status,
        notes: ''
      });
    }
    this.isUpdateStatusModalOpen = true;
  }

  closeUpdateStatusModal(): void {
    this.isUpdateStatusModalOpen = false;
    this.statusForm.reset();
  }

  /**
   * Map string status to numeric enum value
   */
  private mapStatusToEnum(status: string): number {
    const statusMap: Record<string, number> = {
      'NotStarted': 1,
      'InProgress': 2,
      'Completed': 3,
      'OnHold': 4,
      'Delayed': 5
    };
    return statusMap[status] || 1;
  }

  onUpdateStatus(): void {
    if (this.statusForm.invalid || !this.activityDetail) return;

    this.isUpdatingStatus = true;
    const formValue = this.statusForm.value;

    const statusEnum = this.mapStatusToEnum(formValue.status);
    const workDescription = formValue.notes || undefined;

    this.boxService.updateBoxActivityStatus(
      this.activityId,
      statusEnum,
      workDescription,
      undefined
    ).subscribe({
      next: () => {
        this.isUpdatingStatus = false;
        this.closeUpdateStatusModal();
        this.loadActivity();
        document.dispatchEvent(new CustomEvent('app-toast', {
          detail: { message: 'Status updated successfully!', type: 'success' }
        }));
      },
      error: (err) => {
        this.isUpdatingStatus = false;
        
        // Extract validation error message from backend response
        let errorMessage = 'Failed to update status';
        if (err?.error) {
          // Check for FluentValidation error format
          if (err.error.errors && typeof err.error.errors === 'object') {
            const errors = Object.values(err.error.errors).flat() as string[];
            errorMessage = errors.length > 0 ? errors[0] : errorMessage;
          } else if (err.error.message) {
            errorMessage = err.error.message;
          } else if (typeof err.error === 'string') {
            errorMessage = err.error;
          }
        } else if (err?.message) {
          errorMessage = err.message;
        }
        
        document.dispatchEvent(new CustomEvent('app-toast', {
          detail: { message: errorMessage, type: 'error' }
        }));
        console.error('Error updating status:', err);
      }
    });
  }

  // Assign Team Modal
  openAssignTeamModal(): void {
    this.assignTeamSuccess = false; // Reset success state
    
    // Load team members if team is already assigned
    if (this.activityDetail?.teamId) {
      this.loadTeamMembers(this.activityDetail.teamId.toString());
    } else {
      this.availableMembers = [];
    }

    if (this.activityDetail) {
      this.assignTeamForm.patchValue({
        teamId: this.activityDetail.teamId?.toString() || '',
        memberId: this.activityDetail.assignedMemberId || ''
      });
    }

    // Listen to team selection changes to load members
    this.assignTeamForm.get('teamId')?.valueChanges.subscribe(teamId => {
      this.loadTeamMembers(teamId);
      // Clear member selection when team changes
      this.assignTeamForm.patchValue({ memberId: '' }, { emitEvent: false });
    });

    this.isAssignTeamModalOpen = true;
  }

  closeAssignTeamModal(): void {
    this.isAssignTeamModalOpen = false;
    this.assignTeamForm.reset();
    this.availableMembers = [];
    this.assignTeamSuccess = false; // Reset success state
  }

  onAssignTeam(): void {
    if (this.assignTeamForm.invalid || !this.activityDetail) return;

    this.isAssigningTeam = true;
    const formValue = this.assignTeamForm.value;

    console.log('🔄 Assigning team:', {
      activityId: this.activityId,
      teamId: formValue.teamId,
      memberId: formValue.memberId
    });

    // Call API to assign team
    this.boxService.assignActivityToTeam(
      this.activityId,
      formValue.teamId,
      formValue.memberId
    ).subscribe({
      next: (response) => {
        console.log('✅ Team assigned successfully:', response);
        this.isAssigningTeam = false;
        this.assignTeamSuccess = true; // Show success message in modal
        
        // Show success message
        const successMessage = 'Team assigned successfully!';
        console.log('📢 Dispatching toast:', successMessage);
        
        // Dispatch toast notification
        document.dispatchEvent(new CustomEvent('app-toast', {
          detail: { message: successMessage, type: 'success' }
        }));
        
        // Reload activity data
        this.loadActivity();
        
        // Close modal after showing success message for 1.5 seconds
        setTimeout(() => {
          this.closeAssignTeamModal();
        }, 1500);
      },
      error: (err) => {
        console.error('❌ Error assigning team:', err);
        this.isAssigningTeam = false;
        
        // Extract error message from backend response
        let errorMessage = 'Failed to assign team';
        if (err?.error) {
          if (err.error.errors && typeof err.error.errors === 'object') {
            const errors = Object.values(err.error.errors).flat() as string[];
            errorMessage = errors.length > 0 ? errors[0] : errorMessage;
          } else if (err.error.message) {
            errorMessage = err.error.message;
          } else if (typeof err.error === 'string') {
            errorMessage = err.error;
          }
        } else if (err?.message) {
          errorMessage = err.message;
        }
        
        console.log('📢 Dispatching error toast:', errorMessage);
        document.dispatchEvent(new CustomEvent('app-toast', {
          detail: { message: errorMessage, type: 'error' }
        }));
      }
    });
  }

  // Issue Material Modal
  openIssueMaterialModal(): void {
    this.issueMaterialForm.reset();
    this.isIssueMaterialModalOpen = true;
  }

  closeIssueMaterialModal(): void {
    this.isIssueMaterialModalOpen = false;
    this.issueMaterialForm.reset();
  }

  onIssueMaterial(): void {
    if (this.issueMaterialForm.invalid || !this.activityDetail) return;

    this.isIssuingMaterial = true;
    const formValue = this.issueMaterialForm.value;

    // TODO: Call API to issue material
    console.log('Issuing material:', formValue);
    
    // Simulate API call
    setTimeout(() => {
      this.isIssuingMaterial = false;
      this.closeIssueMaterialModal();
      alert('Material issued successfully!');
    }, 1000);
  }

  // Set Schedule Modal
  openSetScheduleModal(): void {
    this.scheduleError = ''; // Clear any previous errors
    this.scheduleSuccess = false; // Reset success state
    if (this.activityDetail) {
      // Extract date in YYYY-MM-DD format without timezone conversion
      let plannedStart = '';
      if (this.activityDetail.plannedStartDate) {
        // Handle both string and Date object inputs
        const date = this.activityDetail.plannedStartDate instanceof Date 
          ? this.activityDetail.plannedStartDate 
          : new Date(this.activityDetail.plannedStartDate);
        
        // Check if date is valid
        if (!isNaN(date.getTime())) {
          // Use local date components to avoid timezone shifts
          const year = date.getFullYear();
          const month = String(date.getMonth() + 1).padStart(2, '0');
          const day = String(date.getDate()).padStart(2, '0');
          plannedStart = `${year}-${month}-${day}`;
        }
      }

      this.scheduleForm.patchValue({
        plannedStartDate: plannedStart,
        duration: this.activityDetail.duration || '',
        notes: ''
      });
    }
    this.isSetScheduleModalOpen = true;
  }

  closeSetScheduleModal(): void {
    this.isSetScheduleModalOpen = false;
    this.scheduleForm.reset();
    this.scheduleError = ''; // Clear error when closing
    this.scheduleSuccess = false; // Reset success state
  }

  onSetSchedule(): void {
    if (this.scheduleForm.invalid || !this.activityDetail) return;

    this.isSettingSchedule = true;
    const formValue = this.scheduleForm.value;

    // Parse date string (YYYY-MM-DD) and create date at UTC midnight to avoid timezone shifts
    // This ensures the date sent to backend matches the date selected by the user
    const dateParts = formValue.plannedStartDate.split('-');
    const plannedStartDate = new Date(Date.UTC(
      parseInt(dateParts[0], 10),
      parseInt(dateParts[1], 10) - 1, // Month is 0-indexed
      parseInt(dateParts[2], 10),
      0, 0, 0, 0 // UTC midnight
    ));
    const duration = parseInt(formValue.duration, 10);

    this.boxService.setActivitySchedule(this.activityId, plannedStartDate, duration).subscribe({
      next: () => {
        console.log('✅ Schedule updated successfully');
        this.isSettingSchedule = false;
        this.scheduleSuccess = true; // Show success message in modal
        
        // Reload activity data
        this.loadActivity();
        
        // Dispatch toast notification
        document.dispatchEvent(new CustomEvent('app-toast', {
          detail: { message: 'Schedule updated successfully!', type: 'success' }
        }));
        
        // Close modal after showing success message for 1.5 seconds
        setTimeout(() => {
          this.closeSetScheduleModal();
        }, 1500);
      },
      error: (err) => {
        this.isSettingSchedule = false;
        
        // Extract validation error message from backend response
        let errorMessage = 'Failed to update schedule';
        if (err?.error) {
          // Check for FluentValidation error format (errors object)
          if (err.error.errors && typeof err.error.errors === 'object') {
            const errors = Object.values(err.error.errors).flat() as string[];
            errorMessage = errors.length > 0 ? errors[0] : errorMessage;
          } 
          // Check for direct message property (may contain field name prefix like "PlannedStartDate ...")
          else if (err.error.message) {
            // Remove field name prefix if present (e.g., "PlannedStartDate Cannot modify..." -> "Cannot modify...")
            const message = err.error.message;
            errorMessage = message.includes(' ') && /^[A-Za-z]+ /.test(message) 
              ? message.substring(message.indexOf(' ') + 1) 
              : message;
          } 
          // Check if error itself is a string
          else if (typeof err.error === 'string') {
            errorMessage = err.error;
          }
        } else if (err?.message) {
          errorMessage = err.message;
        }
        
        // Set error message to display in modal
        this.scheduleError = errorMessage;
        
        // Also show toast notification
        document.dispatchEvent(new CustomEvent('app-toast', {
          detail: { message: errorMessage, type: 'error' }
        }));
        console.error('Error setting schedule:', err);
      }
    });
  }
}
