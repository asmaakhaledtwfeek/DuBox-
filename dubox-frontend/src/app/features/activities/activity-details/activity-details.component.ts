import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { FormsModule } from '@angular/forms';
import { BoxService } from '../../../core/services/box.service';
import { BoxActivity, Box, BoxStatus, canPerformActivityActions, getAvailableActivityStatuses, canPerformActivityActionsByStatus, ActivityStatus } from '../../../core/models/box.model';
import { HeaderComponent } from '../../../shared/components/header/header.component';
import { SidebarComponent } from '../../../shared/components/sidebar/sidebar.component';
import { UpdateProgressModalComponent } from '../update-progress-modal/update-progress-modal.component';
import { ProgressUpdateService } from '../../../core/services/progress-update.service';
import { BoxActivityDetail, ProgressUpdate } from '../../../core/models/progress-update.model';
import { TeamService } from '../../../core/services/team.service';
import { Team, TeamMember, TeamGroup } from '../../../core/models/team.model';
import { ProgressUpdatesTableComponent } from '../../../shared/components/progress-updates-table/progress-updates-table.component';
import { AuditLogService } from '../../../core/services/audit-log.service';
import { AuditLog, AuditLogQueryParams } from '../../../core/models/audit-log.model';
import { DiffUtil } from '../../../core/utils/diff.util';
import { calculateAndFormatDuration, calculateDurationValues } from '../../../core/utils/duration.util';
import { ProjectService } from '../../../core/services/project.service';

@Component({
  selector: 'app-activity-details',
  standalone: true,
  imports: [CommonModule, RouterModule, ReactiveFormsModule, FormsModule, HeaderComponent, SidebarComponent, UpdateProgressModalComponent, ProgressUpdatesTableComponent],
  templateUrl: './activity-details.component.html',
  styleUrls: ['./activity-details.component.scss']
})
export class ActivityDetailsComponent implements OnInit {
  activity: BoxActivity | null = null;
  activityDetail: BoxActivityDetail | null = null;
  allActivities: BoxActivityDetail[] = []; // All activities for the box (for WIR position feature)
  progressHistory: ProgressUpdate[] = [];
  progressHistoryLoading = false;
  progressHistoryError = '';
  selectedProgressUpdate: ProgressUpdate | null = null;
  isProgressModalOpen = false;
  showProgressHistorySearch = false;
  progressHistorySearchTerm = '';
  progressHistoryActivityName = '';
  progressHistoryStatus = '';
  progressHistoryFromDate = '';
  progressHistoryToDate = '';
  activeTab: 'progress-history' | 'activity-logs' = 'progress-history';
  activityId!: string;
  projectId!: string;
  boxId!: string;
  loading = true;
  error = '';
  isModalOpen = false;

  // Activity Logs
  activityLogs: AuditLog[] = [];
  activityLogsLoading = false;
  activityLogsError = '';
  activityLogsCurrentPage = 1;
  activityLogsPageSize = 25;
  activityLogsTotalCount = 0;
  activityLogsTotalPages = 0;
  showActivityLogsSearch = false;
  activityLogsSearchTerm = '';
  activityLogsAction = '';
  activityLogsFromDate = '';
  activityLogsToDate = '';
  availableActivityLogActions: string[] = [];
  activityLogActionSet = new Set<string>();

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
  loadingMembers = false;
  isSettingSchedule = false;

  // Error messages
  scheduleError = '';
  scheduleSuccess = false;
  assignTeamSuccess = false;

  // Data for dropdowns
  availableTeams: Team[] = [];
  availableMembers: TeamMember[] = [];
  availableGroups: TeamGroup[] = [];
  availableMaterials: any[] = [];

  // Permission properties
  canAssignTeam = true;
  canIssueMaterial = true;
  canSetSchedule = true;
  canUpdateProgress = true;
  canChangeStatus = true;
  availableStatuses: ActivityStatus[] = [];
  
  // Project status flags
  isProjectOnHold = false;
  isProjectArchived = false;
  isProjectClosed = false;
  project: any = null;
  
