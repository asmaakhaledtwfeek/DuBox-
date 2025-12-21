import { Component, OnInit, OnDestroy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router, RouterModule } from '@angular/router';
import { FormControl, FormGroup, ReactiveFormsModule, FormsModule, Validators } from '@angular/forms';
import { debounceTime, distinctUntilChanged, skip } from 'rxjs/operators';
import { Subscription } from 'rxjs';
import { FactoryService, Factory, CreateFactoryRequest, ProjectLocation } from '../../../core/services/factory.service';
import { PermissionService } from '../../../core/services/permission.service';
import { HeaderComponent } from '../../../shared/components/header/header.component';
import { SidebarComponent } from '../../../shared/components/sidebar/sidebar.component';

@Component({
  selector: 'app-factories-management',
  standalone: true,
  imports: [CommonModule, RouterModule, ReactiveFormsModule, FormsModule, HeaderComponent, SidebarComponent],
  templateUrl: './factories-management.component.html',
  styleUrls: ['./factories-management.component.scss']
})
export class FactoriesManagementComponent implements OnInit, OnDestroy {
  factories: Factory[] = [];
  filteredFactories: Factory[] = [];
  loading = true;
  error = '';
  canCreate = false;
  
  searchControl = new FormControl('');
  showCreateModal = false;
  creating = false;

  stats = {
    total: 0,
    active: 0,
    totalCapacity: 0,
    totalOccupancy: 0,
    fullFactories: 0
  };
  
  factoryForm = new FormGroup({
    factoryCode: new FormControl('', [Validators.required, Validators.maxLength(50)]),
    factoryName: new FormControl('', [Validators.required, Validators.maxLength(200)]),
    location: new FormControl<ProjectLocation>(ProjectLocation.UAE, [Validators.required]),
    capacity: new FormControl<number | null>(null)
  });

  // Expose ProjectLocation enum to template
  readonly ProjectLocation = ProjectLocation;
  
  // Location options for dropdown
  locationOptions = [
    { value: ProjectLocation.KSA, label: 'KSA' },
    { value: ProjectLocation.UAE, label: 'UAE' }
  ];
  
  private subscriptions: Subscription[] = [];

  constructor(
    private router: Router,
    private factoryService: FactoryService,
    private permissionService: PermissionService
  ) {}

  ngOnInit(): void {
    this.checkPermissions();
    
    this.subscriptions.push(
      this.permissionService.permissions$
        .pipe(skip(1))
        .subscribe(() => {
          this.checkPermissions();
        })
    );
    
    this.loadFactories();
    this.setupSearch();
  }
  
  ngOnDestroy(): void {
    this.subscriptions.forEach(sub => sub.unsubscribe());
  }
  
  private checkPermissions(): void {
    this.canCreate = this.permissionService.canCreate('factories') || this.permissionService.canEdit('factories');
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

  loadFactories(): void {
    this.loading = true;
    this.error = '';
    
    this.factoryService.getAllFactories().subscribe({
      next: (factories) => {
        this.factories = factories;
        this.calculateStats();
        this.applyFilters();
        this.loading = false;
      },
      error: (err) => {
        this.error = err.error?.message || err.message || 'Failed to load factories';
        this.loading = false;
        console.error('Error loading factories:', err);
      }
    });
  }

  calculateStats(): void {
    this.stats = {
      total: this.factories.length,
      active: this.factories.filter(f => f.isActive).length,
      totalCapacity: this.factories
        .filter(f => f.capacity)
        .reduce((sum, f) => sum + (f.capacity || 0), 0),
      totalOccupancy: this.factories.reduce((sum, f) => sum + f.currentOccupancy, 0),
      fullFactories: this.factories.filter(f => f.isFull).length
    };
  }

  applyFilters(): void {
    let filtered = [...this.factories];
    const searchTerm = this.searchControl.value?.toLowerCase() || '';
    
    if (searchTerm) {
      filtered = filtered.filter(factory => {
        const locationText = factory.location === ProjectLocation.KSA ? 'ksa' : 'uae';
        return factory.factoryCode.toLowerCase().includes(searchTerm) ||
               factory.factoryName.toLowerCase().includes(searchTerm) ||
               locationText.includes(searchTerm);
      });
    }
    
    this.filteredFactories = filtered;
  }

  openCreateModal(): void {
    this.showCreateModal = true;
    this.factoryForm.reset({
      location: ProjectLocation.UAE
    });
  }

  closeCreateModal(): void {
    this.showCreateModal = false;
    this.factoryForm.reset({
      location: ProjectLocation.UAE
    });
  }

  onCreateFactory(): void {
    if (this.factoryForm.invalid) {
      return;
    }

    this.creating = true;
    const request: CreateFactoryRequest = {
      factoryCode: this.factoryForm.value.factoryCode || '',
      factoryName: this.factoryForm.value.factoryName || '',
      location: this.factoryForm.value.location || ProjectLocation.UAE,
      capacity: this.factoryForm.value.capacity || undefined
    };

    this.factoryService.createFactory(request).subscribe({
      next: () => {
        this.creating = false;
        this.closeCreateModal();
        this.loadFactories();
      },
      error: (err) => {
        this.error = err.error?.message || err.message || 'Failed to create factory';
        this.creating = false;
        console.error('Error creating factory:', err);
      }
    });
  }

  viewFactory(factoryId: string): void {
    this.router.navigate(['/factories', factoryId]);
  }

  getCapacityPercentage(factory: Factory): number {
    if (!factory.capacity || factory.capacity === 0) return 0;
    return Math.min(100, (factory.currentOccupancy / factory.capacity) * 100);
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
}

