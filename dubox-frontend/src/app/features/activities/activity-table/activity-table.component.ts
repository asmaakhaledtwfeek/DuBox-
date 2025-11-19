import { Component, Input, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { BoxActivity } from '../../../core/models/box.model';
import { UpdateProgressModalComponent } from '../update-progress-modal/update-progress-modal.component';
import { ProgressUpdateService } from '../../../core/services/progress-update.service';
import { BoxActivityDetail } from '../../../core/models/progress-update.model';
import { WIRService } from '../../../core/services/wir.service';
import { WIRRecord } from '../../../core/models/wir.model';

// Combined type for activities and WIR rows
export interface TableRow {
  type: 'activity' | 'wir';
  sequence: number;
  data: BoxActivityDetail | WIRRecord;
}

@Component({
  selector: 'app-activity-table',
  standalone: true,
  imports: [CommonModule, RouterModule, UpdateProgressModalComponent],
  templateUrl: './activity-table.component.html',
  styleUrls: ['./activity-table.component.scss']
})
export class ActivityTableComponent implements OnInit {
  @Input() activities: BoxActivity[] = [];
  @Input() projectId: string = '';
  @Input() boxId: string = '';

  activitiesWithDetails: BoxActivityDetail[] = [];
  wirRecords: WIRRecord[] = [];
  tableRows: TableRow[] = [];
  isModalOpen = false;
  selectedActivity: BoxActivityDetail | null = null;
  isLoading = false;

  constructor(
    private progressUpdateService: ProgressUpdateService,
    private wirService: WIRService
  ) {}

  ngOnInit(): void {
    this.loadActivitiesWithDetails();
  }

  loadActivitiesWithDetails(): void {
    if (!this.boxId) return;

    this.isLoading = true;
    this.progressUpdateService.getBoxActivitiesWithProgress(this.boxId).subscribe({
      next: (activities) => {
        this.activitiesWithDetails = activities;
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
        this.buildTableRows();
        this.isLoading = false;
      },
      error: (error) => {
        console.error('Error loading WIR records:', error);
        // Continue without WIR records
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

    // Sort activities by sequence
    const sortedActivities = [...this.activitiesWithDetails].sort((a, b) => a.sequence - b.sequence);

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
      if (wirRecord) {
        rows.push({
          type: 'wir',
          sequence: activity.sequence,
          data: wirRecord
        });
      }
    });

    this.tableRows = rows;
    console.log('✅ Built table with', rows.length, 'rows (activities + WIRs)');
  }

  /**
   * Open progress update modal
   */
  openProgressModal(activity: BoxActivityDetail): void {
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
   * Get team name from assigned team
   */
  getTeam(activity: any): string {
    const team = activity.assignedTo || activity.teamName || 'N/A';
    
    // Extract team name (e.g., "QC Engineer-Civil" -> "QA/QC")
    if (team.toLowerCase().includes('qc') || team.toLowerCase().includes('qa')) {
      return 'QA/QC';
    } else if (team.toLowerCase().includes('mep') || team.toLowerCase().includes('mechanical') || team.toLowerCase().includes('electrical')) {
      return 'MEP';
    } else if (team.toLowerCase().includes('civil') || team.toLowerCase().includes('finishing')) {
      return 'Civil';
    }
    
    return 'General';
  }

  /**
   * Get status badge class
   */
  getStatusClass(status: string): string {
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
   * Get assigned to name
   */
  getAssignedTo(activity: any): string {
    if (this.isWIRCheckpoint(activity)) {
      return activity.assignedTo || activity.assignedMemberName || 'QC Engineer';
    }
    return activity.assignedTo || activity.assignedMemberName || '-';
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

  /**
   * Get WIR status class
   */
  getWIRStatusClass(status: any): string {
    if (!status) return 'wir-status-pending';
    const statusStr = String(status);
    const statusMap: Record<string, string> = {
      'Pending': 'wir-status-pending',
      'Approved': 'wir-status-approved',
      'Rejected': 'wir-status-rejected'
    };
    return statusMap[statusStr] || 'wir-status-pending';
  }

  /**
   * Format WIR status label
   */
  getWIRStatusLabel(status: any): string {
    return status ? String(status) : 'Pending';
  }
}
