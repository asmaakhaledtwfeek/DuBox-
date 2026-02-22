import { Component, Input, Output, EventEmitter, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ActivityChecklistService, ActivityCheckListItemWithReview, GetActivityChecklistByBoxActivity } from '../../../core/services/activity-checklist.service';

interface GroupedChecklistItem {
  checklistId?: string;
  checklistName?: string;
  checklistCode?: string;
  sections: GroupedSection[];
  isExpanded?: boolean;
}

interface GroupedSection {
  sectionId?: string;
  sectionTitle?: string;
  sectionOrder?: number;
  items: ActivityCheckListItemWithReviewAndDisplaySeq[];
  isExpanded?: boolean;
}

interface ActivityCheckListItemWithReviewAndDisplaySeq extends ActivityCheckListItemWithReview {
  displaySequence?: number;
}

@Component({
  selector: 'app-activity-checklist-review-modal',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './activity-checklist-review-modal.component.html',
  styleUrls: ['./activity-checklist-review-modal.component.scss']
})
export class ActivityChecklistReviewModalComponent implements OnInit {
  @Input() boxActivityId: string = '';
  @Input() activityName: string = '';
  @Input() activityCode: string = '';
  @Input() progressPercentage: number = 0;
  @Output() closeModal = new EventEmitter<void>();
  @Output() reviewSubmitted = new EventEmitter<void>();

  checklistData: GetActivityChecklistByBoxActivity | null = null;
  groupedChecklists: GroupedChecklistItem[] = [];
  loading = false;
  error = '';
  submitting = false;

  constructor(private activityChecklistService: ActivityChecklistService) {}

  ngOnInit(): void {
    this.loadChecklistItems();
  }

  loadChecklistItems(): void {
    if (!this.boxActivityId) {
      this.error = 'Box Activity ID is required';
      return;
    }

    this.loading = true;
    this.error = '';

    this.activityChecklistService.getActivityChecklistByBoxActivity(this.boxActivityId).subscribe({
      next: (response) => {
        if (response.isSuccess && response.data) {
          this.checklistData = response.data;
          
          // Initialize status for items without reviews
          if (this.checklistData?.checklistItems) {
            this.checklistData.checklistItems = this.checklistData.checklistItems.map(item => ({
              ...item,
              reviewStatus: item.reviewStatus || 'Pending'
            }));
            
            // Group items by checklist and section
            this.groupChecklistItems();
          }
        } else {
          this.error = response.message || 'Failed to load checklist items';
        }
        this.loading = false;
      },
      error: (err) => {
        console.error('Error loading checklist items:', err);
        this.error = 'Failed to load checklist items. Please try again.';
        this.loading = false;
      }
    });
  }

  groupChecklistItems(): void {
    if (!this.checklistData?.checklistItems) {
      this.groupedChecklists = [];
      return;
    }

    const checklistsMap = new Map<string, GroupedChecklistItem>();

    this.checklistData.checklistItems.forEach(item => {
      const checklistKey = item.checklistId || 'no-checklist';
      
      // Get or create checklist group
      if (!checklistsMap.has(checklistKey)) {
        checklistsMap.set(checklistKey, {
          checklistId: item.checklistId,
          checklistName: item.checklistName,
          checklistCode: item.checklistCode,
          sections: [],
          isExpanded: false // Collapsed by default
        });
      }

      const checklist = checklistsMap.get(checklistKey)!;
      const sectionKey = item.checklistSectionId || 'no-section';
      
      // Find or create section
      let section = checklist.sections.find(s => (s.sectionId || 'no-section') === sectionKey);
      if (!section) {
        section = {
          sectionId: item.checklistSectionId,
          sectionTitle: item.sectionTitle,
          sectionOrder: item.sectionOrder,
          items: [],
          isExpanded: false // Collapsed by default
        };
        checklist.sections.push(section);
      }

      section.items.push(item as ActivityCheckListItemWithReviewAndDisplaySeq);
    });

    // Convert map to array and sort
    this.groupedChecklists = Array.from(checklistsMap.values());
    
    // Sort sections within each checklist by order
    this.groupedChecklists.forEach(checklist => {
      checklist.sections.sort((a, b) => (a.sectionOrder || 0) - (b.sectionOrder || 0));
      
      // For each section, sort items by original sequence and assign new display sequence
      checklist.sections.forEach(section => {
        // Sort items by original sequence
        section.items.sort((a, b) => a.sequence - b.sequence);
        
        // Assign new display sequence starting from 1
        section.items.forEach((item, index) => {
          item.displaySequence = index + 1;
        });
      });
    });
  }

