# QA/QC Activity Approval Checklist - Implementation Guide

## Overview

The QA/QC Activity Approval Checklist system provides a comprehensive inspection and approval workflow for construction activities. This implementation includes predefined checklists for 6 Work Inspection Requests (WIR-1 through WIR-6), each with specific checkpoints based on Group AMANA's manufacturing stages.

## Features Implemented

### 1. **Predefined Checklists**
Six predefined checklists with detailed checkpoints:

- **WIR-1: Assembly Clearance** (5 checkpoints)
  - Structural joints welding
  - PODS installation
  - MEP cage alignment
  - Box closure
  - Defect inspection

- **WIR-2: Mechanical Clearance** (4 checkpoints)
  - Ductwork and insulation
  - Drainage piping
  - Water piping
  - Fire fighting piping

- **WIR-3: Ceiling Closure** (4 checkpoints)
  - Electrical containment
  - Electrical wiring
  - Dry wall framing
  - DB and ONU panels

- **WIR-4: 3rd Fix Installation** (5 checkpoints)
  - False ceiling
  - Tile fixing
  - Painting
  - Kitchenette and counters
  - Doors and windows

- **WIR-5: 3rd Fix MEP Installation** (8 checkpoints)
  - Switches and sockets
  - Light fittings
  - Copper piping
  - Sanitary fittings
  - Thermostats
  - Air outlets
  - Sprinkler heads
  - Smoke detectors

- **WIR-6: Readiness for Dispatch** (5 checkpoints)
  - Iron mongeries
  - Inspection and wrapping
  - Final quality inspection
  - Punch list resolution
  - Documentation package

### 2. **Checkpoint Validation**
Each checkpoint can be marked as:
- **Pass** ✓ (Green)
- **Fail** ✗ (Red) - Requires remarks
- **N/A** (Gray) - Not applicable

### 3. **Inspector Interface**
- View WIR request information (Box Tag, Activity, Status, Requester)
- Interactive checklist table with status buttons
- Real-time validation
- Inspector notes text area
- Photo upload capability
- Digital signature capture

### 4. **Approval/Rejection Logic**
- **Can Approve When:**
  - All checkpoints are reviewed (no Pending status)
  - No failed items
  - Signature is provided
  - Form is valid

- **Must Reject When:**
  - Any checkpoint marked as Fail
  - Rejection reason is provided
  - Signature is provided

### 5. **Digital Signature**
- HTML5 Canvas-based signature pad
- Mouse and touch support
- Clear signature functionality
- Signature required for approval/rejection
- Signature stored as base64 data URL

### 6. **Photo Uploads**
- Multiple photo uploads supported
- Image preview grid
- Remove photo functionality
- Photos attached to inspection report

## Frontend Architecture

### Models (`wir.model.ts`)

```typescript
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

export enum CheckpointStatus {
  Pass = 'Pass',
  Fail = 'Fail',
  NA = 'N/A',
  Pending = 'Pending'
}
```

### Service (`wir.service.ts`)

Key methods:
- `getAllWIRRecords()`: Get all WIR records
- `getWIRRecord(wirRecordId)`: Get specific WIR record
- `getWIRRecordsByBox(boxId)`: Get WIRs for a box
- `getWIRRecordsByActivity(activityId)`: Get WIRs for an activity
- `approveWIRRecord(request)`: Approve WIR
- `rejectWIRRecord(request)`: Reject WIR
- `getChecklistTemplate(wirCode)`: Get predefined checklist

### Component (`qa-qc-checklist.component.ts`)

Key features:
- Reactive forms with FormArray for checklist items
- Signature pad with canvas drawing
- Photo upload with preview
- Real-time validation
- Approval/rejection logic

## Backend Architecture

### Entities

**WIRRecord** (`WIRRecord.cs`)
- Primary entity for Work Inspection Requests
- Links to BoxActivity, User (requester and inspector)
- Stores status, notes, photos, rejection reason

**WIRCheckpoint** (`WIRCheckpoint.cs`)
- Inspection checkpoint entity
- Links to Box
- Stores WIR number, status, approval details

**WIRChecklistItem** (`WIRChecklistItem.cs`)
- Individual checklist item
- Links to WIRCheckpoint
- Stores checkpoint description, status, remarks

### API Endpoints

#### GET Endpoints
```
GET /api/wirrecords
GET /api/wirrecords/{wirRecordId}
GET /api/wirrecords/box/{boxId}
GET /api/wirrecords/activity/{boxActivityId}  ← NEW
```

#### POST Endpoints
```
POST /api/wirrecords
POST /api/wirrecords/{wirRecordId}/approve
POST /api/wirrecords/{wirRecordId}/reject
```

