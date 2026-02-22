import { Component, OnInit, Input } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { BoxMaterialService } from '../../../../core/services/box-material.service';

interface MaterialSummary {
  totalBoxes: number;
  boxesWithAllMaterials: number;
  boxesWithOverdueMaterials: number;
  boxesWithApproachingMaterials: number;
  totalMaterials: number;
  arrivedMaterials: number;
  overdueMaterials: number;
  approachingMaterials: number;
}

@Component({
  selector: 'app-material-readiness-widget',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './material-readiness-widget.component.html',
  styleUrls: ['./material-readiness-widget.component.scss']
})
export class MaterialReadinessWidgetComponent implements OnInit {
  @Input() projectId!: string;
  
  summary: MaterialSummary = {
    totalBoxes: 0,
    boxesWithAllMaterials: 0,
    boxesWithOverdueMaterials: 0,
    boxesWithApproachingMaterials: 0,
    totalMaterials: 0,
    arrivedMaterials: 0,
    overdueMaterials: 0,
    approachingMaterials: 0
  };
  
  isLoading = false;
  criticalBoxes: Array<{ boxId: string; boxTag: string; overdueCount: number }> = [];

  constructor(private boxMaterialService: BoxMaterialService) {}

  ngOnInit(): void {
    this.loadMaterialReadiness();
  }

  loadMaterialReadiness(): void {
    this.isLoading = true;
    
    // In a real implementation, you would have a dedicated endpoint for project-level summary
    // For now, this is a placeholder that shows the structure
    
    // Example API call would be:
    // this.boxMaterialService.getProjectMaterialSummary(this.projectId).subscribe(...)
    
    // Mock data for demonstration:
    setTimeout(() => {
      this.summary = {
        totalBoxes: 24,
        boxesWithAllMaterials: 15,
        boxesWithOverdueMaterials: 3,
        boxesWithApproachingMaterials: 6,
        totalMaterials: 180,
        arrivedMaterials: 142,
        overdueMaterials: 12,
        approachingMaterials: 26
      };
      
      this.criticalBoxes = [
        { boxId: '1', boxTag: 'BOX-001', overdueCount: 5 },
        { boxId: '2', boxTag: 'BOX-012', overdueCount: 4 },
        { boxId: '3', boxTag: 'BOX-008', overdueCount: 3 }
      ];
      
      this.isLoading = false;
    }, 500);
  }

  getCompletionPercentage(): number {
    if (this.summary.totalMaterials === 0) return 0;
    return Math.round((this.summary.arrivedMaterials / this.summary.totalMaterials) * 100);
  }

  getBoxReadinessPercentage(): number {
    if (this.summary.totalBoxes === 0) return 0;
    return Math.round((this.summary.boxesWithAllMaterials / this.summary.totalBoxes) * 100);
  }

  hasIssues(): boolean {
    return this.summary.boxesWithOverdueMaterials > 0 || this.summary.boxesWithApproachingMaterials > 0;
  }
}






