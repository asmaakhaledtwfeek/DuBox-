import { Component, OnInit, OnDestroy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router, RouterModule } from '@angular/router';
import { FormControl, FormGroup, ReactiveFormsModule, FormsModule, Validators, AbstractControl, ValidationErrors, ValidatorFn } from '@angular/forms';
import { debounceTime, distinctUntilChanged, skip } from 'rxjs/operators';
import { Subscription } from 'rxjs';
import { FactoryService, Factory, CreateFactoryRequest, ProjectLocation } from '../../../core/services/factory.service';
import { PermissionService } from '../../../core/services/permission.service';
import { HeaderComponent } from '../../../shared/components/header/header.component';
import { SidebarComponent } from '../../../shared/components/sidebar/sidebar.component';

// Custom Validators
function singleLetterValidator(): ValidatorFn {
  return (control: AbstractControl): ValidationErrors | null => {
    if (!control.value) {
      return null; // Don't validate empty values (use required validator for that)
    }
    const value = control.value.toString().trim();
    const isValid = /^[a-zA-Z]$/.test(value);
    return isValid ? null : { singleLetter: true };
  };
}

function minMaxRowValidator(): ValidatorFn {
  return (group: AbstractControl): ValidationErrors | null => {
    const formGroup = group as FormGroup;
    const minRow = formGroup.get('minRow')?.value;
    const maxRow = formGroup.get('maxRow')?.value;
    
    if (minRow && maxRow && minRow >= maxRow) {
      return { minMaxRow: true };
    }
    return null;
  };
}

function minMaxBayValidator(): ValidatorFn {
  return (group: AbstractControl): ValidationErrors | null => {
    const formGroup = group as FormGroup;
    const minBay = formGroup.get('minBay')?.value;
    const maxBay = formGroup.get('maxBay')?.value;
    
    if (minBay && maxBay) {
      const minBayUpper = minBay.toString().toUpperCase();
      const maxBayUpper = maxBay.toString().toUpperCase();
      
      if (minBayUpper >= maxBayUpper) {
        return { minMaxBay: true };
      }
    }
    return null;
  };
}

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
    capacity: new FormControl<number | null>(null),
    minRow: new FormControl<number | null>(null, [Validators.required, Validators.min(1)]),
    maxRow: new FormControl<number | null>(null, [Validators.required, Validators.min(1)]),
    minBay: new FormControl<string | null>(null, [Validators.required, Validators.maxLength(1), singleLetterValidator()]),
    maxBay: new FormControl<string | null>(null, [Validators.required, Validators.maxLength(1), singleLetterValidator()])
  }, { validators: [minMaxRowValidator(), minMaxBayValidator()] });

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
    this.setupCapacityAutoCalculation();
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

  private setupCapacityAutoCalculation(): void {
    // Watch for changes in minRow, maxRow, minBay, maxBay
    const minRowControl = this.factoryForm.get('minRow');
    const maxRowControl = this.factoryForm.get('maxRow');
    const minBayControl = this.factoryForm.get('minBay');
    const maxBayControl = this.factoryForm.get('maxBay');
    const capacityControl = this.factoryForm.get('capacity');

    const updateCapacity = () => {
      const minRow = minRowControl?.value;
      const maxRow = maxRowControl?.value;
      const minBay = minBayControl?.value;
      const maxBay = maxBayControl?.value;

      if (minRow && maxRow && minBay && maxBay) {
        // Convert bay letters to numbers (A=1, B=2, etc.)
        const minBayNum = minBay.toString().toUpperCase().charCodeAt(0) - 64; // A=1
        const maxBayNum = maxBay.toString().toUpperCase().charCodeAt(0) - 64;

        // Validate ranges
        if (maxRow >= minRow && maxBayNum >= minBayNum) {
          // Calculate capacity: (maxRow - minRow + 1) * (maxBay - minBay + 1)
          const rowCount = maxRow - minRow + 1;
          const bayCount = maxBayNum - minBayNum + 1;
          const calculatedCapacity = rowCount * bayCount;

          capacityControl?.setValue(calculatedCapacity, { emitEvent: false });
        }
      }
    };

    // Subscribe to value changes
    this.subscriptions.push(
      minRowControl!.valueChanges.subscribe(() => updateCapacity()),
      maxRowControl!.valueChanges.subscribe(() => updateCapacity()),
      minBayControl!.valueChanges.subscribe(() => updateCapacity()),
      maxBayControl!.valueChanges.subscribe(() => updateCapacity())
    );
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
      // Mark all fields as touched to show validation errors
      Object.keys(this.factoryForm.controls).forEach(key => {
        this.factoryForm.get(key)?.markAsTouched();
      });
      return;
    }

    this.creating = true;
    const request: CreateFactoryRequest = {
      factoryCode: this.factoryForm.value.factoryCode || '',
      factoryName: this.factoryForm.value.factoryName || '',
      location: this.factoryForm.value.location || ProjectLocation.UAE,
      capacity: this.factoryForm.value.capacity || undefined,
      minRow: this.factoryForm.value.minRow!,
      maxRow: this.factoryForm.value.maxRow!,
      minBay: this.factoryForm.value.minBay!,
      maxBay: this.factoryForm.value.maxBay!
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

