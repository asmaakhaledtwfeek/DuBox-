import { Injectable } from '@angular/core';
import { Observable, tap, map } from 'rxjs';
import { ApiService } from './api.service';
import { 
  ProgressUpdate, 
  CreateProgressUpdateRequest, 
  ProgressUpdateResponse,
  BoxActivityDetail,
  PaginatedProgressUpdatesResponse,
  ProgressUpdatesSearchParams
} from '../models/progress-update.model';

@Injectable({
  providedIn: 'root'
})
export class ProgressUpdateService {
  private readonly endpoint = 'progressupdates';

  constructor(private apiService: ApiService) {}

  /**
   * Create a new progress update for an activity
   */
  createProgressUpdate(request: CreateProgressUpdateRequest, file?: File): Observable<ProgressUpdateResponse> {
    // If file is provided, send as multipart/form-data
    if (file) {
      const formData = new FormData();
      formData.append('BoxId', request.boxId);
      formData.append('BoxActivityId', request.boxActivityId);
      formData.append('ProgressPercentage', request.progressPercentage.toString());
      
      if (request.workDescription) {
        formData.append('WorkDescription', request.workDescription);
      }
      if (request.issuesEncountered) {
        formData.append('IssuesEncountered', request.issuesEncountered);
      }
      if (request.latitude !== undefined) {
        formData.append('Latitude', request.latitude.toString());
      }
      if (request.longitude !== undefined) {
        formData.append('Longitude', request.longitude.toString());
      }
      if (request.locationDescription) {
        formData.append('LocationDescription', request.locationDescription);
      }
      if (request.imageUrl) {
        formData.append('ImageUrl', request.imageUrl);
      }
      formData.append('File', file);
      formData.append('UpdateMethod', request.updateMethod);
      if (request.deviceInfo) {
        formData.append('DeviceInfo', request.deviceInfo);
      }

      return this.apiService.post<ProgressUpdateResponse>(this.endpoint, formData).pipe(
        tap(response => {
          console.log('✅ Progress update created:', response);
          if (response.wirCreated) {
            console.log('🎯 WIR automatically created:', response.wirCode);
          }
        })
      );
    }
    
    // If imageUrl is provided without file, send as multipart/form-data
    if (request.imageUrl) {
      const formData = new FormData();
      formData.append('BoxId', request.boxId);
      formData.append('BoxActivityId', request.boxActivityId);
      formData.append('ProgressPercentage', request.progressPercentage.toString());
      
      if (request.workDescription) {
        formData.append('WorkDescription', request.workDescription);
      }
      if (request.issuesEncountered) {
        formData.append('IssuesEncountered', request.issuesEncountered);
      }
      if (request.latitude !== undefined) {
        formData.append('Latitude', request.latitude.toString());
      }
      if (request.longitude !== undefined) {
        formData.append('Longitude', request.longitude.toString());
      }
      if (request.locationDescription) {
        formData.append('LocationDescription', request.locationDescription);
      }
      formData.append('ImageUrl', request.imageUrl);
      formData.append('UpdateMethod', request.updateMethod);
      if (request.deviceInfo) {
        formData.append('DeviceInfo', request.deviceInfo);
      }

      return this.apiService.post<ProgressUpdateResponse>(this.endpoint, formData).pipe(
        tap(response => {
          console.log('✅ Progress update created:', response);
          if (response.wirCreated) {
            console.log('🎯 WIR automatically created:', response.wirCode);
          }
        })
      );
    }

    // Otherwise, send as JSON (no image)
    return this.apiService.post<ProgressUpdateResponse>(this.endpoint, request).pipe(
      tap(response => {
        console.log('✅ Progress update created:', response);
        if (response.wirCreated) {
          console.log('🎯 WIR automatically created:', response.wirCode);
        }
      })
    );
  }

  /**
   * Get all progress updates for a specific activity
   */
  getProgressUpdatesByActivity(activityId: string): Observable<ProgressUpdate[]> {
    return this.apiService.get<ProgressUpdate[]>(`${this.endpoint}/activity/${activityId}`);
  }

  /**
   * Get all progress updates for a box with pagination and search
   */
  getProgressUpdatesByBox(
    boxId: string, 
    pageNumber: number = 1, 
    pageSize: number = 10,
    searchParams?: ProgressUpdatesSearchParams
  ): Observable<PaginatedProgressUpdatesResponse> {
    const params = new URLSearchParams();
    params.set('pageNumber', pageNumber.toString());
    params.set('pageSize', pageSize.toString());
    
    if (searchParams) {
      if (searchParams.searchTerm) {
        params.set('searchTerm', searchParams.searchTerm);
      }
      if (searchParams.activityName) {
        params.set('activityName', searchParams.activityName);
      }
      if (searchParams.status) {
        params.set('status', searchParams.status);
      }
      if (searchParams.fromDate) {
        params.set('fromDate', searchParams.fromDate);
      }
      if (searchParams.toDate) {
        params.set('toDate', searchParams.toDate);
      }
    }
    
    return this.apiService.get<any>(`${this.endpoint}/box/${boxId}?${params.toString()}`).pipe(
      map((response: any) => {
        const data = response.data || response.Data || response;
        const items = data.items || data.Items || [];
        const totalCount = data.totalCount ?? data.TotalCount ?? 0;
        const responsePageNumber = data.pageNumber ?? data.PageNumber ?? pageNumber;
        const responsePageSize = data.pageSize ?? data.PageSize ?? pageSize;
        const totalPages = data.totalPages ?? data.TotalPages ?? 0;
        
        return {
          items: items,
          totalCount,
          pageNumber: responsePageNumber,
          pageSize: responsePageSize,
          totalPages
        };
      })
    );
  }

  /**
   * Get activity details with progress history
   */
  getActivityDetails(activityId: string): Observable<BoxActivityDetail> {
    return this.apiService.get<BoxActivityDetail>(`activities/${activityId}`);
  }

  /**
   * Get all activities for a box with their current progress
   */
  getBoxActivitiesWithProgress(boxId: string): Observable<BoxActivityDetail[]> {
    return this.apiService.get<BoxActivityDetail[]>(`activities/box/${boxId}`);
  }

  /**
   * Upload photos for progress update
   */
  uploadProgressPhotos(files: File[]): Observable<string[]> {
    const formData = new FormData();
    files.forEach(file => {
      formData.append('files', file);
    });
    return this.apiService.post<string[]>('upload/progress-photos', formData);
  }
}

