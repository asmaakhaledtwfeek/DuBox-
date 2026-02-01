import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router, RouterModule, ActivatedRoute } from '@angular/router';
import { TeamService } from '../../../core/services/team.service';
import { TeamGroup, TeamMember, TeamMembersDto } from '../../../core/models/team.model';
import { HeaderComponent } from '../../../shared/components/header/header.component';
import { SidebarComponent } from '../../../shared/components/sidebar/sidebar.component';

@Component({
  selector: 'app-assign-group-leader',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterModule, HeaderComponent, SidebarComponent],
  templateUrl: './assign-group-leader.component.html',
  styleUrls: ['./assign-group-leader.component.scss']
})
export class AssignGroupLeaderComponent implements OnInit {
  teamGroup: TeamGroup | null = null;
  teamMembers: TeamMember[] = [];
  filteredMembers: TeamMember[] = [];
  loading = true;
  loadingMembers = false;
  error = '';
  teamId = '';
  groupId = '';
  
  searchTerm = '';
  selectedMemberId: string = '';
  assigningLeader = false;
  assignLeaderError = '';
  assignLeaderSuccess = '';

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private teamService: TeamService
  ) {}

  ngOnInit(): void {
    this.teamId = this.route.snapshot.paramMap.get('teamId') || '';
    this.groupId = this.route.snapshot.paramMap.get('groupId') || '';

    if (this.groupId) {
      this.loadGroupDetails();
      this.loadTeamMembers();
    }
  }

  loadGroupDetails(): void {
    this.loading = true;
    this.teamService.getTeamGroupById(this.groupId).subscribe({
      next: (group) => {
        this.teamGroup = group;
        this.selectedMemberId = group.groupLeaderId || '';
        this.loading = false;
      },
      error: (err) => {
        this.error = err.error?.message || err.message || 'Failed to load team group details';
        this.loading = false;
        console.error('Error loading team group:', err);
      }
    });
  }

  loadTeamMembers(): void {
    if (!this.teamId) return;
    
    this.loadingMembers = true;
    this.teamService.getTeamMembers(this.teamId).subscribe({
      next: (data: TeamMembersDto) => {
        this.teamMembers = data.members.filter(m => m.isActive);
        this.filteredMembers = [...this.teamMembers];
        this.loadingMembers = false;
      },
      error: (err) => {
        console.error('Error loading team members:', err);
        this.error = 'Failed to load team members';
        this.loadingMembers = false;
      }
    });
  }

  onSearch(): void {
    const search = this.searchTerm.toLowerCase().trim();
    
    if (!search) {
      this.filteredMembers = [...this.teamMembers];
      return;
    }

    this.filteredMembers = this.teamMembers.filter(member => {
      const name = (member.employeeName || member.fullName || '').toLowerCase();
      const code = (member.employeeCode || '').toLowerCase();
      const email = (member.email || '').toLowerCase();
      
      return name.includes(search) || 
             code.includes(search) || 
             email.includes(search);
    });
  }

  onAssignLeader(): void {
    if (!this.selectedMemberId || !this.groupId) {
      this.assignLeaderError = 'Please select a crew member';
      return;
    }

    this.assigningLeader = true;
    this.assignLeaderError = '';
    this.assignLeaderSuccess = '';

    this.teamService.assignGroupLeader(this.groupId, this.selectedMemberId).subscribe({
      next: (updatedGroup) => {
        this.assigningLeader = false;
        this.assignLeaderSuccess = 'Group leader assigned successfully!';
        
        setTimeout(() => {
          this.goBack();
        }, 1500);
      },
      error: (err) => {
        this.assigningLeader = false;
        this.assignLeaderError = err.error?.message || err.message || 'Failed to assign group leader. Please try again.';
        console.error('Error assigning leader:', err);
      }
    });
  }

  goBack(): void {
    this.router.navigate(['/teams', this.teamId, 'groups', this.groupId]);
  }

  clearSearch(): void {
    this.searchTerm = '';
    this.onSearch();
  }
}

