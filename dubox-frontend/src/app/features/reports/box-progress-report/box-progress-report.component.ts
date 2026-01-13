import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { HeaderComponent } from '../../../shared/components/header/header.component';
import { SidebarComponent } from '../../../shared/components/sidebar/sidebar.component';
import { ReportsService, BoxProgressData } from '../../../core/services/reports.service';
import { ProjectService } from '../../../core/services/project.service';
import { Project } from '../../../core/models/project.model';
import * as XLSX from 'xlsx';
import { catchError, of } from 'rxjs';

interface BuildingStatus {
  buildingNumber: string;
  nonAssembled: number;
  backing: number;
  released1stFix: number;
  released2ndFix: number;
  released3rdFix: number;
  total: number;
}

interface PhaseData {
  phase: string;
  count: number;
  percentage: number;
}

@Component({
  selector: 'app-box-progress-report',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterModule, HeaderComponent, SidebarComponent],
  templateUrl: './box-progress-report.component.html',
  styleUrls: ['./box-progress-report.component.scss']
})
export class BoxProgressReportComponent implements OnInit {
  buildingStatuses: BuildingStatus[] = [];
  phaseDistribution: PhaseData[] = [];
  projects: Project[] = [];
  selectedProject = '';
  loading = false;
  error = '';

  // Summary stats
  totalBoxes = 0;
  averageProgress = 0;

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

    // Try to get data from reports endpoint first
    this.reportsService.getBoxProgressReport(this.selectedProject || undefined)
      .pipe(
        catchError(err => {
          console.warn('Reports endpoint not available, falling back to boxes endpoint:', err);
          // Fallback to calculating from boxes data
          return this.reportsService.getBoxProgressFromBoxes(this.selectedProject || undefined);
        })
      )
      .subscribe({
        next: (data) => {
          this.buildingStatuses = data;
          this.calculateSummary();
          this.calculatePhaseDistribution();
          this.loading = false;
        },
        error: (err) => {
          console.error('Failed to load box progress data:', err);
          this.error = err?.error?.message || err?.message || 'Failed to load box progress data';
          this.loading = false;
          // Load mock data as last resort
          this.loadMockData();
        }
      });
  }

  onProjectChange(): void {
    this.loadReportData();
  }

  loadMockData(): void {
    // Mock data based on the PDF sample (fallback)
    console.log('📊 Loading mock data as fallback');
    this.buildingStatuses = [
      {
        buildingNumber: 'KJ158-Building 1 (GF,FF,SF)',
        nonAssembled: 48,
        backing: 0,
        released1stFix: 0,
        released2ndFix: 9,
        released3rdFix: 28,
        total: 85
      },
      {
        buildingNumber: 'KJ158-Building 2 (GF,FF,SF)',
        nonAssembled: 32,
        backing: 5,
        released1stFix: 12,
        released2ndFix: 18,
        released3rdFix: 15,
        total: 82
      },
      {
        buildingNumber: 'KJ158-Building 3 (GF,FF,SF)',
        nonAssembled: 25,
        backing: 3,
        released1stFix: 8,
        released2ndFix: 22,
        released3rdFix: 20,
        total: 78
      },
      {
        buildingNumber: 'KJ158-Building 4 (GF,FF,SF)',
        nonAssembled: 48,
        backing: 0,
        released1stFix: 0,
        released2ndFix: 9,
        released3rdFix: 28,
        total: 85
      }
    ];

    this.calculateSummary();
    this.calculatePhaseDistribution();
  }

  calculateSummary(): void {
    this.totalBoxes = this.buildingStatuses.reduce((sum, b) => sum + b.total, 0);
    
    // Calculate average progress based on phase completion
    const totalCompleted = this.buildingStatuses.reduce(
      (sum, b) => sum + b.released1stFix + b.released2ndFix + b.released3rdFix,
      0
    );
    this.averageProgress = this.totalBoxes > 0 
      ? Math.round((totalCompleted / this.totalBoxes) * 100) 
      : 0;
  }

  calculatePhaseDistribution(): void {
    const total = this.totalBoxes;
    const nonAssembled = this.buildingStatuses.reduce((sum, b) => sum + b.nonAssembled, 0);
    const backing = this.buildingStatuses.reduce((sum, b) => sum + b.backing, 0);
    const fix1st = this.buildingStatuses.reduce((sum, b) => sum + b.released1stFix, 0);
    const fix2nd = this.buildingStatuses.reduce((sum, b) => sum + b.released2ndFix, 0);
    const fix3rd = this.buildingStatuses.reduce((sum, b) => sum + b.released3rdFix, 0);

    this.phaseDistribution = [
      { phase: 'Non-Assembled', count: nonAssembled, percentage: (nonAssembled / total) * 100 },
      { phase: 'Backing (Due)', count: backing, percentage: (backing / total) * 100 },
      { phase: 'Released 1st Fix', count: fix1st, percentage: (fix1st / total) * 100 },
      { phase: 'Released 2nd Fix', count: fix2nd, percentage: (fix2nd / total) * 100 },
      { phase: 'Released 3rd Fix', count: fix3rd, percentage: (fix3rd / total) * 100 }
    ];
  }

  getProgressPercentage(building: BuildingStatus): number {
    const completed = building.released1stFix + building.released2ndFix + building.released3rdFix;
    return building.total > 0 ? Math.round((completed / building.total) * 100) : 0;
  }

  getProgressColor(percentage: number): string {
    if (percentage >= 80) return 'green';
    if (percentage >= 50) return 'blue';
    if (percentage >= 25) return 'orange';
    return 'red';
  }

  exportToExcel(): void {
    if (!this.buildingStatuses || this.buildingStatuses.length === 0) {
      alert('No data to export');
      return;
    }

    // Prepare data for Excel
    const exportData = this.buildingStatuses.map(building => ({
      'Building Number': building.buildingNumber,
      'Non-Assembled': building.nonAssembled,
      'Backing (Due Boxes)': building.backing,
      'Released 1st Fix': building.released1stFix,
      'Released 2nd Fix': building.released2ndFix,
      'Released 3rd Fix': building.released3rdFix,
      'Total': building.total,
      'Progress %': this.getProgressPercentage(building)
    }));

    // Create worksheet
    const worksheet: XLSX.WorkSheet = XLSX.utils.json_to_sheet(exportData);

    // Set column widths
    const columnWidths = [
      { wch: 35 }, // Building Number
      { wch: 15 }, // Non-Assembled
      { wch: 20 }, // Backing
      { wch: 18 }, // Released 1st Fix
      { wch: 18 }, // Released 2nd Fix
      { wch: 18 }, // Released 3rd Fix
      { wch: 10 }, // Total
      { wch: 12 }  // Progress %
    ];
    worksheet['!cols'] = columnWidths;

    // Create workbook
    const workbook: XLSX.WorkBook = {
      Sheets: { 'Box Progress Report': worksheet },
      SheetNames: ['Box Progress Report']
    };

    // Generate file name with current date
    const today = new Date();
    const dateStr = today.toISOString().split('T')[0];
    const fileName = `Box_Progress_Report_${dateStr}.xlsx`;

    // Save file
    XLSX.writeFile(workbook, fileName);
  }
}