### Commands & Queries

**Queries:**
- `GetAllWIRRecordsQuery`
- `GetWIRRecordByIdQuery`
- `GetWIRRecordsByBoxQuery`
- `GetWIRRecordsByActivityQuery` ← NEW

**Commands:**
- `CreateWIRRecordCommand`
- `ApproveWIRRecordCommand`
- `RejectWIRRecordCommand`

## Routing

The QA/QC checklist is accessible via:
```
/projects/:projectId/boxes/:boxId/activities/:activityId/qa-qc
```

**Route Configuration:**
```typescript
{
  path: 'projects/:projectId/boxes/:boxId/activities/:activityId/qa-qc',
  canActivate: [authGuard, roleGuard],
  data: {
    roles: [
      UserRole.SystemAdmin,
      UserRole.ProjectManager,
      UserRole.QCInspector,
      UserRole.SiteEngineer,
      UserRole.Foreman
    ]
  },
  component: QaQcChecklistComponent
}
```

## Permissions

The QA/QC module permissions are defined in `permission.service.ts`:

```typescript
qaqc: {
  view: [SystemAdmin, ProjectManager, QCInspector, Viewer],
  create: [SystemAdmin, QCInspector],
  edit: [SystemAdmin, QCInspector],
  delete: [SystemAdmin],
  approve: [SystemAdmin, QCInspector],
  reject: [SystemAdmin, QCInspector],
  export: [SystemAdmin, ProjectManager, QCInspector],
  manage: [SystemAdmin, QCInspector]
}
```

## Usage Workflow

### 1. Activity Completion
When a foreman completes an activity that requires inspection (e.g., "Box Closure"), the system should auto-create a WIR record with status "Pending".

### 2. QC Inspector Notification
QC Inspectors receive notifications about pending WIR records and navigate to the inspection page.

### 3. Inspection Process
1. Inspector views WIR information (Box Tag, Activity, Requester)
2. Goes through each checkpoint and marks as Pass/Fail/N/A
3. Adds remarks for failed items (required)
4. Adds general inspection notes
5. Uploads inspection photos (optional)
6. Provides digital signature (required)

### 4. Decision
- **Approve:** All checkpoints pass → Activity approved for next stage
- **Reject:** Any checkpoint fails → Notification sent for rework

### 5. Post-Approval
- WIR record updated with approval details
- Box activity status updated
- Notification sent to project team
- Next stage can begin

## Testing the Implementation

### Step 1: Navigate to Box Details
```
http://localhost:4200/projects/{projectId}/boxes/{boxId}
```

### Step 2: Navigate to Activity Details (Activities tab in Box Details)
```
http://localhost:4200/projects/{projectId}/boxes/{boxId}/activities/{activityId}
```

### Step 3: Click "QA/QC Checklist" button or navigate to:
```
http://localhost:4200/projects/{projectId}/boxes/{boxId}/activities/{activityId}/qa-qc
```

### Step 4: Test Checklist
1. Verify WIR information displays correctly
2. Mark checkpoints with different statuses
3. Try to approve without completing all items (should be disabled)
4. Mark an item as "Fail" → rejection section should appear
5. Upload photos
6. Sign in the signature pad
7. Approve or Reject

## Backend Setup

### 1. Rebuild Backend
```bash
cd D:/Company/GroupAmana/DuBox-/Dubox.Api
dotnet clean
dotnet build
dotnet run
```

### 2. Verify New Endpoint
Test the new activity endpoint:
```bash
GET https://localhost:7098/api/wirrecords/activity/{activityId}
```

### 3. Database
The WIR tables should already exist:
- `WIRRecords`
- `WIRCheckpoints`
- `WIRChecklistItems`
- `QualityIssues`

## Frontend Setup

### 1. Install Dependencies
```bash
cd dubox-frontend
npm install
```

### 2. Run Development Server
```bash
ng serve
```

### 3. Navigate to QA/QC Page
```
http://localhost:4200/projects/{projectId}/boxes/{boxId}/qa-qc
```

## Key Files Created/Modified

### Frontend
- ✅ `src/app/core/models/wir.model.ts` (NEW)
- ✅ `src/app/core/services/wir.service.ts` (NEW)
- ✅ `src/app/features/boxes/qa-qc-checklist/qa-qc-checklist.component.ts` (UPDATED)
- ✅ `src/app/features/boxes/qa-qc-checklist/qa-qc-checklist.component.html` (UPDATED)
- ✅ `src/app/features/boxes/qa-qc-checklist/qa-qc-checklist.component.scss` (UPDATED)

