import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { ProjectService } from '../../../core/services/project.service';
import { BoxService } from '../../../core/services/box.service';
import { PermissionService } from '../../../core/services/permission.service';
import { Project, ProjectStatus } from '../../../core/models/project.model';
import { Box, BoxImportResult, BoxStatus } from '../../../core/models/box.model';
import { HeaderComponent } from '../../../shared/components/header/header.component';
import { SidebarComponent } from '../../../shared/components/sidebar/sidebar.component';
import { trigger, style, animate, transition } from '@angular/animations';

@Component({
  selector: 'app-project-dashboard',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterModule, HeaderComponent, SidebarComponent],
  templateUrl: './project-dashboard.component.html',
  styleUrl: './project-dashboard.component.scss',
  animations: [
    trigger('fadeOut', [
      transition(':enter', [
        style({ opacity: 0, transform: 'translateY(-6px)' }),
        animate('200ms ease-out', style({ opacity: 1, transform: 'translateY(0)' }))
      ]),
      transition(':leave', [
        animate('400ms ease-in', style({ opacity: 0, transform: 'translateY(-6px)' }))
      ])
    ])
  ]
})
export class ProjectDashboardComponent implements OnInit {
  @ViewChild('fileInput') fileInputRef?: ElementRef<HTMLInputElement>;
  @ViewChild('excelSection') excelSectionRef?: ElementRef<HTMLDivElement>;

  project: Project | null = null;
  projectId: string = '';
  loading = true;
  error = '';
  banner: { message: string; type: 'success' | 'error' } | null = null;
  bannerTimeoutId: any = null;
  showDeleteConfirm = false;
  deleting = false;
  canEdit = false;
  canDelete = false;
  canImportBoxes = false;
  templateDownloading = false;
  isDraggingFile = false;
  selectedFile: File | null = null;
  importingExcel = false;
  importSuccessMessage = '';
  importErrorMessage = '';
  importResult: BoxImportResult | null = null;
  showStatusModal = false;
  statusOptions = Object.values(ProjectStatus).filter(
    (status) => status !== ProjectStatus.Completed
  ) as ProjectStatus[];
  selectedStatus: ProjectStatus | '' = '';
  statusUpdating = false;
  statusError = '';
  
  showCompressionDateModal = false;
  selectedCompressionDate: Date | null = null;
  compressionDateUpdating = false;
  compressionDateError = '';


