import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router, ActivatedRoute, RouterModule } from '@angular/router';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule, FormArray, FormsModule } from '@angular/forms';
import { MaterialTemplateService } from '../../../core/services/material-template.service';
import { MaterialService } from '../../../core/services/material.service';
import { Material } from '../../../core/models/material.model';
import { MaterialTemplateItemDto } from '../../../core/models/material-template.model';
import { HeaderComponent } from '../../../shared/components/header/header.component';
import { SidebarComponent } from '../../../shared/components/sidebar/sidebar.component';

@Component({
  selector: 'app-create-material-template',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, FormsModule, RouterModule, HeaderComponent, SidebarComponent],
  templateUrl: './create-material-template.component.html',
  styleUrl: './create-material-template.component.scss'
})
export class CreateMaterialTemplateComponent implements OnInit {
  templateForm!: FormGroup;
  isEditMode = false;
  templateId: string | null = null;
  
  // Material selection
  availableMaterials: Material[] = [];
  filteredMaterials: Material[] = [];
  groupedMaterials: Map<string, Material[]> = new Map();
  materialSearchTerm = '';
  selectedCategory = '';
  categories: string[] = [];
  units: string[] = [];
  showMaterialSelector = false;
  showGroupedView = true;
  expandedCategories: Set<string> = new Set();
  
  // Materials list collapse state
  materialsListCollapsed = false;
  
  // Grouped selected materials
  expandedMaterialCategories: Set<string> = new Set();
  
  // Confirmation modal
  showConfirmModal = false;
  confirmModalData: {
    title: string;
    message: string;
    confirmText: string;
    cancelText: string;
    onConfirm: () => void;
  } | null = null;
  
  // Custom material form
  showCustomMaterialForm = false;
  customMaterialForm!: FormGroup;
  customMaterialCategory = '';
  customMaterialError: string | null = null;
  savingCustomMaterial = false;
  
  // Loading and error states
  loading = false;
  loadingMaterials = false;
  submitting = false;
  error: string | null = null;
  materialError: string | null = null;

  constructor(
    private fb: FormBuilder,
    private templateService: MaterialTemplateService,
    private materialService: MaterialService,
    private router: Router,
    private route: ActivatedRoute
  ) {}

  ngOnInit(): void {
    this.initializeForm();
    this.initializeCustomMaterialForm();
    this.loadAvailableMaterials();
    
    // Check if edit mode
    this.templateId = this.route.snapshot.paramMap.get('id');
    if (this.templateId) {
      this.isEditMode = true;
      this.loadTemplate();
    }
  }

  initializeForm(): void {
    this.templateForm = this.fb.group({
      templateName: ['', Validators.required],
      templateCode: ['', Validators.required],
      description: [''],
      category: [''],
      items: this.fb.array([])
    });
  }

  initializeCustomMaterialForm(): void {
    this.customMaterialForm = this.fb.group({
      materialName: ['', Validators.required],
      materialCode: ['', Validators.required],
      materialCategory: [''],
      unit: ['', Validators.required],
      quantityPerBox: [null],
      defaultRequiredBeforeDays: [7, [Validators.required, Validators.min(1)]],
      notes: ['']
    });
  }

  get items(): FormArray {
    return this.templateForm.get('items') as FormArray;
  }

  loadTemplate(): void {
    if (!this.templateId) return;
    
    this.loading = true;
    this.error = null;

    this.templateService.getTemplateById(this.templateId).subscribe({
      next: (template) => {
        this.templateForm.patchValue({
          templateName: template.templateName,
          templateCode: template.templateCode,
          description: template.description,
          category: template.category
        });
        
        // In edit mode, disable template code (shouldn't be changed)
        this.templateForm.get('templateCode')?.disable();
        
        // Load template items
        if (template.items && template.items.length > 0) {
          template.items.forEach((item: any) => {
            this.addItemToForm(item);
          });
        }
        
        this.loading = false;
      },
      error: (err) => {
        this.error = 'Failed to load template';
        this.loading = false;
        console.error('Material Template - Error loading template:', err);
      }
    });
  }

