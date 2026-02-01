import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router, RouterModule, ActivatedRoute } from '@angular/router';
import { TeamService } from '../../../core/services/team.service';
import { TeamGroup, TeamMember } from '../../../core/models/team.model';
import { HeaderComponent } from '../../../shared/components/header/header.component';
import { SidebarComponent } from '../../../shared/components/sidebar/sidebar.component';
import { PermissionService } from '../../../core/services/permission.service';

@Component({
  selector: 'app-team-group-details',
  standalone: true,
  imports: [CommonModule, RouterModule, HeaderComponent, SidebarComponent, FormsModule],
  templateUrl: './team-group-details.component.html',
  styleUrls: ['./team-group-details.component.scss']
})
export class TeamGroupDetailsComponent implements OnInit {
  teamGroup: TeamGroup | null = null;
  groupMembers: TeamMember[] = [];
  loading = true;
  loadingMembers = false;
  error = '';
  teamId = '';
  groupId = '';

  // Permission flags
  canEdit = false;
  canManageMembers = false;

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private teamService: TeamService,
    private permissionService: PermissionService
  ) {}

  ngOnInit(): void {
    this.teamId = this.route.snapshot.paramMap.get('teamId') || '';
    this.groupId = this.route.snapshot.paramMap.get('groupId') || '';

    // Check permissions
    this.canEdit = this.permissionService.hasPermission('teams', 'edit');
    this.canManageMembers = this.permissionService.hasPermission('teams', 'manage-members') || 
                            this.permissionService.hasPermission('teams', 'manage');

    if (this.groupId) {
      this.loadGroupDetails();
    }
  }

  loadGroupDetails(): void {
    this.loading = true;
    this.teamService.getTeamGroupById(this.groupId).subscribe({
      next: (group) => {
        this.teamGroup = group;
        this.loading = false;
        this.groupMembers = group.members;
        // Load members after group details are loaded
       // this.loadGroupMembers();
      },
      error: (err) => {
        this.error = err.error?.message || err.message || 'Failed to load team group details';
        this.loading = false;
        console.error('Error loading team group:', err);
      }
    });
  }

  loadGroupMembers(): void {
    this.loadingMembers = true;
    this.teamService.getTeamGroupMembers(this.groupId).subscribe({
      next: (data) => {
        this.groupMembers = data.members;
        this.loadingMembers = false;
      },
      error: (err) => {
        console.error('Error loading group members:', err);
        this.loadingMembers = false;
        // Don't show error, just keep members empty
      }
    });
  }

  goBack(): void {
    this.router.navigate(['/teams', this.teamId]);
  }

  goToTeam(): void {
    this.router.navigate(['/teams', this.teamId]);
  }

  editGroup(): void {
    if (!this.canEdit) {
      return;
    }
    
    this.router.navigate(['/teams', this.teamId, 'groups', this.groupId, 'edit']);
  }

  addMembers(): void {
    if (!this.canManageMembers) {
      return;
    }
    
    this.router.navigate(['/teams', this.teamId, 'groups', this.groupId, 'add-members']);
  }

  navigateToAssignLeader(): void {
    if (!this.canManageMembers) {
      return;
    }
    
    this.router.navigate(['/teams', this.teamId, 'groups', this.groupId, 'assign-leader']);
  }

  formatDate(date: Date | string): string {
    if (!date) return '-';
    const d = new Date(date);
    if (isNaN(d.getTime())) return '-';
    return d.toLocaleDateString('en-US', { 
      year: 'numeric', 
      month: 'short', 
      day: 'numeric',
      hour: '2-digit',
      minute: '2-digit'
    });
  }
}

