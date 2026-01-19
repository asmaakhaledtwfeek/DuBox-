import { Component, Input, OnInit, Output, EventEmitter } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import {
  WirChecklistService,
  WIRWithChecklist,
  ChecklistSection,
  ChecklistItemDetail,
  ReviewWIRRequest
} from '../../../core/services/wir-checklist.service';
import { DetailedFormViewComponent } from './detailed-form-view/detailed-form-view.component';

@Component({
  selector: 'app-dynamic-checklist',
  standalone: true,
  imports: [CommonModule, FormsModule, DetailedFormViewComponent],
  templateUrl: './dynamic-checklist.component.html',
  styleUrls: ['./dynamic-checklist.component.scss']
})
export class DynamicChecklistComponent implements OnInit {
  @Input() boxId!: string;
  @Input() wirNumber?: string; // Optional: Load specific WIR only
  @Input() readonly: boolean = false;
  @Input() boxNumber?: string;
  @Input() location?: string;
  @Output() checklistCompleted = new EventEmitter<void>();

  wirs: WIRWithChecklist[] = [];
  selectedWIR?: WIRWithChecklist;
  loading = false;
  error?: string;
  inspectorRole?: string;
  generalComments?: string;
  viewMode: 'tabs' | 'form' = 'form'; // Default to form view

  constructor(private wirService: WirChecklistService) {}

  ngOnInit(): void {
    this.loadChecklists();
  }

  loadChecklists(): void {
    if (!this.boxId) {
      this.error = 'Box ID is required';
      return;
    }

    this.loading = true;
    this.error = undefined;

    this.wirService.getWIRsByBoxWithChecklist(this.boxId).subscribe({
      next: (response) => {
        if (response.isSuccess) {
          this.wirs = response.data;
          
          // Select first WIR or specific WIR if wirNumber provided
          if (this.wirs.length > 0) {
            if (this.wirNumber) {
              this.selectedWIR = this.wirs.find(w => w.wirNumber === this.wirNumber);
            } else {
              this.selectedWIR = this.wirs[0];
            }
          }
        } else {
          this.error = 'Failed to load checklists';
        }
        this.loading = false;
      },
      error: (err) => {
        this.error = err.error?.message || 'Failed to load checklists';
        this.loading = false;
      }
    });
  }

  selectWIR(wir: WIRWithChecklist): void {
    this.selectedWIR = wir;
  }

  updateItemStatus(item: ChecklistItemDetail, status: 'Pass' | 'Fail' | 'Pending'): void {
    if (this.readonly) return;
    item.status = status;
  }

  get totalProgress(): number {
    if (this.wirs.length === 0) return 0;
    const totalPercentage = this.wirs.reduce((sum, wir) => sum + wir.progressPercentage, 0);
    return Math.round(totalPercentage / this.wirs.length);
  }

  get canSubmit(): boolean {
    if (!this.selectedWIR) return false;
    // Check if all items have been reviewed (Pass or Fail, not Pending)
    return this.selectedWIR.sections.every(section =>
      section.items.every(item => item.status !== 'Pending')
    );
  }

  submitWIR(status: 'Approved' | 'Rejected' | 'ConditionalApproval'): void {
    if (!this.selectedWIR || this.readonly) return;

    const items = this.selectedWIR.sections.flatMap(section =>
      section.items.map(item => ({
        checklistItemId: item.checklistItemId,
        status: item.status,
        remarks: item.remarks
      }))
    );

    const request: ReviewWIRRequest = {
      status,
      comment: this.generalComments,
      inspectorRole: this.inspectorRole,
      items
    };

    this.loading = true;

    this.wirService.reviewWIR(this.selectedWIR.wirId, request).subscribe({
      next: (response) => {
        if (response.isSuccess) {
          alert(`Stage ${this.selectedWIR!.wirNumber} ${status} successfully!`);
          this.loadChecklists(); // Reload to get updated status
          this.checklistCompleted.emit();
        } else {
          alert('Failed to submit WIR: ' + response.message);
        }
        this.loading = false;
      },
      error: (err) => {
        alert('Error submitting WIR: ' + (err.error?.message || err.message));
        this.loading = false;
      }
    });
  }

  getStatusClass(status: string): string {
    switch (status.toLowerCase()) {
      case 'pass':
      case 'approved':
        return 'status-pass';
      case 'fail':
      case 'rejected':
        return 'status-fail';
      case 'pending':
        return 'status-pending';
      default:
        return '';
    }
  }

  getProgressBarClass(percentage: number): string {
    if (percentage >= 80) return 'progress-high';
    if (percentage >= 50) return 'progress-medium';
    return 'progress-low';
  }

  toggleViewMode(): void {
    this.viewMode = this.viewMode === 'tabs' ? 'form' : 'tabs';
  }
}
