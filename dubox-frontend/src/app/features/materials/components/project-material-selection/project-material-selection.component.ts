import { Component, OnInit, Input, Output, EventEmitter, OnChanges, SimpleChanges } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { BoxMaterialService } from '../../../../core/services/box-material.service';
import { ToastService } from '../../../../core/services/toast.service';
import { ProjectMaterialDto } from '../../../../core/models/project-material.model';
import { MaterialTemplateService } from '../../../../core/services/material-template.service';
import { MaterialTemplate, ProjectMaterialTemplate } from '../../../../core/models/material-template.model';
import { ProjectBoxType } from '../../../../core/models/project-configuration.model';

interface MaterialsByCategory {
  category: string;
  materials: ProjectMaterialDto[];
  allSelected: boolean;
  selectedCount: number;
}

@Component({
  selector: 'app-project-material-selection',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './project-material-selection.component.html',
  styleUrls: ['./project-material-selection.component.scss']
})
export class ProjectMaterialSelectionComponent implements OnInit, OnChanges {
  @Input() projectId!: string;
  @Input() boxTypes: ProjectBoxType[] = [];
  @Input() boxTypeTemplates: Map<number, MaterialTemplate> = new Map();

  /** Emits the selected template ID when user changes selection. Emits null when deselected. */
  @Output() templateSelectionChanged = new EventEmitter<string | null>();
  @Output() assignBoxTypeTemplate = new EventEmitter<ProjectBoxType>();
  @Output() changeBoxTypeTemplate = new EventEmitter<ProjectBoxType>();
  @Output() removeBoxTypeTemplate = new EventEmitter<ProjectBoxType>();
  
  materials: ProjectMaterialDto[] = [];
  filteredMaterials: ProjectMaterialDto[] = [];
  materialsByCategory: MaterialsByCategory[] = [];
  searchTerm = '';
  selectedCategory = 'All';
  categories: string[] = ['All'];
  isLoading = false;
  selectAllChecked = false;
  expandedCategories: Set<string> = new Set();

  // Material Templates — single selection only
  availableTemplates: MaterialTemplate[] = [];
  selectedTemplateId: string | null = null;
  loadingTemplates = false;
  showTemplatesSection = true;
  
  // Template search and filter
  templateSearchTerm = '';
  filteredTemplates: MaterialTemplate[] = [];
  
  // Assigned template (persisted in database — single)
  assignedTemplateId: string | null = null;
  loadingAssignedTemplates = false;

  constructor(
    private boxMaterialService: BoxMaterialService,
    private toastService: ToastService,
    private materialTemplateService: MaterialTemplateService
  ) {}

  ngOnInit(): void {
    // Projects now use templates only - no individual material selection
    this.loadMaterialTemplates();
    this.loadAssignedTemplates();
  }

  ngOnChanges(changes: SimpleChanges): void {
    // If projectId changes (e.g., after project creation), reload assigned templates
    if (changes['projectId'] && !changes['projectId'].firstChange && this.projectId) {
      this.loadAssignedTemplates();
    }
  }

  // DEPRECATED: Projects use templates only
  loadProjectMaterials(): void {
    // This method is no longer used - projects only work with templates
    console.warn('loadProjectMaterials called but projects now use templates only');
  }

  // DEPRECATED: Individual material selection methods - Projects use templates only
  filterMaterials(): void {
    console.warn('filterMaterials called but projects now use templates only');
  }

  groupMaterialsByCategory(): void {
    console.warn('groupMaterialsByCategory called but projects now use templates only');
  }

  toggleCategory(category: string): void {
    console.warn('toggleCategory called but projects now use templates only');
  }

  isCategoryExpanded(category: string): boolean {
    return false;
  }

  toggleCategorySelection(categoryGroup: MaterialsByCategory): void {
    console.warn('toggleCategorySelection called but projects now use templates only');
  }

  toggleMaterial(material: ProjectMaterialDto): void {
    console.warn('toggleMaterial called but projects now use templates only');
  }

  toggleSelectAll(): void {
    console.warn('toggleSelectAll called but projects now use templates only');
  }

  updateSelectAllState(): void {
    console.warn('updateSelectAllState called but projects now use templates only');
  }

  saveSelection(): void {
    this.toastService.warning('Projects use material templates. Individual material selection is not available.');
  }

  selectAllMaterials(): void {
    this.toastService.warning('Projects use material templates. Please assign templates instead.');
  }

