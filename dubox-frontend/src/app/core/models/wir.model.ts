// WIR (Work Inspection Request) Models

export interface WIRRecord {
  wirRecordId: string;
  boxActivityId: string;
  boxTag: string;
  activityName: string;
  wirCode: string;
  status: WIRStatus;
  requestedDate: Date;
  requestedBy: string;
  requestedByName: string;
  inspectedBy?: string;
  inspectedByName?: string;
  inspectionDate?: Date;
  inspectionNotes?: string;
  photoUrls?: string;
  rejectionReason?: string;
  checklistItems?: WIRChecklistItem[];
}

export interface WIRChecklistItem {
  checklistItemId?: number;
  wirId?: number;
  checkpointDescription: string;
  referenceDocument?: string;
  status: CheckpointStatus;
  remarks?: string;
  sequence: number;
}

export enum WIRStatus {
  Pending = 'Pending',
  Approved = 'Approved',
  Rejected = 'Rejected',
  ConditionalApproval = 'ConditionalApproval'
}

export enum CheckpointStatus {
  Pass = 'Pass',
  Fail = 'Fail',
  NA = 'N/A',
  Pending = 'Pending'
}

export interface CreateWIRRequest {
  boxActivityId: string;
  wirCode: string;
  photoUrls?: string;
}

export interface ApproveWIRRequest {
  wirRecordId: string;
  inspectionNotes?: string;
  photoUrls?: string;
  signature?: string;
  checklistItems?: WIRChecklistItem[];
}

export interface RejectWIRRequest {
  wirRecordId: string;
  rejectionReason: string;
  inspectionNotes?: string;
  signature?: string;
}

// WIR Checkpoint Models
export interface WIRCheckpoint {
  wirId: string;
  boxId: string;
  boxActivityId?: string;
  projectId?: string;
  projectCode?: string;
  box?: {
    boxId: string;
    projectId?: string;
    projectCode?: string;
    boxTag?: string;
    boxName?: string;
  };
  wirNumber: string;
  wirName?: string;
  wirDescription?: string;
  requestedDate?: Date;
  requestedBy?: string;
  inspectionDate?: Date;
  inspectorName?: string;
  inspectorRole?: string;
  status: WIRCheckpointStatus;
  approvalDate?: Date;
  comments?: string;
  attachmentPath?: string;
  createdDate: Date;
  checklistItems?: WIRCheckpointChecklistItem[];
  qualityIssues?: QualityIssueItem[];
  images?: WIRCheckpointImage[];
}

export interface WIRCheckpointImage {
  wirCheckpointImageId: string;
  wirId: string;
  imageData?: string; // May be null for lightweight responses - use imageUrl instead
  imageType: string;
  originalName?: string;
  fileSize?: number;
  sequence: number;
  createdDate: Date;
  imageUrl?: string; // URL to fetch image on-demand: /api/images/WIRCheckpoint/{wirCheckpointImageId}
}

export interface WIRCheckpointFilter {
  projectCode?: string;
  boxTag?: string;
  status?: WIRCheckpointStatus | string;
  wirNumber?: string;
  from?: string;
  to?: string;
}

export interface WIRCheckpointChecklistItem {
  checklistItemId: string;
  wirId: string;
  checkpointDescription: string;
  referenceDocument?: string;
  status: CheckListItemStatus;
  remarks?: string;
  sequence: number;
  predefinedItemId?: string; // Reference to the predefined item this was cloned from
}

export interface PredefinedChecklistItem {
  predefinedItemId: string;
  checkpointDescription: string;
  referenceDocument?: string;
  sequence: number;
  category?: string;
  isActive: boolean;
}

export enum WIRCheckpointStatus {
  Pending = 'Pending',
  Approved = 'Approved',
  Rejected = 'Rejected',
  ConditionalApproval = 'ConditionalApproval'
}

export enum CheckListItemStatus {
  Pending = 'Pending',
  Pass = 'Pass',
  Fail = 'Fail',
  NA = 'NA'
}

export interface CreateWIRCheckpointRequest {
  boxActivityId: string; // Auto-filled from route
  wirNumber: string; // Auto-filled from WIRRecord
  wirName?: string; // User input
  wirDescription?: string; // User input
  attachmentPath?: string; // User input
  comments?: string; // User input
}

export interface AddChecklistItemsRequest {
  wirId: string;
  predefinedItemIds: string[]; // Array of predefined item IDs to add
}

export interface UpdateChecklistItemRequest {
  checklistItemId: string;
  checkpointDescription?: string;
  referenceDocument?: string;
  status?: CheckListItemStatus;
  remarks?: string;
  sequence?: number;
}

