import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ApiService, PaginatedResponse } from './api.service';
import { Box, BoxActivity, BoxAttachment, BoxLog, BoxFilters, ChecklistItem } from '../models/box.model';

@Injectable({
  providedIn: 'root'
})
export class BoxService {
  private readonly endpoint = 'boxes';

  constructor(private apiService: ApiService) {}

  /**
   * Get all boxes
   */
  getBoxes(filters?: BoxFilters): Observable<Box[]> {
    return this.apiService.get<Box[]>(this.endpoint, filters);
  }

  /**
   * Get boxes by project
   */
  getBoxesByProject(projectId: string, filters?: BoxFilters): Observable<Box[]> {
    return this.apiService.get<Box[]>(`projects/${projectId}/boxes`, filters);
  }

  /**
   * Get box by ID
   */
  getBox(id: string): Observable<Box> {
    return this.apiService.get<Box>(`${this.endpoint}/${id}`);
  }

  /**
   * Create new box
   */
  createBox(box: Partial<Box>): Observable<Box> {
    return this.apiService.post<Box>(this.endpoint, box);
  }

  /**
   * Update box
   */
  updateBox(id: string, box: Partial<Box>): Observable<Box> {
    return this.apiService.put<Box>(`${this.endpoint}/${id}`, box);
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
    return this.apiService.patch<Box>(`${this.endpoint}/${id}/status`, { status });
  }

  /**
   * Get box activities
   */
  getBoxActivities(boxId: string): Observable<BoxActivity[]> {
    return this.apiService.get<BoxActivity[]>(`${this.endpoint}/${boxId}/activities`);
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
   * Generate QR code for box
   */
  generateQRCode(boxId: string): Observable<{ qrCode: string }> {
    return this.apiService.post<{ qrCode: string }>(`${this.endpoint}/${boxId}/qr-code`, {});
  }

  /**
   * Download QR code
   */
  downloadQRCode(boxId: string): Observable<Blob> {
    return this.apiService.download(`${this.endpoint}/${boxId}/qr-code/download`);
  }
}
