export interface Box {
  id: string;
  name: string;
  code: string;
  serialNumber?: string;
  projectId: string;
  projectCode?: string;
  status: BoxStatus;
  type?: string;
  subType?:string;
  boxTypeId?: number;
  boxTypeName?: string;
  boxSubTypeId?: number;
  boxSubTypeName?: string;
  description?: string;
  floor?: string;
  buildingNumber?: string;
  boxFunction?: string;
  zone?: string;
  length?: number;
  width?: number;
  height?: number;
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
  attachments: BoxDrawing[];
  logs: BoxLog[];
  qrCode?: string;
  currentLocationId?: string;
  currentLocationCode?: string;
  currentLocationName?: string;
  factoryId?: string;
  factoryCode?: string;
  factoryName?: string;
  bay?: string;
  row?: string;
  position?: string;
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
  OnHold = 'OnHold',
  Dispatched = 'Dispatched'
}

// Map frontend status strings to backend status numbers
export function getBoxStatusNumber(status: BoxStatus | string): number {
  const statusMap: Record<string, number> = {
    'NotStarted': 1,
    'InProgress': 2,
    'Completed': 3,
    'OnHold': 4,
    'Delayed': 5,
    'Dispatched': 6,
    'QAReview': 2,  // Map to InProgress
    'ReadyForDelivery': 3,  // Map to Completed
    'Delivered': 3  // Map to Completed
  };
  return statusMap[status] || 1;  // Default to NotStarted
}

/**
 * Get available status transitions based on current status and progress
 * Business Rules:
 * - NotStarted: can only change to OnHold
 * - InProgress: can only change to OnHold
 * - Completed: can only change to Dispatched or OnHold
 * - OnHold:
 *   - If progress = 0: can change to NotStarted
 *   - If progress < 100: can change to InProgress
 *   - If progress >= 100: can change to Completed or Dispatched
 * - Dispatched: cannot change status (read-only)
 */
export function getAvailableBoxStatuses(currentStatus: BoxStatus, progress: number): BoxStatus[] {
  switch (currentStatus) {
    case BoxStatus.NotStarted:
      return [BoxStatus.OnHold];
    
    case BoxStatus.InProgress:
      return [BoxStatus.OnHold];
    
    case BoxStatus.Completed:
      return [BoxStatus.Dispatched, BoxStatus.OnHold];
    
    case BoxStatus.OnHold:
      if (progress === 0) {
        return [BoxStatus.NotStarted];
      } else if (progress < 100) {
        return [BoxStatus.InProgress];
      } else {
        return [BoxStatus.Completed, BoxStatus.Dispatched];
      }
    
    case BoxStatus.Dispatched:
      // Dispatched cannot change status
      return [];
    
    default:
      return [];
  }
}

/**
 * Check if box actions (edit, delete, etc.) are allowed based on box status
 * Rules:
 * - NotStarted/InProgress: All actions allowed
 * - Completed: All actions allowed
 * - OnHold: No box actions allowed (only status changes)
 * - Dispatched: No actions allowed (read-only)
 */
export function canPerformBoxActions(boxStatus: BoxStatus): boolean {
  switch (boxStatus) {
    case BoxStatus.NotStarted:
    case BoxStatus.InProgress:
    case BoxStatus.Completed:
      return true;
    case BoxStatus.OnHold:
    case BoxStatus.Dispatched:
      return false;
    default:
      return false;
  }
}

/**
 * Check if activity actions are allowed based on box status
 * Rules:
 * - NotStarted/InProgress: All activity actions allowed
 * - Completed: All activity actions allowed
 * - OnHold: No activity actions allowed
 * - Dispatched: No activity actions allowed
 */
export function canPerformActivityActions(boxStatus: BoxStatus): boolean {
  switch (boxStatus) {
    case BoxStatus.NotStarted:
    case BoxStatus.InProgress:
    case BoxStatus.Completed:
      return true;
    case BoxStatus.OnHold:
    case BoxStatus.Dispatched:
      return false;
    default:
      return false;
  }
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
  Delayed = 'Delayed',
  Approved = 'Approved',
  Rejected = 'Rejected',
  OnHold = 'OnHold'
}

/**
 * Get available activity status transitions based on current status and progress
 * Business Rules:
 * - NotStarted/InProgress: can only change to OnHold
 * - Completed: cannot change status
 * - OnHold: 
 *   - If progress = 0: can change to NotStarted
 *   - If progress < 100: can change to InProgress
 */
