import { Component, OnInit, OnDestroy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { FormControl, ReactiveFormsModule } from '@angular/forms';
import { debounceTime, distinctUntilChanged, takeUntil } from 'rxjs/operators';
import { Subject } from 'rxjs';
import { LocationService, LocationBoxes } from '../../../core/services/location.service';
import { HeaderComponent } from '../../../shared/components/header/header.component';
import { SidebarComponent } from '../../../shared/components/sidebar/sidebar.component';

@Component({
  selector: 'app-location-details',
  standalone: true,
  imports: [CommonModule, RouterModule, ReactiveFormsModule, HeaderComponent, SidebarComponent],
  templateUrl: './location-details.component.html',
  styleUrls: ['./location-details.component.scss']
})
export class LocationDetailsComponent implements OnInit, OnDestroy {
  locationId: string = '';
  locationData: LocationBoxes | null = null;
  filteredBoxes: any[] = [];
  loading = true;
  error = '';
  searchControl = new FormControl('');
  private destroy$ = new Subject<void>();

  constructor(
    private route: ActivatedRoute,
    public router: Router,
    private locationService: LocationService
  ) {}

  ngOnInit(): void {
    this.locationId = this.route.snapshot.params['locationId'];
    if (this.locationId) {
      this.loadLocationData();
      this.setupSearch();
    } else {
      this.error = 'Location ID is missing';
      this.loading = false;
    }
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }

  private setupSearch(): void {
    this.searchControl.valueChanges
      .pipe(
        debounceTime(300),
        distinctUntilChanged(),
        takeUntil(this.destroy$)
      )
      .subscribe(() => {
        this.applyFilters();
      });
  }

  loadLocationData(): void {
    this.loading = true;
    this.error = '';
    
    this.locationService.getBoxesByLocation(this.locationId).subscribe({
      next: (data) => {
        this.locationData = data;
        this.applyFilters();
        this.loading = false;
      },
      error: (err) => {
        this.error = err.error?.message || err.message || 'Failed to load location data';
        this.loading = false;
        console.error('Error loading location data:', err);
      }
    });
  }

  applyFilters(): void {
    if (!this.locationData) {
      this.filteredBoxes = [];
      return;
    }

    const searchTerm = this.searchControl.value?.toLowerCase() || '';
    
    if (!searchTerm) {
      this.filteredBoxes = [...this.locationData.boxes];
      return;
    }

    this.filteredBoxes = this.locationData.boxes.filter(box =>
      box.boxTag?.toLowerCase().includes(searchTerm) ||
      box.boxName?.toLowerCase().includes(searchTerm) ||
      box.serialNumber?.toLowerCase().includes(searchTerm) ||
      box.projectCode?.toLowerCase().includes(searchTerm) ||
      box.boxType?.toLowerCase().includes(searchTerm) ||
      box.status?.toLowerCase().includes(searchTerm)
    );
  }

  goBack(): void {
    this.router.navigate(['/locations']);
  }

  getStatusClass(status: string): string {
    const statusMap: Record<string, string> = {
      'NotStarted': 'badge-secondary',
      'InProgress': 'badge-warning',
      'Completed': 'badge-success',
      'OnHold': 'badge-danger',
      'Dispatched': 'badge-primary',
      'Delivered': 'badge-success',
      'QAReview': 'badge-info'
    };
    return statusMap[status] || 'badge-secondary';
  }

  getStatusLabel(status: string): string {
    return status.replace(/([A-Z])/g, ' $1').trim();
  }

  getStatusPercentage(statusCount: number): number {
    if (!this.locationData || this.locationData.totalBoxes === 0) {
      return 0;
    }
    return Math.round((statusCount / this.locationData.totalBoxes) * 100);
  }

  getAllStatusCounts(): Array<{ status: string; count: number; label: string }> {
    const allStatuses = [
      { key: 'NotStarted', label: 'Not Started' },
      { key: 'InProgress', label: 'In Progress' },
      { key: 'QAReview', label: 'QA Review' },
      { key: 'Completed', label: 'Completed' },
      { key: 'ReadyForDelivery', label: 'Ready for Delivery' },
      { key: 'Delivered', label: 'Delivered' },
      { key: 'OnHold', label: 'On Hold' },
      { key: 'Dispatched', label: 'Dispatched' }
    ];

    if (!this.locationData) {
      return allStatuses.map(s => ({ status: s.key, count: 0, label: s.label }));
    }

    return allStatuses.map(status => {
      const found = this.locationData!.statusCounts.find(sc => sc.status === status.key);
      return {
        status: status.key,
        count: found ? found.count : 0,
        label: status.label
      };
    });
  }
}

