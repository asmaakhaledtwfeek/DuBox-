import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ApiService } from './api.service';
import { Project, ProjectStats } from '../models/project.model';

@Injectable({
  providedIn: 'root'
})
export class ProjectService {
  private readonly endpoint = 'projects';

  constructor(private apiService: ApiService) {}

  /**
   * Get all projects
   */
  getProjects(params?: any): Observable<Project[]> {
    return this.apiService.get<Project[]>(this.endpoint, params);
  }

  /**
   * Get project by ID
   */
  getProject(id: string): Observable<Project> {
    return this.apiService.get<Project>(`${this.endpoint}/${id}`);
  }

  /**
   * Create new project
   */
  createProject(project: Partial<Project>): Observable<Project> {
    return this.apiService.post<Project>(this.endpoint, project);
  }

  /**
   * Update project
   */
  updateProject(id: string, project: Partial<Project>): Observable<Project> {
    return this.apiService.put<Project>(`${this.endpoint}/${id}`, project);
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