  loadAvailableMaterials(): void {
    this.loadingMaterials = true;
    this.materialError = null;

    this.templateService.getAvailableMaterials().subscribe({
      next: (materials) => {
        this.availableMaterials = materials || [];
        console.log('Available materials loaded:', this.availableMaterials);
        console.log('Sample material:', this.availableMaterials[0]);
        this.filteredMaterials = this.availableMaterials;
        this.extractCategories();
        this.extractUnits();
        this.loadingMaterials = false;
      },
      error: (err) => {
        this.materialError = 'Failed to load materials';
        this.loadingMaterials = false;
        console.error('Error loading materials:', err);
      }
    });
  }

  extractCategories(): void {
    const categorySet = new Set<string>();
    this.availableMaterials.forEach(m => {
      if (m.materialCategory) {
        categorySet.add(m.materialCategory);
      }
    });
    this.categories = Array.from(categorySet).sort();
  }

  extractUnits(): void {
    const unitSet = new Set<string>();
    this.availableMaterials.forEach(m => {
      if (m.unit) {
        unitSet.add(m.unit);
      }
    });
    this.units = Array.from(unitSet).sort();
  }

  filterMaterials(): void {
    this.filteredMaterials = this.availableMaterials.filter(m => {
      const matchesSearch = !this.materialSearchTerm || 
        m.materialName.toLowerCase().includes(this.materialSearchTerm.toLowerCase()) ||
        m.materialCode.toLowerCase().includes(this.materialSearchTerm.toLowerCase());
      
      const matchesCategory = !this.selectedCategory || m.materialCategory === this.selectedCategory;
      
      // Don't show materials that are already added
      const notAlreadyAdded = !this.items.value.some((item: any) => item.materialId === m.materialId);
      
      return matchesSearch && matchesCategory && notAlreadyAdded;
    });
    
    this.groupMaterialsByCategory();
  }

  groupMaterialsByCategory(): void {
    this.groupedMaterials.clear();
    this.filteredMaterials.forEach(material => {
      const category = material.materialCategory || 'Uncategorized';
      if (!this.groupedMaterials.has(category)) {
        this.groupedMaterials.set(category, []);
      }
      this.groupedMaterials.get(category)!.push(material);
    });
  }

  getCategoryKeys(): string[] {
    return Array.from(this.groupedMaterials.keys()).sort();
  }

  getMaterialsInCategory(category: string): Material[] {
    return this.groupedMaterials.get(category) || [];
  }

  isCategoryExpanded(category: string): boolean {
    return this.expandedCategories.has(category);
  }

  toggleCategory(category: string): void {
    if (this.expandedCategories.has(category)) {
      this.expandedCategories.delete(category);
    } else {
      this.expandedCategories.add(category);
    }
  }

  expandAllCategories(): void {
    this.getCategoryKeys().forEach(cat => this.expandedCategories.add(cat));
  }

  collapseAllCategories(): void {
    this.expandedCategories.clear();
  }

  toggleMaterialsList(): void {
    this.materialsListCollapsed = !this.materialsListCollapsed;
  }

  // Group selected materials by category
  getGroupedSelectedMaterials(): Map<string, number[]> {
    const grouped = new Map<string, number[]>();
    
    this.items.controls.forEach((item, index) => {
      const category = item.get('materialCategory')?.value || 'Uncategorized';
      if (!grouped.has(category)) {
        grouped.set(category, []);
      }
      grouped.get(category)!.push(index);
    });
    
    return grouped;
  }

  getSelectedMaterialCategoryKeys(): string[] {
    return Array.from(this.getGroupedSelectedMaterials().keys()).sort();
  }

  getMaterialIndicesInCategory(category: string): number[] {
    return this.getGroupedSelectedMaterials().get(category) || [];
  }

  isSelectedCategoryExpanded(category: string): boolean {
    return this.expandedMaterialCategories.has(category);
  }

  toggleSelectedCategory(category: string): void {
    if (this.expandedMaterialCategories.has(category)) {
      this.expandedMaterialCategories.delete(category);
    } else {
      this.expandedMaterialCategories.add(category);
    }
  }

  expandAllSelectedCategories(): void {
    this.getSelectedMaterialCategoryKeys().forEach(cat => this.expandedMaterialCategories.add(cat));
  }

  collapseAllSelectedCategories(): void {
    this.expandedMaterialCategories.clear();
  }

