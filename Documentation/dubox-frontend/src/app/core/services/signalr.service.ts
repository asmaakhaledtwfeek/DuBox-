import { Injectable } from '@angular/core';
import * as signalR from '@microsoft/signalr';
import { BehaviorSubject, Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { AuthService } from './auth.service';

@Injectable({
  providedIn: 'root'
})
export class SignalRService {
  private hubConnection: signalR.HubConnection | null = null;
  private connectionStateSubject = new BehaviorSubject<boolean>(false);
  public connectionState$ = this.connectionStateSubject.asObservable();

  private notificationReceivedSubject = new BehaviorSubject<any>(null);
  public notificationReceived$ = this.notificationReceivedSubject.asObservable();

  private notificationCountSubject = new BehaviorSubject<number>(0);
  public notificationCount$ = this.notificationCountSubject.asObservable();

  constructor(private authService: AuthService) {
    // Start connection when user is authenticated
    this.authService.authState$.subscribe(state => {
      if (state.isAuthenticated && state.token) {
        this.startConnection(state.token);
      } else {
        this.stopConnection();
      }
    });
  }

  private async startConnection(token: string): Promise<void> {
    if (this.hubConnection && this.hubConnection.state === signalR.HubConnectionState.Connected) {
      return; // Already connected
    }

    try {
      // Get the base URL without /api
      const baseUrl = environment.apiUrl.replace(/\/api\/?$/, '');
      
      this.hubConnection = new signalR.HubConnectionBuilder()
        .withUrl(`${baseUrl}/hubs/notifications`, {
          accessTokenFactory: () => token,
          skipNegotiation: true,
          transport: signalR.HttpTransportType.WebSockets
        })
        .withAutomaticReconnect([0, 2000, 5000, 10000, 30000])
        .configureLogging(signalR.LogLevel.Information)
        .build();

      // Set up event handlers
      this.setupEventHandlers();

      // Start the connection
      await this.hubConnection.start();
      console.log('SignalR Connected');
      this.connectionStateSubject.next(true);

    } catch (error) {
      console.error('Error connecting to SignalR:', error);
      this.connectionStateSubject.next(false);
      
      // Retry connection after delay
      setTimeout(() => this.startConnection(token), 5000);
    }
  }

  private setupEventHandlers(): void {
    if (!this.hubConnection) return;

    // Handle incoming notifications
    this.hubConnection.on('ReceiveNotification', (notification: any) => {
      console.log('Received notification:', notification);
      this.notificationReceivedSubject.next(notification);
    });

    // Handle notification count updates
    this.hubConnection.on('NotificationCountUpdated', (count: number) => {
      console.log('Notification count updated:', count);
      this.notificationCountSubject.next(count);
    });

    // Handle reconnection
    this.hubConnection.onreconnecting(() => {
      console.log('SignalR Reconnecting...');
      this.connectionStateSubject.next(false);
    });

    this.hubConnection.onreconnected(() => {
      console.log('SignalR Reconnected');
      this.connectionStateSubject.next(true);
    });

    // Handle disconnection
    this.hubConnection.onclose(() => {
      console.log('SignalR Disconnected');
      this.connectionStateSubject.next(false);
    });
  }

  private async stopConnection(): Promise<void> {
    if (this.hubConnection) {
      try {
        await this.hubConnection.stop();
        console.log('SignalR Connection stopped');
        this.connectionStateSubject.next(false);
      } catch (error) {
        console.error('Error stopping SignalR connection:', error);
      }
    }
  }

  public getConnectionState(): Observable<boolean> {
    return this.connectionState$;
  }

  public getNotificationCount(): number {
    return this.notificationCountSubject.value;
  }
}

