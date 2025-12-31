import { Component, Input, OnInit, OnChanges, SimpleChanges, OnDestroy, Output, EventEmitter, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router, RouterModule, NavigationEnd } from '@angular/router';
import { Subscription } from 'rxjs';
import { filter } from 'rxjs/operators';
import { BoxActivity, canPerformActivityActions, canPerformActivityActionsByStatus } from '../../../core/models/box.model';
import { UpdateProgressModalComponent } from '../update-progress-modal/update-progress-modal.component';
import { WIRApprovalModalComponent } from '../wir-approval-modal/wir-approval-modal.component';
import { ProgressUpdateService } from '../../../core/services/progress-update.service';
import { BoxActivityDetail } from '../../../core/models/progress-update.model';
import { WIRService } from '../../../core/services/wir.service';
import { WIRRecord, WIRStatus, WIRCheckpoint } from '../../../core/models/wir.model';
import { PermissionService } from '../../../core/services/permission.service';
import { calculateAndFormatDuration, calculateDurationInDays } from '../../../core/utils/duration.util';

// Combined type for activities and WIR rows
export interface TableRow {
  type: 'activity' | 'wir';
  sequence: number;
  data: BoxActivityDetail | WIRRecord;
  isPlaceholder?: boolean;
}

@Component({
  selector: 'app-activity-table',
  standalone: true,
  imports: [CommonModule, RouterModule, UpdateProgressModalComponent, WIRApprovalModalComponent],
  providers: [ProgressUpdateService, WIRService],
  templateUrl: './activity-table.component.html',
  styleUrls: ['./activity-table.component.scss']
})
export class ActivityTableComponent implements OnInit, OnChanges, OnDestroy {
  @Input() activities: BoxActivity[] = [];
  @Input() projectId: string = '';
  @Input() boxId: string = '';
  @Input() boxStatus?: string; // Box status to check if activity actions are allowed
  @Input() isProjectOnHold: boolean = false; // Track if project is on hold
  @Input() isProjectArchived: boolean = false; // Track if project is archived
  @Input() isProjectClosed: boolean = false; // Track if project is closed
  @Output() boxDataChanged = new EventEmitter<void>();
  @Output() activityCountChanged = new EventEmitter<number>();

  activitiesWithDetails: BoxActivityDetail[] = [];
  wirRecords: WIRRecord[] = [];
  wirCheckpoints: Map<string, WIRCheckpoint> = new Map(); // Map WIR code to checkpoint
  tableRows: TableRow[] = [];
  isModalOpen = false;
  selectedActivity: BoxActivityDetail | null = null;
  isWIRModalOpen = false;
  selectedWIR: WIRRecord | null = null;
  isLoading = false;
  private routerSubscription?: Subscription;

  // Permission flags
  canUpdateProgress: boolean = false;
  canReviewWIR: boolean = false;
  canViewWIR: boolean = false;
  canManageCheckpoint: boolean = false;

  constructor(
    private progressUpdateService: ProgressUpdateService,
    private wirService: WIRService,
    private router: Router,
    private cdr: ChangeDetectorRef,
    private permissionService: PermissionService
  ) {
    this.updatePermissions();
  }

  private updatePermissions(): void {
    const baseCanUpdateProgress = this.permissionService.hasPermission('activities', 'update-progress');
    const baseCanViewWIR = this.permissionService.hasPermission('wir', 'view') ||
                          this.permissionService.hasPermission('wir', 'approve') || 
                          this.permissionService.hasPermission('wir', 'reject') ||
                          this.permissionService.hasPermission('wir', 'review') ||
                          this.permissionService.hasPermission('wir', 'manage');
    const baseCanReviewWIR = this.permissionService.hasPermission('wir', 'review') || 
                            this.permissionService.hasPermission('wir', 'manage');
    const baseCanManageCheckpoint = this.permissionService.hasPermission('wir', 'create') ||
                                   this.permissionService.hasPermission('wir', 'review') ||
                                   this.permissionService.hasPermission('wir', 'manage');
    
    // Check if activity actions are allowed based on box status
    const canPerformActivityActionsBasedOnBoxStatus = this.boxStatus 
      ? canPerformActivityActions(this.boxStatus as any)
      : true;
    
    // Disable all actions if project is archived, on hold, or closed, or if box status doesn't allow activity actions
    this.canUpdateProgress = baseCanUpdateProgress && !this.isProjectArchived && !this.isProjectOnHold && !this.isProjectClosed && canPerformActivityActionsBasedOnBoxStatus;
    this.canViewWIR = baseCanViewWIR && !this.isProjectArchived && !this.isProjectOnHold && !this.isProjectClosed && canPerformActivityActionsBasedOnBoxStatus;
    this.canReviewWIR = baseCanReviewWIR && !this.isProjectArchived && !this.isProjectOnHold && !this.isProjectClosed && canPerformActivityActionsBasedOnBoxStatus;
    this.canManageCheckpoint = baseCanManageCheckpoint && !this.isProjectArchived && !this.isProjectOnHold && !this.isProjectClosed && canPerformActivityActionsBasedOnBoxStatus;
  }

