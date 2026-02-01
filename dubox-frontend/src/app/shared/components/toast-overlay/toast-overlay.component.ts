import { Component, OnInit, OnDestroy, ChangeDetectorRef, NgZone } from '@angular/core';
import { CommonModule } from '@angular/common';

interface Toast {
  id: number;
  message: string;
  type: 'success' | 'error' | 'warning' | 'info';
  duration?: number;
}

/**
 * Toast overlay that renders ABOVE modals (z-index: 99999).
 * Listens for app-toast events and displays toasts on top of everything.
 */
@Component({
  selector: 'app-toast-overlay',
  standalone: true,
  imports: [CommonModule],
  template: `
    <div class="toast-overlay-container">
      <div *ngFor="let toast of toasts"
           class="toast-overlay toast-{{ toast.type }}"
           (click)="removeToast(toast.id)">
        <div class="toast-icon">
          <svg *ngIf="toast.type === 'success'" xmlns="http://www.w3.org/2000/svg" width="20" height="20" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2">
            <path d="M22 11.08V12a10 10 0 1 1-5.93-9.14"/>
            <polyline points="22 4 12 14.01 9 11.01"/>
          </svg>
          <svg *ngIf="toast.type === 'error'" xmlns="http://www.w3.org/2000/svg" width="20" height="20" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2">
            <circle cx="12" cy="12" r="10"/>
            <line x1="12" y1="8" x2="12" y2="12"/>
            <line x1="12" y1="16" x2="12.01" y2="16"/>
          </svg>
          <svg *ngIf="toast.type === 'warning'" xmlns="http://www.w3.org/2000/svg" width="20" height="20" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2">
            <path d="M10.29 3.86L1.82 18a2 2 0 0 0 1.71 3h16.94a2 2 0 0 0 1.71-3L13.71 3.86a2 2 0 0 0-3.42 0z"/>
            <line x1="12" y1="9" x2="12" y2="13"/>
            <line x1="12" y1="17" x2="12.01" y2="17"/>
          </svg>
          <svg *ngIf="toast.type === 'info'" xmlns="http://www.w3.org/2000/svg" width="20" height="20" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2">
            <circle cx="12" cy="12" r="10"/>
            <line x1="12" y1="16" x2="12" y2="12"/>
            <line x1="12" y1="8" x2="12.01" y2="8"/>
          </svg>
        </div>
        <span class="toast-message">{{ toast.message }}</span>
        <button class="toast-close" (click)="$event.stopPropagation(); removeToast(toast.id)" type="button">×</button>
      </div>
    </div>
  `,
  styles: [`
    .toast-overlay-container {
      position: fixed;
      top: 80px;
      left: 50%;
      transform: translateX(-50%);
      z-index: 99999;
      display: flex;
      flex-direction: column;
      gap: 12px;
      max-width: 420px;
      pointer-events: none;
    }

    .toast-overlay {
      pointer-events: auto;
      display: flex;
      align-items: center;
      gap: 12px;
      padding: 16px 18px;
      border-radius: 10px;
      box-shadow: 0 8px 24px rgba(0, 0, 0, 0.15), 0 2px 8px rgba(0, 0, 0, 0.1);
      border-left: 5px solid;
      min-width: 320px;
      max-width: 90vw;
      animation: toastSlideIn 0.4s ease-out;
    }

    .toast-overlay.toast-success {
      background: #ecfdf5;
      border-left-color: #10b981;
    }
    .toast-overlay.toast-error {
      background: #fee2e2;
      border-left-color: #dc2626;
    }
    .toast-overlay.toast-warning {
      background: #fffbeb;
      border-left-color: #f59e0b;
    }
    .toast-overlay.toast-info {
      background: #eff6ff;
      border-left-color: #3b82f6;
    }

    .toast-icon {
      flex-shrink: 0;
      width: 36px;
      height: 36px;
      border-radius: 50%;
      display: flex;
      align-items: center;
      justify-content: center;
    }

    .toast-success .toast-icon { background: linear-gradient(135deg, #10b981 0%, #059669 100%); }
    .toast-error .toast-icon { background: linear-gradient(135deg, #ef4444 0%, #dc2626 100%); }
    .toast-warning .toast-icon { background: linear-gradient(135deg, #f59e0b 0%, #d97706 100%); }
    .toast-info .toast-icon { background: linear-gradient(135deg, #3b82f6 0%, #2563eb 100%); }

    .toast-icon svg { color: #fff; stroke-width: 2.5; }

    .toast-message {
      flex: 1;
      font-size: 14px;
      font-weight: 500;
      line-height: 1.5;
    }

    .toast-success .toast-message { color: #065f46; }
    .toast-error .toast-message { color: #991b1b; }
    .toast-warning .toast-message { color: #92400e; }
    .toast-info .toast-message { color: #1e40af; }

    .toast-close {
      flex-shrink: 0;
      width: 28px;
      height: 28px;
      border: none;
      background: rgba(0, 0, 0, 0.05);
      color: #6b7280;
      font-size: 22px;
      cursor: pointer;
      border-radius: 6px;
    }

    @keyframes toastSlideIn {
      from { transform: translateY(-20px); opacity: 0; }
      to { transform: translateY(0); opacity: 1; }
    }
  `]
})
export class ToastOverlayComponent implements OnInit, OnDestroy {
  toasts: Toast[] = [];
  private toastId = 0;
  private handler = (e: Event) => this.handleToast(e as CustomEvent);

  constructor(private cdr: ChangeDetectorRef, private ngZone: NgZone) {}

  ngOnInit(): void {
    document.addEventListener('app-toast', this.handler);
  }

  ngOnDestroy(): void {
    document.removeEventListener('app-toast', this.handler);
  }

  private handleToast(event: CustomEvent): void {
    const detail = event?.detail;
    if (!detail?.message) return;

    // Run inside Angular zone so change detection picks up the toast
    this.ngZone.run(() => {
      const toast: Toast = {
        id: ++this.toastId,
        message: detail.message,
        type: detail.type || 'info',
        duration: detail.duration ?? 5000
      };
      this.toasts.push(toast);
      this.cdr.detectChanges();

      if (toast.duration && toast.duration > 0) {
        setTimeout(() => this.removeToast(toast.id), toast.duration);
      }
    });
  }

  removeToast(id: number): void {
    this.toasts = this.toasts.filter(t => t.id !== id);
    this.cdr.detectChanges();
  }
}
