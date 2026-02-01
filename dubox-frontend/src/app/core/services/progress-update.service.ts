import { Injectable } from '@angular/core';
import { Observable, tap, map, of } from 'rxjs';
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
  
  // Cache for box progress updates (keyed by boxId)
  private progressUpdatesCache = new Map<string, {
    data: PaginatedProgressUpdatesResponse;
    params: string; // stringified search params for cache key
    timestamp: number;
  }>();
  private readonly CACHE_DURATION = 5 * 60 * 1000; // 5 minutes

  constructor(private apiService: ApiService) {}

  /**
   * Create a new progress update for an activity
   * Supports multiple files and URLs
   */
  createProgressUpdate(
    request: CreateProgressUpdateRequest, 
    files?: File[], 
    imageUrls?: string[],
    fileNames?: string[]
  ): Observable<ProgressUpdateResponse> {
    // ALWAYS send as multipart/form-data because backend API expects it
    // Backend has [Consumes("multipart/form-data")] attribute
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
    
    // Append WIR position fields:
    // - If undefined, don't append (no WIR activity below)
    // - If null, append empty string (WIR activity exists but record not created yet)
    // - If value exists, append the value
    if (request.wirBay !== undefined) {
      formData.append('WirBay', request.wirBay ?? '');
    }
    if (request.wirRow !== undefined) {
      formData.append('WirRow', request.wirRow ?? '');
    }
    if (request.wirPosition !== undefined) {
      formData.append('WirPosition', request.wirPosition ?? '');
    }
    
    // Append multiple files if provided - ASP.NET Core expects Files parameter name
    if (files && files.length > 0) {
      files.forEach((file) => {
        formData.append('Files', file);
      });
    }
    
    // Append multiple image URLs if provided - ASP.NET Core expects ImageUrls parameter name
    if (imageUrls && imageUrls.length > 0) {
      imageUrls.forEach((url) => {
        formData.append('ImageUrls', url);
      });
    }
    
    // Append file names if provided - for version tracking
    if (fileNames && fileNames.length > 0) {
      fileNames.forEach((name) => {
        formData.append('FileNames', name);
      });
      console.log('📎 VERSION DEBUG - Sending FileNames to backend:', fileNames);
    }
    
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

  /**
   * Get a single progress update by ID
   */
  getProgressUpdateById(progressUpdateId: string): Observable<ProgressUpdate> {
    return this.apiService.get<any>(`${this.endpoint}/${progressUpdateId}`).pipe(
      map((response: any) => {
        // Handle both direct data and wrapped response
        const data = response.data || response.Data || response;
        return data;
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
    searchParams?: ProgressUpdatesSearchParams,
    forceRefresh: boolean = false
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
    
    // Create cache key from boxId and all parameters
    const cacheKey = `${boxId}_${params.toString()}`;
    
    // Check cache if not forcing refresh
    if (!forceRefresh) {
      const cached = this.progressUpdatesCache.get(cacheKey);
      if (cached && (Date.now() - cached.timestamp) < this.CACHE_DURATION) {
        console.log('📋 Returning cached progress updates for box', boxId);
        return of(cached.data);
      }
    }
    
    console.log('🔄 Loading progress updates from API for box', boxId);
    return this.apiService.get<any>(`${this.endpoint}/box/${boxId}?${params.toString()}`).pipe(
      map((response: any) => {
        const data = response.data || response.Data || response;
        const items = data.items || data.Items || [];
        const totalCount = data.totalCount ?? data.TotalCount ?? 0;
        const responsePageNumber = data.pageNumber ?? data.PageNumber ?? pageNumber;
        const responsePageSize = data.pageSize ?? data.PageSize ?? pageSize;
        const totalPages = data.totalPages ?? data.TotalPages ?? 0;
        
        const result = {
          items: items,
          totalCount,
          pageNumber: responsePageNumber,
          pageSize: responsePageSize,
          totalPages
        };
        
        // Cache the result
        this.progressUpdatesCache.set(cacheKey, {
          data: result,
          params: params.toString(),
          timestamp: Date.now()
        });
        
        return result;
      })
    );
  }
  
  /**
   * Clear cache for a specific box or all boxes
   */
  clearCache(boxId?: string): void {
    if (boxId) {
      // Clear all cache entries for this box
      const keysToDelete: string[] = [];
      this.progressUpdatesCache.forEach((_, key) => {
        if (key.startsWith(boxId + '_')) {
          keysToDelete.push(key);
        }
      });
      keysToDelete.forEach(key => this.progressUpdatesCache.delete(key));
      console.log('🗑️ Cleared progress updates cache for box', boxId);
    } else {
      // Clear all cache
      this.progressUpdatesCache.clear();
      console.log('🗑️ Cleared all progress updates cache');
    }
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

