import { Component, Input, Output, EventEmitter, OnInit, OnDestroy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router, RouterModule } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { Subscription } from 'rxjs';
import { BoxService } from '../../../core/services/box.service';
import { Box, BoxActivity } from '../../../core/models/box.model';

@Component({
  selector: 'app-box-quick-view-modal',
  standalone: true,
  imports: [CommonModule, RouterModule, FormsModule],
  templateUrl: './box-quick-view-modal.component.html',
  styleUrls: ['./box-quick-view-modal.component.scss']
})
export class BoxQuickViewModalComponent implements OnInit, OnDestroy {
  @Input() boxId: string = '';
  @Input() projectId: string = '';
  @Output() close = new EventEmitter<void>();
  
  box: Box | null = null;
  loading = true;
  error = '';
  
  // Concrete Panel Delivery
  concreteWalls: Array<{key: string, label: string, checked: boolean}> = [
    {key: 'wall1', label: 'Wall 01', checked: false},
    {key: 'wall2', label: 'Wall 02', checked: false},
    {key: 'wall3', label: 'Wall 03', checked: false},
    {key: 'wall4', label: 'Wall 04', checked: false},
    {key: 'slab', label: 'Slab', checked: false},
    {key: 'soffit', label: 'Soffit', checked: false}
  ];
  
  // Pod Delivery
  podDeliverChecked = false;
  podName: string = '';
  podType: string = '';
  
  private subscriptions: Subscription[] = [];

  constructor(
    private boxService: BoxService,
    private router: Router
  ) {}

  ngOnInit(): void {
    if (this.boxId) {
      this.loadBox();
    }
  }

  ngOnDestroy(): void {
    this.subscriptions.forEach(sub => sub.unsubscribe());
  }

  loadBox(): void {
    this.loading = true;
    this.error = '';
    
    const boxSub = this.boxService.getBox(this.boxId).subscribe({
      next: (box) => {
        this.box = box;
        this.initializeDeliveryInfo(box);
        
        // Load activities if not already loaded
        if (!box.activities || box.activities.length === 0) {
          this.loadActivities();
        } else {
          this.loading = false;
        }
      },
      error: (err) => {
        this.error = err.message || 'Failed to load box details';
        this.loading = false;
        console.error('Error loading box:', err);
      }
    });

    this.subscriptions.push(boxSub);
  }

  loadActivities(): void {
    if (!this.boxId) return;
    
    const activitiesSub = this.boxService.getBoxActivities(this.boxId).subscribe({
      next: (activities) => {
        if (this.box) {
          this.box.activities = activities;
        }
        this.loading = false;
      },
      error: (err) => {
        console.error('Error loading activities:', err);
        this.loading = false;
      }
    });

    this.subscriptions.push(activitiesSub);
  }

  private initializeDeliveryInfo(box: Box): void {
    // Initialize concrete walls
    this.concreteWalls[0].checked = box.wall1 ?? false;
    this.concreteWalls[1].checked = box.wall2 ?? false;
    this.concreteWalls[2].checked = box.wall3 ?? false;
    this.concreteWalls[3].checked = box.wall4 ?? false;
    this.concreteWalls[4].checked = box.slab ?? false;
    this.concreteWalls[5].checked = box.soffit ?? false;
    
    // Initialize pod delivery
    this.podDeliverChecked = box.podDeliver ?? false;
    this.podName = box.podName ?? '';
    this.podType = box.podType ?? '';
  }

  // Activity chart methods
  getActivityCountByStatus(status: string): number {
    if (!this.box || !this.box.activities) {
      return 0;
    }
    
    const normalizedStatus = status.toString().toLowerCase();
    return this.box.activities.filter(activity => {
      if (!activity.status) return false;
      const activityStatus = activity.status.toString().toLowerCase();
      
      if (normalizedStatus === 'completed') {
        const isCompleted = activityStatus === 'completed';
        const isDelayedWith100Progress = activityStatus === 'delayed' && 
                                         (activity.weightPercentage >= 100 || 
                                          (activity as any).progressPercentage >= 100);
        return isCompleted || isDelayedWith100Progress;
      }
      
      return activityStatus === normalizedStatus;
    }).length;
  }

  getActivityPercentage(status: string): number {
    if (!this.box || !this.box.activities || this.box.activities.length === 0) return 0;
    const count = this.getActivityCountByStatus(status);
    return Math.round((count / this.box.activities.length) * 100);
  }

  getCircleSegment(status: string): string {
    const percentage = this.getActivityPercentage(status);
    const circumference = 2 * Math.PI * 80;
    const segmentLength = (percentage / 100) * circumference;
    return `${segmentLength} ${circumference}`;
  }

  getCircleOffset(status: string): number {
    const circumference = 2 * Math.PI * 80;
    let offset = 0;
    
    if (status === 'InProgress') {
      const completedPercentage = this.getActivityPercentage('Completed');
      offset = -((completedPercentage / 100) * circumference);
    } else if (status === 'NotStarted') {
      const completedPercentage = this.getActivityPercentage('Completed');
      const inProgressPercentage = this.getActivityPercentage('InProgress');
      offset = -(((completedPercentage + inProgressPercentage) / 100) * circumference);
    }
    
    return offset;
  }

  viewFullDetails(): void {
    if (this.box && this.box.projectId) {
      this.router.navigate(['/projects', this.box.projectId, 'boxes', this.box.id]);
    }
  }

  closeModal(): void {
    this.close.emit();
  }
}
