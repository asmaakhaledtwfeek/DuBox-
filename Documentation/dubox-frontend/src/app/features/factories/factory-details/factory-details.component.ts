import { Component, OnInit, OnDestroy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { FormControl, ReactiveFormsModule, FormsModule } from '@angular/forms';
import { debounceTime, distinctUntilChanged, skip } from 'rxjs/operators';
import { Subscription } from 'rxjs';
import { FactoryService, Factory, ProjectLocation } from '../../../core/services/factory.service';
import { BoxService } from '../../../core/services/box.service';
import { Box, BoxStatus } from '../../../core/models/box.model';
import { Project, ProjectStatus } from '../../../core/models/project.model';
import { ProjectService } from '../../../core/services/project.service';
import { HeaderComponent } from '../../../shared/components/header/header.component';
import { SidebarComponent } from '../../../shared/components/sidebar/sidebar.component';

@Component({
  selector: 'app-factory-details',
  standalone: true,
  imports: [CommonModule, RouterModule, ReactiveFormsModule, FormsModule, HeaderComponent, SidebarComponent],
  templateUrl: './factory-details.component.html',
  styleUrls: ['./factory-details.component.scss']
})
export class FactoryDetailsComponent implements OnInit, OnDestroy {
  factoryId: string = '';
  factory: Factory | null = null;
  boxes: Box[] = [];
  filteredBoxes: Box[] = [];
  loading = true;
  error = '';
  
  searchControl = new FormControl('');
  selectedStatus: BoxStatus | 'All' | 'CurrentOccupancy' = 'All';
  BoxStatus = BoxStatus;
  ProjectLocation = ProjectLocation; // Expose to template
  
  // Track active status card for visual indication
  activeStatusCard: BoxStatus | 'All' | 'CurrentOccupancy' | null = null;
  
  // New filter properties
  projects: Project[] = [];
  selectedProjectId: string = '';
  selectedBuilding: string = '';
  selectedLevel: string = '';
  selectedZone: string = '';
  
  // Available filter options
  availableBuildings: string[] = [];
  availableLevels: string[] = [];
  availableZones: string[] = [];
  
  private subscriptions: Subscription[] = [];

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private factoryService: FactoryService,
    private boxService: BoxService,
    private projectService: ProjectService
  ) {}

  ngOnInit(): void {
    this.factoryId = this.route.snapshot.params['id'];
    this.loadFactoryDetails();
    this.setupSearch();
  }
  
  ngOnDestroy(): void {
    this.subscriptions.forEach(sub => sub.unsubscribe());
  }

  loadFactoryDetails(): void {
    this.loading = true;
    this.error = '';
    
    // Load factory and boxes in parallel
    this.factoryService.getFactoryById(this.factoryId).subscribe({
      next: (factory) => {
        this.factory = factory;
        this.loadBoxes();
      },
      error: (err) => {
        this.error = err.error?.message || err.message || 'Failed to load factory details';
        this.loading = false;
        console.error('Error loading factory:', err);
      }
    });
  }

  /**
   * Extract unique projects from boxes and load their full details
   */
  loadProjectsFromBoxes(boxes: Box[]): void {
    // Extract unique project IDs from boxes
    const projectIdsSet = new Set<string>();
    boxes.forEach(box => {
      if (box.projectId) {
        projectIdsSet.add(box.projectId);
      }
    });

    const projectIds = Array.from(projectIdsSet);
    
    if (projectIds.length === 0) {
      this.projects = [];
      return;
    }

    // Load all projects and filter to only those that have boxes in this factory
    this.projectService.getProjects().subscribe({
      next: (allProjects) => {
        // Filter to only projects that have boxes in this factory
        // Also exclude archived projects
        this.projects = allProjects.filter(p => 
          projectIds.includes(p.id) && p.status !== ProjectStatus.Archived
        );
      },
      error: (err) => {
        console.error('Error loading projects:', err);
        // Fallback: create minimal project objects from box data
        this.createProjectsFromBoxes(boxes);
      }
    });
  }

  /**
   * Fallback: Create minimal project objects from box data if project service fails
   */
  private createProjectsFromBoxes(boxes: Box[]): void {
    const projectMap = new Map<string, Project>();
    
    boxes.forEach(box => {
      if (box.projectId && !projectMap.has(box.projectId)) {
        projectMap.set(box.projectId, {
          id: box.projectId,
          name: box.projectCode || 'Unknown Project',
          code: box.projectCode || '',
          location: '',
          status: (box.projectStatus as ProjectStatus) || ProjectStatus.Active,
          totalBoxes: 0,
          completedBoxes: 0,
          inProgressBoxes: 0,
          readyForDeliveryBoxes: 0,
          progress: 0
        });
      }
    });

    this.projects = Array.from(projectMap.values()).filter(p => p.status !== ProjectStatus.Archived);
  }

  loadBoxes(): void {
    // Request all boxes including dispatched ones for filtering
    this.boxService.getBoxesByFactory(this.factoryId, true).subscribe({
      next: (boxes) => {
        this.boxes = boxes;
        // Load projects based on boxes in this factory
        this.loadProjectsFromBoxes(boxes);
        this.updateFilterOptions();
        this.applyFilters();
        this.loading = false;
      },
      error: (err) => {
        this.error = err.error?.message || err.message || 'Failed to load boxes';
        this.loading = false;
        console.error('Error loading boxes:', err);
      }
    });
  }

  /**
   * Update available filter options based on boxes in the factory
   * Filters are cascading: Project -> Building -> Level/Zone
   */
  updateFilterOptions(): void {
    const buildingsSet = new Set<string>();
    const levelsSet = new Set<string>();
    const zonesSet = new Set<string>();

    // Start with all boxes, then filter by project if selected
    let boxesToUse = this.boxes;
    
    if (this.selectedProjectId) {
      boxesToUse = boxesToUse.filter(box => box.projectId === this.selectedProjectId);
    }

    // Further filter by building if selected
    if (this.selectedBuilding) {
      boxesToUse = boxesToUse.filter(box => box.buildingNumber === this.selectedBuilding);
    }

    // Collect unique values from filtered boxes
    boxesToUse.forEach(box => {
      if (box.buildingNumber) {
        buildingsSet.add(box.buildingNumber);
      }
      if (box.floor) {
        levelsSet.add(box.floor);
      }
      if (box.zone) {
        zonesSet.add(box.zone);
      }
    });

    this.availableBuildings = Array.from(buildingsSet).sort();
    this.availableLevels = Array.from(levelsSet).sort();
    this.availableZones = Array.from(zonesSet).sort();
  }

  /**
   * Handle project filter change - update dependent filters
   */
  onProjectChange(): void {
    // Reset dependent filters when project changes
    this.selectedBuilding = '';
    this.selectedLevel = '';
    this.selectedZone = '';
    
    // Update available filter options based on selected project
    this.updateFilterOptions();
    
    // Apply filters
    this.applyFilters();
  }

  /**
   * Handle building filter change - update level and zone filters
   */
  onBuildingChange(): void {
    // Reset level and zone when building changes
    this.selectedLevel = '';
    this.selectedZone = '';
    
    // Update available filter options
    this.updateFilterOptions();
    
    // Apply filters
    this.applyFilters();
  }

  /**
   * Handle level or zone filter change
   */
  onLevelOrZoneChange(): void {
    // Update available filter options (in case other filters need updating)
    this.updateFilterOptions();
    
    // Apply filters
    this.applyFilters();
  }

  private setupSearch(): void {
    this.subscriptions.push(
      this.searchControl.valueChanges
        .pipe(
          debounceTime(300),
          distinctUntilChanged()
        )
        .subscribe(() => {
          this.applyFilters();
        })
    );
  }

  applyFilters(): void {
    const searchTerm = this.searchControl.value?.toLowerCase() || '';
    let filtered = this.boxes;
    
    // Apply status filter first
    if (this.selectedStatus === 'CurrentOccupancy') {
      // Show both InProgress and Completed boxes for current occupancy
      filtered = filtered.filter(box => 
        box.status === BoxStatus.InProgress || box.status === BoxStatus.Completed
      );
    } else if (this.selectedStatus !== 'All') {
      filtered = filtered.filter(box => box.status === this.selectedStatus);
    } else {
      // When "All" is selected, exclude dispatched boxes from display by default
      filtered = filtered.filter(box => box.status !== BoxStatus.Dispatched);
    }
    
    // Apply project filter
    if (this.selectedProjectId) {
      filtered = filtered.filter(box => box.projectId === this.selectedProjectId);
    }

    // Apply building filter
    if (this.selectedBuilding) {
      filtered = filtered.filter(box => box.buildingNumber === this.selectedBuilding);
    }

    // Apply level filter (using floor field)
    if (this.selectedLevel) {
      filtered = filtered.filter(box => box.floor === this.selectedLevel);
    }

    // Apply zone filter
    if (this.selectedZone) {
      filtered = filtered.filter(box => box.zone === this.selectedZone);
    }
    
    // Apply search filter
    if (searchTerm) {
      filtered = filtered.filter(box => {
        return box.code?.toLowerCase().includes(searchTerm) ||
               box.name?.toLowerCase().includes(searchTerm) ||
               box.serialNumber?.toLowerCase().includes(searchTerm) ||
               box.type?.toLowerCase().includes(searchTerm);
      });
    }
    
    this.filteredBoxes = filtered;
  }

  /**
   * Clear all filters and reset to default view
   */
  clearFilters(): void {
    this.selectedProjectId = '';
    this.selectedBuilding = '';
    this.selectedLevel = '';
    this.selectedZone = '';
    this.selectedStatus = 'All';
    this.activeStatusCard = null;
    this.searchControl.setValue('');
    // Update filter options to show all available options
    this.updateFilterOptions();
    this.applyFilters();
  }

  onStatusChange(status: BoxStatus | 'All'): void {
    this.selectedStatus = status;
    // Clear active status card when "All" is selected
    this.activeStatusCard = status === 'All' ? null : status;
    this.applyFilters();
  }

  /**
   * Handle status card click - filter boxes by status
   */
  onStatusCardClick(status: BoxStatus): void {
    this.selectedStatus = status;
    this.activeStatusCard = status;
    this.applyFilters();
  }

  /**
   * Handle occupancy card click - show both InProgress and Completed boxes
   */
  onOccupancyCardClick(): void {
    this.selectedStatus = 'CurrentOccupancy';
    this.activeStatusCard = 'CurrentOccupancy';
    this.applyFilters();
  }

  goBack(): void {
    this.router.navigate(['/factories']);
  }

  viewBox(boxId: string, event?: Event): void {
    // Prevent event bubbling if event is provided
    if (event) {
      event.stopPropagation();
    }
    
    // Find the box to get projectId
    const box = this.boxes.find(b => b.id === boxId);
    if (box && box.projectId) {
      this.router.navigate(['/projects', box.projectId, 'boxes', boxId]);
    } else {
      console.error('Box not found or missing projectId:', boxId);
    }
  }

  getLocationLabel(location: ProjectLocation): string {
    switch (location) {
      case ProjectLocation.KSA:
        return 'KSA';
      case ProjectLocation.UAE:
        return 'UAE';
      default:
        return 'N/A';
    }
  }

  /**
   * Calculate current occupancy based on filtered boxes
   * Only counts boxes that are:
   * - InProgress or Completed
   * - From projects that are NOT OnHold, Closed, or Archived
   */
  getCurrentOccupancy(): number {
    return this.boxes.filter(box => {
      // Only count InProgress or Completed boxes
      const isActiveStatus = box.status === BoxStatus.InProgress || box.status === BoxStatus.Completed;
      
      if (!isActiveStatus) {
        return false;
      }

      // Exclude boxes from OnHold, Closed, or Archived projects
      const projectStatus = box.projectStatus;
      if (projectStatus === ProjectStatus.OnHold || 
          projectStatus === ProjectStatus.Closed || 
          projectStatus === ProjectStatus.Archived) {
        return false;
      }

      return true;
    }).length;
  }

  /**
   * Calculate available capacity based on current occupancy
   */
  getAvailableCapacity(): number {
    if (!this.factory || !this.factory.capacity) return 0;
    return Math.max(0, this.factory.capacity - this.getCurrentOccupancy());
  }

  getCapacityPercentage(): number {
    if (!this.factory || !this.factory.capacity || this.factory.capacity === 0) return 0;
    return Math.min(100, (this.getCurrentOccupancy() / this.factory.capacity) * 100);
  }

  getStatusClass(status: BoxStatus): string {
    const statusMap: Partial<Record<BoxStatus, string>> = {
      [BoxStatus.NotStarted]: 'status-not-started',
      [BoxStatus.InProgress]: 'status-in-progress',
      [BoxStatus.Completed]: 'status-completed',
      [BoxStatus.OnHold]: 'status-on-hold',
      [BoxStatus.Dispatched]: 'status-dispatched',
      [BoxStatus.QAReview]: 'status-qa-review',
      [BoxStatus.ReadyForDelivery]: 'status-ready',
      [BoxStatus.Delivered]: 'status-delivered'
    };
    return statusMap[status] || 'status-unknown';
  }

  getDispatchedCount(): number {
    // Use dispatched count from factory (includes all dispatched boxes, even from inactive projects)
    return this.factory?.dispatchedBoxesCount || 0;
  }

  getCompletedCount(): number {
    return this.boxes.filter(box => box.status === BoxStatus.Completed).length;
  }

  getInProgressCount(): number {
    return this.boxes.filter(box => box.status === BoxStatus.InProgress).length;
  }

  getNonDispatchedBoxCount(): number {
    // Return count of boxes excluding dispatched (for display purposes)
    // This should match what's shown in the boxes list
    return this.boxes.filter(box => box.status !== BoxStatus.Dispatched).length;
  }

  getStatusLabel(status: BoxStatus): string {
    const labels: Record<BoxStatus, string> = {
      [BoxStatus.NotStarted]: 'Not Started',
      [BoxStatus.InProgress]: 'In Progress',
      [BoxStatus.Completed]: 'Completed',
      [BoxStatus.OnHold]: 'On Hold',
      [BoxStatus.Dispatched]: 'Dispatched',
      [BoxStatus.QAReview]: 'QA Review',
      [BoxStatus.ReadyForDelivery]: 'Ready',
      [BoxStatus.Delivered]: 'Delivered'
    };
    return labels[status] || status;
  }

  navigateToBox(boxId: string): void {
    this.router.navigate(['/boxes', boxId]);
  }

  // Legend percentage calculations
  getInProgressPercentage(): number {
    if (this.boxes.length === 0) return 0;
    return Math.round((this.getInProgressCount() / this.boxes.length) * 100);
  }

  getCompletedPercentage(): number {
    if (this.boxes.length === 0) return 0;
    return Math.round((this.getCompletedCount() / this.boxes.length) * 100);
  }

  getEmptyPercentage(): number {
    if (!this.factory || !this.factory.capacity) return 0;
    const emptySpots = this.factory.availableCapacity || 0;
    return Math.round((emptySpots / this.factory.capacity) * 100);
  }
}

