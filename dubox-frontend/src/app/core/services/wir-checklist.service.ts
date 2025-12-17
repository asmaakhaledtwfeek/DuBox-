import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';

export interface WIRWithChecklist {
  wirId: string;
  wirNumber: string;
  wirName: string;
  wirDescription?: string;
  status: string;
  requestedDate?: string;
  inspectionDate?: string;
  inspectorName?: string;
  inspectorRole?: string;
  comments?: string;
  sections: ChecklistSection[];
  totalItems: number;
  completedItems: number;
  progressPercentage: number;
}

export interface ChecklistSection {
  sectionLetter: string;
  sectionName: string;
  items: ChecklistItemDetail[];
}

export interface ChecklistItemDetail {
  checklistItemId: string;
  itemNumber: string;
  description: string;
  referenceDocument?: string;
  status: 'Pending' | 'Pass' | 'Fail';
  remarks?: string;
  sequence: number;
}

export interface GenerateWIRsResponse {
  isSuccess: boolean;
  data: any[];
  message: string;
}

export interface UpdateChecklistItemRequest {
  checklistItemId: string;
  status: 'Pending' | 'Pass' | 'Fail';
  remarks?: string;
}

export interface ReviewWIRRequest {
  status: 'Pending' | 'Approved' | 'Rejected' | 'ConditionalApproval';
  comment?: string;
  inspectorRole?: string;
  files?: File[];
  imageUrls?: string[];
  items: ChecklistItemForReview[];
}

export interface ChecklistItemForReview {
  checklistItemId: string;
  status: 'Pending' | 'Pass' | 'Fail';
  remarks?: string;
}

@Injectable({
  providedIn: 'root'
})
export class WirChecklistService {
  private readonly apiUrl = `${environment.apiUrl}/WIRCheckPoints`;

  constructor(private http: HttpClient) {}

  /**
   * Auto-generate all 6 WIRs for a box with predefined checklist items
   */
  generateWIRsForBox(boxId: string): Observable<GenerateWIRsResponse> {
    return this.http.post<GenerateWIRsResponse>(`${this.apiUrl}/generate-for-box/${boxId}`, {});
  }

  /**
   * Get all WIRs for a box with checklist items grouped by category
   */
  getWIRsByBoxWithChecklist(boxId: string): Observable<{ isSuccess: boolean; data: WIRWithChecklist[] }> {
    return this.http.get<{ isSuccess: boolean; data: WIRWithChecklist[] }>(`${this.apiUrl}/box/${boxId}/with-checklist`);
  }

  /**
   * Get single WIR checkpoint by ID
   */
  getWIRById(wirId: string): Observable<{ isSuccess: boolean; data: any }> {
    return this.http.get<{ isSuccess: boolean; data: any }>(`${this.apiUrl}/${wirId}`);
  }

  /**
   * Update a single checklist item
   */
  updateChecklistItem(checklistItemId: string, request: UpdateChecklistItemRequest): Observable<any> {
    return this.http.put(`${this.apiUrl}/checklist-items/${checklistItemId}`, request);
  }

  /**
   * Review and approve/reject a WIR checkpoint
   */
  reviewWIR(wirId: string, request: ReviewWIRRequest): Observable<any> {
    const formData = new FormData();
    
    formData.append('Status', request.status);
    
    if (request.comment) {
      formData.append('Comment', request.comment);
    }
    
    if (request.inspectorRole) {
      formData.append('InspectorRole', request.inspectorRole);
    }

    // Add files
    if (request.files) {
      request.files.forEach((file, index) => {
        formData.append(`Files`, file, file.name);
      });
    }

    // Add image URLs
    if (request.imageUrls) {
      request.imageUrls.forEach((url, index) => {
        formData.append(`ImageUrls`, url);
      });
    }

    // Add items as JSON string
    if (request.items) {
      formData.append('ItemsJson', JSON.stringify(request.items));
    }

    return this.http.put(`${this.apiUrl}/${wirId}/review`, formData);
  }

  /**
   * Get predefined checklist items (for admin/configuration)
   */
  getPredefinedChecklistItems(): Observable<any> {
    return this.http.get(`${this.apiUrl}/predefined-checklist-items`);
  }
}
