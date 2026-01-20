import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { PanelType } from '../models/box.model';

export interface CreatePanelTypeDto {
  projectId: string;
  panelTypeName: string;
  panelTypeCode: string;
  description?: string;
  displayOrder?: number;
}

export interface UpdatePanelTypeDto {
  panelTypeId: string;
  panelTypeName: string;
  panelTypeCode: string;
  description?: string;
  isActive: boolean;
  displayOrder: number;
}

@Injectable({
  providedIn: 'root'
})
export class PanelTypeService {
  private apiUrl = `${environment.apiUrl}/projects`;

  constructor(private http: HttpClient) {}

  getPanelTypesByProject(projectId: string, includeInactive: boolean = false): Observable<any> {
    return this.http.get(`${this.apiUrl}/${projectId}/paneltypes?includeInactive=${includeInactive}`);
  }

  getPanelTypeById(projectId: string, panelTypeId: string): Observable<any> {
    return this.http.get(`${this.apiUrl}/${projectId}/paneltypes/${panelTypeId}`);
  }

  createPanelType(projectId: string, dto: CreatePanelTypeDto): Observable<any> {
    return this.http.post(`${this.apiUrl}/${projectId}/paneltypes`, dto);
  }

  updatePanelType(projectId: string, panelTypeId: string, dto: UpdatePanelTypeDto): Observable<any> {
    return this.http.put(`${this.apiUrl}/${projectId}/paneltypes/${panelTypeId}`, dto);
  }

  deletePanelType(projectId: string, panelTypeId: string): Observable<any> {
    return this.http.delete(`${this.apiUrl}/${projectId}/paneltypes/${panelTypeId}`);
  }
}

