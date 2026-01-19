import { Component, ElementRef, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { Subscription } from 'rxjs';
import { skip } from 'rxjs/operators';
import { ProjectService } from '../../../core/services/project.service';
import { BoxService } from '../../../core/services/box.service';
import { WIRService } from '../../../core/services/wir.service';
import { PermissionService } from '../../../core/services/permission.service';
import { Project, ProjectStatus, getAvailableProjectStatuses, canChangeProjectStatus } from '../../../core/models/project.model';
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
export class ProjectDashboardComponent implements OnInit, OnDestroy {
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
  canChangeStatus = false;
  canImportBoxes = false;
  isProjectOnHold = false;
  isProjectClosed = false;
  isProjectArchived = false;
  templateDownloading = false;
  isDraggingFile = false;
  selectedFile: File | null = null;
  importingExcel = false;
  importSuccessMessage = '';
  importErrorMessage = '';
  importResult: BoxImportResult | null = null;
  showStatusModal = false;
  statusOptions: ProjectStatus[] = [];
  selectedStatus: ProjectStatus | '' = '';
  statusUpdating = false;
  statusError = '';
  canChangeProjectStatus = canChangeProjectStatus;
  
  showCompressionDateModal = false;
  selectedCompressionDate: Date | null = null;
  compressionDateUpdating = false;
  compressionDateError = '';

  boxes: Box[] = [];

  dashboardData = {
    totalBoxes: 0,
    completedBoxes: 0,
    inProgressBoxes: 0,
    dispatchedBoxes: 0,
    notStarted: 0,
    onHold: 0
  };

  qualityIssuesCount = 0;
  
  private subscriptions: Subscription[] = [];

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private projectService: ProjectService,
    private boxService: BoxService,
    private wirService: WIRService,
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
    
    // Check permissions immediately
    this.checkPermissions();
    
    // Subscribe to permission changes to update UI when permissions are loaded
    this.subscriptions.push(
      this.permissionService.permissions$
        .pipe(skip(1)) // Skip initial empty value
        .subscribe(() => {
          console.log('🔄 Permissions updated, re-checking project dashboard permissions');
          this.checkPermissions();
        })
    );
    
    this.loadProject();
  }
  
  ngOnDestroy(): void {
    this.subscriptions.forEach(sub => sub.unsubscribe());
    if (this.bannerTimeoutId) {
      clearTimeout(this.bannerTimeoutId);
    }
  }
  
  private checkPermissions(): void {
    this.canEdit = this.permissionService.canEdit('projects');
    this.canDelete = this.permissionService.canDelete('projects');
    this.canChangeStatus = this.permissionService.hasPermission('projects', 'manage') || 
                           this.permissionService.canEdit('projects');
    this.canImportBoxes = this.permissionService.hasPermission('boxes', 'import');
    console.log('✅ Project dashboard permissions checked:', {
      canEdit: this.canEdit,
      canDelete: this.canDelete,
      canChangeStatus: this.canChangeStatus,
      canImportBoxes: this.canImportBoxes
    });
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
        
        // Check if project is on hold, closed, or archived
        this.isProjectOnHold = this.project.status === 'OnHold';
        this.isProjectClosed = this.project.status === 'Closed';
        this.isProjectArchived = this.project.status === 'Archived';

        // Load boxes to calculate accurate counts
        this.loadBoxesAndCalculateCounts();
        
        // Load quality issues count
        this.loadQualityIssuesCount();
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
        
        // Store boxes for status modal checks
        this.boxes = boxes;
        
        // Calculate counts based on actual box statuses
        const counts = {
          totalBoxes: boxes.length,
          completedBoxes: 0,
          inProgressBoxes: 0,
          dispatchedBoxes: 0,
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
              // ReadyForDelivery is also considered in progress
              counts.inProgressBoxes++;
              break;
            case BoxStatus.Delivered:
              // Delivered is also considered completed
              counts.completedBoxes++;
              break;
            case BoxStatus.Dispatched:
              counts.dispatchedBoxes++;
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
            dispatchedBoxes: 0,
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
    
    // Check if project status can be changed
    if (!canChangeProjectStatus(this.project.status)) {
      this.statusError = 'Archived projects cannot have their status changed. The project is locked.';
      return;
    }
    
    // Get available statuses based on current status and progress
    // For OnHold and Closed projects, progress determines available transitions
    const progress = this.project.progress || 0;
    
    // Check if all boxes are completed or dispatched (for Closed -> Completed transition)
    const allBoxesCompletedOrDispatched = this.boxes.length > 0 && this.boxes.every((box: Box) => 
      box.status === 'Completed' || box.status === 'Dispatched'
    );
    
    // Check if all boxes are dispatched (required for Archived status)
    const allBoxesDispatched = this.boxes.length > 0 && this.boxes.every((box: Box) => 
      box.status === 'Dispatched'
    );
    
    this.statusOptions = getAvailableProjectStatuses(this.project.status, progress, allBoxesCompletedOrDispatched, allBoxesDispatched);
    
    // Show appropriate message for OnHold, Closed, and Completed projects
    if (this.project.status === ProjectStatus.OnHold) {
      if (progress >= 100) {
        if (allBoxesDispatched) {
          this.statusError = 'Project is on hold with 100% progress and all boxes dispatched. You can change status to Completed, Archived, or Closed.';
        } else {
          this.statusError = 'Project is on hold with 100% progress. You can change status to Completed or Closed. All boxes must be dispatched before archiving.';
        }
      } else {
        this.statusError = 'Project is on hold with less than 100% progress. You can change status to Active or Closed.';
      }
    } else if (this.project.status === ProjectStatus.Closed) {
      if (progress >= 100 && allBoxesCompletedOrDispatched) {
        this.statusError = 'Project is closed with 100% progress and all boxes completed or dispatched. You can change status to Completed.';
      } else if (progress < 100) {
        this.statusError = 'Project is closed with less than 100% progress. You can change status to OnHold or Active.';
      } else {
        this.statusError = 'Project is closed with 100% progress, but not all boxes are completed or dispatched. Complete or dispatch all boxes before changing to Completed.';
      }
    } else if (this.project.status === ProjectStatus.Completed) {
      if (allBoxesDispatched) {
        this.statusError = 'Project is completed and all boxes are dispatched. You can change status to OnHold, Closed, or Archived.';
      } else {
        this.statusError = 'Project is completed. You can change status to OnHold or Closed. All boxes must be dispatched before archiving.';
      }
    } else {
      this.statusError = '';
    }
    
    // Set default selection to first available option
    this.selectedStatus = this.statusOptions.length > 0 ? this.statusOptions[0] : '';
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
        // Refresh page to show updated status across all sections
        setTimeout(() => {
          window.location.reload();
        }, 1000);
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

  viewBoxesByStatus(status: BoxStatus): void {
    console.log('🔍 Navigate to boxes with status:', status, 'for project:', this.projectId);
    if (!this.projectId) {
      console.error('❌ Cannot navigate: projectId is undefined');
      alert('Error: Project ID is missing. Cannot view boxes.');
      return;
    }
    this.router.navigate(['/projects', this.projectId, 'boxes'], {
      queryParams: { status: status }
    });
  }

  viewCompletedBoxes(): void {
    this.viewBoxesByStatus(BoxStatus.Completed);
  }

  viewInProgressBoxes(): void {
    this.viewBoxesByStatus(BoxStatus.InProgress);
  }

  viewDispatchedBoxes(): void {
    this.viewBoxesByStatus(BoxStatus.Dispatched);
  }

  loadQualityIssuesCount(): void {
    this.wirService.getQualityIssuesByProject(this.projectId).subscribe({
      next: (issues) => {
        this.qualityIssuesCount = issues.length;
        console.log('✅ Quality issues loaded:', this.qualityIssuesCount);
      },
      error: (err) => {
        console.error('❌ Error loading quality issues:', err);
        this.qualityIssuesCount = 0;
      }
    });
  }

  viewQualityIssues(): void {
    console.log('🔍 Navigate to quality issues for project:', this.projectId);
    if (!this.projectId) {
      console.error('❌ Cannot navigate: projectId is undefined');
      alert('Error: Project ID is missing. Cannot view quality issues.');
      return;
    }
    this.router.navigate(['/projects', this.projectId, 'quality-issues']);
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

    if (!this.projectId) {
      console.error('Project ID is required to download template');
      return;
    }

    this.templateDownloading = true;
    this.boxService.downloadBoxesTemplate(this.projectId).subscribe({
      next: (blob) => {
        const url = window.URL.createObjectURL(blob);
        const link = document.createElement('a');
        link.href = url;
        link.download = 'BoxesImportTemplate.xlsx';
        link.click();
        window.URL.revokeObjectURL(url);
        this.templateDownloading = false;
      },
      error: async (error) => {
        console.error('❌ Error downloading template:', error);
        this.templateDownloading = false;
        
        // When downloading blob, error.error is also a Blob (containing JSON)
        // We need to read it as text to get the actual error message
        if (error.error instanceof Blob) {
          try {
            const errorText = await error.error.text();
            const errorJson = JSON.parse(errorText);
            this.importErrorMessage = errorJson.message || errorJson.title || 'Unable to download template right now.';
            console.error('Parsed error:', errorJson);
          } catch (e) {
            console.error('Failed to parse error blob:', e);
            this.importErrorMessage = 'Unable to download template right now.';
          }
        } else {
          this.importErrorMessage = error?.error?.message || error?.message || 'Unable to download template right now.';
        }
        
        // Scroll to error message
        setTimeout(() => {
          this.excelSectionRef?.nativeElement?.scrollIntoView({ behavior: 'smooth', block: 'center' });
        }, 100);
      }
    });
  }

  onBrowseClick(): void {
    // Check if project is archived, on hold, or closed
    if (this.isProjectArchived) {
      document.dispatchEvent(new CustomEvent('app-toast', {
        detail: { 
          message: 'Cannot upload files. Archived projects are read-only and cannot be modified.',
          type: 'error' 
        }
      }));
      return;
    }
    
    if (this.isProjectOnHold || this.isProjectClosed) {
      const status = this.isProjectClosed ? 'closed' : 'on hold';
      document.dispatchEvent(new CustomEvent('app-toast', {
        detail: { 
          message: `Cannot upload files. Projects ${status} cannot be modified. Only project status changes are allowed.`,
          type: 'error' 
        }
      }));
      return;
    }

    // Try to get the file input element - use ViewChild first, fallback to querySelector
    let fileInput: HTMLInputElement | null = null;
    
    if (this.fileInputRef?.nativeElement) {
      fileInput = this.fileInputRef.nativeElement;
    } else {
      // Fallback: query the element within the excel section for better scoping
      const excelSection = this.excelSectionRef?.nativeElement;
      if (excelSection) {
        fileInput = excelSection.querySelector('input[type="file"][accept=".xlsx,.xls"]') as HTMLInputElement;
      } else {
        // Last resort: query globally
        fileInput = document.querySelector('input[type="file"][accept=".xlsx,.xls"]') as HTMLInputElement;
      }
    }

    if (!fileInput) {
      console.error('File input element not found');
      return;
    }

    fileInput.value = '';
    fileInput.click();
  }

  onFileChange(event: Event): void {
    // Check if project is archived, on hold, or closed
    if (this.isProjectArchived || this.isProjectOnHold || this.isProjectClosed) {
      const status = this.isProjectArchived ? 'archived' : (this.isProjectClosed ? 'closed' : 'on hold');
      const message = this.isProjectArchived 
        ? 'Cannot upload files. Archived projects are read-only and cannot be modified.'
        : `Cannot upload files. Projects ${status} cannot be modified. Only project status changes are allowed.`;
      document.dispatchEvent(new CustomEvent('app-toast', {
        detail: { 
          message: message,
          type: 'error' 
        }
      }));
      const input = event.target as HTMLInputElement;
      if (input) {
        input.value = '';
      }
      return;
    }
    
    const input = event.target as HTMLInputElement;
    if (input.files && input.files.length > 0) {
      this.setSelectedFile(input.files[0]);
    }
  }

  onDragOver(event: DragEvent): void {
    // Prevent drag & drop if project is archived, on hold, or closed
    if (this.isProjectArchived || this.isProjectOnHold || this.isProjectClosed) {
      event.preventDefault();
      return;
    }
    
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

    // Check if project is archived, on hold, or closed
    if (this.isProjectArchived) {
      document.dispatchEvent(new CustomEvent('app-toast', {
        detail: { 
          message: 'Cannot upload files. Archived projects are read-only and cannot be modified.',
          type: 'error' 
        }
      }));
      return;
    }
    
    if (this.isProjectOnHold || this.isProjectClosed) {
      const status = this.isProjectClosed ? 'closed' : 'on hold';
      document.dispatchEvent(new CustomEvent('app-toast', {
        detail: { 
          message: `Cannot upload files. Projects ${status} cannot be modified. Only project status changes are allowed.`,
          type: 'error' 
        }
      }));
      return;
    }

    if (event.dataTransfer && event.dataTransfer.files.length > 0) {
      this.setSelectedFile(event.dataTransfer.files[0]);
      event.dataTransfer.clearData();
    }
  }

  private setSelectedFile(file: File): void {
    // Check if project is archived, on hold, or closed
    if (this.isProjectArchived) {
      document.dispatchEvent(new CustomEvent('app-toast', {
        detail: { 
          message: 'Cannot upload files. Archived projects are read-only and cannot be modified.',
          type: 'error' 
        }
      }));
      return;
    }
    
    if (this.isProjectOnHold || this.isProjectClosed) {
      const status = this.isProjectClosed ? 'closed' : 'on hold';
      document.dispatchEvent(new CustomEvent('app-toast', {
        detail: { 
          message: `Cannot upload files. Projects ${status} cannot be modified. Only project status changes are allowed.`,
          type: 'error' 
        }
      }));
      return;
    }
    
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

    // Validate file size (10 MB limit)
    const maxSize = 10 * 1024 * 1024; // 10 MB in bytes
    if (this.selectedFile.size > maxSize) {
      this.importErrorMessage = 'File size exceeds the maximum limit of 10 MB. Please choose a smaller file.';
      return;
    }

    // Validate file type
    if (!this.isValidExcelFile(this.selectedFile)) {
      this.importErrorMessage = 'Invalid file type. Please upload an Excel file (.xlsx or .xls).';
      return;
    }

    this.importingExcel = true;
    this.importErrorMessage = '';
    this.importSuccessMessage = '';
    this.importResult = null;

    console.log('📤 Uploading file:', {
      name: this.selectedFile.name,
      size: this.selectedFile.size,
      type: this.selectedFile.type,
      projectId: this.projectId
    });

    this.boxService.importBoxesFromExcel(this.projectId, this.selectedFile).subscribe({
      next: (result) => {
        this.importingExcel = false;
        this.importResult = result;
        this.importSuccessMessage = `Import completed. ${result.successCount} boxes added, ${result.failureCount} failed.`;
        if (result.successCount > 0) {
          this.loadBoxesAndCalculateCounts();
        }
      },
      error: async (error) => {
        this.importingExcel = false;
        console.error('❌ Excel import failed:', error);
        
        let errorMessage = 'Failed to import Excel file. Please try again.';
        
        // Handle status 0 (network/CORS/connection issues)
        if (error.status === 0) {
          errorMessage = 'Network error: Unable to connect to the server. Please check your internet connection and ensure the server is running.';
        } else if (error.error) {
          // Try to extract error message from different response structures
          if (typeof error.error === 'string') {
            errorMessage = error.error;
          } else if (error.error.message) {
            errorMessage = error.error.message;
          } else if (error.error.errors && Array.isArray(error.error.errors) && error.error.errors.length > 0) {
            errorMessage = error.error.errors[0];
          } else if (error.error.title) {
            errorMessage = error.error.title;
          } else if (error.error instanceof Blob) {
            // Handle blob error response
            try {
              const text = await error.error.text();
              const errorJson = JSON.parse(text);
              errorMessage = errorJson.message || errorJson.title || errorMessage;
            } catch (e) {
              console.error('Failed to parse blob error:', e);
            }
          }
        } else if (error.message) {
          errorMessage = error.message;
        }
        
        this.importErrorMessage = errorMessage;
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
    
    // Check if project is archived, on hold, or closed
    if (this.isProjectArchived) {
      document.dispatchEvent(new CustomEvent('app-toast', {
        detail: { 
          message: 'Cannot edit project. Archived projects are read-only and cannot be modified.',
          type: 'error' 
        }
      }));
      return;
    }
    
    if (this.isProjectOnHold || this.isProjectClosed) {
      const status = this.isProjectClosed ? 'closed' : 'on hold';
      document.dispatchEvent(new CustomEvent('app-toast', {
        detail: { 
          message: `Cannot edit project. Projects ${status} cannot be modified. Only project status changes are allowed.`,
          type: 'error' 
        }
      }));
      return;
    }

    this.router.navigate(['/projects', this.projectId, 'edit']);
  }

  openDeleteConfirm(): void {
    if (this.isProjectArchived) {
      document.dispatchEvent(new CustomEvent('app-toast', {
        detail: { 
          message: 'Cannot delete project. Archived projects are read-only and cannot be deleted.',
          type: 'error' 
        }
      }));
      return;
    }
    this.showDeleteConfirm = true;
  }

  cancelDelete(): void {
    this.showDeleteConfirm = false;
  }

  deleteProject(): void {
    if (this.deleting || !this.projectId) {
      return;
    }
    
    if (this.isProjectArchived) {
      this.showDeleteConfirm = false;
      document.dispatchEvent(new CustomEvent('app-toast', {
        detail: { 
          message: 'Cannot delete project. Archived projects are read-only and cannot be deleted.',
          type: 'error' 
        }
      }));
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
    
    // Check if project is archived, on hold, or closed
    if (this.isProjectArchived) {
      document.dispatchEvent(new CustomEvent('app-toast', {
        detail: { 
          message: 'Cannot set compression start date. Archived projects are read-only and cannot be modified.',
          type: 'error' 
        }
      }));
      return;
    }
    
    if (this.isProjectOnHold || this.isProjectClosed) {
      const status = this.isProjectClosed ? 'closed' : 'on hold';
      document.dispatchEvent(new CustomEvent('app-toast', {
        detail: { 
          message: `Cannot set compression start date. Projects ${status} cannot be modified. Only project status changes are allowed.`,
          type: 'error' 
        }
      }));
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
