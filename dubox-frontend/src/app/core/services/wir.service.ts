import { Injectable } from '@angular/core';
import { Observable, forkJoin, of } from 'rxjs';
import { map, switchMap, catchError } from 'rxjs/operators';
import { ApiService } from './api.service';
import { 
  WIRRecord, 
  CreateWIRRequest, 
  ApproveWIRRequest, 
  RejectWIRRequest, 
  WIR_CHECKLIST_TEMPLATES,
  WIRCheckpoint,
  CreateWIRCheckpointRequest,
  AddChecklistItemsRequest,
  ReviewWIRCheckpointRequest
} from '../models/wir.model';

@Injectable({
  providedIn: 'root'
})
export class WIRService {
  private readonly endpoint = 'wirrecords';

  constructor(private apiService: ApiService) {}

  /**
   * Transform backend WIR response to frontend model
   */
  private transformWIR(backendWIR: any): WIRRecord {
    return {
      wirRecordId: backendWIR.wirRecordId || backendWIR.WIRRecordId,
      boxActivityId: backendWIR.boxActivityId || backendWIR.BoxActivityId,
      boxTag: backendWIR.boxTag || backendWIR.BoxTag || '',
      activityName: backendWIR.activityName || backendWIR.ActivityName || '',
      wirCode: backendWIR.wirCode || backendWIR.WIRCode || '',
      status: backendWIR.status || backendWIR.Status,
      requestedDate: new Date(backendWIR.requestedDate || backendWIR.RequestedDate),
      requestedBy: backendWIR.requestedBy || backendWIR.RequestedBy,
      requestedByName: backendWIR.requestedByName || backendWIR.RequestedByName || '',
      inspectedBy: backendWIR.inspectedBy || backendWIR.InspectedBy,
      inspectedByName: backendWIR.inspectedByName || backendWIR.InspectedByName,
      inspectionDate: backendWIR.inspectionDate ? new Date(backendWIR.inspectionDate || backendWIR.InspectionDate) : undefined,
      inspectionNotes: backendWIR.inspectionNotes || backendWIR.InspectionNotes,
      photoUrls: backendWIR.photoUrls || backendWIR.PhotoUrls,
      rejectionReason: backendWIR.rejectionReason || backendWIR.RejectionReason
    };
  }

  /**
   * Get all WIR records
   */
  getAllWIRRecords(): Observable<WIRRecord[]> {
    return this.apiService.get<any[]>(this.endpoint).pipe(
      map(wirs => wirs.map(w => this.transformWIR(w)))
    );
  }

  /**
   * Get WIR record by ID
   */
  getWIRRecord(wirRecordId: string): Observable<WIRRecord> {
    return this.apiService.get<any>(`${this.endpoint}/${wirRecordId}`).pipe(
      map(wir => this.transformWIR(wir))
    );
  }

  /**
   * Get WIR records for a specific box
   */
  getWIRRecordsByBox(boxId: string): Observable<WIRRecord[]> {
    return this.apiService.get<any[]>(`${this.endpoint}/box/${boxId}`).pipe(
      map(wirs => wirs.map(w => this.transformWIR(w)))
    );
  }

  /**
   * Get WIR records for a specific activity
   */
  getWIRRecordsByActivity(boxActivityId: string): Observable<WIRRecord[]> {
    return this.apiService.get<any[]>(`${this.endpoint}/activity/${boxActivityId}`).pipe(
      map(wirs => wirs.map(w => this.transformWIR(w)))
    );
  }

  /**
   * Create new WIR record
   */
  createWIRRecord(request: CreateWIRRequest): Observable<WIRRecord> {
    return this.apiService.post<any>(this.endpoint, request).pipe(
      map(wir => this.transformWIR(wir))
    );
  }

  /**
   * Approve WIR record
   */
  approveWIRRecord(request: ApproveWIRRequest): Observable<WIRRecord> {
    return this.apiService.put<any>(`${this.endpoint}/${request.wirRecordId}/approve`, request).pipe(
      map(wir => this.transformWIR(wir))
    );
  }

  /**
   * Reject WIR record
   */
  rejectWIRRecord(request: RejectWIRRequest): Observable<WIRRecord> {
    return this.apiService.put<any>(`${this.endpoint}/${request.wirRecordId}/reject`, request).pipe(
      map(wir => this.transformWIR(wir))
    );
  }

  /**
   * Get predefined checklist template for WIR code
   */
  getChecklistTemplate(wirCode: string) {
    return WIR_CHECKLIST_TEMPLATES[wirCode] || [];
  }

  /**
   * Upload inspection photos
   */
  uploadInspectionPhotos(wirRecordId: string, files: File[]): Observable<string[]> {
    const formData = new FormData();
    files.forEach(file => {
      formData.append('files', file);
    });
    
    return this.apiService.post<string[]>(`${this.endpoint}/${wirRecordId}/photos`, formData);
  }

