import { Component, OnInit, OnDestroy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, RouterModule } from '@angular/router';
import { Subject, takeUntil } from 'rxjs';
import { PublicBoxService, PublicBox } from '../../../core/services/public-box.service';

@Component({
  selector: 'app-public-box-view',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './public-box-view.component.html',
  styleUrls: ['./public-box-view.component.scss']
})
export class PublicBoxViewComponent implements OnInit, OnDestroy {
  box: PublicBox | null = null;
  loading = true;
  error = '';
  boxId = '';
  currentYear = new Date().getFullYear();

  private destroy$ = new Subject<void>();

  constructor(
    private route: ActivatedRoute,
    private publicBoxService: PublicBoxService
  ) {}

  ngOnInit(): void {
    this.boxId = this.route.snapshot.paramMap.get('boxId') || '';
    if (this.boxId) {
      this.loadBox();
    } else {
      this.error = 'Invalid box ID';
      this.loading = false;
    }
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }

  loadBox(): void {
    this.loading = true;
    this.error = '';

    this.publicBoxService.getPublicBox(this.boxId)
      .pipe(takeUntil(this.destroy$))
      .subscribe({
        next: (box) => {
          this.box = box;
          this.loading = false;
        },
        error: (err) => {
          console.error('Error loading box:', err);
          if (err.status === 404) {
            this.error = 'Box not found. The QR code may be invalid or the box has been removed.';
          } else {
            this.error = 'Unable to load box details. Please try again later.';
          }
          this.loading = false;
        }
      });
  }

  getStatusInfo(status: string): { label: string; class: string; color: string } {
    return this.publicBoxService.getStatusInfo(status);
  }

  getProgressColor(progress: number): string {
    if (progress >= 100) return '#10b981';
    if (progress >= 75) return '#3b82f6';
    if (progress >= 50) return '#f59e0b';
    if (progress >= 25) return '#f97316';
    return '#ef4444';
  }

  formatDate(date: Date | undefined): string {
    if (!date) return '—';
    return new Date(date).toLocaleDateString('en-US', {
      year: 'numeric',
      month: 'short',
      day: 'numeric'
    });
  }

  getDimensions(): string {
    if (!this.box) return '—';
    const parts: string[] = [];
    if (this.box.length) parts.push(`L: ${this.box.length}`);
    if (this.box.width) parts.push(`W: ${this.box.width}`);
    if (this.box.height) parts.push(`H: ${this.box.height}`);
    if (parts.length === 0) return '—';
    return parts.join(' × ') + (this.box.unitOfMeasure ? ` ${this.box.unitOfMeasure}` : '');
  }

  getLocationDisplay(): string {
    if (!this.box) return '—';
    const parts: string[] = [];
    if (this.box.factoryName) parts.push(this.box.factoryName);
    if (this.box.currentLocationName) parts.push(this.box.currentLocationName);
    if (this.box.bay) parts.push(`Bay ${this.box.bay}`);
    if (this.box.row) parts.push(`Row ${this.box.row}`);
    if (this.box.position) parts.push(`Pos ${this.box.position}`);
    return parts.length > 0 ? parts.join(' • ') : '—';
  }
}