  removeCategoryMaterials(category: string): void {
    const indices = this.getMaterialIndicesInCategory(category);
    if (indices.length === 0) return;
    
    // Show modern confirmation modal
    this.confirmModalData = {
      title: 'Remove Materials',
      message: `Are you sure you want to remove all ${indices.length} material(s) from ${category}?`,
      confirmText: 'Remove All',
      cancelText: 'Cancel',
      onConfirm: () => {
        // Remove in reverse order to maintain correct indices
        indices.sort((a, b) => b - a).forEach(index => {
          this.items.removeAt(index);
        });
        
        // Update display orders after bulk removal
        this.updateDisplayOrders();
        
        // Force change detection to ensure UI updates
        this.items.markAsDirty();
        this.templateForm.markAsDirty();
        this.templateForm.updateValueAndValidity();
        
        this.closeConfirmModal();
      }
    };
    this.showConfirmModal = true;
  }

  closeConfirmModal(): void {
    this.showConfirmModal = false;
    this.confirmModalData = null;
  }

  confirmAction(): void {
    if (this.confirmModalData?.onConfirm) {
      this.confirmModalData.onConfirm();
    }
  }

  selectAllInCategory(category: string): void {
    const materials = this.groupedMaterials.get(category) || [];
    materials.forEach(material => {
      this.addMaterialSilently(material);
    });
    this.filterMaterials(); // Refresh to hide added materials
  }

  addMaterialSilently(material: Material): void {
    // Check if already added
    const alreadyAdded = this.items.value.some((item: any) => item.materialId === material.materialId);
    if (alreadyAdded) return;

    const itemGroup = this.fb.group({
      materialId: [material.materialId, Validators.required],
      materialCode: [material.materialCode],
      materialName: [material.materialName],
      materialCategory: [material.materialCategory],
      materialUnit: [material.unit],
      materialQuantityPerBox: [material.quantityPerBox],
      requiredBeforeDays: [material.defaultRequiredBeforeDays || 7, [Validators.required, Validators.min(1)]],
      isRequired: [true],
      notes: [''],
      displayOrder: [this.items.length + 1]
    });
    
    this.items.push(itemGroup);
  }

  addMaterial(material: Material): void {
    this.addMaterialSilently(material);
    this.filterMaterials(); // Refresh to hide added material
  }

  addItemToForm(item: any): void {
    const itemGroup = this.fb.group({
      materialId: [item.materialId, Validators.required],
      materialCode: [item.materialCode],
      materialName: [item.materialName],
      materialCategory: [item.materialCategory],
      materialUnit: [item.unit || item.materialUnit],
      materialQuantityPerBox: [item.quantityPerBox || item.materialQuantityPerBox],
      requiredBeforeDays: [item.requiredBeforeDays, [Validators.required, Validators.min(1)]],
      isRequired: [item.isRequired],
      notes: [item.notes || ''],
      displayOrder: [item.displayOrder]
    });
    
    this.items.push(itemGroup);
  }

  removeItem(index: number): void {
    try {
      // Validate index
      if (index < 0 || index >= this.items.length) {
        console.error('Material Template - Invalid index for removal:', index);
        return;
      }
      
      // Log removal for debugging (can be removed in production)
      const itemToRemove = this.items.at(index);
      console.log('Material Template - Removing item:', {
        index,
        materialCode: itemToRemove.get('materialCode')?.value,
        totalItemsBefore: this.items.length
      });
      
      // Remove the item from the form array
      this.items.removeAt(index);
      
      // Update display orders for remaining items
      this.updateDisplayOrders();
      
      // Force change detection to ensure UI updates
      this.items.markAsDirty();
      this.templateForm.markAsDirty();
      this.templateForm.updateValueAndValidity();
      
    } catch (error) {
      console.error('Material Template - Error removing item:', error);
    }
  }

  moveItemUp(index: number): void {
    if (index === 0) return;
    const item = this.items.at(index);
    this.items.removeAt(index);
    this.items.insert(index - 1, item);
    this.updateDisplayOrders();
  }

  moveItemDown(index: number): void {
    if (index === this.items.length - 1) return;
    const item = this.items.at(index);
    this.items.removeAt(index);
    this.items.insert(index + 1, item);
    this.updateDisplayOrders();
  }

  updateDisplayOrders(): void {
    this.items.controls.forEach((control, index) => {
      control.patchValue({ displayOrder: index + 1 });
    });
  }

  openMaterialSelector(): void {
    this.showMaterialSelector = true;
    this.materialSearchTerm = '';
    this.selectedCategory = '';
    this.filterMaterials();
  }

