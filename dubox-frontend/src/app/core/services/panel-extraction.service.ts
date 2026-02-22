import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';

export interface PanelExtractionReviewDto {
  panelTypes: ReviewPanelTypeDto[];
  boxPanels: ReviewBoxPanelDto[];
  warnings: string[];
  boxTags: string[];
}

export interface ReviewPanelTypeDto {
  panelTypeId?: string;
  panelTypeCode: string;
  panelTypeName: string;
  volumeM3?: number;
  weightTon?: number;
  concreteGrade?: string;
  coverMm?: number;
  embedsJson?: string;
  isNew: boolean;
}

export interface ReviewBoxPanelDto {
  panelTypeCode: string;
  panelName: string;
  quantityInThisBox: number;
}

export interface PanelExtractionConfirmationDto {
  panelTypesCreated: number;
  panelTypesUpdated: number;
  boxPanelsCreated: number;
  createdPanelTypeIds: string[];
  createdBoxPanelIds: string[];
  errors: string[];
}

export interface BoxTagMapping {
  boxTag: string;
  boxId?: string;
  newBoxName?: string;
}

export interface ConfirmPanelExtractionRequest {
  boxId?: string;
  panelTypes: ReviewPanelTypeDto[];
  boxPanels: ReviewBoxPanelDto[];
  boxTagMappings: BoxTagMapping[];
}

@Injectable({
  providedIn: 'root'
})
export class PanelExtractionService {
  private apiUrl = `${environment.apiUrl}/projects`;

  constructor(private http: HttpClient) {}

  /**
   * Extract panels from PDF using AI
   * @param projectId Project ID
   * @param file PDF file
   * @param boxId Optional box ID to associate panels with
   * @param boxTagsFile Optional second PDF file for box tags extraction
   * @returns Observable of extraction review data
   */
  extractPanelsFromPdf(
    projectId: string,
    file: File,
    boxId?: string,
    boxTagsFile?: File
  ): Observable<any> {
    const formData = new FormData();
    formData.append('file', file);
    if (boxId) {
      formData.append('boxId', boxId);
    }
    if (boxTagsFile) {
      formData.append('boxTagsFile', boxTagsFile);
    }

    return this.http.post(
      `${this.apiUrl}/${projectId}/panels/extract-from-pdf`,
      formData
    );
  }

  /**
   * Confirm and save reviewed panel extraction data
   * @param projectId Project ID
   * @param request Confirmation request with reviewed data
   * @returns Observable of confirmation result
   */
  confirmPanelExtraction(
    projectId: string,
    request: ConfirmPanelExtractionRequest
  ): Observable<any> {
    return this.http.post(
      `${this.apiUrl}/${projectId}/panels/confirm-extraction`,
      request
    );
  }
}