  // ========== WIR Checkpoint Methods ==========

  /**
   * Get WIR checkpoint by ID
   */
  getWIRCheckpointById(wirId: string): Observable<WIRCheckpoint> {
    return this.apiService.get<any>(`wircheckpoints/${wirId}`).pipe(
      map(data => this.transformWIRCheckpoint(data))
    );
  }

  /**
   * Get WIR checkpoints by box ID
   */
  getWIRCheckpointsByBox(boxId: string): Observable<WIRCheckpoint[]> {
    return this.apiService.get<any[]>(`wircheckpoints/box/${boxId}`).pipe(
      map(checkpoints => checkpoints.map(c => this.transformWIRCheckpoint(c)))
    );
  }

  /**
   * Get WIR checkpoint by box activity ID (searches through box checkpoints by WIRNumber)
   */
  getWIRCheckpointByActivity(boxId: string, wirCode: string): Observable<WIRCheckpoint | null> {
    return this.getWIRCheckpointsByBox(boxId).pipe(
      map(checkpoints => {
        // Find checkpoint that matches the WIR code/number
        return checkpoints.find(c => 
          c.wirNumber === wirCode || 
          c.wirNumber?.toLowerCase() === wirCode?.toLowerCase() ||
          c.status === 'Pending'
        ) || null;
      }),
      catchError(() => of(null))
    );
  }

  /**
   * Create WIR checkpoint
   */
  createWIRCheckpoint(request: CreateWIRCheckpointRequest): Observable<WIRCheckpoint> {
    return this.apiService.post<any>('wircheckpoints', request).pipe(
      map(data => this.transformWIRCheckpoint(data))
    );
  }

  /**
   * Add checklist items to WIR checkpoint
   */
  addChecklistItems(request: AddChecklistItemsRequest): Observable<WIRCheckpoint> {
    return this.apiService.post<any>(`wircheckpoints/${request.wirId}/checklist-items`, request).pipe(
      map(data => this.transformWIRCheckpoint(data))
    );
  }

  /**
   * Review WIR checkpoint (approve/reject)
   */
  reviewWIRCheckpoint(request: ReviewWIRCheckpointRequest): Observable<WIRCheckpoint> {
    return this.apiService.put<any>(`wircheckpoints/${request.wirId}/review`, request).pipe(
      map(data => this.transformWIRCheckpoint(data))
    );
  }

  /**
   * Transform backend WIR checkpoint to frontend model
   */
  private transformWIRCheckpoint(backendCheckpoint: any): WIRCheckpoint {
    return {
      wirId: backendCheckpoint.wirId || backendCheckpoint.WIRId,
      boxId: backendCheckpoint.boxId || backendCheckpoint.BoxId,
      boxActivityId: backendCheckpoint.boxActivityId || backendCheckpoint.BoxActivityId,
      wirNumber: backendCheckpoint.wirNumber || backendCheckpoint.WIRNumber || '',
      wirName: backendCheckpoint.wirName || backendCheckpoint.WIRName,
      wirDescription: backendCheckpoint.wirDescription || backendCheckpoint.WIRDescription,
      requestedDate: backendCheckpoint.requestedDate ? new Date(backendCheckpoint.requestedDate) : undefined,
      requestedBy: backendCheckpoint.requestedBy || backendCheckpoint.RequestedBy,
      inspectionDate: backendCheckpoint.inspectionDate ? new Date(backendCheckpoint.inspectionDate) : undefined,
      inspectorName: backendCheckpoint.inspectorName || backendCheckpoint.InspectorName,
      inspectorRole: backendCheckpoint.inspectorRole || backendCheckpoint.InspectorRole,
      status: backendCheckpoint.status || backendCheckpoint.Status || 'Pending',
      approvalDate: backendCheckpoint.approvalDate ? new Date(backendCheckpoint.approvalDate) : undefined,
      comments: backendCheckpoint.comments || backendCheckpoint.Comments,
      attachmentPath: backendCheckpoint.attachmentPath || backendCheckpoint.AttachmentPath,
      createdDate: backendCheckpoint.createdDate ? new Date(backendCheckpoint.createdDate) : new Date(),
      checklistItems: (backendCheckpoint.checklistItems || backendCheckpoint.ChecklistItems || []).map((item: any) => ({
        checklistItemId: item.checklistItemId || item.ChecklistItemId,
        wirId: item.wirId || item.WIRId,
        checkpointDescription: item.checkpointDescription || item.CheckpointDescription || item.itemName || item.ItemName,
        referenceDocument: item.referenceDocument || item.ReferenceDocument,
        status: item.status || item.Status || 'Pending',
        remarks: item.remarks || item.Remarks || item.comments || item.Comments,
        sequence: item.sequence || item.Sequence || 0
      })),
      qualityIssues: backendCheckpoint.qualityIssues || backendCheckpoint.QualityIssues || []
    };
  }
}

