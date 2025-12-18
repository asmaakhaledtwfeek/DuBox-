import { Component, OnInit, OnDestroy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { FormControl, ReactiveFormsModule } from '@angular/forms';
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
  imports: [CommonModule, RouterModule, ReactiveFormsModule, HeaderComponent, SidebarComponent],
  templateUrl: './boxes-list.component.html',
  styleUrls: ['./boxes-list.component.scss']
})
export class BoxesListComponent implements OnInit, OnDestroy {
  projectId: string = '';
  projectName = '';
  projectCode = '';
  boxes: Box[] = [];
  filteredBoxes: Box[] = [];
  boxTypes: BoxTypeStat[] = [];
  filteredBoxTypes: BoxTypeStat[] = [];
  selectedBoxType: string | null = null;
  showBoxTypes = true;
  loading = true;
  error = '';
  canCreate = false;
  
  searchControl = new FormControl('');
  boxTypeSearchControl = new FormControl('');
  selectedStatus: BoxStatus | 'All' = 'All';
  BoxStatus = BoxStatus;
  
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
    
    if (boxType) {
      this.selectedBoxType = boxType;
      this.showBoxTypes = false;
      this.loadBoxes();
    } else {
      this.loadBoxTypes();
    }
    
    this.setupSearch();
  }
  
  ngOnDestroy(): void {
    this.subscriptions.forEach(sub => sub.unsubscribe());
  }
  
  private checkPermissions(): void {
    this.canCreate = this.permissionService.canCreate('boxes');
    console.log('✅ Can create box:', this.canCreate);
  }

  loadProjectDetails(): void {
    if (!this.projectId) {
      return;
    }

    this.projectService.getProject(this.projectId).subscribe({
      next: (project) => {
        this.projectName = project.name || '';
        this.projectCode = project.code || '';
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
    
    this.boxService.getBoxTypeStatsByProject(this.projectId).subscribe({
      next: (response) => {
        this.boxTypes = response.boxTypeStats || [];
        this.filteredBoxTypes = [...this.boxTypes];
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

  loadBoxes(): void {
    this.loading = true;
    this.error = '';
    
    this.boxService.getBoxesByProject(this.projectId).subscribe({
      next: (boxes) => {
        // Filter boxes by selected type if a type is selected
        if (this.selectedBoxType) {
          this.boxes = boxes.filter(box => box.type === this.selectedBoxType);
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

  viewBoxType(boxType: string): void {
    this.selectedBoxType = boxType;
    this.showBoxTypes = false;
    this.boxTypeSearchControl.setValue('');
    this.router.navigate([], {
      relativeTo: this.route,
      queryParams: { boxType: boxType },
      queryParamsHandling: 'merge'
    });
    this.loadBoxes();
  }

  backToBoxTypes(): void {
    this.selectedBoxType = null;
    this.showBoxTypes = true;
    this.boxes = [];
    this.filteredBoxes = [];
    this.searchControl.setValue('');
    this.selectedStatus = 'All';
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

  private applyFilters(): void {
    let filtered = [...this.boxes];

    // Apply status filter
    if (this.selectedStatus !== 'All') {
      filtered = filtered.filter(box => box.status === this.selectedStatus);
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

  private applyBoxTypeFilters(): void {
    const searchTerm = this.boxTypeSearchControl.value?.toLowerCase() || '';
    
    if (!searchTerm) {
      this.filteredBoxTypes = [...this.boxTypes];
      return;
    }

    this.filteredBoxTypes = this.boxTypes.filter(boxType => 
      boxType.boxType?.toLowerCase().includes(searchTerm)
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

  // Expose Math to template
  Math = Math;
}
