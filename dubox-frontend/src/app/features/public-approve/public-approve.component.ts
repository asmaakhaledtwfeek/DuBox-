import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../../environments/environment';

interface ApprovalResponse {
  isSuccess: boolean;
  data?: {
    boxPanelId: string;
    panelName: string;
    approvalType: string;
    approvalStatus: string;
    approvalDate: string;
    message: string;
  };
  message?: string;
}

@Component({
  selector: 'app-public-approve',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './public-approve.component.html',
  styleUrl: './public-approve.component.scss'
})
export class PublicApproveComponent implements OnInit {
  panelId: string = '';
  token: string = '';
  approvalType: string = '';
  
  loading = false;
  success = false;
  error = '';
  
  panelName = '';
  approvalMessage = '';
  approvalDate: Date | null = null;
  
  // Location data
  latitude: number | null = null;
  longitude: number | null = null;
  deviceInfo = '';
  notes = '';
  
  gettingLocation = false;

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private http: HttpClient
  ) {
    this.deviceInfo = this.getDeviceInfo();
  }

  ngOnInit(): void {
    // Get query parameters
    this.route.queryParams.subscribe(params => {
      this.panelId = params['panelId'] || '';
      this.token = params['token'] || '';
      // Note: approvalType is no longer required - backend determines it automatically
      this.approvalType = params['type'] || 'Auto';
      
      if (!this.panelId || !this.token) {
        this.error = 'Invalid approval link. Please scan a valid QR code.';
      } else {
        // Automatically get location
        this.getLocation();
      }
    });
  }

  getLocation(): void {
    if (!navigator.geolocation) {
      console.warn('Geolocation not supported');
      return;
    }

    this.gettingLocation = true;
    navigator.geolocation.getCurrentPosition(
      (position) => {
        this.latitude = position.coords.latitude;
        this.longitude = position.coords.longitude;
        this.gettingLocation = false;
      },
      (error) => {
        console.warn('Geolocation error:', error);
        this.gettingLocation = false;
      }
    );
  }

  getDeviceInfo(): string {
    const ua = navigator.userAgent;
    let device = 'Unknown Device';
    
    if (/iPhone/i.test(ua)) {
      device = 'iPhone';
    } else if (/iPad/i.test(ua)) {
      device = 'iPad';
    } else if (/Android/i.test(ua)) {
      device = 'Android';
    } else if (/Windows/i.test(ua)) {
      device = 'Windows';
    } else if (/Mac/i.test(ua)) {
      device = 'Mac';
    }
    
    return device;
  }

  async approve(): Promise<void> {
    if (!this.panelId || !this.token) {
      this.error = 'Missing required information';
      return;
    }

    this.loading = true;
    this.error = '';
    this.success = false;

    const command = {
      boxPanelId: this.panelId,
      approvalToken: this.token,
      approvalType: this.approvalType,
      latitude: this.latitude,
      longitude: this.longitude,
      deviceInfo: this.deviceInfo,
      notes: this.notes || undefined
    };

    try {
      const response = await this.http.post<ApprovalResponse>(
        `${environment.apiUrl}/boxes/panels/public-approve`,
        command
      ).toPromise();

      this.loading = false;

      if (response && response.isSuccess && response.data) {
        this.success = true;
        this.panelName = response.data.panelName;
        this.approvalMessage = response.data.message;
        this.approvalDate = new Date(response.data.approvalDate);
      } else {
        this.error = response?.message || 'Approval failed';
      }
    } catch (err: any) {
      this.loading = false;
      this.error = err?.error?.message || err?.message || 'Failed to approve panel. Please try again.';
    }
  }

  retry(): void {
    this.success = false;
    this.error = '';
    this.notes = '';
  }
}

