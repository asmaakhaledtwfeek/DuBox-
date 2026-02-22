import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { environment } from '../../../environments/environment';
import {
  ActivityTemplate,
  ActivityMaster,
  CreateActivityTemplateRequest,
  CreateActivityTemplateFromMasterRequest,
  UpdateActivityTemplateRequest,
  AddActivityToTemplateRequest,
  CreateScheduleActivityFromMasterRequest,
  CreateScheduleActivitiesFromTemplateRequest
} from '../models/activity-template.model';

@Injectable({
  providedIn: 'root'
})
export class ActivityTemplateService {
  private readonly apiUrl = `${environment.apiUrl}/activity-templates`;
  private readonly activityMasterUrl = `${environment.apiUrl}/activities/masters`;
  private readonly scheduleUrl = `${environment.apiUrl}/schedule-activities`;

  constructor(private http: HttpClient) {}

  /**
   * Get all activity templates
   */
  getAllTemplates(activeOnly: boolean = true, searchTerm?: string): Observable<ActivityTemplate[]> {
    let params = new HttpParams();
    params = params.set('activeOnly', activeOnly.toString());
    if (searchTerm) {
      params = params.set('searchTerm', searchTerm);
    }
    return this.http.get<any>(this.apiUrl, { params }).pipe(
      map(response => response.data || [])
    );
  }

  /**
   * Get template by ID
   */
  getTemplateById(id: string): Observable<ActivityTemplate> {
    return this.http.get<any>(`${this.apiUrl}/${id}`).pipe(
      map(response => response.data)
    );
  }

  /**
   * Create a new template manually
   */
  createTemplate(request: CreateActivityTemplateRequest): Observable<any> {
    return this.http.post<any>(this.apiUrl, request);
  }

  /**
   * Create a template from Activity Master
   */
  createTemplateFromMaster(request: CreateActivityTemplateFromMasterRequest): Observable<any> {
    return this.http.post<any>(`${this.apiUrl}/from-master`, request);
  }

  /**
   * Update an existing template
   */
  updateTemplate(id: string, request: UpdateActivityTemplateRequest): Observable<any> {
    return this.http.put<any>(`${this.apiUrl}/${id}`, request);
  }

  /**
   * Delete a template
   */
  deleteTemplate(id: string): Observable<any> {
    return this.http.delete<any>(`${this.apiUrl}/${id}`);
  }

  /**
   * Add activity to template
   */
  addActivityToTemplate(request: AddActivityToTemplateRequest): Observable<any> {
    return this.http.post<any>(`${this.apiUrl}/add-activity`, request);
  }

  /**
   * Remove activity from template
   */
  removeActivityFromTemplate(templateId: string, activityId: string): Observable<any> {
    return this.http.delete<any>(`${this.apiUrl}/${templateId}/activities/${activityId}`);
  }

  /**
   * Get all activity masters
   */
  getAllActivityMasters(searchTerm?: string, stage?: string): Observable<ActivityMaster[]> {
    let params = new HttpParams();
    if (searchTerm) {
      params = params.set('searchTerm', searchTerm);
    }
    if (stage) {
      params = params.set('stage', stage);
    }
    return this.http.get<any>(this.activityMasterUrl, { params }).pipe(
      map(response => response.data || [])
    );
  }

  /**
   * Get activity master by ID
   */
  getActivityMasterById(id: string): Observable<ActivityMaster> {
    return this.http.get<any>(`${this.activityMasterUrl}/${id}`).pipe(
      map(response => response.data)
    );
  }

  /**
   * Create schedule activity from master
   */
  createScheduleActivityFromMaster(request: CreateScheduleActivityFromMasterRequest): Observable<any> {
    return this.http.post<any>(`${this.scheduleUrl}/from-master`, request);
  }

  /**
   * Create schedule activities from template
   */
  createScheduleActivitiesFromTemplate(request: CreateScheduleActivitiesFromTemplateRequest): Observable<any> {
    return this.http.post<any>(`${this.scheduleUrl}/from-template`, request);
  }

  /**
   * Get all schedule activities
   */
  getAllScheduleActivities(projectId?: string): Observable<any> {
    let params = new HttpParams();
    if (projectId) {
      params = params.set('projectId', projectId);
    }
    return this.http.get<any>(this.scheduleUrl, { params }).pipe(
      map(response => response.data || [])
    );
  }

  /**
   * Get all checklists
   */
  getAllChecklists(): Observable<any> {
    return this.http.get<any>(`${environment.apiUrl}/checklists`).pipe(
      map(response => response.data || [])
    );
  }

  /**
   * Get checklists by WIR code
   */
  getChecklistsByWirCode(wirCode: string): Observable<any> {
    return this.http.get<any>(`${environment.apiUrl}/checklists/by-wir-code/${wirCode}`).pipe(
      map(response => response.data || [])
    );
  }

  /**
   * Get checklist items for an activity template activity
   */
  getChecklistItemsForTemplateActivity(activityTemplateActivityId: string): Observable<any> {
    return this.http.get<any>(`${environment.apiUrl}/activity-checklist-items/by-activity-template-activity/${activityTemplateActivityId}`).pipe(
      map(response => response.data || [])
    );
  }

  /**
   * Assign checklist items to activity template activity
   */
  assignChecklistItemsToActivity(activityTemplateActivityId: string, checklistItemIds: string[]): Observable<any> {
    const payload = {
      activityTemplateActivityId: activityTemplateActivityId,
      checklistItems: checklistItemIds.map(itemId => ({
        predefinedChecklistItemId: itemId
      }))
    };
    return this.http.post<any>(`${environment.apiUrl}/activity-checklist-items/bulk`, payload);
  }

  /**
   * Get all teams for activity assignment
   */
  getAllTeams(): Observable<any> {
    return this.http.get<any>(`${environment.apiUrl}/teams`).pipe(
      map(response => response.data || [])
    );
  }
}