  ngOnInit(): void {
    this.loadActivitiesWithDetails();
    
    // Subscribe to router events to reload checkpoints when returning to the page
    this.routerSubscription = this.router.events
      .pipe(filter(event => event instanceof NavigationEnd))
      .subscribe(() => {
        // Reload checkpoints when navigation ends (user might have created a checkpoint)
        if (this.boxId) {
          this.loadWIRCheckpoints();
        }
      });
  }

  ngOnChanges(changes: SimpleChanges): void {
    // Reload data when boxId changes
    if (changes['boxId'] && !changes['boxId'].firstChange && changes['boxId'].currentValue) {
      this.loadActivitiesWithDetails();
    }
    
    // Update permissions when project status or box status changes
    if (changes['isProjectOnHold'] || changes['isProjectArchived'] || changes['isProjectClosed'] || changes['boxStatus']) {
      this.updatePermissions();
    }
  }

  ngOnDestroy(): void {
    // Clean up router subscription
    if (this.routerSubscription) {
      this.routerSubscription.unsubscribe();
    }
  }

  loadActivitiesWithDetails(): void {
    if (!this.boxId) return;

    this.isLoading = true;
    this.progressUpdateService.getBoxActivitiesWithProgress(this.boxId).subscribe({
      next: (activities) => {
        console.log(`📊 Loaded ${activities ? activities.length : 0} activities from API`);
        console.log('📊 Raw activities:', activities);
        
        // Ensure activities is an array
        const activitiesArray = Array.isArray(activities) ? activities : [];
        
        // Map the API response to match BoxActivityDetail interface
        // API returns: { seq, name, id, ... } but component expects: { sequence, activityName, boxActivityId, ... }
        this.activitiesWithDetails = activitiesArray.map((a: any) => ({
          boxActivityId: a.boxActivityId || a.id,
          boxId: a.boxId || this.boxId,
          activityMasterId: a.activityMasterId,
          activityName: a.activityName || a.name,
          activityCode: a.activityCode,
          sequence: a.sequence || a.seq,
          status: a.status || 'NotStarted',
          progressPercentage: a.progressPercentage || 0,
          plannedStartDate: a.plannedStartDate,
          plannedEndDate: a.plannedEndDate,
          actualStartDate: a.actualStartDate,
          actualEndDate: a.actualEndDate,
          duration: a.duration,
          actualDuration: a.actualDuration !== undefined && a.actualDuration !== null 
            ? a.actualDuration 
            : this.calculateActualDuration(a.actualStartDate, a.actualEndDate),
          workDescription: a.workDescription,
          issuesEncountered: a.issuesEncountered,
          teamId: a.teamId,
          teamName: a.teamName || a.assignedTeam,
          assignedMemberId: a.assignedMemberId,
          assignedMemberName: a.assignedMemberName || a.assignedTo,
          materialsAvailable: a.materialsAvailable !== undefined ? a.materialsAvailable : true,
          isActive: a.isActive !== undefined ? a.isActive : true,
          createdDate: a.createdDate,
          modifiedDate: a.modifiedDate,
          activityMaster: a.activityMaster,
          isWIRCheckpoint: a.isWIRCheckpoint,
          wirCode: a.wirCode,
          stage: a.stage
        }));
        
        console.log('📊 Mapped activities:', this.activitiesWithDetails.length);
        console.log('📊 Activity sequences:', this.activitiesWithDetails.map(a => ({ seq: a.sequence, name: a.activityName, id: a.boxActivityId })));
        this.loadWIRRecords();
      },
      error: (error) => {
        console.error('Error loading activities:', error);
        this.isLoading = false;
      }
    });
  }

  loadWIRRecords(): void {
    this.wirService.getWIRRecordsByBox(this.boxId).subscribe({
      next: (wirRecords) => {
        this.wirRecords = wirRecords;
        // Debug: Log WIR records with location data
        console.log('📋 WIR Records loaded:', wirRecords.map(w => ({
          wirCode: w.wirCode,
          bay: w.bay,
          row: w.row,
          position: w.position
        })));
        // Load checkpoints for all WIR records
        this.loadWIRCheckpoints();
      },
      error: (error) => {
        console.error('Error loading WIR records:', error);
        // Continue without WIR records
        this.buildTableRows();
        this.isLoading = false;
      }
    });
  }

