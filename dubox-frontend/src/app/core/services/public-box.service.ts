import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { map, catchError } from 'rxjs/operators';
import { environment } from '../../../environments/environment';

/**
 * Public box data model (read-only view)
 */
export interface PublicBox {
  boxId: string;
  projectCode: string;
  projectName: string;
  clientName: string;
  boxTag: string;
  serialNumber?: string;
  boxName?: string;
  boxType: string;
  boxSubTypeName?: string;
  floor?: string;
  buildingNumber?: string;
  boxFunction?: string;
  zone?: string;
  progressPercentage: number;
  status: string;
  length?: number;
  width?: number;
  height?: number;
  unitOfMeasure?: string;
  plannedStartDate?: Date;
  actualStartDate?: Date;
  plannedEndDate?: Date;
  actualEndDate?: Date;
  activitiesCount: number;
  currentLocationName?: string;
  factoryName?: string;
  bay?: string;
  row?: string;
  position?: string;
}

/**
 * Service for accessing public box data (no authentication required)
 * Used for QR code scans and public box view
 */
@Injectable({
  providedIn: 'root'
})
export class PublicBoxService {
  private readonly publicApiUrl = environment.publicApiUrl;

  constructor(private http: HttpClient) {}

  /**
   * Get public box details by ID (no authentication required)
   * @param boxId The box ID
   * @returns Observable of public box data
   */
  getPublicBox(boxId: string): Observable<PublicBox> {
    return this.http.get<any>(`${this.publicApiUrl}/boxes/${boxId}`).pipe(
      map(response => {
        // Handle wrapped response (Result<T>) or direct response
        const data = response?.data || response;
        return this.transformPublicBox(data);
      }),
      catchError(error => {
        console.error('Error fetching public box:', error);
        return throwError(() => error);
      })
    );
  }

  /**
   * Transform backend response to PublicBox model
   */
  private transformPublicBox(data: any): PublicBox {
    return {
      boxId: data.boxId,
      projectCode: data.projectCode || '',
      projectName: data.projectName || '',
      clientName: data.clientName || '',
      boxTag: data.boxTag || '',
      serialNumber: data.serialNumber,
      boxName: data.boxName,
      boxType: data.boxType || '',
      boxSubTypeName: data.boxSubTypeName,
      floor: data.floor,
      buildingNumber: data.buildingNumber,
      boxFunction: data.boxFunction,
      zone: data.zone,
      progressPercentage: data.progressPercentage || 0,
      status: data.status || 'Unknown',
      length: data.length,
      width: data.width,
      height: data.height,
      unitOfMeasure: data.unitOfMeasure,
      plannedStartDate: data.plannedStartDate ? new Date(data.plannedStartDate) : undefined,
      actualStartDate: data.actualStartDate ? new Date(data.actualStartDate) : undefined,
      plannedEndDate: data.plannedEndDate ? new Date(data.plannedEndDate) : undefined,
      actualEndDate: data.actualEndDate ? new Date(data.actualEndDate) : undefined,
      activitiesCount: data.activitiesCount || 0,
      currentLocationName: data.currentLocationName,
      factoryName: data.factoryName,
      bay: data.bay,
      row: data.row,
      position: data.position
    };
  }

  /**
   * Get status display info
   */
  getStatusInfo(status: string): { label: string; class: string; color: string } {
    const statusMap: Record<string, { label: string; class: string; color: string }> = {
      'NotStarted': { label: 'Not Started', class: 'status-not-started', color: '#6b7280' },
      'InProgress': { label: 'In Progress', class: 'status-in-progress', color: '#3b82f6' },
      'Completed': { label: 'Completed', class: 'status-completed', color: '#10b981' },
      'OnHold': { label: 'On Hold', class: 'status-on-hold', color: '#f59e0b' },
      'Delayed': { label: 'Delayed', class: 'status-delayed', color: '#ef4444' },
      'Dispatched': { label: 'Dispatched', class: 'status-dispatched', color: '#8b5cf6' }
    };
    return statusMap[status] || { label: status, class: 'status-unknown', color: '#6b7280' };
  }
}

