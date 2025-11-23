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

  // Data for dropdowns
  availableTeams: any[] = [];
  availableMembers: any[] = [];
  availableMaterials: any[] = [];

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private fb: FormBuilder,
    private boxService: BoxService,
    private progressUpdateService: ProgressUpdateService
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
      plannedEndDate: ['', Validators.required],
      duration: ['', [Validators.required, Validators.min(1)]],
      notes: ['']
    });
  }

  loadDropdownData(): void {
    // TODO: Load teams, members, and materials from services
    // For now, using mock data
    this.availableTeams = [
      { id: '1', name: 'Civil Team' },
      { id: '2', name: 'MEP Team' },
      { id: '3', name: 'QA/QC Team' }
    ];

    this.availableMembers = [
      { id: '1', name: 'John Doe' },
      { id: '2', name: 'Jane Smith' },
      { id: '3', name: 'Mike Johnson' }
    ];

    this.availableMaterials = [
      { id: '1', name: 'Cement', unit: 'bags' },
      { id: '2', name: 'Steel Bars', unit: 'tons' },
      { id: '3', name: 'Concrete', unit: 'm³' }
    ];
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
    console.log('Progress updated:', response);
    
    // Reload activity details
    this.loadActivity();
    
    // Show WIR notification if created
    if (response.wirCreated) {
      alert(`✅ Progress updated! WIR ${response.wirCode} has been created for QC inspection.`);
    }
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

  onUpdateStatus(): void {
    if (this.statusForm.invalid || !this.activityDetail) return;

    this.isUpdatingStatus = true;
    const formValue = this.statusForm.value;

    // TODO: Call API to update activity status
    console.log('Updating status:', formValue);
    
    // Simulate API call
    setTimeout(() => {
      this.isUpdatingStatus = false;
      this.closeUpdateStatusModal();
      this.loadActivity();
      alert('Status updated successfully!');
    }, 1000);
  }

  // Assign Team Modal
  openAssignTeamModal(): void {
    if (this.activityDetail) {
      this.assignTeamForm.patchValue({
        teamId: this.activityDetail.teamId?.toString() || '',
        memberId: this.activityDetail.assignedMemberId || ''
      });
    }
    this.isAssignTeamModalOpen = true;
  }

  closeAssignTeamModal(): void {
    this.isAssignTeamModalOpen = false;
    this.assignTeamForm.reset();
  }

  onAssignTeam(): void {
    if (this.assignTeamForm.invalid || !this.activityDetail) return;

    this.isAssigningTeam = true;
    const formValue = this.assignTeamForm.value;

    // TODO: Call API to assign team
    console.log('Assigning team:', formValue);
    
    // Simulate API call
    setTimeout(() => {
      this.isAssigningTeam = false;
      this.closeAssignTeamModal();
      this.loadActivity();
      alert('Team assigned successfully!');
    }, 1000);
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
    if (this.activityDetail) {
      const plannedStart = this.activityDetail.plannedStartDate 
        ? new Date(this.activityDetail.plannedStartDate).toISOString().split('T')[0]
        : '';
      const plannedEnd = this.activityDetail.plannedEndDate 
        ? new Date(this.activityDetail.plannedEndDate).toISOString().split('T')[0]
        : '';

      this.scheduleForm.patchValue({
        plannedStartDate: plannedStart,
        plannedEndDate: plannedEnd,
        duration: this.activityDetail.duration || '',
        notes: ''
      });
    }
    this.isSetScheduleModalOpen = true;
  }

  closeSetScheduleModal(): void {
    this.isSetScheduleModalOpen = false;
    this.scheduleForm.reset();
  }

  onSetSchedule(): void {
    if (this.scheduleForm.invalid || !this.activityDetail) return;

    this.isSettingSchedule = true;
    const formValue = this.scheduleForm.value;

    // TODO: Call API to set schedule
    console.log('Setting schedule:', formValue);
    
    // Simulate API call
    setTimeout(() => {
      this.isSettingSchedule = false;
      this.closeSetScheduleModal();
      this.loadActivity();
      alert('Schedule updated successfully!');
    }, 1000);
  }
}