  loadWIRCheckpoints(): void {
    if (!this.boxId) {
      this.buildTableRows();
      this.isLoading = false;
      return;
    }

    this.wirService.getWIRCheckpointsByBox(this.boxId).subscribe({
      next: (checkpoints) => {
        // Map checkpoints by WIR number/code
        this.wirCheckpoints.clear();
        checkpoints.forEach(checkpoint => {
          if (checkpoint.wirNumber) {
            this.wirCheckpoints.set(checkpoint.wirNumber.toUpperCase(), checkpoint);
          }
        });
        this.buildTableRows();
        this.isLoading = false;
      },
      error: (error) => {
        console.error('Error loading WIR checkpoints:', error);
        // Continue without checkpoints
        this.buildTableRows();
        this.isLoading = false;
      }
    });
  }

  buildTableRows(): void {
    const rows: TableRow[] = [];
    
    // Create a map of WIR records by their activity ID
    const wirMap = new Map<string, WIRRecord>();
    this.wirRecords.forEach(wir => {
      wirMap.set(wir.boxActivityId, wir);
    });

    // Use all activities (no filtering by isActive)
    console.log(`📊 Total activities: ${this.activitiesWithDetails.length}`);
    
    // Sort activities by sequence and remove duplicates by boxActivityId
    const uniqueActivities = new Map<string, BoxActivityDetail>();
    const duplicateSequences = new Map<number, number>();
    const seenBoxActivityIds = new Set<string>();
    
    this.activitiesWithDetails.forEach(activity => {
      // Use boxActivityId as the unique key (it's always present and unique)
      const key = activity.boxActivityId;
      
      if (!key) {
        console.warn(`⚠️ Activity missing boxActivityId:`, activity);
        return;
      }
      
      // Check for duplicate boxActivityId
      if (seenBoxActivityIds.has(key)) {
        console.warn(`⚠️ Duplicate boxActivityId detected: ${key}`, activity);
        return; // Skip duplicates
      }
      
      seenBoxActivityIds.add(key);
      uniqueActivities.set(key, activity);
      
      // Track sequence numbers to detect duplicates
      if (activity.sequence) {
        duplicateSequences.set(activity.sequence, (duplicateSequences.get(activity.sequence) || 0) + 1);
      }
    });
    
    // Check for duplicate sequence numbers
    const duplicateSeqs = Array.from(duplicateSequences.entries()).filter(([seq, count]) => count > 1);
    if (duplicateSeqs.length > 0) {
      console.warn(`⚠️ Found duplicate sequence numbers:`, duplicateSeqs);
      console.warn(`⚠️ Activities with duplicate sequences:`, 
        this.activitiesWithDetails.filter((a: BoxActivityDetail) => duplicateSeqs.some(([seq]) => a.sequence === seq))
      );
    }
    
    const sortedActivities = Array.from(uniqueActivities.values()).sort((a, b) => (a.sequence || 0) - (b.sequence || 0));
    
    console.log(`📊 After deduplication: ${sortedActivities.length} unique activities`);
    console.log('📊 Sorted sequences:', sortedActivities.map(a => ({ seq: a.sequence, name: a.activityName })));
    
    // Final validation: should match expected count
    if (sortedActivities.length !== this.activitiesWithDetails.length) {
      console.warn(`⚠️ Count mismatch: ${sortedActivities.length} unique vs ${this.activitiesWithDetails.length} total`);
    }

    // Build combined rows: activity followed by its WIR (if exists)
    sortedActivities.forEach(activity => {
      // Add activity row
      rows.push({
        type: 'activity',
        sequence: activity.sequence,
        data: activity
      });

      // If this activity has a WIR record, add WIR row after it
      const wirRecord = wirMap.get(activity.boxActivityId);
      if (this.isWIRCheckpoint(activity) || wirRecord) {
        const wirData = wirRecord || this.createPlaceholderWIR(activity);
        rows.push({
          type: 'wir',
          sequence: activity.sequence,
          data: wirData,
          isPlaceholder: !wirRecord
        });
      }
    });

    this.tableRows = rows;
    
    // Log for debugging - separate activity and WIR counts
    const activityCount = rows.filter(r => r.type === 'activity').length;
    const wirCount = rows.filter(r => r.type === 'wir').length;
    console.log(`📊 Activity Table: ${this.activitiesWithDetails.length} activities loaded, ${activityCount} activity rows displayed, ${wirCount} WIR rows, ${rows.length} total rows`);
    
    // Emit the actual activity count to parent component
    this.activityCountChanged.emit(activityCount);
  }

