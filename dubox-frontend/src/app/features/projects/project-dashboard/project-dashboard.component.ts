import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { ProjectService } from '../../../core/services/project.service';
import { Project } from '../../../core/models/project.model';
import { HeaderComponent } from '../../../shared/components/header/header.component';
import { SidebarComponent } from '../../../shared/components/sidebar/sidebar.component';

@Component({
  selector: 'app-project-dashboard',
  standalone: true,
  imports: [CommonModule, RouterModule, HeaderComponent, SidebarComponent],
  templateUrl: './project-dashboard.component.html',
  styleUrl: './project-dashboard.component.scss'
})
export class ProjectDashboardComponent implements OnInit {
  project: Project | null = null;
  projectId: string = '';
  loading = true;
  error = '';

  dashboardData = {
    totalBoxes: 0,
    completedBoxes: 0,
    inProgressBoxes: 0,
    readyForDelivery: 0,
    notStarted: 0,
    onHold: 0
  };

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private projectService: ProjectService
  ) {}

  ngOnInit(): void {
    this.projectId = this.route.snapshot.params['id'];
    this.loadProject();
  }

  loadProject(): void {
    this.loading = true;
    this.projectService.getProject(this.projectId).subscribe({
      next: (project) => {
        this.project = project;
        this.dashboardData = {
          totalBoxes: project.totalBoxes || 0,
          completedBoxes: project.completedBoxes || 0,
          inProgressBoxes: project.inProgressBoxes || 0,
          readyForDelivery: project.readyForDeliveryBoxes || 0,
          notStarted: 0,
          onHold: 0
        };
        this.loading = false;
      },
      error: (error) => {
        this.error = 'Failed to load project';
        this.loading = false;
        console.error('Error loading project:', error);
      }
    });
  }

  viewBoxes(): void {
    this.router.navigate(['/projects', this.projectId, 'boxes']);
  }

  goBack(): void {
    this.router.navigate(['/projects']);
  }
}
