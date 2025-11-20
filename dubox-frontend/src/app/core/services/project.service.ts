import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { ApiService } from './api.service';
import { Project, ProjectStats } from '../models/project.model';

@Injectable({
  providedIn: 'root'
})
export class ProjectService {
  private readonly endpoint = 'projects';

  constructor(private apiService: ApiService) {}

  /**
   * Transform backend project response to frontend model
   */
  private transformProject(backendProject: any): Project {
    console.log('🔄 Transforming project:', backendProject);
    console.log('🔑 Available keys:', Object.keys(backendProject));
    
    // Try multiple possible field name variations (camelCase, PascalCase)
    const id = backendProject.projectId || backendProject.ProjectId || backendProject.id || backendProject.Id;
    const name = backendProject.projectName || backendProject.ProjectName || backendProject.name || backendProject.Name;
    const code = backendProject.projectCode || backendProject.ProjectCode || backendProject.code || backendProject.Code;
    
    if (!id) {
      console.error('❌ No project ID found! Backend project:', backendProject);
    }
    
    const transformed = {
      id: id,
      name: name,
      code: code,
      location: backendProject.location || backendProject.Location || '',
      description: backendProject.description || backendProject.Description,
      startDate:( backendProject.startDate || backendProject.StartDate) ? new Date(backendProject.startDate || backendProject.StartDate) : undefined,
      endDate: (backendProject.plannedEndDate || backendProject.PlannedEndDate) ? new Date(backendProject.plannedEndDate || backendProject.PlannedEndDate) : undefined,
      status: backendProject.status || backendProject.Status,
      totalBoxes: backendProject.totalBoxes || backendProject.TotalBoxes || 0,
      completedBoxes: backendProject.completedBoxes || backendProject.CompletedBoxes || 0,
      inProgressBoxes: backendProject.inProgressBoxes || backendProject.InProgressBoxes || 0,
      readyForDeliveryBoxes: backendProject.readyForDeliveryBoxes || backendProject.ReadyForDeliveryBoxes || 0,
      progress: backendProject.progress || backendProject.Progress || 0,
      createdBy: backendProject.createdBy || backendProject.CreatedBy,
      updatedBy: backendProject.modifiedBy || backendProject.ModifiedBy || backendProject.updatedBy || backendProject.UpdatedBy,
      createdAt: (backendProject.createdDate || backendProject.CreatedDate) ? new Date(backendProject.createdDate || backendProject.CreatedDate) : undefined,
      updatedAt: (backendProject.modifiedDate || backendProject.ModifiedDate) ? new Date(backendProject.modifiedDate || backendProject.ModifiedDate) : undefined
    };
    
    console.log('✅ Transformed project:', transformed);
    console.log('🆔 Project ID:', transformed.id);
    
    if (!transformed.id) {
      console.error('⚠️ WARNING: Transformed project has no ID!');
    }
    
    return transformed;
  }

  /**
   * Get all projects
   */
  getProjects(params?: any): Observable<Project[]> {
    return this.apiService.get<any[]>(this.endpoint, params).pipe(
      map(projects => {
        console.log('🔥 RAW PROJECTS FROM API (before transformation):', projects);
        console.log('🔥 Is it an array?', Array.isArray(projects));
        
        if (!projects) {
          console.error('❌ Projects is null/undefined!');
          return [];
        }
        
        if (!Array.isArray(projects)) {
          console.error('❌ Projects is NOT an array! Type:', typeof projects);
          console.error('❌ Projects value:', projects);
          return [];
        }
        
        if (projects.length > 0) {
          console.log('🔥 FIRST PROJECT RAW:', projects[0]);
          console.log('🔥 FIRST PROJECT KEYS:', Object.keys(projects[0]));
          console.log('🔥 FIRST PROJECT projectId:', projects[0].projectId);
        } else {
          console.log('⚠️ No projects in array');
        }
        
        const transformed = projects.map(p => this.transformProject(p));
        console.log('🔥 TRANSFORMED PROJECTS:', transformed);
        if (transformed.length > 0) {
          console.log('🔥 FIRST TRANSFORMED PROJECT:', transformed[0]);
          console.log('🔥 FIRST TRANSFORMED PROJECT ID:', transformed[0].id);
        }
        
        return transformed;
      })
    );
  }

  /**
   * Get project by ID
   */
  getProject(id: string): Observable<Project> {
    return this.apiService.get<any>(`${this.endpoint}/${id}`).pipe(
      map(project => this.transformProject(project))
    );
  }

  /**
   * Create new project
   */
  createProject(project: Partial<Project>): Observable<Project> {
    return this.apiService.post<any>(this.endpoint, project).pipe(
      map(response => this.transformProject(response))
    );
  }

  /**
   * Update project
   */
  updateProject(id: string, project: Partial<Project>): Observable<Project> {
    return this.apiService.put<any>(`${this.endpoint}/${id}`, project).pipe(
      map(response => this.transformProject(response))
    );
  }

  /**
   * Delete project
   */
  deleteProject(id: string): Observable<void> {
    return this.apiService.delete<void>(`${this.endpoint}/${id}`);
  }

  /**
   * Get project statistics
   */
  getProjectStats(id?: string): Observable<ProjectStats> {
    const url = id ? `${this.endpoint}/${id}/stats` : `${this.endpoint}/stats`;
    return this.apiService.get<ProjectStats>(url);
  }

  /**
   * Get project dashboard data
   */
  getProjectDashboard(id: string): Observable<any> {
    return this.apiService.get<any>(`${this.endpoint}/${id}/dashboard`);
  }
}
