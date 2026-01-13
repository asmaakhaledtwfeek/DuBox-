import { Component, Input, Output, EventEmitter, OnInit, OnChanges, SimpleChanges, OnDestroy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { TeamService } from '../../../core/services/team.service';
import { Team, TeamMember } from '../../../core/models/team.model';
import { Subscription } from 'rxjs';

export interface AssignableIssue {
  issueId?: string;
  issueType?: string;
  severity?: string;
  assignedTo?: string | null;
  assignedToUserId?: string | null;
  assignedTeamName?: string | null;
}

@Component({
  selector: 'app-assign-to-crew-modal',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './assign-to-crew-modal.component.html',
  styleUrls: ['./assign-to-crew-modal.component.scss']
})
export class AssignToCrewModalComponent implements OnInit, OnChanges, OnDestroy {
  @Input() issue: AssignableIssue | null = null;
  @Input() isOpen: boolean = false;
  @Output() close = new EventEmitter<void>();
  @Output() assign = new EventEmitter<{ teamId: string | null; memberId: string | null }>();

  availableTeams: Team[] = [];
  availableMembers: TeamMember[] = [];
  loadingTeams = false;
  loadingMembers = false;
  assignLoading = false;
  assignError = '';
  selectedTeamId: string | null = null;
  selectedMemberId: string | null = null;

  private teamIdSubscription?: Subscription;

  constructor(private teamService: TeamService) {}

  ngOnInit(): void {
    if (this.isOpen && this.issue) {
      this.initializeModal();
    }
  }

  ngOnChanges(changes: SimpleChanges): void {
    if (changes['isOpen'] && changes['isOpen'].currentValue && this.issue) {
      this.initializeModal();
    } else if (changes['isOpen'] && !changes['isOpen'].currentValue) {
      this.resetModal();
    }
  }

  ngOnDestroy(): void {
    if (this.teamIdSubscription) {
      this.teamIdSubscription.unsubscribe();
    }
  }

  private initializeModal(): void {
    if (!this.issue) return;

    this.assignError = '';
    
    // Preselect team and member if issue is already assigned
    const assignedTeamId = this.issue.assignedTo || (this.issue as any).assignedToTeamId || (this.issue as any).AssignedToTeamId;
    const assignedMemberId = this.issue.assignedToUserId || (this.issue as any).assignedToMemberId || (this.issue as any).AssignedToMemberId;
    
    // Load teams first, then preselect team and member if they exist
    this.loadAvailableTeams(() => {
      if (assignedTeamId) {
        const teamId = assignedTeamId.toString();
        const teamExists = this.availableTeams.some(t => t.teamId === teamId);
        
        if (teamExists) {
          this.selectedTeamId = teamId;
          // Load members and preselect member if it exists
          if (assignedMemberId) {
            this.loadTeamMembers(teamId, (members) => {
              const memberId = assignedMemberId.toString();
              const memberExists = members.some(m => m.teamMemberId === memberId);
              if (memberExists) {
                this.selectedMemberId = memberId;
              }
            });
          } else {
            this.loadTeamMembers(teamId);
          }
        } else {
          // Team not in available teams (might be inactive), but still set it
          console.warn('Team not found in available teams:', teamId);
          this.selectedTeamId = teamId;
          if (assignedMemberId) {
            this.selectedMemberId = assignedMemberId.toString();
          }
        }
      } else {
        // No team assigned, reset selections
        this.selectedTeamId = null;
        this.selectedMemberId = null;
        this.availableMembers = [];
      }
    });
  }

  private resetModal(): void {
    this.selectedTeamId = null;
    this.selectedMemberId = null;
    this.availableMembers = [];
    this.assignError = '';
    this.assignLoading = false;
    
    if (this.teamIdSubscription) {
      this.teamIdSubscription.unsubscribe();
      this.teamIdSubscription = undefined;
    }
  }

  onClose(): void {
    this.resetModal();
    this.close.emit();
  }

  onBackdropClick(): void {
    if (!this.assignLoading) {
      this.onClose();
    }
  }

  loadAvailableTeams(onComplete?: () => void): void {
    this.loadingTeams = true;
    this.teamService.getTeams().subscribe({
      next: (teams) => {
        this.availableTeams = teams.filter(team => team.isActive);
        this.loadingTeams = false;
        if (onComplete) {
          onComplete();
        }
      },
      error: (err) => {
        console.error('Error loading teams:', err);
        this.loadingTeams = false;
        this.availableTeams = [];
        this.assignError = 'Failed to load teams. Please try again.';
        if (onComplete) {
          onComplete();
        }
      }
    });
  }

  onTeamSelectionChange(teamId: string): void {
    this.selectedTeamId = teamId;
    this.selectedMemberId = null; // Reset member selection when team changes
    
    if (teamId) {
      this.loadTeamMembers(teamId);
    } else {
      this.availableMembers = [];
    }
  }

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
      next: (response) => {
        this.availableMembers = response.members || [];
        this.loadingMembers = false;
        if (onComplete) {
          onComplete(this.availableMembers);
        }
      },
      error: (err) => {
        console.error('Error loading team members:', err);
        this.availableMembers = [];
        this.loadingMembers = false;
        if (onComplete) {
          onComplete([]);
        }
      }
    });
  }

  onAssign(): void {
    if (!this.issue || !this.issue.issueId) {
      this.assignError = 'Invalid issue selected';
      return;
    }

    const teamId = this.selectedTeamId && this.selectedTeamId.trim() !== '' ? this.selectedTeamId : null;
    const memberId = this.selectedMemberId && this.selectedMemberId.trim() !== '' ? this.selectedMemberId : null;

    this.assign.emit({ teamId, memberId });
  }

  getIssueId(): string | undefined {
    return this.issue?.issueId || (this.issue as any)?.IssueId || (this.issue as any)?.qualityIssueId || (this.issue as any)?.QualityIssueId;
  }
}

