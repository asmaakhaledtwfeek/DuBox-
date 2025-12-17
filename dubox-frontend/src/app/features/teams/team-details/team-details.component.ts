import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router, RouterModule, ActivatedRoute } from '@angular/router';
import { TeamService } from '../../../core/services/team.service';
import { Team, TeamMembersDto, TeamMember, CompleteTeamMemberProfile, CreateTeamGroup, TeamGroup, PaginatedTeamGroupsResponse } from '../../../core/models/team.model';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule, FormsModule } from '@angular/forms';
import { HeaderComponent } from '../../../shared/components/header/header.component';
import { SidebarComponent } from '../../../shared/components/sidebar/sidebar.component';
import { PermissionService } from '../../../core/services/permission.service';

@Component({
  selector: 'app-team-details',
  standalone: true,
  imports: [CommonModule, RouterModule, ReactiveFormsModule, FormsModule, HeaderComponent, SidebarComponent],
  templateUrl: './team-details.component.html',
  styleUrls: ['./team-details.component.scss']
})
export class TeamDetailsComponent implements OnInit {
  team: Team | null = null;
  teamMembers: TeamMembersDto | null = null;
  teamGroups: TeamGroup[] = [];
  loading = true;
  loadingGroups = false;
  error = '';
  teamId = '';
  activeTab: 'overview' | 'members' = 'overview';
  
  // Team Groups Pagination
  teamGroupsSearchTerm = '';
  teamGroupsCurrentPage = 1;
  teamGroupsPageSize = 10;
  teamGroupsTotalCount = 0;
  teamGroupsTotalPages = 0;
  
  showCompleteProfileModal = false;
  selectedMember: TeamMember | null = null;
  completeProfileForm!: FormGroup;
  completingProfile = false;
  completeProfileError = '';
  completeProfileSuccess = '';
  
  showRemoveConfirmModal = false;
  memberToRemove: TeamMember | null = null;
  removingMember = false;
  removeMemberError = '';
  
  // Create Team Group Modal
  showCreateGroupModal = false;
  teamGroupForm!: FormGroup;
  loadingGroup = false;
  groupError = '';
  groupSuccessMessage = '';
  
