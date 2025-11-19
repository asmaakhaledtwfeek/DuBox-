import { Injectable } from '@angular/core';
import { Observable, tap } from 'rxjs';
import { ApiService } from './api.service';
import { 
  ProgressUpdate, 
  CreateProgressUpdateRequest, 
  ProgressUpdateResponse,
  BoxActivityDetail 
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
  createProgressUpdate(request: CreateProgressUpdateRequest): Observable<ProgressUpdateResponse> {
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
   * Get all progress updates for a box
   */
  getProgressUpdatesByBox(boxId: string): Observable<ProgressUpdate[]> {
    return this.apiService.get<ProgressUpdate[]>(`${this.endpoint}/box/${boxId}`);
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

