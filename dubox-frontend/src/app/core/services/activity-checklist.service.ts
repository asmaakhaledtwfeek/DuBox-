import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { environment } from '../../../environments/environment';

export interface ActivityCheckListItemWithReview {
  activityCheckListItemId: string;
  activityMasterId?: string;
  activityTemplateActivityId?: string;
  predefinedChecklistItemId: string;
  checkpointDescription: string;
  reference?: string;
  sequence: number;
  isMandatory: boolean;
  isActive: boolean;
  // Grouping information
  checklistSectionId?: string;
  sectionTitle?: string;
  sectionOrder?: number;
  checklistId?: string;
  checklistName?: string;
  checklistCode?: string;
  // Review information
  reviewId?: string;
  reviewStatus: 'Pending' | 'Pass' | 'Fail' | 'NA';
  remarks?: string;
  reviewedBy?: string;
  reviewedByName?: string;
  reviewedDate?: string;
}

export interface GetActivityChecklistByBoxActivity {
  boxActivityId: string;
  activityName: string;
  activityCode: string;
  progressPercentage: number;
  checklistItems: ActivityCheckListItemWithReview[];
}

export interface ReviewActivityCheckListItem {
  boxActivityId: string;
  activityCheckListItemId: string;
  status: 'Pending' | 'Pass' | 'Fail' | 'NA';
  remarks?: string;
}

export interface SubmitActivityChecklistReview {
  boxActivityId: string;
  reviews: ReviewActivityCheckListItem[];
}

export interface ActivityChecklistReviewSummary {
  totalReviews: number;
  pendingReviews: number;
  passedReviews: number;
  failedReviews: number;
  naReviews: number;
}

export interface ActivityReviewItem {
  boxActivityId: string;
  activityName: string;
  activityCode: string;
  boxId: string;
  boxTag: string;
  projectId: string;
  projectCode: string;
  projectName: string;
  totalItems: number;
  reviewedItems: number;
  pendingItems: number;
  passedItems: number;
  failedItems: number;
  reviewStatus: 'Pending' | 'Completed' | 'Partial';
  lastReviewedDate?: string;
  lastReviewedBy?: string;
  progressPercentage: number;
}

export interface GetActivityReviewsParams {
  page?: number;
  pageSize?: number;
  projectId?: string;
  boxId?: string;
  buildingNumber?: string;
  floor?: string;
  boxTypeId?: number;
  reviewStatus?: string;
  reviewedBy?: string;
  searchTerm?: string;
}

export interface PaginatedActivityReviews {
  items: ActivityReviewItem[];
  totalCount: number;
  page: number;
  pageSize: number;
  totalPages: number;
}

@Injectable({
  providedIn: 'root'
})
export class ActivityChecklistService {
  private apiUrl = `${environment.apiUrl}/activity-checklist-items`;

  constructor(private http: HttpClient) {}

  /**
   * Get checklist items for a specific BoxActivity with review status
   */
  getActivityChecklistByBoxActivity(boxActivityId: string): Observable<any> {
    return this.http.get<any>(`${this.apiUrl}/by-box-activity/${boxActivityId}`);
  }

  /**
   * Submit reviews for activity checklist items
   * Can only be done when activity progress is 100%
   */
  submitChecklistReview(data: SubmitActivityChecklistReview): Observable<any> {
    return this.http.post<any>(`${this.apiUrl}/review`, data);
  }

  /**
   * Get summary/count of activity checklist reviews
   */
  getActivityReviewSummary(): Observable<ActivityChecklistReviewSummary> {
    return this.http.get<any>(`${this.apiUrl}/reviews/summary`).pipe(
      map((response: any) => {
        // API may return { data: { ... } } or direct DTO
        const raw = response?.data ?? response;
        if (!raw) return { totalReviews: 0, pendingReviews: 0, passedReviews: 0, failedReviews: 0, naReviews: 0 };
        return {
          totalReviews: raw.totalReviews ?? raw.TotalReviews ?? 0,
          pendingReviews: raw.pendingReviews ?? raw.PendingReviews ?? 0,
          passedReviews: raw.passedReviews ?? raw.PassedReviews ?? 0,
          failedReviews: raw.failedReviews ?? raw.FailedReviews ?? 0,
          naReviews: raw.naReviews ?? raw.NaReviews ?? 0
        };
      })
    );
  }

  /**
   * Get paginated list of activity reviews
   */
  getActivityReviews(params: GetActivityReviewsParams): Observable<PaginatedActivityReviews> {
    const httpParams: any = {};
    if (params.page) httpParams.page = params.page.toString();
    if (params.pageSize) httpParams.pageSize = params.pageSize.toString();
    if (params.projectId) httpParams.projectId = params.projectId;
    if (params.boxId) httpParams.boxId = params.boxId;
    if (params.buildingNumber) httpParams.buildingNumber = params.buildingNumber;
    if (params.floor) httpParams.floor = params.floor;
    if (params.boxTypeId) httpParams.boxTypeId = params.boxTypeId.toString();
    if (params.reviewStatus) httpParams.reviewStatus = params.reviewStatus;
    if (params.reviewedBy) httpParams.reviewedBy = params.reviewedBy;
    if (params.searchTerm) httpParams.searchTerm = params.searchTerm;

    return this.http.get<any>(`${this.apiUrl}/reviews`, { params: httpParams }).pipe(
      map((response: any) => response.data || response)
    );
  }
}





