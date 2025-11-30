import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { HeaderComponent } from '../../../shared/components/header/header.component';
import { SidebarComponent } from '../../../shared/components/sidebar/sidebar.component';
import { ReportsService, TeamProductivityData } from '../../../core/services/reports.service';
import { ProjectService } from '../../../core/services/project.service';
import { Project } from '../../../core/models/project.model';
import * as XLSX from 'xlsx';
import { catchError, of } from 'rxjs';

interface TeamProductivity {
  teamName: string;
  totalActivities: number;
  completedActivities: number;
  inProgress: number;
  averageCompletionTime: number;
  efficiency: number;
}

@Component({
  selector: 'app-team-productivity-report',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterModule, HeaderComponent, SidebarComponent],
  templateUrl: './team-productivity-report.component.html',
  styleUrls: ['./team-productivity-report.component.scss']
})
export class TeamProductivityReportComponent implements OnInit {
  teams: TeamProductivity[] = [];
  projects: Project[] = [];
  selectedProject = '';
  loading = false;
  error = '';

  constructor(
    private reportsService: ReportsService,
    private projectService: ProjectService
  ) {}

  ngOnInit(): void {
    this.loadProjects();
    this.loadReportData();
  }

  loadProjects(): void {
    this.projectService.getProjects().subscribe({
      next: (projects) => {
        this.projects = projects;
      },
      error: (err) => {
        console.error('Failed to load projects:', err);
      }
    });
  }

  loadReportData(): void {
    this.loading = true;
    this.error = '';

    this.reportsService.getTeamProductivityReport(this.selectedProject || undefined)
      .pipe(
        catchError(err => {
          console.error('Failed to load team productivity data:', err);
          this.error = err?.error?.message || err?.message || 'Failed to load team productivity data';
          // Load mock data as fallback
          this.loadMockData();
          return of([]);
        })
      )
      .subscribe({
        next: (data) => {
          if (data && data.length > 0) {
            this.teams = data.map(team => ({
              teamName: team.teamName,
              totalActivities: team.totalActivities,
              completedActivities: team.completedActivities,
              inProgress: team.inProgress,
              averageCompletionTime: team.averageCompletionTime,
              efficiency: team.efficiency
            }));
          }
          this.loading = false;
        },
        error: (err) => {
          console.error('Failed to load team productivity data:', err);
          this.loadMockData();
          this.loading = false;
        }
      });
  }

  onProjectChange(): void {
    this.loadReportData();
  }

  loadMockData(): void {
    console.log('📊 Loading mock data as fallback');
    this.teams = [
      {
        teamName: 'Civil Team A',
        totalActivities: 45,
        completedActivities: 38,
        inProgress: 7,
        averageCompletionTime: 3.5,
        efficiency: 84
      },
      {
        teamName: 'MEP Team B',
        totalActivities: 52,
        completedActivities: 41,
        inProgress: 11,
        averageCompletionTime: 4.2,
        efficiency: 79
      },
      {
        teamName: 'Civil Team C',
        totalActivities: 38,
        completedActivities: 35,
        inProgress: 3,
        averageCompletionTime: 3.1,
        efficiency: 92
      },
      {
        teamName: 'MEP Team D',
        totalActivities: 48,
        completedActivities: 39,
        inProgress: 9,
        averageCompletionTime: 3.8,
        efficiency: 81
      }
    ];
  }

  getEfficiencyColor(efficiency: number): string {
    if (efficiency >= 85) return 'green';
    if (efficiency >= 70) return 'blue';
    if (efficiency >= 50) return 'orange';
    return 'red';
  }

  exportToExcel(): void {
    if (!this.teams || this.teams.length === 0) {
      alert('No data to export');
      return;
    }

    const exportData = this.teams.map(team => ({
      'Team Name': team.teamName,
      'Total Activities': team.totalActivities,
      'Completed Activities': team.completedActivities,
      'In Progress': team.inProgress,
      'Avg Completion Time (days)': team.averageCompletionTime,
      'Efficiency %': team.efficiency
    }));

    const worksheet: XLSX.WorkSheet = XLSX.utils.json_to_sheet(exportData);
    const workbook: XLSX.WorkBook = {
      Sheets: { 'Team Productivity': worksheet },
      SheetNames: ['Team Productivity']
    };

    const today = new Date();
    const dateStr = today.toISOString().split('T')[0];
    XLSX.writeFile(workbook, `Team_Productivity_Report_${dateStr}.xlsx`);
  }
}
