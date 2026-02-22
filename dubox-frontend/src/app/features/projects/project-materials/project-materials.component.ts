import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { ProjectService } from '../../../core/services/project.service';
import { HeaderComponent } from '../../../shared/components/header/header.component';
import { SidebarComponent } from '../../../shared/components/sidebar/sidebar.component';
import { ProjectMaterialSelectionComponent } from '../../materials/components/project-material-selection/project-material-selection.component';
import { Project } from '../../../core/models/project.model';

@Component({
  selector: 'app-project-materials',
  standalone: true,
  imports: [
    CommonModule, 
    RouterModule, 
    HeaderComponent, 
    SidebarComponent, 
    ProjectMaterialSelectionComponent
  ],
  templateUrl: './project-materials.component.html',
  styleUrls: ['./project-materials.component.scss']
})
export class ProjectMaterialsComponent implements OnInit {
  projectId: string = '';
  project: Project | null = null;
  loading = true;
  error = '';

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private projectService: ProjectService
  ) {}

  ngOnInit(): void {
    this.projectId = this.route.snapshot.params['id'];
    if (!this.projectId) {
      this.error = 'Project ID is missing';
      this.loading = false;
      return;
    }
    
    this.loadProject();
  }

  loadProject(): void {
    this.loading = true;
    this.projectService.getProject(this.projectId).subscribe({
      next: (project) => {
        this.project = project;
        this.loading = false;
      },
      error: (error) => {
        this.error = 'Failed to load project';
        this.loading = false;
        console.error('Error loading project:', error);
      }
    });
  }

  goBack(): void {
    this.router.navigate(['/projects', this.projectId, 'dashboard']);
  }

  goToProjectDashboard(): void {
    this.router.navigate(['/projects', this.projectId, 'dashboard']);
  }
}






