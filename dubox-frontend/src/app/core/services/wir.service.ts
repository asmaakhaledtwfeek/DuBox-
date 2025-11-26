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
  ReviewWIRCheckpointRequest,
  AddQualityIssuesRequest,
  QualityIssueItem,
  QualityIssueDetails,
  UpdateQualityIssueStatusRequest,
  WIRCheckpointFilter
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

  getWIRCheckpoints(params?: WIRCheckpointFilter): Observable<WIRCheckpoint[]> {
    return this.apiService.get<any>(`wircheckpoints`, params).pipe(
      map(response => {
        const checkpoints = response || [];
        return (checkpoints as any[]).map(c => this.transformWIRCheckpoint(c));
      })
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
   * Add quality issues to WIR checkpoint
   */
  addQualityIssues(request: AddQualityIssuesRequest): Observable<WIRCheckpoint> {
    return this.apiService.post<any>(`wircheckpoints/${request.wirId}/quality-issues`, request).pipe(
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
   * Get quality issues for a box
   */
  getQualityIssuesByBox(boxId: string): Observable<QualityIssueDetails[]> {
    return this.apiService.get<any>(`qualityissues/box/${boxId}`).pipe(
      map(response => {
        const issues = response?.data || response || [];
        return issues.map((issue: any) => this.transformQualityIssueDetails(issue));
      })
    );
  }

  updateQualityIssueStatus(issueId: string, payload: UpdateQualityIssueStatusRequest): Observable<QualityIssueDetails> {
    return this.apiService.put<any>(`qualityissues/${issueId}/status`, payload).pipe(
      map(response => {
        const issue = response?.data || response;
        return this.transformQualityIssueDetails(issue);
      })
    );
  }

  /**
   * Transform backend WIR checkpoint to frontend model
   */
  private transformWIRCheckpoint(backendCheckpoint: any): WIRCheckpoint {
    const rawBox = backendCheckpoint.box || backendCheckpoint.Box;

    return {
      wirId: backendCheckpoint.wirId || backendCheckpoint.WIRId,
      boxId: backendCheckpoint.boxId || backendCheckpoint.BoxId,
      boxActivityId: backendCheckpoint.boxActivityId || backendCheckpoint.BoxActivityId,
      projectId:
        rawBox?.projectId ||
        rawBox?.ProjectId ||
        backendCheckpoint.projectId ||
        backendCheckpoint.ProjectId,
      projectCode:
        rawBox?.projectCode ||
        rawBox?.ProjectCode ||
        backendCheckpoint.projectCode ||
        backendCheckpoint.ProjectCode,
      box: rawBox
        ? {
            boxId: rawBox.boxId || rawBox.BoxId || backendCheckpoint.boxId || backendCheckpoint.BoxId,
            projectId: rawBox.projectId || rawBox.ProjectId,
            projectCode: rawBox.projectCode || rawBox.ProjectCode,
            boxTag: rawBox.boxTag || rawBox.BoxTag,
            boxName: rawBox.boxName || rawBox.BoxName
          }
        : undefined,
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
      qualityIssues: (backendCheckpoint.qualityIssues || backendCheckpoint.QualityIssues || []).map((issue: any) => {
        // Normalize status: backend might return enum name or number, normalize to frontend type
        let normalizedStatus = issue.status || issue.Status || 'Open';
        if (typeof normalizedStatus === 'number') {
          const statusMap: Record<number, string> = { 1: 'Open', 2: 'InProgress', 3: 'Resolved', 4: 'Closed' };
          normalizedStatus = statusMap[normalizedStatus] || 'Open';
        }
        // Ensure status matches frontend type
        const validStatuses = ['Open', 'InProgress', 'Resolved', 'Closed'];
        if (!validStatuses.includes(normalizedStatus)) {
          normalizedStatus = 'Open';
        }
        
        const transformed: QualityIssueItem & { issueId?: string } = {
          issueId: issue.issueId || issue.IssueId || '',
          issueType: issue.issueType || issue.IssueType || 'Defect',
          severity: issue.severity || issue.Severity || 'Minor',
          issueDescription: issue.issueDescription || issue.IssueDescription || '',
          assignedTo: issue.assignedTo || issue.AssignedTo,
          dueDate: issue.dueDate ? new Date(issue.dueDate) : undefined,
          photoPath: issue.photoPath || issue.PhotoPath,
          reportedBy: issue.reportedBy || issue.ReportedBy,
          issueDate: issue.issueDate ? new Date(issue.issueDate) : undefined,
          status: normalizedStatus
        };
        return transformed;
      })
    };
  }

  private transformQualityIssueDetails(issue: any): QualityIssueDetails {
    return {
      issueId: issue.issueId || issue.IssueId,
      issueType: issue.issueType || issue.IssueType,
      severity: issue.severity || issue.Severity,
      issueDescription: issue.issueDescription || issue.IssueDescription,
      reportedBy: issue.reportedBy || issue.ReportedBy,
      assignedTo: issue.assignedTo || issue.AssignedTo,
      dueDate: issue.dueDate ? new Date(issue.dueDate) : undefined,
      photoPath: issue.photoPath || issue.PhotoPath,
      issueDate: issue.issueDate ? new Date(issue.issueDate) : undefined,
      status: issue.status || issue.Status,
      resolutionDate: issue.resolutionDate ? new Date(issue.resolutionDate) : undefined,
      resolutionDescription: issue.resolutionDescription || issue.ResolutionDescription,
      boxId: issue.boxId || issue.BoxId,
      boxName: issue.boxName || issue.BoxName,
      boxTag: issue.boxTag || issue.BoxTag,
      wirId: issue.wirId || issue.WIRId,
      wirNumber: issue.wirNumber || issue.WIRNumber,
      wirName: issue.wirName || issue.WIRName,
      wirStatus: issue.wirStatus || issue.WIRStatus,
      wirRequestedDate: issue.wirRequestedDate ? new Date(issue.wirRequestedDate) : undefined,
      inspectorName: issue.inspectorName || issue.InspectorName,
      isOverdue: issue.isOverdue ?? issue.IsOverdue,
      overdueDays: issue.overdueDays ?? issue.OverdueDays
    };
  }
}

