import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Router } from '@angular/router';
import { HeaderComponent } from '../../../shared/components/header/header.component';
import { SidebarComponent } from '../../../shared/components/sidebar/sidebar.component';
import { NotificationService } from '../../../core/services/notification.service';
import { ToastService } from '../../../core/services/toast.service';

interface Notification {
  notificationId: string;
  notificationType: string;
  priority: string;
  title: string;
  message: string;
  directLink?: string;
  isRead: boolean;
  readDate?: Date;
  createdDate: Date;
  relatedIssueId?: string;
  relatedIssueNumber?: string;
  relatedCommentId?: string;
  commentAuthorName?: string;
}

@Component({
  selector: 'app-notifications-center',
  standalone: true,
  imports: [CommonModule, RouterModule, HeaderComponent, SidebarComponent],
  templateUrl: './notifications-center.component.html',
  styleUrls: ['./notifications-center.component.scss']
})
export class NotificationsCenterComponent implements OnInit {
  notifications: Notification[] = [];
  loading = false;
  filter: 'all' | 'unread' = 'all';
  currentPage = 1;
  pageSize = 20;
  totalPages = 1;
  totalCount = 0;
  unreadCount = 0;

  constructor(
    public notificationService: NotificationService,
    private toastService: ToastService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.loadNotifications();
    
    // Subscribe to unread count updates
    this.notificationService.unreadCount$.subscribe(count => {
      this.unreadCount = count;
    });
  }

  loadNotifications(): void {
    this.loading = true;
    const unreadOnly = this.filter === 'unread';
    
    console.log('🔍 Loading notifications with filter:', { unreadOnly, currentPage: this.currentPage, pageSize: this.pageSize });
    
    this.notificationService.getNotifications({ 
      unreadOnly, 
      pageNumber: this.currentPage, 
      pageSize: this.pageSize 
    }).subscribe({
      next: (response: any) => {
        console.log('📩 Received notifications response:', response);
        
        // Handle both response formats: direct and wrapped in data
        const data = response.data || response;
        
        this.notifications = data.notifications || [];
        this.totalCount = data.totalCount || 0;
        this.totalPages = data.totalPages || 1;
        
        console.log('✅ Parsed notifications:', {
          count: this.notifications.length,
          totalCount: this.totalCount,
          totalPages: this.totalPages,
          notifications: this.notifications
        });
        
        this.loading = false;
      },
      error: (error) => {
        console.error('❌ Error loading notifications:', error);
        this.toastService.showError('Failed to load notifications');
        this.loading = false;
      }
    });
  }

  markAsRead(notification: Notification): void {
    if (notification.isRead) {
      this.navigateToLink(notification);
      return;
    }

    this.notificationService.markAsRead(notification.notificationId).subscribe({
      next: () => {
        notification.isRead = true;
        notification.readDate = new Date();
        this.navigateToLink(notification);
      },
      error: (error) => {
        console.error('Error marking notification as read:', error);
      }
    });
  }

  navigateToLink(notification: Notification): void {
    if (notification.directLink) {
      this.router.navigateByUrl(notification.directLink);
    } else if (notification.relatedIssueId) {
      // Navigate to quality issue
      this.router.navigate(['/quality-issues', notification.relatedIssueId]);
    }
  }

  setFilter(filter: 'all' | 'unread'): void {
    this.filter = filter;
    this.currentPage = 1;
    this.loadNotifications();
  }

  nextPage(): void {
    if (this.currentPage < this.totalPages) {
      this.currentPage++;
      this.loadNotifications();
    }
  }

  previousPage(): void {
    if (this.currentPage > 1) {
      this.currentPage--;
      this.loadNotifications();
    }
  }

  getNotificationIcon(type: string): string {
    switch (type) {
      case 'CommentAdded':
        return 'bi-chat-left-text';
      case 'CommentUpdated':
        return 'bi-chat-left-dots';
      case 'Alert':
        return 'bi-exclamation-triangle';
      case 'Warning':
        return 'bi-exclamation-circle';
      case 'Info':
        return 'bi-info-circle';
      default:
        return 'bi-bell';
    }
  }

  getNotificationClass(type: string): string {
    switch (type) {
      case 'CommentAdded':
      case 'CommentUpdated':
        return 'notification-comment';
      case 'Alert':
        return 'notification-alert';
      case 'Warning':
        return 'notification-warning';
      case 'Info':
        return 'notification-info';
      default:
        return 'notification-default';
    }
  }

  formatTime(date: Date | string): string {
    const d = new Date(date);
    const now = new Date();
    const diff = now.getTime() - d.getTime();
    const minutes = Math.floor(diff / 60000);
    const hours = Math.floor(minutes / 60);
    const days = Math.floor(hours / 24);

    if (days > 7) {
      return d.toLocaleDateString();
    } else if (days > 0) {
      return `${days}d ago`;
    } else if (hours > 0) {
      return `${hours}h ago`;
    } else if (minutes > 0) {
      return `${minutes}m ago`;
    } else {
      return 'Just now';
    }
  }
}