  toggleChecklist(checklist: GroupedChecklistItem): void {
    checklist.isExpanded = !checklist.isExpanded;
  }

  toggleSection(section: GroupedSection): void {
    section.isExpanded = !section.isExpanded;
  }

  expandAllChecklists(): void {
    this.groupedChecklists.forEach(checklist => {
      checklist.isExpanded = true;
      checklist.sections.forEach(section => {
        section.isExpanded = true;
      });
    });
  }

  collapseAllChecklists(): void {
    this.groupedChecklists.forEach(checklist => {
      checklist.isExpanded = false;
      checklist.sections.forEach(section => {
        section.isExpanded = false;
      });
    });
  }

  onStatusChange(item: ActivityCheckListItemWithReview, status: 'Pending' | 'Pass' | 'Fail' | 'NA'): void {
    item.reviewStatus = status;
  }

  canSubmitReview(): boolean {
    if (!this.checklistData || this.progressPercentage < 100) {
      return false;
    }

    // Check if all mandatory items have been reviewed (not Pending)
    const mandatoryItems = this.checklistData.checklistItems.filter(item => item.isMandatory);
    return mandatoryItems.every(item => item.reviewStatus !== 'Pending');
  }

  getProgressWarningMessage(): string {
    if (this.progressPercentage < 100) {
      return `Activity checklist can only be reviewed after progress reaches 100%. Current progress: ${this.progressPercentage}%`;
    }
    return '';
  }

  onSubmit(): void {
    if (!this.canSubmitReview() || !this.checklistData) {
      return;
    }

    this.submitting = true;
    this.error = '';

    const reviews = this.checklistData.checklistItems.map(item => ({
      boxActivityId: this.boxActivityId,
      activityCheckListItemId: item.activityCheckListItemId,
      status: item.reviewStatus,
      remarks: item.remarks || ''
    }));

    this.activityChecklistService.submitChecklistReview({
      boxActivityId: this.boxActivityId,
      reviews
    }).subscribe({
      next: (response) => {
        if (response.isSuccess) {
          this.reviewSubmitted.emit();
          this.close();
        } else {
          this.error = response.message || 'Failed to submit review';
        }
        this.submitting = false;
      },
      error: (err) => {
        console.error('Error submitting review:', err);
        this.error = err.error?.message || 'Failed to submit review. Please try again.';
        this.submitting = false;
      }
    });
  }

  /**
   * Approve all checklist items
   */
  approveAll(): void {
    if (!this.checklistData || this.progressPercentage < 100) {
      return;
    }

    this.checklistData.checklistItems.forEach(item => {
      item.reviewStatus = 'Pass';
    });
  }

  /**
   * Reject all checklist items
   */
  rejectAll(): void {
    if (!this.checklistData || this.progressPercentage < 100) {
      return;
    }

    this.checklistData.checklistItems.forEach(item => {
      item.reviewStatus = 'Fail';
    });
  }

  /**
   * Approve all items in a specific section
   */
  approveSectionAll(section: GroupedSection): void {
    if (this.progressPercentage < 100) {
      return;
    }

    section.items.forEach(item => {
      item.reviewStatus = 'Pass';
    });
  }

  /**
   * Reject all items in a specific section
   */
  rejectSectionAll(section: GroupedSection): void {
    if (this.progressPercentage < 100) {
      return;
    }

    section.items.forEach(item => {
      item.reviewStatus = 'Fail';
    });
  }

  close(): void {
    this.closeModal.emit();
  }

  getStatusBadgeClass(status: string): string {
    switch (status) {
      case 'Pass':
        return 'status-pass';
      case 'Fail':
        return 'status-fail';
      case 'NA':
        return 'status-na';
      case 'Pending':
      default:
        return 'status-pending';
    }
  }

  getCompletionPercentage(): number {
    if (!this.checklistData || this.checklistData.checklistItems.length === 0) {
      return 0;
    }

    const reviewedCount = this.checklistData.checklistItems.filter(
      item => item.reviewStatus !== 'Pending'
    ).length;

    return Math.round((reviewedCount / this.checklistData.checklistItems.length) * 100);
  }
}

