import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { ApiService } from './api.service';
import { WIRRecord, CreateWIRRequest, ApproveWIRRequest, RejectWIRRequest, WIR_CHECKLIST_TEMPLATES } from '../models/wir.model';

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
    return this.apiService.post<any>(`${this.endpoint}/${request.wirRecordId}/approve`, request).pipe(
      map(wir => this.transformWIR(wir))
    );
  }

  /**
   * Reject WIR record
   */
  rejectWIRRecord(request: RejectWIRRequest): Observable<WIRRecord> {
    return this.apiService.post<any>(`${this.endpoint}/${request.wirRecordId}/reject`, request).pipe(
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
}

