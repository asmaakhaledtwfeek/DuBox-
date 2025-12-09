import { Component, OnInit, OnDestroy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router, RouterModule } from '@angular/router';
import { FormControl, ReactiveFormsModule, FormsModule } from '@angular/forms';
import { debounceTime, distinctUntilChanged, skip } from 'rxjs/operators';
import { Subscription } from 'rxjs';
import { MaterialService } from '../../../core/services/material.service';
import { PermissionService } from '../../../core/services/permission.service';
import { Material } from '../../../core/models/material.model';
import { HeaderComponent } from '../../../shared/components/header/header.component';
import { SidebarComponent } from '../../../shared/components/sidebar/sidebar.component';

@Component({
  selector: 'app-materials-dashboard',
  standalone: true,
  imports: [CommonModule, RouterModule, ReactiveFormsModule, FormsModule, HeaderComponent, SidebarComponent],
  templateUrl: './materials-dashboard.component.html',
  styleUrls: ['./materials-dashboard.component.scss']
})
export class MaterialsDashboardComponent implements OnInit, OnDestroy {
  materials: Material[] = [];
  filteredMaterials: Material[] = [];
  loading = true;
  error = '';
  canCreate = false;
  canEdit = false;
  
  searchControl = new FormControl('');
  selectedCategory: string = 'All';
  showLowStockOnly = false;
  
  categories: string[] = [];
  stats = {
    total: 0,
    lowStock: 0,
    needsReorder: 0,
    active: 0
  };
  
  private subscriptions: Subscription[] = [];

  constructor(
    private router: Router,
    private materialService: MaterialService,
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
          console.log('🔄 Permissions updated, re-checking materials permissions');
          this.checkPermissions();
        })
    );
    
    this.loadMaterials();
    this.setupSearch();
  }
  
  ngOnDestroy(): void {
    this.subscriptions.forEach(sub => sub.unsubscribe());
  }
  
  private checkPermissions(): void {
    this.canCreate = this.permissionService.canCreate('materials');
    this.canEdit = this.permissionService.canEdit('materials');
    console.log('✅ Materials permissions checked:', { canCreate: this.canCreate, canEdit: this.canEdit });
  }

  private setupSearch(): void {
    this.searchControl.valueChanges
      .pipe(
        debounceTime(300),
        distinctUntilChanged()
      )
      .subscribe(searchTerm => {
        this.applyFilters();
      });
  }

  loadMaterials(): void {
    this.loading = true;
    this.error = '';
    
    this.materialService.getMaterials().subscribe({
      next: (materials) => {
        this.materials = materials;
        this.extractCategories();
        this.calculateStats();
        this.applyFilters();
        this.loading = false;
      },
      error: (err) => {
        this.error = err.error?.message || err.message || 'Failed to load materials';
        this.loading = false;
        console.error('Error loading materials:', err);
      }
    });
  }

  extractCategories(): void {
    const categorySet = new Set<string>();
    this.materials.forEach(m => {
      if (m.materialCategory) {
        categorySet.add(m.materialCategory);
      }
    });
    this.categories = Array.from(categorySet).sort();
  }

  calculateStats(): void {
    this.stats = {
      total: this.materials.length,
      lowStock: this.materials.filter(m => m.isLowStock).length,
      needsReorder: this.materials.filter(m => m.needsReorder).length,
      active: this.materials.filter(m => m.isActive).length
    };
  }

  applyFilters(): void {
    let filtered = [...this.materials];
    
    // Search filter
    const searchTerm = this.searchControl.value?.toLowerCase() || '';
    if (searchTerm) {
      filtered = filtered.filter(m => 
        m.materialCode.toLowerCase().includes(searchTerm) ||
        m.materialName.toLowerCase().includes(searchTerm) ||
        (m.materialCategory && m.materialCategory.toLowerCase().includes(searchTerm)) ||
        (m.supplierName && m.supplierName.toLowerCase().includes(searchTerm))
      );
    }
    
    // Category filter
    if (this.selectedCategory !== 'All') {
      filtered = filtered.filter(m => m.materialCategory === this.selectedCategory);
    }
    
    // Low stock filter
    if (this.showLowStockOnly) {
      filtered = filtered.filter(m => m.isLowStock || m.needsReorder);
    }
    
    this.filteredMaterials = filtered;
  }

  onCategoryChange(): void {
    this.applyFilters();
  }

  onLowStockToggle(): void {
    this.applyFilters();
  }

  createMaterial(): void {
    this.router.navigate(['/materials/create']);
  }

  viewDetails(materialId: string): void {
    this.router.navigate(['/materials', materialId]);
  }

  editMaterial(materialId: string): void {
    this.router.navigate(['/materials', materialId, 'edit']);
  }

  getStockStatusClass(material: Material): string {
    if (material.needsReorder) return 'status-critical';
    if (material.isLowStock) return 'status-warning';
    return 'status-ok';
  }

  getStockStatusText(material: Material): string {
    if (material.needsReorder) return 'Critical';
    if (material.isLowStock) return 'Low';
    return 'OK';
  }
}