export interface ReviewWIRCheckpointRequest {
  wIRId: string;
  status: WIRCheckpointStatus;
  comment?: string;
  inspectorRole?: string;
  files?: File[];
  imageUrls?: string[];
  items: ChecklistItemForReview[];
}

export interface ChecklistItemForReview {
  checklistItemId: string;
  remarks?: string;
  status: CheckListItemStatus;
}

export type IssueType = 'Defect' | 'NonConformance' | 'Observation';
export type SeverityType = 'Critical' | 'Major' | 'Minor';

export type QualityIssueStatus = 'Open' | 'InProgress' | 'Resolved' | 'Closed';

export interface QualityIssueItem {
  issueType: IssueType;
  severity: SeverityType;
  issueDescription: string;
  assignedTo?: string;
  dueDate?: string | Date;
  photoPath?: string;
  reportedBy?: string;
  issueDate?: string | Date;
  status?: QualityIssueStatus | string;
  imageDataUrls?: string[];
}

export interface QualityIssueImage {
  qualityIssueImageId: string;
  issueId: string;
  imageData?: string; // May be null for lightweight responses - use imageUrl instead
  imageType: 'file' | 'url';
  originalName?: string;
  fileSize?: number;
  sequence: number;
  createdDate: Date;
  imageUrl?: string; // URL to fetch image on-demand: /api/images/QualityIssue/{qualityIssueImageId}
}

export interface QualityIssueDetails extends QualityIssueItem {
  issueId: string;
  status?: QualityIssueStatus;
  resolutionDate?: string | Date;
  resolutionDescription?: string;
  boxId?: string;
  boxName?: string;
  boxTag?: string;
  wirId?: string;
  wirNumber?: string;
  wirName?: string;
  wirStatus?: WIRCheckpointStatus;
  wirRequestedDate?: string | Date;
  inspectorName?: string;
  isOverdue?: boolean;
  overdueDays?: number;
  images?: QualityIssueImage[];
}

export interface AddQualityIssueRequest {
  wirId: string;
  issueType: IssueType;
  severity: SeverityType;
  issueDescription: string;
  assignedTo?: string;
  dueDate?: string | Date;
  imageUrls?: string[];
  files?: File[];
}

export interface CreateQualityIssueForBoxRequest {
  boxId: string;
  issueType: IssueType;
  severity: SeverityType;
  issueDescription: string;
  assignedTo?: string;
  dueDate?: string | Date;
  imageUrls?: string[];
  files?: File[];
}

export interface UpdateQualityIssueStatusRequest {
  issueId: string;
  status: QualityIssueStatus;
  resolutionDescription?: string | null;
  photoPath?: string | null; // Deprecated - kept for backward compatibility
}