### Backend
- ✅ `Dubox.Application/Features/WIRRecords/Queries/GetWIRRecordsByActivityQuery.cs` (NEW)
- ✅ `Dubox.Application/Features/WIRRecords/Queries/GetWIRRecordsByActivityQueryHandler.cs` (NEW)
- ✅ `Dubox.Api/Controllers/WIRRecordsController.cs` (UPDATED)

### Existing Backend (Already Implemented)
- ✅ `Dubox.Domain/Entities/WIRRecord.cs`
- ✅ `Dubox.Domain/Entities/WIRCheckpoint.cs`
- ✅ `Dubox.Domain/Entities/WIRChecklistItem.cs`
- ✅ `Dubox.Application/Features/WIRRecords/Commands/ApproveWIRRecordCommand.cs`
- ✅ `Dubox.Application/Features/WIRRecords/Commands/RejectWIRRecordCommand.cs`

## Validation Rules

1. **All checkpoints must be reviewed** (no Pending status remains)
2. **Failed items require remarks**
3. **Signature is always required**
4. **Rejection requires a rejection reason**
5. **Inspector notes are optional but recommended**
6. **Photos are optional**

## UI Features

### Responsive Design
- Mobile-friendly signature pad
- Touch support for tablets
- Responsive table layout
- Adaptive button layouts

### Visual Feedback
- Color-coded status buttons (Green/Red/Gray)
- Disabled states for invalid actions
- Loading spinners
- Error messages
- Success alerts

### Accessibility
- ARIA labels
- Keyboard navigation
- Screen reader support
- High contrast colors

## Future Enhancements

1. **Offline Support**: Allow inspections to be completed offline
2. **Photo Annotations**: Draw on photos to highlight issues
3. **Voice Notes**: Audio recordings for inspection notes
4. **Report Generation**: PDF export of completed inspections
5. **Analytics Dashboard**: Track approval rates, common failures
6. **Email Notifications**: Automated emails for approvals/rejections
7. **Mobile App**: Native iOS/Android app for field inspections
8. **Barcode Scanner**: Scan box tags for quick access

## Troubleshooting

### Issue: WIR record not loading
- Verify the activity exists and has a WIR code
- Check backend API is running
- Check browser console for errors
- Verify API endpoint: `GET /api/wirrecords/activity/{activityId}`

### Issue: Signature not saving
- Ensure canvas is properly initialized
- Check `signatureDataUrl` is populated
- Verify form validation is passing

### Issue: Photos not uploading
- Check file size limits (5MB default)
- Verify file types (JPG, PNG only)
- Check backend endpoint for photo upload

### Issue: Cannot approve/reject
- Verify all checkpoints are reviewed
- Check signature is provided
- For rejection, verify rejection reason is filled
- Check user has QCInspector role

## API Response Examples

### GET /api/wirrecords/activity/{activityId}
```json
{
  "isSuccess": true,
  "data": [
    {
      "wirRecordId": "guid",
      "boxActivityId": "guid",
      "boxTag": "B1-GF-BED-01",
      "activityName": "WIR-1",
      "wirCode": "WIR-1",
      "status": "Pending",
      "requestedDate": "2024-11-16T10:00:00Z",
      "requestedBy": "guid",
      "requestedByName": "Ahmed Mohammed",
      "inspectedBy": null,
      "inspectedByName": null,
      "inspectionDate": null,
      "inspectionNotes": null,
      "photoUrls": null,
      "rejectionReason": null
    }
  ]
}
```

### POST /api/wirrecords/{wirRecordId}/approve
```json
{
  "wirRecordId": "guid",
  "inspectionNotes": "All assembly work meets specifications.",
  "photoUrls": "photo1.jpg,photo2.jpg",
  "signature": "data:image/png;base64,..."
}
```

## Security Considerations

1. **Role-based access**: Only QC Inspectors and Admins can approve/reject
2. **JWT authentication**: All endpoints require valid token
3. **Audit trail**: All actions logged with user and timestamp
4. **Signature verification**: Digital signatures stored for audit
5. **Photo validation**: File type and size restrictions

## Performance Optimization

1. **Lazy loading**: Component loaded on-demand
2. **Image compression**: Photos compressed before upload
3. **Debouncing**: Form validation debounced
4. **Caching**: Checklist templates cached in memory
5. **Pagination**: Large WIR lists paginated

## Conclusion

This QA/QC Activity Approval Checklist system provides a comprehensive, user-friendly interface for quality inspectors to review and approve construction activities with predefined checkpoints, digital signatures, and photo documentation. The system ensures quality standards are met before activities proceed to the next stage.

For questions or issues, refer to the main project documentation or contact the development team.