  getSelectedCount(): number {
    return 0; // Projects use templates, not individual materials
  }

  getLeadTimeDisplay(days: number): string {
    return days >= 30 ? '1 Month' : '1 Week';
  }

  /**
   * Load available material templates
   */
  loadMaterialTemplates(): void {
    this.loadingTemplates = true;
    this.materialTemplateService.getAllTemplates(true).subscribe({
      next: (templates) => {
        this.availableTemplates = templates || [];
        this.filteredTemplates = this.availableTemplates;
        this.loadingTemplates = false;
      },
      error: (error) => {
        console.error('Error loading templates:', error);
        this.toastService.error('Failed to load templates');
        this.loadingTemplates = false;
      }
    });
  }

  /**
   * Load assigned template for this project from database (single template only)
   */
  loadAssignedTemplates(): void {
    this.loadingAssignedTemplates = true;
    this.materialTemplateService.getProjectTemplates(this.projectId).subscribe({
      next: (assignments: ProjectMaterialTemplate[]) => {
        // Single template per project — take the first assignment if any
        if (assignments.length > 0) {
          this.assignedTemplateId = assignments[0].materialTemplateId;
          this.selectedTemplateId = this.assignedTemplateId;
        } else {
          this.assignedTemplateId = null;
          this.selectedTemplateId = null;
        }
        this.loadingAssignedTemplates = false;
      },
      error: (error: any) => {
        console.error('Error loading assigned templates:', error);
        this.loadingAssignedTemplates = false;
      }
    });
  }

  /**
   * Filter templates based on search term
   */
  filterTemplates(): void {
    if (!this.templateSearchTerm) {
      this.filteredTemplates = this.availableTemplates;
      return;
    }

    const searchLower = this.templateSearchTerm.toLowerCase();
    this.filteredTemplates = this.availableTemplates.filter(template =>
      template.templateName.toLowerCase().includes(searchLower) ||
      template.templateCode.toLowerCase().includes(searchLower) ||
      (template.category && template.category.toLowerCase().includes(searchLower))
    );
  }

  /**
   * Toggle template selection (single-select, no immediate API call).
   * The parent component is responsible for persisting the selection on form submit.
   */
  toggleTemplateSelection(templateId: string): void {
    if (this.selectedTemplateId === templateId) {
      // Deselect current template
      this.selectedTemplateId = null;
      this.templateSelectionChanged.emit(null);
    } else {
      // Select new template (replaces any previously selected)
      this.selectedTemplateId = templateId;
      this.templateSelectionChanged.emit(templateId);
    }
  }

  /**
   * Check if template is currently selected (pending, not necessarily saved)
   */
  isTemplateSelected(templateId: string): boolean {
    return this.selectedTemplateId === templateId;
  }

  /**
   * Check if template is assigned (persisted in database)
   */
  isTemplateAssigned(templateId: string): boolean {
    return this.assignedTemplateId === templateId;
  }

  /**
   * Get count of selected templates (0 or 1 — single-select)
   */
  getSelectedTemplatesCount(): number {
    return this.selectedTemplateId ? 1 : 0;
  }

  applySelectedTemplates(): void {
    this.toastService.info('Templates are assigned directly to the project. Materials will be available when creating boxes.');
  }

  /**
   * Clear the current template selection (does not call API — parent handles persistence)
   */
  clearTemplateSelections(): void {
    if (!this.selectedTemplateId) return;
    this.selectedTemplateId = null;
    this.templateSelectionChanged.emit(null);
    this.toastService.success('Template selection cleared');
  }

  /**
   * Toggle templates section visibility
   */
  toggleTemplatesSection(): void {
    this.showTemplatesSection = !this.showTemplatesSection;
  }

  /**
   * Get template for a specific box type
   */
  getBoxTypeTemplate(boxTypeId: number): MaterialTemplate | null {
    return this.boxTypeTemplates.get(boxTypeId) || null;
  }

  /**
   * Handle assign box type template click
   */
  onAssignBoxTypeTemplate(boxType: ProjectBoxType): void {
    this.assignBoxTypeTemplate.emit(boxType);
  }

  /**
   * Handle change box type template click
   */
  onChangeBoxTypeTemplate(boxType: ProjectBoxType): void {
    this.changeBoxTypeTemplate.emit(boxType);
  }

  /**
   * Handle remove box type template click
   */
  onRemoveBoxTypeTemplate(boxType: ProjectBoxType): void {
    this.removeBoxTypeTemplate.emit(boxType);
  }
}

