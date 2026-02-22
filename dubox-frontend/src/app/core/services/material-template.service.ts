import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { environment } from '../../../environments/environment';
import {
  MaterialTemplate,
  ProjectMaterialTemplate,
  BoxTypeMaterialTemplate,
  CreateMaterialTemplateRequest,
  UpdateMaterialTemplateRequest,
  AssignToProjectRequest,
  AssignToBoxTypeRequest
} from '../models/material-template.model';

@Injectable({
  providedIn: 'root'
})
export class MaterialTemplateService {
  private readonly apiUrl = `${environment.apiUrl}/material-templates`;

  constructor(private http: HttpClient) {}

  /**
   * Get all material templates
   */
  getAllTemplates(activeOnly: boolean = true, category?: string, searchTerm?: string): Observable<any> {
    let params = new HttpParams();
    params = params.set('activeOnly', activeOnly.toString());
    if (category) {
      params = params.set('category', category);
    }
    if (searchTerm) {
      params = params.set('searchTerm', searchTerm);
    }
    return this.http.get<any>(this.apiUrl, { params }).pipe(
      map(response => response.data || [])
    );
  }

  /**
   * Get template by ID
   */
  getTemplateById(id: string): Observable<any> {
    return this.http.get<any>(`${this.apiUrl}/${id}`).pipe(
      map(response => response.data)
    );
  }

  /**
   * Create a new template
   */
  createTemplate(request: CreateMaterialTemplateRequest): Observable<any> {
    return this.http.post<any>(this.apiUrl, request);
  }

  /**
   * Update an existing template
   */
  updateTemplate(id: string, request: UpdateMaterialTemplateRequest): Observable<any> {
    return this.http.put<any>(`${this.apiUrl}/${id}`, request);
  }

  /**
   * Delete (deactivate) a template
   */
  deleteTemplate(id: string): Observable<any> {
    return this.http.delete<any>(`${this.apiUrl}/${id}`);
  }

  /**
   * Assign template to project
   */
  assignToProject(templateId: string, request: AssignToProjectRequest): Observable<any> {
    return this.http.post<any>(`${this.apiUrl}/${templateId}/assign-to-project`, request);
  }

  /**
   * Assign template to box type
   */
  assignToBoxType(templateId: string, request: AssignToBoxTypeRequest): Observable<any> {
    return this.http.post<any>(`${this.apiUrl}/${templateId}/assign-to-box-type`, request);
  }

  /**
   * Get templates assigned to a project
   */
  getProjectTemplates(projectId: string): Observable<any> {
    return this.http.get<any>(`${this.apiUrl}/projects/${projectId}`).pipe(
      map(response => response.data || [])
    );
  }

  /**
   * Get templates assigned to box types in a project
   */
  getBoxTypeTemplates(projectId: string): Observable<any> {
    return this.http.get<any>(`${this.apiUrl}/projects/${projectId}/box-types`).pipe(
      map(response => response.data || [])
    );
  }

  /**
   * Get available materials for template
   */
  getAvailableMaterials(searchTerm?: string, category?: string): Observable<any> {
    let params = new HttpParams();
    if (searchTerm) {
      params = params.set('searchTerm', searchTerm);
    }
    if (category) {
      params = params.set('category', category);
    }
    return this.http.get<any>(`${this.apiUrl}/available-materials`, { params }).pipe(
      map(response => response.data || [])
    );
  }

  /**
   * Remove template from project
   */
  removeFromProject(projectId: string, templateId: string): Observable<any> {
    return this.http.delete<any>(`${this.apiUrl}/projects/${projectId}/templates/${templateId}`);
  }

  /**
   * Remove template from box type
   */
  removeFromBoxType(boxTypeId: number, templateId: string): Observable<any> {
    return this.http.delete<any>(`${this.apiUrl}/box-types/${boxTypeId}/templates/${templateId}`);
  }
}

