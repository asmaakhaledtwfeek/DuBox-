export interface Box {
  id: string;
  name: string;
  code: string;
  projectId: string;
  status: BoxStatus;
  type?: string;
  description?: string;
  floor?: string;
  building?: string;
  zone?: string;
  length?: number;
  width?: number;
  height?: number;
  bimModelReference?: string;
  revitElementId?: string;
  assignedTeam?: string;
  assignedTo?: string;
  plannedStartDate?: Date;
  actualStartDate?: Date;
  plannedEndDate?: Date;
  actualEndDate?: Date;
  duration?: number;
  notes?: string;
  progress: number;
  activities: BoxActivity[];
  activitiesCount?: number;
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

// Map frontend status strings to backend status numbers
export function getBoxStatusNumber(status: BoxStatus | string): number {
  const statusMap: Record<string, number> = {
    'NotStarted': 1,
    'InProgress': 2,
    'Completed': 3,
    'OnHold': 4,
    'Delayed': 5,
    'QAReview': 2,  // Map to InProgress
    'ReadyForDelivery': 3,  // Map to Completed
    'Delivered': 3  // Map to Completed
  };
  return statusMap[status] || 1;  // Default to NotStarted
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
  name?: string;  // Item name/title
  description: string;
  notes?: string;  // Additional notes
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

export interface BoxImportResult {
  successCount: number;
  failureCount: number;
  errors: string[];
  importedBoxes: ImportedBoxPreview[];
}

export interface ImportedBoxPreview {
  boxId: string;
  projectId: string;
  projectCode: string;
  boxTag: string;
  boxName?: string;
  boxType?: string;
  floor?: string;
  building?: string;
  zone?: string;
  status?: string;
}

