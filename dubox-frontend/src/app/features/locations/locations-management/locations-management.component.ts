import { Component, OnInit, OnDestroy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router, RouterModule } from '@angular/router';
import { FormControl, FormGroup, ReactiveFormsModule, FormsModule, Validators } from '@angular/forms';
import { debounceTime, distinctUntilChanged, skip } from 'rxjs/operators';
import { Subscription } from 'rxjs';
import { LocationService, FactoryLocation, CreateLocationRequest } from '../../../core/services/location.service';
import { PermissionService } from '../../../core/services/permission.service';
import { HeaderComponent } from '../../../shared/components/header/header.component';
import { SidebarComponent } from '../../../shared/components/sidebar/sidebar.component';

@Component({
  selector: 'app-locations-management',
  standalone: true,
  imports: [CommonModule, RouterModule, ReactiveFormsModule, FormsModule, HeaderComponent, SidebarComponent],
  templateUrl: './locations-management.component.html',
  styleUrls: ['./locations-management.component.scss']
})
export class LocationsManagementComponent implements OnInit, OnDestroy {
  locations: FactoryLocation[] = [];
  filteredLocations: FactoryLocation[] = [];
  loading = true;
  error = '';
  canCreate = false;
  
  searchControl = new FormControl('');
  showCreateModal = false;
  creating = false;
  checkingExistence = false;
  locationExists = false;
  locationExistsMessage = '';

  stats = {
    total: 0,
    active: 0,
    totalCapacity: 0,
    totalOccupancy: 0,
    fullLocations: 0
  };
  
  locationForm = new FormGroup({
    locationCode: new FormControl('', [Validators.required, Validators.maxLength(50)]),
    locationName: new FormControl('', [Validators.required, Validators.maxLength(200)]),
    locationType: new FormControl('', [Validators.maxLength(50)]),
    bay: new FormControl('', [Validators.maxLength(50)]),
    row: new FormControl('', [Validators.maxLength(50)]),
    position: new FormControl('', [Validators.maxLength(50)]),
    capacity: new FormControl<number | null>(null)
  });
  
  private subscriptions: Subscription[] = [];

  constructor(
    private router: Router,
    private locationService: LocationService,
    private permissionService: PermissionService
  ) {}

  ngOnInit(): void {
    // Check permissions immediately
    this.checkPermissions();
    
    // Subscribe to permission changes to update UI when permissions are loaded
    this.subscriptions.push(
      this.permissionService.permissions$
        .pipe(skip(1)) // Skip initial empty value
        .subscribe(() => {
          console.log('🔄 Permissions updated, re-checking locations permissions');
          this.checkPermissions();
        })
    );
    
    this.loadLocations();
    this.setupSearch();
    this.setupLocationCodeCheck();
  }
  
  ngOnDestroy(): void {
    this.subscriptions.forEach(sub => sub.unsubscribe());
  }
  
  private checkPermissions(): void {
    this.canCreate = this.permissionService.canCreate('locations') || this.permissionService.canEdit('locations');
    console.log('✅ Can create location:', this.canCreate);
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
  }

  private setupLocationCodeCheck(): void {
    this.locationForm.get('locationCode')?.valueChanges
      .pipe(
        debounceTime(500),
        distinctUntilChanged()
      )
      .subscribe(code => {
        if (code && code.trim().length > 0) {
          this.checkLocationExists(code.trim());
        } else {
          this.locationExists = false;
          this.locationExistsMessage = '';
        }
      });
  }

  loadLocations(): void {
    this.loading = true;
    this.error = '';
    
    this.locationService.getLocations().subscribe({
      next: (locations) => {
        this.locations = locations;
        this.calculateStats();
        this.applyFilters();
        this.loading = false;
      },
      error: (err) => {
        this.error = err.error?.message || err.message || 'Failed to load locations';
        this.loading = false;
        console.error('Error loading locations:', err);
      }
    });
  }

  calculateStats(): void {
    this.stats = {
      total: this.locations.length,
      active: this.locations.filter(l => l.isActive).length,
      totalCapacity: this.locations
        .filter(l => l.capacity)
        .reduce((sum, l) => sum + (l.capacity || 0), 0),
      totalOccupancy: this.locations.reduce((sum, l) => sum + l.currentOccupancy, 0),
      fullLocations: this.locations.filter(l => l.isFull).length
    };
  }

  checkLocationExists(locationCode: string): void {
    this.checkingExistence = true;
    this.locationExists = false;
    this.locationExistsMessage = '';
    
    this.locationService.checkLocationExists(locationCode).subscribe({
      next: (exists) => {
        this.locationExists = exists;
        this.locationExistsMessage = exists 
          ? 'This location code already exists' 
          : 'Location code is available';
        this.checkingExistence = false;
      },
      error: (err) => {
        console.error('Error checking location existence:', err);
        this.checkingExistence = false;
      }
    });
  }

  applyFilters(): void {
    let filtered = [...this.locations];
    const searchTerm = this.searchControl.value?.toLowerCase() || '';
    
    if (searchTerm) {
      filtered = filtered.filter(location =>
        location.locationCode.toLowerCase().includes(searchTerm) ||
        location.locationName.toLowerCase().includes(searchTerm) ||
        location.locationType?.toLowerCase().includes(searchTerm) ||
        location.bay?.toLowerCase().includes(searchTerm) ||
        location.row?.toLowerCase().includes(searchTerm) ||
        location.position?.toLowerCase().includes(searchTerm)
      );
    }
    
    this.filteredLocations = filtered;
  }

  openCreateModal(): void {
    this.showCreateModal = true;
    this.locationForm.reset();
    this.locationExists = false;
    this.locationExistsMessage = '';
  }

  closeCreateModal(): void {
    this.showCreateModal = false;
    this.locationForm.reset();
    this.locationExists = false;
    this.locationExistsMessage = '';
  }

  createLocation(): void {
    if (this.locationForm.invalid) {
      this.locationForm.markAllAsTouched();
      return;
    }

    if (this.locationExists) {
      return;
    }

    this.creating = true;
    const formValue = this.locationForm.value;
    
    const request: CreateLocationRequest = {
      locationCode: formValue.locationCode!.trim(),
      locationName: formValue.locationName!.trim(),
      locationType: formValue.locationType?.trim() || undefined,
      bay: formValue.bay?.trim() || undefined,
      row: formValue.row?.trim() || undefined,
      position: formValue.position?.trim() || undefined,
      capacity: formValue.capacity || undefined
    };

    this.locationService.createLocation(request).subscribe({
      next: (location) => {
        this.locations.push(location);
        this.calculateStats();
        this.applyFilters();
        this.closeCreateModal();
        this.creating = false;
      },
      error: (err) => {
        this.error = err.error?.message || err.message || 'Failed to create location';
        this.creating = false;
        console.error('Error creating location:', err);
      }
    });
  }

  viewLocation(locationId: string): void {
    this.router.navigate(['/locations', locationId]);
  }

  getCapacityDisplay(location: FactoryLocation): string {
    if (!location.capacity) {
      return 'Unlimited';
    }
    return `${location.currentOccupancy} / ${location.capacity}`;
  }

  getCapacityPercentage(location: FactoryLocation): number {
    if (!location.capacity) {
      return 0;
    }
    return (location.currentOccupancy / location.capacity) * 100;
  }
}

