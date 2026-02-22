import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { BoxTypeMaterial, MarkMaterialArrivedRequest } from '../models/box-type-material.model';

@Injectable({
  providedIn: 'root'
})
export class BoxTypeMaterialService {
  private apiUrl = `${environment.apiUrl}/box-type-materials`;

  constructor(private http: HttpClient) {}

  /**
   * Get all materials for a specific box type
   */
  getBoxTypeMaterials(projectBoxTypeId: number): Observable<BoxTypeMaterial[]> {
    return this.http.get<BoxTypeMaterial[]>(`${this.apiUrl}/box-type/${projectBoxTypeId}`);
  }

  /**
   * Get all box type materials for a project (across all box types)
   * Optionally filter by building and/or level
   */
  getProjectBoxTypeMaterials(projectId: string, buildingNumber?: string, floor?: string): Observable<BoxTypeMaterial[]> {
    let params: any = {};
    if (buildingNumber) {
      params.buildingNumber = buildingNumber;
    }
    if (floor) {
      params.floor = floor;
    }
    return this.http.get<BoxTypeMaterial[]>(`${this.apiUrl}/project/${projectId}`, { params });
  }

  /**
   * Mark a box type material as arrived
   */
  markAsArrived(boxTypeMaterialId: string, request: MarkMaterialArrivedRequest): Observable<any> {
    const body = {
      deliveryProgress: request.deliveryProgress ?? 100,
      arrivedQuantity: request.arrivedQuantity,
      deliveredQuantity: request.deliveredQuantity,
      notes: request.notes
    };
    return this.http.post(`${this.apiUrl}/${boxTypeMaterialId}/mark-arrived`, body);
  }

  /**
   * Mark a box type material as pending (undo arrival)
   */
  markAsPending(boxTypeMaterialId: string): Observable<any> {
    return this.http.post(`${this.apiUrl}/${boxTypeMaterialId}/mark-pending`, {});
  }

  /**
   * Sync box type materials from project-level materials and templates
   */
  syncBoxTypeMaterials(projectId: string): Observable<{ message: string; count: number }> {
    return this.http.post<{ message: string; count: number }>(`${this.apiUrl}/project/${projectId}/sync`, {});
  }

  /**
   * Remove a material assignment from a specific box type
   * Allows users to remove materials that were auto-assigned from project-level selections
   */
  removeBoxTypeMaterial(boxTypeMaterialId: string): Observable<{ message: string }> {
    return this.http.delete<{ message: string }>(`${this.apiUrl}/${boxTypeMaterialId}`);
  }

  /**
   * Update the quantity per box for a specific box type material
   */
  updateQuantityPerBox(boxTypeMaterialId: string, quantityPerBox: number): Observable<{ message: string }> {
    return this.http.put<{ message: string }>(`${this.apiUrl}/${boxTypeMaterialId}/quantity`, {
      quantityPerBox
    });
  }

  /**
   * Export box type materials to Excel for a specific box type
   */
  exportBoxTypeMaterialsToExcel(projectBoxTypeId: number): Observable<Blob> {
    return this.http.get(`${this.apiUrl}/box-type/${projectBoxTypeId}/export`, { 
      responseType: 'blob' 
    });
  }

  /**
   * Import box type materials from Excel file
   */
  importBoxTypeMaterialsFromExcel(projectBoxTypeId: number, file: File): Observable<{
    message: string;
    successCount: number;
    failureCount: number;
    errors: string[];
    warnings: string[];
  }> {
    const formData = new FormData();
    formData.append('file', file);
    
    return this.http.post<{
      message: string;
      successCount: number;
      failureCount: number;
      errors: string[];
      warnings: string[];
    }>(`${this.apiUrl}/box-type/${projectBoxTypeId}/import`, formData);
  }

  /**
   * Export all box type materials for a project to Excel
   */
  exportProjectMaterialsToExcel(projectId: string): Observable<Blob> {
    return this.http.get(`${this.apiUrl}/project/${projectId}/export`, { 
      responseType: 'blob' 
    });
  }

  /**
   * Import all box type materials for a project from Excel file
   */
  importProjectMaterialsFromExcel(projectId: string, file: File): Observable<{
    message: string;
    successCount: number;
    failureCount: number;
    errors: string[];
    warnings: string[];
  }> {
    const formData = new FormData();
    formData.append('file', file);
    
    return this.http.post<{
      message: string;
      successCount: number;
      failureCount: number;
      errors: string[];
      warnings: string[];
    }>(`${this.apiUrl}/project/${projectId}/import`, formData);
  }
}

