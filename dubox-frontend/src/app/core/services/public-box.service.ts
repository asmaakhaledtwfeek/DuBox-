import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { map, catchError } from 'rxjs/operators';
import { environment } from '../../../environments/environment';
import { BoxSummary } from '../models/box.model';

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
   * Get public box summary (no authentication required)
   * @param boxId The box ID
   * @returns Observable of box summary data
   */
  getPublicBoxSummary(boxId: string): Observable<BoxSummary> {
    return this.http.get<any>(`${this.publicApiUrl}/boxes/${boxId}/summary`).pipe(
      map(response => {
        const data = response?.data || response;
        return data as BoxSummary;
      }),
      catchError(error => {
        console.error('Error fetching public box summary:', error);
        return throwError(() => error);
      })
    );
  }

  /**
   * Get public box attachments (no authentication required)
   * @param boxId The box ID
   * @returns Observable of box attachments with data URLs
   */
  getPublicBoxAttachments(boxId: string): Observable<any> {
    return this.http.get<any>(`${this.publicApiUrl}/boxes/${boxId}/attachments`).pipe(
      map(response => {
        const data = response?.data || response;
        
        // Convert ImageData to data URLs for public viewing
        if (data) {
          if (data.wirCheckpointImages) {
            data.wirCheckpointImages = data.wirCheckpointImages.map((img: any) => 
              this.transformAttachmentWithDataUrl(img)
            );
          }
          if (data.progressUpdateImages) {
            data.progressUpdateImages = data.progressUpdateImages.map((img: any) => 
              this.transformAttachmentWithDataUrl(img)
            );
          }
          if (data.qualityIssueImages) {
            data.qualityIssueImages = data.qualityIssueImages.map((img: any) => 
              this.transformAttachmentWithDataUrl(img)
            );
          }
        }
        
        console.log('📎 Transformed public attachments:', data);
        return data;
      }),
      catchError(error => {
        console.error('Error fetching public box attachments:', error);
        return throwError(() => error);
      })
    );
  }

  /**
   * Transform attachment with ImageData to include imageUrl as data URL
   * This allows images to be displayed without authentication
   */
  private transformAttachmentWithDataUrl(attachment: any): any {
    if (!attachment) return attachment;

    // If imageData exists, convert it to a data URL
    if (attachment.imageData && attachment.imageData.trim() !== '') {
      const mimeType = this.getMimeTypeFromExtension(attachment.originalName || attachment.fileName);
      attachment.imageUrl = `data:${mimeType};base64,${attachment.imageData}`;
      console.log('✅ Created data URL for:', attachment.originalName, '(length:', attachment.imageUrl.length, ')');
    } else {
      console.warn('⚠️ No imageData found for:', attachment.originalName);
    }

    return attachment;
  }

  /**
   * Get MIME type from file extension
   */
  private getMimeTypeFromExtension(fileName: string): string {
    if (!fileName) return 'image/jpeg';
    
    const ext = fileName.split('.').pop()?.toLowerCase();
    const mimeTypes: Record<string, string> = {
      'jpg': 'image/jpeg',
      'jpeg': 'image/jpeg',
      'png': 'image/png',
      'gif': 'image/gif',
      'webp': 'image/webp',
      'svg': 'image/svg+xml',
      'bmp': 'image/bmp',
      'ico': 'image/x-icon'
    };
    
    return mimeTypes[ext || ''] || 'image/jpeg';
  }

  /**
   * Get public box drawings (no authentication required)
   * @param boxId The box ID
   * @returns Observable of box drawings
   */
  getPublicBoxDrawings(boxId: string): Observable<any[]> {
    return this.http.get<any>(`${this.publicApiUrl}/boxes/${boxId}/drawings`).pipe(
      map(response => {
        const data = response?.data || response;
        return Array.isArray(data) ? data : [];
      }),
      catchError(error => {
        console.error('Error fetching public box drawings:', error);
        return throwError(() => error);
      })
    );
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