  // Box status
  box: Box | null = null;
  boxStatus: BoxStatus | null = null;

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private fb: FormBuilder,
    private boxService: BoxService,
    private progressUpdateService: ProgressUpdateService,
    private teamService: TeamService,
    private auditLogService: AuditLogService,
    private projectService: ProjectService
  ) {}

  returnTo: string | null = null; // Store return context

  ngOnInit(): void {
    this.projectId = this.route.snapshot.params['projectId'];
    this.boxId = this.route.snapshot.params['boxId'];
    this.activityId = this.route.snapshot.params['activityId'];
    
    // Get returnTo query parameter to determine where to navigate back
    this.returnTo = this.route.snapshot.queryParams['returnTo'] || null;
    
    if (!this.projectId || !this.boxId || !this.activityId) {
      this.error = 'Missing required parameters';
      this.loading = false;
      return;
    }
    
    this.initForms();
    this.loadProjectDetails();
    this.loadBox(); // Load box to check its status
    this.loadActivity();
    this.loadAllActivities(); // Load all activities for WIR position feature
    this.loadDropdownData();
  }

  private loadProjectDetails(): void {
    if (!this.projectId) return;
    
    this.projectService.getProject(this.projectId).subscribe({
      next: (project) => {
        this.project = project;
        this.isProjectArchived = project.status === 'Archived';
        this.isProjectOnHold = project.status === 'OnHold';
        this.isProjectClosed = project.status === 'Closed';
        this.updatePermissions();
        console.log('📁 Project loaded in activity details. Status:', project.status, 'Is Archived:', this.isProjectArchived, 'Is OnHold:', this.isProjectOnHold, 'Is Closed:', this.isProjectClosed);
      },
      error: (err) => {
        console.error('Error loading project details:', err);
      }
    });
  }

  private loadBox(): void {
    if (!this.boxId) return;
    
    this.boxService.getBox(this.boxId).subscribe({
      next: (box) => {
        this.box = box;
        this.boxStatus = box.status;
        this.updatePermissions();
        console.log('📦 Box loaded in activity details. Status:', box.status);
      },
      error: (err) => {
        console.error('Error loading box details:', err);
        // Still update permissions even if box load fails
        this.updatePermissions();
      }
    });
  }

  private updatePermissions(): void {
    // Check if activity actions are allowed based on box status
    const canPerformActivityActionsBasedOnBoxStatus = this.boxStatus 
      ? canPerformActivityActions(this.boxStatus)
      : true;
    
    // Check if activity actions are allowed based on activity status
    // Allow progress updates for completed activities (to update position only)
    const isCompleted = this.activity?.status === ActivityStatus.Completed || this.activity?.status === ActivityStatus.Delayed;
    const canPerformActionsByActivityStatus = this.activity 
      ? (isCompleted || canPerformActivityActionsByStatus(this.activity.status as ActivityStatus))
      : true;
    
    // Update available statuses for dropdown
    if (this.activity && this.activityDetail) {
      const progress = this.activity.weightPercentage || this.activityDetail.progressPercentage || 0;
      this.availableStatuses = getAvailableActivityStatuses(this.activity.status as ActivityStatus, progress);
      this.canChangeStatus = this.availableStatuses.length > 0;
    } else {
      this.availableStatuses = [];
      this.canChangeStatus = false;
    }
    
    // Disable all actions if:
    // - Project is archived, on hold, or closed
    // - Box status doesn't allow activity actions
    // - Activity status doesn't allow actions (OnHold only, Completed is allowed for position updates)
    const baseCanPerformActions = !this.isProjectArchived && !this.isProjectOnHold && !this.isProjectClosed && canPerformActivityActionsBasedOnBoxStatus && canPerformActionsByActivityStatus;
    
    this.canAssignTeam = baseCanPerformActions && !isCompleted; // Don't allow team assignment for completed/delayed
    this.canIssueMaterial = baseCanPerformActions && !isCompleted; // Don't allow material issuance for completed/delayed
    this.canSetSchedule = baseCanPerformActions && !isCompleted; // Don't allow schedule changes for completed/delayed
    this.canUpdateProgress = baseCanPerformActions; // Allow progress modal for completed/delayed (position updates only, handled by modal)
    
    // Disable forms if project is on hold, closed, or archived, or if box/activity status doesn't allow activity actions
    if (this.isProjectArchived || this.isProjectOnHold || this.isProjectClosed || !canPerformActivityActionsBasedOnBoxStatus || !canPerformActionsByActivityStatus) {
      this.statusForm?.disable();
      this.assignTeamForm?.disable();
      this.issueMaterialForm?.disable();
      this.scheduleForm?.disable();
    }
    
    console.log('✅ Activity permissions updated:', {
      canAssignTeam: this.canAssignTeam,
      canIssueMaterial: this.canIssueMaterial,
      canSetSchedule: this.canSetSchedule,
      canUpdateProgress: this.canUpdateProgress,
      canChangeStatus: this.canChangeStatus,
      availableStatuses: this.availableStatuses,
      isProjectArchived: this.isProjectArchived,
      isProjectOnHold: this.isProjectOnHold,
      boxStatus: this.boxStatus,
      activityStatus: this.activity?.status,
      canPerformActivityActionsBasedOnBoxStatus,
      canPerformActionsByActivityStatus
    });
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
   * Load team groups when a team is selected
   */
  loadTeamGroups(teamId: string, onComplete?: (groups: TeamGroup[]) => void): void {
    if (!teamId) {
      this.availableGroups = [];
      if (onComplete) {
        onComplete([]);
      }
      return;
    }

    this.teamService.getTeamGroupsPaginated({ teamId, isActive: true, pageSize: 100 }).subscribe({
      next: (response) => {
        this.availableGroups = response.items || [];
        console.log('✅ Team groups loaded:', this.availableGroups);
        if (onComplete) {
          onComplete(this.availableGroups);
        }
      },
      error: (err) => {
        console.error('❌ Error loading team groups:', err);
        this.availableGroups = [];
        if (onComplete) {
          onComplete([]);
        }
      }
    });
  }

  /**
   * Load team members when a team is selected
   */
  loadTeamMembers(teamId: string, onComplete?: (members: TeamMember[]) => void): void {
    if (!teamId) {
      this.availableMembers = [];
      if (onComplete) {
        onComplete([]);
      }
      return;
    }

    this.loadingMembers = true;
    this.teamService.getTeamMembers(teamId).subscribe({
      next: (response: any) => {
        this.availableMembers = response.members || [];
        console.log('✅ Team members loaded:', this.availableMembers);
        this.loadingMembers = false;
        if (onComplete) {
          onComplete(this.availableMembers);
        }
      },
      error: (err: any) => {
        console.error('❌ Error loading team members:', err);
        this.availableMembers = [];
        this.loadingMembers = false;
        if (onComplete) {
          onComplete([]);
        }
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
        
        // Update permissions after activity is loaded
        this.updatePermissions();
        
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

  /**
   * Load all activities for the box (used for WIR position feature)
   */
  loadAllActivities(): void {
    if (!this.boxId) {
      console.warn('⚠️ BoxId not available, cannot load all activities');
      return;
    }

    this.progressUpdateService.getBoxActivitiesWithProgress(this.boxId).subscribe({
      next: (activities) => {
        this.allActivities = activities || [];
        console.log(`✅ Loaded ${this.allActivities.length} activities for WIR position feature`);
      },
      error: (err) => {
        console.error('❌ Error loading all activities:', err);
        // Don't show error to user, just log it - modal will work without WIR position feature
        this.allActivities = [];
      }
    });
  }

  loadProgressHistory(): void {
    this.progressHistoryLoading = true;
    this.progressHistoryError = '';
    
    this.progressUpdateService.getProgressUpdatesByActivity(this.activityId).subscribe({
      next: (history) => {
        let filteredHistory = history.sort((a, b) => 
          new Date(b.updateDate || b.createdDate || '').getTime() - 
          new Date(a.updateDate || a.createdDate || '').getTime()
        );
        
        // Apply client-side filtering if search is active
        if (this.showProgressHistorySearch) {
          filteredHistory = this.applyProgressHistoryFilters(filteredHistory);
        }
        
        this.progressHistory = filteredHistory;
        this.progressHistoryLoading = false;
        console.log('✅ Progress history loaded:', this.progressHistory.length, 'updates');
      },
      error: (err) => {
        console.warn('⚠️ Could not load progress history:', err);
        this.progressHistoryError = err?.error?.message || err?.message || 'Failed to load progress history';
        this.progressHistoryLoading = false;
        // Not critical, just log the warning
      }
    });
  }

  applyProgressHistoryFilters(history: ProgressUpdate[]): ProgressUpdate[] {
    return history.filter(update => {
      // Search term filter
      if (this.progressHistorySearchTerm?.trim()) {
        const searchLower = this.progressHistorySearchTerm.toLowerCase();
        const matchesSearch = 
          (update.workDescription?.toLowerCase().includes(searchLower)) ||
          (update.issuesEncountered?.toLowerCase().includes(searchLower)) ||
          (update.activityName?.toLowerCase().includes(searchLower)) ||
          (update.updatedByName?.toLowerCase().includes(searchLower)) ||
          (update.updatedBy?.toLowerCase().includes(searchLower)) ||
          (update.locationDescription?.toLowerCase().includes(searchLower));
        if (!matchesSearch) return false;
      }

      // Activity name filter (should always match since it's for a specific activity, but keep for consistency)
      if (this.progressHistoryActivityName?.trim()) {
        const activityLower = this.progressHistoryActivityName.toLowerCase();
        if (!update.activityName?.toLowerCase().includes(activityLower)) {
          return false;
        }
      }

      // Status filter
      if (this.progressHistoryStatus?.trim()) {
        const updateStatus = typeof update.status === 'string' ? update.status.toLowerCase() : String(update.status).toLowerCase();
        if (updateStatus !== this.progressHistoryStatus.toLowerCase()) {
          return false;
        }
      }

      // Date range filter
      if (this.progressHistoryFromDate || this.progressHistoryToDate) {
        const updateDate = new Date(update.updateDate || update.createdDate || '');
        if (isNaN(updateDate.getTime())) return false;

        if (this.progressHistoryFromDate) {
          const fromDate = new Date(this.progressHistoryFromDate);
          fromDate.setHours(0, 0, 0, 0);
          if (updateDate < fromDate) return false;
        }

        if (this.progressHistoryToDate) {
          const toDate = new Date(this.progressHistoryToDate);
          toDate.setHours(23, 59, 59, 999);
          if (updateDate > toDate) return false;
        }
      }

      return true;
    });
  }

  toggleProgressHistorySearch(): void {
    this.showProgressHistorySearch = !this.showProgressHistorySearch;
    if (!this.showProgressHistorySearch) {
      this.clearProgressHistorySearch();
    } else {
      // Reapply filters when opening search
      this.loadProgressHistory();
    }
  }

  onProgressHistorySearchChange(searchParams: {
    searchTerm: string;
    activityName: string;
    status: string;
    fromDate: string;
    toDate: string;
  }): void {
    this.progressHistorySearchTerm = searchParams.searchTerm;
    this.progressHistoryActivityName = searchParams.activityName;
    this.progressHistoryStatus = searchParams.status;
    this.progressHistoryFromDate = searchParams.fromDate;
    this.progressHistoryToDate = searchParams.toDate;
    this.loadProgressHistory();
  }

  clearProgressHistorySearch(): void {
    this.progressHistorySearchTerm = '';
    this.progressHistoryActivityName = '';
    this.progressHistoryStatus = '';
    this.progressHistoryFromDate = '';
    this.progressHistoryToDate = '';
    this.loadProgressHistory();
  }

  // Tab Navigation
  setActiveTab(tab: 'progress-history' | 'activity-logs'): void {
    this.activeTab = tab;
    if (tab === 'activity-logs' && this.activityLogs.length === 0 && !this.activityLogsLoading) {
      this.loadActivityLogs(this.activityLogsCurrentPage, this.activityLogsPageSize);
    }
  }

  // Activity Logs Methods
  loadActivityLogs(page: number = 1, pageSize: number = 25): void {
    if (!this.activityId) {
      return;
    }

    this.activityLogsLoading = true;
    this.activityLogsError = '';
    this.activityLogsCurrentPage = page;
    this.activityLogsPageSize = pageSize;

    const params: AuditLogQueryParams = {
      pageNumber: page,
      pageSize: pageSize,
      tableName: 'BoxActivity',
      recordId: this.activityId
    };

    if (this.activityLogsSearchTerm?.trim()) {
      params.searchTerm = this.activityLogsSearchTerm.trim();
    }
    if (this.activityLogsAction?.trim()) {
      params.action = this.activityLogsAction.trim();
    }
    if (this.activityLogsFromDate) {
      const from = new Date(this.activityLogsFromDate);
      from.setHours(0, 0, 0, 0);
      params.fromDate = from.toISOString();
    }
    if (this.activityLogsToDate) {
      const to = new Date(this.activityLogsToDate);
      to.setHours(23, 59, 59, 999);
      params.toDate = to.toISOString();
    }

    this.auditLogService.getAuditLogs(params).subscribe({
      next: (response) => {
        this.activityLogs = response.items;
        this.activityLogsTotalCount = response.totalCount || 0;
        this.activityLogsTotalPages = response.totalPages || 0;
        this.activityLogsCurrentPage = response.pageNumber || page;
        this.activityLogsPageSize = response.pageSize || pageSize;
        
        // Update available actions from current page
        this.activityLogActionSet.clear();
        this.activityLogs.forEach(log => {
          if (log.action) {
            this.activityLogActionSet.add(log.action);
          }
        });
        this.availableActivityLogActions = Array.from(this.activityLogActionSet).sort((a, b) => a.localeCompare(b));
        
        this.activityLogsLoading = false;
      },
      error: (err) => {
        console.error('Error loading activity logs:', err);
        this.activityLogsError = err?.error?.message || err?.message || 'Failed to load activity logs';
        this.activityLogsLoading = false;
      }
    });
  }

  toggleActivityLogsSearch(): void {
    this.showActivityLogsSearch = !this.showActivityLogsSearch;
    if (!this.showActivityLogsSearch) {
      this.clearActivityLogsSearch();
    }
  }

  applyActivityLogsFilters(): void {
    this.activityLogsCurrentPage = 1;
    this.loadActivityLogs(1, this.activityLogsPageSize);
  }

  clearActivityLogsSearch(): void {
    this.activityLogsSearchTerm = '';
    this.activityLogsAction = '';
    this.activityLogsFromDate = '';
    this.activityLogsToDate = '';
    this.activityLogsCurrentPage = 1;
    this.loadActivityLogs(1, this.activityLogsPageSize);
  }

  onActivityLogsPageChange(page: number): void {
    this.loadActivityLogs(page, this.activityLogsPageSize);
  }

  onActivityLogsPageSizeChange(size: number): void {
    this.loadActivityLogs(1, size);
  }

  // Helper methods for activity logs display
  getActionIcon(action: string): string {
    return DiffUtil.getActionIcon(action);
  }

  getActionLabel(action: string): string {
    return DiffUtil.getActionLabel(action);
  }

  formatDescription(log: AuditLog): string {
    return log.description || log.entityDisplayName || `${log.action} on ${log.tableName}`;
  }

  getLogAuthor(log: AuditLog): string {
    return log.changedByFullName || log.changedByUsername || log.changedBy || 'System';
  }

  isExpanded(logId: string): boolean {
    // Simple implementation - could be enhanced with a Set
    return false;
  }

  toggleExpand(logId: string): void {
    // Simple implementation - could be enhanced with a Set
  }

  getVisibleChanges(log: AuditLog): any[] {
    return log.changes?.slice(0, 3) || [];
  }

  getHiddenChanges(log: AuditLog): any[] {
    return log.changes?.slice(3) || [];
  }

  trackByLogId(_index: number, log: AuditLog): string {
    return log.auditLogId;
  }

  readonly Math = Math;

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
    // Check if project status allows actions
    if (this.isProjectArchived || this.isProjectOnHold || this.isProjectClosed) {
      document.dispatchEvent(new CustomEvent('app-toast', {
        detail: { 
          message: this.isProjectArchived 
            ? 'Cannot update progress. This project is archived and read-only.' 
            : 'Cannot update progress. This project is on hold. Only project status changes are allowed.',
          type: 'error' 
        }
      }));
      return;
    }
    
    // Check if box status allows activity actions
    if (this.boxStatus && !canPerformActivityActions(this.boxStatus)) {
      const statusMessage = this.boxStatus === 'Dispatched' 
        ? 'Cannot update progress. The box is dispatched and no actions are allowed on activities.'
        : 'Cannot update progress. The box is on hold and no actions are allowed on activities. Only box status changes are allowed.';
      document.dispatchEvent(new CustomEvent('app-toast', {
        detail: { 
          message: statusMessage,
          type: 'error' 
        }
      }));
      return;
    }
    
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
    // Reload all activities to ensure WIR position data is up to date
    this.loadAllActivities();
    
    // Reload progress history to show the new update
    this.loadProgressHistory();
    
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

  openProgressDetails(update: ProgressUpdate): void {
    // Fetch the full details from the backend using the new endpoint
    if (update.progressUpdateId) {
      this.progressUpdateService.getProgressUpdateById(update.progressUpdateId).subscribe({
        next: (fullUpdate) => {
          this.selectedProgressUpdate = fullUpdate;
          this.isProgressModalOpen = true;
          document.body.style.overflow = 'hidden';
        },
        error: (err) => {
          console.error('Error fetching progress update details:', err);
          // Fallback to the update object we already have
          this.selectedProgressUpdate = update;
          this.isProgressModalOpen = true;
          document.body.style.overflow = 'hidden';
        }
      });
    } else {
      // If no ID is available, use the update object we have
      this.selectedProgressUpdate = update;
      this.isProgressModalOpen = true;
      document.body.style.overflow = 'hidden';
    }
  }

  closeProgressDetails(): void {
    this.isProgressModalOpen = false;
    this.selectedProgressUpdate = null;
    document.body.style.overflow = '';
  }

  getProgressStatusClass(status: string | any): string {
    if (!status) return 'status-unknown';
    
    const statusStr = typeof status === 'string' ? status : String(status);
    const normalized = statusStr.toLowerCase();
    
    switch (normalized) {
      case 'notstarted':
        return 'status-not-started';
      case 'inprogress':
        return 'status-in-progress';
      case 'completed':
        return 'status-completed';
      case 'onhold':
        return 'status-on-hold';
      case 'delayed':
        return 'status-delayed';
      default:
        return 'status-unknown';
    }
  }

  getProgressStatusLabel(status: string | any): string {
    if (!status) return '—';
    
    const statusStr = typeof status === 'string' ? status : String(status);
    const normalized = statusStr.toLowerCase();
    
    switch (normalized) {
      case 'notstarted':
        return 'Not Started';
      case 'inprogress':
        return 'In Progress';
      case 'completed':
        return 'Completed';
      case 'onhold':
        return 'On Hold';
      case 'delayed':
        return 'Delayed';
      default:
        return statusStr;
    }
  }

  getPhotoUrls(progressUpdate: any): string[] {
    // First, try to use the new Images array
    if (progressUpdate.images && Array.isArray(progressUpdate.images) && progressUpdate.images.length > 0) {
      return progressUpdate.images
        .sort((a: any, b: any) => (a.sequence || 0) - (b.sequence || 0))
        .map((img: any) => {
          // Try imageData first (full base64), then imageUrl (for on-demand loading)
          let url = img.imageData || img.ImageData || img.imageUrl || img.ImageUrl || '';
          
          // If it's a relative URL (starts with /api/), prepend the base URL
          if (url && url.startsWith('/api/')) {
            // Get base URL from current location
            const baseUrl = `${window.location.protocol}//${window.location.host}`;
            url = baseUrl + url;
          }
          
          return url;
        })
        .filter((url: string) => url && url.trim().length > 0);
    }
    
    // Fallback to old photo field (deprecated but kept for backward compatibility)
    const photoUrls = progressUpdate.photo || progressUpdate.Photo;
    if (!photoUrls) return [];
    
    try {
      const parsed = JSON.parse(photoUrls);
      if (Array.isArray(parsed)) {
        return parsed.filter((url: string) => url && typeof url === 'string' && url.trim().length > 0);
      }
    } catch {
      // If not JSON, try splitting by comma
      if (typeof photoUrls === 'string') {
        return photoUrls.split(',').map((url: string) => url.trim()).filter((url: string) => url.length > 0);
      }
    }
    
    return [];
  }

  openPhotoInNewTab(photoUrl: string): void {
    window.open(photoUrl, '_blank');
  }

  downloadPhoto(photoUrl: string): void {
    const link = document.createElement('a');
    link.href = photoUrl;
    link.download = photoUrl.split('/').pop() || 'photo.jpg';
    link.target = '_blank';
    document.body.appendChild(link);
    link.click();
    document.body.removeChild(link);
  }

  onImageError(event: Event): void {
    const img = event.target as HTMLImageElement;
    img.style.display = 'none';
  }

  goBack(): void {
    // Navigate back based on returnTo context
    if (this.returnTo === 'progress-history') {
      // Navigate back to box details page with progress-updates tab active
      this.router.navigate(['/projects', this.projectId, 'boxes', this.boxId], {
        queryParams: { tab: 'progress-updates' }
      });
    } else if (this.returnTo === 'attachments') {
      // Navigate back to box details page with attachments tab active
      this.router.navigate(['/projects', this.projectId, 'boxes', this.boxId], {
        queryParams: { tab: 'attachments' }
      });
    } else {
      // Default: navigate back to activities table (box details page with activities tab)
      this.router.navigate(['/projects', this.projectId, 'boxes', this.boxId], {
        queryParams: { tab: 'activities' }
      });
    }
  }

  /**
   * Get formatted actual duration display text
   * - Less than 24 hours: shows hours (e.g., "5 hours", "12.5 hours")
   * - 24 hours or more: shows days and hours (e.g., "2 days 5 hours")
   */
  getActualDurationDisplay(): string {
    if (!this.activityDetail) return '—';
    
    // If we have actual start and end dates, calculate precise duration
    if (this.activityDetail.actualStartDate && this.activityDetail.actualEndDate) {
      const formatted = calculateAndFormatDuration(
        this.activityDetail.actualStartDate,
        this.activityDetail.actualEndDate
      );
      if (formatted) return formatted;
    }
    
    // Fallback: If actualDuration is explicitly set (legacy days value), show it
    if (this.activityDetail.actualDuration !== undefined && this.activityDetail.actualDuration !== null) {
      return this.activityDetail.actualDuration === 1 
        ? '1 day' 
        : `${this.activityDetail.actualDuration} days`;
    }
    
    return '—';
  }

  /**
   * Get variance between actual and planned duration
   * Returns formatted string with sign
   */
  getDurationVariance(): string {
    if (!this.activityDetail || !this.activityDetail.actualStartDate || !this.activityDetail.actualEndDate) {
      return '—';
    }
    
    const plannedDays = this.activityDetail.duration || this.activity?.plannedDuration || 0;
    if (!plannedDays) return '—';
    
    // Calculate actual duration values
    const durationValues = calculateDurationValues(
      this.activityDetail.actualStartDate,
      this.activityDetail.actualEndDate
    );
    
    if (!durationValues) return '—';
    
    // Convert planned days to hours for accurate comparison
    const plannedHours = plannedDays * 24;
    const actualHours = durationValues.totalHours;
    const varianceHours = actualHours - plannedHours;
    
    // Format variance
    if (Math.abs(varianceHours) < 24) {
      const hours = Math.round(varianceHours * 10) / 10;
      const sign = hours > 0 ? '+' : '';
      return `${sign}${hours} hours`;
    } else {
      const days = Math.floor(Math.abs(varianceHours) / 24);
      const hours = Math.round(Math.abs(varianceHours) % 24);
      const sign = varianceHours > 0 ? '+' : '-';
      
      if (hours === 0) {
        return `${sign}${days} ${days === 1 ? 'day' : 'days'}`;
      }
      return `${sign}${days} ${days === 1 ? 'day' : 'days'} ${hours} ${hours === 1 ? 'hour' : 'hours'}`;
    }
  }

  /**
   * Check if actual duration exceeded planned duration
   */
  isDelayed(): boolean {
    if (!this.activityDetail || !this.activityDetail.actualStartDate || !this.activityDetail.actualEndDate) {
      return false;
    }
    
    const plannedDays = this.activityDetail.duration || this.activity?.plannedDuration || 0;
    if (!plannedDays) return false;
    
    const durationValues = calculateDurationValues(
      this.activityDetail.actualStartDate,
      this.activityDetail.actualEndDate
    );
    
    if (!durationValues) return false;
    
    const plannedHours = plannedDays * 24;
    return durationValues.totalHours > plannedHours;
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
    if (this.isProjectArchived || this.isProjectOnHold || this.isProjectClosed) {
      let message = 'Cannot update activity status.';
      if (this.isProjectArchived) {
        message = 'Cannot update activity status. This project is archived and read-only.';
      } else if (this.isProjectClosed) {
        message = 'Cannot update activity status. This project is closed. Only project status changes are allowed.';
      } else if (this.isProjectOnHold) {
        message = 'Cannot update activity status. This project is on hold. Only project status changes are allowed.';
      }
      document.dispatchEvent(new CustomEvent('app-toast', {
        detail: { 
          message: message,
          type: 'error' 
        }
      }));
      return;
    }
    
    // Check if box status allows activity actions
    if (this.boxStatus && !canPerformActivityActions(this.boxStatus)) {
      const statusMessage = this.boxStatus === 'Dispatched' 
        ? 'Cannot update activity status. The box is dispatched and no actions are allowed on activities.'
        : 'Cannot update activity status. The box is on hold and no actions are allowed on activities. Only box status changes are allowed.';
      document.dispatchEvent(new CustomEvent('app-toast', {
        detail: { 
          message: statusMessage,
          type: 'error' 
        }
      }));
      return;
    }
    
    // Check if activity status allows status changes
    if (this.activity && !this.canChangeStatus) {
      const activityStatus = this.activity.status;
      let message = 'Cannot update activity status.';
      if (activityStatus === 'Completed') {
        message = 'Cannot update activity status. Activities in "Completed" status cannot be modified.';
      } else if (activityStatus === 'OnHold') {
        message = 'Cannot update activity status. Activities in "OnHold" status can only be changed to "NotStarted" (if progress = 0%) or "InProgress" (if progress < 100%).';
      }
      document.dispatchEvent(new CustomEvent('app-toast', {
        detail: { 
          message: message,
          type: 'error' 
        }
      }));
      return;
    }
    
    if (this.activity) {
      // Set default status to first available status if current status is not available
      const currentStatus = this.activity.status as ActivityStatus;
      const defaultStatus = this.availableStatuses.length > 0 
        ? (this.availableStatuses.includes(currentStatus) ? currentStatus : this.availableStatuses[0])
        : currentStatus;
      
      this.statusForm.patchValue({
        status: defaultStatus,
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
    if (this.isProjectArchived || this.isProjectOnHold || this.isProjectClosed) {
      let message = 'Cannot update activity status.';
      if (this.isProjectArchived) {
        message = 'Cannot update activity status. This project is archived and read-only.';
      } else if (this.isProjectClosed) {
        message = 'Cannot update activity status. This project is closed. Only project status changes are allowed.';
      } else if (this.isProjectOnHold) {
        message = 'Cannot update activity status. This project is on hold. Only project status changes are allowed.';
      }
      document.dispatchEvent(new CustomEvent('app-toast', {
        detail: { 
          message: message,
          type: 'error' 
        }
      }));
      return;
    }

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
    // Check if project status allows actions
    if (this.isProjectArchived || this.isProjectOnHold || this.isProjectClosed) {
      document.dispatchEvent(new CustomEvent('app-toast', {
        detail: { 
          message: this.isProjectArchived 
            ? 'Cannot assign activity to team. This project is archived and read-only.' 
            : 'Cannot assign activity to team. This project is on hold. Only project status changes are allowed.',
          type: 'error' 
        }
      }));
      return;
    }
    
    // Check if box status allows activity actions
    if (this.boxStatus && !canPerformActivityActions(this.boxStatus)) {
      const statusMessage = this.boxStatus === 'Dispatched' 
        ? 'Cannot assign activity to team. The box is dispatched and no actions are allowed on activities.'
        : 'Cannot assign activity to team. The box is on hold and no actions are allowed on activities. Only box status changes are allowed.';
      document.dispatchEvent(new CustomEvent('app-toast', {
        detail: { 
          message: statusMessage,
          type: 'error' 
        }
      }));
      return;
    }
    
    this.assignTeamSuccess = false; // Reset success state
    
    // Pre-select team if it exists
    if (this.activityDetail?.teamId) {
      const teamId = this.activityDetail.teamId.toString();
      const assignedMemberId = this.activityDetail.assignedMemberId?.toString() || '';
      
      // Set the teamId and load members
      this.assignTeamForm.patchValue({
        teamId: teamId,
        memberId: '' // Will be set after members load
      }, { emitEvent: false });
      
      // Load members and pre-select the assigned member
      this.loadTeamMembers(teamId, (members) => {
        // Pre-select member if it exists and is in the loaded members
        if (assignedMemberId && members.some(m => m.teamMemberId === assignedMemberId)) {
          this.assignTeamForm.patchValue({
            memberId: assignedMemberId
          }, { emitEvent: false });
        }
      });
    } else {
      this.availableMembers = [];
      this.assignTeamForm.patchValue({
        teamId: '',
        memberId: ''
      }, { emitEvent: false });
    }

    // Listen to team selection changes to load members
    this.assignTeamForm.get('teamId')?.valueChanges.subscribe(teamId => {
      if (teamId) {
        this.loadTeamMembers(teamId);
        // Clear member selection when team changes
        this.assignTeamForm.patchValue({ memberId: '' }, { emitEvent: false });
      } else {
        this.availableMembers = [];
        this.assignTeamForm.patchValue({ memberId: '' }, { emitEvent: false });
      }
    });

    this.isAssignTeamModalOpen = true;
  }

  closeAssignTeamModal(): void {
    this.isAssignTeamModalOpen = false;
    this.assignTeamForm.reset();
    this.availableGroups = [];
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
      null, // teamGroupId - no longer used
      formValue.memberId || null
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
    if (this.isProjectArchived || this.isProjectOnHold || this.isProjectClosed) {
      document.dispatchEvent(new CustomEvent('app-toast', {
        detail: { 
          message: this.isProjectArchived 
            ? 'Cannot issue materials. This project is archived and read-only.' 
            : 'Cannot issue materials. This project is on hold. Only project status changes are allowed.',
          type: 'error' 
        }
      }));
      return;
    }
    
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
    if (this.isProjectArchived || this.isProjectOnHold || this.isProjectClosed) {
      document.dispatchEvent(new CustomEvent('app-toast', {
        detail: { 
          message: this.isProjectArchived 
            ? 'Cannot set schedule. This project is archived and read-only.' 
            : 'Cannot set schedule. This project is on hold. Only project status changes are allowed.',
          type: 'error' 
        }
      }));
      return;
    }
    
    // Check if box status allows activity actions
    if (this.boxStatus && !canPerformActivityActions(this.boxStatus)) {
      const statusMessage = this.boxStatus === 'Dispatched' 
        ? 'Cannot set schedule. The box is dispatched and no actions are allowed on activities.'
        : 'Cannot set schedule. The box is on hold and no actions are allowed on activities. Only box status changes are allowed.';
      document.dispatchEvent(new CustomEvent('app-toast', {
        detail: { 
          message: statusMessage,
          type: 'error' 
        }
      }));
      return;
    }
    
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

  getActionType(action: string): string {
    const upperAction = action.toUpperCase();
    if (upperAction.includes('INSERT') || upperAction.includes('CREATE')) {
      return 'create';
    } else if (upperAction.includes('UPDATE') || upperAction.includes('MODIFY') || upperAction.includes('CHANGE')) {
      return 'update';
    } else if (upperAction.includes('DELETE') || upperAction.includes('REMOVE')) {
      return 'delete';
    }
    return 'other';
  }
}
