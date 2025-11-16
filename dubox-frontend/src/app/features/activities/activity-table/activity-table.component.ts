import { Component, Input } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { BoxActivity } from '../../../core/models/box.model';

@Component({
  selector: 'app-activity-table',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './activity-table.component.html',
  styleUrls: ['./activity-table.component.scss']
})
export class ActivityTableComponent {
  @Input() activities: BoxActivity[] = [];
  @Input() projectId: string = '';
  @Input() boxId: string = '';

  /**
   * Check if activity is a WIR checkpoint
   */
  isWIRCheckpoint(activity: BoxActivity): boolean {
    // Check if activity name contains WIR or is marked as WIR checkpoint
    return activity.name?.toLowerCase().includes('wir') || 
           activity.name?.toLowerCase().includes('clearance') ||
           activity.name?.toLowerCase().includes('inspection') ||
           false;
  }

  /**
   * Get team name from assigned team
   */
  getTeam(activity: BoxActivity): string {
    const team = activity.assignedTo || 'N/A';
    
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
  formatDate(date?: Date): string {
    if (!date) return '';
    const d = new Date(date);
    const day = d.getDate().toString().padStart(2, '0');
    const month = d.toLocaleString('en-US', { month: 'short' });
    const year = d.getFullYear().toString().slice(-2);
    return `${day}-${month}-${year}`;
  }

  /**
   * Get progress percentage
   */
  getProgress(activity: BoxActivity): number {
    return activity.weightPercentage || 0;
  }
}