  /**
   * Get count of activity rows only (excluding WIR rows)
   */
  get activityCount(): number {
    return this.tableRows.filter(row => row.type === 'activity').length;
  }

  /**
   * Get count of WIR rows only
   */
  get wirRowCount(): number {
    return this.tableRows.filter(row => row.type === 'wir').length;
  }

  /**
   * Open progress update modal
   */
  openProgressModal(activity: BoxActivityDetail): void {
    // Allow opening modal for completed activities (to update position only)
    // Only block OnHold activities
    if (activity.status === 'OnHold') {
      document.dispatchEvent(new CustomEvent('app-toast', {
        detail: { 
          message: 'Cannot update progress. Activities in "OnHold" status cannot be modified. Please change the activity status first.',
          type: 'error' 
        }
      }));
      return;
    }

    // Block if activity is completed/delayed and next WIR has position disabled
    if (this.shouldDisableUpdateProgress(activity)) {
      document.dispatchEvent(new CustomEvent('app-toast', {
        detail: { 
          message: 'Cannot update progress. Activity is completed/delayed and the next WIR position is locked.',
          type: 'error' 
        }
      }));
      return;
    }

    this.selectedActivity = activity;
    this.isModalOpen = true;
  }

  /**
   * Close progress modal
   */
  closeProgressModal(): void {
    this.isModalOpen = false;
    this.selectedActivity = null;
  }

  /**
   * Handle progress update completion
   */
  onProgressUpdated(response: any): void {
    console.log('Progress updated:', response);
    
    // Reload activities to get updated data
    this.loadActivitiesWithDetails();
    
    // Notify parent component that box data may have changed (status, progress, etc.)
    this.boxDataChanged.emit();
    
    // Show WIR notification if created
    if (response.wirCreated) {
      alert(`✅ Progress updated! WIR ${response.wirCode} has been created for QC inspection.`);
    }
  }

  /**
   * Check if activity is a WIR checkpoint
   */
  isWIRCheckpoint(activity: any): boolean {
    if ('isWIRCheckpoint' in activity) {
      return activity.isWIRCheckpoint;
    }
    // Fallback: Check if activity name contains WIR
    return activity.name?.toLowerCase().includes('wir') || 
           activity.activityName?.toLowerCase().includes('wir') ||
           activity.name?.toLowerCase().includes('clearance') ||
           activity.activityName?.toLowerCase().includes('clearance') ||
           false;
  }

  /**
   * Get team name from activity
   */
  getTeam(activity: any): string {
    // Use teamName directly from the activity
    const teamName = activity?.teamName;
    
    if (!teamName || teamName === 'N/A' || teamName === '-') {
      return '';
    }
    
    return teamName;
  }

  /**
   * Get status badge class
   */
  getStatusClass(status?: string | null): string {
    if (!status) return 'status-default';
    const statusMap: Record<string, string> = {
      'NotStarted': 'status-not-started',
      'InProgress': 'status-in-progress',
      'Completed': 'status-completed',
      'Approved': 'status-approved',
      'OnHold': 'status-on-hold'
    };
    return statusMap[status] || 'status-default';
  }

  /**
   * Get status label for display
   */
  getStatusLabel(status?: string): string {
    if (!status) return '—';
    
    const statusMap: Record<string, string> = {
      'NotStarted': 'Not Started',
      'InProgress': 'In Progress',
      'Completed': 'Completed',
      'Approved': 'Approved',
      'OnHold': 'On Hold',
      'Delayed': 'Delayed'
    };
    return statusMap[status] || status;
  }

  /**
   * Format date
   */
  formatDate(date?: Date | string): string {
    if (!date) return '';
    const d = new Date(date);
    if (isNaN(d.getTime())) return '';
    const day = d.getDate().toString().padStart(2, '0');
    const month = d.toLocaleString('en-US', { month: 'short' });
    const year = d.getFullYear().toString().slice(-2);
    return `${day}-${month}-${year}`;
  }

  /**
   * Calculate actual duration in days (legacy - for backward compatibility)
   * Use calculateAndFormatDuration from utils for display
   */
  calculateActualDuration(actualStartDate?: Date | string, actualEndDate?: Date | string): number | undefined {
    return calculateDurationInDays(actualStartDate, actualEndDate);
  }

