import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, Router } from '@angular/router';
import { WIRService } from '../../../core/services/wir.service';
import { QualityIssueDetails } from '../../../core/models/wir.model';
import { HeaderComponent } from '../../../shared/components/header/header.component';
import { SidebarComponent } from '../../../shared/components/sidebar/sidebar.component';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-project-quality-issues',
  standalone: true,
  imports: [CommonModule, HeaderComponent, SidebarComponent, FormsModule],
  templateUrl: './project-quality-issues.component.html',
  styleUrl: './project-quality-issues.component.scss'
})
export class ProjectQualityIssuesComponent implements OnInit {
  projectId: string = '';
  projectName: string = '';
  qualityIssues: QualityIssueDetails[] = [];
  filteredQualityIssues: QualityIssueDetails[] = [];
  loading = true;
  error = '';

  // Filters
  searchTerm = '';
  selectedStatus = '';
  selectedType = '';
  selectedSeverity = '';
  activeKpiCard = '';

  statusOptions = ['Open', 'InProgress', 'Resolved', 'Closed'];
  typeOptions = ['Defect', 'NonConformance', 'Observation'];
  severityOptions = ['Critical', 'Major', 'Minor'];

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private wirService: WIRService
  ) {}

  ngOnInit(): void {
    this.projectId = this.route.snapshot.params['id'];
    console.log('📋 Project Quality Issues - Project ID:', this.projectId);
    
    if (!this.projectId) {
      console.error('❌ No project ID in route!');
      this.error = 'Project ID is missing';
      this.loading = false;
      return;
    }
    
    this.loadQualityIssues();
  }

  loadQualityIssues(): void {
    this.loading = true;
    this.error = '';
    
    this.wirService.getQualityIssuesByProject(this.projectId).subscribe({
      next: (issues) => {
        console.log('✅ Quality issues loaded:', issues.length);
        this.qualityIssues = issues;
        
        // Get project name from first issue
        if (issues.length > 0) {
          this.projectName = issues[0].projectName || '';
        }
        
        this.applyFilters();
        this.loading = false;
      },
      error: (err) => {
        console.error('❌ Error loading quality issues:', err);
        this.error = 'Failed to load quality issues';
        this.loading = false;
      }
    });
  }

  applyFilters(): void {
    let filtered = [...this.qualityIssues];

    // Search filter
    if (this.searchTerm) {
      const term = this.searchTerm.toLowerCase();
      filtered = filtered.filter(issue =>
        (issue.issueNumber?.toLowerCase().includes(term)) ||
        (issue.issueDescription?.toLowerCase().includes(term)) ||
        (issue.boxTag?.toLowerCase().includes(term)) ||
        (issue.boxName?.toLowerCase().includes(term))
      );
    }

    // Status filter
    if (this.selectedStatus) {
      filtered = filtered.filter(issue => issue.status === this.selectedStatus);
    }

    // Type filter
    if (this.selectedType) {
      filtered = filtered.filter(issue => issue.issueType === this.selectedType);
    }

    // Severity filter
    if (this.selectedSeverity) {
      filtered = filtered.filter(issue => issue.severity === this.selectedSeverity);
    }

    this.filteredQualityIssues = filtered;
    
    // Sync activeKpiCard with selectedStatus
    this.activeKpiCard = this.selectedStatus;
  }

  resetFilters(): void {
    this.searchTerm = '';
    this.selectedStatus = '';
    this.selectedType = '';
    this.selectedSeverity = '';
    this.activeKpiCard = '';
    this.applyFilters();
  }

  onKpiCardClick(status: string): void {
    // If clicking the same card, clear the filter
    if (this.activeKpiCard === status) {
      this.activeKpiCard = '';
      this.selectedStatus = '';
    } else {
      this.activeKpiCard = status;
      this.selectedStatus = status;
    }
    this.applyFilters();
  }

  getStatusClass(status: string | undefined): string {
    switch (status) {
      case 'Open':
        return 'status-pill status-open';
      case 'InProgress':
        return 'status-pill status-in-progress';
      case 'Resolved':
        return 'status-pill status-resolved';
      case 'Closed':
        return 'status-pill status-closed';
      default:
        return 'status-pill status-default';
    }
  }

  getStatusLabel(status: string | undefined): string {
    switch (status) {
      case 'InProgress':
        return 'In Progress';
      default:
        return status || '—';
    }
  }

  getSeverityClass(severity: string | undefined): string {
    switch (severity?.toLowerCase()) {
      case 'critical':
        return 'severity-critical';
      case 'major':
        return 'severity-major';
      case 'minor':
        return 'severity-minor';
      default:
        return '';
    }
  }

  formatDate(date: Date | string | undefined): string {
    if (!date) return '—';
    const d = new Date(date);
    if (isNaN(d.getTime())) return '—';
    return d.toLocaleDateString('en-US', { year: 'numeric', month: 'short', day: 'numeric' });
  }

  viewIssueDetails(issue: QualityIssueDetails): void {
    // Navigate to box details with quality issue tab
    if (issue.boxId) {
      this.router.navigate(['/boxes', issue.boxId], {
        queryParams: { tab: 'quality-issues' }
      });
    }
  }

  goBack(): void {
    this.router.navigate(['/projects', this.projectId]);
  }

  exportToExcel(): void {
    console.log('📊 Exporting quality issues to Excel');
    // TODO: Implement Excel export
    alert('Excel export feature coming soon!');
  }

  getStatusCount(status: string): number {
    return this.qualityIssues.filter(issue => issue.status === status).length;
  }

  formatStageNumber(wirNumber: string | undefined): string {
    if (!wirNumber) return '—';
    // Replace "WIR" with "Stage" in the display
    return wirNumber.replace(/WIR/gi, 'Stage');
  }
}

