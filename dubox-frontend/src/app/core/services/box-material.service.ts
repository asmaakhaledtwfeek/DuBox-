import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ApiService } from './api.service';
import { BoxMaterial } from '../models/box-material.model';
import { ProjectMaterialDto } from '../models/project-material.model';

@Injectable({
  providedIn: 'root'
})
export class BoxMaterialService {
  
  constructor(private apiService: ApiService) {}

  /**
   * Get material checklist for a box
   */
  getBoxMaterials(
    boxId: string, 
    onlyOverdue?: boolean, 
    onlyPending?: boolean
  ): Observable<BoxMaterial[]> {
    const params: any = {};
    if (onlyOverdue !== undefined) params.onlyOverdue = onlyOverdue;
    if (onlyPending !== undefined) params.onlyPending = onlyPending;
    
    return this.apiService.get<BoxMaterial[]>(`boxes/${boxId}/materials`, params);
  }

  /**
   * Mark material as arrived or not arrived
   */
  markMaterialArrived(
    boxId: string, 
    materialId: string, 
    isArrived: boolean
  ): Observable<any> {
    const endpoint = isArrived 
      ? `boxes/${boxId}/materials/${materialId}/arrived`
      : `boxes/${boxId}/materials/${materialId}/not-arrived`;
    
    return this.apiService.put<any>(endpoint, {});
  }

  /**
   * Get project materials
   */
  getProjectMaterials(projectId: string, selectedOnly: boolean = false): Observable<ProjectMaterialDto[]> {
    return this.apiService.get<ProjectMaterialDto[]>(
      `projects/${projectId}/materials`,
      { selectedOnly }
    );
  }

  /**
   * Select materials for a project
   */
  selectProjectMaterials(
    projectId: string,
    materialIds: string[],
    selectAll: boolean = false
  ): Observable<any> {
    return this.apiService.post(
      `projects/${projectId}/materials/select`,
      { materialIds, selectAll }
    );
  }
}

