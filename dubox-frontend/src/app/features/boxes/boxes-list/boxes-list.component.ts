import { Component, OnInit, OnDestroy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { FormControl, ReactiveFormsModule, FormsModule } from '@angular/forms';
import { debounceTime, distinctUntilChanged, map, catchError, skip } from 'rxjs/operators';
import { forkJoin, of, Subscription } from 'rxjs';
import { BoxService } from '../../../core/services/box.service';
import { PermissionService } from '../../../core/services/permission.service';
import { Box, BoxStatus, BoxTypeStat, PanelStatus, BoxFilters } from '../../../core/models/box.model';
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

  // Permanent list of all buildings loaded from endpoint (never reduced by cascading)
  private allAvailableBuildings: string[] = [];

  // Box types view – permanent box-type list derived from stats (never reduced)
  private allAvailableBoxTypesBaseline: string[] = [];

  // Box types view – permanent endpoint baselines (restored when no parent filter is active)
  private allAvailableSubTypesFromEndpoint: string[] = [];
  private allAvailableFloorsFromEndpoint: string[] = [];
  private allAvailableZonesFromEndpoint: string[] = [];

  // Box types view – accumulated options per active parent-filter context
  private allAvailableSubTypesForBoxTypes: string[] = [];
  private subTypeBoxTypeCtx: string = '';
  private allAvailableFloorsForBoxTypes: string[] = [];
  private floorsBoxBuildingCtx: string = '';
  private allAvailableZonesForBoxTypes: string[] = [];
  private zonesCtxKey: string = '';
  
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

  // Boxes list view – accumulated options per active parent-filter context
  private allAvailableBoxBuildings: string[] = [];
  private allAvailableBoxSubTypes: string[] = [];
  private allAvailableBoxFloors: string[] = [];
  private boxFloorsBuildingCtx: string = '';
  private allAvailableBoxZones: string[] = [];
  private boxZonesCtxKey: string = '';
  
  /** When set, filter boxes by "ready to start" (all panels SecondApprovalApproved). 'true' = ready only, 'false' = not ready only. */
  readyToStartFilter: 'true' | 'false' | null = null;
  
  // Pagination properties
  currentPage = 1;
  pageSize = 50;
  totalCount = 0;
  totalPages = 0;
  hasPreviousPage = false;
  hasNextPage = false;
  Math = Math; // Expose Math to template

  /** Always use API totalCount since backend now handles all filtering */
  get displayTotalCount(): number {
    return this.totalCount > 0 ? this.totalCount : this.boxes.length;
  }
  
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
    const view = this.route.snapshot.queryParams['view'];
    const readyToStart = this.route.snapshot.queryParams['readyToStart'];
    
    // Set status filter if provided in query params
    if (status && Object.values(BoxStatus).includes(status as BoxStatus)) {
      this.selectedStatus = status as BoxStatus;
    }
    
    // Set ready-to-start filter when navigating from project dashboard cards
    if (readyToStart === 'true' || readyToStart === 'false') {
      this.readyToStartFilter = readyToStart;
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
    
    // Determine which view we're showing based on query params
    const willShowBoxTypes = !(boxType || status || view === 'boxes' || this.readyToStartFilter);
    
    // Set building/floor filters to the appropriate variables based on the view
    if (building) {
      if (willShowBoxTypes) {
        this.selectedFilterBuilding = building;
      } else {
        // When showing boxes list, use the boxes list filter variables
        this.selectedBoxBuildingFilter = building;
      }
    }
    if (floor) {
      if (willShowBoxTypes) {
        this.selectedFilterFloor = floor;
      } else {
        // When showing boxes list, use the boxes list filter variables
        this.selectedBoxFloorFilter = floor;
      }
    }
    
    if (boxType || status || view === 'boxes' || this.readyToStartFilter) {
      // If status, view=boxes, or readyToStart is provided without boxType, load all boxes (boxes list, not box types)
      if ((status && !boxType) || view === 'boxes' || this.readyToStartFilter) {
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
        this.onBackendFilterChange();
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
    
    // Load box types and filter options in parallel (efficient - no box data loaded)
    forkJoin({
      boxTypes: this.boxService.getBoxTypeStatsByProject(this.projectId),
      filterOptions: this.boxService.getBoxFilterOptions(this.projectId)
    }).subscribe({
      next: (result) => {
        this.boxTypes = result.boxTypes.boxTypeStats || [];
        this.filteredBoxTypes = [...this.boxTypes];
        
        // Populate Box Type dropdown from the stats data (the filter options endpoint
        // does not include box types, so we derive them from the fetched stats).
        this.availableBoxTypes = this.boxTypes
          .map(bt => bt.boxType)
          .filter(bt => !!bt)
          .sort();
        // Store as permanent baseline so recalculateFilterCounts never empties the list
        this.allAvailableBoxTypesBaseline = [...this.availableBoxTypes];

        // Populate filter options from efficient endpoint
        this.loadFilterOptionsFromEndpoint(result.filterOptions);
        
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
   * Load filter options from efficient endpoint (no box data loaded)
   */
  private loadFilterOptionsFromEndpoint(filterOptions: {
    subTypes: Array<{ value: string; count: number }>;
    buildings: Array<{ value: string; count: number }>;
    floors: Array<{ value: string; count: number }>;
    zones: Array<{ value: string; count: number }>;
  }): void {
    // Clear existing counts
    this.subTypeCounts.clear();
    this.buildingCounts.clear();
    this.floorCounts.clear();
    this.zoneCounts.clear();
    
    // Populate from endpoint data
    filterOptions.subTypes.forEach(st => this.subTypeCounts.set(st.value, st.count));
    filterOptions.buildings.forEach(b => this.buildingCounts.set(b.value, b.count));
    filterOptions.floors.forEach(f => this.floorCounts.set(f.value, f.count));
    filterOptions.zones.forEach(z => this.zoneCounts.set(z.value, z.count));
    
    // Set available options
    this.availableSubTypes = filterOptions.subTypes.map(st => st.value).sort();
    this.availableBuildings = filterOptions.buildings.map(b => b.value).sort();
    this.availableFloors = filterOptions.floors.map(f => f.value).sort();
    this.availableZones = filterOptions.zones.map(z => z.value).sort();

    // Preserve the full lists from the endpoint as permanent baselines.
    // These are restored whenever the corresponding parent filter is cleared.
    this.allAvailableBuildings = [...this.availableBuildings];
    this.allAvailableSubTypesFromEndpoint = [...this.availableSubTypes];
    this.allAvailableFloorsFromEndpoint = [...this.availableFloors];
    this.allAvailableZonesFromEndpoint = [...this.availableZones];
    
    console.log('📊 Filter options loaded from endpoint:', {
      subTypes: filterOptions.subTypes.length,
      buildings: filterOptions.buildings.length,
      floors: filterOptions.floors.length,
      zones: filterOptions.zones.length
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
      
      // Count buildings from boxes for counts display only
      if (box.buildingNumber) {
        buildingMap.set(box.buildingNumber, (buildingMap.get(box.buildingNumber) || 0) + 1);
      }
    });

    // Ensure all buildings from the original endpoint data are always present in the map
    // (allProjectBoxes may be paginated/partial, so some buildings could be missing)
    this.allAvailableBuildings.forEach(b => {
      if (!buildingMap.has(b)) {
        buildingMap.set(b, 0);
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
    
    // Box Types: always show all box types from the permanent baseline so selecting a box
    // type (or any other filter) never empties the dropdown.
    this.availableBoxTypes = this.allAvailableBoxTypesBaseline.length > 0
      ? [...this.allAvailableBoxTypesBaseline]
      : Array.from(boxTypeMap.keys()).filter(k => boxTypeMap.get(k)! > 0).sort();

    // SubTypes: when no box-type filter is active, restore the full endpoint list.
    // When a box-type IS selected, accumulate subtypes for that context so selecting one
    // subtype never removes the others from the dropdown.
    if (!this.selectedFilterBoxType) {
      this.availableSubTypes = [...this.allAvailableSubTypesFromEndpoint];
    } else {
      if (this.subTypeBoxTypeCtx !== this.selectedFilterBoxType) {
        this.allAvailableSubTypesForBoxTypes = [];
        this.subTypeBoxTypeCtx = this.selectedFilterBoxType;
      }
      Array.from(subTypeMap.keys()).filter(k => subTypeMap.get(k)! > 0).forEach(st => {
        if (!this.allAvailableSubTypesForBoxTypes.includes(st)) {
          this.allAvailableSubTypesForBoxTypes.push(st);
        }
      });
      this.availableSubTypes = [...this.allAvailableSubTypesForBoxTypes].sort();
    }

    // Buildings: always show the full endpoint list so selecting a building never hides others.
    this.availableBuildings = this.allAvailableBuildings.length > 0
      ? [...this.allAvailableBuildings]
      : Array.from(buildingMap.keys()).sort();

    // Floors: when no building is selected, restore the full endpoint list.
    // When a building IS selected, accumulate floors for that building so selecting a floor
    // never removes the other floors from the dropdown.
    if (!this.selectedFilterBuilding) {
      this.availableFloors = [...this.allAvailableFloorsFromEndpoint];
    } else {
      if (this.floorsBoxBuildingCtx !== this.selectedFilterBuilding) {
        this.allAvailableFloorsForBoxTypes = [];
        this.floorsBoxBuildingCtx = this.selectedFilterBuilding;
      }
      Array.from(floorMap.keys()).filter(k => floorMap.get(k)! > 0).forEach(f => {
        if (!this.allAvailableFloorsForBoxTypes.includes(f)) {
          this.allAvailableFloorsForBoxTypes.push(f);
        }
      });
      this.availableFloors = [...this.allAvailableFloorsForBoxTypes].sort();
    }

    // Zones: when no parent filters are active, restore the full endpoint list.
    // When a building/floor IS selected, accumulate zones for that context so selecting
    // a zone never removes the other zones from the dropdown.
    const currentZonesCtx = `${this.selectedFilterBuilding}|${this.selectedFilterFloor}`;
    if (!this.selectedFilterBuilding && !this.selectedFilterFloor) {
      this.availableZones = [...this.allAvailableZonesFromEndpoint];
    } else {
      if (this.zonesCtxKey !== currentZonesCtx) {
        this.allAvailableZonesForBoxTypes = [];
        this.zonesCtxKey = currentZonesCtx;
      }
      Array.from(zoneMap.keys()).filter(k => zoneMap.get(k)! > 0).forEach(z => {
        if (!this.allAvailableZonesForBoxTypes.includes(z)) {
          this.allAvailableZonesForBoxTypes.push(z);
        }
      });
      this.availableZones = [...this.allAvailableZonesForBoxTypes].sort();
    }
    
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

  /** Build filters for backend so the API returns only matching boxes (faster, less data). */
  private getBackendFilters(): BoxFilters | undefined {
    const status = this.selectedStatus !== 'All' ? [this.selectedStatus] : undefined;
    const boxType = this.selectedBoxType?.trim() || undefined;
    // Use selectedBoxSubTypeFilter from dropdown if set, otherwise use selectedBoxSubType from navigation
    const boxSubType = this.selectedBoxSubTypeFilter?.trim() || this.selectedBoxSubType?.trim() || undefined;
    const buildingNumber = this.selectedBoxBuildingFilter?.trim() || undefined;
    const floor = this.selectedBoxFloorFilter?.trim() || undefined;
    const zone = this.selectedBoxZoneFilter?.trim() || undefined;
    const search = this.searchControl.value?.trim() || undefined;
    if (!status && !boxType && !boxSubType && !buildingNumber && !floor && !zone && !search) return undefined;
    return { status, boxType, boxSubType, buildingNumber, floor, zone, search };
  }

  loadBoxes(): void {
    this.loading = true;
    this.error = '';
    const filters = this.getBackendFilters() || {};
    
    // Add pagination to filters
    filters.page = this.currentPage;
    filters.pageSize = this.pageSize;
    
    this.boxService.getBoxesByProjectPaginated(this.projectId, filters).subscribe({
      next: (response) => {
        console.log('📦 Pagination response:', response);
        
        // Update pagination metadata
        this.totalCount = response.totalCount;
        this.totalPages = response.totalPages;
        this.hasPreviousPage = response.hasPreviousPage;
        this.hasNextPage = response.hasNextPage;
        
        // Backend now filters by box type, so use the response items directly
        this.boxes = response.items;
        console.log('📦 Received boxes from backend (already filtered):', this.boxes.length);
        
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

  /** Called when status/building/floor/zone/search/subtype change – refetch from backend so filtering happens on server. */
  onBackendFilterChange(): void {
    this.currentPage = 1; // Reset to first page when filters change
    this.loadBoxes();
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
      
      // Count buildings from current page boxes
      if (box.buildingNumber) {
        buildingMap.set(box.buildingNumber, (buildingMap.get(box.buildingNumber) || 0) + 1);
      }
    });

    // Merge previously seen buildings so the dropdown never loses options when filters are applied.
    // (When a building filter is active the backend only returns boxes for that building,
    // so buildingMap would otherwise be missing the other buildings.)
    this.allAvailableBoxBuildings.forEach(b => {
      if (!buildingMap.has(b)) {
        buildingMap.set(b, 0);
      }
    });
    // Accumulate any newly discovered buildings
    Array.from(buildingMap.keys()).forEach(b => {
      if (!this.allAvailableBoxBuildings.includes(b)) {
        this.allAvailableBoxBuildings.push(b);
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
    
    // SubTypes: accumulate all subtypes seen for the current box type context so selecting
    // one subtype never removes the others from the dropdown.
    Array.from(subTypeMap.keys()).filter(k => subTypeMap.get(k)! > 0).forEach(st => {
      if (!this.allAvailableBoxSubTypes.includes(st)) {
        this.allAvailableBoxSubTypes.push(st);
      }
    });
    this.availableBoxSubTypes = [...this.allAvailableBoxSubTypes].sort();

    // Buildings: always show all accumulated buildings so selecting one never hides others.
    this.availableBoxBuildings = [...this.allAvailableBoxBuildings].sort();

    // Floors: cascade from building only. Accumulate floors for the current building context
    // so selecting a floor never removes the other floors from the dropdown.
    if (this.boxFloorsBuildingCtx !== this.selectedBoxBuildingFilter) {
      this.allAvailableBoxFloors = [];
      this.boxFloorsBuildingCtx = this.selectedBoxBuildingFilter;
    }
    Array.from(floorMap.keys()).filter(k => floorMap.get(k)! > 0).forEach(f => {
      if (!this.allAvailableBoxFloors.includes(f)) {
        this.allAvailableBoxFloors.push(f);
      }
    });
    this.availableBoxFloors = [...this.allAvailableBoxFloors].sort();

    // Zones: cascade from building + floor. Accumulate zones for the current context so
    // selecting a zone never removes the other zones from the dropdown.
    const currentBoxZonesCtx = `${this.selectedBoxBuildingFilter}|${this.selectedBoxFloorFilter}`;
    if (this.boxZonesCtxKey !== currentBoxZonesCtx) {
      this.allAvailableBoxZones = [];
      this.boxZonesCtxKey = currentBoxZonesCtx;
    }
    Array.from(zoneMap.keys()).filter(k => zoneMap.get(k)! > 0).forEach(z => {
      if (!this.allAvailableBoxZones.includes(z)) {
        this.allAvailableBoxZones.push(z);
      }
    });
    this.availableBoxZones = [...this.allAvailableBoxZones].sort();
    
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
    this.currentPage = 1; // Reset to first page
    this.resetBoxListAccumulatedFilters();
    
    // Transfer building/floor filters from box types view to boxes list view
    if (this.selectedFilterBuilding) {
      this.selectedBoxBuildingFilter = this.selectedFilterBuilding;
    }
    if (this.selectedFilterFloor) {
      this.selectedBoxFloorFilter = this.selectedFilterFloor;
    }
    
    this.router.navigate([], {
      relativeTo: this.route,
      queryParams: { boxType: boxType, boxSubType: null },
      queryParamsHandling: 'merge'
    });
    this.loadBoxes();
  }

  /**
   * Navigate to box type materials management page
   */
  manageBoxTypeMaterials(boxType: BoxTypeStat): void {
    // Find the project box type ID from the boxType object
    // The boxType.boxType is the TypeName, we need to get the ID
    // We'll navigate using the project ID and let the backend query filter by TypeName
    // Or we can pass the TypeName as a query parameter
    
    // For now, navigate to the box type materials page for the project
    // The page will show all box types and user can filter
    this.router.navigate(['/projects', this.projectId, 'box-type-materials'], {
      queryParams: { boxType: boxType.boxType }
    });
  }

  viewBoxSubType(boxType: string, subType: string, event: Event): void {
    event.stopPropagation(); // Prevent card click
    this.selectedBoxType = boxType;
    this.selectedBoxSubType = subType;
    this.showBoxTypes = false;
    this.boxTypeSearchControl.setValue('');
    this.currentPage = 1; // Reset to first page
    this.resetBoxListAccumulatedFilters();
    
    // Transfer building/floor filters from box types view to boxes list view
    if (this.selectedFilterBuilding) {
      this.selectedBoxBuildingFilter = this.selectedFilterBuilding;
    }
    if (this.selectedFilterFloor) {
      this.selectedBoxFloorFilter = this.selectedFilterFloor;
    }
    
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
    
    // Transfer building/floor filters from boxes list view back to box types view
    if (this.selectedBoxBuildingFilter) {
      this.selectedFilterBuilding = this.selectedBoxBuildingFilter;
    }
    if (this.selectedBoxFloorFilter) {
      this.selectedFilterFloor = this.selectedBoxFloorFilter;
    }
    
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
    this.onBackendFilterChange();
  }

  /**
   * Returns true if the box is "ready to start" (box has ReadyToStart status).
   * Matches the logic used on the project dashboard for boxesReadyToStart / boxesNotReadyToStart.
   */
  private isBoxReadyToStart(box: Box): boolean {
    return box.status === BoxStatus.ReadyToStart;
  }

  applyFilters(): void {
    // Most filters are now handled by backend (status, subtype, building, floor, zone, search)
    // This method now only handles special client-side filters if needed
    
    let filtered = [...this.boxes];

    // Apply ready-to-start filter (from project dashboard card clicks)
    // This is a special filter not sent to backend
    if (this.readyToStartFilter === 'true') {
      filtered = filtered.filter(box => this.isBoxReadyToStart(box));
    } else if (this.readyToStartFilter === 'false') {
      filtered = filtered.filter(box => !this.isBoxReadyToStart(box));
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
    this.selectedStatus = 'All';
    this.readyToStartFilter = null;
    // Reset accumulated lists so the next unfiltered load rebuilds them cleanly
    this.resetBoxListAccumulatedFilters();
    
    // Update URL to remove readyToStart and other query params when clearing
    this.router.navigate([], {
      relativeTo: this.route,
      queryParams: { view: null, status: null, readyToStart: null, building: null, floor: null },
      queryParamsHandling: 'merge'
    });
    
    // Refetch from backend with no filters so we get all boxes
    this.loadBoxes();
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
      (this.selectedStatus !== BoxStatus.InProgress && this.selectedStatus !== 'All') ||
      this.readyToStartFilter
    );
  }

  applyBoxTypeFilters(): void {
    const searchTerm = this.boxTypeSearchControl.value?.toLowerCase() || '';
    
    let filtered = [...this.boxTypes];
    
    // Check if any filters are applied that require box data for cascading
    const hasLocationFilters = this.selectedFilterBuilding || this.selectedFilterFloor || this.selectedFilterZone;
    
    // If filters are applied and we don't have box data yet, load it for cascading
    if (hasLocationFilters && this.allProjectBoxes.length === 0) {
      this.loadAllBoxesForCascading();
      return; // Will reapply filters after boxes are loaded
    }
    
    // Start with all boxes for cascading filter calculation (if loaded)
    let filteredBoxes = [...this.allProjectBoxes];
    
    // Apply box type filter
    if (this.selectedFilterBoxType) {
      filtered = filtered.filter(boxType => 
        boxType.boxType === this.selectedFilterBoxType
      );
      
      // Filter boxes by selected box type (if boxes are loaded)
      if (this.allProjectBoxes.length > 0) {
        filteredBoxes = filteredBoxes.filter(box => {
          const parts = (box.code || '').split('-');
          const boxType = parts.length >= 4 ? parts[3] : '';
          return boxType === this.selectedFilterBoxType;
        });
      }
    }
    
    // Apply subtype filter - filter within the selected box type only
    if (this.selectedFilterSubType) {
      // Filter boxes by selected subtype (if boxes are loaded)
      if (this.allProjectBoxes.length > 0) {
        filteredBoxes = filteredBoxes.filter(box => {
          const parts = (box.code || '').split('-');
          const subType = parts.length >= 5 ? parts[4] : '';
          return subType === this.selectedFilterSubType;
        });
      }
      
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
    
    // Apply location filters (if boxes are loaded)
    if (this.allProjectBoxes.length > 0) {
      if (this.selectedFilterBuilding) {
        filteredBoxes = filteredBoxes.filter(box => 
          box.buildingNumber === this.selectedFilterBuilding
        );
      }
      
      if (this.selectedFilterFloor) {
        filteredBoxes = filteredBoxes.filter(box => 
          box.floor === this.selectedFilterFloor
        );
      }
      
      if (this.selectedFilterZone) {
        filteredBoxes = filteredBoxes.filter(box => 
          box.zone === this.selectedFilterZone
        );
      }
      
      // Recalculate counts with proper one-way cascading (uses this.allProjectBoxes internally)
      this.recalculateFilterCounts(this.allProjectBoxes);
      
      // Update card counts based on filtered boxes
      this.updateBoxTypeCardCounts(filtered, filteredBoxes);
    }
    
    // Apply search filter to box types (after recalculating counts)
    if (searchTerm) {
      filtered = filtered.filter(boxType => 
        boxType.boxType?.toLowerCase().includes(searchTerm)
      );
    }
    
    // Filter box types based on location filters (if boxes are loaded)
    if (hasLocationFilters && this.allProjectBoxes.length > 0) {
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
   * Load all boxes for cascading filter calculations (only when needed)
   */
  private loadAllBoxesForCascading(): void {
    console.log('📦 Loading all boxes for cascading filter calculations...');
    this.boxService.getBoxesByProject(this.projectId).subscribe({
      next: (boxes) => {
        this.allProjectBoxes = boxes;
        console.log('✅ Boxes loaded for cascading:', boxes.length);
        // Reapply filters now that boxes are loaded
        this.applyBoxTypeFilters();
      },
      error: (err) => {
        console.error('❌ Error loading boxes for cascading:', err);
        this.allProjectBoxes = [];
        this.applyBoxTypeFilters();
      }
    });
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
    // Reset accumulated filter lists so endpoint baselines are restored immediately
    this.resetBoxTypesAccumulatedFilters();
    
    // Recalculate counts from all boxes when filters are cleared
    if (this.allProjectBoxes.length > 0) {
      this.recalculateFilterCounts(this.allProjectBoxes);
    }
    this.applyBoxTypeFilters();
  }

  /** Reset accumulated context-aware filter lists for the box types view. */
  private resetBoxTypesAccumulatedFilters(): void {
    this.allAvailableSubTypesForBoxTypes = [];
    this.subTypeBoxTypeCtx = '';
    this.allAvailableFloorsForBoxTypes = [];
    this.floorsBoxBuildingCtx = '';
    this.allAvailableZonesForBoxTypes = [];
    this.zonesCtxKey = '';
  }

  /** Reset accumulated context-aware filter lists for the boxes list view. */
  private resetBoxListAccumulatedFilters(): void {
    this.allAvailableBoxBuildings = [];
    this.allAvailableBoxSubTypes = [];
    this.allAvailableBoxFloors = [];
    this.boxFloorsBuildingCtx = '';
    this.allAvailableBoxZones = [];
    this.boxZonesCtxKey = '';
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
      [BoxStatus.ReadyToStart]: 'badge-info',
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
      [BoxStatus.ReadyToStart]: 'Ready to Start',
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

  // Pagination methods
  goToPage(page: number): void {
    if (page >= 1 && page <= this.totalPages && page !== this.currentPage) {
      this.currentPage = page;
      this.loadBoxes();
      window.scrollTo({ top: 0, behavior: 'smooth' });
    }
  }

  goToFirstPage(): void {
    this.goToPage(1);
  }

  goToLastPage(): void {
    this.goToPage(this.totalPages);
  }

  goToNextPage(): void {
    if (this.hasNextPage) {
      this.goToPage(this.currentPage + 1);
    }
  }

  goToPreviousPage(): void {
    if (this.hasPreviousPage) {
      this.goToPage(this.currentPage - 1);
    }
  }

  onPageSizeChange(): void {
    this.currentPage = 1; // Reset to first page when changing page size
    this.loadBoxes();
  }

  getVisiblePages(): number[] {
    const pages: number[] = [];
    const maxVisiblePages = 7;
    
    if (this.totalPages <= maxVisiblePages) {
      // Show all pages if total pages is less than max visible
      for (let i = 1; i <= this.totalPages; i++) {
        pages.push(i);
      }
    } else {
      // Show pages around current page
      const halfVisible = Math.floor(maxVisiblePages / 2);
      let startPage = Math.max(1, this.currentPage - halfVisible);
      let endPage = Math.min(this.totalPages, startPage + maxVisiblePages - 1);
      
      // Adjust start page if we're near the end
      if (endPage - startPage < maxVisiblePages - 1) {
        startPage = Math.max(1, endPage - maxVisiblePages + 1);
      }
      
      for (let i = startPage; i <= endPage; i++) {
        pages.push(i);
      }
    }
    
    return pages;
  }
}
