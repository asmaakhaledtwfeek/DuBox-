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