  /**
   * Get display text for actual duration with flexible formatting:
   * - Less than 24 hours: shows hours (e.g., "5 hours", "12.5 hours")
   * - 24 hours or more: shows days and hours (e.g., "2 days 5 hours")
   */
  getActualDurationDisplay(activity: BoxActivityDetail | null): string {
    if (!activity) return '-';
    
    // If we have actual start and end dates, calculate precise duration
    if (activity.actualStartDate && activity.actualEndDate) {
      const formatted = calculateAndFormatDuration(activity.actualStartDate, activity.actualEndDate);
      if (formatted) return formatted;
    }
    
    // Fallback: If actualDuration is explicitly set (legacy days value), show it
    if (activity.actualDuration !== undefined && activity.actualDuration !== null) {
      return activity.actualDuration === 1 ? '1 day' : `${activity.actualDuration} days`;
    }
    
    // If we can't calculate, show "-"
    return '-';
  }

  /**
   * Get progress percentage
   */
  getProgress(activity: any): number {
    return activity.weightPercentage || activity.progressPercentage || 0;
  }

  /**
   * Get activity name
   */
  getActivityName(activity: any): string {
    return activity.name || activity.activityName || 'Unknown Activity';
  }

  /**
   * Get activity ID
   */
  getActivityId(activity: any): string {
    return activity.id || activity.boxActivityId || '';
  }

  /**
   * Get assigned to name for regular activities
   * Note: WIR rows have their own getWIRInspectorName() method
   */
  getAssignedTo(activity: any): string {
    // Use assignedMemberName which contains the full name of the assigned person
    const assignedName = activity?.assignedMemberName;
    
    if (!assignedName || assignedName === 'N/A' || assignedName === '') {
      return '-';
    }
    
    return assignedName;
  }

  /**
   * Get WIR description for checkpoint
   */
  getWIRDescription(wir: any): string {
    if (!wir) return 'Clearance/WIR';
    
    const wirCode = wir.wirCode || '';
    const descriptionMap: Record<string, string> = {
      'WIR-1': 'Release from Assembly - WIR-1',
      'WIR-2': 'Mechanical Clearance - WIR-2',
      'WIR-3': 'Ceiling Closure - WIR-3',
      'WIR-4': '3rd Fix Installation - WIR-4',
      'WIR-5': '3rd Fix Installation - WIR-5',
      'WIR-6': 'Readiness for Dispatch - WIR-6'
    };
    
    return descriptionMap[wirCode] || `${wirCode} Clearance`;
  }

  /**
   * Get activities list (use detailed if available)
   */
  getActivitiesList(): any[] {
    return this.activitiesWithDetails.length > 0 ? this.activitiesWithDetails : this.activities;
  }

  /**
   * Check if row is a WIR row
   */
  isWIRRow(row: TableRow): boolean {
    return row.type === 'wir';
  }

  /**
   * Check if row is an activity row
   */
  isActivityRow(row: TableRow): boolean {
    return row.type === 'activity';
  }

  /**
   * Get activity from row
   */
  getActivity(row: TableRow): BoxActivityDetail | null {
    return row.type === 'activity' ? (row.data as BoxActivityDetail) : null;
  }

  /**
   * Get WIR record from row
   */
  getWIR(row: TableRow): WIRRecord | null {
    return row.type === 'wir' ? (row.data as WIRRecord) : null;
  }

  hasRealWIR(row: TableRow): boolean {
    return this.isWIRRow(row) && !row.isPlaceholder && !!(row.data as WIRRecord)?.wirRecordId;
  }

  isPlaceholderWIR(row: TableRow): boolean {
    return this.isWIRRow(row) && !!row.isPlaceholder;
  }

  /**
   * Get WIR display status (prefer checkpoint status if available, otherwise WIR record status)
   */
  getWIRDisplayStatus(row: TableRow): any {
    const wir = this.getWIR(row);
    if (!wir) return 'Pending';
    
    // If checkpoint exists, use checkpoint status
    const checkpoint = this.getCheckpoint(wir);
    if (checkpoint && checkpoint.status) {
      return checkpoint.status;
    }
    
    // Otherwise, fall back to WIR record status
    return wir.status || 'Pending';
  }

