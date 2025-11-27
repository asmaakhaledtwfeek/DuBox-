import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { HeaderComponent } from '../../../shared/components/header/header.component';
import { SidebarComponent } from '../../../shared/components/sidebar/sidebar.component';
import { ReportsService, ReportSummary, BoxProgressData, TeamProductivityData, MissingMaterialsData, PhaseReadinessData } from '../../../core/services/reports.service';
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
  boxProgressData: BoxProgressData[] = [];
  teamProductivityData: TeamProductivityData[] = [];
  missingMaterialsData: MissingMaterialsData[] = [];
  phaseReadinessData: PhaseReadinessData[] = [];
  
  // Projects for filtering
  projects: Project[] = [];
  selectedProject = '';

  reports: ReportCard[] = [
    {
      id: 'box-progress',
      title: 'Box Progress Report',
      description: 'Track the progress of all boxes across different phases and stages',
      icon: 'box',
      color: 'blue'
    },
    {
      id: 'team-productivity',
      title: 'Team Productivity Report',
      description: 'Monitor the productivity and performance of deployed teams',
      icon: 'team',
      color: 'green'
    },
    {
      id: 'phase-readiness',
      title: 'Phase Readiness Report',
      description: 'Check readiness of phases and activities to launch successors',
      icon: 'phases',
      color: 'purple'
    },
    {
      id: 'missing-materials',
      title: 'Missing Materials Report',
      description: 'Identify missing materials related to any activity or phase',
      icon: 'materials',
      color: 'orange'
    }
  ];

  constructor(
    private reportsService: ReportsService,
    private projectService: ProjectService
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
    this.activeReport = reportId;
    
    if (reportId === 'box-progress') {
      this.loadBoxProgressReport();
    } else if (reportId === 'team-productivity') {
      this.loadTeamProductivityReport();
    } else if (reportId === 'missing-materials') {
      this.loadMissingMaterialsReport();
    } else if (reportId === 'phase-readiness') {
      this.loadPhaseReadinessReport();
    }
  }

  closeReport(): void {
    this.activeReport = null;
    this.selectedProject = '';
  }

  onProjectChange(): void {
    if (this.activeReport === 'box-progress') {
      this.loadBoxProgressReport();
    } else if (this.activeReport === 'team-productivity') {
      this.loadTeamProductivityReport();
    } else if (this.activeReport === 'missing-materials') {
      this.loadMissingMaterialsReport();
    } else if (this.activeReport === 'phase-readiness') {
      this.loadPhaseReadinessReport();
    }
  }

  loadBoxProgressReport(): void {
    this.loading = true;
    this.reportsService.getBoxProgressReport(this.selectedProject || undefined).subscribe({
      next: (data) => {
        this.boxProgressData = data;
        this.loading = false;
      },
      error: (err) => {
        console.error('Failed to load box progress:', err);
        this.loading = false;
      }
    });
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

  loadMissingMaterialsReport(): void {
    this.loading = true;
    this.reportsService.getMissingMaterialsReport(this.selectedProject || undefined).subscribe({
      next: (data) => {
        this.missingMaterialsData = data;
        this.loading = false;
      },
      error: (err) => {
        console.error('Failed to load missing materials:', err);
        this.loading = false;
      }
    });
  }

  loadPhaseReadinessReport(): void {
    this.loading = true;
    this.reportsService.getPhaseReadinessReport(this.selectedProject || undefined).subscribe({
      next: (data) => {
        this.phaseReadinessData = data;
        this.loading = false;
      },
      error: (err) => {
        console.error('Failed to load phase readiness:', err);
        this.loading = false;
      }
    });
  }

  // Box Progress Report methods
  getBoxProgressPercentage(building: BoxProgressData): number {
    const completed = building.released1stFix + building.released2ndFix + building.released3rdFix;
    return building.total > 0 ? Math.round((completed / building.total) * 100) : 0;
  }

  getProgressColor(percentage: number): string {
    if (percentage >= 80) return 'green';
    if (percentage >= 50) return 'blue';
    if (percentage >= 25) return 'orange';
    return 'red';
  }

  exportBoxProgressToExcel(): void {
    if (!this.boxProgressData || this.boxProgressData.length === 0) {
      alert('No data to export');
      return;
    }

    const exportData = this.boxProgressData.map(building => ({
      'Building': building.building,
      'Non-Assembled': building.nonAssembled,
      'Backing (Due Boxes)': building.backing,
      'Released 1st Fix': building.released1stFix,
      'Released 2nd Fix': building.released2ndFix,
      'Released 3rd Fix': building.released3rdFix,
      'Total': building.total,
      'Progress %': this.getBoxProgressPercentage(building)
    }));

    const worksheet: XLSX.WorkSheet = XLSX.utils.json_to_sheet(exportData);
    const workbook: XLSX.WorkBook = {
      Sheets: { 'Box Progress Report': worksheet },
      SheetNames: ['Box Progress Report']
    };

    const today = new Date();
    const dateStr = today.toISOString().split('T')[0];
    XLSX.writeFile(workbook, `Box_Progress_Report_${dateStr}.xlsx`);
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

  exportMissingMaterialsToExcel(): void {
    if (!this.missingMaterialsData || this.missingMaterialsData.length === 0) {
      alert('No data to export');
      return;
    }

    const exportData = this.missingMaterialsData.map(material => ({
      'Material Name': material.materialName,
      'Material Code': material.materialCode,
      'Required Qty': material.requiredQuantity,
      'Available Qty': material.availableQuantity,
      'Shortage Qty': material.shortageQuantity,
      'Unit': material.unit,
      'Affected Boxes': material.affectedBoxes
    }));

    const worksheet: XLSX.WorkSheet = XLSX.utils.json_to_sheet(exportData);
    const workbook: XLSX.WorkBook = {
      Sheets: { 'Missing Materials': worksheet },
      SheetNames: ['Missing Materials']
    };

    const today = new Date();
    const dateStr = today.toISOString().split('T')[0];
    XLSX.writeFile(workbook, `Missing_Materials_Report_${dateStr}.xlsx`);
  }

  exportPhaseReadinessToExcel(): void {
    if (!this.phaseReadinessData || this.phaseReadinessData.length === 0) {
      alert('No data to export');
      return;
    }

    const exportData = this.phaseReadinessData.map(phase => ({
      'Phase Name': phase.phaseName,
      'Total Boxes': phase.totalBoxes,
      'Ready Boxes': phase.readyBoxes,
      'Pending Boxes': phase.pendingBoxes,
      'Readiness %': phase.readinessPercentage,
      'Blocking Issues': phase.blockingIssues.join('; ')
    }));

    const worksheet: XLSX.WorkSheet = XLSX.utils.json_to_sheet(exportData);
    const workbook: XLSX.WorkBook = {
      Sheets: { 'Phase Readiness': worksheet },
      SheetNames: ['Phase Readiness']
    };

    const today = new Date();
    const dateStr = today.toISOString().split('T')[0];
    XLSX.writeFile(workbook, `Phase_Readiness_Report_${dateStr}.xlsx`);
  }

  getReadinessColor(percentage: number): string {
    if (percentage >= 90) return 'green';
    if (percentage >= 70) return 'blue';
    if (percentage >= 50) return 'orange';
    return 'red';
  }

  getIconSvg(icon: string): string {
    const icons: Record<string, string> = {
      box: '<path d="M21 16V8a2 2 0 0 0-1-1.73l-7-4a2 2 0 0 0-2 0l-7 4A2 2 0 0 0 3 8v8a2 2 0 0 0 1 1.73l7 4a2 2 0 0 0 2 0l7-4A2 2 0 0 0 21 16z"/><polyline points="3.27 6.96 12 12.01 20.73 6.96"/><line x1="12" y1="22.08" x2="12" y2="12"/>',
      team: '<path d="M17 21v-2a4 4 0 0 0-4-4H5a4 4 0 0 0-4 4v2"/><circle cx="9" cy="7" r="4"/><path d="M23 21v-2a4 4 0 0 0-3-3.87"/><path d="M16 3.13a4 4 0 0 1 0 7.75"/>',
      phases: '<rect x="3" y="3" width="18" height="18" rx="2" ry="2"/><line x1="3" y1="9" x2="21" y2="9"/><line x1="9" y1="21" x2="9" y2="9"/>',
      materials: '<line x1="16.5" y1="9.4" x2="7.5" y2="4.21"/><path d="M21 16V8a2 2 0 0 0-1-1.73l-7-4a2 2 0 0 0-2 0l-7 4A2 2 0 0 0 3 8v8a2 2 0 0 0 1 1.73l7 4a2 2 0 0 0 2 0l7-4A2 2 0 0 0 21 16z"/><polyline points="3.27 6.96 12 12.01 20.73 6.96"/><line x1="12" y1="22.08" x2="12" y2="12"/>'
    };
    return icons[icon] || '';
  }
}
