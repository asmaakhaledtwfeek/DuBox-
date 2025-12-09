import { Component, OnInit, OnDestroy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router, RouterModule } from '@angular/router';
import { FormControl, ReactiveFormsModule, FormsModule } from '@angular/forms';
import { debounceTime, distinctUntilChanged, skip } from 'rxjs/operators';
import { Subscription } from 'rxjs';
import { TeamService } from '../../../core/services/team.service';
import { PermissionService } from '../../../core/services/permission.service';
import { ReportsService } from '../../../core/services/reports.service';
import { Team, PaginatedTeamsResponse } from '../../../core/models/team.model';
import { TeamActivitiesResponse, TeamActivityDetail } from '../../../core/models/teams-performance-report.model';
import { HeaderComponent } from '../../../shared/components/header/header.component';
import { SidebarComponent } from '../../../shared/components/sidebar/sidebar.component';

@Component({
  selector: 'app-teams-dashboard',
  standalone: true,
  imports: [CommonModule, RouterModule, ReactiveFormsModule, FormsModule, HeaderComponent, SidebarComponent],
  templateUrl: './teams-dashboard.component.html',
  styleUrls: ['./teams-dashboard.component.scss']
})
export class TeamsDashboardComponent implements OnInit, OnDestroy {
  teams: Team[] = [];
  paginatedTeams: Team[] = [];
  loading = true;
  error = '';
  canCreate = false;
  canEdit = false;
  
  searchControl = new FormControl('');
  selectedDepartment: string = 'All';
  selectedTrade: string = 'All';
  showActiveOnly = true;
  
  departments: string[] = [];
  trades: string[] = [];
  stats = {
    total: 0,
    active: 0,
    inactive: 0,
    totalMembers: 0
  };

  // Pagination
  currentPage = 1;
  pageSize = 25;
  totalCount = 0;
  totalPages = 0;

  // Team Activities Modal
  showActivitiesModal = false;
  selectedTeam: Team | null = null;
  teamActivities: TeamActivitiesResponse | null = null;
  loadingActivities = false;
  
  private subscriptions: Subscription[] = [];

  constructor(
    private router: Router,
    private teamService: TeamService,
    private permissionService: PermissionService,
    private reportsService: ReportsService
  ) {}

  ngOnInit(): void {
    // Check permissions immediately
    this.checkPermissions();
    
    // Subscribe to permission changes to update UI when permissions are loaded
    this.subscriptions.push(
      this.permissionService.permissions$
        .pipe(skip(1)) // Skip initial empty value
        .subscribe(() => {
          console.log('🔄 Permissions updated, re-checking teams permissions');
          this.checkPermissions();
        })
    );
    
    this.loadTeams();
    this.loadTeamsPaginated();
    this.setupSearch();
  }
  
  ngOnDestroy(): void {
    this.subscriptions.forEach(sub => sub.unsubscribe());
  }
  
  private checkPermissions(): void {
    this.canCreate = this.permissionService.canCreate('teams');
    this.canEdit = this.permissionService.canEdit('teams');
    console.log('✅ Teams permissions checked:', { canCreate: this.canCreate, canEdit: this.canEdit });
  }

  private setupSearch(): void {
    this.searchControl.valueChanges
      .pipe(
        debounceTime(300),
        distinctUntilChanged()
      )
      .subscribe(searchTerm => {
        this.currentPage = 1;
        this.loadTeamsPaginated();
      });
  }

  loadTeams(): void {
    // Load all teams for stats and filter options (departments, trades)
    this.teamService.getTeams().subscribe({
      next: (teams) => {
        this.teams = teams;
        this.extractDepartments();
        this.extractTrades();
        this.calculateStats();
      },
      error: (err) => {
        console.error('Error loading teams for stats:', err);
      }
    });
  }

  loadTeamsPaginated(): void {
    this.loading = true;
    this.error = '';

    const params: any = {
      page: this.currentPage,
      pageSize: this.pageSize
    };

    // Apply search filter
    const searchTerm = (this.searchControl.value || '').trim();
    if (searchTerm) {
      params.search = searchTerm;
    }

    // Apply department filter
    if (this.selectedDepartment !== 'All') {
      params.department = this.selectedDepartment;
    }

    // Apply trade filter
    if (this.selectedTrade !== 'All') {
      params.trade = this.selectedTrade;
    }

    // Apply active filter
    if (this.showActiveOnly) {
      params.isActive = true;
    }

    this.teamService.getTeamsPaginated(params).subscribe({
      next: (response) => {
        this.paginatedTeams = response.items;
        this.totalCount = response.totalCount;
        this.totalPages = response.totalPages;
        this.currentPage = response.page;
        this.loading = false;
      },
      error: (err) => {
        console.error('Error loading teams - Full error:', err);
        console.error('Error status:', err.status);
        console.error('Error statusText:', err.statusText);
        console.error('Error error:', err.error);
        console.error('Error message:', err.message);
        
        const errorMessage = err.error?.title || err.error?.detail || err.error?.message || err.message || 'Failed to load teams';
        this.error = `Error ${err.status || 'Unknown'}: ${errorMessage}`;
        this.loading = false;
      }
    });
  }

