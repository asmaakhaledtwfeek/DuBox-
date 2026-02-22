import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router, ActivatedRoute, RouterModule } from '@angular/router';
import { ActivityTemplateService } from '../../../core/services/activity-template.service';
import { ActivityTemplate } from '../../../core/models/activity-template.model';
import { HeaderComponent } from '../../../shared/components/header/header.component';
import { SidebarComponent } from '../../../shared/components/sidebar/sidebar.component';

@Component({
  selector: 'app-view-activity-template',
  standalone: true,
  imports: [CommonModule, RouterModule, HeaderComponent, SidebarComponent],
  templateUrl: './view-activity-template.component.html',
  styleUrl: './view-activity-template.component.scss'
})
export class ViewActivityTemplateComponent implements OnInit {
  template: ActivityTemplate | null = null;
  templateId: string | null = null;
  loading = false;
  error: string | null = null;

  // Grouped activities by stage
  groupedActivities: Map<string, any[]> = new Map();
  expandedStages: Set<string> = new Set();
  private stageActivitiesCache: Map<string, any[]> = new Map();

  constructor(
    private templateService: ActivityTemplateService,
    private router: Router,
    private route: ActivatedRoute,
    private cdr: ChangeDetectorRef
  ) {}

  ngOnInit(): void {
    this.templateId = this.route.snapshot.paramMap.get('id');
    if (this.templateId) {
      this.loadTemplate();
    }
  }

  loadTemplate(): void {
    if (!this.templateId) return;

    this.loading = true;
    this.error = null;
    this.cdr.detectChanges();

    this.templateService.getTemplateById(this.templateId).subscribe({
      next: (template) => {
        this.template = template;
        this.groupActivitiesByStage();
        this.expandAllStages();
        this.loading = false;
        this.cdr.detectChanges();
      },
      error: (err) => {
        this.error = 'Failed to load template details';
        this.loading = false;
        this.cdr.detectChanges();
        console.error('Error loading template:', err);
      }
    });
  }

  groupActivitiesByStage(): void {
    if (!this.template || !this.template.activities) return;

    this.groupedActivities.clear();
    this.template.activities.forEach(activity => {
      const stage = activity.stage || 'Uncategorized';
      if (!this.groupedActivities.has(stage)) {
        this.groupedActivities.set(stage, []);
      }
      this.groupedActivities.get(stage)!.push(activity);
    });
  }

  getStageKeys(): string[] {
    return Array.from(this.groupedActivities.keys()).sort((a, b) => {
      const numA = parseInt(a.replace(/\D/g, ''), 10) || 0;
      const numB = parseInt(b.replace(/\D/g, ''), 10) || 0;
      return numA - numB;
    });
  }

  getActivitiesInStage(stage: string): any[] {
    // Use cache if stage is expanded and cache exists
    if (this.expandedStages.has(stage) && this.stageActivitiesCache.has(stage)) {
      return this.stageActivitiesCache.get(stage)!;
    }
    
    return this.groupedActivities.get(stage) || [];
  }

  isStageExpanded(stage: string): boolean {
    return this.expandedStages.has(stage);
  }

  toggleStage(stage: string): void {
    if (this.expandedStages.has(stage)) {
      this.expandedStages.delete(stage);
      // Clean up cache for collapsed stage
      this.stageActivitiesCache.delete(stage);
    } else {
      // Pre-compute and cache all activities for this stage before expanding
      const activities = this.groupedActivities.get(stage) || [];
      this.stageActivitiesCache.set(stage, activities);
      
      // Expand the stage
      this.expandedStages.add(stage);
    }
    // Trigger change detection manually for Set mutations
    this.cdr.detectChanges();
  }

  expandAllStages(): void {
    // Pre-compute and cache all activities for all stages before expanding
    this.getStageKeys().forEach(stage => {
      const activities = this.groupedActivities.get(stage) || [];
      this.stageActivitiesCache.set(stage, activities);
      this.expandedStages.add(stage);
    });
    this.cdr.detectChanges();
  }

  collapseAllStages(): void {
    this.expandedStages.clear();
    this.stageActivitiesCache.clear();
    this.cdr.detectChanges();
  }

  navigateToEdit(): void {
    if (this.templateId) {
      this.router.navigate(['/schedule/activity-templates', this.templateId, 'edit']);
    }
  }

  goBack(): void {
    this.router.navigate(['/schedule/activity-templates']);
  }

  getStatusClass(isActive: boolean): string {
    return isActive ? 'status-active' : 'status-inactive';
  }

  getStatusText(isActive: boolean): string {
    return isActive ? 'Active' : 'Inactive';
  }

  formatStageNum(n: number): string {
    return String(n).padStart(2, '0');
  }
}
