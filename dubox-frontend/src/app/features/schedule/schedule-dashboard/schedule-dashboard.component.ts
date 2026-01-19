import { Component, OnInit, HostListener } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../../../environments/environment';
import { HeaderComponent } from '../../../shared/components/header/header.component';
import { SidebarComponent } from '../../../shared/components/sidebar/sidebar.component';
import { TeamService } from '../../../core/services/team.service';
import { Team, TeamMember } from '../../../core/models/team.model';

interface ScheduleActivity {
  scheduleActivityId: string;
  activityName: string;
  activityCode: string;
  plannedStartDate: string;
  plannedFinishDate: string;
  status: string;
  percentComplete: number;
  teamCount: number;
  materialCount: number;
}

interface CreateActivityRequest {
  activityName: string;
  activityCode: string;
  description?: string;
  plannedStartDate: string;
  plannedFinishDate: string;
  projectId?: string;
}

interface Project {
  projectId: string;
  projectCode: string;
  projectName: string;
  clientName?: string;
  location?: string;
  status: string;
}

@Component({
  selector: 'app-schedule-dashboard',
  standalone: true,
  imports: [CommonModule, RouterModule, FormsModule, HeaderComponent, SidebarComponent],
  templateUrl: './schedule-dashboard.component.html',
  styleUrl: './schedule-dashboard.component.scss'
})
export class ScheduleDashboardComponent implements OnInit {
  activities: ScheduleActivity[] = [];
  teams: Team[] = [];
  teamMembers: TeamMember[] = [];
  loading = false;
  loadingTeams = false;
  loadingMembers = false;
  error: string | null = null;
  
  // Project Selection
  projects: Project[] = [];
  selectedProject: Project | null = null;
  projectsLoading = false;
  
  // View Options
  currentView: 'card' | 'list' | 'detailed' = 'card';
  showViewDropdown = false;
  
  // Create Activity Form
  showCreateForm = false;
  createActivityError = '';
  creatingActivity = false;
  newActivity: CreateActivityRequest = {
    activityName: '',
    activityCode: '',
    description: '',
    plannedStartDate: '',
    plannedFinishDate: '',
    projectId: ''
  };

  // Assign Team Modal
  showAssignTeamModal = false;
  selectedActivityId: string | null = null;
  selectedTeamId: string = '';
  selectedMemberId: string = '';
  assignTeamNotes: string = '';
  assignTeamError: string = '';
  assigningTeam = false;

  // Success Modal
  showSuccessModal = false;
  successMessage = '';

  // Assign Material Modal
  showAssignMaterialModal = false;
  assignMaterialError = '';
  assigningMaterial = false;
  newMaterial = {
    materialName: '',
    materialCode: '',
    quantity: 0,
    unit: '',
    notes: ''
  };

  constructor(
    private http: HttpClient,
    private teamService: TeamService
  ) {}

  ngOnInit(): void {
    this.loadProjects();
    this.loadTeams();
  }

  loadProjects(): void {
    this.projectsLoading = true;
    
    this.http.get<any>(`${environment.apiUrl}/projects`)
      .subscribe({
        next: (response) => {
          this.projects = response.data || response || [];
          this.projectsLoading = false;
        },
        error: (err) => {
          console.error('Error loading projects:', err);
          this.projectsLoading = false;
        }
      });
  }

  onProjectSelect(project: Project | null): void {
    this.selectedProject = project;
    if (this.selectedProject) {
      this.loadActivities();
    } else {
      this.activities = [];
    }
  }

  loadActivities(): void {
    if (!this.selectedProject) {
      this.activities = [];
      return;
    }

    this.loading = true;
    this.error = null;

    // Load activities filtered by project
    this.http.get<any>(`${environment.apiUrl}/schedule/activities/project/${this.selectedProject.projectId}`).subscribe({
      next: (response) => {
        this.activities = response.data || [];
        this.loading = false;
      },
      error: (err) => {
        this.error = 'Failed to load schedule activities';
        this.loading = false;
        console.error('Error loading activities:', err);
      }
    });
  }

  loadTeams(): void {
    this.loadingTeams = true;
    this.teamService.getTeams().subscribe({
      next: (teams) => {
        this.teams = teams.filter(team => team.isActive);
        this.loadingTeams = false;
      },
      error: (err) => {
        console.error('Error loading teams:', err);
        this.loadingTeams = false;
      }
    });
  }

  loadTeamMembers(teamId: string): void {
    if (!teamId) {
      this.teamMembers = [];
      return;
    }

    this.loadingMembers = true;
    this.teamService.getTeamMembers(teamId).subscribe({
      next: (response) => {
        // Filter to only include members that have a userId
        this.teamMembers = (response.members || []).filter(member => member.userId && member.userId.trim() !== '');
        this.loadingMembers = false;
      },
      error: (err) => {
        console.error('Error loading team members:', err);
        this.teamMembers = [];
        this.loadingMembers = false;
      }
    });
  }

  onTeamChange(): void {
    // Reset member selection when team changes
    this.selectedMemberId = '';
    this.teamMembers = [];
    
    // Load team members if a team is selected
    if (this.selectedTeamId) {
      this.loadTeamMembers(this.selectedTeamId);
    }
  }

