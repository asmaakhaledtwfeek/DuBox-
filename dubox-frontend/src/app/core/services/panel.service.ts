import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { BoxPanel, PanelStatus } from '../models/box.model';

export interface ScanPanelRequest {
  barcode: string;  // Backend API expects 'barcode' field, but contains QR code data
  scanType: string; // Dispatch, FactoryArrival, Installation, Inspection
  scanLocation?: string;
  latitude?: number;
  longitude?: number;
  notes?: string;
}

export interface ApprovePanelRequest {
  boxPanelId: string;
  approvalStatus: string; // Approved, Rejected
  notes?: string;
}

export interface UpdatePanelWorkflowRequest {
  boxPanelId: string;
  workflowStatus: string; // InProgress, Completed, PutOnHold
  currentStage?: number; // PanelStageEnum: 1=MoldPrep, 2=Reinforcement, 3=Casting, 4=Curing
  notes?: string;
  assignedToTeamId?: string; // QC Team for PutOnHold issues
}

@Injectable({
  providedIn: 'root'
})
export class PanelService {
  private apiUrl = `${environment.apiUrl}/boxes`;

  constructor(private http: HttpClient) {}

  updatePanelStatus(boxPanelId: string, panelStatus: PanelStatus): Observable<any> {
    return this.http.put(`${this.apiUrl}/panels/${boxPanelId}/status`, {
      boxPanelId,
      panelStatus
    });
  }

  scanPanel(request: ScanPanelRequest): Observable<any> {
    return this.http.post(`${this.apiUrl}/panels/scan`, request);
  }

  approvePanelFirstApproval(request: ApprovePanelRequest): Observable<any> {
    return this.http.post(`${this.apiUrl}/panels/${request.boxPanelId}/first-approval`, request);
  }

  approvePanelSecondApproval(request: ApprovePanelRequest): Observable<any> {
    return this.http.post(`${this.apiUrl}/panels/${request.boxPanelId}/second-approval`, request);
  }

  getPanelByBarcode(barcode: string): Observable<BoxPanel> {
    return this.http.get<BoxPanel>(`${this.apiUrl}/panels/barcode/${barcode}`);
  }

  getPanelsByBoxId(boxId: string): Observable<BoxPanel[]> {
    return this.http.get<BoxPanel[]>(`${this.apiUrl}/${boxId}/panels`);
  }

  updatePanelWorkflowStatus(request: UpdatePanelWorkflowRequest): Observable<any> {
    return this.http.post<any>(`${this.apiUrl}/panels/${request.boxPanelId}/workflow-status`, request);
  }
}
