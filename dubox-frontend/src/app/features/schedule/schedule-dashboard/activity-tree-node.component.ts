import { Component, Input, OnChanges, Output, EventEmitter, ChangeDetectionStrategy, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

export interface ActivityTreeNode {
  scheduleActivityId: string;
  activityName: string;
  activityCode: string;
  plannedStartDate: string;
  plannedFinishDate: string;
  actualStartDate?: string;
  actualFinishDate?: string;
  status: string;
  percentComplete: number;
  weight: number;
  teamCount: number;
  materialCount: number;
  parentActivityId?: string;
  children?: ActivityTreeNode[];
  expanded?: boolean; // UI state for expand/collapse
  visible?: boolean; // UI state for visibility filtering
}

@Component({
  selector: 'app-activity-tree-node',
  standalone: true,
  imports: [CommonModule, FormsModule],
  changeDetection: ChangeDetectionStrategy.OnPush,
  template: `
    <div class="activity-node" 
         [class.hidden]="!isActivityVisible()">
      <!-- Current Activity Details -->
      <div class="activity-row" 
           [class.ghost-row]="isAncestorOfSelected && !isSelected()"
           (click)="toggleExpand()">
        <!-- Activity Name Column with Indentation -->
        <div class="activity-name-column" [style.padding-left.px]="(currentLevel - 1) * 24">
          <!-- Checkbox -->
          <div class="checkbox-control" (click)="$event.stopPropagation()">
            <input 
              type="checkbox" 
              [checked]="isSelected()"
              (change)="onCheckboxChange($event)"
              class="activity-checkbox">
          </div>

          <!-- Expand/Collapse Button or Spacer -->
          <div class="expand-control">
            <button 
              *ngIf="hasChildren()" 
              class="expand-btn"
              [class.expanded]="isExpanded"
              type="button">
              <svg width="12" height="12" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="3">
                <polyline points="9 18 15 12 9 6"/>
              </svg>
            </button>
            <div *ngIf="!hasChildren()" class="spacer"></div>
          </div>

          <!-- Activity Icon -->
          <div class="activity-icon" [class.has-children]="hasChildren()">
            <svg *ngIf="hasChildren()" width="16" height="16" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2">
              <path d="M22 19a2 2 0 0 1-2 2H4a2 2 0 0 1-2-2V5a2 2 0 0 1 2-2h5l2 3h9a2 2 0 0 1 2 2z"/>
            </svg>
            <svg *ngIf="!hasChildren()" width="16" height="16" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2">
              <path d="M14 2H6a2 2 0 0 0-2 2v16a2 2 0 0 0 2 2h12a2 2 0 0 0 2-2V8z"/>
              <polyline points="14 2 14 8 20 8"/>
            </svg>
          </div>

          <!-- Activity Details -->
          <div class="activity-details">
            <span class="activity-name">{{ activity.activityName }}</span>
            <span class="activity-code">{{ activity.activityCode }}</span>
          </div>
        </div>

        <!-- Planned Dates (moved before status) -->
        <div class="dates-column">
          <span class="date-item" title="Planned Start">
            <svg width="14" height="14" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2">
              <rect x="3" y="4" width="18" height="18" rx="2" ry="2"/>
              <line x1="16" y1="2" x2="16" y2="6"/>
              <line x1="8" y1="2" x2="8" y2="6"/>
              <line x1="3" y1="10" x2="21" y2="10"/>
            </svg>
            {{ formatDate(activity.plannedStartDate) }}
          </span>
          <span class="date-separator">→</span>
          <span class="date-item" title="Planned Finish">
            <svg width="14" height="14" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2">
              <rect x="3" y="4" width="18" height="18" rx="2" ry="2"/>
              <line x1="16" y1="2" x2="16" y2="6"/>
              <line x1="8" y1="2" x2="8" y2="6"/>
              <line x1="3" y1="10" x2="21" y2="10"/>
              <polyline points="10 14 14 18 18 14"/>
            </svg>
            {{ formatDate(activity.plannedFinishDate) }}
          </span>
        </div>

        <!-- Status Badge -->
        <div class="status-column">
          <span class="status-badge" [class]="'status-' + getStatusClass()">
            {{ activity.status }}
          </span>
        </div>

        <!-- Progress -->
        <div class="progress-column">
          <div class="progress-mini">
            <div class="progress-bar">
              <div class="progress-fill" [style.width.%]="activity.percentComplete"></div>
            </div>
            <span class="progress-text">{{ activity.percentComplete }}%</span>
          </div>
          <button 
            class="btn-edit-progress" 
            (click)="onEditProgress($event)" 
            title="Update Progress"
            type="button">
            <svg width="14" height="14" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2">
              <path d="M11 4H4a2 2 0 0 0-2 2v14a2 2 0 0 0 2 2h14a2 2 0 0 0 2-2v-7"/>
              <path d="M18.5 2.5a2.121 2.121 0 0 1 3 3L12 15l-4 1 1-4 9.5-9.5z"/>
            </svg>
          </button>
        </div>

        <!-- Actual Dates (new column) -->
        <div class="dates-column actual-dates-column">
          <div class="dates-content">
            <!-- Start date exists -->
            <span class="date-item" title="Actual Start" *ngIf="activity.actualStartDate">
              <svg width="14" height="14" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2">
                <rect x="3" y="4" width="18" height="18" rx="2" ry="2"/>
                <line x1="16" y1="2" x2="16" y2="6"/>
                <line x1="8" y1="2" x2="8" y2="6"/>
                <line x1="3" y1="10" x2="21" y2="10"/>
              </svg>
              {{ formatDate(activity.actualStartDate) }}
            </span>
            
            <!-- Arrow separator -->
            <span class="date-separator" *ngIf="activity.actualStartDate">→</span>
            
            <!-- Finish date exists -->
            <span class="date-item" title="Actual Finish" *ngIf="activity.actualFinishDate">
              <svg width="14" height="14" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2">
                <rect x="3" y="4" width="18" height="18" rx="2" ry="2"/>
                <line x1="16" y1="2" x2="16" y2="6"/>
                <line x1="8" y1="2" x2="8" y2="6"/>
                <line x1="3" y1="10" x2="21" y2="10"/>
                <polyline points="10 14 14 18 18 14"/>
              </svg>
              {{ formatDate(activity.actualFinishDate) }}
            </span>
            
            <!-- Start exists but finish doesn't - show "Ongoing" -->
            <span class="date-item ongoing" title="In Progress - Not yet finished" *ngIf="activity.actualStartDate && !activity.actualFinishDate">
              <svg width="14" height="14" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2">
                <circle cx="12" cy="12" r="10"/>
                <polyline points="12 6 12 12 16 14"/>
              </svg>
              Ongoing
            </span>
            
            <!-- No dates at all -->
            <span class="no-data" *ngIf="!activity.actualStartDate && !activity.actualFinishDate">-</span>
          </div>
          <button 
            class="btn-edit-dates" 
            (click)="onEditDates($event)" 
            title="Update Actual Dates"
            type="button">
            <svg width="14" height="14" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2">
              <rect x="3" y="4" width="18" height="18" rx="2" ry="2"/>
              <line x1="16" y1="2" x2="16" y2="6"/>
              <line x1="8" y1="2" x2="8" y2="6"/>
              <line x1="3" y1="10" x2="21" y2="10"/>
              <path d="M8 14h.01M12 14h.01M16 14h.01M8 18h.01M12 18h.01M16 18h.01"/>
            </svg>
          </button>
        </div>
      </div>

      <!-- Children (Recursive) -->
      <div *ngIf="shouldShowChildren()" class="children-container">
        <app-activity-tree-node
          *ngFor="let child of activity.children; trackBy: trackByChildId"
          [activity]="child"
          [selectedLevel]="selectedLevel"
          [currentLevel]="currentLevel + 1"
          [expandAllState]="expandAllState"
          [updateTrigger]="updateTrigger"
          [focusedViewActive]="focusedViewActive"
          [selectedActivityIds]="selectedActivityIds"
          [isInSelectedBranch]="isSelected() || isInSelectedBranch"
          [isAncestorOfSelected]="isChildAncestorOfSelected(child)"
          (selectionChange)="selectionChange.emit($event)"
          (editProgress)="editProgress.emit($event)"
          (editDates)="editDates.emit($event)">
        </app-activity-tree-node>
      </div>
    </div>
  `,
  styles: [`
    .activity-node {
      position: relative;
      display: block;
      width: 100%;
      /* Performance optimizations */
      contain: content;
      will-change: transform;
    }

    .activity-node.hidden {
      display: none;
    }

    .activity-row {
      display: grid;
      grid-template-columns: minmax(250px, 2fr) minmax(200px, 1.6fr) minmax(90px, 0.6fr) minmax(210px, 1.7fr) minmax(200px, 1.6fr);
      align-items: center;
      gap: 18px;
      padding: 16px 20px;
      min-height: 72px;
      background: white;
      border-bottom: 1px solid #e2e8f0;
      border-left: 3px solid transparent;
      cursor: pointer;
      transition: all 0.2s ease;
      /* Performance optimizations */
      contain: layout paint;
      transform: translateZ(0);
      position: relative;
      width: 100%;
      
      &::before {
        content: '';
        position: absolute;
        left: 0;
        top: 0;
        bottom: 0;
        width: 0;
        background: linear-gradient(90deg, rgba(27, 154, 170, 0.1) 0%, transparent 100%);
        transition: width 0.3s ease;
      }
    }

    .activity-row.ghost-row {
      opacity: 0.5;
      background: #fafbfc;
      font-style: italic;
    }

    .activity-name-column {
      display: flex;
      align-items: center;
      gap: 8px;
      min-width: 0;
      /* Removed transition for better performance */
      /* transition: padding-left 0.2s ease; */
    }

    .checkbox-control {
      display: flex;
      align-items: center;
      flex-shrink: 0;
    }

    .activity-checkbox {
      width: 18px;
      height: 18px;
      cursor: pointer;
      border: 2px solid #cbd5e1;
      border-radius: 5px;
      transition: all 0.3s ease;
      appearance: none;
      position: relative;
      background: white;

      &:hover {
        border-color: #1b9aaa;
        box-shadow: 0 0 0 3px rgba(27, 154, 170, 0.1);
      }

      &:checked {
        background: linear-gradient(135deg, #1b9aaa 0%, #2563eb 100%);
        border-color: #1b9aaa;
        box-shadow: 0 2px 6px rgba(27, 154, 170, 0.3);
        
        &::after {
          content: '✓';
          position: absolute;
          top: 50%;
          left: 50%;
          transform: translate(-50%, -50%);
          color: white;
          font-size: 12px;
          font-weight: bold;
        }
      }
    }

    .expand-control {
      width: 20px;
      height: 20px;
      display: flex;
      align-items: center;
      justify-content: center;
      flex-shrink: 0;
    }

    .expand-btn {
      width: 20px;
      height: 20px;
      padding: 0;
      background: #f1f5f9;
      border: none;
      border-radius: 4px;
      cursor: pointer;
      display: flex;
      align-items: center;
      justify-content: center;
      transition: all 0.2s ease;
    }

    .expand-btn:hover {
      background: #e2e8f0;
      transform: scale(1.1);
    }

    .expand-btn svg {
      color: #64748b;
      transition: transform 0.2s ease;
    }

    .expand-btn.expanded {
      background: rgba(27, 154, 170, 0.1);
    }

    .expand-btn.expanded svg {
      transform: rotate(90deg);
      color: #1b9aaa;
    }

    .spacer {
      width: 20px;
      height: 20px;
    }

    .activity-icon {
      width: 34px;
      height: 34px;
      border-radius: 8px;
      background: linear-gradient(135deg, #f1f5f9 0%, #e2e8f0 100%);
      display: flex;
      align-items: center;
      justify-content: center;
      flex-shrink: 0;
      border: 1px solid #e2e8f0;
      transition: all 0.3s ease;
    }

    .activity-icon svg {
      color: #64748b;
      transition: color 0.3s ease;
    }

    .activity-icon.has-children {
      background: linear-gradient(135deg, #1b9aaa 0%, #2563eb 100%);
      border: 1px solid #1b9aaa;
      box-shadow: 0 2px 6px rgba(27, 154, 170, 0.25);
    }

    .activity-icon.has-children svg {
      color: white;
    }
    
    .activity-details {
      display: flex;
      flex-direction: column;
      align-items: flex-start;
      gap: 6px;
      min-width: 0;
    }

    .activity-name {
      font-size: 14px;
      font-weight: 600;
      color: #1e293b;
      overflow: hidden;
      text-overflow: ellipsis;
      white-space: nowrap;
      width: 100%;
    }

    .activity-code {
      padding: 3px 10px;
      background: linear-gradient(135deg, #f1f5f9 0%, #e2e8f0 100%);
      color: #475569;
      border-radius: 6px;
      font-size: 11px;
      font-family: 'Courier New', monospace;
      font-weight: 700;
      white-space: nowrap;
      border: 1px solid #e2e8f0;
    }

    .dates-column {
      display: flex;
      align-items: center;
      justify-content: center;
      gap: 10px;
      flex-wrap: nowrap;
      padding: 0 8px;
    }

    .status-column {
      display: flex;
      align-items: center;
      justify-content: center;
      padding: 0 8px;
    }

    .progress-column {
      display: flex;
      align-items: center;
      justify-content: center;
      gap: 12px;
      padding: 0 12px;
    }

    .status-badge {
      padding: 9px 18px;
      border-radius: 18px;
      font-size: 12px;
      font-weight: 900;
      text-transform: uppercase;
      letter-spacing: 1px;
      white-space: nowrap;
      border: 2px solid transparent;
      transition: all 0.25s cubic-bezier(0.4, 0, 0.2, 1);
      box-shadow: 0 2px 6px rgba(0, 0, 0, 0.1), 0 1px 3px rgba(0, 0, 0, 0.06);
      text-shadow: 0 1px 2px rgba(0, 0, 0, 0.05);
    }

    .status-planned {
      background: linear-gradient(135deg, #dbeafe 0%, #bfdbfe 100%);
      color: #1e40af;
      border-color: #60a5fa;
    }

    .status-in-progress, .status-in_progress {
      background: linear-gradient(135deg, #fed7aa 0%, #fdba74 100%);
      color: #9a3412;
      border-color: #fb923c;
    }

    .status-completed {
      background: linear-gradient(135deg, #a7f3d0 0%, #6ee7b7 100%);
      color: #065f46;
      border-color: #34d399;
    }

    .status-on-hold, .status-on_hold {
      background: linear-gradient(135deg, #fecaca 0%, #fca5a5 100%);
      color: #991b1b;
      border-color: #f87171;
    }
    

    .progress-mini {
      display: flex;
      align-items: center;
      gap: 14px;
      width: 100%;
      padding: 4px 0;
    }

    .progress-bar {
      flex: 1;
      min-width: 130px;
      height: 20px;
      background: linear-gradient(135deg, #f1f5f9 0%, #e2e8f0 100%);
      border-radius: 14px;
      overflow: hidden;
      box-shadow: inset 0 3px 6px rgba(0, 0, 0, 0.15), 0 1px 2px rgba(0, 0, 0, 0.05);
      border: 2px solid #cbd5e1;
      position: relative;
      transition: all 0.25s cubic-bezier(0.4, 0, 0.2, 1);
      
      &::after {
        content: '';
        position: absolute;
        top: 0;
        left: 0;
        right: 0;
        height: 50%;
        background: linear-gradient(180deg, rgba(255, 255, 255, 0.5) 0%, transparent 100%);
        border-radius: 14px 14px 0 0;
      }
    }
    
    .progress-fill {
      height: 100%;
      background: linear-gradient(90deg, #1b9aaa 0%, #2563eb 60%, #3b82f6 100%);
      transition: width 0.4s cubic-bezier(0.4, 0, 0.2, 1);
      box-shadow: 0 2px 8px rgba(27, 154, 170, 0.5);
      position: relative;
      
      &::before {
        content: '';
        position: absolute;
        top: 0;
        left: 0;
        right: 0;
        height: 50%;
        background: linear-gradient(180deg, rgba(255, 255, 255, 0.4) 0%, transparent 100%);
      }
    }

    .progress-text {
      font-size: 15px;
      font-weight: 900;
      color: #1e293b;
      min-width: 52px;
      text-align: right;
      background: linear-gradient(135deg, #1b9aaa 0%, #2563eb 70%, #3b82f6 100%);
      -webkit-background-clip: text;
      -webkit-text-fill-color: transparent;
      background-clip: text;
      letter-spacing: 0.5px;
      text-shadow: 0 1px 2px rgba(0, 0, 0, 0.05);
    }

    .btn-edit-progress {
      width: 28px;
      height: 28px;
      padding: 0;
      background: linear-gradient(135deg, #ffffff 0%, #f8fafc 100%);
      border: 2px solid #cbd5e1;
      border-radius: 8px;
      cursor: pointer;
      display: flex;
      align-items: center;
      justify-content: center;
      transition: all 0.25s cubic-bezier(0.4, 0, 0.2, 1);
      opacity: 1;
      margin-left: 6px;
      box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1), 0 1px 2px rgba(0, 0, 0, 0.06);
    }

    .btn-edit-progress:hover {
      background: linear-gradient(135deg, #1b9aaa 0%, #2563eb 100%);
      border-color: #2563eb;
      transform: scale(1.15) translateY(-1px);
      box-shadow: 0 4px 10px rgba(27, 154, 170, 0.4), 0 2px 5px rgba(27, 154, 170, 0.2);
    }

    .btn-edit-progress:hover svg {
      color: white;
    }

    .btn-edit-progress svg {
      color: #64748b;
      transition: color 0.2s ease;
    }

    .actual-dates-column {
      display: flex;
      align-items: center;
      justify-content: center;
      gap: 10px;
      padding: 0 8px;
    }

    .dates-content {
      display: flex;
      align-items: center;
      gap: 8px;
      flex-wrap: wrap;
    }

    .btn-edit-dates {
      width: 28px;
      height: 28px;
      padding: 0;
      background: linear-gradient(135deg, #ffffff 0%, #f8fafc 100%);
      border: 2px solid #cbd5e1;
      border-radius: 8px;
      cursor: pointer;
      display: flex;
      align-items: center;
      justify-content: center;
      transition: all 0.25s cubic-bezier(0.4, 0, 0.2, 1);
      opacity: 1;
      flex-shrink: 0;
      box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1), 0 1px 2px rgba(0, 0, 0, 0.06);
    }

    .btn-edit-dates:hover {
      background: linear-gradient(135deg, #2563eb 0%, #3b82f6 100%);
      border-color: #2563eb;
      transform: scale(1.15) translateY(-1px);
      box-shadow: 0 4px 10px rgba(37, 99, 235, 0.4), 0 2px 5px rgba(37, 99, 235, 0.2);
    }

    .btn-edit-dates:hover svg {
      color: white;
    }

    .btn-edit-dates svg {
      color: #64748b;
      transition: color 0.2s ease;
    }

    .date-item {
      display: flex;
      align-items: center;
      gap: 7px;
      font-size: 13px;
      color: #0f172a;
      font-weight: 700;
      padding: 8px 14px;
      background: linear-gradient(135deg, #ffffff 0%, #f8fafc 100%);
      border-radius: 10px;
      white-space: nowrap;
      border: 2px solid #cbd5e1;
      transition: all 0.25s cubic-bezier(0.4, 0, 0.2, 1);
      box-shadow: 0 2px 4px rgba(0, 0, 0, 0.06), 0 1px 2px rgba(0, 0, 0, 0.03);
      letter-spacing: 0.3px;
    }
    
    .date-item svg {
      color: #2563eb;
      flex-shrink: 0;
      width: 16px;
      height: 16px;
      filter: drop-shadow(0 1px 2px rgba(37, 99, 235, 0.2));
    }

    .date-item.ongoing {
      background: #fef3c7;
      color: #92400e;
      border: 1px solid #fbbf24;
      font-weight: 600;
      animation: pulse-ongoing 2s ease-in-out infinite;
    }

    .date-item.ongoing svg {
      color: #f59e0b;
      animation: rotate-clock 3s linear infinite;
    }

    @keyframes pulse-ongoing {
      0%, 100% { opacity: 1; }
      50% { opacity: 0.8; }
    }

    @keyframes rotate-clock {
      0% { transform: rotate(0deg); }
      100% { transform: rotate(360deg); }
    }

    .date-separator {
      color: #64748b;
      font-weight: 800;
      font-size: 16px;
      margin: 0 6px;
      flex-shrink: 0;
      text-shadow: 0 1px 2px rgba(0, 0, 0, 0.05);
    }

    .no-data {
      color: #cbd5e1;
      font-size: 14px;
      text-align: center;
      width: 100%;
    }

    /* Children container - no margin to keep columns aligned */
    .children-container {
      position: relative;
      display: block;
      width: 100%;
      /* Performance optimizations */
      contain: content;
    }

    /* Responsive adjustments */
    @media (max-width: 1400px) and (min-width: 1367px) {
      .activity-row {
        grid-template-columns: minmax(220px, 2fr) minmax(170px, 1.4fr) minmax(75px, 0.5fr) minmax(180px, 1.5fr) minmax(170px, 1.4fr);
        gap: 14px;
      }

      .progress-bar {
        min-width: 115px;
        height: 18px;
      }

      .date-item {
        font-size: 12px;
        padding: 7px 12px;
        border: 2px solid #cbd5e1;
        font-weight: 700;
        
        svg {
          width: 15px;
          height: 15px;
        }
      }
      
      .status-badge {
        font-size: 11px;
        padding: 8px 15px;
        font-weight: 900;
        border: 2px solid transparent;
      }
      
      .progress-text {
        font-size: 14px;
        font-weight: 900;
      }
      
      .date-separator {
        font-size: 15px;
        font-weight: 800;
      }
    }

    // iPad/Tablet responsive - Optimized for iPad Pro
    @media (max-width: 1366px) and (min-width: 769px) {
      .activity-node {
        width: 100%;
        min-width: 950px;
        contain: none;
        overflow: visible;
      }

      .children-container {
        width: 100%;
        min-width: 950px;
      }

      .activity-row {
        grid-template-columns: 220px 180px 80px 180px 170px;
        gap: 14px;
        padding: 16px 18px;
        min-height: 60px;
        align-items: center;
        width: 100%;
        min-width: 950px;
      }

      .activity-name-column {
        gap: 8px;
        position: relative;
        z-index: 1;

        .activity-details {
          overflow: hidden;
          
          .activity-name {
            font-size: 14px;
            line-height: 1.5;
            overflow: hidden;
            text-overflow: ellipsis;
            white-space: nowrap;
            font-weight: 600;
            max-width: 100%;
          }

          .activity-code {
            font-size: 11px;
            padding: 3px 8px;
            font-weight: 700;
            white-space: nowrap;
            overflow: hidden;
            text-overflow: ellipsis;
            max-width: 100%;
          }
        }

        .expand-control {
          button {
            width: 22px;
            height: 22px;
            padding: 3px;

            svg {
              width: 12px;
              height: 12px;
            }
          }

          .spacer {
            width: 22px;
            height: 22px;
          }
        }

        .activity-icon {
          width: 28px;
          height: 28px;

          svg {
            width: 15px;
            height: 15px;
          }
        }

        .checkbox-control {
          input {
            width: 16px;
            height: 16px;
          }
        }
      }

      .dates-column {
        gap: 10px;
        flex-wrap: wrap;
        justify-content: center;
        padding: 0 8px;
        position: relative;
        z-index: 1;

        .date-item {
          font-size: 11px;
          padding: 6px 10px;
          white-space: nowrap;
          border: 2px solid #cbd5e1;
          font-weight: 700;
          overflow: hidden;
          text-overflow: ellipsis;
          max-width: 100%;

          svg {
            width: 13px;
            height: 13px;
            flex-shrink: 0;
          }
        }

        .date-separator {
          font-size: 14px;
          margin: 0 4px;
          flex-shrink: 0;
          font-weight: 800;
        }
      }

      .status-column {
        position: relative;
        z-index: 1;
        
        .status-badge {
          padding: 7px 12px;
          font-size: 11px;
          font-weight: 900;
          border: 2px solid transparent;
          white-space: nowrap;
          overflow: hidden;
          text-overflow: ellipsis;
          max-width: 100%;
        }
      }

      .progress-column {
        gap: 10px;
        padding: 0 10px;
        position: relative;
        z-index: 1;
        
        .progress-mini {
          gap: 12px;
          flex-wrap: nowrap;
        }
        
        .progress-bar {
          min-width: 100px;
          height: 18px;
          border: 2px solid #cbd5e1;
        }

        .progress-text {
          font-size: 14px;
          font-weight: 900;
          min-width: 45px;
          white-space: nowrap;
        }
        
        .btn-edit-progress {
          width: 26px;
          height: 26px;
          border: 2px solid #cbd5e1;
          flex-shrink: 0;
          
          svg {
            width: 14px;
            height: 14px;
          }
        }
      }

      .actual-dates-column {
        padding: 0 8px;
        gap: 10px;
        position: relative;
        z-index: 1;
        
        .dates-content {
          flex-wrap: wrap;
        }
        
        .btn-edit-dates {
          width: 26px;
          height: 26px;
          border: 2px solid #cbd5e1;
          flex-shrink: 0;
          
          svg {
            width: 14px;
            height: 14px;
          }
        }
      }
    }

    @media (max-width: 768px) {
      .activity-row {
        display: grid !important;
        grid-template-columns: 1fr 1fr !important;
        grid-auto-rows: auto !important;
        gap: 10px;
        padding: 12px;
        background: white;
        border-radius: 8px;
        margin-bottom: 8px;
        box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1);
        cursor: default;
        border: 1px solid #e2e8f0;
      }

      .activity-name-column {
        grid-column: 1 / -1;
        width: 100%;
        min-width: 0;
        padding-left: 0 !important;
        gap: 8px;
        display: flex;
        align-items: flex-start;
        overflow: hidden;
      }

      .activity-icon {
        width: 32px;
        height: 32px;
        border-radius: 6px;
        flex-shrink: 0;
        
        svg {
          width: 16px;
          height: 16px;
        }
      }

      .activity-details {
        gap: 4px;
        flex: 1;
        min-width: 0;
        overflow: hidden;
        display: flex;
        flex-direction: column;
      }

      .activity-name {
        font-size: 14px;
        font-weight: 700;
        line-height: 1.4;
        color: #1e293b;
        word-wrap: break-word;
        word-break: break-word;
        white-space: normal;
        display: block;
      }

      .activity-code {
        font-size: 11px;
        padding: 3px 8px;
        word-wrap: break-word;
        word-break: break-all;
        white-space: normal;
        max-width: fit-content;
        background: #e2e8f0;
        border-radius: 4px;
      }

      // Mobile info rows with labels
      .dates-column {
        grid-column: 1 / -1;
        width: 100%;
        display: flex;
        flex-direction: column;
        align-items: stretch;
        padding: 10px;
        background: #eff6ff;
        border-radius: 6px;
        border-left: 3px solid #3b82f6;
        gap: 4px;

        &::before {
          content: 'PLANNED';
          display: block;
          font-weight: 800;
          color: #475569;
          font-size: 10px;
          text-transform: uppercase;
          letter-spacing: 0.5px;
          margin-bottom: 4px;
        }

        .date-item {
          font-size: 12px;
          display: flex;
          align-items: center;
          gap: 6px;
          flex-wrap: wrap;
        }

        .date-separator {
          font-size: 12px;
          margin: 0 4px;
        }

        .date-item svg {
          width: 13px;
          height: 13px;
          flex-shrink: 0;
        }
      }

      .status-column {
        grid-column: 1 / 2;
        display: flex;
        flex-direction: column;
        align-items: stretch;
        padding: 10px;
        background: #f0fdf4;
        border-radius: 6px;
        border-left: 3px solid #10b981;
        gap: 4px;
        min-height: 100px;

        &::before {
          content: 'STATUS';
          display: block;
          font-weight: 800;
          color: #475569;
          font-size: 10px;
          text-transform: uppercase;
          letter-spacing: 0.5px;
          margin-bottom: 4px;
        }

        .status-badge {
          font-size: 11px;
          padding: 6px 12px;
          align-self: flex-start;
          font-weight: 700;
          word-wrap: break-word;
          word-break: break-word;
          white-space: normal;
          text-align: center;
        }
      }

      .progress-column {
        grid-column: 2 / 3;
        display: flex;
        flex-direction: column;
        align-items: stretch;
        padding: 10px;
        background: #fef3c7;
        border-radius: 6px;
        border-left: 3px solid #f59e0b;
        gap: 4px;

        &::before {
          content: 'PROGRESS';
          display: block;
          font-weight: 800;
          color: #475569;
          font-size: 10px;
          text-transform: uppercase;
          letter-spacing: 0.5px;
          margin-bottom: 4px;
        }

        .progress-mini {
          width: 100%;
          max-width: none;
          flex-direction: column;
          align-items: stretch;
          gap: 6px;
          flex: 1;
        }

        .progress-bar {
          width: 100%;
          height: 10px;
          border-radius: 5px;
          background: rgba(255, 255, 255, 0.7);
        }

        .progress-fill {
          height: 100%;
          border-radius: 5px;
        }

        .progress-text {
          font-size: 13px;
          font-weight: 800;
          color: #78350f;
          text-align: center;
        }

        .btn-edit-progress {
          padding: 6px;
          background: white;
          border-radius: 4px;
          align-self: center;
          margin-top: 4px;
          
          svg {
            width: 14px;
            height: 14px;
          }
        }
      }

      .actual-dates-column {
        grid-column: 1 / -1;
        width: 100%;
        display: flex;
        flex-direction: column;
        align-items: stretch;
        padding: 10px;
        background: #f5f3ff;
        border-radius: 6px;
        border-left: 3px solid #8b5cf6;
        gap: 4px;

        &::before {
          content: 'ACTUAL';
          display: block;
          font-weight: 800;
          color: #475569;
          font-size: 10px;
          text-transform: uppercase;
          letter-spacing: 0.5px;
          margin-bottom: 4px;
        }

        .dates-content {
          width: 100%;
          flex-wrap: wrap;
          gap: 4px;
          justify-content: flex-start;
          display: flex;
          align-items: center;

          .date-item {
            font-size: 12px;
            display: flex;
            align-items: center;
            gap: 6px;
            flex-wrap: wrap;
            
            svg {
              width: 13px;
              height: 13px;
              flex-shrink: 0;
            }
          }

          .date-separator {
            font-size: 12px;
            margin: 0 4px;
          }

          .no-data {
            font-size: 12px;
            color: #94a3b8;
          }
        }

        .btn-edit-dates {
          margin-left: auto;
          margin-top: 4px;
          padding: 6px;
          background: white;
          border-radius: 4px;
          
          svg {
            width: 14px;
            height: 14px;
          }
        }
      }

      .children-container {
        margin-left: 20px;
        margin-top: 8px;
        padding-top: 8px;
        border-top: 2px solid #e2e8f0;
      }

      // Reduce expand button size
      .expand-btn {
        width: 22px;
        height: 22px;
        flex-shrink: 0;
        
        svg {
          width: 12px;
          height: 12px;
        }
      }

      .expand-control {
        width: 22px;
        flex-shrink: 0;
      }

      .activity-checkbox {
        width: 16px;
        height: 16px;
        flex-shrink: 0;
      }

      .checkbox-control {
        width: 16px;
        flex-shrink: 0;
      }

      .spacer {
        width: 22px;
        height: 22px;
        flex-shrink: 0;
      }

      .date-separator {
        font-size: 12px;
        margin: 0 4px;
      }
    }
  `]
})
export class ActivityTreeNodeComponent implements OnChanges {
  @Input() activity!: ActivityTreeNode;
  @Input() selectedLevel: number = 0; // 0 means show all levels
  @Input() currentLevel: number = 1; // Current depth level
  @Input() expandAllState: boolean = false; // Global expand all state
  @Input() updateTrigger: number = 0; // Trigger for forcing updates
  @Input() focusedViewActive: boolean = false; // Whether focused view is active
  @Input() selectedActivityIds: Set<string> = new Set<string>(); // Selected activity IDs
  @Input() isInSelectedBranch: boolean = false; // Whether this activity is in a selected branch
  @Input() isAncestorOfSelected: boolean = false; // Whether this activity is an ancestor of a selected activity
  
  @Output() selectionChange = new EventEmitter<{ activityId: string, selected: boolean }>();
  @Output() editProgress = new EventEmitter<ActivityTreeNode>();
  @Output() editDates = new EventEmitter<ActivityTreeNode>();
  
  // Manage expand/collapse state in this component
  private _isExpanded: boolean = false;
  
  // Cache expensive computations (only cache things that don't change frequently)
  private _cachedHasChildren: boolean = false;
  private _cachedIsSelected: boolean = false;
  private _cachedIsVisible: boolean = true;
  private _cachedShouldShowChildren: boolean = false;
  
  constructor(private cdr: ChangeDetectorRef) {}

  get isExpanded(): boolean {
    // If activity has an expanded property, use it; otherwise use internal state
    return this.activity?.expanded !== undefined ? this.activity.expanded : this._isExpanded;
  }

  set isExpanded(value: boolean) {
    this._isExpanded = value;
    // Also update the activity's expanded property
    if (this.activity) {
      this.activity.expanded = value;
    }
  }

  ngOnChanges(): void {
    // Recalculate cached values (only cache expensive operations)
    this._cachedHasChildren = !!(this.activity.children && this.activity.children.length > 0);
    this._cachedIsSelected = this.selectedActivityIds.has(this.activity.scheduleActivityId);
    
    // Cache visibility check
    this._cachedIsVisible = this.computeIsVisible();
    
    // Sync with activity's expanded state if it exists (highest priority)
    if (this.activity?.expanded !== undefined) {
      this._isExpanded = this.activity.expanded;
    } else if (this._cachedHasChildren) {
      // Update expand state based on expandAllState input only if no explicit state
      this.isExpanded = this.expandAllState;
    }
    
    // Auto-expand when in focused view and this activity is selected or an ancestor
    if (this.focusedViewActive && this._cachedHasChildren) {
      if (this._cachedIsSelected || this.isAncestorOfSelected) {
        this.isExpanded = true;
      }
    }
    
    // Cache shouldShowChildren result
    this._cachedShouldShowChildren = this.computeShouldShowChildren();
    
    // Mark for check to ensure view updates with OnPush strategy
    this.cdr.markForCheck();
  }

  hasChildren(): boolean {
    return this._cachedHasChildren;
  }

  toggleExpand(): void {
    if (this._cachedHasChildren) {
      this.isExpanded = !this.isExpanded;
      // Recalculate cached shouldShowChildren when expand state changes
      this._cachedShouldShowChildren = this.computeShouldShowChildren();
      this.cdr.markForCheck();
    }
  }

  shouldShowChildren(): boolean {
    // Return cached value computed in ngOnChanges
    return this._cachedShouldShowChildren;
  }
  
  private computeShouldShowChildren(): boolean {
    // Compute based on current expanded state
    
    // If in focused view, ignore level filters - show all children of selected branch
    if (this.focusedViewActive && (this._cachedIsSelected || this.isInSelectedBranch)) {
      return this.isExpanded && this._cachedHasChildren;
    }
    
    // If selectedLevel is 0, show all levels
    if (this.selectedLevel === 0) {
      return this.isExpanded && this._cachedHasChildren;
    }
    
    // Otherwise, only show children if we're within the selected level
    return this.isExpanded && this._cachedHasChildren && this.currentLevel < this.selectedLevel;
  }

  // Check if this activity is selected - returns cached value
  isSelected(): boolean {
    return this._cachedIsSelected;
  }

  // Handle checkbox change
  onCheckboxChange(event: Event): void {
    event.stopPropagation(); // Prevent row click from triggering
    const checkbox = event.target as HTMLInputElement;
    this.selectionChange.emit({
      activityId: this.activity.scheduleActivityId,
      selected: checkbox.checked
    });
  }

  // Handle edit progress button click
  onEditProgress(event: Event): void {
    event.stopPropagation(); // Prevent row click from triggering expand/collapse
    this.editProgress.emit(this.activity);
  }

  // Handle edit dates button click
  onEditDates(event: Event): void {
    event.stopPropagation(); // Prevent row click from triggering expand/collapse
    this.editDates.emit(this.activity);
  }

  // Combined visibility check that considers both explicit visible flag and focused view logic
  // Returns cached value computed in ngOnChanges for better performance
  isActivityVisible(): boolean {
    return this._cachedIsVisible;
  }
  
  private computeIsVisible(): boolean {
    // PRIORITY 1: Check focused view first
    // If in focused view, only show activities that are part of the selected tree
    if (this.focusedViewActive) {
      // In focused view, ignore level filters - show the full selected branch
      return this.computeShouldShowInFocusedView();
    }
    
    // PRIORITY 2: Not in focused view - apply level filtering
    // Check explicit visible property (controlled by level filtering)
    if (this.activity && this.activity.visible === false) {
      return false;
    }
    
    // Check level filter visibility (only applies when selectedLevel > 0)
    if (this.selectedLevel > 0 && this.currentLevel > this.selectedLevel) {
      return false;
    }
    
    // Activity is visible if it passes all checks
    return true;
  }

  // Check if this activity should be visible in focused view
  private computeShouldShowInFocusedView(): boolean {
    if (!this.focusedViewActive) {
      return true;
    }
    
    // Show if this activity is selected
    if (this._cachedIsSelected) {
      return true;
    }
    
    // Show if this activity is in a selected branch (descendant of selected)
    // This means it's a child, grandchild, etc. of a selected activity
    if (this.isInSelectedBranch) {
      return true;
    }
    
    // Show if this activity is an ancestor (parent, grandparent, etc.) of a selected activity
    // This keeps the path visible so children can render
    if (this.isAncestorOfSelected) {
      return true;
    }
    
    // Hide siblings and other unrelated activities
    return false;
  }

  // Check if a child activity should be marked as ancestor of selected
  // NOTE: This method is called during template rendering for each child
  // To optimize, we rely on parent component's pre-computed descendant cache
  // The parent passes isAncestorOfSelected as input, so we don't compute it here
  isChildAncestorOfSelected(child: ActivityTreeNode): boolean {
    // Quick check: if this activity is already marked as ancestor or selected,
    // then check if child has any selected descendants
    if (!this.focusedViewActive) {
      return false;
    }
    
    // Optimized: check only immediate children for selection
    // Let the parent component handle deep tree traversal via its cache
    return this.hasDirectlySelectedDescendants(child);
  }

  // Optimized: only check direct descendants, not full recursive tree
  private hasDirectlySelectedDescendants(activity: ActivityTreeNode): boolean {
    if (!activity.children) return false;
    
    // Only check direct children and their immediate children (2 levels max)
    // This is much faster than full recursive traversal
    for (const child of activity.children) {
      if (this.selectedActivityIds.has(child.scheduleActivityId)) {
        return true;
      }
      // Check one more level (grandchildren) only
      if (child.children) {
        for (const grandchild of child.children) {
          if (this.selectedActivityIds.has(grandchild.scheduleActivityId)) {
            return true;
          }
        }
      }
    }
    return false;
  }

  getStatusClass(): string {
    return this.activity.status.toLowerCase().replace(/\s+/g, '_');
  }

  formatDate(dateString: string): string {
    if (!dateString) return 'N/A';
    
    try {
      const date = new Date(dateString);
      const day = date.getDate().toString().padStart(2, '0');
      const month = date.toLocaleString('en-US', { month: 'short' });
      const year = date.getFullYear().toString().slice(-2);
      
      return `${day}-${month}-${year}`;
    } catch (error) {
      return 'N/A';
    }
  }

  // TrackBy function for better performance with children
  trackByChildId(index: number, child: ActivityTreeNode): string {
    return child.scheduleActivityId;
  }
}
