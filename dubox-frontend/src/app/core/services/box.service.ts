import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { ApiService, PaginatedResponse } from './api.service';
import { Box, BoxActivity, BoxAttachment, BoxLog, BoxFilters, ChecklistItem } from '../models/box.model';

@Injectable({
  providedIn: 'root'
})
export class BoxService {
  private readonly endpoint = 'boxes';

  constructor(private apiService: ApiService) {}

  /**
   * Map backend numeric status to frontend string status
   */
  private mapStatus(status: any): string {
    // If already a string, check if it's valid
    if (typeof status === 'string') {
      const validStatuses = ['NotStarted', 'InProgress', 'Completed', 'OnHold', 'Delayed', 'QAReview', 'ReadyForDelivery', 'Delivered'];
      if (validStatuses.includes(status)) {
        return status;
      }
    }

    // Map numeric status to string
    const statusMap: Record<number, string> = {
      1: 'NotStarted',
      2: 'InProgress',
      3: 'Completed',
      4: 'OnHold',
      5: 'Delayed'
    };

    const numericStatus = typeof status === 'number' ? status : parseInt(status, 10);
    return statusMap[numericStatus] || 'NotStarted';
  }

  /**
   * Transform backend box response to frontend model
   */
  private transformBox(backendBox: any): Box {
    console.log('🔄 Transforming box:', backendBox.boxId || backendBox.id);
    console.log('🔍 QR Code fields:', { 
      qrCodeImage: backendBox.qrCodeImage ? 'EXISTS' : 'MISSING',
      qrCode: backendBox.qrCode ? 'EXISTS' : 'MISSING',
      qrCodeString: backendBox.qrCodeString
    });
    
    // Get QR code and ensure it has the data URL prefix
    let qrCode = backendBox.qrCodeImage || backendBox.qrCode;
    if (qrCode && !qrCode.startsWith('data:')) {
      qrCode = `data:image/png;base64,${qrCode}`;
      console.log('✅ Added data URL prefix to QR code');
    }

    // Map status from backend format to frontend format
    const rawStatus = backendBox.status || backendBox.boxStatus || backendBox.Status;
    const mappedStatus = this.mapStatus(rawStatus);
    
    return {
      id: backendBox.boxId || backendBox.id,
      name: backendBox.boxName || backendBox.name,
      code: backendBox.boxTag || backendBox.boxCode || backendBox.code,
      projectId: backendBox.projectId,
      status: mappedStatus as any,
      type: backendBox.boxType || backendBox.type,
      description: backendBox.description,
      floor: backendBox.floor,
      building: backendBox.building,
      zone: backendBox.zone,
      length: backendBox.length,
      width: backendBox.width,
      height: backendBox.height,
      bimModelReference: backendBox.bimModelReference || backendBox.bimModelRef,
      revitElementId: backendBox.revitElementId,
      assignedTeam: backendBox.assignedTeam,
      assignedTo: backendBox.assignedTo,
      plannedStartDate: backendBox.plannedStartDate ? new Date(backendBox.plannedStartDate) : undefined,
      actualStartDate: backendBox.actualStartDate ? new Date(backendBox.actualStartDate) : undefined,
      plannedEndDate: backendBox.plannedEndDate ? new Date(backendBox.plannedEndDate) : undefined,
      actualEndDate: backendBox.actualEndDate ? new Date(backendBox.actualEndDate) : undefined,
      progress: backendBox.progressPercentage || backendBox.progress || 0,
      activities: backendBox.activities || [],
      attachments: backendBox.attachments || [],
      logs: backendBox.logs || [],
      qrCode: qrCode,
      createdBy: backendBox.createdBy,
      updatedBy: backendBox.modifiedBy || backendBox.updatedBy,
      createdAt: backendBox.createdDate ? new Date(backendBox.createdDate) : undefined,
      updatedAt: backendBox.modifiedDate ? new Date(backendBox.modifiedDate) : undefined,
      activitiesCount:backendBox.activitiesCount || backendBox.ActivitiesCount||0
    };
  }

  /**
   * Get all boxes
   */
  getBoxes(filters?: BoxFilters): Observable<Box[]> {
    return this.apiService.get<any[]>(this.endpoint, filters).pipe(
      map(boxes => boxes.map(b => this.transformBox(b)))
    );
  }

  /**
   * Get boxes by project
   */
  getBoxesByProject(projectId: string, filters?: BoxFilters): Observable<Box[]> {
    return this.apiService.get<any[]>(`${this.endpoint}/project/${projectId}`, filters).pipe(
      map(boxes => boxes.map(b => this.transformBox(b)))
    );
  }

  /**
   * Get box by ID
   */
  getBox(id: string): Observable<Box> {
    return this.apiService.get<any>(`${this.endpoint}/${id}`).pipe(
      map(box => this.transformBox(box))
    );
  }

  /**
   * Create new box
   */
  createBox(box: Partial<Box>): Observable<Box> {
    return this.apiService.post<any>(this.endpoint, box).pipe(
      map(response => this.transformBox(response))
    );
  }