  /**
   * Get WIR status class
   * If all checklist items are PASS, use approved class, otherwise use under-review class
   */
  getWIRStatusClass(status: any, wirRecord?: WIRRecord | null): string {
    if (!wirRecord) return 'wir-status-pending';

    const checkpoint = this.getCheckpoint(wirRecord);
    
    // No checkpoint created → pending class
    if (!checkpoint) {
      return 'wir-status-pending';
    }
    
    // Checkpoint exists - check its status
    const checkpointStatus = checkpoint.status ? String(checkpoint.status) : 'Pending';
    
    // If checkpoint status is Pending → under-review class
    if (checkpointStatus === 'Pending') {
      return 'wir-status-under-review';
    }
    
    // If checkpoint status is Approved, check if all items are Pass
    if (checkpointStatus === 'Approved') {
      if (this.areAllChecklistItemsPass(checkpoint)) {
        return 'wir-status-approved'; // All items passed
      }
      return 'wir-status-under-review'; // Approved but still reviewing items
    }
    
    // Map other statuses to their classes
    const statusMap: Record<string, string> = {
      'Rejected': 'wir-status-rejected',
      'ConditionalApproval': 'wir-status-conditional'
    };
    return statusMap[checkpointStatus] || 'wir-status-pending';
  }

  /**
   * Format WIR status label
   * Logic:
   * - Not created (no checkpoint) → "Pending"
   * - Created but not reviewed (checkpoint status is Pending) → "Under Review"
   * - Reviewed (checkpoint has final status) → Show final status (Approved, Rejected, ConditionalApproval)
   *   - If Approved but not all items are Pass → "Under Review"
   */
  getWIRStatusLabel(status: any, wirRecord?: WIRRecord | null): string {
    if (!wirRecord) return 'Pending';
    
    const checkpoint = this.getCheckpoint(wirRecord);
    
    // No checkpoint created → "Pending"
    if (!checkpoint) {
      return 'Pending';
    }
    
    // Checkpoint exists - check its status
    const checkpointStatus = checkpoint.status ? String(checkpoint.status) : 'Pending';
    
    // If checkpoint status is Pending → "Under Review"
    if (checkpointStatus === 'Pending') {
      return 'Under Review';
    }
    
    // If checkpoint status is Approved, check if all items are Pass
    if (checkpointStatus === 'Approved') {
      if (this.areAllChecklistItemsPass(checkpoint)) {
        return 'Approved'; // All items passed - fully approved
      }
      return 'Under Review'; // Approved but still reviewing items
    }
    
    // For other statuses (Rejected, ConditionalApproval), show the actual status
    return checkpointStatus;
  }

  /**
   * Check if all checklist items in a checkpoint have PASS status
   */
  private areAllChecklistItemsPass(checkpoint: WIRCheckpoint): boolean {
    if (!checkpoint.checklistItems || checkpoint.checklistItems.length === 0) {
      return false;
    }

    return checkpoint.checklistItems.every(item => {
      const status = item.status;
      if (!status) return false;
      const statusStr = typeof status === 'string' ? status : String(status);
      return statusStr === 'Pass' || statusStr === 'pass';
    });
  }

  private createPlaceholderWIR(activity: BoxActivityDetail): WIRRecord {
    const wirCode = activity.wirCode || activity.activityMaster?.wirCode || this.generateWirCode(activity.sequence);
    return {
      wirRecordId: '',
      boxActivityId: activity.boxActivityId,
      boxTag: '',
      activityName: activity.activityName,
      wirCode,
      status: WIRStatus.Pending,
      checkpointStatus: WIRStatus.Pending,
      requestedDate: new Date(),
      requestedBy: '',
      requestedByName: '',
      inspectedBy: activity.assignedMemberId,
      inspectedByName: activity.assignedMemberName || 'QC Engineer',
      inspectionDate: undefined,
      inspectionNotes: '',
      photoUrls: '',
      rejectionReason: '',
      checklistItems: [],
      bay: undefined,
      row: undefined,
      position: undefined
    };
  }

  private generateWirCode(sequence: number): string {
    if (!sequence) return 'WIR';
    return `WIR-${sequence}`;
  }

  /**
   * Check if WIR has assigned inspector (or should show default)
   * Returns true if WIR exists (even if not assigned, we'll show "QC Engineer" by default)
   */
  hasWIRInspector(row: TableRow): boolean {
    const wir = this.getWIR(row);
    return !!wir; // Return true if WIR exists, regardless of assignment
  }

  /**
   * Get WIR inspector name, defaulting to "QC Engineer" if not assigned
   */
  getWIRInspectorName(row: TableRow): string {
    const wir = this.getWIR(row);
    return wir?.inspectedByName || 'QC Engineer';
  }

  /**
   * Get WIR bay
   */
  getWIRBay(row: TableRow): string {
    const wir = this.getWIR(row);
    const bay = wir?.bay;
    return (bay && bay.trim() !== '') ? bay : '-';
  }

