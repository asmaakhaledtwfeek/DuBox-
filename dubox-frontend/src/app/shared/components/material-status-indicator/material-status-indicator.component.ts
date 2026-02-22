import { Component, Input, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { BoxMaterialService } from '../../../core/services/box-material.service';

@Component({
  selector: 'app-material-status-indicator',
  standalone: true,
  imports: [CommonModule],
  template: `
    <div class="material-status-indicator" [attr.data-status]="overallStatus" *ngIf="!isLoading">
      <span class="status-icon">{{ getStatusIcon() }}</span>
      <span class="status-text">{{ statusText }}</span>
      <span class="status-details" *ngIf="totalMaterials > 0">
        {{ arrivedCount }}/{{ totalMaterials }}
      </span>
    </div>
    <div class="material-status-loading" *ngIf="isLoading">
      <span class="spinner-border spinner-border-sm"></span>
    </div>
  `,
  styles: [`
    .material-status-indicator {
      display: inline-flex;
      align-items: center;
      gap: 6px;
      padding: 4px 10px;
      border-radius: 12px;
      font-size: 12px;
      font-weight: 500;
      
      &[data-status="complete"] {
        background: #d4edda;
        color: #155724;
      }
      
      &[data-status="overdue"] {
        background: #f8d7da;
        color: #721c24;
      }
      
      &[data-status="approaching"] {
        background: #fff3cd;
        color: #856404;
      }
      
      &[data-status="pending"] {
        background: #d1ecf1;
        color: #0c5460;
      }
      
      &[data-status="no-materials"] {
        background: #e2e3e5;
        color: #383d41;
      }
      
      .status-icon {
        font-size: 14px;
      }
      
      .status-details {
        margin-left: 4px;
        opacity: 0.8;
      }
    }
    
    .material-status-loading {
      padding: 4px 10px;
    }
  `]
})
export class MaterialStatusIndicatorComponent implements OnInit {
  @Input() boxId!: string;
  
  isLoading = false;
  totalMaterials = 0;
  arrivedCount = 0;
  overdueCount = 0;
  approachingCount = 0;
  overallStatus: 'complete' | 'overdue' | 'approaching' | 'pending' | 'no-materials' = 'pending';
  statusText = '';

  constructor(private boxMaterialService: BoxMaterialService) {}

  ngOnInit(): void {
    this.loadMaterialStatus();
  }

  loadMaterialStatus(): void {
    this.isLoading = true;
    this.boxMaterialService.getBoxMaterials(this.boxId).subscribe({
      next: (materials) => {
        this.totalMaterials = materials.length;
        
        if (this.totalMaterials === 0) {
          this.overallStatus = 'no-materials';
          this.statusText = 'No materials';
          this.isLoading = false;
          return;
        }
        
        this.arrivedCount = materials.filter(m => m.isArrived).length;
        this.overdueCount = materials.filter(m => !m.isArrived && this.isOverdue(new Date(m.requiredByDate))).length;
        this.approachingCount = materials.filter(m => 
          !m.isArrived && 
          !this.isOverdue(new Date(m.requiredByDate)) && 
          this.calculateDaysRemaining(new Date(m.requiredByDate)) <= 10
        ).length;
        
        // Determine overall status
        if (this.arrivedCount === this.totalMaterials) {
          this.overallStatus = 'complete';
          this.statusText = 'All Ready';
        } else if (this.overdueCount > 0) {
          this.overallStatus = 'overdue';
          this.statusText = `${this.overdueCount} Overdue`;
        } else if (this.approachingCount > 0) {
          this.overallStatus = 'approaching';
          this.statusText = `${this.approachingCount} Due Soon`;
        } else {
          this.overallStatus = 'pending';
          this.statusText = 'Pending';
        }
        
        this.isLoading = false;
      },
      error: () => {
        this.isLoading = false;
        this.overallStatus = 'no-materials';
        this.statusText = 'Error';
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

  isOverdue(requiredByDate: Date): boolean {
    return this.calculateDaysRemaining(requiredByDate) < 0;
  }

  getStatusIcon(): string {
    switch (this.overallStatus) {
      case 'complete': return '✓';
      case 'overdue': return '⚠';
      case 'approaching': return '⏰';
      case 'pending': return '○';
      default: return '—';
    }
  }
}