  private extractDepartments(): void {
    const deptSet = new Set<string>();
    this.teams.forEach(team => {
      if (team.departmentName) {
        deptSet.add(team.departmentName);
      }
    });
    this.departments = Array.from(deptSet).sort();
  }

  private extractTrades(): void {
    const tradeSet = new Set<string>();
    this.teams.forEach(team => {
      if (team.trade) {
        tradeSet.add(team.trade);
      }
    });
    this.trades = Array.from(tradeSet).sort();
  }

  private calculateStats(): void {
    this.stats.total = this.teams.length;
    this.stats.active = this.teams.filter(t => t.isActive).length;
    this.stats.inactive = this.teams.filter(t => !t.isActive).length;
    this.stats.totalMembers = this.teams.reduce((sum, team) => sum + (team.teamSize || 0), 0);
  }


  onDepartmentChange(): void {
    this.currentPage = 1;
    this.loadTeamsPaginated();
  }

  onTradeChange(): void {
    this.currentPage = 1;
    this.loadTeamsPaginated();
  }

  onActiveToggle(): void {
    this.currentPage = 1;
    this.loadTeamsPaginated();
  }

  onPageChange(page: number): void {
    if (page >= 1 && page <= this.totalPages) {
      this.currentPage = page;
      this.loadTeamsPaginated();
    }
  }

  onPageChangePage(page: number | string): void {
    if (typeof page === 'number') {
      this.onPageChange(page);
    }
  }

  onPageSizeChange(): void {
    this.currentPage = 1;
    this.loadTeamsPaginated();
  }

  getPageNumbers(): (number | string)[] {
    const pages: (number | string)[] = [];
    const maxVisible = 5;
    
    if (this.totalPages <= maxVisible) {
      for (let i = 1; i <= this.totalPages; i++) {
        pages.push(i);
      }
    } else {
      if (this.currentPage <= 3) {
        for (let i = 1; i <= 4; i++) {
          pages.push(i);
        }
        pages.push('...');
        pages.push(this.totalPages);
      } else if (this.currentPage >= this.totalPages - 2) {
        pages.push(1);
        pages.push('...');
        for (let i = this.totalPages - 3; i <= this.totalPages; i++) {
          pages.push(i);
        }
      } else {
        pages.push(1);
        pages.push('...');
        for (let i = this.currentPage - 1; i <= this.currentPage + 1; i++) {
          pages.push(i);
        }
        pages.push('...');
        pages.push(this.totalPages);
      }
    }
    
    return pages;
  }

  getStartIndex(): number {
    return this.totalCount === 0 ? 0 : (this.currentPage - 1) * this.pageSize + 1;
  }

  getEndIndex(): number {
    return Math.min(this.currentPage * this.pageSize, this.totalCount);
  }

  createTeam(): void {
    this.router.navigate(['/teams/create']);
  }

  viewDetails(teamId: string): void {
    this.router.navigate(['/teams', teamId]);
  }

  editTeam(teamId: string): void {
    this.router.navigate(['/teams', teamId, 'edit']);
  }

  viewTeamActivities(team: Team): void {
    this.selectedTeam = team;
    this.showActivitiesModal = true;
    this.loadingActivities = true;

    this.reportsService.getTeamActivities(team.teamId, {}).subscribe({
      next: (data) => {
        this.teamActivities = data;
        this.loadingActivities = false;
      },
      error: (err) => {
        console.error('Failed to load team activities:', err);
        this.loadingActivities = false;
        alert('Failed to load team activities. Please try again.');
      }
    });
  }

  closeActivitiesModal(): void {
    this.showActivitiesModal = false;
    this.selectedTeam = null;
    this.teamActivities = null;
  }

  navigateToActivityDetails(activity: TeamActivityDetail): void {
    if (!activity.activityId || !activity.boxId || !activity.projectId) {
      console.error('Missing required IDs for navigation');
      return;
    }
    
    this.closeActivitiesModal();
    this.router.navigate([
      '/projects',
      activity.projectId,
      'boxes',
      activity.boxId,
      'activities',
      activity.activityId
    ]);
  }

  getStatusLabel(status: string): string {
    return status || 'N/A';
  }

  getStatusClass(status: string): string {
    const statusLower = status.toLowerCase();
    if (statusLower.includes('completed')) return 'status-completed';
    if (statusLower.includes('progress')) return 'status-in-progress';
    if (statusLower.includes('delayed')) return 'status-delayed';
    if (statusLower.includes('hold')) return 'status-on-hold';
    return 'status-default';
  }

  formatProgress(progress: number): string {
    return `${progress.toFixed(1)}%`;
  }

  formatDate(dateString?: string): string {
    if (!dateString) return 'N/A';
    try {
      const date = new Date(dateString);
      return date.toLocaleDateString('en-US', { year: 'numeric', month: 'short', day: 'numeric' });
    } catch {
      return 'N/A';
    }
  }
}

