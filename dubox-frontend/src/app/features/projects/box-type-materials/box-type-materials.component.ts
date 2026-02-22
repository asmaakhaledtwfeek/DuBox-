import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { BoxTypeMaterialService } from '../../../core/services/box-type-material.service';
import { BoxTypeMaterial } from '../../../core/models/box-type-material.model';
import { ProjectService } from '../../../core/services/project.service';
import { BoxService } from '../../../core/services/box.service';
import { HeaderComponent } from '../../../shared/components/header/header.component';
import { SidebarComponent } from '../../../shared/components/sidebar/sidebar.component';
import * as ExcelJS from 'exceljs';

@Component({
  selector: 'app-box-type-materials',
  standalone: true,
  imports: [CommonModule, RouterModule, FormsModule, HeaderComponent, SidebarComponent],
  templateUrl: './box-type-materials.component.html',
  styleUrl: './box-type-materials.component.scss'
})
export class BoxTypeMaterialsComponent implements OnInit {
  projectId!: string;
  projectName: string = '';
  materials: BoxTypeMaterial[] = [];
  filteredMaterials: BoxTypeMaterial[] = [];
  loading = false;
  error = '';
  successMessage = '';
  hasErrors = false;
  // Filters
  searchTerm = '';
  selectedBoxType: string = 'all';
  selectedStatus: string = 'all';
  selectedMaterialCode: string = 'all';
  selectedBuilding: string = 'all';
  selectedLevel: string = 'all';
  
  // Available buildings and levels
  availableBuildings: string[] = [];
  availableLevels: string[] = [];
  
  // Grouping
  groupedByBoxType: { [boxTypeName: string]: BoxTypeMaterial[] } = {};
  
  // Collapse state
  collapsedBoxTypes: { [boxTypeName: string]: boolean } = {};
  
  // Modal state
  isMarkArrivedModalOpen = false;
  isRemoveConfirmModalOpen = false;
  isBulkApproveModalOpen = false;
  selectedMaterial: BoxTypeMaterial | null = null;
  selectedBoxTypeForBulkApprove: string = '';
  arrivedQuantity: number | null = null;
  deliveredQuantity: number | null = null;
  arrivedNotes: string = '';
  /** Delivery progress 0-100. When 100, material is marked as delivered and cannot revert. */
  deliveryProgress = 100;
  
  // Inline editing state
  editingQuantityPerBox: { [key: string]: boolean } = {};
  tempQuantityPerBox: { [key: string]: number | undefined } = {};

