import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Router } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { HeaderComponent } from '../../../shared/components/header/header.component';
import { SidebarComponent } from '../../../shared/components/sidebar/sidebar.component';
import { ReportsService, ReportSummary, TeamProductivityData } from '../../../core/services/reports.service';
import { ProjectService } from '../../../core/services/project.service';
import { Project } from '../../../core/models/project.model';
import * as XLSX from 'xlsx';

interface ReportCard {
  id: string;
  title: string;
  description: string;
  icon: string;
  color: string;
}

@Component({
  selector: 'app-reports-dashboard',
  standalone: true,
  imports: [CommonModule, RouterModule, FormsModule, HeaderComponent, SidebarComponent],
  templateUrl: './reports-dashboard.component.html',
  styleUrls: ['./reports-dashboard.component.scss']
})
export class ReportsDashboardComponent implements OnInit {
  loading = false;
  summary: ReportSummary = {
    totalBoxes: 0,
    averageProgress: 0,
    pendingActivities: 0,
    activeTeams: 0
  };

  // Active report tracking
  activeReport: string | null = null;
  teamProductivityData: TeamProductivityData[] = [];
  
  // Projects for filtering
  projects: Project[] = [];
  selectedProject = '';

  reports: ReportCard[] = [
    {
      id: 'projects-summary',
      title: 'Projects Summary Report',
      description: 'Aggregated information about all projects with KPIs, status distribution, and project details',
      icon: 'projects',
      color: 'indigo'
    },
    {
      id: 'boxes-summary',
      title: 'Boxes Summary Report',
      description: 'Comprehensive report with filtering, KPIs, charts, and detailed box information',
      icon: 'boxes',
      color: 'teal'
    },
    {
      id: 'team-productivity',
      title: 'Team Productivity Report',
      description: 'Monitor the productivity and performance of deployed teams',
      icon: 'team',
      color: 'green'
    }
  ];

  constructor(
    private reportsService: ReportsService,
    private projectService: ProjectService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.loadSummary();
    this.loadProjects();
  }

  loadSummary(): void {
    this.loading = true;
    this.reportsService.getReportSummary().subscribe({
      next: (data) => {
        this.summary = data;
        this.loading = false;
      },
      error: (err) => {
        console.error('Failed to load report summary:', err);
        this.loading = false;
      }
    });
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

  showReport(reportId: string): void {
    // Navigate to separate report pages instead of inline display
    if (reportId === 'projects-summary') {
      this.router.navigate(['/reports/projects']);
      return;
    }
    if (reportId === 'boxes-summary') {
      this.router.navigate(['/reports/boxes']);
      return;
    }
    
    this.activeReport = reportId;
    
    if (reportId === 'team-productivity') {
      this.loadTeamProductivityReport();
    }
  }

  closeReport(): void {
    this.activeReport = null;
    this.selectedProject = '';
  }

  onProjectChange(): void {
    if (this.activeReport === 'team-productivity') {
      this.loadTeamProductivityReport();
    }
  }

  loadTeamProductivityReport(): void {
    this.loading = true;
    this.reportsService.getTeamProductivityReport(this.selectedProject || undefined).subscribe({
      next: (data) => {
        this.teamProductivityData = data;
        this.loading = false;
      },
      error: (err) => {
        console.error('Failed to load team productivity:', err);
        this.loading = false;
      }
    });
  }

  // Team Productivity methods
  getEfficiencyColor(efficiency: number): string {
    if (efficiency >= 85) return 'green';
    if (efficiency >= 70) return 'blue';
    if (efficiency >= 50) return 'orange';
    return 'red';
  }

  exportTeamProductivityToExcel(): void {
    if (!this.teamProductivityData || this.teamProductivityData.length === 0) {
      alert('No data to export');
      return;
    }

    const exportData = this.teamProductivityData.map(team => ({
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

  getIconSvg(icon: string): string {
    const icons: Record<string, string> = {
      projects: '<path d="M16 4h2a2 2 0 0 1 2 2v14a2 2 0 0 1-2 2H6a2 2 0 0 1-2-2V6a2 2 0 0 1 2-2h2"/><rect x="8" y="2" width="8" height="4" rx="1" ry="1"/><path d="M9 14l2 2 4-4"/>',
      boxes: '<path d="M21 16V8a2 2 0 0 0-1-1.73l-7-4a2 2 0 0 0-2 0l-7 4A2 2 0 0 0 3 8v8a2 2 0 0 0 1 1.73l7 4a2 2 0 0 0 2 0l7-4A2 2 0 0 0 21 16z"/><polyline points="3.27 6.96 12 12.01 20.73 6.96"/><line x1="12" y1="22.08" x2="12" y2="12"/><circle cx="7" cy="8" r="1"/><circle cx="17" cy="8" r="1"/><circle cx="7" cy="16" r="1"/><circle cx="17" cy="16" r="1"/>',
      team: '<path d="M17 21v-2a4 4 0 0 0-4-4H5a4 4 0 0 0-4 4v2"/><circle cx="9" cy="7" r="4"/><path d="M23 21v-2a4 4 0 0 0-3-3.87"/><path d="M16 3.13a4 4 0 0 1 0 7.75"/>'
    };
    return icons[icon] || '';
  }
}