export function getAvailableActivityStatuses(currentStatus: ActivityStatus, progress: number): ActivityStatus[] {
  switch (currentStatus) {
    case ActivityStatus.NotStarted:
    case ActivityStatus.InProgress:
      return [ActivityStatus.OnHold];
    
    case ActivityStatus.Completed:
      // Cannot change status from Completed
      return [];
    
    case ActivityStatus.OnHold:
      if (progress === 0) {
        return [ActivityStatus.NotStarted];
      } else if (progress < 100) {
        return [ActivityStatus.InProgress];
      } else {
        // Progress >= 100, but still on hold - can change to InProgress or Completed
        return [ActivityStatus.InProgress, ActivityStatus.Completed];
      }
    
    default:
      return [];
  }
}

/**
 * Check if activity actions are allowed based on activity status
 * Rules:
 * - NotStarted/InProgress: All actions allowed
 * - Completed: No actions allowed (read-only)
 * - OnHold: No actions allowed (only status changes)
 */
export function canPerformActivityActionsByStatus(activityStatus: ActivityStatus): boolean {
  switch (activityStatus) {
    case ActivityStatus.NotStarted:
    case ActivityStatus.InProgress:
      return true;
    case ActivityStatus.Completed:
    case ActivityStatus.OnHold:
      return false;
    default:
      return false;
  }
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

export interface BoxDrawing {
  id: string;
  boxId: string;
  fileName: string;
  fileUrl: string;
  fileType: string;
  fileSize: number;
  version?: number; // Version number for files with same name
  description?: string;
  uploadedBy: string;
  uploadedAt: Date;
  createdDate?: Date; // Creation date
  drawingUrl?: string; // URL for drawing
  originalFileName?: string; // Original filename
  fileExtension?: string; // File extension (.pdf, .dwg, etc.)
  imageType?: string; // "file" or "url"
  imageUrl?: string; // Image URL
  displayUrl?: string; // Display URL
  updateDate?: Date; // Update date
  createdByName?: string; // Name of creator
}

export interface BoxDrawingDto {
  boxDrawingId: string;
  boxId: string;
  drawingUrl?: string;
  fileData?: string;
  originalFileName?: string;
  fileExtension?: string;
  fileType?: string; // "url" or "file"
  fileSize?: number;
  version?: number; // Version number for files with same name
  createdDate: Date;
  createdBy?: string;
  createdByName?: string; // Name of user who created this drawing
}

export interface BoxDrawingImage {
  progressUpdateImageId: string;
  progressUpdateId: string;
  imageUrl?: string;
  imageData?: string;
  imageType: 'file' | 'url';
  originalName?: string;
  fileSize?: number;
  sequence: number;
  createdDate: Date;
  activityName?: string;
  progressPercentage?: number;
  updateDate?: Date;
}

export interface BoxDrawingsResponse {
  images: BoxDrawingImage[];
  totalCount: number;
}

export interface BoxAllAttachmentsResponse {
  wirCheckpointImages: BoxAttachmentDto[];
  progressUpdateImages: BoxAttachmentDto[];
  qualityIssueImages: BoxAttachmentDto[];
  totalCount: number;
}

export interface BoxAttachmentDto {
  imageId: string;
  imageData: string;
  imageType: string;
  originalName?: string;
  fileSize?: number;
  sequence: number;
  version?: number;
  createdDate: Date;
  createdBy?: string;
  referenceId: string;
  referenceType: string;
  referenceName?: string;
  boxActivityId?: string;
  activityName?: string;
}

export interface BoxLog {
  id: string;
  boxId: string;
  action: string;
  description: string;
  tableName?: string; // Table name from audit log (e.g., "Box", "QualityIssue")
  entityDisplayName?: string; // Display name of the entity
  field?: string;
  oldValue?: string;
  newValue?: string;
  oldValues?: string; // JSON string of all old values (like audit logs)
  newValues?: string; // JSON string of all new values (like audit logs)
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
  buildingNumber?: string;
  boxFunction?: string;
  zone?: string;
  status?: string;
}

export interface BoxSubTypeStat {
  subTypeName: string;
  subTypeAbbreviation?: string;
  boxCount: number;
  progress: number;
}

export interface BoxTypeStat {
  boxType: string;
  boxCount: number;
  overallProgress: number;
  subTypes?: BoxSubTypeStat[];
}

export interface BoxTypeStatsByProject {
  projectId: string;
  boxTypeStats: BoxTypeStat[];
}

export interface ProjectTypeCategory {
  categoryId: number;
  categoryName: string;
  abbreviation?: string;
}

export interface BoxType {
  boxTypeId: number;
  boxTypeName: string;
  abbreviation?: string;
  categoryId: number;
  hasSubTypes: boolean;
}

export interface BoxSubType {
  boxSubTypeId: number;
  boxSubTypeName: string;
  abbreviation?: string;
  boxTypeId: number;
}

