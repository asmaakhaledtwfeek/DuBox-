import { Component, OnInit, OnDestroy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { FormControl, ReactiveFormsModule, FormsModule } from '@angular/forms';
import { debounceTime, distinctUntilChanged, map, catchError, skip } from 'rxjs/operators';
import { forkJoin, of, Subscription } from 'rxjs';
import { BoxService } from '../../../core/services/box.service';
import { PermissionService } from '../../../core/services/permission.service';
import { Box, BoxStatus, BoxTypeStat } from '../../../core/models/box.model';
import { ProjectService } from '../../../core/services/project.service';
import { HeaderComponent } from '../../../shared/components/header/header.component';
import { SidebarComponent } from '../../../shared/components/sidebar/sidebar.component';

@Component({
  selector: 'app-boxes-list',
  standalone: true,
  imports: [CommonModule, RouterModule, ReactiveFormsModule, FormsModule, HeaderComponent, SidebarComponent],
  templateUrl: './boxes-list.component.html',
  styleUrls: ['./boxes-list.component.scss']
})
export class BoxesListComponent implements OnInit, OnDestroy {
  projectId: string = '';
  projectName = '';
  projectCode = '';
  project: any = null; // Store project details
  isProjectArchived = false; // Track if project is archived
  isProjectOnHold = false; // Track if project is on hold
  isProjectClosed = false; // Track if project is closed
  boxes: Box[] = [];
  filteredBoxes: Box[] = [];
  boxTypes: BoxTypeStat[] = [];
  filteredBoxTypes: BoxTypeStat[] = [];
  selectedBoxType: string | null = null;
  selectedBoxSubType: string | null = null;
  showBoxTypes = true;
  loading = true;
  error = '';
  canCreate = false;
  showDuplicateConfirm = false;
  boxToDuplicate: Box | null = null;
  duplicating = false;
  
  searchControl = new FormControl('');
  boxTypeSearchControl = new FormControl('');
  selectedStatus: BoxStatus | 'All' = BoxStatus.InProgress;
  BoxStatus = BoxStatus;
  
  // Filters for box types page
  selectedFilterBoxType: string = '';
  selectedFilterSubType: string = '';
  selectedFilterBuilding: string = '';
  selectedFilterFloor: string = '';
  selectedFilterZone: string = '';
  
  // Available filter options for box types page
  availableBoxTypes: string[] = [];
  availableSubTypes: string[] = [];
  availableBuildings: string[] = [];
  availableFloors: string[] = [];
  availableZones: string[] = [];
  
  // Counts for each filter option
  boxTypeCounts: Map<string, number> = new Map();
  subTypeCounts: Map<string, number> = new Map();
  buildingCounts: Map<string, number> = new Map();
  floorCounts: Map<string, number> = new Map();
  zoneCounts: Map<string, number> = new Map();
  
  // Store all boxes for cascading filter calculations
  allProjectBoxes: Box[] = [];
  
  // Filters for boxes list page
  selectedBoxSubTypeFilter: string = '';
  selectedBoxBuildingFilter: string = '';
  selectedBoxFloorFilter: string = '';
  selectedBoxZoneFilter: string = '';
  
  // Available filter options for boxes list page
  availableBoxSubTypes: string[] = [];
  availableBoxBuildings: string[] = [];
  availableBoxFloors: string[] = [];
  availableBoxZones: string[] = [];
  
  // Counts for boxes list page filters
  boxSubTypeCounts: Map<string, number> = new Map();
  boxBuildingCounts: Map<string, number> = new Map();
  boxFloorCounts: Map<string, number> = new Map();
  boxZoneCounts: Map<string, number> = new Map();
  
  private subscriptions: Subscription[] = [];

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private boxService: BoxService,
    private permissionService: PermissionService,
    private projectService: ProjectService
  ) {}

  ngOnInit(): void {
    this.projectId = this.route.snapshot.params['id'];
    const boxType = this.route.snapshot.queryParams['boxType'];
    const boxSubType = this.route.snapshot.queryParams['boxSubType'];
    const status = this.route.snapshot.queryParams['status'];
    const building = this.route.snapshot.queryParams['building'];
    const floor = this.route.snapshot.queryParams['floor'];
    
    // Set status filter if provided in query params
    if (status && Object.values(BoxStatus).includes(status as BoxStatus)) {
      this.selectedStatus = status as BoxStatus;
    }
    
    // Set building/floor filters if provided in query params
    if (building) {
      this.selectedFilterBuilding = building;
    }
    if (floor) {
      this.selectedFilterFloor = floor;
    }
    
    // Check permissions immediately
    this.checkPermissions();
    
    // Subscribe to permission changes to update UI when permissions are loaded
    this.subscriptions.push(
      this.permissionService.permissions$
        .pipe(skip(1)) // Skip initial empty value
        .subscribe(() => {
          console.log('🔄 Permissions updated, re-checking boxes permissions');
          this.checkPermissions();
        })
    );
    
    this.loadProjectDetails();
    
    if (boxType || status) {
      // If status is provided without boxType, load all boxes
      if (status && !boxType) {
        this.showBoxTypes = false;
        this.loadBoxes();
      } else if (boxType) {
        this.selectedBoxType = boxType;
        this.selectedBoxSubType = boxSubType || null;
        this.showBoxTypes = false;
        this.loadBoxes();
      }
    } else {
      this.loadBoxTypes();
    }
    
    this.setupSearch();
  }
  
  ngOnDestroy(): void {
    this.subscriptions.forEach(sub => sub.unsubscribe());
  }
  
  private checkPermissions(): void {
    const baseCanCreate = this.permissionService.canCreate('boxes');
    // Disable create if project is archived or on hold
    this.canCreate = baseCanCreate && !this.isProjectArchived && !this.isProjectOnHold && !this.isProjectClosed;
    console.log('✅ Can create box:', this.canCreate, 'Is Project Archived:', this.isProjectArchived, 'Is OnHold:', this.isProjectOnHold);
  }

  loadProjectDetails(): void {
    if (!this.projectId) {
      return;
    }

    this.projectService.getProject(this.projectId).subscribe({
      next: (project) => {
        this.project = project;
        this.projectName = project.name || '';
        this.projectCode = project.code || '';
        this.isProjectArchived = project.status === 'Archived';
        this.isProjectOnHold = project.status === 'OnHold';
        this.isProjectClosed = project.status === 'Closed';
        // Re-check permissions after loading project status
        this.checkPermissions();
        console.log('📁 Project loaded. Status:', project.status, 'Is Archived:', this.isProjectArchived, 'Is OnHold:', this.isProjectOnHold, 'Is Closed:', this.isProjectClosed);
      },
      error: (err) => {
        console.error('Error loading project details:', err);
      }
    });
  }

  private setupSearch(): void {
    this.searchControl.valueChanges
      .pipe(
        debounceTime(300),
        distinctUntilChanged()
      )
      .subscribe(() => {
        this.applyFilters();
      });

    // Setup box type search
    this.boxTypeSearchControl.valueChanges
      .pipe(
        debounceTime(300),
        distinctUntilChanged()
      )
      .subscribe(() => {
        this.applyBoxTypeFilters();
      });
  }

  loadBoxTypes(): void {
    this.loading = true;
    this.error = '';
    this.showBoxTypes = true;
    this.selectedBoxType = null;
    
    // Load both box types and boxes in parallel for faster loading
    forkJoin({
      boxTypes: this.boxService.getBoxTypeStatsByProject(this.projectId),
      boxes: this.boxService.getBoxesByProject(this.projectId)
    }).subscribe({
      next: (result) => {
        this.boxTypes = result.boxTypes.boxTypeStats || [];
        this.filteredBoxTypes = [...this.boxTypes];
        
        // Store all boxes and calculate filter options immediately
        this.allProjectBoxes = result.boxes;
        this.recalculateFilterCounts(result.boxes);
        
        this.applyBoxTypeFilters();
        this.loading = false;
      },
      error: (err) => {
        this.error = err.message || 'Failed to load box types';
        this.loading = false;
        console.error('Error loading box types:', err);
      }
    });
  }
  
  /**
   * Recalculate filter counts with proper one-way cascading (parent → child only)
   * - Box Type filters SubTypes (not reverse)
   * - Building filters Floors (not reverse)
   * - Floor filters Zones (not reverse)
   */
  private recalculateFilterCounts(boxes: Box[]): void {
    // Clear existing counts
    this.boxTypeCounts.clear();
    this.subTypeCounts.clear();
    this.buildingCounts.clear();
    this.floorCounts.clear();
    this.zoneCounts.clear();
    
    // Extract box types and count them
    const boxTypeMap = new Map<string, number>();
    const subTypeMap = new Map<string, number>();
    const buildingMap = new Map<string, number>();
    const floorMap = new Map<string, number>();
    const zoneMap = new Map<string, number>();
    
    // Always count from ALL boxes for parent filters
    this.allProjectBoxes.forEach(box => {
      const parts = (box.code || '').split('-');
      
      // Always show all box types
      if (parts.length >= 4 && parts[3]) {
        const boxType = parts[3];
        boxTypeMap.set(boxType, (boxTypeMap.get(boxType) || 0) + 1);
      }
      
      // Always show all buildings
      if (box.buildingNumber) {
        buildingMap.set(box.buildingNumber, (buildingMap.get(box.buildingNumber) || 0) + 1);
      }
    });
    
    // For child filters, use the filtered boxes for cascading
    // SubTypes: filtered by Box Type only
    let subTypeFilteredBoxes = this.allProjectBoxes;
    if (this.selectedFilterBoxType) {
      subTypeFilteredBoxes = this.allProjectBoxes.filter(box => {
        const parts = (box.code || '').split('-');
        const boxType = parts.length >= 4 ? parts[3] : '';
        return boxType === this.selectedFilterBoxType;
      });
    }
    
    subTypeFilteredBoxes.forEach(box => {
      const parts = (box.code || '').split('-');
      if (parts.length >= 5 && parts[4]) {
        const subType = parts[4];
        subTypeMap.set(subType, (subTypeMap.get(subType) || 0) + 1);
      }
    });
    
    // Floors: filtered by Building only
    let floorFilteredBoxes = this.allProjectBoxes;
    if (this.selectedFilterBuilding) {
      floorFilteredBoxes = this.allProjectBoxes.filter(box => 
        box.buildingNumber === this.selectedFilterBuilding
      );
      }
      
    floorFilteredBoxes.forEach(box => {
      if (box.floor) {
        floorMap.set(box.floor, (floorMap.get(box.floor) || 0) + 1);
      }
    });
      
    // Zones: filtered by Building and Floor
    let zoneFilteredBoxes = this.allProjectBoxes;
    if (this.selectedFilterBuilding) {
      zoneFilteredBoxes = zoneFilteredBoxes.filter(box => 
        box.buildingNumber === this.selectedFilterBuilding
      );
    }
    if (this.selectedFilterFloor) {
      zoneFilteredBoxes = zoneFilteredBoxes.filter(box => 
        box.floor === this.selectedFilterFloor
      );
    }
    
    zoneFilteredBoxes.forEach(box => {
      if (box.zone) {
        zoneMap.set(box.zone, (zoneMap.get(box.zone) || 0) + 1);
      }
    });
    
    // Only show options that exist in the filtered data (count > 0)
    this.availableBoxTypes = Array.from(boxTypeMap.keys()).filter(k => boxTypeMap.get(k)! > 0).sort();
    this.availableSubTypes = Array.from(subTypeMap.keys()).filter(k => subTypeMap.get(k)! > 0).sort();
    this.availableBuildings = Array.from(buildingMap.keys()).filter(k => buildingMap.get(k)! > 0).sort();
    this.availableFloors = Array.from(floorMap.keys()).filter(k => floorMap.get(k)! > 0).sort();
    this.availableZones = Array.from(zoneMap.keys()).filter(k => zoneMap.get(k)! > 0).sort();
    
    // Store counts
    this.boxTypeCounts = boxTypeMap;
    this.subTypeCounts = subTypeMap;
    this.buildingCounts = buildingMap;
    this.floorCounts = floorMap;
    this.zoneCounts = zoneMap;
    
    console.log('📊 Filter counts recalculated (one-way cascading):', {
      totalBoxes: this.allProjectBoxes.length,
      counts: {
        boxTypeCounts: Array.from(this.boxTypeCounts.entries()),
        subTypeCounts: Array.from(this.subTypeCounts.entries()),
        buildingCounts: Array.from(this.buildingCounts.entries()),
        floorCounts: Array.from(this.floorCounts.entries()),
        zoneCounts: Array.from(this.zoneCounts.entries())
      }
    });
  }

  loadBoxes(): void {
    this.loading = true;
    this.error = '';
    
    this.boxService.getBoxesByProject(this.projectId).subscribe({
      next: (boxes) => {
        // Filter boxes by selected type if a type is selected
        if (this.selectedBoxType) {
          // Parse BoxTag to extract type and subtype abbreviations
          // BoxTag format: ProjectNumber-Building-Floor-Type-SubType
          this.boxes = boxes.filter(box => {
            const parts = (box.code || '').split('-');
            // Type is at position 3 (index 3), SubType is at position 4 (index 4)
            const boxType = parts.length >= 4 ? parts[3] : '';
            const boxSubType = parts.length >= 5 ? parts[4] : '';
            
            // Filter by type and subtype (if subtype is selected)
            const typeMatches = boxType === this.selectedBoxType;
            const subTypeMatches = !this.selectedBoxSubType || boxSubType === this.selectedBoxSubType;
            
            return typeMatches && subTypeMatches;
          });
          console.log(`🔍 Filtering boxes by type "${this.selectedBoxType}"${this.selectedBoxSubType ? ' and subtype "' + this.selectedBoxSubType + '"' : ''}:`, {
            totalBoxes: boxes.length,
            filteredBoxes: this.boxes.length,
            sampleBoxTag: boxes[0]?.code
          });
        } else {
          this.boxes = boxes;
        }
        
        // Load activities for all boxes in parallel for efficient search
        this.loadActivitiesForBoxes(this.boxes);
      },
      error: (err) => {
        this.error = err.message || 'Failed to load boxes';
        this.loading = false;
        console.error('Error loading boxes:', err);
      }
    });
  }

  private loadActivitiesForBoxes(boxes: Box[]): void {
    if (boxes.length === 0) {
      this.filteredBoxes = [];
      this.loading = false;
      this.applyFilters();
      return;
    }

    // Extract filter options from current boxes
    this.loadBoxFilterOptions(boxes);

    // Load activities for all boxes in parallel
    // Use catchError to handle individual failures gracefully
    const activityObservables = boxes.map(box => 
      this.boxService.getBoxActivities(box.id).pipe(
        // Map to include box reference
        map(activities => ({ boxId: box.id, activities })),
        // Handle individual errors - return empty array if request fails
        catchError(() => {
          console.warn(`Failed to load activities for box ${box.id}`);
          return of({ boxId: box.id, activities: [] });
        })
      )
    );

    // Use forkJoin to load all activities in parallel
    forkJoin(activityObservables).subscribe({
      next: (results) => {
        // Map activities to their respective boxes
        results.forEach((result: any) => {
          if (result && result.boxId) {
            const box = this.boxes.find(b => b.id === result.boxId);
            if (box) {
              box.activities = result.activities || [];
            }
          }
        });

        this.filteredBoxes = this.boxes;
        this.loading = false;
        this.applyFilters();
      },
      error: (err) => {
        console.error('Error loading activities:', err);
        // Continue even if activities fail to load
        this.filteredBoxes = this.boxes;
        this.loading = false;
        this.applyFilters();
      }
    });
  }
  
  /**
   * Load filter options from boxes list
   */
  private loadBoxFilterOptions(boxes: Box[]): void {
    // Recalculate counts for boxes list page filters
    this.recalculateBoxFilterCounts(boxes);
  }
  
  /**
   * Recalculate box filter counts with proper one-way cascading (boxes list page)
   * - SubType shows all within current box type
   * - Building filters Floors (not reverse)
   * - Floor filters Zones (not reverse)
   */
  private recalculateBoxFilterCounts(boxes: Box[]): void {
    // Clear existing counts
    this.boxSubTypeCounts.clear();
    this.boxBuildingCounts.clear();
    this.boxFloorCounts.clear();
    this.boxZoneCounts.clear();
    
    // Count maps for boxes list page
    const subTypeMap = new Map<string, number>();
    const buildingMap = new Map<string, number>();
    const floorMap = new Map<string, number>();
    const zoneMap = new Map<string, number>();
    
    // Always count from ALL boxes for parent filters (SubType, Building)
    this.boxes.forEach(box => {
      const parts = (box.code || '').split('-');
      
      // Always show all subtypes
      if (parts.length >= 5 && parts[4]) {
        const subType = parts[4];
        subTypeMap.set(subType, (subTypeMap.get(subType) || 0) + 1);
      }
      
      // Always show all buildings
      if (box.buildingNumber) {
        buildingMap.set(box.buildingNumber, (buildingMap.get(box.buildingNumber) || 0) + 1);
      }
    });
      
    // Floors: filtered by Building only
    let floorFilteredBoxes = this.boxes;
    if (this.selectedBoxBuildingFilter) {
      floorFilteredBoxes = this.boxes.filter(box => 
        box.buildingNumber === this.selectedBoxBuildingFilter
      );
    }
    
    floorFilteredBoxes.forEach(box => {
      if (box.floor) {
        floorMap.set(box.floor, (floorMap.get(box.floor) || 0) + 1);
      }
    });
      
    // Zones: filtered by Building and Floor
    let zoneFilteredBoxes = this.boxes;
    if (this.selectedBoxBuildingFilter) {
      zoneFilteredBoxes = zoneFilteredBoxes.filter(box => 
        box.buildingNumber === this.selectedBoxBuildingFilter
      );
    }
    if (this.selectedBoxFloorFilter) {
      zoneFilteredBoxes = zoneFilteredBoxes.filter(box => 
        box.floor === this.selectedBoxFloorFilter
      );
    }
    
    zoneFilteredBoxes.forEach(box => {
      if (box.zone) {
        zoneMap.set(box.zone, (zoneMap.get(box.zone) || 0) + 1);
      }
    });
    
    // Only show options that exist in the filtered data (count > 0)
    this.availableBoxSubTypes = Array.from(subTypeMap.keys()).filter(k => subTypeMap.get(k)! > 0).sort();
    this.availableBoxBuildings = Array.from(buildingMap.keys()).filter(k => buildingMap.get(k)! > 0).sort();
    this.availableBoxFloors = Array.from(floorMap.keys()).filter(k => floorMap.get(k)! > 0).sort();
    this.availableBoxZones = Array.from(zoneMap.keys()).filter(k => zoneMap.get(k)! > 0).sort();
    
    // Store counts
    this.boxSubTypeCounts = subTypeMap;
    this.boxBuildingCounts = buildingMap;
    this.boxFloorCounts = floorMap;
    this.boxZoneCounts = zoneMap;
    
    console.log('📊 Box filter counts recalculated (one-way cascading):', {
      totalBoxes: this.boxes.length,
      counts: {
        boxSubTypeCounts: Array.from(this.boxSubTypeCounts.entries()),
        boxBuildingCounts: Array.from(this.boxBuildingCounts.entries()),
        boxFloorCounts: Array.from(this.boxFloorCounts.entries()),
        boxZoneCounts: Array.from(this.boxZoneCounts.entries())
      }
    });
  }

  viewBoxType(boxType: string): void {
    this.selectedBoxType = boxType;
    this.selectedBoxSubType = null; // Reset subtype when viewing all types
    this.showBoxTypes = false;
    this.boxTypeSearchControl.setValue('');
    this.router.navigate([], {
      relativeTo: this.route,
      queryParams: { boxType: boxType, boxSubType: null },
      queryParamsHandling: 'merge'
    });
    this.loadBoxes();
  }

  viewBoxSubType(boxType: string, subType: string, event: Event): void {
    event.stopPropagation(); // Prevent card click
    this.selectedBoxType = boxType;
    this.selectedBoxSubType = subType;
    this.showBoxTypes = false;
    this.boxTypeSearchControl.setValue('');
    this.router.navigate([], {
      relativeTo: this.route,
      queryParams: { boxType: boxType, boxSubType: subType },
      queryParamsHandling: 'merge'
    });
    this.loadBoxes();
  }

  backToBoxTypes(): void {
    this.selectedBoxType = null;
    this.selectedBoxSubType = null;
    this.showBoxTypes = true;
    this.boxes = [];
    this.filteredBoxes = [];
    this.searchControl.setValue('');
    this.selectedStatus = BoxStatus.InProgress;
    
    // Reset box filters
    this.selectedBoxSubTypeFilter = '';
    this.selectedBoxBuildingFilter = '';
    this.selectedBoxFloorFilter = '';
    this.selectedBoxZoneFilter = '';
    
    this.router.navigate([], {
      relativeTo: this.route,
      queryParams: {},
      queryParamsHandling: 'merge'
    });
    this.loadBoxTypes();
  }

  backToProject(): void {
    this.router.navigate(['/projects', this.projectId, 'dashboard']);
  }

  filterByStatus(status: BoxStatus | 'All'): void {
    this.selectedStatus = status;
    this.applyFilters();
  }

  applyFilters(): void {
    let filtered = [...this.boxes];
    let countsSource = [...this.boxes]; // Track boxes for count calculation

    // Apply status filter
    if (this.selectedStatus !== 'All') {
      filtered = filtered.filter(box => box.status === this.selectedStatus);
      countsSource = countsSource.filter(box => box.status === this.selectedStatus);
    }

    // Apply subtype filter
    if (this.selectedBoxSubTypeFilter) {
      filtered = filtered.filter(box => {
        const parts = (box.code || '').split('-');
        const boxSubType = parts.length >= 5 ? parts[4] : '';
        return boxSubType === this.selectedBoxSubTypeFilter;
      });
      countsSource = countsSource.filter(box => {
        const parts = (box.code || '').split('-');
        const boxSubType = parts.length >= 5 ? parts[4] : '';
        return boxSubType === this.selectedBoxSubTypeFilter;
      });
    }

    // Apply building filter
    if (this.selectedBoxBuildingFilter) {
      filtered = filtered.filter(box => box.buildingNumber === this.selectedBoxBuildingFilter);
      countsSource = countsSource.filter(box => box.buildingNumber === this.selectedBoxBuildingFilter);
    }

    // Apply floor filter
    if (this.selectedBoxFloorFilter) {
      filtered = filtered.filter(box => box.floor === this.selectedBoxFloorFilter);
      countsSource = countsSource.filter(box => box.floor === this.selectedBoxFloorFilter);
    }

    // Apply zone filter
    if (this.selectedBoxZoneFilter) {
      filtered = filtered.filter(box => box.zone === this.selectedBoxZoneFilter);
      countsSource = countsSource.filter(box => box.zone === this.selectedBoxZoneFilter);
    }

    // Recalculate counts based on filtered boxes (cascading effect)
    // Only recalculate if we have filters applied (excluding search and status)
    if (this.selectedBoxSubTypeFilter || this.selectedBoxBuildingFilter || 
        this.selectedBoxFloorFilter || this.selectedBoxZoneFilter) {
      this.recalculateBoxFilterCounts(countsSource);
    }

    // Apply search filter (includes box properties and activity properties)
    const searchTerm = this.searchControl.value?.toLowerCase() || '';
    if (searchTerm) {
      filtered = filtered.filter(box => {
        // Search in box properties
        const matchesBoxProperties = 
          box.name?.toLowerCase().includes(searchTerm) ||
          box.code?.toLowerCase().includes(searchTerm) ||
          box.serialNumber?.toLowerCase().includes(searchTerm) ||
          box.type?.toLowerCase().includes(searchTerm) ||
          box.floor?.toLowerCase().includes(searchTerm) ||
          box.buildingNumber?.toLowerCase().includes(searchTerm) ||
          box.zone?.toLowerCase().includes(searchTerm) ||
          box.assignedTeam?.toLowerCase().includes(searchTerm) ||
          box.assignedTo?.toLowerCase().includes(searchTerm);

        // Search in activity properties
        const matchesActivityProperties = box.activities?.some(activity => {
          // Get activity name from multiple possible property names (handles both transformed and raw data)
          const activityName = (
            activity.name?.toLowerCase() || 
            (activity as any).activityName?.toLowerCase() || 
            (activity as any).ActivityName?.toLowerCase() || 
            ''
          );
          
          const activityStatus = activity.status?.toLowerCase() || '';
          const assignedTo = activity.assignedTo?.toLowerCase() || '';
          const description = activity.description?.toLowerCase() || '';
          
          // Check if search term matches activity properties
          // Priority: Activity name search is explicitly checked first
          return activityName.includes(searchTerm) ||
                 activityStatus.includes(searchTerm) ||
                 assignedTo.includes(searchTerm) ||
                 description.includes(searchTerm) ||
                 // Check date fields (format dates as strings for search)
                 this.formatDateForSearch(activity.plannedStartDate)?.includes(searchTerm) ||
                 this.formatDateForSearch(activity.plannedEndDate)?.includes(searchTerm) ||
                 this.formatDateForSearch(activity.actualStartDate)?.includes(searchTerm) ||
                 this.formatDateForSearch(activity.actualEndDate)?.includes(searchTerm);
        }) || false;

        return matchesBoxProperties || matchesActivityProperties;
      });
    }

    this.filteredBoxes = filtered;
  }
  
  /**
   * Clear all box filters
   */
  clearBoxFilters(): void {
    this.selectedBoxSubTypeFilter = '';
    this.selectedBoxBuildingFilter = '';
    this.selectedBoxFloorFilter = '';
    this.selectedBoxZoneFilter = '';
    this.searchControl.setValue('');
    this.selectedStatus = BoxStatus.InProgress;
    
    // Recalculate counts from all boxes when filters are cleared
    if (this.boxes.length > 0) {
      this.recalculateBoxFilterCounts(this.boxes);
    }
    
    this.applyFilters();
  }
  
  /**
   * Check if any box filters are applied
   */
  hasActiveBoxFilters(): boolean {
    return !!(
      this.selectedBoxSubTypeFilter ||
      this.selectedBoxBuildingFilter ||
      this.selectedBoxFloorFilter ||
      this.selectedBoxZoneFilter ||
      this.searchControl.value ||
      (this.selectedStatus !== BoxStatus.InProgress && this.selectedStatus !== 'All')
    );
  }

  applyBoxTypeFilters(): void {
    const searchTerm = this.boxTypeSearchControl.value?.toLowerCase() || '';
    
    let filtered = [...this.boxTypes];
    
    // Start with all boxes for cascading filter calculation
    let filteredBoxes = [...this.allProjectBoxes];
    
    // Apply box type filter
    if (this.selectedFilterBoxType) {
      filtered = filtered.filter(boxType => 
        boxType.boxType === this.selectedFilterBoxType
      );
      
      // Filter boxes by selected box type
      filteredBoxes = filteredBoxes.filter(box => {
        const parts = (box.code || '').split('-');
        const boxType = parts.length >= 4 ? parts[3] : '';
        return boxType === this.selectedFilterBoxType;
      });
    }
    
    // Apply subtype filter - filter within the selected box type only
    if (this.selectedFilterSubType) {
      // Filter boxes by selected subtype
      filteredBoxes = filteredBoxes.filter(box => {
        const parts = (box.code || '').split('-');
        const subType = parts.length >= 5 ? parts[4] : '';
        return subType === this.selectedFilterSubType;
      });
      
      // Don't filter out box types - subtype filter works within the selected box type
      // Only filter box types if no box type is selected
      if (!this.selectedFilterBoxType) {
        filtered = filtered.filter(boxType => 
          boxType.subTypes?.some(st => 
            st.subTypeAbbreviation === this.selectedFilterSubType || 
            st.subTypeName === this.selectedFilterSubType
          )
        );
      }
    }
    
    // Apply building filter
    if (this.selectedFilterBuilding) {
      filteredBoxes = filteredBoxes.filter(box => 
        box.buildingNumber === this.selectedFilterBuilding
      );
    }
    
    // Apply floor filter
    if (this.selectedFilterFloor) {
      filteredBoxes = filteredBoxes.filter(box => 
        box.floor === this.selectedFilterFloor
      );
    }
    
    // Apply zone filter
    if (this.selectedFilterZone) {
      filteredBoxes = filteredBoxes.filter(box => 
        box.zone === this.selectedFilterZone
      );
    }
    
    // Recalculate counts with proper one-way cascading (uses this.allProjectBoxes internally)
    this.recalculateFilterCounts(this.allProjectBoxes);
    
    // Update card counts based on filtered boxes
    this.updateBoxTypeCardCounts(filtered, filteredBoxes);
    
    // Apply search filter to box types (after recalculating counts)
    if (searchTerm) {
      filtered = filtered.filter(boxType => 
        boxType.boxType?.toLowerCase().includes(searchTerm)
      );
    }
    
    // Filter box types based on location filters
    if (this.selectedFilterBuilding || this.selectedFilterFloor || this.selectedFilterZone) {
      // Get unique box types from filtered boxes
      const validBoxTypes = new Set(
        filteredBoxes.map(box => {
          const parts = (box.code || '').split('-');
          return parts.length >= 4 ? parts[3] : '';
        }).filter(type => type !== '')
      );
      
      filtered = filtered.filter(boxType => 
        validBoxTypes.has(boxType.boxType)
      );
    }
    
    this.filteredBoxTypes = filtered;
  }
  
  /**
   * Update box type card counts based on filtered boxes
   */
  private updateBoxTypeCardCounts(boxTypes: BoxTypeStat[], filteredBoxes: Box[]): void {
    // Create maps to count boxes by type and subtype from filtered boxes
    const boxTypeCountMap = new Map<string, number>();
    const subTypeCountMap = new Map<string, Map<string, number>>();
    // Map to count drawings per subtype
    const drawingCountMap = new Map<string, Map<string, number>>();
    
    filteredBoxes.forEach(box => {
      const parts = (box.code || '').split('-');
      const boxType = parts.length >= 4 ? parts[3] : '';
      const subType = parts.length >= 5 ? parts[4] : '';
      
      if (boxType) {
        // Count total boxes per box type
        boxTypeCountMap.set(boxType, (boxTypeCountMap.get(boxType) || 0) + 1);
        
        // Count boxes per subtype within each box type
        if (subType) {
          if (!subTypeCountMap.has(boxType)) {
            subTypeCountMap.set(boxType, new Map<string, number>());
          }
          const subTypeMap = subTypeCountMap.get(boxType)!;
          subTypeMap.set(subType, (subTypeMap.get(subType) || 0) + 1);
          
          // Count drawings per subtype
          const drawingCount = box.drawingsCount || 0;
          if (!drawingCountMap.has(boxType)) {
            drawingCountMap.set(boxType, new Map<string, number>());
          }
          const drawingMap = drawingCountMap.get(boxType)!;
          drawingMap.set(subType, (drawingMap.get(subType) || 0) + drawingCount);
        }
      }
    });
    
    // Update each box type card with filtered counts
    boxTypes.forEach(boxType => {
      const filteredCount = boxTypeCountMap.get(boxType.boxType) || 0;
      boxType.boxCount = filteredCount;
      
      // Update subtype counts
      if (boxType.subTypes && boxType.subTypes.length > 0) {
        const subTypeMap = subTypeCountMap.get(boxType.boxType);
        const drawingMap = drawingCountMap.get(boxType.boxType);
        boxType.subTypes.forEach(subType => {
          const subTypeName = subType.subTypeAbbreviation || subType.subTypeName;
          const subTypeFilteredCount = subTypeMap?.get(subTypeName) || 0;
          subType.boxCount = subTypeFilteredCount;
          
          // Set drawing count for the subtype
          subType.drawingsCount = drawingMap?.get(subTypeName) || 0;
          
          // Recalculate progress percentage if needed
          if (filteredCount > 0) {
            subType.progress = (subTypeFilteredCount / filteredCount) * 100;
          } else {
            subType.progress = 0;
          }
        });
      }
    });
  }
  
  /**
   * Clear all box type filters
   */
  clearBoxTypeFilters(): void {
    this.selectedFilterBoxType = '';
    this.selectedFilterSubType = '';
    this.selectedFilterBuilding = '';
    this.selectedFilterFloor = '';
    this.selectedFilterZone = '';
    this.boxTypeSearchControl.setValue('');
    
    // Recalculate counts from all boxes when filters are cleared
    if (this.allProjectBoxes.length > 0) {
      this.recalculateFilterCounts(this.allProjectBoxes);
    }
    this.applyBoxTypeFilters();
  }
  
  /**
   * Check if any filters are applied
   */
  hasActiveFilters(): boolean {
    return !!(
      this.selectedFilterBoxType ||
      this.selectedFilterSubType ||
      this.selectedFilterBuilding ||
      this.selectedFilterFloor ||
      this.selectedFilterZone ||
      this.selectedStatus ||
      this.boxTypeSearchControl.value ||
      this.searchControl.value
    );
  }

  private formatDateForSearch(date?: Date): string | null {
    if (!date) return null;
    
    try {
      const d = typeof date === 'string' ? new Date(date) : date;
      if (isNaN(d.getTime())) return null;
      
      // Format as YYYY-MM-DD, MM/DD/YYYY, and month name for flexible search
      const year = d.getFullYear();
      const month = String(d.getMonth() + 1).padStart(2, '0');
      const day = String(d.getDate()).padStart(2, '0');
      const monthNames = ['january', 'february', 'march', 'april', 'may', 'june',
                         'july', 'august', 'september', 'october', 'november', 'december'];
      const monthName = monthNames[d.getMonth()];
      
      return `${year}-${month}-${day} ${month}/${day}/${year} ${monthName} ${year}`.toLowerCase();
    } catch {
      return null;
    }
  }

  viewBox(boxId: string): void {
    this.router.navigate(['/projects', this.projectId, 'boxes', boxId]);
  }

  createBox(): void {
    this.router.navigate(['/boxes/create'], { 
      queryParams: { projectId: this.projectId }
    });
  }

  duplicateBox(box: Box): void {
    if (!this.canCreate) {
      return;
    }
    
    // Show confirmation modal
    this.boxToDuplicate = box;
    this.showDuplicateConfirm = true;
  }

  confirmDuplicate(): void {
    if (!this.boxToDuplicate) {
      return;
    }

    this.duplicating = true;
    
    // Include activities by default, drawings optional (can be configured)
    const includeActivities = true;
    const includeDrawings = false; // Set to true if you want to copy drawing references
    
    // Call the duplicate endpoint
    this.boxService.duplicateBox(this.boxToDuplicate.id, includeActivities, includeDrawings).subscribe({
      next: (duplicatedBox) => {
        console.log('✅ Box duplicated successfully:', duplicatedBox);
        this.duplicating = false;
        this.showDuplicateConfirm = false;
        this.boxToDuplicate = null;
        
        // Show success message
        document.dispatchEvent(new CustomEvent('app-toast', {
          detail: { 
            message: `Box duplicated successfully! New box: ${duplicatedBox.code}`,
            type: 'success' 
          }
        }));
        
        // Reload boxes to show the new duplicated box
        this.loadBoxes();
      },
      error: (err) => {
        console.error('❌ Error duplicating box:', err);
        this.duplicating = false;
        this.showDuplicateConfirm = false;
        this.boxToDuplicate = null;
        
        const errorMessage = err.error?.message || err.error?.title || err.message || 'Unknown error';
        document.dispatchEvent(new CustomEvent('app-toast', {
          detail: { 
            message: `Failed to duplicate box: ${errorMessage}`,
            type: 'error' 
          }
        }));
      }
    });
  }

  cancelDuplicate(): void {
    this.showDuplicateConfirm = false;
    this.boxToDuplicate = null;
    this.duplicating = false;
  }

  getDrawingsCount(box: Box): number {
    console.log('🔍 Drawing count:', box.drawingsCount);
    // Check for drawingsCount property (TypeScript convention)
    if (box.drawingsCount !== undefined && box.drawingsCount !== null) {
      return box.drawingsCount;
    }
    
    // Check for DrawingsCount property (C# convention from backend)
    if ((box as any).DrawingsCount !== undefined && (box as any).DrawingsCount !== null) {
      return (box as any).DrawingsCount;
    }
    
    // Return 0 as default if no count is available
    return 0;
  }
  
  /**
   * Get formatted display text for filter option with count
   */
  getFilterOptionDisplay(value: string, count: number | undefined): string {
    if (count !== undefined && count > 0) {
      return `${value} (${count})`;
    }
    return value;
  }
  
  /**
   * Get filtered subtypes for a box type card based on applied filters
   */
  getFilteredSubTypes(boxType: BoxTypeStat): any[] {
    if (!boxType.subTypes || boxType.subTypes.length === 0) {
      return [];
    }
    
    // If no filters are applied, show all subtypes
    if (!this.hasActiveFilters()) {
      return boxType.subTypes;
    }
    
    // Filter subtypes based on applied filters
    return boxType.subTypes.filter(subType => {
      const subTypeAbbr = subType.subTypeAbbreviation || subType.subTypeName;
      
      // If a specific sub type filter is selected, only show that sub type
      if (this.selectedFilterSubType && subTypeAbbr !== this.selectedFilterSubType) {
        return false;
      }
      
      // Check if this subtype exists in the filtered boxes
      const hasMatchingBoxes = this.allProjectBoxes.some(box => {
        const parts = (box.code || '').split('-');
        const boxTypeFromCode = parts.length >= 4 ? parts[3] : '';
        const subTypeFromCode = parts.length >= 5 ? parts[4] : '';
        
        // Must match the box type
        if (boxTypeFromCode !== boxType.boxType) {
          return false;
        }
        
        // Must match the subtype
        if (subTypeFromCode !== subTypeAbbr) {
          return false;
        }
        
        // Apply location filters
        if (this.selectedFilterBuilding && box.buildingNumber !== this.selectedFilterBuilding) {
          return false;
        }
        
        if (this.selectedFilterFloor && box.floor !== this.selectedFilterFloor) {
          return false;
        }
        
        if (this.selectedFilterZone && box.zone !== this.selectedFilterZone) {
          return false;
        }
        
        return true;
      });
      
      return hasMatchingBoxes;
    });
  }
  
  /**
   * Get count for a specific filter value
   */
  getFilterCount(filterType: 'boxType' | 'subType' | 'building' | 'floor' | 'zone' | 'boxSubType' | 'boxBuilding' | 'boxFloor' | 'boxZone', value: string): number {
    switch (filterType) {
      case 'boxType':
        return this.boxTypeCounts.get(value) || 0;
      case 'subType':
        return this.subTypeCounts.get(value) || 0;
      case 'building':
        return this.buildingCounts.get(value) || 0;
      case 'floor':
        return this.floorCounts.get(value) || 0;
      case 'zone':
        return this.zoneCounts.get(value) || 0;
      case 'boxSubType':
        return this.boxSubTypeCounts.get(value) || 0;
      case 'boxBuilding':
        return this.boxBuildingCounts.get(value) || 0;
      case 'boxFloor':
        return this.boxFloorCounts.get(value) || 0;
      case 'boxZone':
        return this.boxZoneCounts.get(value) || 0;
      default:
        return 0;
    }
  }

  getStatusClass(status: BoxStatus): string {
    const statusMap: Record<BoxStatus, string> = {
      [BoxStatus.NotStarted]: 'badge-secondary',
      [BoxStatus.InProgress]: 'badge-warning',
      [BoxStatus.QAReview]: 'badge-info',
      [BoxStatus.Completed]: 'badge-success',
      [BoxStatus.ReadyForDelivery]: 'badge-primary',
      [BoxStatus.Delivered]: 'badge-success',
      [BoxStatus.OnHold]: 'badge-danger',
      [BoxStatus.Dispatched]: 'badge-primary'
    };
    return statusMap[status] || 'badge-secondary';
  }

  getStatusLabel(status: BoxStatus): string {
    const labels: Record<BoxStatus, string> = {
      [BoxStatus.NotStarted]: 'Not Started',
      [BoxStatus.InProgress]: 'In Progress',
      [BoxStatus.QAReview]: 'QA Review',
      [BoxStatus.Completed]: 'Completed',
      [BoxStatus.ReadyForDelivery]: 'Ready for Delivery',
      [BoxStatus.Delivered]: 'Delivered',
      [BoxStatus.OnHold]: 'On Hold',
      [BoxStatus.Dispatched]: 'Dispatched'
    };
    return labels[status] || status;
  }

  /**
   * Extract box type abbreviation from BoxTag
   * BoxTag format: ProjectNumber-Building-Floor-Type-SubType
   */
  getBoxTypeFromTag(box: Box): string {
    const parts = (box.code || '').split('-');
    // Type is at position 3 (index 3)
    return parts.length >= 4 ? parts[3] : '';
  }

  /**
   * Extract box subtype abbreviation from BoxTag
   * BoxTag format: ProjectNumber-Building-Floor-Type-SubType
   */
  getBoxSubTypeFromTag(box: Box): string {
    const parts = (box.code || '').split('-');
    // SubType is at position 4 (index 4)
    return parts.length >= 5 ? parts[4] : '';
  }

  // Expose Math to template
  Math = Math;

  /**
   * Rounds a number to one decimal place
   * @param value The number to round
   * @returns The number rounded to 1 decimal place
   */
  roundToOneDecimal(value: number | undefined | null): number {
    if (value === undefined || value === null) {
      return 0;
    }
    return Math.round(value * 10) / 10;
  }
}