  closeMaterialSelector(): void {
    this.showMaterialSelector = false;
  }

  onSubmit(): void {
    if (this.templateForm.invalid) {
      this.markFormGroupTouched(this.templateForm);
      return;
    }

    if (this.items.length === 0) {
      this.error = 'Please add at least one material to the template';
      return;
    }

    this.submitting = true;
    this.error = null;

    const formValue = this.templateForm.getRawValue(); // Use getRawValue to include disabled fields
    const items: MaterialTemplateItemDto[] = formValue.items.map((item: any) => ({
      materialId: item.materialId,
      requiredBeforeDays: item.requiredBeforeDays,
      isRequired: item.isRequired,
      notes: item.notes,
      displayOrder: item.displayOrder
    }));

    if (this.isEditMode && this.templateId) {
      // Update existing template
      const updateRequest = {
        templateName: formValue.templateName,
        description: formValue.description,
        category: formValue.category,
        items: items
      };

      this.templateService.updateTemplate(this.templateId, updateRequest).subscribe({
        next: () => {
          this.submitting = false;
          this.router.navigate(['/schedule/material-templates']);
        },
        error: (err) => {
          this.submitting = false;
          this.error = err.error?.error || 'Failed to update template';
          console.error('Error updating template:', err);
        }
      });
    } else {
      // Create new template
      const createRequest = {
        templateName: formValue.templateName,
        templateCode: formValue.templateCode,
        description: formValue.description,
        category: formValue.category,
        items: items
      };

      this.templateService.createTemplate(createRequest).subscribe({
        next: () => {
          this.submitting = false;
          this.router.navigate(['/schedule/material-templates']);
        },
        error: (err) => {
          this.submitting = false;
          this.error = err.error?.error || 'Failed to create template';
          console.error('Error creating template:', err);
        }
      });
    }
  }

  cancel(): void {
    this.router.navigate(['/schedule/material-templates']);
  }

  private markFormGroupTouched(formGroup: FormGroup): void {
    Object.keys(formGroup.controls).forEach(key => {
      const control = formGroup.get(key);
      control?.markAsTouched();

      if (control instanceof FormGroup) {
        this.markFormGroupTouched(control);
      }
    });
  }

  isFieldInvalid(fieldName: string): boolean {
    const field = this.templateForm.get(fieldName);
    return !!(field && field.invalid && field.touched);
  }

  trackByIndex(index: number): number {
    return index;
  }

  trackByMaterialId(index: number, item: any): string {
    return item.get('materialId')?.value || index;
  }

  // Custom Material Methods
  openCustomMaterialForm(category: string): void {
    this.customMaterialCategory = category;
    this.customMaterialForm.patchValue({
      materialCategory: category,
      materialName: '',
      materialCode: '',
      unit: '',
      quantityPerBox: null,
      defaultRequiredBeforeDays: 7,
      notes: ''
    });
    this.customMaterialForm.markAsUntouched();
    this.customMaterialError = null;
    this.showCustomMaterialForm = true;
  }

  closeCustomMaterialForm(): void {
    this.showCustomMaterialForm = false;
    this.customMaterialForm.reset();
    this.customMaterialCategory = '';
    this.customMaterialError = null;
  }

  saveCustomMaterial(): void {
    if (this.customMaterialForm.invalid) {
      this.markFormGroupTouched(this.customMaterialForm);
      return;
    }

    this.savingCustomMaterial = true;
    this.customMaterialError = null;

    const customMaterial = this.customMaterialForm.value;

    // Create a material object to save to the backend
    this.materialService.createMaterial(customMaterial).subscribe({
      next: (createdMaterial) => {
        // Add the created material to available materials
        this.availableMaterials.push(createdMaterial);
        
        // Add it to the template items
        this.addMaterialSilently(createdMaterial);
        
        // Refresh the categories, units, and filters
        this.extractCategories();
        this.extractUnits();
        this.filterMaterials();
        
        // Close the form
        this.savingCustomMaterial = false;
        this.closeCustomMaterialForm();
      },
      error: (err) => {
        this.savingCustomMaterial = false;
        this.customMaterialError = err.error?.message || err.error?.error || 'Failed to create custom material';
        console.error('Error creating custom material:', err);
      }
    });
  }
}

