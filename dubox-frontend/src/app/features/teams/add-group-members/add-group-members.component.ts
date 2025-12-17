import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router, RouterModule, ActivatedRoute } from '@angular/router';
import { TeamService } from '../../../core/services/team.service';
import { TeamGroup, TeamMember, TeamMembersDto } from '../../../core/models/team.model';
import { HeaderComponent } from '../../../shared/components/header/header.component';
import { SidebarComponent } from '../../../shared/components/sidebar/sidebar.component';

@Component({
  selector: 'app-add-group-members',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterModule, HeaderComponent, SidebarComponent],
  templateUrl: './add-group-members.component.html',
  styleUrls: ['./add-group-members.component.scss']
})
export class AddGroupMembersComponent implements OnInit {
  teamGroup: TeamGroup | null = null;
  teamMembers: TeamMember[] = [];
  availableMembers: TeamMember[] = [];
  filteredMembers: TeamMember[] = [];
  selectedMemberIds: Set<string> = new Set();
  
  loading = true;
  loadingMembers = false;
  submitting = false;
  error = '';
  success = '';
  
  teamId = '';
  groupId = '';
  searchTerm = '';

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
        this.loading = false;
        // Pre-select current group members
        this.preselectCurrentMembers();
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
        // Filter to show members not in any group OR in the current group
        this.availableMembers = this.teamMembers.filter(m => 
          !m.teamGroupId || 
          m.teamGroupId === '' || 
          m.teamGroupId === this.groupId
        );
        this.filteredMembers = [...this.availableMembers];
        this.loadingMembers = false;
        // Pre-select current group members after loading
        this.preselectCurrentMembers();
      },
      error: (err) => {
        console.error('Error loading team members:', err);
        this.error = 'Failed to load team members';
        this.loadingMembers = false;
      }
    });
  }

  preselectCurrentMembers(): void {
    // Pre-select members who are already in the current group
    if (this.teamGroup && this.teamGroup.members && this.teamGroup.members.length > 0) {
      this.teamGroup.members.forEach(member => {
        this.selectedMemberIds.add(member.teamMemberId);
      });
    }
  }

  onSearch(): void {
    const search = this.searchTerm.toLowerCase().trim();
    
    if (!search) {
      this.filteredMembers = [...this.availableMembers];
      return;
    }

    this.filteredMembers = this.availableMembers.filter(member => {
      const name = (member.employeeName || member.fullName || '').toLowerCase();
      const code = (member.employeeCode || '').toLowerCase();
      const email = (member.email || '').toLowerCase();
      
      return name.includes(search) || 
             code.includes(search) || 
             email.includes(search);
    });
  }

  toggleMemberSelection(memberId: string): void {
    if (this.selectedMemberIds.has(memberId)) {
      this.selectedMemberIds.delete(memberId);
    } else {
      this.selectedMemberIds.add(memberId);
    }
  }

  selectAll(): void {
    this.filteredMembers.forEach(member => {
      this.selectedMemberIds.add(member.teamMemberId);
    });
  }

  deselectAll(): void {
    this.selectedMemberIds.clear();
  }

  isSelected(memberId: string): boolean {
    return this.selectedMemberIds.has(memberId);
  }

  isGroupLeader(memberId: string): boolean {
    return this.teamGroup?.groupLeaderId === memberId;
  }

  onAddMembers(): void {
    if (this.selectedMemberIds.size === 0) {
      this.error = 'Please select at least one member';
      return;
    }

    this.submitting = true;
    this.error = '';
    this.success = '';

    const memberIds = Array.from(this.selectedMemberIds);
    
    this.teamService.addMembersToGroup(this.groupId, memberIds).subscribe({
      next: (updatedGroup) => {
        this.submitting = false;
        this.success = `Successfully added ${memberIds.length} member(s) to the group!`;
        
        setTimeout(() => {
          this.goBack();
        }, 1500);
      },
      error: (err) => {
        this.submitting = false;
        this.error = err.error?.message || err.message || 'Failed to add members. Please try again.';
        console.error('Error adding members:', err);
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

