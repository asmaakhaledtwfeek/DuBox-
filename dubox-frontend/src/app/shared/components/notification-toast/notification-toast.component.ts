import { Component, OnInit, OnDestroy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { NotificationService, Notification } from '../../../core/services/notification.service';
import { Subscription } from 'rxjs';
import { trigger, transition, style, animate } from '@angular/animations';

@Component({
  selector: 'app-notification-toast',
  standalone: true,
  imports: [CommonModule, RouterModule],
  template: `
    <div class="notification-toast-container">
      <div 
        *ngFor="let notification of activeNotifications" 
        class="notification-toast"
        [class.alert]="notification.priority === 'High'"
        [class.warning]="notification.priority === 'Medium'"
        [class.info]="notification.priority === 'Low'"
        [@slideIn]
      >
        <div class="toast-icon">
          {{ getNotificationIcon(notification) }}
        </div>
        <div class="toast-content">
          <div class="toast-title">{{ notification.title }}</div>
          <div class="toast-message">{{ notification.message }}</div>
          <div class="toast-actions" *ngIf="notification.relatedBoxId">
            <a [routerLink]="['/boxes', notification.relatedBoxId]" class="toast-link">
              View Box
            </a>
          </div>
        </div>
        <button class="toast-close" (click)="dismissNotification(notification)">
          ×
        </button>
      </div>
    </div>
  `,
  styles: [`
    .notification-toast-container {
      position: fixed;
      top: 80px;
      right: 20px;
      z-index: 9999;
      display: flex;
      flex-direction: column;
      gap: 12px;
      max-width: 400px;
    }

    .notification-toast {
      display: flex;
      gap: 12px;
      padding: 16px;
      background: white;
      border-radius: 8px;
      box-shadow: 0 4px 12px rgba(0,0,0,0.15);
      border-left: 4px solid;
      animation: slideIn 0.3s ease-out;

      &.alert {
        border-left-color: #dc3545;
      }

      &.warning {
        border-left-color: #ffc107;
      }

      &.info {
        border-left-color: #17a2b8;
      }

      .toast-icon {
        font-size: 24px;
        flex-shrink: 0;
      }

      .toast-content {
        flex: 1;
        min-width: 0;

        .toast-title {
          font-weight: 600;
          font-size: 14px;
          color: #333;
          margin-bottom: 4px;
        }

        .toast-message {
          font-size: 13px;
          color: #666;
          line-height: 1.4;
          margin-bottom: 8px;
        }

        .toast-actions {
          .toast-link {
            font-size: 12px;
            color: #007bff;
            text-decoration: none;
            font-weight: 500;

            &:hover {
              text-decoration: underline;
            }
          }
        }
      }

      .toast-close {
        border: none;
        background: transparent;
        font-size: 24px;
        color: #999;
        cursor: pointer;
        padding: 0;
        width: 24px;
        height: 24px;
        display: flex;
        align-items: center;
        justify-content: center;
        flex-shrink: 0;

        &:hover {
          color: #333;
        }
      }
    }

    @keyframes slideIn {
      from {
        transform: translateX(400px);
        opacity: 0;
      }
      to {
        transform: translateX(0);
        opacity: 1;
      }
    }
  `],
  animations: [
    trigger('slideIn', [
      transition(':enter', [
        style({ transform: 'translateX(400px)', opacity: 0 }),
        animate('300ms ease-out', style({ transform: 'translateX(0)', opacity: 1 }))
      ])
    ])
  ]
})
export class NotificationToastComponent implements OnInit, OnDestroy {
  activeNotifications: Notification[] = [];
  private subscription?: Subscription;

  constructor(private notificationService: NotificationService) {}

  ngOnInit(): void {
    this.subscription = this.notificationService.notification$.subscribe(notification => {
      this.showNotification(notification);
    });
  }

  ngOnDestroy(): void {
    this.subscription?.unsubscribe();
  }

  showNotification(notification: Notification): void {
    this.activeNotifications.push(notification);

    // Auto-dismiss after 8 seconds (longer for material alerts)
    const dismissTime = notification.notificationType === 'MaterialAlert' ? 10000 : 8000;
    setTimeout(() => {
      this.dismissNotification(notification);
    }, dismissTime);
  }

  dismissNotification(notification: Notification): void {
    const index = this.activeNotifications.indexOf(notification);
    if (index > -1) {
      this.activeNotifications.splice(index, 1);
    }

    // Mark as read in the backend
    this.notificationService.markAsRead(notification.notificationId).subscribe();
  }

  getNotificationIcon(notification: Notification): string {
    if (notification.notificationType === 'MaterialAlert') {
      return notification.priority === 'High' ? '⚠' : '⏰';
    }
    
    switch (notification.priority) {
      case 'High': return '🔴';
      case 'Medium': return '⚠️';
      default: return 'ℹ️';
    }
  }
}






