import { Component, OnInit, OnDestroy, HostListener } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router, RouterModule } from '@angular/router';
import { FormControl, ReactiveFormsModule, FormsModule } from '@angular/forms';
import { debounceTime, distinctUntilChanged, skip } from 'rxjs/operators';
import { Subscription } from 'rxjs';
import { MaterialService } from '../../../core/services/material.service';
import { PermissionService } from '../../../core/services/permission.service';
import { ProjectService } from '../../../core/services/project.service';
import { Material, CreateMaterial } from '../../../core/models/material.model';
import { HeaderComponent } from '../../../shared/components/header/header.component';
import { SidebarComponent } from '../../../shared/components/sidebar/sidebar.component';

// Import the Project model instead of defining a new interface
import { Project } from '../../../core/models/project.model';

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
  loading = false;
  error = '';
  canCreate = false;
  canEdit = false;
  
  // Project Selection
  projects: Project[] = [];
  selectedProject: Project | null = null;
  projectsLoading = false;
  
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
  
  // Create Material Modal
  showCreateForm = false;
  createMaterialError = '';
  creatingMaterial = false;
  newMaterial: CreateMaterial = {
    materialCode: '',
    materialName: '',
    materialCategory: '',
    unit: '',
    unitCost: 0,
    currentStock: 0,
    minimumStock: 0,
    reorderLevel: 0,
    supplierName: '',
    projectId: ''
  };

  // Success Modal
  showSuccessModal = false;
  successMessage = '';
  
  private subscriptions: Subscription[] = [];

  constructor(
    private router: Router,
    private materialService: MaterialService,
    private permissionService: PermissionService,
    private projectService: ProjectService
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
    
    this.loadProjects();
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

  loadProjects(): void {
    this.projectsLoading = true;
    
    this.projectService.getProjects().subscribe({
      next: (projects) => {
        this.projects = projects;
        this.projectsLoading = false;
        console.log('✅ Loaded projects:', projects);
      },
      error: (err) => {
        console.error('Error loading projects:', err);
        this.projectsLoading = false;
      }
    });
  }

  onProjectSelect(project: Project | null): void {
    this.selectedProject = project;
    if (this.selectedProject) {
      this.loadMaterials();
    } else {
      this.materials = [];
      this.filteredMaterials = [];
    }
  }

  loadMaterials(): void {
    if (!this.selectedProject) {
      this.materials = [];
      return;
    }

    this.loading = true;
    this.error = '';
    
    // Load materials filtered by project using backend endpoint
    this.materialService.getMaterialsByProject(this.selectedProject.id).subscribe({
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
    if (!this.selectedProject) {
      return;
    }
    
    this.showCreateForm = true;
    this.createMaterialError = '';
    this.newMaterial = {
      materialCode: '',
      materialName: '',
      materialCategory: '',
      unit: '',
      unitCost: 0,
      currentStock: 0,
      minimumStock: 0,
      reorderLevel: 0,
      supplierName: '',
      projectId: this.selectedProject.id
    };
  }

  closeCreateForm(): void {
    this.showCreateForm = false;
    this.createMaterialError = '';
  }

  saveMaterial(): void {
    if (!this.newMaterial.materialCode || !this.newMaterial.materialName) {
      this.createMaterialError = 'Please fill in all required fields';
      return;
    }

    this.createMaterialError = '';
    this.creatingMaterial = true;

    this.materialService.createMaterial(this.newMaterial).subscribe({
      next: (response) => {
        this.creatingMaterial = false;
        this.closeCreateForm();
        this.successMessage = 'Material created successfully!';
        this.showSuccessModal = true;
        this.loadMaterials();
      },
      error: (err) => {
        this.creatingMaterial = false;
        const errorMessage = err.error?.error?.message || err.error?.message || err.message || 'Unknown error occurred';
        this.createMaterialError = `Failed to create material: ${errorMessage}`;
        console.error('Error creating material:', err);
      }
    });
  }

  closeSuccessModal(): void {
    this.showSuccessModal = false;
    this.successMessage = '';
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

  @HostListener('document:click', ['$event'])
  onDocumentClick(event: Event): void {
    // Can be used for dropdown interactions if needed
  }
}

