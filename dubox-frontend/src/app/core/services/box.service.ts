import { Injectable } from '@angular/core';
import { Observable, throwError } from 'rxjs';
import { map, catchError } from 'rxjs/operators';
import { ApiService, PaginatedResponse } from './api.service';
import { Box, BoxActivity, BoxAttachment, BoxImportResult, BoxLog, BoxFilters, ChecklistItem, ImportedBoxPreview, BoxTypeStatsByProject, BoxAttachmentsResponse, BoxAttachmentImage } from '../models/box.model';

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
      const validStatuses = ['NotStarted', 'InProgress', 'Completed', 'OnHold', 'Delayed', 'Dispatched', 'QAReview', 'ReadyForDelivery', 'Delivered'];
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
      5: 'Delayed',
      6: 'Dispatched'
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
      serialNumber: backendBox.serialNumber || backendBox.SerialNumber,
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
      duration: backendBox.duration ?? backendBox.Duration,
      bimModelReference: backendBox.bimModelReference || backendBox.bimModelRef,
      revitElementId: backendBox.revitElementId,
      assignedTeam: backendBox.assignedTeam,
      assignedTo: backendBox.assignedTo,
      plannedStartDate: this.parseDate(
        backendBox.plannedStartDate ??
        backendBox.plannedStartdDate ??
        backendBox.PlannedStartDate ??
        backendBox.PlannedStartdDate
      ),
      actualStartDate: this.parseDate(backendBox.actualStartDate ?? backendBox.ActualStartDate),
      plannedEndDate: this.parseDate(backendBox.plannedEndDate ?? backendBox.PlannedEndDate),
      actualEndDate: this.parseDate(backendBox.actualEndDate ?? backendBox.ActualEndDate),
      progress: backendBox.progressPercentage || backendBox.progress || 0,
      activities: backendBox.activities || [],
      attachments: backendBox.attachments || [],
      logs: backendBox.logs || [],
      notes: backendBox.notes ?? backendBox.Notes,
      qrCode: qrCode,
      currentLocationId: backendBox.currentLocationId,
      currentLocationCode: backendBox.currentLocationCode,
      currentLocationName: backendBox.currentLocationName,
      createdBy: backendBox.createdBy,
      updatedBy: backendBox.modifiedBy || backendBox.updatedBy,
      createdAt: this.parseDate(backendBox.createdDate ?? backendBox.CreatedDate),
      updatedAt: this.parseDate(backendBox.modifiedDate ?? backendBox.ModifiedDate),
      activitiesCount:backendBox.activitiesCount || backendBox.ActivitiesCount||0
    };
  }

  private transformImportResult(result: any): BoxImportResult {
    const importedBoxes = (result?.importedBoxes || result?.ImportedBoxes || []).map((box: any) => ({
      boxId: box.boxId || box.BoxId,
      projectId: box.projectId || box.ProjectId,
      projectCode: box.projectCode || box.ProjectCode,
      boxTag: box.boxTag || box.BoxTag,
      boxName: box.boxName || box.BoxName,
      boxType: box.boxType || box.BoxType,
      floor: box.floor || box.Floor,
      building: box.building || box.Building,
      zone: box.zone || box.Zone,
      status: this.mapStatus(box.status || box.Status)
    })) as ImportedBoxPreview[];

    return {
      successCount: result?.successCount ?? result?.SuccessCount ?? 0,
      failureCount: result?.failureCount ?? result?.FailureCount ?? 0,
      errors: result?.errors ?? result?.Errors ?? [],
      importedBoxes
    };
  }

  private parseDate(value?: string | Date | null): Date | undefined {
    if (!value) {
      return undefined;
    }

    if (value instanceof Date) {
      return value;
    }

    const parsed = new Date(value);
    return isNaN(parsed.getTime()) ? undefined : parsed;
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
  updateBoxStatus(id: string, status: string | number): Observable<Box> {
    // Backend expects: { boxId: Guid, status: int }
    const statusNumber = typeof status === 'string' ? parseInt(status, 10) : status;
    return this.apiService.patch<any>(`${this.endpoint}/${id}/status`, { 
      boxId: id, 
      status: statusNumber 
    }).pipe(
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
   * Update box activity status using the activities endpoint
   */
  updateBoxActivityStatus(activityId: string, status: number, workDescription?: string, issuesEncountered?: string): Observable<any> {
    return this.apiService.put<any>(
      `activities/update-status/${activityId}`,
      {
        boxActivityId: activityId,
        status: status,
        workDescription: workDescription || null,
        issuesEncountered: issuesEncountered || null
      }
    );
  }

  /**
   * Set activity schedule
   */
  setActivitySchedule(activityId: string, plannedStartDate: Date, duration: number): Observable<any> {
    return this.apiService.put<any>(
      `activities/set-activity-schedule/${activityId}`,
      {
        activityId: activityId,
        plannedStartDate: plannedStartDate.toISOString(),
        duration: duration
      }
    );
  }

  /**
   * Assign activity to team
   */
  assignActivityToTeam(activityId: string, teamId: string, teamMemberId: string): Observable<any> {
    return this.apiService.put<any>(
      'activities/Assign-team',
      {
        boxActivityId: activityId,
        teamId: teamId,
        teamMemberId: teamMemberId
      }
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
   * Get box logs with pagination and search
   */
  getBoxLogs(
    boxId: string, 
    page: number = 1, 
    pageSize: number = 25,
    searchTerm?: string,
    action?: string,
    fromDate?: string,
    toDate?: string
  ): Observable<PaginatedResponse<BoxLog>> {
    const params: any = {
      pageNumber: page,
      pageSize: pageSize
    };
    if (searchTerm) params.searchTerm = searchTerm;
    if (action) params.action = action;
    if (fromDate) params.fromDate = fromDate;
    if (toDate) params.toDate = toDate;
    
    return this.apiService.get<PaginatedResponse<BoxLog>>(`${this.endpoint}/${boxId}/logs`, params);
  }

  /**
   * Get box attachment images (file and URL types) from progress updates
   */
  getBoxAttachmentImages(boxId: string): Observable<BoxAttachmentsResponse> {
    console.log('🔍 Fetching box attachments for boxId:', boxId);
    console.log('🔗 Full URL:', `${this.endpoint}/${boxId}/attachement`);
    
    return this.apiService.get<any>(`${this.endpoint}/${boxId}/attachement`).pipe(
      map(response => {
      
        const data = response.data || response.Data || response;
        const images = data;
        
        const mappedImages = images.map((img: any) => {
          const mapped = {
            progressUpdateImageId: img.progressUpdateImageId || img.ProgressUpdateImageId || img.id || img.Id,
            progressUpdateId: img.progressUpdateId || img.ProgressUpdateId,
            imageUrl: img.imageUrl || img.ImageUrl,
            imageData: img.imageData || img.ImageData,
            imageType: (img.imageType || img.ImageType || 'file') as 'file' | 'url',
            originalName: img.originalName || img.OriginalName, // Original URL for URL-type images
            fileSize: img.fileSize || img.FileSize,
            sequence: img.sequence || img.Sequence || 0,
            createdDate: new Date(img.createdDate || img.CreatedDate),
            activityName: img.activityName || img.ActivityName,
            progressPercentage: img.progressPercentage || img.ProgressPercentage,
            updateDate: img.updateDate || img.UpdateDate ? new Date(img.updateDate || img.UpdateDate) : undefined
          };
          return mapped;
        });

        const result = {
          images: mappedImages,
          totalCount: data.totalCount || data.TotalCount || images.length
        };
        
        return result;
      }),
      catchError(err => {
        console.error('❌ Error in getBoxAttachmentImages:', err);
        console.error('❌ Error status:', err.status);
        console.error('❌ Error message:', err.message);
        console.error('❌ Error body:', err.error);
        throw err;
      })
    );
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

  /**
   * Download Excel template for bulk box import
   */
  downloadBoxesTemplate(): Observable<Blob> {
    return this.apiService.download(`${this.endpoint}/template`);
  }

  /**
   * Import boxes from Excel file
   */
  importBoxesFromExcel(projectId: string, file: File): Observable<BoxImportResult> {
    const endpoint = `${this.endpoint}/import-excel?projectId=${projectId}`;
    return this.apiService.upload<any>(endpoint, file).pipe(
      map(result => this.transformImportResult(result))
    );
  }

  /**
   * Get box type statistics by project
   */
  getBoxTypeStatsByProject(projectId: string): Observable<BoxTypeStatsByProject> {
    return this.apiService.get<BoxTypeStatsByProject>(`${this.endpoint}/project/${projectId}/box-type-stats`);
  }
}