  dashboardData = {
    totalBoxes: 0,
    completedBoxes: 0,
    inProgressBoxes: 0,
    readyForDelivery: 0,
    notStarted: 0,
    onHold: 0
  };

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private projectService: ProjectService,
    private boxService: BoxService,
    private permissionService: PermissionService
  ) {}

  ngOnInit(): void {
    this.projectId = this.route.snapshot.params['id'];
    console.log('🏠 Project Dashboard - Project ID from route:', this.projectId);
    if (!this.projectId) {
      console.error('❌ No project ID in route!');
      this.error = 'Project ID is missing';
      this.loading = false;
      return;
    }
    this.canEdit = this.permissionService.canEdit('projects');
    this.canDelete = this.permissionService.canDelete('projects');
    this.canImportBoxes = this.permissionService.hasPermission('boxes', 'import');
    this.loadProject();
  }

  loadProject(): void {
    this.loading = true;
    console.log('📡 Loading project data for ID:', this.projectId);
    
    this.projectService.getProject(this.projectId).subscribe({
      next: (project) => {
        console.log('✅ Project loaded:', project);
        console.log('🆔 Project ID:', project.id);

        this.project = {
          ...project,
          startDate: project.startDate ? new Date(project.startDate) : undefined,
          plannedStartDate: project.plannedStartDate ? new Date(project.plannedStartDate) : undefined,
          actualStartDate: project.actualStartDate ? new Date(project.actualStartDate) : undefined,
          compressionStartDate: project.compressionStartDate ? new Date(project.compressionStartDate) : undefined
        };

        // Load boxes to calculate accurate counts
        this.loadBoxesAndCalculateCounts();
      },
      error: (error) => {
        this.error = 'Failed to load project';
        this.loading = false;
        console.error('❌ Error loading project:', error);
      }
    });
  }

  loadBoxesAndCalculateCounts(): void {
    this.boxService.getBoxesByProject(this.projectId).subscribe({
      next: (boxes) => {
        console.log('✅ Boxes loaded:', boxes.length);
        
        // Calculate counts based on actual box statuses
        const counts = {
          totalBoxes: boxes.length,
          completedBoxes: 0,
          inProgressBoxes: 0,
          readyForDelivery: 0,
          notStarted: 0,
          onHold: 0
        };

        boxes.forEach(box => {
          const status = box.status as BoxStatus;
          switch (status) {
            case BoxStatus.NotStarted:
              counts.notStarted++;
              break;
            case BoxStatus.InProgress:
              counts.inProgressBoxes++;
              break;
            case BoxStatus.QAReview:
              // QA Review is also considered in progress
              counts.inProgressBoxes++;
              break;
            case BoxStatus.Completed:
              counts.completedBoxes++;
              break;
            case BoxStatus.ReadyForDelivery:
              counts.readyForDelivery++;
              break;
            case BoxStatus.Delivered:
              // Delivered is also considered completed
              counts.completedBoxes++;
              break;
            case BoxStatus.Dispatched:
              // Dispatched is also considered completed
              counts.completedBoxes++;
              break;
            case BoxStatus.OnHold:
              counts.onHold++;
              break;
          }
        });

        this.dashboardData = counts;

        if (this.project) {
          const earliestActualStart = boxes
            .map(b => b.actualStartDate)
            .filter((date): date is Date => !!date)
            .sort((a, b) => a.getTime() - b.getTime())[0];

          if (earliestActualStart) {
            this.project.actualStartDate = earliestActualStart;
          } 
        }
        console.log('📊 Calculated box counts:', this.dashboardData);
        console.log('📊 Project progress (from ProgressPercentage):', this.project?.progress + '%');
        this.loading = false;
      },
      error: (err) => {
        console.error('❌ Error loading boxes:', err);
        // Fallback to project data if boxes can't be loaded
        if (this.project) {
          this.dashboardData = {
            totalBoxes: this.project.totalBoxes || 0,
            completedBoxes: this.project.completedBoxes || 0,
            inProgressBoxes: this.project.inProgressBoxes || 0,
            readyForDelivery: this.project.readyForDeliveryBoxes || 0,
            notStarted: 0,
            onHold: 0
          };
        }
        this.loading = false;
      }
    });
  }

  openStatusModal(): void {
    if (!this.project) {
      return;
    }
    this.selectedStatus = (this.project.status as ProjectStatus) || ProjectStatus.Active;
    this.statusError = '';
    this.showStatusModal = true;
    document.body.style.overflow = 'hidden';
  }

  closeStatusModal(): void {
    this.showStatusModal = false;
    this.statusUpdating = false;
    this.statusError = '';
    document.body.style.overflow = '';
  }

  updateStatus(): void {
    if (!this.project || !this.selectedStatus || this.statusUpdating) {
      return;
    }
    this.statusUpdating = true;
    this.statusError = '';

    this.projectService.updateProjectStatus(this.project.id, this.selectedStatus).subscribe({
      next: (updatedProject) => {
        this.project = {
          ...updatedProject,
          startDate: updatedProject.startDate ? new Date(updatedProject.startDate) : undefined,
          plannedStartDate: updatedProject.plannedStartDate ? new Date(updatedProject.plannedStartDate) : undefined,
          actualStartDate: updatedProject.actualStartDate ? new Date(updatedProject.actualStartDate) : undefined,
          compressionStartDate: updatedProject.compressionStartDate ? new Date(updatedProject.compressionStartDate) : undefined
        };
        this.statusUpdating = false;
        this.closeStatusModal();
        document.dispatchEvent(new CustomEvent('app-toast', {
          detail: { message: 'Project status updated successfully', type: 'success' }
        }));
      },
      error: (error) => {
        console.error('Failed to update project status', error);
        this.statusUpdating = false;
        this.statusError = error?.error?.message || 'Failed to update project status';
      }
    });
  }


  viewBoxes(): void {
    console.log('🔍 Navigate to boxes for project:', this.projectId);
    if (!this.projectId) {
      console.error('❌ Cannot navigate: projectId is undefined');
      alert('Error: Project ID is missing. Cannot view boxes.');
      return;
    }
    this.router.navigate(['/projects', this.projectId, 'boxes']);
  }

  openImportExcel(): void {
    this.excelSectionRef?.nativeElement.scrollIntoView({ behavior: 'smooth', block: 'start' });
    setTimeout(() => {
      if (this.fileInputRef) {
        this.fileInputRef.nativeElement.focus();
      }
    }, 350);
  }

  downloadTemplate(): void {
    if (this.templateDownloading) {
      return;
    }

    this.templateDownloading = true;
    this.boxService.downloadBoxesTemplate().subscribe({
      next: (blob) => {
        const url = window.URL.createObjectURL(blob);
        const link = document.createElement('a');
        link.href = url;
        link.download = 'BoxesImportTemplate.xlsx';
        link.click();
        window.URL.revokeObjectURL(url);
        this.templateDownloading = false;
      },
      error: (error) => {
        console.error('❌ Error downloading template:', error);
        this.templateDownloading = false;
        this.importErrorMessage = error?.error?.message || 'Unable to download template right now.';
      }
    });
  }

  onBrowseClick(): void {
    if (!this.fileInputRef) {
      return;
    }

    this.fileInputRef.nativeElement.value = '';
    this.fileInputRef.nativeElement.click();
  }

  onFileChange(event: Event): void {
    const input = event.target as HTMLInputElement;
    if (input.files && input.files.length > 0) {
      this.setSelectedFile(input.files[0]);
    }
  }

  onDragOver(event: DragEvent): void {
    event.preventDefault();
    event.stopPropagation();
    this.isDraggingFile = true;
  }

  onDragLeave(event: DragEvent): void {
    event.preventDefault();
    event.stopPropagation();
    this.isDraggingFile = false;
  }

  onDrop(event: DragEvent): void {
    event.preventDefault();
    event.stopPropagation();
    this.isDraggingFile = false;

    if (event.dataTransfer && event.dataTransfer.files.length > 0) {
      this.setSelectedFile(event.dataTransfer.files[0]);
      event.dataTransfer.clearData();
    }
  }

  private setSelectedFile(file: File): void {
    if (!this.isValidExcelFile(file)) {
      this.importErrorMessage = 'Please upload a valid Excel file (.xlsx or .xls).';
      this.selectedFile = null;
      return;
    }

    this.importErrorMessage = '';
    this.importSuccessMessage = '';
    this.importResult = null;
    this.selectedFile = file;
  }

  removeSelectedFile(): void {
    this.selectedFile = null;
    if (this.fileInputRef) {
      this.fileInputRef.nativeElement.value = '';
    }
  }

  uploadSelectedFile(): void {
    if (!this.selectedFile || !this.projectId) {
      this.importErrorMessage = 'Please select a project and choose an Excel file to upload.';
      return;
    }

    this.importingExcel = true;
    this.importErrorMessage = '';
    this.importSuccessMessage = '';
    this.importResult = null;

    this.boxService.importBoxesFromExcel(this.projectId, this.selectedFile).subscribe({
      next: (result) => {
        this.importingExcel = false;
        this.importResult = result;
        this.importSuccessMessage = `Import completed. ${result.successCount} boxes added, ${result.failureCount} failed.`;
        if (result.successCount > 0) {
          this.loadBoxesAndCalculateCounts();
        }
      },
      error: (error) => {
        this.importingExcel = false;
        console.error('❌ Excel import failed:', error);
        this.importErrorMessage = error?.error?.message || 'Failed to import Excel file. Please try again.';
      }
    });
  }

  private isValidExcelFile(file: File): boolean {
    const allowedExtensions = ['.xlsx', '.xls'];
    const fileExtension = file.name.substring(file.name.lastIndexOf('.')).toLowerCase();
    return allowedExtensions.includes(fileExtension);
  }

  formatFileSize(bytes: number): string {
    if (!bytes) {
      return '0 KB';
    }
    const sizes = ['Bytes', 'KB', 'MB', 'GB'];
    const i = Math.floor(Math.log(bytes) / Math.log(1024));
    const size = bytes / Math.pow(1024, i);
    return `${size.toFixed(1)} ${sizes[i]}`;
  }

  goBack(): void {
    this.router.navigate(['/projects']);
  }

  getProjectStatusClass(status: ProjectStatus | string | undefined): string {
    switch (status) {
      case ProjectStatus.Active:
        return 'badge badge-success';
      case ProjectStatus.OnHold:
        return 'badge badge-warning';
      case ProjectStatus.Completed:
        return 'badge badge-success';
      case ProjectStatus.Archived:
        return 'badge badge-neutral';
      case ProjectStatus.Closed:
        return 'badge badge-danger';
      default:
        return 'badge badge-neutral';
    }
  }

  editProject(): void {
    if (!this.projectId) {
      return;
    }

    this.router.navigate(['/projects/create'], {
      queryParams: { mode: 'edit', projectId: this.projectId }
    });
  }

  openDeleteConfirm(): void {
    this.showDeleteConfirm = true;
  }

  cancelDelete(): void {
    this.showDeleteConfirm = false;
  }

  deleteProject(): void {
    if (this.deleting || !this.projectId) {
      return;
    }

    this.deleting = true;
    this.error = '';

    this.projectService.deleteProject(this.projectId).subscribe({
      next: () => {
        this.deleting = false;
        this.showDeleteConfirm = false;
        this.showBanner('Project deleted successfully.', 'success');
        setTimeout(() => {
          this.router.navigate(['/projects']);
        }, 1200);
      },
      error: (err) => {
        this.deleting = false;
        this.showDeleteConfirm = false;
        const message = err?.error?.message || err.message || 'Failed to delete project';
        this.showBanner(message, 'error');
        console.error('❌ Error deleting project:', err);
      }
    });
  }

  formatProgress(progress: number | undefined): string {
    const value = typeof progress === 'number' && isFinite(progress) ? progress : 0;
    return value.toFixed(2);
  }

  getProgressForBar(progress: number | undefined): number {
    const value = typeof progress === 'number' && isFinite(progress) ? progress : 0;
    return Math.min(value, 100);
  }

  getProgressColor(progress: number | undefined): string {
    const value = typeof progress === 'number' && isFinite(progress) ? progress : 0;
    const normalizedProgress = Math.max(0, Math.min(value, 100));
    if (normalizedProgress >= 75) return 'var(--success-color)';
    if (normalizedProgress >= 50) return 'var(--info-color)';
    if (normalizedProgress >= 25) return 'var(--warning-color)';
    return 'var(--error-color)';
  }

  private showBanner(message: string, type: 'success' | 'error'): void {
    this.banner = { message, type };
    if (this.bannerTimeoutId) {
      clearTimeout(this.bannerTimeoutId);
    }
    window.scrollTo({ top: 0, behavior: 'smooth' });
    this.bannerTimeoutId = setTimeout(() => {
      this.banner = null;
      this.bannerTimeoutId = null;
    }, 5000);
  }

  dismissBanner(): void {
    if (this.bannerTimeoutId) {
      clearTimeout(this.bannerTimeoutId);
      this.bannerTimeoutId = null;
    }
    this.banner = null;
  }

  /**
   * Get the priority start date based on: ActualStartDate > CompressionStartDate > PlannedStartDate
   */
  getPriorityStartDate(): Date | undefined {
    if (!this.project) return undefined;
    
    const actual = this.normalizeDate(this.project.actualStartDate);
    if (actual) return actual;
    
    const compression = this.normalizeDate(this.project.compressionStartDate);
    if (compression) return compression;
    
    return this.normalizeDate(this.project.plannedStartDate);
  }

  /**
   * Get the label for the priority start date
   */
  getPriorityStartDateLabel(): string {
    if (!this.project) return 'Not Scheduled';
    
    if (this.normalizeDate(this.project.actualStartDate)) {
      return 'Started:';
    }
    if (this.normalizeDate(this.project.compressionStartDate)) {
      return 'Compression Start:';
    }
    if (this.normalizeDate(this.project.plannedStartDate)) {
      return 'Planned Start:';
    }
    return 'Not Scheduled';
  }

  /**
   * Get formatted priority start date or fallback text
   */
  getPriorityStartDateDisplay(): string {
    const date = this.getPriorityStartDate();
    if (!date) return 'Not Scheduled';
    return date.toLocaleDateString('en-US', { year: 'numeric', month: 'short', day: 'numeric' });
  }

  get startDateLabel(): string {
    return this.getPriorityStartDateLabel();
  }

  get startDateValue(): Date | undefined {
    return this.getPriorityStartDate();
  }

  private hasActualStartDate(): boolean {
    const value = this.project?.actualStartDate;
    const normalized = this.normalizeDate(value);
    return !!normalized;
  }

  private normalizeDate(value: Date | string | undefined | null): Date | undefined {
    if (!value) {
      return undefined;
    }

    const dateValue = value instanceof Date ? value : new Date(value);
    if (isNaN(dateValue.getTime())) {
      return undefined;
    }

    if (dateValue.getUTCFullYear() <= 1900) {
      return undefined;
    }

    return dateValue;
  }

  openCompressionDateModal(): void {
    if (!this.project) {
      return;
    }
    this.selectedCompressionDate = this.project.compressionStartDate ? new Date(this.project.compressionStartDate) : null;
    this.compressionDateError = '';
    this.showCompressionDateModal = true;
    document.body.style.overflow = 'hidden';
  }

  closeCompressionDateModal(): void {
    this.showCompressionDateModal = false;
    this.compressionDateUpdating = false;
    this.compressionDateError = '';
    document.body.style.overflow = '';
  }

  updateCompressionStartDate(): void {
    if (!this.project || this.compressionDateUpdating) {
      return;
    }
    this.compressionDateUpdating = true;
    this.compressionDateError = '';

    this.projectService.updateCompressionStartDate(this.project.id, this.selectedCompressionDate).subscribe({
      next: (updatedProject) => {
        this.project = {
          ...updatedProject,
          startDate: updatedProject.startDate ? new Date(updatedProject.startDate) : undefined,
          plannedStartDate: updatedProject.plannedStartDate ? new Date(updatedProject.plannedStartDate) : undefined,
          actualStartDate: updatedProject.actualStartDate ? new Date(updatedProject.actualStartDate) : undefined,
          compressionStartDate: updatedProject.compressionStartDate ? new Date(updatedProject.compressionStartDate) : undefined
        };
        this.compressionDateUpdating = false;
        this.closeCompressionDateModal();
        document.dispatchEvent(new CustomEvent('app-toast', {
          detail: { message: 'Compression start date updated successfully', type: 'success' }
        }));
      },
      error: (error) => {
        console.error('Failed to update compression start date', error);
        this.compressionDateUpdating = false;
        this.compressionDateError = error?.error?.message || 'Failed to update compression start date';
      }
    });
  }

  formatDateForInput(date: Date | null | undefined): string {
    if (!date) return '';
    const d = date instanceof Date ? date : new Date(date);
    if (isNaN(d.getTime())) return '';
    const year = d.getFullYear();
    const month = String(d.getMonth() + 1).padStart(2, '0');
    const day = String(d.getDate()).padStart(2, '0');
    return `${year}-${month}-${day}`;
  }

  onCompressionDateChange(event: Event): void {
    const target = event.target as HTMLInputElement;
    const value = target.value;
    this.selectedCompressionDate = value ? new Date(value) : null;
  }
}
