import { Component, ElementRef, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { HttpClient } from '@angular/common/http';
import { Subscription, forkJoin, of } from 'rxjs';
import { skip, catchError } from 'rxjs/operators';
import { environment } from '../../../../environments/environment';
import { ProjectService } from '../../../core/services/project.service';
import { BoxService } from '../../../core/services/box.service';
import { WIRService } from '../../../core/services/wir.service';
import { PermissionService } from '../../../core/services/permission.service';
import { PanelTypeService } from '../../../core/services/panel-type.service';
import { BoxMaterialService } from '../../../core/services/box-material.service';
import { BoxTypeMaterialService } from '../../../core/services/box-type-material.service';
import { MaterialTemplateService } from '../../../core/services/material-template.service';
import { Project, ProjectStatus, getAvailableProjectStatuses, canChangeProjectStatus } from '../../../core/models/project.model';
import { Box, BoxImportResult, BoxStatus, PanelStatus } from '../../../core/models/box.model';
import { HeaderComponent } from '../../../shared/components/header/header.component';
import { SidebarComponent } from '../../../shared/components/sidebar/sidebar.component';
import { trigger, style, animate, transition } from '@angular/animations';
import { DateTimeDisplayPipe } from '../../../shared/pipes/date-time-display.pipe';
import { MaterialTemplate, ProjectMaterialTemplate, MaterialTemplateItem } from '../../../core/models/material-template.model';

@Component({
  selector: 'app-project-dashboard',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterModule, HeaderComponent, SidebarComponent, DateTimeDisplayPipe],
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
  banner: { message: string; type: 'success' | 'error' | 'warning' } | null = null;
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
  
  // Box Panels Excel
  boxPanelsExcelDownloading = false;
  boxPanelsExcelUploading = false;
  selectedPanelsFile: File | null = null;
  panelsImportSuccessMessage = '';
  panelsImportErrorMessage = '';
  panelsImportResult: { successCount: number; failureCount: number; errors: string[] } | null = null;

  dashboardData = {
    totalBoxes: 0,
    completedBoxes: 0,
    inProgressBoxes: 0,
    dispatchedBoxes: 0,
    notStarted: 0,
    onHold: 0,
    boxesReadyToStart: 0,
    boxesNotReadyToStart: 0
  };

  qualityIssuesCount = 0;
  panelTypesCount = 0;
  /** Count of all materials across all box types in this project (for Project Materials card) */
  projectMaterialsCount = 0;
  
  materialStatistics = {
    totalMaterials: 0,
    selectedMaterials: 0,
    materialsArrived: 0,
    materialsPending: 0
  };

  // Weather Report - Complete Interface
  weatherReport: {
    reportId?: string;
    projectId?: string;
    projectCode?: string;
    projectName?: string;
    reportDate?: Date;
    // Temperature
    currentTemperature?: number;
    minTemperature?: number;
    maxTemperature?: number;
    // Humidity
    humidity?: number;
    // Precipitation
    precipitationProbability?: number;
    precipitationAmount?: number;
    // Wind
    windSpeed?: number;
    windGust?: number;
    windDirection?: number;
    // Pressure
    pressure?: number;
    // Solar Radiation
    solarRadiation?: number;
    // Sun and Moon
    sunrise?: string;
    sunset?: string;
    moonrise?: string;
    moonset?: string;
    // Location
    latitude?: number;
    longitude?: number;
    elevation?: number;
    // Status
    description?: string;
    isFavorable?: boolean;
    alertMessage?: string;
    qualityIssueCreated?: boolean;
    createdDate?: Date;
  } | null = null;
  weatherLoading = false;
  
  // Building and Floor breakdown
  buildingFloorBreakdown: { 
    building: string; 
    totalBoxes: number;
    inProgressCount: number;
    completedCount: number;
    floors: { 
      floor: string; 
      boxCount: number;
      inProgressCount: number;
      completedCount: number;
    }[] 
  }[] = [];
  
  private subscriptions: Subscription[] = [];

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private http: HttpClient,
    private projectService: ProjectService,
    private boxService: BoxService,
    private wirService: WIRService,
    private permissionService: PermissionService,
    private panelTypeService: PanelTypeService,
    private boxMaterialService: BoxMaterialService,
    private boxTypeMaterialService: BoxTypeMaterialService,
    private materialTemplateService: MaterialTemplateService
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

        // Load panel types count
        this.loadPanelTypesCount();
        
        // Load project materials count (all box type materials)
        this.loadProjectMaterialsCount();
        
        // Load material statistics
        this.loadMaterialStatistics();

        // Load weather report
        this.loadWeatherReport();
      },
      error: (error) => {
        this.error = 'Failed to load project';
        this.loading = false;
        console.error('❌ Error loading project:', error);
      }
    });
  }

  loadBoxesAndCalculateCounts(): void {
    // Use countOnly to get status counts without loading all boxes
    this.boxService.getBoxesByProjectPaginated(this.projectId, { 
      countOnly: true, 
      page: 1, 
      pageSize: 1 
    }).subscribe({
      next: (response) => {
        console.log('✅ Status counts loaded:', response.statusCounts);
        
        const statusCounts = response.statusCounts;
        
        if (statusCounts) {
          this.dashboardData = {
            totalBoxes: response.totalCount,
            completedBoxes: statusCounts.completed,
            inProgressBoxes: statusCounts.inProgress,
            dispatchedBoxes: statusCounts.dispatched,
            notStarted: statusCounts.notStarted,
            onHold: statusCounts.onHold,
            boxesReadyToStart: statusCounts.readyToStart,
            boxesNotReadyToStart: statusCounts.notStarted // NotStarted boxes are boxes not ready to start
          };
        } else {
          // Fallback if statusCounts not available
          this.dashboardData = {
            totalBoxes: response.totalCount,
            completedBoxes: 0,
            inProgressBoxes: 0,
            dispatchedBoxes: 0,
            notStarted: 0,
            onHold: 0,
            boxesReadyToStart: 0,
            boxesNotReadyToStart: 0
          };
        }

        console.log('📊 Calculated box counts:', this.dashboardData);
        this.loading = false;
        
        // Load building/floor breakdown using efficient endpoint
        this.loadBuildingFloorBreakdown();
      },
      error: (err) => {
        console.error('❌ Error loading box counts:', err);
        // Fallback to project data if boxes can't be loaded
        if (this.project) {
          this.dashboardData = {
            totalBoxes: this.project.totalBoxes || 0,
            completedBoxes: this.project.completedBoxes || 0,
            inProgressBoxes: this.project.inProgressBoxes || 0,
            dispatchedBoxes: 0,
            notStarted: 0,
            onHold: 0,
            boxesReadyToStart: 0,
            boxesNotReadyToStart: 0
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
    
    // Load boxes if not already loaded (needed for status validation)
    if (this.boxes.length === 0) {
      this.loadBoxesForStatusCheck();
      return; // Will reopen modal after boxes are loaded
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
    console.log('🔍 Navigate to box type templates for project:', this.projectId);
    if (!this.projectId) {
      console.error('❌ Cannot navigate: projectId is undefined');
      alert('Error: Project ID is missing. Cannot view box types.');
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

  viewReadyToStartBoxes(): void {
    this.viewBoxesByStatus(BoxStatus.ReadyToStart);
  }

  viewNotReadyToStartBoxes(): void {
    this.viewBoxesByStatus(BoxStatus.NotStarted);
  }

  viewBoxTypeMaterials(): void {
    console.log('🔍 Navigate to box type materials for project:', this.projectId);
    if (!this.projectId) {
      console.error('❌ Cannot navigate: projectId is undefined');
      alert('Error: Project ID is missing. Cannot view box type materials.');
      return;
    }
    this.router.navigate(['/projects', this.projectId, 'box-type-materials']);
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

  loadPanelTypesCount(): void {
    this.panelTypeService.getPanelTypesByProject(this.projectId).subscribe({
      next: (response) => {
        console.log('📦 Panel types response:', response);
        const types = response?.data || response || [];
        this.panelTypesCount = Array.isArray(types) ? types.length : 0;
        console.log('✅ Panel types loaded:', this.panelTypesCount);
      },
      error: (err) => {
        console.error('❌ Error loading panel types:', err);
        this.panelTypesCount = 0;
      }
    });
  }

  /**
   * Load count of all materials across all box types in the project (for Project Materials card).
   */
  loadProjectMaterialsCount(): void {
    this.boxTypeMaterialService.getProjectBoxTypeMaterials(this.projectId).subscribe({
      next: (materials) => {
        this.projectMaterialsCount = Array.isArray(materials) ? materials.length : 0;
        console.log('✅ Project materials count loaded:', this.projectMaterialsCount);
      },
      error: (err) => {
        console.error('❌ Error loading project materials count:', err);
        this.projectMaterialsCount = 0;
      }
    });
  }

  loadMaterialStatistics(): void {
    // Load material templates assigned to this project
    // Count materials from templates, not individual box type materials
    this.materialTemplateService.getProjectTemplates(this.projectId).subscribe({
      next: (projectTemplates) => {
        console.log('📦 Project templates loaded:', projectTemplates.length, 'templates');
        
        // If no templates assigned, set to zero
        if (projectTemplates.length === 0) {
          this.materialStatistics.selectedMaterials = 0;
          this.materialStatistics.totalMaterials = 0;
          this.materialStatistics.materialsArrived = 0;
          this.materialStatistics.materialsPending = 0;
          console.log('ℹ️ No templates assigned to project');
          return;
        }
        
        // Load all template details in parallel using forkJoin
        const templateRequests = projectTemplates.map((assignment: ProjectMaterialTemplate) => 
          this.materialTemplateService.getTemplateById(assignment.materialTemplateId).pipe(
            catchError(err => {
              console.error('❌ Error loading template:', assignment.materialTemplateId, err);
              return of(null); // Return null for failed requests
            })
          )
        );
        
        forkJoin<(MaterialTemplate | null)[]>(templateRequests).subscribe({
          next: (templates: (MaterialTemplate | null)[]) => {
            // Get all unique materials from all assigned templates
            const allMaterialIds = new Set<string>();
            let totalMaterialsFromTemplates = 0;
            
            templates.forEach((template: MaterialTemplate | null) => {
              if (template && template.items) {
                template.items.forEach((item: MaterialTemplateItem) => {
                  allMaterialIds.add(item.materialId);
                  totalMaterialsFromTemplates++;
                });
              }
            });
            
            // Update statistics
            this.materialStatistics.selectedMaterials = allMaterialIds.size;
            this.materialStatistics.totalMaterials = totalMaterialsFromTemplates;
            this.materialStatistics.materialsArrived = 0; // Not tracked at template level
            this.materialStatistics.materialsPending = 0; // Not tracked at template level
            
            console.log('✅ Material statistics from templates:', {
              templatesCount: projectTemplates.length,
              uniqueMaterials: this.materialStatistics.selectedMaterials,
              totalMaterialsInTemplates: this.materialStatistics.totalMaterials,
              templatesProcessed: templates.filter((t: MaterialTemplate | null) => t !== null).length
            });
          },
          error: (err) => {
            console.error('❌ Error loading template details:', err);
            this.materialStatistics = {
              totalMaterials: 0,
              selectedMaterials: 0,
              materialsArrived: 0,
              materialsPending: 0
            };
          }
        });
      },
      error: (err) => {
        console.error('❌ Error loading project templates:', err);
        // Set to zero if templates can't be loaded
        this.materialStatistics = {
          totalMaterials: 0,
          selectedMaterials: 0,
          materialsArrived: 0,
          materialsPending: 0
        };
      }
    });
  }

  loadWeatherReport(): void {
    this.weatherLoading = true;
    this.http.get<any>(`${environment.apiUrl}/projects/weather/${this.projectId}`).subscribe({
      next: (response) => {
        if (response?.isSuccess && response?.data) {
          this.weatherReport = response.data;
          console.log('✅ Weather report loaded:', this.weatherReport);
        } else {
          this.weatherReport = null;
          console.log('ℹ️ No weather report available for today');
        }
        this.weatherLoading = false;
      },
      error: (err) => {
        console.error('❌ Error loading weather report:', err);
        this.weatherReport = null;
        this.weatherLoading = false;
      }
    });
  }

  getWeatherIcon(): string {
    if (!this.weatherReport || !this.weatherReport.description) {
      return '☀️';
    }
    
    const desc = this.weatherReport.description.toLowerCase();
    if (desc.includes('rain') || desc.includes('drizzle')) return '🌧️';
    if (desc.includes('cloud')) return '☁️';
    if (desc.includes('clear') || desc.includes('sun')) return '☀️';
    if (desc.includes('storm') || desc.includes('thunder')) return '⛈️';
    if (desc.includes('snow')) return '🌨️';
    if (desc.includes('wind')) return '💨';
    return '🌤️';
  }

  formatTime(dateString?: string): string {
    if (!dateString) return 'N/A';
    const date = new Date(dateString);
    return date.toLocaleTimeString('en-US', { hour: '2-digit', minute: '2-digit', hour12: false });
  }

  getWindDirectionText(degrees: number | null | undefined): string {
    if (degrees === null || degrees === undefined) {
      return 'N/A';
    }
    
    const directions = ['N', 'NNE', 'NE', 'ENE', 'E', 'ESE', 'SE', 'SSE', 'S', 'SSW', 'SW', 'WSW', 'W', 'WNW', 'NW', 'NNW'];
    const index = Math.round((degrees % 360) / 22.5) % 16;
    return directions[index];
  }

  getWeatherIconSvg(): string {
    if (!this.weatherReport || !this.weatherReport.description) {
      return 'clear';
    }
    
    const desc = this.weatherReport.description.toLowerCase();
    if (desc.includes('rain') || desc.includes('drizzle')) return 'rain';
    if (desc.includes('cloud') || desc.includes('overcast')) return 'cloudy';
    if (desc.includes('clear')) return 'clear';
    if (desc.includes('storm') || desc.includes('thunder')) return 'storm';
    if (desc.includes('snow')) return 'snow';
    if (desc.includes('mist') || desc.includes('fog')) return 'mist';
    return 'clear';
  }

  viewProjectMaterials(): void {
    console.log('🔍 Navigate to project materials for project:', this.projectId);
    if (!this.projectId) {
      console.error('❌ Cannot navigate: projectId is undefined');
      alert('Error: Project ID is missing. Cannot view materials.');
      return;
    }
    this.router.navigate(['/projects', this.projectId, 'materials']);
  }

  /**
   * Load building/floor breakdown using efficient endpoint (no box data loaded)
   */
  private loadBuildingFloorBreakdown(): void {
    this.boxService.getBuildingFloorBreakdown(this.projectId).subscribe({
      next: (response) => {
        this.buildingFloorBreakdown = response.buildings.map(building => ({
          building: building.building,
          totalBoxes: building.totalBoxes,
          inProgressCount: building.inProgressCount || 0,
          completedCount: building.completedCount || 0,
          floors: building.floors.map(floor => ({
            floor: floor.floor,
            boxCount: floor.boxCount,
            inProgressCount: floor.inProgressCount || 0,
            completedCount: floor.completedCount || 0
          }))
        }));
        console.log('🏢 Building/Floor breakdown loaded:', this.buildingFloorBreakdown);
      },
      error: (err) => {
        console.error('❌ Error loading building/floor breakdown:', err);
        this.buildingFloorBreakdown = [];
      }
    });
  }

  /**
   * Load boxes for status validation (only when needed)
   */
  private loadBoxesForStatusCheck(): void {
    console.log('📦 Loading boxes for status validation...');
    this.boxService.getBoxesByProject(this.projectId).subscribe({
      next: (boxes) => {
        this.boxes = boxes;
        console.log('✅ Boxes loaded for status check:', boxes.length);
        // Reopen modal now that boxes are loaded
        this.openStatusModal();
      },
      error: (err) => {
        console.error('❌ Error loading boxes for status check:', err);
        // Continue with empty boxes array
        this.boxes = [];
        this.openStatusModal();
      }
    });
  }

  /**
   * Navigate to boxes list filtered by building
   */
  viewBoxesByBuilding(building: string): void {
    this.router.navigate(['/projects', this.projectId, 'boxes'], {
      queryParams: { building: building }
    });
  }

  /**
   * Navigate to boxes list filtered by building and floor
   */
  viewBoxesByBuildingAndFloor(building: string, floor: string): void {
    this.router.navigate(['/projects', this.projectId, 'boxes'], {
      queryParams: { building: building, floor: floor }
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

  viewPanelTypes(): void {
    console.log('🔍 Navigate to panel types for project:', this.projectId);
    if (!this.projectId) {
      console.error('❌ Cannot navigate: projectId is undefined');
      alert('Error: Project ID is missing. Cannot view panel types.');
      return;
    }
    this.router.navigate(['/projects', this.projectId, 'panel-types']);
  }

  navigateToAIPanelExtraction(): void {
    console.log('🤖 Navigate to AI Panel Extraction for project:', this.projectId);
    if (!this.projectId) {
      console.error('❌ Cannot navigate: projectId is undefined');
      alert('Error: Project ID is missing. Cannot extract panels.');
      return;
    }
    this.router.navigate(['/projects', this.projectId, 'panels', 'extract']);
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
      next: (response) => {
        const url = window.URL.createObjectURL(response.blob);
        const link = document.createElement('a');
        link.href = url;
        link.download = response.filename;
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
        
        // Check if project was archived instead of deleted
        if (message.includes('moved to archived')) {
          this.showBanner(message, 'warning');
          // Reload project data to show updated archived status
          setTimeout(() => {
            this.loadProject();
          }, 1500);
        } else {
          this.showBanner(message, 'error');
          console.error('❌ Error deleting project:', err);
        }
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

  private showBanner(message: string, type: 'success' | 'error' | 'warning'): void {
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

  /**
   * Get normalized contractor logo URL
   */
  getContractorLogoUrl(): string | null {
    return this.normalizeImageUrl(this.project?.contractorImageUrl);
  }

  /**
   * Get normalized sub-contractor logo URL
   */
  getSubContractorLogoUrl(): string | null {
    return this.normalizeImageUrl(this.project?.subContractorImageUrl);
  }

  /**
   * Get normalized client logo URL
   */
  getClientLogoUrl(): string | null {
    return this.normalizeImageUrl(this.project?.clientImageUrl);
  }

  /**
   * Check if project has any logos
   */
  hasAnyLogo(): boolean {
    return !!(this.getContractorLogoUrl() || this.getSubContractorLogoUrl() || this.getClientLogoUrl());
  }

  /**
   * Handle logo loading error
   */
  onLogoError(logoType: 'contractor' | 'subContractor' | 'client'): void {
    console.error(`❌ Failed to load ${logoType} logo`);
    // Clear the failed logo from project object to hide it
    if (this.project) {
      if (logoType === 'contractor') {
        this.project.contractorImageUrl = undefined;
      } else if (logoType === 'subContractor') {
        this.project.subContractorImageUrl = undefined;
      } else if (logoType === 'client') {
        this.project.clientImageUrl = undefined;
      }
    }
  }

  /**
   * Normalize image URL - handles relative URLs, absolute URLs, and empty values
   */
  private normalizeImageUrl(url: string | null | undefined): string | null {
    if (!url || url.trim() === '') {
      return null;
    }

    const trimmedUrl = url.trim();
    
    // If it's a relative URL starting with /api/ or just /, convert to absolute URL
    if (trimmedUrl.startsWith('/api/') || (trimmedUrl.startsWith('/') && !trimmedUrl.startsWith('http'))) {
      const baseUrl = `${window.location.protocol}//${window.location.host}`;
      return `${baseUrl}${trimmedUrl}`;
    }
    
    // If it's already an absolute URL (starts with http:// or https://), return as-is
    if (trimmedUrl.startsWith('http://') || trimmedUrl.startsWith('https://')) {
      return trimmedUrl;
    }
    
    // For any other URL format, return as is (might be a blob URL or other valid URL)
    return trimmedUrl;
  }

  downloadBoxPanelsExcel(): void {
    if (this.boxPanelsExcelDownloading || !this.projectId) {
      return;
    }

    // Validate projectId is a valid GUID format
    const guidPattern = /^[0-9a-f]{8}-[0-9a-f]{4}-[0-9a-f]{4}-[0-9a-f]{4}-[0-9a-f]{12}$/i;
    if (!guidPattern.test(this.projectId)) {
      this.panelsImportErrorMessage = 'Invalid project ID format. Please refresh the page and try again.';
      return;
    }

    this.boxPanelsExcelDownloading = true;
    this.panelsImportErrorMessage = '';
    this.panelsImportSuccessMessage = '';

    this.projectService.downloadBoxPanelsExcel(this.projectId).subscribe({
      next: (blob) => {
        // Check if the blob is actually an error response (small size or wrong content type)
        if (blob.size < 100) {
          // Likely an error response, try to parse it
          blob.text().then(text => {
            try {
              const errorJson = JSON.parse(text);
              this.panelsImportErrorMessage = errorJson.message || errorJson.title || 'Failed to generate Excel file.';
            } catch (e) {
              this.panelsImportErrorMessage = 'Failed to generate Excel file. The project may not have any box panels.';
            }
            this.boxPanelsExcelDownloading = false;
          });
          return;
        }

        const url = window.URL.createObjectURL(blob);
        const link = document.createElement('a');
        link.href = url;
        link.download = `${this.project?.code || 'project'}-BoxPanels.xlsx`;
        link.click();
        window.URL.revokeObjectURL(url);
        this.boxPanelsExcelDownloading = false;
        this.panelsImportSuccessMessage = 'Excel file downloaded successfully';
        setTimeout(() => {
          this.panelsImportSuccessMessage = '';
        }, 3000);
      },
      error: async (error) => {
        console.error('❌ Error downloading box panels Excel:', error);
        this.boxPanelsExcelDownloading = false;
        
        let errorMessage = 'Unable to download Excel file.';
        
        // Handle error response (could be from downloadWithResponse)
        if (error.body instanceof Blob) {
          try {
            const errorText = await error.body.text();
            const errorJson = JSON.parse(errorText);
            errorMessage = errorJson.message || errorJson.title || errorJson.errors?.[0] || errorMessage;
          } catch (e) {
            // If parsing fails, use status-based message
            if (error.status === 400) {
              errorMessage = 'Bad request. The project may not exist or may not have any box panels.';
            } else if (error.status === 404) {
              errorMessage = 'Project not found.';
            } else if (error.status === 401) {
              errorMessage = 'Unauthorized. Please log in again.';
            } else if (error.status === 500) {
              errorMessage = 'Server error. Please try again later.';
            }
          }
        } else if (error.error instanceof Blob) {
          try {
            const errorText = await error.error.text();
            const errorJson = JSON.parse(errorText);
            errorMessage = errorJson.message || errorJson.title || errorJson.errors?.[0] || errorMessage;
          } catch (e) {
            // If parsing fails, check error status
            if (error.status === 400) {
              errorMessage = 'Bad request. The project may not exist or may not have any box panels.';
            } else if (error.status === 404) {
              errorMessage = 'Project not found.';
            } else if (error.status === 401) {
              errorMessage = 'Unauthorized. Please log in again.';
            } else if (error.status === 500) {
              errorMessage = 'Server error. Please try again later.';
            }
          }
        } else if (error.error) {
          // Try to extract error message from various possible formats
          errorMessage = error.error.message || 
                       error.error.title || 
                       error.error.errors?.[0] || 
                       (typeof error.error === 'string' ? error.error : errorMessage);
        } else if (error.status === 400) {
          errorMessage = 'Bad request. The project may not exist or may not have any box panels.';
        } else if (error.status === 404) {
          errorMessage = 'Project not found.';
        } else if (error.status === 401) {
          errorMessage = 'Unauthorized. Please log in again.';
        } else if (error.status === 500) {
          errorMessage = 'Server error. Please try again later.';
        }
        
        this.panelsImportErrorMessage = errorMessage;
        setTimeout(() => {
          this.panelsImportErrorMessage = '';
        }, 5000);
      }
    });
  }

  onPanelsFileChange(event: Event): void {
    const input = event.target as HTMLInputElement;
    if (input?.files && input.files.length > 0) {
      this.selectedPanelsFile = input.files[0];
      this.panelsImportErrorMessage = '';
      this.panelsImportSuccessMessage = '';
      this.panelsImportResult = null;
    }
  }

  removeSelectedPanelsFile(): void {
    this.selectedPanelsFile = null;
    this.panelsImportErrorMessage = '';
    this.panelsImportSuccessMessage = '';
    this.panelsImportResult = null;
  }

  uploadBoxPanelsExcel(): void {
    if (!this.selectedPanelsFile || !this.projectId || this.boxPanelsExcelUploading) {
      return;
    }

    this.boxPanelsExcelUploading = true;
    this.panelsImportErrorMessage = '';
    this.panelsImportSuccessMessage = '';
    this.panelsImportResult = null;

    this.projectService.uploadBoxPanelsExcel(this.projectId, this.selectedPanelsFile).subscribe({
      next: (result) => {
        this.boxPanelsExcelUploading = false;
        this.panelsImportResult = result;
        
        if (result.failureCount === 0) {
          this.panelsImportSuccessMessage = `Successfully imported ${result.successCount} box panel(s).`;
          this.selectedPanelsFile = null;
        } else {
          this.panelsImportSuccessMessage = `Imported ${result.successCount} box panel(s) with ${result.failureCount} error(s).`;
        }
      },
      error: (error) => {
        console.error('❌ Error uploading box panels Excel:', error);
        this.boxPanelsExcelUploading = false;
        this.panelsImportErrorMessage = error?.error?.message || error?.message || 'Failed to upload Excel file.';
      }
    });
  }
}