  // Permission flags
  canManageMembers = false;
  canEditTeam = false;
  canCreate = false;

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private teamService: TeamService,
    private fb: FormBuilder,
    private permissionService: PermissionService
  ) {}

  ngOnInit(): void {
    this.teamId = this.route.snapshot.paramMap.get('id') || '';
    
    // Check permissions
    this.canManageMembers = this.permissionService.hasPermission('teams', 'manage-members') || 
                            this.permissionService.hasPermission('teams', 'manage');
    this.canEditTeam = this.permissionService.hasPermission('teams', 'edit');
    this.canCreate = this.permissionService.canCreate('teams');
    
    this.completeProfileForm = this.fb.group({
      employeeCode: ['', [Validators.required, Validators.maxLength(50)]],
      employeeName: ['', [Validators.required, Validators.maxLength(200)]],
      mobileNumber: ['', [Validators.maxLength(20)]]
    });

    this.teamGroupForm = this.fb.group({
      groupTag: ['', [Validators.required, Validators.maxLength(50)]],
      groupType: ['', [Validators.required, Validators.maxLength(100)]]
    });
    
    if (this.teamId) {
      this.loadTeamDetails();
      this.loadTeamMembers();
      this.loadTeamGroups();
    }
  }

  loadTeamDetails(): void {
    this.teamService.getTeamById(this.teamId).subscribe({
      next: (team) => {
        this.team = team;
        this.loading = false;
      },
      error: (err) => {
        this.error = err.error?.message || err.message || 'Failed to load team details';
        this.loading = false;
        console.error('Error loading team:', err);
      }
    });
  }

  loadTeamMembers(): void {
    this.teamService.getTeamMembers(this.teamId).subscribe({
      next: (members) => {
        this.teamMembers = members;
      },
      error: (err) => {
        console.error('Error loading team members:', err);
      }
    });
  }

  loadTeamGroups(): void {
    this.loadingGroups = true;
    
    const params: any = {
      page: this.teamGroupsCurrentPage,
      pageSize: this.teamGroupsPageSize,
      teamId: this.teamId
    };

    // Apply search filter
    const searchTerm = this.teamGroupsSearchTerm.trim();
    if (searchTerm) {
      params.search = searchTerm;
    }

    this.teamService.getTeamGroupsPaginated(params).subscribe({
      next: (response: PaginatedTeamGroupsResponse) => {
        this.teamGroups = response.items;
        this.teamGroupsTotalCount = response.totalCount;
        this.teamGroupsTotalPages = response.totalPages;
        this.teamGroupsCurrentPage = response.page;
        this.loadingGroups = false;
      },
      error: (err) => {
        console.error('Error loading team groups:', err);
        this.loadingGroups = false;
      }
    });
  }

  onTeamGroupsSearch(): void {
    this.teamGroupsCurrentPage = 1; // Reset to first page when searching
    this.loadTeamGroups();
  }

  onTeamGroupsPageChange(page: number): void {
    this.teamGroupsCurrentPage = page;
    this.loadTeamGroups();
  }

  onTeamGroupsPageSizeChange(pageSize: number): void {
    this.teamGroupsPageSize = pageSize;
    this.teamGroupsCurrentPage = 1; // Reset to first page when changing page size
    this.loadTeamGroups();
  }

  clearGroupsSearch(): void {
    this.teamGroupsSearchTerm = '';
    this.teamGroupsCurrentPage = 1;
    this.loadTeamGroups();
  }

  goBack(): void {
    this.router.navigate(['/teams']);
  }

  editTeam(): void {
    this.router.navigate(['/teams', this.teamId, 'edit']);
  }

  addMembers(): void {
    this.router.navigate(['/teams', this.teamId, 'add-members']);
  }

  setActiveTab(tab: 'overview' | 'members'): void {
    this.activeTab = tab;
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

  openCompleteProfileModal(member: TeamMember): void {
    this.selectedMember = member;
    this.completeProfileForm.patchValue({
      employeeCode: member.employeeCode || '',
      employeeName: member.employeeName || member.fullName || '',
      mobileNumber: member.mobileNumber || ''
    });
    this.completeProfileError = '';
    this.completeProfileSuccess = '';
    this.showCompleteProfileModal = true;
  }

  closeCompleteProfileModal(): void {
    this.showCompleteProfileModal = false;
    this.selectedMember = null;
    this.completeProfileForm.reset();
    this.completeProfileError = '';
    this.completeProfileSuccess = '';
  }

  onCompleteProfile(): void {
    if (this.completeProfileForm.invalid || !this.selectedMember) {
      this.markFormGroupTouched(this.completeProfileForm);
      return;
    }

    this.completingProfile = true;
    this.completeProfileError = '';
    this.completeProfileSuccess = '';

    const formValue = this.completeProfileForm.value;
    const profileData: CompleteTeamMemberProfile = {
      teamMemberId: this.selectedMember.teamMemberId,
      employeeCode: formValue.employeeCode.trim(),
      employeeName: formValue.employeeName.trim(),
      mobileNumber: formValue.mobileNumber?.trim() || undefined
    };

    this.teamService.completeMemberProfile(profileData).subscribe({
      next: () => {
        this.completingProfile = false;
        this.completeProfileSuccess = 'Member profile completed successfully!';
        // Reload team members to show updated data
        this.loadTeamMembers();
        setTimeout(() => {
          this.closeCompleteProfileModal();
        }, 1500);
      },
      error: (err) => {
        this.completingProfile = false;
        this.completeProfileError = err.error?.message || err.message || 'Failed to complete member profile. Please try again.';
        console.error('Error completing profile:', err);
      }
    });
  }

  needsProfileCompletion(member: TeamMember): boolean {
    return !member.employeeCode || !member.employeeName || !member.mobileNumber;
  }

  openRemoveMemberModal(member: TeamMember): void {
    // Permission check: Can manage team members
    if (!this.canManageMembers) {
      this.removeMemberError = 'You do not have permission to remove team members.';
      return;
    }
    
    this.memberToRemove = member;
    this.removeMemberError = '';
    this.showRemoveConfirmModal = true;
  }

  closeRemoveMemberModal(): void {
    this.showRemoveConfirmModal = false;
    this.memberToRemove = null;
    this.removeMemberError = '';
  }

  confirmRemoveMember(): void {
    if (!this.memberToRemove || !this.teamId) {
      return;
    }

    // Permission check: Can manage team members
    if (!this.canManageMembers) {
      this.removeMemberError = 'You do not have permission to remove team members.';
      return;
    }

    this.removingMember = true;
    this.removeMemberError = '';

    this.teamService.removeTeamMember(this.teamId, this.memberToRemove.teamMemberId).subscribe({
      next: () => {
        this.removingMember = false;
        this.closeRemoveMemberModal();
        // Reload team members to reflect the change
        this.loadTeamMembers();
        this.loadTeamDetails(); // Reload team to update team size
      },
      error: (err) => {
        this.removingMember = false;
        this.removeMemberError = err.error?.message || err.message || 'Failed to remove team member. Please try again.';
        console.error('Error removing team member:', err);
      }
    });
  }

  // Create Team Group Methods
  openCreateGroupModal(): void {
    this.showCreateGroupModal = true;
    this.groupError = '';
    this.groupSuccessMessage = '';
    this.teamGroupForm.reset();
  }

  closeCreateGroupModal(): void {
    this.showCreateGroupModal = false;
    this.teamGroupForm.reset();
    this.groupError = '';
    this.groupSuccessMessage = '';
  }

  onCreateGroupSubmit(): void {
    if (this.teamGroupForm.invalid) {
      this.markFormGroupTouched(this.teamGroupForm);
      return;
    }

    this.loadingGroup = true;
    this.groupError = '';
    this.groupSuccessMessage = '';

    const formValue = this.teamGroupForm.value;
    const teamGroupData: CreateTeamGroup = {
      teamId: this.teamId,
      groupTag: formValue.groupTag.trim(),
      groupType: formValue.groupType.trim()
    };

    this.teamService.createTeamGroup(teamGroupData).subscribe({
      next: (teamGroup) => {
        this.loadingGroup = false;
        this.groupSuccessMessage = `Team group created successfully!`;
        setTimeout(() => {
          this.closeCreateGroupModal();
          // Reload team groups to show the new one
          this.loadTeamGroups();
        }, 1500);
      },
      error: (err) => {
        this.loadingGroup = false;
        this.groupError = err.error?.message || err.error?.detail || err.message || 'Failed to create team group. Please try again.';
        console.error('Error creating team group:', err);
      }
    });
  }

  private markFormGroupTouched(formGroup: FormGroup): void {
    Object.keys(formGroup.controls).forEach(key => {
      const control = formGroup.get(key);
      control?.markAsTouched();
    });
  }

  // Expose Math to template
  Math = Math;
}

