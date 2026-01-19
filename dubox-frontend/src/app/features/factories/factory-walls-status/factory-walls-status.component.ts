import { Component, OnInit, OnDestroy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { Subscription } from 'rxjs';
import { FactoryService, Factory } from '../../../core/services/factory.service';
import { BoxService } from '../../../core/services/box.service';
import { Box } from '../../../core/models/box.model';
import { ProjectService } from '../../../core/services/project.service';
import { Project } from '../../../core/models/project.model';
import { HeaderComponent } from '../../../shared/components/header/header.component';
import { SidebarComponent } from '../../../shared/components/sidebar/sidebar.component';

@Component({
  selector: 'app-factory-walls-status',
  standalone: true,
  imports: [CommonModule, RouterModule, FormsModule, HeaderComponent, SidebarComponent],
  templateUrl: './factory-walls-status.component.html',
  styleUrls: ['./factory-walls-status.component.scss']
})
export class FactoryWallsStatusComponent implements OnInit, OnDestroy {
  factoryId: string = '';
  factory: Factory | null = null;
  boxes: Box[] = [];
  filteredBoxes: Box[] = [];
  projects: Project[] = [];
  selectedProjectId: string = '';
  selectedBuildingNumber: string = '';
  selectedFloor: string = '';
  availableBuildingNumbers: string[] = [];
  availableFloors: string[] = [];
  loading = true;
  error = '';
  
  private subscriptions: Subscription[] = [];

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private factoryService: FactoryService,
    private boxService: BoxService,
    private projectService: ProjectService
  ) {}

  ngOnInit(): void {
    this.factoryId = this.route.snapshot.params['id'];
    this.loadProjects();
    this.loadData();
  }
  
  ngOnDestroy(): void {
    this.subscriptions.forEach(sub => sub.unsubscribe());
  }

  loadData(): void {
    this.loading = true;
    this.error = '';

    // Load factory details
    const factorySub = this.factoryService.getFactoryById(this.factoryId).subscribe({
      next: (factory: Factory) => {
        this.factory = factory;
        this.loadBoxes();
      },
      error: (err: any) => {
        this.error = 'Failed to load factory details';
        console.error('Error loading factory:', err);
        this.loading = false;
      }
    });

    this.subscriptions.push(factorySub);
  }

  loadProjects(): void {
    const projectsSub = this.projectService.getProjects().subscribe({
      next: (projects: Project[]) => {
        // Store all non-archived projects
        const allProjects = projects.filter(p => p.status !== 'Archived');
        // Will filter to show only projects with boxes after boxes are loaded
        this.projects = allProjects;
      },
      error: (err: any) => {
        console.error('Error loading projects:', err);
      }
    });

    this.subscriptions.push(projectsSub);
  }

  loadBoxes(): void {
    const boxesSub = this.boxService.getBoxesByFactory(this.factoryId).subscribe({
      next: (boxes: Box[]) => {
        // Filter boxes with position data
        this.boxes = boxes.filter(box => box.bay || box.row || box.position);
        this.filteredBoxes = [...this.boxes];
        this.updateProjectsList();
        this.updateBuildingsAndLevels();
        this.applyFilters();
        this.loading = false;
      },
      error: (err: any) => {
        this.error = 'Failed to load boxes';
        console.error('Error loading boxes:', err);
        this.loading = false;
      }
    });

    this.subscriptions.push(boxesSub);
  }

  /**
   * Update projects list to only show projects that have boxes in this factory
   */
  updateProjectsList(): void {
    if (this.boxes.length === 0) {
      return;
    }

    // Get unique project IDs from boxes
    const projectIdsWithBoxes = new Set<string>();
    this.boxes.forEach(box => {
      if (box.projectId) {
        projectIdsWithBoxes.add(box.projectId);
      }
    });

    // Filter projects to only show those with boxes
    this.projects = this.projects.filter(p => projectIdsWithBoxes.has(p.id));
  }

  /**
   * Update available building numbers and floors from boxes
   */
  updateBuildingsAndLevels(): void {
    if (this.boxes.length === 0) {
      return;
    }

    // Get unique building numbers
    const buildingsSet = new Set<string>();
    this.boxes.forEach(box => {
      if (box.buildingNumber) {
        buildingsSet.add(box.buildingNumber);
      }
    });
    this.availableBuildingNumbers = Array.from(buildingsSet).sort();

    // Get unique floors
    const floorsSet = new Set<string>();
    this.boxes.forEach(box => {
      if (box.floor) {
        floorsSet.add(box.floor);
      }
    });
    this.availableFloors = Array.from(floorsSet).sort();
  }

  applyFilters(): void {
    let filtered = [...this.boxes];

    // Filter by project
    if (this.selectedProjectId) {
      filtered = filtered.filter(box => box.projectId === this.selectedProjectId);
    }

    // Filter by building number
    if (this.selectedBuildingNumber) {
      filtered = filtered.filter(box => box.buildingNumber === this.selectedBuildingNumber);
    }

    // Filter by floor
    if (this.selectedFloor) {
      filtered = filtered.filter(box => box.floor === this.selectedFloor);
    }

    this.filteredBoxes = filtered;
  }

  clearFilters(): void {
    this.selectedProjectId = '';
    this.selectedBuildingNumber = '';
    this.selectedFloor = '';
    this.applyFilters();
  }

  goBack(): void {
    this.router.navigate(['/factories', this.factoryId, 'layout']);
  }

  /**
   * Calculate completion statistics
   */
  getCompletionStats(): { 
    total: number;
    wall1Complete: number;
    wall2Complete: number;
    wall3Complete: number;
    wall4Complete: number;
    slabComplete: number;
    soffitComplete: number;
    allComplete: number;
    podDeliverComplete: number;
  } {
    const stats = {
      total: this.filteredBoxes.length,
      wall1Complete: 0,
      wall2Complete: 0,
      wall3Complete: 0,
      wall4Complete: 0,
      slabComplete: 0,
      soffitComplete: 0,
      allComplete: 0,
      podDeliverComplete: 0
    };

    this.filteredBoxes.forEach(box => {
      if (box.wall1) stats.wall1Complete++;
      if (box.wall2) stats.wall2Complete++;
      if (box.wall3) stats.wall3Complete++;
      if (box.wall4) stats.wall4Complete++;
      if (box.slab) stats.slabComplete++;
      if (box.soffit) stats.soffitComplete++;
      if (box.podDeliver) stats.podDeliverComplete++;
      
      if (box.wall1 && box.wall2 && box.wall3 && box.wall4 && box.slab && box.soffit) {
        stats.allComplete++;
      }
    });

    return stats;
  }

  /**
   * Calculate percentage for a specific component
   */
  getCompletionPercentage(completed: number): number {
    if (this.filteredBoxes.length === 0) return 0;
    return Math.round((completed / this.filteredBoxes.length) * 100);
  }

  /**
   * Navigate to box details
   */
  viewBox(boxId: string): void {
    const box = this.filteredBoxes.find(b => b.id === boxId);
    if (box && box.projectId) {
      this.router.navigate(['/projects', box.projectId, 'boxes', boxId]);
    }
  }
}

