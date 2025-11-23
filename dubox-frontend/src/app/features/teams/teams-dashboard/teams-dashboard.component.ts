import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router, RouterModule } from '@angular/router';
import { FormControl, ReactiveFormsModule, FormsModule } from '@angular/forms';
import { debounceTime, distinctUntilChanged } from 'rxjs/operators';
import { TeamService } from '../../../core/services/team.service';
import { PermissionService } from '../../../core/services/permission.service';
import { Team } from '../../../core/models/team.model';
import { HeaderComponent } from '../../../shared/components/header/header.component';
import { SidebarComponent } from '../../../shared/components/sidebar/sidebar.component';

@Component({
  selector: 'app-teams-dashboard',
  standalone: true,
  imports: [CommonModule, RouterModule, ReactiveFormsModule, FormsModule, HeaderComponent, SidebarComponent],
  templateUrl: './teams-dashboard.component.html',
  styleUrls: ['./teams-dashboard.component.scss']
})
export class TeamsDashboardComponent implements OnInit {
  teams: Team[] = [];
  filteredTeams: Team[] = [];
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

  constructor(
    private router: Router,
    private teamService: TeamService,
    private permissionService: PermissionService
  ) {}

  ngOnInit(): void {
    this.canCreate = this.permissionService.canCreate('teams');
    this.canEdit = this.permissionService.canEdit('teams');
    
    this.loadTeams();
    this.setupSearch();
  }

  private setupSearch(): void {
    this.searchControl.valueChanges
      .pipe(
        debounceTime(300),
        distinctUntilChanged()
      )
      .subscribe(searchTerm => {
        this.applyFilters();
      });
  }

  loadTeams(): void {
    this.loading = true;
    this.error = '';
    
    this.teamService.getTeams().subscribe({
      next: (teams) => {
        console.log('📦 Teams received from service:', teams);
        console.log('📦 First team:', teams[0]);
        console.log('📦 First team departmentName:', teams[0]?.departmentName);
        this.teams = teams;
        this.extractDepartments();
        this.extractTrades();
        this.calculateStats();
        this.applyFilters();
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

  applyFilters(): void {
    let filtered = [...this.teams];
    const searchTerm = (this.searchControl.value || '').toLowerCase();

    // Search filter
    if (searchTerm) {
      filtered = filtered.filter(team =>
        team.teamCode.toLowerCase().includes(searchTerm) ||
        team.teamName.toLowerCase().includes(searchTerm) ||
        team.departmentName.toLowerCase().includes(searchTerm) ||
        (team.trade && team.trade.toLowerCase().includes(searchTerm)) ||
        (team.teamLeaderName && team.teamLeaderName.toLowerCase().includes(searchTerm))
      );
    }

    // Department filter
    if (this.selectedDepartment !== 'All') {
      filtered = filtered.filter(team => team.departmentName === this.selectedDepartment);
    }

    // Trade filter
    if (this.selectedTrade !== 'All') {
      filtered = filtered.filter(team => team.trade === this.selectedTrade);
    }

    // Active filter
    if (this.showActiveOnly) {
      filtered = filtered.filter(team => team.isActive);
    }

    this.filteredTeams = filtered;
  }

  onDepartmentChange(): void {
    this.applyFilters();
  }

  onTradeChange(): void {
    this.applyFilters();
  }

  onActiveToggle(): void {
    this.applyFilters();
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
}