  /**
   * Update box
   */
  updateBox(id: string, box: Partial<Box>): Observable<Box> {
    return this.apiService.put<any>(`${this.endpoint}/${id}`, box).pipe(
      map(response => this.transformBox(response))
    );
  }

  /**
   * Delete box
   */
  deleteBox(id: string): Observable<void> {
    return this.apiService.delete<void>(`${this.endpoint}/${id}`);
  }

  /**
   * Update box status
   */
  updateBoxStatus(id: string, status: string): Observable<Box> {
    return this.apiService.patch<any>(`${this.endpoint}/${id}/status`, { status }).pipe(
      map(response => this.transformBox(response))
    );
  }

  /**
   * Transform backend activity response to frontend model
   */
  private transformActivity(backendActivity: any): BoxActivity {
    return {
      id: backendActivity.boxActivityId || backendActivity.id,
      boxId: backendActivity.boxId,
      name: backendActivity.activityName || backendActivity.name,
      description: backendActivity.workDescription || backendActivity.description,
      status: backendActivity.status as any,  // Backend uses string status
      sequence: backendActivity.sequence,
      assignedTo: backendActivity.assignedTeam,
      plannedDuration: this.calculateDuration(backendActivity.plannedStartDate, backendActivity.plannedEndDate),
      actualDuration: this.calculateDuration(backendActivity.actualStartDate, backendActivity.actualEndDate),
      weightPercentage: backendActivity.progressPercentage || 0,
      plannedStartDate: backendActivity.plannedStartDate ? new Date(backendActivity.plannedStartDate) : undefined,
      actualStartDate: backendActivity.actualStartDate ? new Date(backendActivity.actualStartDate) : undefined,
      plannedEndDate: backendActivity.plannedEndDate ? new Date(backendActivity.plannedEndDate) : undefined,
      actualEndDate: backendActivity.actualEndDate ? new Date(backendActivity.actualEndDate) : undefined
    };
  }

  /**
   * Calculate duration in days between two dates
   */
  private calculateDuration(startDate?: string, endDate?: string): number {
    if (!startDate || !endDate) return 0;
    const start = new Date(startDate);
    const end = new Date(endDate);
    const diff = end.getTime() - start.getTime();
    return Math.ceil(diff / (1000 * 60 * 60 * 24));
  }

  /**
   * Get box activities
   */
  getBoxActivities(boxId: string): Observable<BoxActivity[]> {
    return this.apiService.get<any[]>(`activities/box/${boxId}`).pipe(
      map(activities => activities.map(a => this.transformActivity(a)))
    );
  }

  /**
   * Get activity details by ID
   */
  getActivityDetails(activityId: string): Observable<BoxActivity> {
    return this.apiService.get<any>(`activities/${activityId}`).pipe(
      map(activity => this.transformActivity(activity))
    );
  }

  /**
   * Update activity status
   */
  updateActivityStatus(boxId: string, activityId: string, status: string): Observable<BoxActivity> {
    return this.apiService.patch<BoxActivity>(
      `${this.endpoint}/${boxId}/activities/${activityId}/status`,
      { status }
    );
  }

  /**
   * Get activity checklist
   */
  getActivityChecklist(boxId: string, activityId: string): Observable<ChecklistItem[]> {
    return this.apiService.get<ChecklistItem[]>(
      `${this.endpoint}/${boxId}/activities/${activityId}/checklist`
    );
  }

  /**
   * Update checklist item
   */
  updateChecklistItem(
    boxId: string,
    activityId: string,
    itemId: string,
    data: Partial<ChecklistItem>
  ): Observable<ChecklistItem> {
    return this.apiService.patch<ChecklistItem>(
      `${this.endpoint}/${boxId}/activities/${activityId}/checklist/${itemId}`,
      data
    );
  }

  /**
   * Get box attachments
   */
  getBoxAttachments(boxId: string): Observable<BoxAttachment[]> {
    return this.apiService.get<BoxAttachment[]>(`${this.endpoint}/${boxId}/attachments`);
  }

  /**
   * Upload box attachment
   */
  uploadAttachment(boxId: string, file: File, description?: string): Observable<BoxAttachment> {
    return this.apiService.upload<BoxAttachment>(
      `${this.endpoint}/${boxId}/attachments`,
      file,
      { description }
    );
  }

  /**
   * Delete attachment
   */
  deleteAttachment(boxId: string, attachmentId: string): Observable<void> {
    return this.apiService.delete<void>(`${this.endpoint}/${boxId}/attachments/${attachmentId}`);
  }

  /**
   * Get box logs
   */
  getBoxLogs(boxId: string): Observable<BoxLog[]> {
    return this.apiService.get<BoxLog[]>(`${this.endpoint}/${boxId}/logs`);
  }

  /**
   * Generate QR code for box (returns base64 string)
   */
  generateQRCode(boxId: string): Observable<string> {
    return this.apiService.get<string>(`${this.endpoint}/generate-qrcode/${boxId}`);
  }

  /**
   * Download QR code
   */
  downloadQRCode(boxId: string): Observable<Blob> {
    return this.apiService.download(`${this.endpoint}/${boxId}/qr-code/download`);
  }
}
