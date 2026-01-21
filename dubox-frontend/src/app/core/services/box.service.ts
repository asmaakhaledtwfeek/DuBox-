import { Injectable } from '@angular/core';
import { Observable, throwError, of } from 'rxjs';
import { map, catchError } from 'rxjs/operators';
import { ApiService, PaginatedResponse } from './api.service';
import { Box, BoxActivity, BoxDrawing, BoxDrawingDto, BoxImportResult, BoxLog, BoxFilters, ChecklistItem, ImportedBoxPreview, BoxTypeStatsByProject, BoxDrawingsResponse, BoxDrawingImage, BoxAllAttachmentsResponse, BoxType, BoxSubType, BoxSummary, BoxPanel } from '../models/box.model';

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
    console.log(backendBox);
    return {
      id: backendBox.boxId || backendBox.id,
      name: backendBox.boxName || backendBox.name,
      code: backendBox.boxTag || backendBox.boxCode || backendBox.code,
      serialNumber: backendBox.serialNumber || backendBox.SerialNumber,
      boxNumber: backendBox.boxNumber || backendBox.BoxNumber,
      projectId: backendBox.projectId,
      projectCode: backendBox.projectCode || backendBox.ProjectCode,
      projectStatus: backendBox.projectStatus || backendBox.ProjectStatus,
      status: mappedStatus as any,
      type: backendBox.boxType || backendBox.type || backendBox.boxTypeName || '',
      boxTypeId: backendBox.boxTypeId || backendBox.typeId || null,
      boxTypeName: backendBox.boxType || backendBox.boxTypeName || backendBox.type || '',
      boxSubTypeId: backendBox.boxSubTypeId || null,
      boxSubTypeName: backendBox.boxSubTypeName || null,
      subType: backendBox.boxSubTypeId || backendBox.boxSubTypeId,
      description: backendBox.description,
      floor: backendBox.floor,
      buildingNumber: backendBox.buildingNumber,
      boxFunction: backendBox.boxFunction,
      zone: backendBox.zone,
      length: backendBox.length,
      width: backendBox.width,
      height: backendBox.height,
      duration: backendBox.duration ?? backendBox.Duration,
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
      activitiesCount: backendBox.activitiesCount || backendBox.ActivitiesCount || 0,
      drawingsCount: backendBox.drawingsCount || backendBox.DrawingsCount || 0,
      attachments: backendBox.attachments || [],
      logs: backendBox.logs || [],
      notes: backendBox.notes ?? backendBox.Notes,
      qrCode: qrCode,
      currentLocationId: backendBox.currentLocationId,
      currentLocationCode: backendBox.currentLocationCode,
      currentLocationName: backendBox.currentLocationName,
      factoryId: backendBox.factoryId || backendBox.FactoryId,
      factoryCode: backendBox.factoryCode || backendBox.FactoryCode,
      factoryName: backendBox.factoryName || backendBox.FactoryName,
      bay: backendBox.bay || backendBox.Bay,
      row: backendBox.row || backendBox.Row,
      position: backendBox.position || backendBox.Position,
      // Delivery information fields
      wall1: backendBox.wall1,
      wall2: backendBox.wall2,
      wall3: backendBox.wall3,
      wall4: backendBox.wall4,
      slab: backendBox.slab,
      soffit: backendBox.soffit,
      podDeliver: backendBox.podDeliver,
      podName: backendBox.podName,
      podType: backendBox.podType,
      boxPanels: this.transformBoxPanels(backendBox.boxPanels || backendBox.BoxPanels || []),
      createdBy: backendBox.createdBy,
      updatedBy: backendBox.modifiedBy || backendBox.updatedBy,
      createdAt: this.parseDate(backendBox.createdDate ?? backendBox.CreatedDate),
      updatedAt: this.parseDate(backendBox.modifiedDate ?? backendBox.ModifiedDate)
    };
  }

  /**
   * Transform backend box panels response to frontend model
   */
  private transformBoxPanels(backendPanels: any[]): BoxPanel[] {
    if (!Array.isArray(backendPanels)) {
      return [];
    }
    return backendPanels.map((panel: any) => ({
      boxPanelId: panel.boxPanelId || panel.BoxPanelId,
      boxId: panel.boxId || panel.BoxId,
      projectId: panel.projectId || panel.ProjectId,
      panelTypeId: panel.panelTypeId || panel.PanelTypeId,
      // Panel Type Name and Code - map from backend TypeName and TypeCode
      typeName: panel.typeName || panel.TypeName,
      typeCode: panel.typeCode || panel.TypeCode,
      panelName: panel.panelName || panel.PanelName,
      panelStatus: panel.panelStatus || panel.PanelStatus || 1, // Default to NotStarted (1)
      
      // Barcode & QR
      barcode: panel.barcode || panel.Barcode || '',
      qrCodeUrl: panel.qrCodeUrl || panel.QRCodeUrl,
      
      // Manufacturing
      manufacturerName: panel.manufacturerName || panel.ManufacturerName,
      manufacturedDate: this.parseDate(panel.manufacturedDate || panel.ManufacturedDate),
      
      // Delivery & Tracking
      dispatchedDate: this.parseDate(panel.dispatchedDate || panel.DispatchedDate),
      estimatedArrivalDate: this.parseDate(panel.estimatedArrivalDate || panel.EstimatedArrivalDate),
      actualArrivalDate: this.parseDate(panel.actualArrivalDate || panel.ActualArrivalDate),
      deliveryNoteNumber: panel.deliveryNoteNumber || panel.DeliveryNoteNumber,
      deliveryNoteUrl: panel.deliveryNoteUrl || panel.DeliveryNoteUrl,
      
      // First Approval
      firstApprovalStatus: panel.firstApprovalStatus || panel.FirstApprovalStatus,
      firstApprovalBy: panel.firstApprovalBy || panel.FirstApprovalBy,
      firstApprovalDate: this.parseDate(panel.firstApprovalDate || panel.FirstApprovalDate),
      firstApprovalNotes: panel.firstApprovalNotes || panel.FirstApprovalNotes,
      
      // Second Approval
      secondApprovalStatus: panel.secondApprovalStatus || panel.SecondApprovalStatus,
      secondApprovalBy: panel.secondApprovalBy || panel.SecondApprovalBy,
      secondApprovalDate: this.parseDate(panel.secondApprovalDate || panel.SecondApprovalDate),
      secondApprovalNotes: panel.secondApprovalNotes || panel.SecondApprovalNotes,
      
      // Location
      currentLocationStatus: panel.currentLocationStatus || panel.CurrentLocationStatus,
      scannedAtFactory: this.parseDate(panel.scannedAtFactory || panel.ScannedAtFactory),
      installedDate: this.parseDate(panel.installedDate || panel.InstalledDate),
      
      // Physical Info
      weight: panel.weight || panel.Weight,
      dimensions: panel.dimensions || panel.Dimensions,
      notes: panel.notes || panel.Notes,
      
      createdDate: this.parseDate(panel.createdDate || panel.CreatedDate) || new Date(),
      modifiedDate: this.parseDate(panel.modifiedDate || panel.ModifiedDate)
    } as BoxPanel));
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
      buildingNumber: box.buildingNumber || box.BuildingNumber,
      boxFunction: box.boxFunction || box.BoxFunction,
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
   * Get boxes by factory ID
   * @param factoryId Factory ID
   * @param includeDispatched Whether to include dispatched boxes (default: false)
   */
  getBoxesByFactory(factoryId: string, includeDispatched: boolean = false): Observable<Box[]> {
    // Backend now always returns paginated response, so we need to handle it
    // Use a large page size to get all boxes (backend limits to 100)
    const params: any = {
      page: '1',
      pageSize: '100'
    };
    if (includeDispatched) {
      params.includeDispatched = 'true';
    }
    return this.apiService.get<any>(`${this.endpoint}/factory/${factoryId}`, params).pipe(
      map(response => {
        // Handle paginated response - extract items array
        const items = response.items || response.Items || [];
        return items.map((b: any) => this.transformBox(b));
      })
    );
  }

  /**
   * Get boxes by factory with pagination
   * @param factoryId Factory ID
   * @param page Page number (default: 1)
   * @param pageSize Page size (default: 50)
   * @param includeDispatched Whether to include dispatched boxes (default: false)
   */
  getBoxesByFactoryPaginated(
    factoryId: string, 
    page: number = 1, 
    pageSize: number = 50, 
    includeDispatched: boolean = false
  ): Observable<PaginatedResponse<Box>> {
    const params: any = {
      page: page.toString(),
      pageSize: pageSize.toString()
    };
    if (includeDispatched) {
      params.includeDispatched = 'true';
    }
    return this.apiService.get<any>(`${this.endpoint}/factory/${factoryId}`, params).pipe(
      map(response => {
        // API service already extracts data from Result wrapper
        // Handle both camelCase and PascalCase properties from backend DTO
        const items = (response.items || response.Items || []).map((b: any) => this.transformBox(b));
        const totalCount = response.totalCount ?? response.TotalCount ?? 0;
        // Backend uses 'Page' and 'PageSize', frontend expects 'pageNumber' and 'pageSize'
        const pageNumber = response.pageNumber ?? response.PageNumber ?? response.page ?? response.Page ?? page;
        const responsePageSize = response.pageSize ?? response.PageSize ?? pageSize;
        const totalPages = response.totalPages ?? response.TotalPages ?? 0;

        return {
          items,
          totalCount,
          pageNumber,
          pageSize: responsePageSize,
          totalPages
        };
      })
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
   * Duplicate box (creates a copy with new serial number)
   */
  duplicateBox(id: string, includeActivities: boolean = true, includeDrawings: boolean = false): Observable<Box> {
    return this.apiService.post<any>(`${this.endpoint}/${id}/duplicate`, { 
      includeActivities,
      includeDrawings
    }).pipe(
      map(response => this.transformBox(response))
    );
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
   * Update box delivery information (concrete panels and pod delivery)
   */
  updateBoxDeliveryInfo(id: string, deliveryInfo: {
    wall1?: boolean;
    wall2?: boolean;
    wall3?: boolean;
    wall4?: boolean;
    slab?: boolean;
    soffit?: boolean;
    podDeliver?: boolean;
    podName?: string;
    podType?: string;
  }): Observable<Box> {
    return this.apiService.patch<any>(`${this.endpoint}/${id}/delivery-info`, {
      boxId: id,
      ...deliveryInfo
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
   * Assign activity to team and optionally to a team member
   */
  assignActivityToTeam(activityId: string, teamId: string, assignedMemberId?: string | null): Observable<any> {
    return this.apiService.put<any>(
      'activities/Assign-team',
      {
        boxActivityId: activityId,
        teamId: teamId,
        assignedMemberId: assignedMemberId
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
   * Get box drawings
   */
  getBoxDrawings(boxId: string): Observable<BoxDrawing[]> {
    return this.apiService.get<BoxDrawing[]>(`${this.endpoint}/${boxId}/attachments`);
  }

  /**
   * Get box drawings from BoxDrawings endpoint (PDF/DWG files and URLs)
   */
  getBoxDrawingsFromEndpoint(boxId: string): Observable<BoxDrawingDto[]> {
    return this.apiService.get<BoxDrawingDto[]>(`boxdrawings/box/${boxId}`).pipe(
      map(drawings => {
        // Ensure we have an array
        const drawingsArray = Array.isArray(drawings) ? drawings : [];
        // Transform backend DTO to match frontend expectations (handle both camelCase and PascalCase)
        return drawingsArray.map((dto: any) => ({
          boxDrawingId: dto.boxDrawingId || dto.BoxDrawingId,
          boxId: dto.boxId || dto.BoxId,
          drawingUrl: dto.drawingUrl || dto.DrawingUrl,
          drawingFileName: dto.drawingFileName || dto.DrawingFileName, // New field for blob storage file name
          downloadUrl: dto.downloadUrl || dto.DownloadUrl, // New field for blob storage download URL
          fileData: dto.fileData || dto.FileData, // Deprecated, kept for backward compatibility
          originalFileName: dto.originalFileName || dto.OriginalFileName,
          fileExtension: dto.fileExtension || dto.FileExtension,
          fileType: (dto.fileType || dto.FileType || 'file') as 'file' | 'url',
          fileSize: dto.fileSize || dto.FileSize,
          version: dto.version || dto.Version || 1,
          createdDate: dto.createdDate || dto.CreatedDate ? new Date(dto.createdDate || dto.CreatedDate) : new Date(),
          createdBy: dto.createdBy || dto.CreatedBy,
          createdByName: dto.createdByName || dto.CreatedByName
        } as BoxDrawingDto));
      }),
      catchError(err => {
        console.error('❌ Error fetching box drawings:', err);
        return throwError(() => err);
      })
    );
  }

  /**
   * Get download URL for a box drawing file
   */
  getBoxDrawingDownloadUrl(drawingId: string): string {
    return `${(this.apiService as any).baseUrl}/boxdrawings/${drawingId}/file`;
  }

  /**
   * Upload box drawing
   */
  uploadDrawing(boxId: string, file: File, description?: string): Observable<BoxDrawing> {
    return this.apiService.upload<BoxDrawing>(
      `${this.endpoint}/${boxId}/attachments`,
      file,
      { description }
    );
  }

  /**
   * Upload box drawing (PDF or DWG file) or URL
   */
  uploadBoxDrawing(request: { boxId: string; drawingUrl?: string; file?: File }): Observable<any> {
    const formData = new FormData();
    formData.append('BoxId', request.boxId);
    
    if (request.drawingUrl) {
      formData.append('DrawingUrl', request.drawingUrl);
    }
    
    if (request.file) {
      formData.append('File', request.file);
      console.log('📤 Uploading drawing with filename:', request.file.name);
    }
    
    return this.apiService.postFormData<any>('boxdrawings', formData);
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
    toDate?: string,
    changedBy?: string
  ): Observable<PaginatedResponse<BoxLog>> {
    const params: any = {
      pageNumber: page,
      pageSize: pageSize
    };
    if (searchTerm) params.searchTerm = searchTerm;
    if (action) params.action = action;
    if (fromDate) params.fromDate = fromDate;
    if (toDate) params.toDate = toDate;
    if (changedBy) params.changedBy = changedBy;
    
    return this.apiService.get<PaginatedResponse<BoxLog>>(`${this.endpoint}/${boxId}/logs`, params);
  }

  /**
   * Get box drawing images (file and URL types) from progress updates
   */
  getBoxDrawingImages(boxId: string): Observable<BoxDrawingsResponse> {
    console.log('🔍 Fetching box drawings for boxId:', boxId);
    console.log('🔗 Full URL:', `${this.endpoint}/${boxId}/drawing`);
    
    return this.apiService.get<any>(`${this.endpoint}/${boxId}/drawing`).pipe(
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
        console.error('❌ Error in getBoxDrawingImages:', err);
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
   * Returns an object with blob and filename
   */
  downloadBoxesTemplate(projectId: string): Observable<{ blob: Blob; filename: string }> {
    return this.apiService.downloadWithResponse(`${this.endpoint}/template?projectId=${projectId}`).pipe(
      map((response: any) => {
        const blob = response.body;
        let filename = 'BoxesImportTemplate.xlsx'; // default filename
        
        // Try to extract filename from Content-Disposition header
        const contentDisposition = response.headers.get('Content-Disposition');
        if (contentDisposition) {
          console.log('Content-Disposition header:', contentDisposition);
          
          // Try to match filename*=UTF-8''filename or filename="filename" or filename=filename
          let filenameMatch = contentDisposition.match(/filename\*=(?:UTF-8'')?([^;\n]*)/i);
          if (filenameMatch && filenameMatch[1]) {
            filename = decodeURIComponent(filenameMatch[1]);
          } else {
            filenameMatch = contentDisposition.match(/filename=["']?([^;"'\n]+)["']?/i);
            if (filenameMatch && filenameMatch[1]) {
              filename = filenameMatch[1].replace(/['"]/g, '').trim();
            }
          }
          
          console.log('Extracted filename:', filename);
        } else {
          console.warn('No Content-Disposition header found in response');
        }
        
        return { blob, filename };
      })
    );
  }

  /**
   * Import boxes from Excel file
   */
  importBoxesFromExcel(projectId: string, file: File): Observable<BoxImportResult> {
    // Try both query parameter and FormData for projectId to ensure compatibility
    const endpoint = `${this.endpoint}/import-excel?projectId=${projectId}`;
    
    console.log('📤 Importing boxes from Excel:', {
      endpoint,
      fileName: file.name,
      fileSize: file.size,
      fileType: file.type,
      projectId
    });
    
    // Use FormData like other file uploads in the application
    const formData = new FormData();
    formData.append('file', file);
    // Also include projectId in FormData as backup (backend can read from query string)
    formData.append('projectId', projectId);
    
    // Log FormData contents for debugging
    console.log('📦 FormData contents:', {
      hasFile: formData.has('file'),
      hasProjectId: formData.has('projectId'),
      fileKey: 'file',
      projectIdKey: 'projectId'
    });
    
    return this.apiService.postFormData<any>(endpoint, formData).pipe(
      map(result => {
        console.log('✅ Excel import result received:', result);
        return this.transformImportResult(result);
      })
    );
  }

  /**
   * Get box type statistics by project
   */
  getBoxTypeStatsByProject(projectId: string): Observable<BoxTypeStatsByProject> {
    return this.apiService.get<BoxTypeStatsByProject>(`${this.endpoint}/project/${projectId}/box-type-stats`);
  }

  /**
   * Get all box attachments (WIR, Progress Update, Quality Issue images)
   */
  getAllBoxAttachments(boxId: string): Observable<BoxAllAttachmentsResponse> {
    console.log('🔍 Fetching all box attachments for boxId:', boxId);
    return this.apiService.get<any>(`${this.endpoint}/${boxId}/attachments`).pipe(
      map(response => {
        console.log('✅ Raw API response:', response);
        
        const data = response?.data || response;
        
        return {
          wirCheckpointImages: data.wirCheckpointImages || [],
          progressUpdateImages: data.progressUpdateImages || [],
          qualityIssueImages: data.qualityIssueImages || [],
          totalCount: data.totalCount || 0
        };
      }),
      catchError(err => {
        console.error('❌ Error fetching box attachments:', err);
        throw err;
      })
    );
  }

  /**
   * Get box summary (activities, WIR checkpoints, quality issues, attachments, drawings)
   */
  getBoxSummary(boxId: string): Observable<BoxSummary> {
    return this.apiService.get<any>(`${this.endpoint}/${boxId}/summary`).pipe(
      map(response => {
        const data = response?.data || response;
        return data as BoxSummary;
      }),
      catchError(err => {
        console.error('❌ Error fetching box summary:', err);
        return throwError(() => err);
      })
    );
  }

  /**
   * Get all project type categories
   */
  getAllProjectTypeCategories(): Observable<any[]> {
    return this.apiService.get<any>('boxtypes/categories').pipe(
      map(response => response?.data || response || []),
      catchError(err => {
        console.error('❌ Error fetching categories:', err);
        return throwError(() => err);
      })
    );
  }

  /**
   * Get box types by category ID
   */
  getBoxTypesByCategory(categoryId: number): Observable<BoxType[]> {
    return this.apiService.get<any>(`boxtypes/by-category/${categoryId}`).pipe(
      map(response => response?.data || response || []),
      catchError(err => {
        console.error('❌ Error fetching box types:', err);
        return throwError(() => err);
      })
    );
  }

  /**
   * Get box subtypes by box type ID
   */
  getBoxSubTypesByBoxType(boxTypeId: number): Observable<BoxSubType[]> {
    return this.apiService.get<any>(`boxtypes/${boxTypeId}/subtypes`).pipe(
      map(response => response?.data || response || []),
      catchError(err => {
        console.error('❌ Error fetching box subtypes:', err);
        return throwError(() => err);
      })
    );
  }

  /**
   * Get used box letters for filtering
   * Returns array of letters (A-F) that are already used for the given combination
   */
  getUsedBoxLetters(params: { projectId: string; buildingNumber: string; floor: string; boxTypeId: number }): Observable<string[]> {
    return this.apiService.get<any>(`${this.endpoint}/used-box-letters`, params).pipe(
      map(response => {
        const data = response?.data || response;
        // Ensure we return an array of strings
        return Array.isArray(data) ? data : [];
      }),
      catchError(err => {
        console.error('❌ Error fetching used box letters (API may not exist yet):', err);
        // Return empty array on error to show all letters as available
        return of([]);
      })
    );
  }

  /**
   * Get box zones (now returns zone codes from ProjectZone configurations)
   */
  getBoxZones(): Observable<any[]> {
    return this.apiService.get<any>(`${this.endpoint}/zones`).pipe(
      map(response => {
        const data = response?.data || response;
        return Array.isArray(data) ? data : [];
      }),
      catchError(error => {
        console.error('Error fetching box zones:', error);
        return of([]);
      })
    );
  }

  /**
   * Update box panel status
   */
  updateBoxPanelStatus(boxPanelId: string, panelStatus: number): Observable<any> {
    return this.apiService.put<any>(`${this.endpoint}/panels/${boxPanelId}/status`, {
      boxPanelId: boxPanelId,
      panelStatus: panelStatus
    });
  }
}
