import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router, RouterModule, ActivatedRoute } from '@angular/router';
import { TeamService } from '../../../core/services/team.service';
import { Team, TeamMembersDto, TeamMember, CompleteTeamMemberProfile } from '../../../core/models/team.model';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { HeaderComponent } from '../../../shared/components/header/header.component';
import { SidebarComponent } from '../../../shared/components/sidebar/sidebar.component';
import { PermissionService } from '../../../core/services/permission.service';

@Component({
  selector: 'app-team-details',
  standalone: true,
  imports: [CommonModule, RouterModule, ReactiveFormsModule, HeaderComponent, SidebarComponent],
  templateUrl: './team-details.component.html',
  styleUrls: ['./team-details.component.scss']
})
export class TeamDetailsComponent implements OnInit {
  team: Team | null = null;
  teamMembers: TeamMembersDto | null = null;
  loading = true;
  error = '';
  teamId = '';
  activeTab: 'overview' | 'members' = 'overview';
  
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
  canManageMembers = false;

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private teamService: TeamService,
    private fb: FormBuilder,
    private permissionService: PermissionService
  ) {}

  ngOnInit(): void {
    this.teamId = this.route.snapshot.paramMap.get('id') || '';
    
    // Check permission to manage team members
    this.canManageMembers = this.permissionService.hasPermission('teams', 'manage-members') || 
                            this.permissionService.hasPermission('teams', 'manage');
    
    this.completeProfileForm = this.fb.group({
      employeeCode: ['', [Validators.required, Validators.maxLength(50)]],
      employeeName: ['', [Validators.required, Validators.maxLength(200)]],
      mobileNumber: ['', [Validators.maxLength(20)]]
    });
    
    if (this.teamId) {
      this.loadTeamDetails();
      this.loadTeamMembers();
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

  private markFormGroupTouched(formGroup: FormGroup): void {
    Object.keys(formGroup.controls).forEach(key => {
      const control = formGroup.get(key);
      control?.markAsTouched();
    });
  }
}