  /**
   * Get WIR row (position row)
   */
  getWIRRow(row: TableRow): string {
    const wir = this.getWIR(row);
    const rowValue = wir?.row;
    return (rowValue && rowValue.trim() !== '') ? rowValue : '-';
  }

  /**
   * Get WIR position
   */
  getWIRPosition(row: TableRow): string {
    const wir = this.getWIR(row);
    const position = wir?.position;
    return (position && position.trim() !== '') ? position : '-';
  }

  /**
   * Check if WIR has location info (bay, row, or position)
   */
  hasWIRLocation(row: TableRow): boolean {
    const wir = this.getWIR(row);
    if (!wir) return false;
    // Check if any location field exists and is not empty or dash
    const hasBay = wir.bay && wir.bay !== '-' && wir.bay.trim() !== '';
    const hasRow = wir.row && wir.row !== '-' && wir.row.trim() !== '';
    const hasPosition = wir.position && wir.position !== '-' && wir.position.trim() !== '';
    return !!(hasBay || hasRow || hasPosition);
  }

  /**
   * Check if WIR has actions
   */
  hasWIRActions(row: TableRow): boolean {
    return this.hasRealWIR(row);
  }

  /**
   * Get colspan for Activity cell (merge with Assigned to if empty)
   */
  getActivityColspan(row: TableRow): number {
    return this.hasWIRInspector(row) ? 1 : 2;
  }

  /**
   * Get colspan for Status cell (merge with Actions if empty)
   */
  getStatusColspan(row: TableRow): number {
    return this.hasWIRActions(row) ? 1 : 2;
  }

  /**
   * Open WIR approval modal
   */
  openWIRApprovalModal(wirRecord: WIRRecord): void {
    this.selectedWIR = wirRecord;
    // Use setTimeout to ensure the modal component is created before setting isOpen
    setTimeout(() => {
      this.isWIRModalOpen = true;
      this.cdr.detectChanges();
    }, 0);
  }

  /**
   * Close WIR approval modal
   */
  closeWIRApprovalModal(): void {
    this.isWIRModalOpen = false;
    this.selectedWIR = null;
  }

  /**
   * Handle WIR update (approve/reject)
   */
  onWIRUpdated(updatedWIR: WIRRecord): void {
    console.log('WIR updated:', updatedWIR);
    
    // Reload WIR records and checkpoints to get updated data
    this.loadWIRRecords();
    
    // Notify parent component
    this.boxDataChanged.emit();
  }

  /**
   * Check if WIR has been reviewed (status is not Pending)
   */
  isWIRReviewed(row: TableRow): boolean {
    const wir = this.getWIR(row);
    if (!wir) return false;
    
    const status = wir.status;
    return status !== WIRStatus.Pending && status !== null && status !== undefined;
  }

  /**
   * Check if a checkpoint exists for a WIR record
   */
  hasCheckpoint(wirRecord: WIRRecord): boolean {
    if (!wirRecord?.wirCode) return false;
    return this.wirCheckpoints.has(wirRecord.wirCode.toUpperCase());
  }

  /**
   * Get checkpoint for a WIR record
   */
  getCheckpoint(wirRecord: WIRRecord): WIRCheckpoint | null {
    if (!wirRecord?.wirCode) return null;
    return this.wirCheckpoints.get(wirRecord.wirCode.toUpperCase()) || null;
  }

  /**
   * Determine the appropriate step to navigate to based on checkpoint status
   */
  getCheckpointStep(checkpoint: WIRCheckpoint): 'add-items' | 'review' | 'quality-issues' {
    // Step 1 (Create Checkpoint) is always completed if checkpoint exists
    
    // Check checkpoint status
    const status = checkpoint.status as any;
    const isPending = !status || status === 'Pending' || status === 'pending';
    
    if (isPending) {
      // Status is Pending → Navigate to Step 2 (Add Items)
      return 'add-items';
    } else {
      // Status is NOT Pending (Approved, Rejected, etc.) → Navigate to Step 3 (Review)
      return 'review';
    }
  }

  /**
   * Check if user can view or add checkpoint for a WIR record
   * Requires create, review, or manage permission (NOT just view)
   */
  canViewOrAddCheckpoint(wirRecord: WIRRecord): boolean {
    // User needs create, review, or manage permission to access checkpoints
    // wir.view permission alone is NOT sufficient
    return this.canManageCheckpoint;
  }

