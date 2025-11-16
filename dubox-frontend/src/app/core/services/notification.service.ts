import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';
import { tap } from 'rxjs/operators';
import { ApiService } from './api.service';
import { Notification } from '../models/notification.model';

@Injectable({
  providedIn: 'root'
})
export class NotificationService {
  private readonly endpoint = 'notifications';
  private unreadCountSubject = new BehaviorSubject<number>(0);
  public unreadCount$ = this.unreadCountSubject.asObservable();

  constructor(private apiService: ApiService) {
    this.loadUnreadCount();
  }

  /**
   * Get all notifications
   */
  getNotifications(params?: any): Observable<Notification[]> {
    return this.apiService.get<Notification[]>(this.endpoint, params);
  }

  /**
   * Get unread notifications
   */
  getUnreadNotifications(): Observable<Notification[]> {
    return this.apiService.get<Notification[]>(`${this.endpoint}/unread`);
  }

  /**
   * Get notification by ID
   */
  getNotification(id: string): Observable<Notification> {
    return this.apiService.get<Notification>(`${this.endpoint}/${id}`);
  }

  /**
   * Mark notification as read
   */
  markAsRead(id: string): Observable<void> {
    return this.apiService.patch<void>(`${this.endpoint}/${id}/read`, {}).pipe(
      tap(() => this.decrementUnreadCount())
    );
  }

  /**
   * Mark all notifications as read
   */
  markAllAsRead(): Observable<void> {
    return this.apiService.post<void>(`${this.endpoint}/mark-all-read`, {}).pipe(
      tap(() => this.unreadCountSubject.next(0))
    );
  }

  /**
   * Delete notification
   */
  deleteNotification(id: string): Observable<void> {
    return this.apiService.delete<void>(`${this.endpoint}/${id}`);
  }

  /**
   * Get unread count
   */
  getUnreadCount(): Observable<{ count: number }> {
    return this.apiService.get<{ count: number }>(`${this.endpoint}/unread/count`).pipe(
      tap(result => this.unreadCountSubject.next(result.count))
    );
  }

  /**
   * Load unread count
   */
  private loadUnreadCount(): void {
    this.getUnreadCount().subscribe();
  }

  /**
   * Decrement unread count
   */
  private decrementUnreadCount(): void {
    const current = this.unreadCountSubject.value;
    if (current > 0) {
      this.unreadCountSubject.next(current - 1);
    }
  }

  /**
   * Increment unread count (for real-time updates)
   */
  incrementUnreadCount(): void {
    const current = this.unreadCountSubject.value;
    this.unreadCountSubject.next(current + 1);
  }
}
