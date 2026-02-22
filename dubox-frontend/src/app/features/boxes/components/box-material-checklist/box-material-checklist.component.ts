import { Component, OnInit, Input } from '@angular/core';
import { CommonModule } from '@angular/common';
import { BoxMaterialService } from '../../../../core/services/box-material.service';
import { BoxMaterial } from '../../../../core/models/box-material.model';
import { ToastService } from '../../../../core/services/toast.service';

@Component({
  selector: 'app-box-material-checklist',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './box-material-checklist.component.html',
  styleUrls: ['./box-material-checklist.component.scss']
})
export class BoxMaterialChecklistComponent implements OnInit {
  @Input() boxId!: string;
  
  materials: BoxMaterial[] = [];
  isLoading = false;
  filterStatus: 'all' | 'arrived' | 'pending' | 'overdue' = 'all';

  constructor(
    private boxMaterialService: BoxMaterialService,
    private toastService: ToastService
  ) {}

  ngOnInit(): void {
    this.loadBoxMaterials();
  }

  loadBoxMaterials(): void {
    this.isLoading = true;
    this.boxMaterialService.getBoxMaterials(this.boxId).subscribe({
      next: (materials) => {
        this.materials = materials.map(m => ({
          ...m,
          requiredByDate: new Date(m.requiredByDate),
          arrivedDate: m.arrivedDate ? new Date(m.arrivedDate) : undefined,
          daysUntilRequired: this.calculateDaysRemaining(new Date(m.requiredByDate)),
          isOverdue: this.isOverdue(new Date(m.requiredByDate), m.isArrived),
          status: this.getStatus(new Date(m.requiredByDate), m.isArrived)
        }));
        this.isLoading = false;
      },
      error: (error) => {
        this.toastService.error('Failed to load materials');
        this.isLoading = false;
      }
    });
  }

  calculateDaysRemaining(requiredByDate: Date): number {
    const today = new Date();
    today.setHours(0, 0, 0, 0);
    const required = new Date(requiredByDate);
    required.setHours(0, 0, 0, 0);
    return Math.ceil((required.getTime() - today.getTime()) / (1000 * 60 * 60 * 24));
  }

  isOverdue(requiredByDate: Date, isArrived: boolean): boolean {
    return !isArrived && this.calculateDaysRemaining(requiredByDate) < 0;
  }

  getStatus(requiredByDate: Date, isArrived: boolean): 'Arrived' | 'Overdue' | 'Approaching' | 'Pending' {
    if (isArrived) return 'Arrived';
    
    const daysRemaining = this.calculateDaysRemaining(requiredByDate);
    if (daysRemaining < 0) return 'Overdue';
    if (daysRemaining <= 10) return 'Approaching';
    return 'Pending';
  }

  // Material arrival toggling is disabled - this is a read-only view
  // Users can only view material status, not change it
  toggleMaterialArrival(material: BoxMaterial): void {
    // Read-only mode - no actions allowed
    return;
  }

  getFilteredMaterials(): BoxMaterial[] {
    switch (this.filterStatus) {
      case 'arrived':
        return this.materials.filter(m => m.isArrived);
      case 'pending':
        return this.materials.filter(m => !m.isArrived && !m.isOverdue);
      case 'overdue':
        return this.materials.filter(m => m.isOverdue);
      default:
        return this.materials;
    }
  }

  getMaterialsByCategory(): { categoryName: string; materials: BoxMaterial[] }[] {
    const filteredMaterials = this.getFilteredMaterials();
    
    // Group materials by category
    const categoryMap = new Map<string, BoxMaterial[]>();
    
    filteredMaterials.forEach(material => {
      const category = material.materialCategory || 'Uncategorized';
      if (!categoryMap.has(category)) {
        categoryMap.set(category, []);
      }
      categoryMap.get(category)!.push(material);
    });
    
    // Convert to array and sort
    return Array.from(categoryMap.entries())
      .map(([categoryName, materials]) => ({
        categoryName,
        materials: materials.sort((a, b) => a.materialName.localeCompare(b.materialName))
      }))
      .sort((a, b) => a.categoryName.localeCompare(b.categoryName));
  }

  getStatusCounts(): { all: number; arrived: number; pending: number; overdue: number } {
    return {
      all: this.materials.length,
      arrived: this.materials.filter(m => m.isArrived).length,
      pending: this.materials.filter(m => !m.isArrived && !m.isOverdue).length,
      overdue: this.materials.filter(m => m.isOverdue).length
    };
  }

  getCompletionPercentage(): number {
    if (this.materials.length === 0) return 0;
    const arrived = this.materials.filter(m => m.isArrived).length;
    return Math.round((arrived / this.materials.length) * 100);
  }

  getDaysRemainingText(material: BoxMaterial): string {
    if (material.isArrived) {
      return 'Arrived';
    }
    
    const days = material.daysUntilRequired;
    if (days < 0) {
      return `${Math.abs(days)} days overdue`;
    } else if (days === 0) {
      return 'Due today';
    } else if (days === 1) {
      return '1 day remaining';
    } else {
      return `${days} days remaining`;
    }
  }

  getLeadTimeDisplay(days: number): string {
    return days >= 30 ? '1 Month' : '1 Week';
  }
}