  // Statistics
  stats = {
    totalMaterials: 0,
    arrivedCount: 0,
    pendingCount: 0,
    boxTypesCount: 0
  };

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private boxTypeMaterialService: BoxTypeMaterialService,
    private projectService: ProjectService,
    private boxService: BoxService,
    private cdr: ChangeDetectorRef
  ) {}

  // Store building and floor from query params for navigation
  private buildingFromQuery: string | null = null;
  private floorFromQuery: string | null = null;

  ngOnInit(): void {
    this.projectId = this.route.snapshot.paramMap.get('projectId') || '';
    
    // Check for query parameters to pre-filter by box type (only on initial load)
    const boxTypeParam = this.route.snapshot.queryParamMap.get('boxType');
    if (boxTypeParam && boxTypeParam !== 'all') {
      this.selectedBoxType = boxTypeParam;
    }
    
    // Store building and floor from query params for later navigation
    this.buildingFromQuery = this.route.snapshot.queryParamMap.get('building');
    this.floorFromQuery = this.route.snapshot.queryParamMap.get('floor');
    
    // Apply building and floor filters if provided in query params
    if (this.buildingFromQuery) {
      this.selectedBuilding = this.buildingFromQuery;
    }
    if (this.floorFromQuery) {
      this.selectedLevel = this.floorFromQuery;
    }
    
    if (this.projectId) {
      this.loadProjectDetails();
      this.loadBuildingsAndLevels();
      this.loadMaterials();
    }
  }

  loadProjectDetails(): void {
    this.projectService.getProject(this.projectId).subscribe({
      next: (project: any) => {
        this.projectName = project.name || project.projectName;
      },
      error: (err: any) => {
        console.error('Error loading project:', err);
      }
    });
  }

  loadBuildingsAndLevels(): void {
    // Load all boxes for the project to get distinct buildings and levels
    this.boxService.getBoxesByProject(this.projectId, {}).subscribe({
      next: (boxes) => {
        // Get distinct buildings
        const buildings = boxes
          .map(box => box.buildingNumber)
          .filter((building, index, self) => building && self.indexOf(building) === index)
          .sort();
        this.availableBuildings = buildings as string[];

        // Get distinct levels
        const levels = boxes
          .map(box => box.floor)
          .filter((floor, index, self) => floor && self.indexOf(floor) === index)
          .sort();
        this.availableLevels = levels as string[];
      },
      error: (err) => {
        console.error('Error loading buildings and levels:', err);
      }
    });
  }

  loadMaterials(): void {
    this.loading = true;
    // Don't clear error if it's an import error that needs to persist
    if (!this.hasErrors) {
      this.error = '';
    }
    
    // Prepare filter parameters
    const buildingFilter = this.selectedBuilding !== 'all' ? this.selectedBuilding : undefined;
    const levelFilter = this.selectedLevel !== 'all' ? this.selectedLevel : undefined;
    
    this.boxTypeMaterialService.getProjectBoxTypeMaterials(this.projectId, buildingFilter, levelFilter).subscribe({
      next: (materials) => {
        this.materials = materials;
        this.applyFilters();
        this.groupMaterialsByBoxType();
        this.calculateStats();
        this.loading = false;
      },
      error: (err) => {
        this.error = 'Failed to load box type materials';
        this.hasErrors = false;
        console.error('Error:', err);
        this.loading = false;
      }
    });
  }

  syncMaterials(): void {
    if (!confirm('This will synchronize materials from assigned templates to all box types. Continue?')) {
      return;
    }

    this.loading = true;
    this.error = '';
    this.hasErrors = false;
    
    this.boxTypeMaterialService.syncBoxTypeMaterials(this.projectId).subscribe({
      next: (response) => {
        this.successMessage = response.message;
        this.loadMaterials();
        setTimeout(() => this.successMessage = '', 5000);
      },
      error: (err) => {
        this.error = 'Failed to sync materials: ' + (err.error?.message || err.message);
        this.hasErrors = false;
        console.error('Error:', err);
        this.loading = false;
      }
    });
  }

  applyFilters(): void {
    // Ensure we have materials to filter
    if (!this.materials || this.materials.length === 0) {
      this.filteredMaterials = [];
      this.groupedByBoxType = {};
      return;
    }

    // Apply all filters
    this.filteredMaterials = this.materials.filter(material => {
      // Search filter - check if search term matches material name, code, or box type
      const searchLower = this.searchTerm?.toLowerCase().trim() || '';
      const matchesSearch = !searchLower || 
        material.materialName?.toLowerCase().includes(searchLower) ||
        material.materialCode?.toLowerCase().includes(searchLower) ||
        material.boxTypeName?.toLowerCase().includes(searchLower);
      
      // Box type filter
      const matchesBoxType = !this.selectedBoxType || 
        this.selectedBoxType === 'all' || 
        material.boxTypeName === this.selectedBoxType;
      
      // Status filter
      const matchesStatus = !this.selectedStatus || 
        this.selectedStatus === 'all' ||
        (this.selectedStatus === 'arrived' && material.isArrived === true) ||
        (this.selectedStatus === 'pending' && material.isArrived === false);
      
      // Material code filter
      const matchesMaterialCode = !this.selectedMaterialCode || 
        this.selectedMaterialCode === 'all' || 
        material.materialCode === this.selectedMaterialCode;
      
      return matchesSearch && matchesBoxType && matchesStatus && matchesMaterialCode;
    });

    // Group the filtered results by box type
    this.groupMaterialsByBoxType();
  }

  resetFilters(): void {
    this.searchTerm = '';
    this.selectedBoxType = 'all';
    this.selectedMaterialCode = 'all';
    this.selectedStatus = 'all';
    this.selectedBuilding = 'all';
    this.selectedLevel = 'all';
    // Reload materials without filters
    this.loadMaterials();
  }
  
  onBuildingOrLevelChange(): void {
    // When building or level filter changes, reload materials from server
    this.loadMaterials();
  }

  groupMaterialsByBoxType(): void {
    // Reset the grouped object
    this.groupedByBoxType = {};
    
    // Group filtered materials by box type
    if (this.filteredMaterials && this.filteredMaterials.length > 0) {
      this.filteredMaterials.forEach(material => {
        const boxType = material.boxTypeName || 'Unknown';
        if (!this.groupedByBoxType[boxType]) {
          this.groupedByBoxType[boxType] = [];
          // Set new box types as collapsed by default
          if (this.collapsedBoxTypes[boxType] === undefined) {
            this.collapsedBoxTypes[boxType] = true;
          }
        }
        this.groupedByBoxType[boxType].push(material);
      });
    }
  }

  calculateStats(): void {
    this.stats.totalMaterials = this.materials.length;
    this.stats.arrivedCount = this.materials.filter(m => m.isArrived).length;
    this.stats.pendingCount = this.materials.filter(m => !m.isArrived).length;
    
    const uniqueBoxTypes = new Set(this.materials.map(m => m.boxTypeName));
    this.stats.boxTypesCount = uniqueBoxTypes.size;
  }

  get boxTypes(): string[] {
    const types = Array.from(new Set(this.materials.map(m => m.boxTypeName)));
    return types.sort();
  }

  get materialCodes(): string[] {
    const codes = Array.from(new Set(this.materials.map(m => m.materialCode).filter(code => code)));
    return codes.sort();
  }

  get groupedBoxTypes(): string[] {
    return Object.keys(this.groupedByBoxType).sort();
  }

  openMarkArrivedModal(material: BoxTypeMaterial): void {
    this.selectedMaterial = material;
    this.arrivedQuantity = material.arrivedQuantity ?? null;
    this.deliveredQuantity = material.deliveredQuantity ?? material.arrivedQuantity ?? null;
    this.arrivedNotes = material.notes ?? '';
    this.deliveryProgress = material.deliveryProgress ?? (material.isArrived ? 100 : 0);
    this.isMarkArrivedModalOpen = true;
    
    // If deliveredQuantity exists, calculate initial percentage
    if (this.deliveredQuantity !== null && this.deliveredQuantity !== undefined) {
      const totalQty = this.getTotalQuantity(material);
      if (totalQty > 0) {
        this.deliveryProgress = Math.round((this.deliveredQuantity / totalQty) * 100);
      }
    }
  }

  closeMarkArrivedModal(): void {
    this.isMarkArrivedModalOpen = false;
    this.selectedMaterial = null;
    this.arrivedQuantity = null;
    this.deliveredQuantity = null;
    this.arrivedNotes = '';
    this.deliveryProgress = 100;
  }

  adjustProgress(amount: number): void {
    this.deliveryProgress = Math.min(100, Math.max(0, this.deliveryProgress + amount));
    // When progress changes via buttons, update quantity
    this.updateQuantityFromProgress();
  }

  onProgressChange(): void {
    // Ensure progress is within bounds
    if (this.deliveryProgress < 0) {
      this.deliveryProgress = 0;
    } else if (this.deliveryProgress > 100) {
      this.deliveryProgress = 100;
    }
    // When progress changes via slider or input, update quantity
    this.updateQuantityFromProgress();
  }

  /**
   * Calculate quantity from percentage (Progress → Quantity)
   * Formula: Quantity = (Percentage / 100) × Total Quantity
   */
  updateQuantityFromProgress(): void {
    if (!this.selectedMaterial) return;
    
    const totalQuantity = this.getTotalQuantity(this.selectedMaterial);
    if (totalQuantity > 0) {
      const calculatedQuantity = (this.deliveryProgress / 100) * totalQuantity;
      this.deliveredQuantity = Math.round(calculatedQuantity);
    }
  }

  /**
   * Calculate percentage from quantity (Quantity → Progress)
   * Formula: Percentage = (Current Quantity / Total Quantity) × 100
   */
  onQuantityChange(): void {
    if (!this.selectedMaterial || this.deliveredQuantity === null || this.deliveredQuantity === undefined) return;
    
    const totalQuantity = this.getTotalQuantity(this.selectedMaterial);
    if (totalQuantity > 0) {
      const calculatedProgress = (this.deliveredQuantity / totalQuantity) * 100;
      this.deliveryProgress = Math.min(100, Math.max(0, Math.round(calculatedProgress)));
    }
  }

  confirmMarkArrived(): void {
    if (!this.selectedMaterial) return;

    // Validate delivered quantity doesn't exceed total quantity needed
    const totalQuantityNeeded = this.getTotalQuantity(this.selectedMaterial);
    if (this.deliveredQuantity !== null && this.deliveredQuantity !== undefined) {
      if (this.deliveredQuantity > totalQuantityNeeded) {
        this.error = `Delivered quantity (${this.deliveredQuantity}) cannot exceed total quantity needed (${totalQuantityNeeded})`;
        this.hasErrors = false;
        setTimeout(() => this.error = '', 5000);
        return;
      }
    }

    const progress = Math.min(100, Math.max(0, this.deliveryProgress));
    this.loading = true;
    this.boxTypeMaterialService.markAsArrived(
      this.selectedMaterial.boxTypeMaterialId,
      {
        deliveryProgress: progress,
        arrivedQuantity: this.deliveredQuantity ?? this.arrivedQuantity ?? undefined,
        deliveredQuantity: this.deliveredQuantity ?? undefined,
        notes: this.arrivedNotes || undefined
      }
    ).subscribe({
      next: () => {
        this.successMessage = `Material "${this.selectedMaterial!.materialName}" marked as delivered for box type "${this.selectedMaterial!.boxTypeName}"`;
        this.closeMarkArrivedModal();
        this.loadMaterials();
        setTimeout(() => this.successMessage = '', 5000);
      },
      error: (err) => {
        this.error = 'Failed to mark material as delivered';
        this.hasErrors = false;
        console.error('Error:', err);
        this.loading = false;
      }
    });
  }

  markAsPending(material: BoxTypeMaterial): void {
    if (!confirm(`Are you sure you want to mark "${material.materialName}" as pending for "${material.boxTypeName}"?`)) {
      return;
    }

    this.loading = true;
    this.boxTypeMaterialService.markAsPending(material.boxTypeMaterialId).subscribe({
      next: () => {
        this.successMessage = `Material "${material.materialName}" marked as pending for box type "${material.boxTypeName}"`;
        this.loadMaterials();
        setTimeout(() => this.successMessage = '', 5000);
      },
      error: (err) => {
        this.error = 'Failed to mark material as pending';
        this.hasErrors = false;
        console.error('Error:', err);
        this.loading = false;
      }
    });
  }

  openRemoveConfirmModal(material: BoxTypeMaterial): void {
    this.selectedMaterial = material;
    this.isRemoveConfirmModalOpen = true;
  }

  closeRemoveConfirmModal(): void {
    this.isRemoveConfirmModalOpen = false;
    this.selectedMaterial = null;
  }

  confirmRemoveMaterial(): void {
    if (!this.selectedMaterial) return;

    this.loading = true;
    this.boxTypeMaterialService.removeBoxTypeMaterial(this.selectedMaterial.boxTypeMaterialId).subscribe({
      next: () => {
        this.successMessage = `Material "${this.selectedMaterial!.materialName}" removed from box type "${this.selectedMaterial!.boxTypeName}"`;
        this.closeRemoveConfirmModal();
        this.loadMaterials();
        setTimeout(() => this.successMessage = '', 5000);
      },
      error: (err) => {
        this.error = 'Failed to remove material from box type';
        this.hasErrors = false;
        console.error('Error:', err);
        this.loading = false;
        this.closeRemoveConfirmModal();
      }
    });
  }

  openBulkApproveModal(boxTypeName: string): void {
    const pendingMaterials = this.groupedByBoxType[boxTypeName].filter(m => !m.isArrived);
    if (pendingMaterials.length === 0) {
      alert(`All materials for "${boxTypeName}" have already been marked as delivered.`);
      return;
    }
    this.selectedBoxTypeForBulkApprove = boxTypeName;
    this.isBulkApproveModalOpen = true;
  }

  closeBulkApproveModal(): void {
    this.isBulkApproveModalOpen = false;
    this.selectedBoxTypeForBulkApprove = '';
  }

  confirmBulkApprove(): void {
    if (!this.selectedBoxTypeForBulkApprove) return;

    const materials = this.groupedByBoxType[this.selectedBoxTypeForBulkApprove];
    const pendingMaterials = materials.filter(m => !m.isArrived);
    
    if (pendingMaterials.length === 0) {
      this.closeBulkApproveModal();
      return;
    }

    this.loading = true;
    let completed = 0;
    let failed = 0;

    // Mark each pending material as arrived
    pendingMaterials.forEach((material, index) => {
      this.boxTypeMaterialService.markAsArrived(
        material.boxTypeMaterialId,
        {
          arrivedQuantity: undefined,
          notes: `Bulk approved for ${this.selectedBoxTypeForBulkApprove}`
        }
      ).subscribe({
        next: () => {
          completed++;
          if (completed + failed === pendingMaterials.length) {
            this.finishBulkApprove(completed, failed);
          }
        },
        error: (err) => {
          failed++;
          console.error('Error marking material as delivered:', err);
          if (completed + failed === pendingMaterials.length) {
            this.finishBulkApprove(completed, failed);
          }
        }
      });
    });
  }

  private finishBulkApprove(completed: number, failed: number): void {
    this.closeBulkApproveModal();
    this.loadMaterials();
    
    if (failed === 0) {
      this.successMessage = `Successfully marked ${completed} material(s) as delivered for "${this.selectedBoxTypeForBulkApprove}"`;
      this.hasErrors = false;
    } else {
      this.error = `Marked ${completed} as delivered, but ${failed} failed. Please try again for failed items.`;
      this.hasErrors = false;
    }
    
    setTimeout(() => {
      this.successMessage = '';
      this.error = '';
      this.hasErrors = false;
    }, 5000);
  }

  getStatusClass(material: BoxTypeMaterial): string {
    return material.isArrived ? 'status-arrived' : 'status-pending';
  }

  getStatusIcon(material: BoxTypeMaterial): string {
    return material.isArrived ? '✓' : '○';
  }

  getStatusText(material: BoxTypeMaterial): string {
    if (material.isArrived) return 'DELIVERED';
    const p = material.deliveryProgress ?? 0;
    return p > 0 ? `${p}%` : 'PENDING';
  }

  formatDate(date: Date | undefined): string {
    if (!date) return '-';
    return new Date(date).toLocaleDateString();
  }
  
  getPendingCountForBoxType(boxTypeName: string): number {
    const materials = this.groupedByBoxType[boxTypeName];
    return materials ? materials.filter(m => !m.isArrived).length : 0;
  }

  /**
   * Calculate total material count for a box type (materials × boxes)
   * If no boxes exist yet, shows just the material count
   */
  getTotalMaterialCountForBoxType(boxTypeName: string): number {
    const materials = this.groupedByBoxType[boxTypeName];
    if (!materials || materials.length === 0) return 0;
    
    // Get the box count from any material in this group (they all have the same boxCount)
    const boxCount = materials[0].boxCount || 0;
    const materialCount = materials.length;
    
    // If no boxes exist yet, return just the material count
    // Otherwise, return materials × boxes
    return boxCount === 0 ? materialCount : materialCount * boxCount;
  }

  /**
   * Calculate delivered material count × boxes count for a box type
   * Shows: (delivered materials count × boxes count)
   */
  getDeliveredMaterialCountForBoxType(boxTypeName: string): number {
    const materials = this.groupedByBoxType[boxTypeName];
    if (!materials || materials.length === 0) return 0;
    
    // Get the box count from any material in this group
    const boxCount = materials[0].boxCount || 0;
    
    // Count delivered materials
    const deliveredMaterialCount = materials.filter(m => m.isArrived).length;
    
    // If no boxes exist yet, return just the delivered material count
    // Otherwise, return delivered materials × boxes
    return boxCount === 0 ? deliveredMaterialCount : deliveredMaterialCount * boxCount;
  }

  /**
   * Get box count for a box type
   */
  getBoxCountForBoxType(boxTypeName: string): number {
    const materials = this.groupedByBoxType[boxTypeName];
    if (!materials || materials.length === 0) return 0;
    return materials[0].boxCount || 0;
  }

  /**
   * Navigate to boxes page filtered by box type
   */
  navigateToBoxType(boxTypeName: string): void {
    // Build query params, preserving building and floor filters if they exist
    const queryParams: any = { boxType: boxTypeName };
    
    // Preserve building and floor filters from query params if they were provided
    if (this.buildingFromQuery) {
      queryParams.building = this.buildingFromQuery;
    }
    if (this.floorFromQuery) {
      queryParams.floor = this.floorFromQuery;
    }
    
    // Navigate to boxes page with box type filter and preserved building/floor filters
    this.router.navigate(['/projects', this.projectId, 'boxes'], {
      queryParams: queryParams
    });
  }

  /**
   * Get the count of delivered materials only (not multiplied by boxes)
   */
  getDeliveredMaterialCountOnly(boxTypeName: string): number {
    const materials = this.groupedByBoxType[boxTypeName];
    if (!materials || materials.length === 0) return 0;
    return materials.filter(m => m.isArrived).length;
  }

  /**
   * Toggle collapse state for a box type
   */
  toggleBoxTypeCollapse(boxTypeName: string): void {
    this.collapsedBoxTypes[boxTypeName] = !this.collapsedBoxTypes[boxTypeName];
  }

  /**
   * Check if a box type is collapsed
   */
  isBoxTypeCollapsed(boxTypeName: string): boolean {
    return !!this.collapsedBoxTypes[boxTypeName];
  }

  /**
   * Get the count of unique materials (not multiplied by box count)
   */
  getUniqueMaterialCount(boxTypeName: string): number {
    const materials = this.groupedByBoxType[boxTypeName];
    return materials ? materials.length : 0;
  }

  /**
   * Calculate total quantity needed for a material (boxCount × quantityPerBox)
   */
  getTotalQuantity(material: BoxTypeMaterial): number {
    const boxCount = material.boxCount || 0;
    const quantityPerBox = material.quantityPerBox || 0;
    return boxCount * quantityPerBox;
  }

  /**
   * Check if delivered quantity is valid (not exceeding total quantity needed)
   */
  isDeliveredQuantityValid(): boolean {
    if (!this.selectedMaterial || this.deliveredQuantity === null || this.deliveredQuantity === undefined) {
      return true;
    }
    const totalQuantityNeeded = this.getTotalQuantity(this.selectedMaterial);
    return this.deliveredQuantity <= totalQuantityNeeded;
  }

  /**
   * Get validation error message for delivered quantity
   */
  getDeliveredQuantityError(): string {
    if (!this.selectedMaterial || this.deliveredQuantity === null || this.deliveredQuantity === undefined) {
      return '';
    }
    const totalQuantityNeeded = this.getTotalQuantity(this.selectedMaterial);
    if (this.deliveredQuantity > totalQuantityNeeded) {
      return `Maximum allowed: ${totalQuantityNeeded}`;
    }
    return '';
  }

  /**
   * Start editing quantity per box inline
   */
  startEditQuantityPerBox(material: BoxTypeMaterial): void {
    this.editingQuantityPerBox[material.boxTypeMaterialId] = true;
    this.tempQuantityPerBox[material.boxTypeMaterialId] = material.quantityPerBox;
  }

  /**
   * Cancel editing quantity per box
   */
  cancelEditQuantityPerBox(material: BoxTypeMaterial): void {
    this.editingQuantityPerBox[material.boxTypeMaterialId] = false;
    delete this.tempQuantityPerBox[material.boxTypeMaterialId];
  }

  /**
   * Save quantity per box
   */
  saveQuantityPerBox(material: BoxTypeMaterial): void {
    const newQuantity = this.tempQuantityPerBox[material.boxTypeMaterialId];
    
    if (newQuantity === undefined || newQuantity < 0) {
      this.error = 'Please enter a valid quantity';
      this.hasErrors = false;
      return;
    }

    this.loading = true;
    
    // Call backend API to update quantity per box
    this.boxTypeMaterialService.updateQuantityPerBox(material.boxTypeMaterialId, newQuantity).subscribe({
      next: () => {
        material.quantityPerBox = newQuantity;
        this.editingQuantityPerBox[material.boxTypeMaterialId] = false;
        delete this.tempQuantityPerBox[material.boxTypeMaterialId];
        
        this.successMessage = 'Quantity per box updated successfully';
        this.loading = false;
        setTimeout(() => this.successMessage = '', 3000);
      },
      error: (err) => {
        this.error = 'Failed to update quantity per box: ' + (err.error?.message || err.message);
        this.hasErrors = false;
        console.error('Error:', err);
        this.loading = false;
        setTimeout(() => this.error = '', 5000);
      }
    });
  }

  /**
   * Check if quantity per box is being edited
   */
  isEditingQuantityPerBox(material: BoxTypeMaterial): boolean {
    return !!this.editingQuantityPerBox[material.boxTypeMaterialId];
  }

  /**
   * Dismiss import error message manually
   */
  dismissError(): void {
    this.error = '';
    this.hasErrors = false;
  }

  /**
   * Export box type materials to Excel
   * Each box type will be a separate sheet
   * Excludes "Delivered Date" and "Delivered By" columns
   */
  async exportToExcel(): Promise<void> {
    try {
      // Create a new workbook
      const workbook = new ExcelJS.Workbook();
      workbook.creator = 'DuBox System';
      workbook.created = new Date();

      // Process each box type as a separate sheet
      for (const boxTypeName of this.groupedBoxTypes) {
        const materials = this.groupedByBoxType[boxTypeName];
        if (!materials || materials.length === 0) continue;

        await this.addBoxTypeToWorkbook(workbook, boxTypeName, materials);
      }

      // Generate Excel file
      const buffer = await workbook.xlsx.writeBuffer();
      const blob = new Blob([buffer], { 
        type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet' 
      });

      // Download file
      const fileName = `Project_Materials_${this.projectName.replace(/[^a-z0-9]/gi, '_')}_${new Date().toISOString().split('T')[0]}.xlsx`;
      const link = document.createElement('a');
      link.href = window.URL.createObjectURL(blob);
      link.download = fileName;
      document.body.appendChild(link);
      link.click();
      document.body.removeChild(link);
      window.URL.revokeObjectURL(link.href);

      this.successMessage = 'Excel file exported successfully!';
      setTimeout(() => this.successMessage = '', 3000);
    } catch (error) {
      console.error('Error exporting to Excel:', error);
      this.error = 'Failed to export Excel file. Please try again.';
      this.hasErrors = false;
      setTimeout(() => this.error = '', 5000);
    }
  }

  /**
   * Export a single box type to Excel
   * Excludes "Delivered Date" and "Delivered By" columns
   */
  async exportBoxTypeToExcel(boxTypeName: string): Promise<void> {
    try {
      const materials = this.groupedByBoxType[boxTypeName];
      if (!materials || materials.length === 0) {
        this.error = 'No materials to export for this box type';
        this.hasErrors = false;
        setTimeout(() => this.error = '', 3000);
        return;
      }

      // Create a new workbook
      const workbook = new ExcelJS.Workbook();
      workbook.creator = 'DuBox System';
      workbook.created = new Date();

      // Add this box type to the workbook
      await this.addBoxTypeToWorkbook(workbook, boxTypeName, materials);

      // Generate Excel file
      const buffer = await workbook.xlsx.writeBuffer();
      const blob = new Blob([buffer], { 
        type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet' 
      });

      // Download file
      const fileName = `${boxTypeName.replace(/[^a-z0-9]/gi, '_')}_Materials_${new Date().toISOString().split('T')[0]}.xlsx`;
      const link = document.createElement('a');
      link.href = window.URL.createObjectURL(blob);
      link.download = fileName;
      document.body.appendChild(link);
      link.click();
      document.body.removeChild(link);
      window.URL.revokeObjectURL(link.href);

      this.successMessage = `Materials for "${boxTypeName}" exported successfully!`;
      setTimeout(() => this.successMessage = '', 3000);
    } catch (error) {
      console.error('Error exporting to Excel:', error);
      this.error = 'Failed to export Excel file. Please try again.';
      this.hasErrors = false;
      setTimeout(() => this.error = '', 5000);
    }
  }

  /**
   * Add a box type sheet to the workbook
   */
  private async addBoxTypeToWorkbook(workbook: ExcelJS.Workbook, boxTypeName: string, materials: BoxTypeMaterial[]): Promise<void> {
    // Create worksheet for this box type
    const worksheet = workbook.addWorksheet(this.sanitizeSheetName(boxTypeName));

    // Get box count for this box type
    const boxCount = materials[0]?.boxCount || 0;

    // Define columns (excluding Delivered Date and Delivered By)
    worksheet.columns = [
      { header: 'Material Code', key: 'materialCode', width: 18 },
      { header: 'Material Name', key: 'materialName', width: 30 },
      { header: 'Category', key: 'category', width: 18 },
      { header: 'Unit', key: 'unit', width: 12 },
      { header: 'Quantity Per Box', key: 'quantityPerBox', width: 18 },
      { header: 'Total Quantity (Auto-calculated)', key: 'totalQuantity', width: 28 },
      { header: 'Status', key: 'status', width: 12 },
      { header: 'Delivered Quantity', key: 'deliveredQuantity', width: 18 },
      { header: 'Required Before (Days)', key: 'requiredBeforeDays', width: 22 },
      { header: 'Notes', key: 'notes', width: 30 }
    ];

    // Style the header row
    const headerRow = worksheet.getRow(1);
    headerRow.font = { bold: true, color: { argb: 'FFFFFFFF' } };
    headerRow.fill = {
      type: 'pattern',
      pattern: 'solid',
      fgColor: { argb: 'FF4472C4' }
    };
    headerRow.alignment = { vertical: 'middle', horizontal: 'center' };
    headerRow.height = 20;

    // Add data rows
    materials.forEach((material, index) => {
      const rowNum = index + 2; // +2 because row 1 is header
      const row = worksheet.addRow({
        materialCode: material.materialCode,
        materialName: material.materialName,
        category: material.materialCategory || '-',
        unit: material.unit || '-',
        quantityPerBox: material.quantityPerBox || 0,
        totalQuantity: '', // Will be replaced with formula
        status: this.getStatusText(material),
        deliveredQuantity: material.deliveredQuantity || material.arrivedQuantity || '-',
        requiredBeforeDays: `${material.requiredBeforeDays} days`,
        notes: material.notes || '-'
      });

      // Set the Total Quantity column (column F) as a formula: Quantity Per Box (E) × Box Count
      const totalQtyCell = worksheet.getCell(`F${rowNum}`);
      totalQtyCell.value = { formula: `E${rowNum}*${boxCount}`, result: this.getTotalQuantity(material) };
      
      // Make the Total Quantity column visually distinct (light gray background)
      totalQtyCell.fill = {
        type: 'pattern',
        pattern: 'solid',
        fgColor: { argb: 'FFF0F0F0' } // Light gray to indicate it's calculated
      };
      totalQtyCell.font = { italic: true, color: { argb: 'FF666666' } };

      // Apply conditional formatting based on status
      if (material.isArrived) {
        row.fill = {
          type: 'pattern',
          pattern: 'solid',
          fgColor: { argb: 'FFD4EDDA' } // Light green
        };
      } else {
        const progress = material.deliveryProgress ?? 0;
        if (progress > 0) {
          row.fill = {
            type: 'pattern',
            pattern: 'solid',
            fgColor: { argb: 'FFFFF3CD' } // Light yellow
          };
        } else {
          row.fill = {
            type: 'pattern',
            pattern: 'solid',
            fgColor: { argb: 'FFFFFFFF' } // White
          };
        }
      }

      // Apply borders to all cells
      row.eachCell((cell) => {
        cell.border = {
          top: { style: 'thin' },
          left: { style: 'thin' },
          bottom: { style: 'thin' },
          right: { style: 'thin' }
        };
        cell.alignment = { vertical: 'middle', horizontal: 'left', wrapText: true };
      });

      // Re-apply the gray background to Total Quantity column (since row.fill might override it)
      totalQtyCell.fill = {
        type: 'pattern',
        pattern: 'solid',
        fgColor: { argb: 'FFF0F0F0' }
      };
    });

    // Add summary row with info about the calculated column
    const summaryRowNum = worksheet.rowCount + 2;
    worksheet.mergeCells(`A${summaryRowNum}:C${summaryRowNum}`);
    const summaryCell = worksheet.getCell(`A${summaryRowNum}`);
    summaryCell.value = `Box Type Summary: ${boxCount} boxes, ${materials.length} materials`;
    summaryCell.font = { bold: true, color: { argb: 'FF2563EB' } };
    summaryCell.alignment = { vertical: 'middle', horizontal: 'left' };

    // Add note about the calculated column
    const noteRowNum = summaryRowNum + 1;
    worksheet.mergeCells(`A${noteRowNum}:F${noteRowNum}`);
    const noteCell = worksheet.getCell(`A${noteRowNum}`);
    noteCell.value = `Note: "Total Quantity" is auto-calculated as (Quantity Per Box × ${boxCount} boxes). Do not edit this column.`;
    noteCell.font = { italic: true, color: { argb: 'FF666666' } };
    noteCell.alignment = { vertical: 'middle', horizontal: 'left' };

    // Auto-fit columns
    worksheet.columns.forEach((column) => {
      if (column.width && column.width < 10) {
        column.width = 10;
      }
    });
  }

  /**
   * Sanitize sheet name to comply with Excel naming rules
   * - Max 31 characters
   * - No special characters: \ / ? * [ ]
   */
  private sanitizeSheetName(name: string): string {
    let sanitized = name.replace(/[\\\/\?\*\[\]]/g, '_');
    if (sanitized.length > 31) {
      sanitized = sanitized.substring(0, 31);
    }
    return sanitized;
  }

  /**
   * Trigger file input for importing box type materials from Excel
   */
  triggerImportExcel(boxTypeName: string): void {
    // Store the box type name for later use
    const materials = this.groupedByBoxType[boxTypeName];
    if (!materials || materials.length === 0) {
      this.error = 'No materials found for this box type';
      this.hasErrors = false;
      setTimeout(() => this.error = '', 3000);
      return;
    }

    const projectBoxTypeId = materials[0].projectBoxTypeId;
    
    // Create a temporary file input
    const fileInput = document.createElement('input');
    fileInput.type = 'file';
    fileInput.accept = '.xlsx,.xls';
    fileInput.style.display = 'none';
    
    fileInput.onchange = (event: any) => {
      const file = event.target.files[0];
      if (file) {
        this.importBoxTypeMaterials(projectBoxTypeId, boxTypeName, file);
      }
      // Remove the file input from DOM
      document.body.removeChild(fileInput);
    };
    
    // Add to DOM and trigger click
    document.body.appendChild(fileInput);
    fileInput.click();
  }

  /**
   * Import box type materials from Excel file
   */
  importBoxTypeMaterials(projectBoxTypeId: number, boxTypeName: string, file: File): void {
    this.loading = true;
    this.error = '';
    this.successMessage = '';
    this.hasErrors = false;
    
    this.boxTypeMaterialService.importBoxTypeMaterialsFromExcel(projectBoxTypeId, file).subscribe({
      next: (response) => {
        this.loading = false;
        
        console.log('Import response:', response); // Debug log
        
        // Show success message if there were successful updates
        if (response.successCount > 0) {
          let message = `✓ ${response.successCount} material(s) updated successfully`;
          
          // If there are failures, mention them in the success message
          if (response.failureCount > 0) {
            message += `\n⚠ ${response.failureCount} material(s) FAILED - See details below`;
          }
          
          this.successMessage = message;
          console.log('Setting success message:', this.successMessage); // Debug log
        }
        
        // Show errors if any (with VERY prominent formatting)
        if (response.errors && response.errors.length > 0) {
          const failureCount = response.failureCount || response.errors.length;
          this.hasErrors = response.errors?.length > 0;
          console.log('Processing errors. Count:', response.errors.length);
          console.log('Errors array:', response.errors);
          
          // Create a clear, formatted error message
          let errorMessage = `${failureCount} MATERIAL(S) FAILED TO IMPORT\n\n`;
          errorMessage += `The following rows have errors and were NOT imported:\n\n`;
          
          // Add each error with clear formatting
          const errorList = response.errors.slice(0, 10).map((err, index) => {
            return `${index + 1}. ${err}`;
          }).join('\n\n');
          
          errorMessage += errorList;
          
          // Add count of remaining errors if more than 10
          if (response.errors.length > 10) {
            errorMessage += `\n\n... and ${response.errors.length - 10} MORE ERRORS`;
          }
          
          this.error = errorMessage;
          console.log('Setting error message:', this.error); // Debug log
          console.log('this.error variable is now:', this.error); // Additional debug
          
          // Force change detection to ensure UI updates
          this.cdr.detectChanges();
          console.log('After detectChanges, this.error is:', this.error);
          // Force change detection
         
        } else if (response.failureCount > 0 && response.successCount === 0) {
          // If ALL rows failed but no specific errors provided
          this.error = `ALL ${response.failureCount} MATERIAL(S) FAILED TO IMPORT\n\nPlease check your Excel file and try again.`;
          console.log('All failed, error set to:', this.error);
          this.cdr.detectChanges();
        }
        
        // Show warnings if any (in console for now)
        if (response.warnings && response.warnings.length > 0) {
          console.warn('Import warnings:', response.warnings);
        }
        
        // Reload materials only if there were successful updates
        if (response.successCount > 0) {
          setTimeout(() => {
            this.loadMaterials();
          }, 1000);
        }
        console.log('this.error variable is now:', this.error);
      },
     
    });
  }

  /**
   * Trigger file input for importing ALL project materials from Excel
   */
  triggerImportAllMaterials(): void {
    if (this.materials.length === 0) {
      this.error = 'No materials found in this project';
      this.hasErrors = false;
      setTimeout(() => this.error = '', 3000);
      return;
    }

    // Create a temporary file input
    const fileInput = document.createElement('input');
    fileInput.type = 'file';
    fileInput.accept = '.xlsx,.xls';
    fileInput.style.display = 'none';
    
    fileInput.onchange = (event: any) => {
      const file = event.target.files[0];
      if (file) {
        this.importAllProjectMaterials(file);
      }
      // Remove the file input from DOM
      document.body.removeChild(fileInput);
    };
    
    // Add to DOM and trigger click
    document.body.appendChild(fileInput);
    fileInput.click();
  }

  /**
   * Import ALL project materials from Excel file
   */
  importAllProjectMaterials(file: File): void {
    this.loading = true;
    this.error = '';
    this.successMessage = '';
    this.hasErrors = false;
    
    this.boxTypeMaterialService.importProjectMaterialsFromExcel(this.projectId, file).subscribe({
      next: (response) => {
        this.loading = false;
        
        console.log('Import All response:', response);
        
        // Show success message if there were successful updates
        if (response.successCount > 0) {
          let message = `✓ ${response.successCount} material(s) updated successfully across all box types`;
          
          if (response.failureCount > 0) {
            message += `\n⚠ ${response.failureCount} material(s) FAILED - See details below`;
          }
          
          this.successMessage = message;
        }
        
        // Show errors if any
        if (response.errors && response.errors.length > 0) {
          const failureCount = response.failureCount || response.errors.length;
          this.hasErrors = response.errors?.length > 0;
          
          let errorMessage = `${failureCount} MATERIAL(S) FAILED TO IMPORT\n\n`;
          errorMessage += `The following rows have errors and were NOT imported:\n\n`;
          
          const errorList = response.errors.slice(0, 10).map((err, index) => {
            return `${index + 1}. ${err}`;
          }).join('\n\n');
          
          errorMessage += errorList;
          
          if (response.errors.length > 10) {
            errorMessage += `\n\n... and ${response.errors.length - 10} MORE ERRORS`;
          }
          
          this.error = errorMessage;
          this.cdr.detectChanges();
        } else if (response.failureCount > 0 && response.successCount === 0) {
          this.error = `ALL ${response.failureCount} MATERIAL(S) FAILED TO IMPORT\n\nPlease check your Excel file and try again.`;
          this.cdr.detectChanges();
        }
        
        // Show warnings if any
        if (response.warnings && response.warnings.length > 0) {
          console.warn('Import warnings:', response.warnings);
        }
        
        // Reload materials if there were successful updates
        if (response.successCount > 0) {
          setTimeout(() => {
            this.loadMaterials();
          }, 1000);
        }
      },
      error: (err) => {
        this.error = 'Failed to import materials: ' + (err.error?.message || err.message);
        this.hasErrors = false;
        console.error('Error importing materials:', err);
        this.loading = false;
        setTimeout(() => this.error = '', 5000);
      }
    });
  }
}

