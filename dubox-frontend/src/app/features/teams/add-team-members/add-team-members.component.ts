import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router, RouterModule, ActivatedRoute } from '@angular/router';
import { FormBuilder, FormGroup, ReactiveFormsModule, FormsModule } from '@angular/forms';
import { TeamService } from '../../../core/services/team.service';
import { UserService, UserDto } from '../../../core/services/user.service';
import { Team, AssignTeamMembers, TeamMember } from '../../../core/models/team.model';
import { HeaderComponent } from '../../../shared/components/header/header.component';
import { SidebarComponent } from '../../../shared/components/sidebar/sidebar.component';

@Component({
  selector: 'app-add-team-members',
  standalone: true,
  imports: [CommonModule, RouterModule, ReactiveFormsModule, FormsModule, HeaderComponent, SidebarComponent],
  templateUrl: './add-team-members.component.html',
  styleUrls: ['./add-team-members.component.scss']
})
export class AddTeamMembersComponent implements OnInit {
  team: Team | null = null;
  allUsers: UserDto[] = [];
  availableUsers: UserDto[] = [];
  filteredUsers: UserDto[] = [];
  selectedUserIds: string[] = [];
  currentTeamMembers: TeamMember[] = [];
  teamId: string = '';
  loading = true;
  saving = false;
  error = '';
  successMessage = '';
  searchTerm: string = '';

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private teamService: TeamService,
    private userService: UserService,
    private fb: FormBuilder
  ) {}

  ngOnInit(): void {
    this.teamId = this.route.snapshot.params['id'];
    this.loadTeam();
    this.loadUsers();
    this.loadCurrentTeamMembers();
  }

  loadTeam(): void {
    this.teamService.getTeamById(this.teamId).subscribe({
      next: (team) => {
        this.team = team;
        this.filterUsersByDepartment();
        this.loading = false;
      },
      error: (err) => {
        this.error = err.error?.message || err.message || 'Failed to load crew';
        this.loading = false;
        console.error('Error loading team:', err);
      }
    });
  }

  loadUsers(): void {
    // Fetch a large page size to get all users for team member selection
    this.userService.getUsers(1, 1000).subscribe({
      next: (response) => {
        this.allUsers = response.items.filter((u: UserDto) => u.isActive);
        this.filterUsersByDepartment();
      },
      error: (err) => {
        console.error('Error loading users:', err);
        this.error = 'Failed to load users';
      }
    });
  }

  filterUsersByDepartment(): void {
    if (!this.team?.departmentId) {
      this.availableUsers = [];
      this.filteredUsers = [];
      return;
    }

    // Filter users by team's department
    this.availableUsers = this.allUsers.filter(user => 
      user.departmentId === this.team?.departmentId && 
      user.isActive
    );

    // Initialize filtered users with all available users
    this.filteredUsers = [...this.availableUsers];

    // Apply search filter if search term exists
    this.applySearchFilter();

    // Pre-select current team members
    this.preSelectCurrentMembers();
  }

  applySearchFilter(): void {
    if (!this.searchTerm || this.searchTerm.trim() === '') {
      this.filteredUsers = [...this.availableUsers];
      return;
    }

    const search = this.searchTerm.toLowerCase().trim();
    this.filteredUsers = this.availableUsers.filter(user => {
      const fullName = (user.fullName || '').toLowerCase();
      const email = (user.email || '').toLowerCase();
      return fullName.includes(search) || email.includes(search);
    });
  }

  onSearchChange(): void {
    this.applySearchFilter();
  }

  clearSearch(): void {
    this.searchTerm = '';
    this.applySearchFilter();
  }

  loadCurrentTeamMembers(): void {
    this.teamService.getTeamMembers(this.teamId).subscribe({
      next: (teamMembers) => {
        this.currentTeamMembers = teamMembers.members || [];
        // Pre-select current members after loading
        this.preSelectCurrentMembers();
      },
      error: (err) => {
        console.error('Error loading team members:', err);
      }
    });
  }

  preSelectCurrentMembers(): void {
    if (this.currentTeamMembers.length === 0 || this.availableUsers.length === 0) {
      return;
    }

    // Get user IDs of current team members
    const currentMemberUserIds = this.currentTeamMembers
      .map(member => member.userId)
      .filter(id => id);

    // Pre-select users who are already team members
    const existingMemberIds = this.availableUsers
      .filter(user => currentMemberUserIds.includes(user.userId))
      .map(user => user.userId);

    // Merge with already selected IDs (avoid duplicates)
    existingMemberIds.forEach(id => {
      if (!this.selectedUserIds.includes(id)) {
        this.selectedUserIds.push(id);
      }
    });
  }

  toggleUserSelection(userId: string): void {
    const index = this.selectedUserIds.indexOf(userId);
    if (index > -1) {
      this.selectedUserIds.splice(index, 1);
    } else {
      this.selectedUserIds.push(userId);
    }
  }

  isUserSelected(userId: string): boolean {
    return this.selectedUserIds.includes(userId);
  }

  onSubmit(): void {
    if (this.selectedUserIds.length === 0) {
      this.error = 'Please select at least one user to add to the crew';
      return;
    }

    this.saving = true;
    this.error = '';
    this.successMessage = '';

    const assignData: AssignTeamMembers = {
      teamId: this.teamId,
      userIds: this.selectedUserIds
    };

    this.teamService.assignTeamMembers(assignData).subscribe({
      next: () => {
        this.saving = false;
        this.successMessage = `Successfully added ${this.selectedUserIds.length} member(s) to the crew!`;
        setTimeout(() => {
          this.router.navigate(['/teams', this.teamId]);
        }, 1500);
      },
      error: (err) => {
        this.saving = false;
        this.error = err.error?.message || err.message || 'Failed to add members to crew. Please try again.';
        console.error('Error adding members:', err);
      }
    });
  }

  onCancel(): void {
    this.router.navigate(['/teams', this.teamId]);
  }

  getSelectedCount(): number {
    return this.selectedUserIds.length;
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

