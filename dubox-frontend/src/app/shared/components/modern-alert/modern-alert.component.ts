import { Component, EventEmitter, Input, Output } from '@angular/core';
import { CommonModule } from '@angular/common';

export type AlertType = 'success' | 'error' | 'warning' | 'info';

@Component({
  selector: 'app-modern-alert',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './modern-alert.component.html',
  styleUrls: ['./modern-alert.component.scss']
})
export class ModernAlertComponent {
  @Input() show: boolean = false;
  @Input() type: AlertType = 'info';
  @Input() title: string = '';
  @Input() message: string = '';
  @Output() close = new EventEmitter<void>();

  onClose(): void {
    this.close.emit();
  }

  onBackdropClick(event: MouseEvent): void {
    if (event.target === event.currentTarget) {
      this.onClose();
    }
  }
}
