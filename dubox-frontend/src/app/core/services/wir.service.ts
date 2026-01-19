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
  UpdateChecklistItemRequest,
  PredefinedChecklistItem,
  PredefinedChecklistItemWithChecklistDto,
  ReviewWIRCheckpointRequest,
  AddQualityIssueRequest,
  CreateQualityIssueForBoxRequest,
  QualityIssueItem,
  QualityIssueDetails,
  QualityIssueImage,
  UpdateQualityIssueStatusRequest,
  WIRCheckpointFilter,
  WIRCheckpointChecklistItem,
  PaginatedQualityIssuesResponse,
  PaginatedWIRCheckpointsResponse
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
      boxName: backendWIR.boxName || backendWIR.BoxName,
      activityName: backendWIR.activityName || backendWIR.ActivityName || '',
      wirCode: backendWIR.wirCode || backendWIR.WIRCode || '',
      status: backendWIR.status || backendWIR.Status,
      checkpointStatus: backendWIR.checkpointStatus || backendWIR.CheckpointStatus,
      requestedDate: new Date(backendWIR.requestedDate || backendWIR.RequestedDate),
      requestedBy: backendWIR.requestedBy || backendWIR.RequestedBy,
      requestedByName: backendWIR.requestedByName || backendWIR.RequestedByName || '',
      inspectedBy: backendWIR.inspectedBy || backendWIR.InspectedBy,
      inspectedByName: backendWIR.inspectedByName || backendWIR.InspectedByName,
      inspectionDate: backendWIR.inspectionDate ? new Date(backendWIR.inspectionDate || backendWIR.InspectionDate) : undefined,
      inspectionNotes: backendWIR.inspectionNotes || backendWIR.InspectionNotes,
      photoUrls: backendWIR.photoUrls || backendWIR.PhotoUrls,
      rejectionReason: backendWIR.rejectionReason || backendWIR.RejectionReason,
      bay: backendWIR.bay || backendWIR.Bay,
      row: backendWIR.row || backendWIR.Row,
      position: backendWIR.position || backendWIR.Position,
      activityNames:backendWIR.activityNames,
      activityCount:backendWIR.activityCount
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

  getWIRCheckpoints(params?: WIRCheckpointFilter): Observable<PaginatedWIRCheckpointsResponse> {
    return this.apiService.get<any>(`wircheckpoints`, params).pipe(
      map(response => {
        const data = response?.data || response;
        if (data?.items) {
          // Paginated response
          console.log('getWIRCheckpoints - data:', data.items);
          console.log('getWIRCheckpoints - summary:', data.summary);
          return {
            items: data.items.map((c: any) => this.transformWIRCheckpoint(c)),
            totalCount: data.totalCount || 0,
            page: data.page || 1,
            pageSize: data.pageSize || 25,
            totalPages: data.totalPages || 0,
            summary: data.summary ? {
              totalCheckpoints: data.summary.totalCheckpoints || 0,
              pendingReviews: data.summary.pendingReviews || 0,
              approved: data.summary.approved || 0,
              rejected: data.summary.rejected || 0,
              conditionalApproval: data.summary.conditionalApproval || 0
            } : undefined
          };
        } else {
          // Fallback for non-paginated response (backward compatibility)
          const checkpoints = Array.isArray(data) ? data : [];
          return {
            items: checkpoints.map((c: any) => this.transformWIRCheckpoint(c)),
            totalCount: checkpoints.length,
            page: 1,
            pageSize: checkpoints.length || 25,
            totalPages: 1
          };
        }
      })
    );
  }

  /**
   * Get WIR checkpoint by box activity ID (searches through box checkpoints by WIRNumber)
   */
  getWIRCheckpointByActivity(boxId: string, wirCode: string): Observable<WIRCheckpoint | null> {
    return this.getWIRCheckpointsByBox(boxId).pipe(
      map(checkpoints => {
        // Find checkpoint that matches the WIR code/number exactly
        // Remove the fallback to 'Pending' status as it could match wrong checkpoint
        const matched = checkpoints.find(c => 
          c.wirNumber === wirCode || 
          c.wirNumber?.toLowerCase() === wirCode?.toLowerCase()
        );
        console.log('getWIRCheckpointByActivity - searching for WIR code:', wirCode);
        console.log('getWIRCheckpointByActivity - available checkpoints:', checkpoints.map(c => ({ wirNumber: c.wirNumber, wirId: c.wirId })));
        console.log('getWIRCheckpointByActivity - matched checkpoint:', matched);
        return matched || null;
      }),
      catchError(() => of(null))
    );
  }

  /**
   * Create WIR checkpoint
   */
  createWIRCheckpoint(request: CreateWIRCheckpointRequest): Observable<WIRCheckpoint> {
    // If files or imageUrls are provided, send as multipart/form-data
    if ((request.files && request.files.length > 0) || (request.imageUrls && request.imageUrls.length > 0)) {
      const formData = new FormData();
      formData.append('BoxActivityId', request.boxActivityId);
      formData.append('WIRNumber', request.wirNumber);
      
      if (request.wirName) {
        formData.append('WIRName', request.wirName);
      }
      if (request.wirDescription) {
        formData.append('WIRDescription', request.wirDescription);
      }
      if (request.attachmentPath) {
        formData.append('AttachmentPath', request.attachmentPath);
      }
      if (request.comments) {
        formData.append('Comments', request.comments);
      }
      
      // Append files if any
      if (request.files && request.files.length > 0) {
        request.files.forEach(file => {
          formData.append('Files', file);
        });
      }
      
      // Append image URLs if any
      if (request.imageUrls && request.imageUrls.length > 0) {
        request.imageUrls.forEach(url => {
          formData.append('ImageUrls', url);
        });
      }
      
      // Append file names if any (for version tracking)
      if (request.fileNames && request.fileNames.length > 0) {
        request.fileNames.forEach(name => {
          formData.append('FileNames', name);
        });
        console.log('📎 VERSION DEBUG - Sending FileNames to backend (WIR):', request.fileNames);
      }
      
      return this.apiService.postFormData<any>('wircheckpoints', formData).pipe(
        map(data => this.transformWIRCheckpoint(data))
      );
    } else {
      // Backward compatibility: send as JSON if no files/URLs
      return this.apiService.post<any>('wircheckpoints', request).pipe(
        map(data => this.transformWIRCheckpoint(data))
      );
    }
  }

  /**
   * Get all predefined checklist items
   * @param wirNumber Optional WIR number to filter items
   */
  getPredefinedChecklistItems(wirNumber?: string): Observable<PredefinedChecklistItem[]> {
    const url = wirNumber 
      ? `wircheckpoints/predefined-checklist-items?wirNumber=${wirNumber}`
      : 'wircheckpoints/predefined-checklist-items';
      
    return this.apiService.get<any[]>(url).pipe(
      map(response => {
        const items = Array.isArray(response) ? response : (response as any)?.data || [];
        return items.map((item: any) => ({
          predefinedItemId: item.predefinedItemId || item.PredefinedItemId,
          wirNumber: item.wirNumber || item.WIRNumber || '',
          itemNumber: item.itemNumber || item.ItemNumber,
          checkpointDescription: item.checkpointDescription || item.CheckpointDescription,
          categoryId: item.categoryId || item.CategoryId,
          category: item.category || item.Category,
          categoryName: item.categoryName || item.CategoryName,
          referenceId: item.referenceId || item.ReferenceId,
          referenceDocument: item.referenceDocument || item.ReferenceDocument,
          referenceName: item.referenceName || item.ReferenceName,
          sequence: item.sequence || item.Sequence || 0,
          isActive: item.isActive ?? item.IsActive ?? true
        }));
      })
    );
  }

  /**
   * Get predefined checklist items by WIR checkpoint ID with checklist and section grouping
   * @param checkpointId WIR checkpoint ID
   */
  getPredefinedChecklistItemsByCheckpointId(checkpointId: string): Observable<PredefinedChecklistItemWithChecklistDto[]> {
    return this.apiService.get<any>(`wircheckpoints/predefined-checklist-items/${checkpointId}`).pipe(
      map(response => {
        console.log('Raw API response:', response);
        
        // Handle different response formats
        let items = [];
        if (Array.isArray(response)) {
          items = response;
        } else if (response?.data && Array.isArray(response.data)) {
          items = response.data;
        } else if (response?.value && Array.isArray(response.value)) {
          items = response.value;
        } else {
          console.warn('Unexpected response format:', response);
          items = [];
        }
        
        console.log('Extracted items:', items);
        
        return items.map((item: any) => ({
          predefinedItemId: item.predefinedItemId || item.PredefinedItemId || item.id,
          checkpointDescription: item.checkpointDescription || item.CheckpointDescription || item.description || '',
          reference: item.reference || item.Reference,
          sequence: item.sequence || item.Sequence || 0,
          isActive: item.isActive ?? item.IsActive ?? true,
          
          // ChecklistSection information
          checklistSectionId: item.checklistSectionId || item.ChecklistSectionId,
          sectionTitle: item.sectionTitle || item.SectionTitle,
          sectionOrder: item.sectionOrder || item.SectionOrder,
          
          // Checklist information
          checklistId: item.checklistId || item.ChecklistId,
          checklistName: item.checklistName || item.ChecklistName,
          checklistCode: item.checklistCode || item.ChecklistCode,
          checklistDiscipline: item.checklistDiscipline || item.ChecklistDiscipline,
          checklistSubDiscipline: item.checklistSubDiscipline || item.ChecklistSubDiscipline,
          checklistPageNumber: item.checklistPageNumber || item.ChecklistPageNumber,
          checklistWIRCode: item.checklistWIRCode || item.ChecklistWIRCode,
          checklistReferenceDocuments: item.checklistReferenceDocuments || item.ChecklistReferenceDocuments,
          checklistSignatureRoles: item.checklistSignatureRoles || item.ChecklistSignatureRoles
        }));
      })
    );
  }

  /**
   * Add predefined checklist items to WIR checkpoint
   */
  addChecklistItems(request: AddChecklistItemsRequest): Observable<WIRCheckpoint> {
    return this.apiService.post<any>(`wircheckpoints/${request.wirId}/checklist-items`, {
      wirId: request.wirId,
      predefinedItemIds: request.predefinedItemIds
    }).pipe(
      map(data => this.transformWIRCheckpoint(data))
    );
  }

  /**
   * Update a checklist item
   */
  updateChecklistItem(request: UpdateChecklistItemRequest): Observable<WIRCheckpointChecklistItem> {
    return this.apiService.put<any>(`wircheckpoints/checklist-items/${request.checklistItemId}`, {
      checklistItemId: request.checklistItemId,
      checkpointDescription: request.checkpointDescription,
      referenceDocument: request.referenceDocument,
      status: request.status,
      remarks: request.remarks,
      sequence: request.sequence
    }).pipe(
      map((item: any) => ({
        checklistItemId: item.checklistItemId || item.ChecklistItemId,
        wirId: item.wirId || item.WIRId,
        checkpointDescription: item.checkpointDescription || item.CheckpointDescription,
        referenceDocument: item.referenceDocument || item.ReferenceDocument,
        status: item.status || item.Status || 'Pending',
        remarks: item.remarks || item.Remarks,
        sequence: item.sequence || item.Sequence || 0,
        predefinedItemId: item.predefinedItemId || item.PredefinedItemId
      }))
    );
  }

  /**
   * Delete a checklist item
   */
  deleteChecklistItem(checklistItemId: string): Observable<boolean> {
    return this.apiService.delete<boolean>(`wircheckpoints/checklist-items/${checklistItemId}`);
  }

  /**
   * Add quality issues to WIR checkpoint
   */
  /**
   * Add a single quality issue (new approach - one issue at a time)
   */
  addQualityIssue(request: AddQualityIssueRequest): Observable<WIRCheckpoint> {
    // Send as FormData if files are present, otherwise as JSON
   
      const formData = new FormData();
      formData.append('IssueType', request.issueType);
      formData.append('Severity', request.severity);
      formData.append('IssueDescription', request.issueDescription);
      
      if (request.assignedTo) {
        formData.append('AssignedTo', request.assignedTo);
      }
      
      if (request.assignedToUserId) {
        formData.append('AssignedToUserId', request.assignedToUserId);
      }
      
      if (request.ccUserId) {
        formData.append('CCUserId', request.ccUserId);
      }
      
      if (request.dueDate) {
        const dueDateStr = request.dueDate instanceof Date 
          ? request.dueDate.toISOString() 
          : request.dueDate;
        formData.append('DueDate', dueDateStr);
      }
      
      // Append image URLs if any
      if (request.imageUrls && request.imageUrls.length > 0) {
        request.imageUrls.forEach(url => {
          formData.append('ImageUrls', url);
        });
      }
      
      // Append files
      if (request.files && request.files.length > 0) {
        request.files.forEach((file: File) => {
          formData.append('Files', file);
        });
      }
      
      return this.apiService.postFormData<any>(`wircheckpoints/${request.wirId}/quality-issue`, formData).pipe(
        map(data => this.transformWIRCheckpoint(data))
      );
    
  }


  /**
   * Review WIR checkpoint (approve/reject)
   */
  reviewWIRCheckpoint(request: ReviewWIRCheckpointRequest): Observable<WIRCheckpoint> {
    // Always send as multipart/form-data to match backend signature
    const formData = new FormData();
    formData.append('Status', request.status.toString());
    
    if (request.comment) {
      formData.append('Comment', request.comment);
    }
    
    if (request.inspectorRole) {
      formData.append('InspectorRole', request.inspectorRole);
    }
    
    // Append files if any
    if (request.files && request.files.length > 0) {
      request.files.forEach(file => {
        formData.append('Files', file);
      });
    }
    
    // Append image URLs if any
    if (request.imageUrls && request.imageUrls.length > 0) {
      request.imageUrls.forEach(url => {
        formData.append('ImageUrls', url);
      });
    }
    
    // Append items as JSON string
    formData.append('ItemsJson', JSON.stringify(request.items));
    
    // Use putFormData for FormData to ensure proper Content-Type handling
    return this.apiService.putFormData<any>(`wircheckpoints/${request.wIRId}/review`, formData).pipe(
      map(data => this.transformWIRCheckpoint(data))
    );
  }

  /**
   * Get all quality issues (with optional filters and pagination)
   */
  getAllQualityIssues(params?: {
    searchTerm?: string;
    status?: string;
    severity?: string;
    issueType?: string;
    page?: number;
    pageSize?: number;
  }): Observable<PaginatedQualityIssuesResponse> {
    return this.apiService.get<any>('qualityissues', params).pipe(
      map(response => {
        const data = response?.data || response;
        if (data?.items) {
          // Paginated response
          console.log('getAllQualityIssues - data:', data.items);
          console.log('getAllQualityIssues - summary:', data.summary);
          return {
            items: data.items.map((issue: any) => this.transformQualityIssueDetails(issue)),
            totalCount: data.totalCount || 0,
            page: data.page || 1,
            pageSize: data.pageSize || 25,
            totalPages: data.totalPages || 0,
            summary: data.summary ? {
              totalIssues: data.summary.totalIssues || 0,
              openIssues: data.summary.openIssues || 0,
              inProgressIssues: data.summary.inProgressIssues || 0,
              resolvedIssues: data.summary.resolvedIssues || 0,
              closedIssues: data.summary.closedIssues || 0
            } : undefined
          };
        } else {
          // Fallback for non-paginated response (backward compatibility)
          const issues = Array.isArray(data) ? data : [];
          return {
            items: issues.map((issue: any) => this.transformQualityIssueDetails(issue)),
            totalCount: issues.length,
            page: 1,
            pageSize: issues.length || 25,
            totalPages: 1
          };
        }
      })
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

  /**
   * Get quality issues for a project
   */
  getQualityIssuesByProject(projectId: string): Observable<QualityIssueDetails[]> {
    return this.apiService.get<any>(`qualityissues/project/${projectId}`).pipe(
      map(response => {
        const issues = response?.data || response || [];
        return issues.map((issue: any) => this.transformQualityIssueDetails(issue));
      })
    );
  }

  /**
   * Get a single quality issue by id
   */
  getQualityIssueById(issueId: string): Observable<QualityIssueDetails> {
    return this.apiService.get<any>(`qualityissues/${issueId}`).pipe(
      map(response => {
        const issue = response?.data || response;
        return this.transformQualityIssueDetails(issue);
      })
    );
  }

  /**
   * Create a quality issue directly for a box (without WIR checkpoint)
   */
  createQualityIssueForBox(request: CreateQualityIssueForBoxRequest): Observable<QualityIssueDetails> {
    // Always send multipart/form-data to match backend expectations and avoid JSON parsing issues
    const formData = new FormData();
    formData.append('BoxId', request.boxId);
    formData.append('IssueType', request.issueType);
    formData.append('Severity', request.severity);
    formData.append('IssueDescription', request.issueDescription);
    
    if (request.assignedTo) {
      formData.append('AssignedTo', request.assignedTo);
    }
    
    if (request.assignedToUserId) {
      formData.append('AssignedToUserId', request.assignedToUserId);
    }
    
    if (request.ccUserId) {
      formData.append('CCUserId', request.ccUserId);
    }
    
    if (request.dueDate) {
      const dueDateStr = request.dueDate instanceof Date 
        ? request.dueDate.toISOString() 
        : request.dueDate;
      formData.append('DueDate', dueDateStr);
    }
    
    // Append image URLs if any
    if (request.imageUrls && request.imageUrls.length > 0) {
      request.imageUrls.forEach((url: string) => {
        formData.append('ImageUrls', url);
      });
    }
    
    // Append files if any
    if (request.files && request.files.length > 0) {
      request.files.forEach((file: File) => {
        formData.append('Files', file);
      });
    }
    
    // Append file names if any (for version tracking)
    if (request.fileNames && request.fileNames.length > 0) {
      request.fileNames.forEach((name: string) => {
        formData.append('FileNames', name);
      });
      console.log('📎 VERSION DEBUG - Sending FileNames to backend (Quality Issue):', request.fileNames);
    }
    
    return this.apiService.postFormData<any>('qualityissues', formData).pipe(
      map(response => {
        const issue = response?.data || response;
        return this.transformQualityIssueDetails(issue);
      })
    );
  }

  /**
   * Assign a quality issue to a team and optionally to a team member
   */
  assignQualityIssueToTeam(issueId: string, teamId: string | null, teamMemberId?: string | null): Observable<QualityIssueDetails> {
    const request = {
      issueId: issueId,
      teamId: teamId || null,
      teamMemberId: teamMemberId || null
    };
    
    return this.apiService.put<QualityIssueDetails>(`qualityissues/${issueId}/assign`, request).pipe(
      map((response: any) => {
        const data = response?.data || response;
        return this.transformQualityIssueDetails(data);
      })
    );
  }

  updateQualityIssueStatus(
    issueId: string, 
    payload: UpdateQualityIssueStatusRequest,
    files?: File[],
    imageUrls?: string[]
  ): Observable<QualityIssueDetails> {
    // Always send multipart/form-data because backend endpoint [Consumes("multipart/form-data")]
    const formData = new FormData();
    formData.append('Status', payload.status);
    
    if (payload.resolutionDescription) {
      formData.append('ResolutionDescription', payload.resolutionDescription);
    }
    
    if (files && files.length > 0) {
      files.forEach((file) => {
        formData.append('Files', file);
      });
    }
    
    if (imageUrls && imageUrls.length > 0) {
      imageUrls.forEach((url) => {
        formData.append('ImageUrls', url);
      });
    }

    return this.apiService.put<any>(`qualityissues/${issueId}/status`, formData).pipe(
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
    
    // Debug: Log raw backend response
    console.log('transformWIRCheckpoint - raw backend data:', backendCheckpoint);
    console.log('transformWIRCheckpoint - checklistItems in response:', backendCheckpoint.checklistItems || backendCheckpoint.ChecklistItems);
    console.log('transformWIRCheckpoint - checklistItems count:', (backendCheckpoint.checklistItems || backendCheckpoint.ChecklistItems || []).length);

    return {
      wirId: backendCheckpoint.wirId || backendCheckpoint.WIRId,
      boxId: backendCheckpoint.boxId || backendCheckpoint.BoxId,
      boxActivityId: backendCheckpoint.boxActivityId || backendCheckpoint.BoxActivityId,
      projectId:
        rawBox?.projectId ||
        rawBox?.ProjectId ||
        backendCheckpoint.projectId ||
        backendCheckpoint.ProjectId,
        projectName:
        rawBox?.projectName ||
        rawBox?.ProjectName ||
        backendCheckpoint.projectName ||
        backendCheckpoint.ProjectName,
      projectCode:
        rawBox?.projectCode ||
        rawBox?.ProjectCode ||
        backendCheckpoint.projectCode ||
        backendCheckpoint.ProjectCode,
      client:
        backendCheckpoint.client ||
        backendCheckpoint.Client,
      box: rawBox
        ? {
            boxId: rawBox.boxId || rawBox.BoxId || backendCheckpoint.boxId || backendCheckpoint.BoxId,
            projectId: rawBox.projectId || rawBox.ProjectId,
            projectCode: rawBox.projectCode || rawBox.ProjectCode,
            boxTag: rawBox.boxTag || rawBox.BoxTag,
            boxName: rawBox.boxName || rawBox.BoxName
          }
        : (backendCheckpoint.boxTag || backendCheckpoint.BoxTag || backendCheckpoint.boxName || backendCheckpoint.BoxName
          ? {
              boxId: backendCheckpoint.boxId || backendCheckpoint.BoxId || '',
              projectId: backendCheckpoint.projectId || backendCheckpoint.ProjectId,
              projectCode: backendCheckpoint.projectCode || backendCheckpoint.ProjectCode,
              boxTag: backendCheckpoint.boxTag || backendCheckpoint.BoxTag || '',
              boxName: backendCheckpoint.boxName || backendCheckpoint.BoxName || ''
            }
          : undefined),
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
      checklistItems: (() => {
        const rawItems = backendCheckpoint.checklistItems || backendCheckpoint.ChecklistItems || [];
        console.log('Transforming checklist items, raw count:', rawItems.length);
        const transformed = rawItems.map((item: any, index: number) => {
          const transformedItem = {
            checklistItemId: item.checklistItemId || item.ChecklistItemId,
            wirId: item.wirId || item.WIRId,
            checkpointDescription: item.checkpointDescription || item.CheckpointDescription || item.itemName || item.ItemName,
            referenceDocument: item.referenceDocument || item.ReferenceDocument,
            status: item.status || item.Status || 'Pending',
            remarks: item.remarks || item.Remarks || item.comments || item.Comments,
            sequence: item.sequence || item.Sequence || 0,
            predefinedItemId: item.predefinedItemId || item.PredefinedItemId,
            // Category information
            categoryId: item.categoryId || item.CategoryId,
            categoryName: item.categoryName || item.CategoryName,
            // Section information
            sectionId: item.sectionId || item.SectionId,
            sectionName: item.sectionName || item.SectionName,
            sectionOrder: item.sectionOrder ?? item.SectionOrder,
            // Checklist information
            checklistId: item.checklistId || item.ChecklistId,
            checklistName: item.checklistName || item.ChecklistName,
            checklistCode: item.checklistCode || item.ChecklistCode
          };
          console.log(`Transformed item ${index + 1}:`, transformedItem);
          return transformedItem;
        });
        console.log('Final transformed checklist items count:', transformed.length);
        return transformed;
      })(),
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
        
        // Transform images array if present
        const rawImages = issue.images || issue.Images || [];
        const transformedImages = Array.isArray(rawImages) ? rawImages.map((img: any) => ({
          qualityIssueImageId: img.qualityIssueImageId || img.QualityIssueImageId || '',
          issueId: img.issueId || img.IssueId || issue.issueId || issue.IssueId || '',
          imageData: img.imageData || img.ImageData || '',
          imageType: (img.imageType || img.ImageType || 'file') as 'file' | 'url',
          originalName: img.originalName || img.OriginalName,
          fileSize: img.fileSize ?? img.FileSize,
          sequence: img.sequence ?? img.Sequence ?? 0,
          createdDate: img.createdDate ? new Date(img.createdDate) : (img.CreatedDate ? new Date(img.CreatedDate) : new Date())
        })) : [];
        
        // Debug logging
        if (transformedImages.length > 0) {
          console.log(`[WIR Service] Quality Issue ${issue.issueId || issue.IssueId} has ${transformedImages.length} images`);
        }
        
        const transformed: QualityIssueItem = {
          issueId: issue.issueId || issue.IssueId || '',
          issueNumber: issue.issueNumber || issue.IssueNumber,
          issueType: issue.issueType || issue.IssueType || 'Defect',
          severity: issue.severity || issue.Severity || 'Minor',
          issueDescription: issue.issueDescription || issue.IssueDescription || '',
          assignedTo: issue.assignedTo || issue.AssignedTo,
          assignedTeam: issue.assignedTeam || issue.AssignedTeam, // Team name from backend
          assignedToUserId: issue.assignedToUserId || issue.AssignedToUserId,
          assignedToUserName: issue.assignedToUserName || issue.AssignedToUserName || issue.AssignedUserName,
          ccUserId: issue.ccUserId || issue.CCUserId,
          ccUserName: issue.ccUserName || issue.CCUserName || issue.CcUserName,
          dueDate: issue.dueDate ? new Date(issue.dueDate) : undefined,
          photoPath: issue.photoPath || issue.PhotoPath,
          reportedBy: issue.reportedBy || issue.ReportedBy,
          issueDate: issue.issueDate ? new Date(issue.issueDate) : undefined,
          status: normalizedStatus,
          images: transformedImages.length > 0 ? transformedImages : undefined
        };
        return transformed;
      }),
      images: (() => {
        const rawImages = backendCheckpoint.images || backendCheckpoint.Images || [];
        console.log('transformWIRCheckpoint - raw images:', rawImages);
        console.log('transformWIRCheckpoint - images count:', rawImages.length);
        return rawImages.map((img: any) => ({
          wirCheckpointImageId: img.wirCheckpointImageId || img.WIRCheckpointImageId || '',
          wirId: img.wirId || img.WIRId || backendCheckpoint.wirId || backendCheckpoint.WIRId,
          imageData: img.imageData || img.ImageData || '',
          imageType: img.imageType || img.ImageType || 'file',
          originalName: img.originalName || img.OriginalName,
          fileSize: img.fileSize ?? img.FileSize,
          sequence: img.sequence ?? img.Sequence ?? 0,
          createdDate: img.createdDate ? new Date(img.createdDate) : (img.CreatedDate ? new Date(img.CreatedDate) : new Date())
        }));
      })()
    };
  }

  private transformQualityIssueDetails(issue: any): QualityIssueDetails {
    // Debug: Log raw backend response for quality issue
    console.log('🔍 transformQualityIssueDetails - Raw issue data:', issue);
    console.log('🔍 IssueNumber:', issue.issueNumber, issue.IssueNumber);
    console.log('🔍 AssignedTeamName:', issue.assignedTeamName, issue.AssignedTeamName);
    console.log('🔍 AssignedToUserName:', issue.assignedToUserName, issue.AssignedToUserName);
    console.log('🔍 CCUserId:', issue.ccUserId, issue.CCUserId);
    console.log('🔍 CCUserName:', issue.ccUserName, issue.CCUserName);
    
    // Transform images array if present
    const images = issue.images || issue.Images || [];
    const transformedImages = Array.isArray(images) ? images.map((img: any) => ({
      qualityIssueImageId: img.qualityIssueImageId || img.QualityIssueImageId,
      issueId: img.issueId || img.IssueId,
      imageData: img.imageData || img.ImageData,
      imageUrl: img.imageUrl || img.ImageUrl, // backend may return url for on-demand fetch
      imageType: (img.imageType || img.ImageType || 'file') as 'file' | 'url',
      originalName: img.originalName || img.OriginalName,
      fileSize: img.fileSize || img.FileSize,
      sequence: img.sequence ?? img.Sequence ?? 0,
      createdDate: img.createdDate ? new Date(img.createdDate) : (img.CreatedDate ? new Date(img.CreatedDate) : new Date())
    })) : [];

    const transformed = {
      issueId: issue.issueId || issue.IssueId,
      issueNumber: issue.issueNumber || issue.IssueNumber,
      issueType: issue.issueType || issue.IssueType,
      severity: issue.severity || issue.Severity,
      issueDescription: issue.issueDescription || issue.IssueDescription,
      reportedBy: issue.reportedBy || issue.ReportedBy,
      assignedTo: issue.assignedTo || issue.AssignedTo,
      assignedTeamName: issue.assignedTeamName || issue.AssignedTeamName,
      assignedToUserId: issue.assignedToUserId || issue.AssignedToUserId,
      assignedToUserName: issue.assignedToUserName || issue.AssignedToUserName,
      ccUserId: issue.ccUserId || issue.CCUserId,
      ccUserName: issue.ccUserName || issue.CCUserName,
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
      overdueDays: issue.overdueDays ?? issue.OverdueDays,
      images: transformedImages,
      projectName: issue.projectName || issue.ProjectName,
      projectId: issue.projectId || issue.ProjectId,
      projectCode: issue.projectCode || issue.ProjectCode,
    };
    
    console.log('✅ transformQualityIssueDetails - Transformed issue:', transformed);
    console.log('✅ Final IssueNumber:', transformed.issueNumber);
    console.log('✅ Final AssignedTeamName:', transformed.assignedTeamName);
    console.log('✅ Final AssignedToUserName:', transformed.assignedToUserName);
    console.log('✅ Final CCUserName:', transformed.ccUserName);
    
    return transformed;
  }
}

