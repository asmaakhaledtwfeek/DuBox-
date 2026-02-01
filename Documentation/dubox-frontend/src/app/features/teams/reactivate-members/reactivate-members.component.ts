import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router, RouterModule, ActivatedRoute } from '@angular/router';
import { TeamService } from '../../../core/services/team.service';
import { Team, TeamMembersDto, TeamMember } from '../../../core/models/team.model';
import { HeaderComponent } from '../../../shared/components/header/header.component';
import { SidebarComponent } from '../../../shared/components/sidebar/sidebar.component';

@Component({
  selector: 'app-reactivate-members',
  standalone: true,
  imports: [CommonModule, RouterModule, HeaderComponent, SidebarComponent],
  templateUrl: './reactivate-members.component.html',
  styleUrls: ['./reactivate-members.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class ReactivateMembersComponent implements OnInit {
  team: Team | null = null;
  inactiveTeamMembers: TeamMembersDto | null = null;
  teamId: string = '';
  loading = true;
  reactivatingMember = false;
  reactivatingMemberId: string | null = null;
  error = '';
  successMessage = '';

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private teamService: TeamService
  ) {}

  ngOnInit(): void {
    this.teamId = this.route.snapshot.params['id'];
    this.loadTeam();
    this.loadInactiveTeamMembers();
  }

  loadTeam(): void {
    this.teamService.getTeamById(this.teamId).subscribe({
      next: (team) => {
        this.team = team;
      },
      error: (err) => {
        this.error = err.error?.message || err.message || 'Failed to load crew';
        this.loading = false;
        console.error('Error loading team:', err);
      }
    });
  }

  loadInactiveTeamMembers(): void {
    this.loading = true;
    this.teamService.getInactiveTeamMembers(this.teamId).subscribe({
      next: (members) => {
        this.inactiveTeamMembers = members;
        this.loading = false;
      },
      error: (err) => {
        this.loading = false;
        this.error = err.error?.message || err.message || 'Failed to load inactive members';
        console.error('Error loading inactive members:', err);
      }
    });
  }

  reactivateMember(member: TeamMember): void {
    if (this.reactivatingMember) return;

    this.reactivatingMember = true;
    this.reactivatingMemberId = member.teamMemberId;
    this.error = '';
    this.successMessage = '';

    this.teamService.reactivateTeamMember(this.teamId, member.teamMemberId).subscribe({
      next: () => {
        this.reactivatingMember = false;
        this.reactivatingMemberId = null;
        this.successMessage = `Successfully reactivated ${member.employeeName || member.fullName || member.email}`;
        
        // Reload inactive members
        this.loadInactiveTeamMembers();
        
        setTimeout(() => {
          this.successMessage = '';
        }, 3000);
      },
      error: (err) => {
        this.reactivatingMember = false;
        this.reactivatingMemberId = null;
        this.error = err.error?.message || err.message || 'Failed to reactivate member. Please try again.';
        console.error('Error reactivating member:', err);
      }
    });
  }

  onCancel(): void {
    this.router.navigate(['/teams', this.teamId]);
  }

  getUserInitials(name: string): string {
    if (!name) return '?';
    const parts = name.trim().split(/\s+/);
    if (parts.length >= 2) {
      return (parts[0][0] + parts[parts.length - 1][0]).toUpperCase();
    }
    return name.substring(0, 2).toUpperCase();
  }
}

