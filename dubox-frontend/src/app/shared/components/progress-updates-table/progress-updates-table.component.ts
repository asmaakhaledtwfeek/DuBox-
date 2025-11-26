import { CommonModule } from '@angular/common';
import { Component, EventEmitter, Input, Output } from '@angular/core';
import { ProgressUpdate } from '../../../core/models/progress-update.model';

@Component({
  selector: 'app-progress-updates-table',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './progress-updates-table.component.html',
  styleUrls: ['./progress-updates-table.component.scss']
})
export class ProgressUpdatesTableComponent {
  @Input() updates: ProgressUpdate[] | null = [];
  @Input() loading = false;
  @Input() error = '';
  @Output() refresh = new EventEmitter<void>();
  @Output() viewDetails = new EventEmitter<ProgressUpdate>();

  onRefresh(): void {
    this.refresh.emit();
  }

  trackById(_index: number, item: ProgressUpdate): string | undefined {
    return item.progressUpdateId;
  }

  openDetails(update: ProgressUpdate): void {
    this.viewDetails.emit(update);
  }
}

