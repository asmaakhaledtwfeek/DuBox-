// Progress Update Models

export interface ProgressUpdate {
  progressUpdateId?: string;
  boxActivityId: string;
  status: ActivityProgressStatus;
  progressPercentage: number;
  workDescription?: string;
  issuesEncountered?: string;
  materialsAvailable: boolean;
  qualityCheckPassed: boolean;
  photoUrls?: string;
  teamId?: string;
  teamMemberId?: string;
  updatedBy?: string;
  updatedByName?: string;
  updateDate?: Date;
  createdDate?: Date;
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
  photoUrls?: string[];
  updateMethod: string; // "Web" or "Mobile"
  deviceInfo?: string;
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
  workDescription?: string;
  issuesEncountered?: string;
  teamId?: number;
  teamName?: string;
  assignedMemberId?: string;
  assignedMemberName?: string;
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

