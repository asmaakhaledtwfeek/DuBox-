// Progress Update Models

export interface ProgressUpdate {
  progressUpdateId?: string;
  boxId?: string;
  boxTag?: string;
  boxActivityId: string;
  activityName?: string;
  status: ActivityProgressStatus | string;
  progressPercentage: number;
  workDescription?: string;
  issuesEncountered?: string;
  materialsAvailable?: boolean;
  qualityCheckPassed?: boolean;
  photo?: string; // Deprecated: Use images array instead
  images?: ProgressUpdateImage[];
  teamId?: string;
  teamMemberId?: string;
  updatedBy?: string;
  updatedByName?: string;
  updateDate?: Date;
  createdDate?: Date;
  locationDescription?: string;
  updateMethod?: string;
  boxProgressSnapshot?: number;
}

export enum ActivityProgressStatus {
  NotStarted = 'NotStarted',
  InProgress = 'InProgress',
  Completed = 'Completed',
  OnHold = 'OnHold',
  Delayed = 'Delayed'
}

export interface CreateProgressUpdateRequest {
  boxId: string;
  boxActivityId: string;
  progressPercentage: number;
  workDescription?: string;
  issuesEncountered?: string;
  latitude?: number;
  longitude?: number;
  locationDescription?: string;
  imageUrl?: string; // For URL input method
  updateMethod: string; // "Web" or "Mobile"
  deviceInfo?: string;
  // WIR Position fields (optional, only sent if WIR exists below activity)
  wirBay?: string;
  wirRow?: string;
  wirPosition?: string;
}

export interface ProgressUpdateResponse {
  progressUpdate: ProgressUpdate;
  wirCreated?: boolean;
  wirRecordId?: string;
  wirCode?: string;
}

// Activity Master model to match backend
export interface ActivityMaster {
  activityMasterId: string;
  activityCode: string;
  activityName: string;
  stage: string;
  stageNumber: number;
  sequenceInStage: number;
  overallSequence: number;
  description?: string;
  estimatedDurationDays: number;
  isWIRCheckpoint: boolean;
  wirCode?: string;
  applicableBoxTypes?: string;
  dependsOnActivities?: string;
  isActive: boolean;
}

// Box Activity with all properties from backend
export interface BoxActivityDetail {
  boxActivityId: string;
  boxId: string;
  activityMasterId: string;
  activityName: string;
  activityCode?: string;
  sequence: number;
  status: ActivityProgressStatus;
  progressPercentage: number;
  plannedStartDate?: Date;
  plannedEndDate?: Date;
  actualStartDate?: Date;
  actualEndDate?: Date;
  duration?: number;
  actualDuration?: number;
  workDescription?: string;
  issuesEncountered?: string;
  teamId?: string;
  teamName?: string;
  assignedMemberId?: string;
  assignedMemberName?: string;
  assignedGroupId?: string;
  assignedGroupTag?: string;
  materialsAvailable: boolean;
  isActive: boolean;
  createdDate: Date;
  modifiedDate?: Date;
  
  // Navigation properties
  activityMaster?: ActivityMaster;
  progressUpdates?: ProgressUpdate[];
  wirRecords?: any[];
  
  // Additional frontend properties
  isWIRCheckpoint?: boolean;
  wirCode?: string;
}

export interface PaginatedProgressUpdatesResponse {
  items: ProgressUpdate[];
  totalCount: number;
  pageNumber: number;
  pageSize: number;
  totalPages: number;
}

export interface ProgressUpdateImage {
  progressUpdateImageId: string;
  progressUpdateId: string;
  imageData?: string; // May be null for lightweight responses - use imageUrl instead
  imageType: 'file' | 'url';
  originalName?: string;
  fileSize?: number;
  sequence: number;
  createdDate: Date;
  imageUrl?: string; // URL to fetch image on-demand: /api/images/ProgressUpdate/{progressUpdateImageId}
}

export interface ProgressUpdatesSearchParams {
  searchTerm?: string;
  activityName?: string;
  status?: string;
  fromDate?: string;
  toDate?: string;
}

