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

export interface ActivityTemplate {
  activityTemplateId: string;
  templateName: string;
  description?: string;
  stageCount: number;
  isActive: boolean;
  activityCount: number;
  activities?: ActivityTemplateActivity[];
  createdDate: Date;
  createdBy?: string;
  modifiedDate?: Date;
  modifiedBy?: string;
}

export interface ActivityTemplateActivity {
  activityTemplateActivityId: string;
  activityTemplateId: string;
  sourceActivityMasterId?: string;
  isCustomActivity: boolean;
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
}

export interface CreateActivityTemplateRequest {
  templateName: string;
  description?: string;
  stageCount: number;
  activities: CreateActivityTemplateActivityDto[];
}

export interface CreateActivityTemplateActivityDto {
  activityTemplateActivityId?: string;  // For tracking new vs existing activities
  sourceActivityMasterId?: string;
  isCustomActivity: boolean;
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
  assignedTeamId?: string;  // Team GUID
  selectedChecklistItemIds?: string[];  // For display purposes
  selectedChecklistItems?: Array<{     // For API calls
    predefinedChecklistItemId: string;
    sequence: number;
    isMandatory: boolean;
  }>;
  targetStageNumber?: number;  // Override stage when adding from activity master to a specific stage
}

export interface CreateActivityTemplateFromMasterRequest {
  templateName: string;
  description?: string;
  stageCount?: number;
  activityMasterIds: string[];
}

export interface UpdateActivityTemplateRequest {
  activityTemplateId: string;
  templateName: string;
  description?: string;
  stageCount: number;
  isActive: boolean;
}

export interface AddActivityToTemplateRequest {
  activityTemplateId: string;
  sourceActivityMasterId?: string;
  isCustomActivity: boolean;
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
  targetStageNumber?: number;  // Override stage when adding from activity master to a specific stage
}

export interface ScheduleActivity {
  scheduleActivityId: string;
  sourceActivityMasterId?: string;
  isCustomActivity: boolean;
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
  plannedStartDate: Date;
  plannedFinishDate: Date;
  actualStartDate?: Date;
  actualFinishDate?: Date;
  status: string;
  percentComplete: number;
  weight: number;
  projectId?: string;
  projectName?: string;
}

export interface CreateScheduleActivityFromMasterRequest {
  activityMasterId: string;
  plannedStartDate: Date;
  plannedFinishDate: Date;
  projectId?: string;
}

export interface CreateScheduleActivitiesFromTemplateRequest {
  activityTemplateId: string;
  baseStartDate: Date;
  projectId?: string;
}

// Checklist-related interfaces
export interface PredefinedChecklistItem {
  predefinedItemId: string;
  description: string;
  sequence: number;
  reference?: string;
  checklistSectionId?: string;
  checklistSection?: ChecklistSection;
  isActive: boolean;
  createdDate: Date;
}

export interface ChecklistSection {
  checklistSectionId: string;
  sectionName: string;
  displayOrder: number;
  checklistId: string;
  checklist?: Checklist;
  items?: PredefinedChecklistItem[];
}

export interface Checklist {
  checklistId: string;
  checklistName: string;
  wirCode?: string;
  isWirStageChecklist?: boolean;
  createdDate: Date;
  createdBy?: string;
  sections?: ChecklistSection[];
}

// Stage interface for UI
export interface ActivityStage {
  stageNumber: number;
  stageName: string;
  activities: CreateActivityTemplateActivityDto[];
  expanded?: boolean;
  hasWIR?: boolean;
  wirActivityId?: string;
}