  openCreateForm(): void {
    if (!this.selectedProject) {
      return;
    }
    
    this.showCreateForm = true;
    this.createActivityError = '';
    this.newActivity = {
      activityName: '',
      activityCode: '',
      description: '',
      plannedStartDate: '',
      plannedFinishDate: '',
      projectId: this.selectedProject.projectId
    };
  }

  closeCreateForm(): void {
    this.showCreateForm = false;
    this.createActivityError = '';
  }

  createActivity(): void {
    if (!this.newActivity.activityName || !this.newActivity.activityCode || 
        !this.newActivity.plannedStartDate || !this.newActivity.plannedFinishDate) {
      this.createActivityError = 'Please fill in all required fields';
      return;
    }

    this.createActivityError = '';
    this.creatingActivity = true;

    this.http.post<any>(`${environment.apiUrl}/schedule/activities`, this.newActivity).subscribe({
      next: (response) => {
        this.creatingActivity = false;
        this.closeCreateForm();
        this.successMessage = 'Activity created successfully!';
        this.showSuccessModal = true;
        this.loadActivities();
      },
      error: (err) => {
        this.creatingActivity = false;
        const errorMessage = err.error?.error?.message || err.error?.message || err.message || 'Unknown error occurred';
        this.createActivityError = `Failed to create activity: ${errorMessage}`;
        console.error('Error creating activity:', err);
      }
    });
  }

  openAssignTeamModal(activityId: string): void {
    this.selectedActivityId = activityId;
    this.showAssignTeamModal = true;
    this.selectedTeamId = '';
    this.selectedMemberId = '';
    this.teamMembers = [];
    this.assignTeamNotes = '';
    this.assignTeamError = '';
    
    // Load teams if not already loaded
    if (this.teams.length === 0) {
      this.loadTeams();
    }
  }

  closeAssignTeamModal(): void {
    this.showAssignTeamModal = false;
    this.selectedActivityId = null;
    this.assignTeamError = '';
  }

  closeSuccessModal(): void {
    this.showSuccessModal = false;
    this.successMessage = '';
  }

  assignTeam(): void {
    if (!this.selectedTeamId) {
      this.assignTeamError = 'Please select a team';
      return;
    }

    this.assignTeamError = '';
    this.assigningTeam = true;

    const request = {
      teamId: this.selectedTeamId,
      memberId: this.selectedMemberId || null,
      notes: this.assignTeamNotes
    };

    this.http.post<any>(
      `${environment.apiUrl}/schedule/activities/${this.selectedActivityId}/assign-team`,
      request
    ).subscribe({
      next: (response) => {
        this.assigningTeam = false;
        this.closeAssignTeamModal();
        this.successMessage = 'Team assigned successfully!';
        this.showSuccessModal = true;
        this.loadActivities();
      },
      error: (err) => {
        this.assigningTeam = false;
        const errorMessage = err.error?.error?.message || err.error?.message || err.message || 'Unknown error occurred';
        this.assignTeamError = `Failed to assign team: ${errorMessage}`;
        console.error('Error assigning team:', err);
      }
    });
  }

  openAssignMaterialModal(activityId: string): void {
    this.selectedActivityId = activityId;
    this.showAssignMaterialModal = true;
    this.assignMaterialError = '';
    this.newMaterial = {
      materialName: '',
      materialCode: '',
      quantity: 0,
      unit: '',
      notes: ''
    };
  }

  closeAssignMaterialModal(): void {
    this.showAssignMaterialModal = false;
    this.selectedActivityId = null;
    this.assignMaterialError = '';
  }

  assignMaterial(): void {
    if (!this.newMaterial.materialName || this.newMaterial.quantity <= 0) {
      this.assignMaterialError = 'Please fill in material name and quantity';
      return;
    }

    this.assignMaterialError = '';
    this.assigningMaterial = true;

    this.http.post<any>(
      `${environment.apiUrl}/schedule/activities/${this.selectedActivityId}/assign-material`,
      this.newMaterial
    ).subscribe({
      next: (response) => {
        this.assigningMaterial = false;
        this.closeAssignMaterialModal();
        this.successMessage = 'Material assigned successfully!';
        this.showSuccessModal = true;
        this.loadActivities();
      },
      error: (err) => {
        this.assigningMaterial = false;
        const errorMessage = err.error?.error?.message || err.error?.message || err.message || 'Unknown error occurred';
        this.assignMaterialError = `Failed to assign material: ${errorMessage}`;
        console.error('Error assigning material:', err);
      }
    });
  }

  getStatusClass(status: string): string {
    switch (status.toLowerCase()) {
      case 'completed':
        return 'status-completed';
      case 'in progress':
        return 'status-in-progress';
      case 'on hold':
        return 'status-on-hold';
      default:
        return 'status-planned';
    }
  }

  // View Management
  setView(view: 'card' | 'list' | 'detailed'): void {
    this.currentView = view;
    this.showViewDropdown = false;
  }

  toggleViewDropdown(event: Event): void {
    event.stopPropagation();
    this.showViewDropdown = !this.showViewDropdown;
  }

  closeViewDropdown(): void {
    this.showViewDropdown = false;
  }

  @HostListener('document:click', ['$event'])
  onDocumentClick(event: Event): void {
    // Close dropdown when clicking outside
    if (this.showViewDropdown) {
      this.showViewDropdown = false;
    }
  }
}