// Predefined Checklist Templates for each WIR Code
export const WIR_CHECKLIST_TEMPLATES: Record<string, WIRChecklistItem[]> = {
  'WIR-1': [ // Assembly Clearance
    {
      sequence: 1,
      checkpointDescription: 'All structural joints properly welded and inspected',
      referenceDocument: 'AWS D1.1 Welding Code',
      status: CheckpointStatus.Pending,
      remarks: ''
    },
    {
      sequence: 2,
      checkpointDescription: 'PODS (Plumbing Offsite Drainage System) correctly installed',
      referenceDocument: 'IPC 2018',
      status: CheckpointStatus.Pending,
      remarks: ''
    },
    {
      sequence: 3,
      checkpointDescription: 'MEP cage alignment verified',
      referenceDocument: 'Project Specifications',
      status: CheckpointStatus.Pending,
      remarks: ''
    },
    {
      sequence: 4,
      checkpointDescription: 'Box closure completed and sealed',
      referenceDocument: 'Manufacturer Guidelines',
      status: CheckpointStatus.Pending,
      remarks: ''
    },
    {
      sequence: 5,
      checkpointDescription: 'No visible defects or damage',
      referenceDocument: 'Visual Inspection Standard',
      status: CheckpointStatus.Pending,
      remarks: ''
    }
  ],
  'WIR-2': [ // Mechanical Clearance
    {
      sequence: 1,
      checkpointDescription: 'All ductwork and insulation properly installed',
      referenceDocument: 'SMACNA Standards',
      status: CheckpointStatus.Pending,
      remarks: ''
    },
    {
      sequence: 2,
      checkpointDescription: 'Drainage piping installed and tested',
      referenceDocument: 'IPC 2018',
      status: CheckpointStatus.Pending,
      remarks: ''
    },
    {
      sequence: 3,
      checkpointDescription: 'Water piping connections verified',
      referenceDocument: 'Project Specifications',
      status: CheckpointStatus.Pending,
      remarks: ''
    },
    {
      sequence: 4,
      checkpointDescription: 'Fire fighting piping installed',
      referenceDocument: 'NFPA 13',
      status: CheckpointStatus.Pending,
      remarks: ''
    }
  ],
  'WIR-3': [ // Ceiling Closure
    {
      sequence: 1,
      checkpointDescription: 'Electrical containment properly installed',
      referenceDocument: 'NEC 2020',
      status: CheckpointStatus.Pending,
      remarks: ''
    },
    {
      sequence: 2,
      checkpointDescription: 'Electrical wiring completed',
      referenceDocument: 'Electrical Drawings',
      status: CheckpointStatus.Pending,
      remarks: ''
    },
    {
      sequence: 3,
      checkpointDescription: 'Dry wall framing installed',
      referenceDocument: 'ASTM C754',
      status: CheckpointStatus.Pending,
      remarks: ''
    },
    {
      sequence: 4,
      checkpointDescription: 'DB and ONU panels mounted',
      referenceDocument: 'Electrical Specifications',
      status: CheckpointStatus.Pending,
      remarks: ''
    }
  ],
  'WIR-4': [ // 3rd Fix Installation
    {
      sequence: 1,
      checkpointDescription: 'False ceiling installed',
      referenceDocument: 'Ceiling Specifications',
      status: CheckpointStatus.Pending,
      remarks: ''
    },
    {
      sequence: 2,
      checkpointDescription: 'Tile fixing completed',
      referenceDocument: 'ANSI A108',
      status: CheckpointStatus.Pending,
      remarks: ''
    },
    {
      sequence: 3,
      checkpointDescription: 'Internal and external painting completed',
      referenceDocument: 'Paint Specifications',
      status: CheckpointStatus.Pending,
      remarks: ''
    },
    {
      sequence: 4,
      checkpointDescription: 'Kitchenette and counters installed',
      referenceDocument: 'Architectural Drawings',
      status: CheckpointStatus.Pending,
      remarks: ''
    },
    {
      sequence: 5,
      checkpointDescription: 'Doors and windows installed',
      referenceDocument: 'Door/Window Schedule',
      status: CheckpointStatus.Pending,
      remarks: ''
    }
  ],
  'WIR-5': [ // 3rd Fix MEP Installation
    {
      sequence: 1,
      checkpointDescription: 'Switches and sockets installed',
      referenceDocument: 'NEC 2020',
      status: CheckpointStatus.Pending,
      remarks: ''
    },
    {
      sequence: 2,
      checkpointDescription: 'Light fittings installed',
      referenceDocument: 'Lighting Schedule',
      status: CheckpointStatus.Pending,
      remarks: ''
    },
    {
      sequence: 3,
      checkpointDescription: 'Copper piping connections completed',
      referenceDocument: 'Plumbing Specifications',
      status: CheckpointStatus.Pending,
      remarks: ''
    },
    {
      sequence: 4,
      checkpointDescription: 'Sanitary fittings for kitchen installed',
      referenceDocument: 'Fixture Schedule',
      status: CheckpointStatus.Pending,
      remarks: ''
    },
    {
      sequence: 5,
      checkpointDescription: 'Thermostats installed',
      referenceDocument: 'HVAC Specifications',
      status: CheckpointStatus.Pending,
      remarks: ''
    },
    {
      sequence: 6,
      checkpointDescription: 'Air outlets installed',
      referenceDocument: 'HVAC Drawings',
      status: CheckpointStatus.Pending,
      remarks: ''
    },
    {
      sequence: 7,
      checkpointDescription: 'Sprinkler heads installed',
      referenceDocument: 'NFPA 13',
      status: CheckpointStatus.Pending,
      remarks: ''
    },
    {
      sequence: 8,
      checkpointDescription: 'Smoke detectors installed',
      referenceDocument: 'NFPA 72',
      status: CheckpointStatus.Pending,
      remarks: ''
    }
  ],
  'WIR-6': [ // Readiness for Dispatch
    {
      sequence: 1,
      checkpointDescription: 'Iron mongeries installed',
      referenceDocument: 'Hardware Schedule',
      status: CheckpointStatus.Pending,
      remarks: ''
    },
    {
      sequence: 2,
      checkpointDescription: 'Inspection and wrapping completed',
      referenceDocument: 'Packaging Standards',
      status: CheckpointStatus.Pending,
      remarks: ''
    },
    {
      sequence: 3,
      checkpointDescription: 'Final quality inspection passed',
      referenceDocument: 'QC Checklist',
      status: CheckpointStatus.Pending,
      remarks: ''
    },
    {
      sequence: 4,
      checkpointDescription: 'All punch list items resolved',
      referenceDocument: 'Punch List',
      status: CheckpointStatus.Pending,
      remarks: ''
    },
    {
      sequence: 5,
      checkpointDescription: 'Documentation package completed',
      referenceDocument: 'Document Control',
      status: CheckpointStatus.Pending,
      remarks: ''
    }
  ]
};

