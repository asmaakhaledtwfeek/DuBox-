import { Injectable } from '@angular/core';
import { Observable, Subject } from 'rxjs';
import { ApiService } from './api.service';
import { HubConnection, HubConnectionBuilder } from '@microsoft/signalr';
import { environment } from '../../../environments/environment';

export interface Notification {
  notificationId: string;
  notificationType: string;
  priority: string;
  title: string;
  message: string;
  relatedBoxId?: string;
  relatedActivityId?: string;
  recipientUserId?: string;
  targetRole?: string;
  isRead: boolean;
  createdDate: Date;
}

@Injectable({
  providedIn: 'root'
})
export class NotificationService {
  private hubConnection?: HubConnection;
  private notificationSubject = new Subject<Notification>();
  private unreadCountSubject = new Subject<number>();
  
  public notification$ = this.notificationSubject.asObservable();
  public unreadCount$ = this.unreadCountSubject.asObservable();

  constructor(private apiService: ApiService) {
    this.initializeSignalRConnection();
  }

  private initializeSignalRConnection(): void {
    // Use apiBaseUrl (without /api) for SignalR hub
    const hubUrl = environment.apiBaseUrl || environment.apiUrl.replace('/api', '');
    
    this.hubConnection = new HubConnectionBuilder()
      .withUrl(`${hubUrl}/hubs/notifications`, {
        accessTokenFactory: () => {
          // Get the JWT token from localStorage (using the same key as AuthService)
          const token = localStorage.getItem('dubox_auth_token') || '';
          return token;
        }
      })
      .withAutomaticReconnect()
      .build();

    this.hubConnection.on('ReceiveNotification', (notification: Notification) => {
      this.notificationSubject.next(notification);
    });

    this.hubConnection.on('ReceiveMaterialAlert', (notification: Notification) => {
      // Handle material-specific notifications with special UI treatment
      notification.notificationType = 'MaterialAlert';
      this.notificationSubject.next(notification);
    });

    this.startConnection();
  }

  private async startConnection(): Promise<void> {
    try {
      await this.hubConnection?.start();
      console.log('SignalR Connected for notifications');
    } catch (err) {
      console.error('Error connecting to notification hub:', err);
      setTimeout(() => this.startConnection(), 5000);
    }
  }

  getNotifications(params?: { unreadOnly?: boolean; pageNumber?: number; pageSize?: number }): Observable<any> {
    const queryParams = params || {};
    return this.apiService.get<any>('notifications', queryParams);
  }

  markAsRead(notificationId: string): Observable<any> {
    return this.apiService.put(`notifications/${notificationId}/read`, {});
  }

  markAllAsRead(): Observable<any> {
    return this.apiService.post('notifications/mark-all-read', {});
  }

  deleteNotification(notificationId: string): Observable<any> {
    return this.apiService.delete(`notifications/${notificationId}`);
  }

  getUnreadCount(): Observable<{ count: number }> {
    return this.apiService.get<{ count: number }>('notifications/unread/count');
  }

  refreshUnreadCount(): void {
    this.getUnreadCount().subscribe({
      next: (response) => {
        this.unreadCountSubject.next(response.count);
      },
      error: (err) => {
        console.error('Failed to refresh unread count:', err);
      }
    });
  }

  getMaterialNotifications(projectId?: string): Observable<Notification[]> {
    const params = projectId ? { projectId, notificationType: 'MaterialAlert' } : { notificationType: 'MaterialAlert' };
    return this.apiService.get<Notification[]>('notifications', params);
  }
}
