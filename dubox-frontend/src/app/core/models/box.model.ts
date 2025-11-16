export interface Box {
  id: string;
  name: string;
  code: string;
  projectId: string;
  status: BoxStatus;
  type?: string;
  description?: string;
  assignedTeam?: string;
  assignedTo?: string;
  plannedStartDate?: Date;
  actualStartDate?: Date;
  plannedEndDate?: Date;
  actualEndDate?: Date;
  progress: number;
  activities: BoxActivity[];
  attachments: BoxAttachment[];
  logs: BoxLog[];
  qrCode?: string;
  createdBy?: string;
  updatedBy?: string;
  createdAt?: Date;
  updatedAt?: Date;
}

export enum BoxStatus {
  NotStarted = 'NotStarted',
  InProgress = 'InProgress',
  QAReview = 'QAReview',
  Completed = 'Completed',
  ReadyForDelivery = 'ReadyForDelivery',
  Delivered = 'Delivered',
  OnHold = 'OnHold'
}

export interface BoxActivity {
  id: string;
  boxId: string;
  name: string;
  description?: string;
  status: ActivityStatus;
  sequence: number;
  assignedTo?: string;
  plannedDuration: number;
  actualDuration?: number;
  weightPercentage: number;
  plannedStartDate?: Date;
  actualStartDate?: Date;
  plannedEndDate?: Date;
  actualEndDate?: Date;
  checklistItems?: ChecklistItem[];
  dependencies?: string[];
  createdAt?: Date;
  updatedAt?: Date;
}

export enum ActivityStatus {
  NotStarted = 'NotStarted',
  InProgress = 'InProgress',
  Completed = 'Completed',
  Approved = 'Approved',
  Rejected = 'Rejected',
  OnHold = 'OnHold'
}

export interface ChecklistItem {
  id: string;
  activityId: string;
  description: string;
  isRequired: boolean;
  isCompleted: boolean;
  completedBy?: string;
  completedAt?: Date;
  remarks?: string;
}

export interface BoxAttachment {
  id: string;
  boxId: string;
  fileName: string;
  fileUrl: string;
  fileType: string;
  fileSize: number;
  description?: string;
  uploadedBy: string;
  uploadedAt: Date;
}

export interface BoxLog {
  id: string;
  boxId: string;
  action: string;
  description: string;
  field?: string;
  oldValue?: string;
  newValue?: string;
  performedBy: string;
  performedAt: Date;
}

export interface BoxFilters {
  projectId?: string;
  status?: BoxStatus[];
  assignedTo?: string;
  search?: string;
  dateFrom?: Date;
  dateTo?: Date;
}