  /**
   * Navigate to create or view WIR checkpoint page
   */
  navigateToCreateWIRCheckpoint(wirRecord: WIRRecord): void {
    if (!this.projectId || !this.boxId || !wirRecord.boxActivityId) {
      console.error('Missing required IDs for navigation');
      return;
    }
    
    const checkpoint = this.getCheckpoint(wirRecord);
    
    if (checkpoint) {
      // Checkpoint exists - navigate to the appropriate step based on checkpoint status
      const targetStep = this.getCheckpointStep(checkpoint);
      
      this.router.navigate([
        '/projects',
        this.projectId,
        'boxes',
        this.boxId,
        'activities',
        wirRecord.boxActivityId,
        'qa-qc'
      ], {
        queryParams: { step: targetStep }
      }).then(() => {
        // Reload checkpoints after navigation to ensure button text is updated
        this.loadWIRCheckpoints();
      });
    } else {
      // No checkpoint exists - navigate to create (Step 1 of multi-step flow)
      this.router.navigate([
        '/projects',
        this.projectId,
        'boxes',
        this.boxId,
        'activities',
        wirRecord.boxActivityId,
        'qa-qc'
      ], {
        queryParams: { step: 'create-checkpoint' }
      }).then(() => {
        // Reload checkpoints after navigation to ensure button text is updated
        this.loadWIRCheckpoints();
      });
    }
  }

  /**
   * Public method to refresh checkpoints (can be called from parent component)
   */
  refreshCheckpoints(): void {
    this.loadWIRCheckpoints();
  }

  /**
   * Navigate to activity details page with returnTo query param
   */
  navigateToActivityDetails(activity: BoxActivityDetail | null): void {
    if (!activity || !this.projectId || !this.boxId || !activity.boxActivityId) {
      console.error('Missing required IDs for navigation');
      return;
    }
    
    // Navigate to activity details with returnTo query param to indicate coming from activities table
    this.router.navigate(
      ['/projects', this.projectId, 'boxes', this.boxId, 'activities', activity.boxActivityId],
      { queryParams: { returnTo: 'activities' } }
    );
  }

  /**
   * Check if update progress button should be disabled
   * Disabled when: activity is completed or delayed AND next WIR has position disabled (position values already set)
   */
  shouldDisableUpdateProgress(activity: BoxActivityDetail): boolean {
    // Only disable if activity is completed or delayed
    if (!activity || (activity.status !== 'Completed' && activity.status !== 'Delayed')) {
      return false;
    }

    // Find the next WIR (at or after this activity's sequence)
    const currentSequence = activity.sequence || 0;
    const nextWIR = this.findNextWIR(currentSequence);

    if (!nextWIR) {
      // No next WIR found - allow update
      return false;
    }

    // Check if the next WIR has position values set (which means position is disabled/locked)
    const hasPositionValues = this.hasWIRPositionValues(nextWIR);
    
    // Disable button if next WIR has position values set
    return hasPositionValues;
  }

  /**
   * Find the next WIR record (at or after the given sequence)
   * This finds the WIR checkpoint that controls position values for activities at or after this sequence
   */
  private findNextWIR(sequence: number): WIRRecord | null {
    // Find WIR records that are at or after this sequence
    // These are the WIRs that would control position values for the current activity
    const nextWIRRecords = this.wirRecords
      .filter(wir => {
        // Find the activity this WIR belongs to
        const activity = this.activitiesWithDetails.find(a => a.boxActivityId === wir.boxActivityId);
        if (!activity) return false;
        // Include WIRs at or after the current sequence (they control position for this activity)
        return (activity.sequence || 0) >= sequence;
      })
      .sort((a, b) => {
        // Sort by sequence ascending to get the nearest/first WIR
        const seqA = this.activitiesWithDetails.find(a2 => a2.boxActivityId === a.boxActivityId)?.sequence || 0;
        const seqB = this.activitiesWithDetails.find(a2 => a2.boxActivityId === b.boxActivityId)?.sequence || 0;
        return seqA - seqB;
      });

    return nextWIRRecords.length > 0 ? nextWIRRecords[0] : null;
  }

  /**
   * Check if WIR has position values set (bay, row, or position)
   */
  private hasWIRPositionValues(wir: WIRRecord): boolean {
    if (!wir) return false;
    
    const bayValue = (wir.bay && wir.bay.trim() !== '' && wir.bay !== '-') ? wir.bay : '';
    const rowValue = (wir.row && wir.row.trim() !== '' && wir.row !== '-') ? wir.row : '';
    const positionValue = (wir.position && wir.position.trim() !== '' && wir.position !== '-') ? wir.position : '';
    
    // Check if any position value is set
    return !!(bayValue || rowValue || positionValue);
  }
}
