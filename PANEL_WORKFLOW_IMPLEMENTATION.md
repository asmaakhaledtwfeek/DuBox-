# Panel Approval Workflow Implementation

## Overview
Implemented a comprehensive panel approval workflow system at the **Pre-cast Location stage** with progress tracking, multiple statuses, automatic quality issue creation, and notification system.

## Features Implemented

### 1. **Backend Changes**

#### Database Schema Updates
- **BoxPanel Entity** (`Dubox.Domain/Entities/BoxPanel.cs`)
  - Added `PreCastProgress` (decimal): 0-100% progress tracking
  - Added `WorkflowStatus` (string): Current workflow status
  - Added `WorkflowStatusDate` (DateTime): When status was last updated
  - Added `WorkflowStatusBy` (Guid): User who updated the status
  - Added `WorkflowStatusNotes` (string): Notes for status update
  - Added `QualityIssueId` (Guid): Reference to automatically created quality issue

#### Enums Extended
- **PanelStatusEnum** (`Dubox.Domain/Enums/PanelStatusEnum.cs`)
  ```csharp
  NotStarted = 1
  InProgress = 2
  OnHold = 3
  Rejected = 4
  NotFixed = 5
  Fixed = 6
  FirstApprovalApproved = 7
  SecondApprovalApproved = 8
  SecondApprovalRejected = 9
  ```

#### New Commands
- **UpdatePanelWorkflowStatusCommand** - Updates panel workflow status with automatic quality issue creation
  - Validates workflow status transitions
  - Creates quality issues automatically for "OnHold" and "Rejected" statuses
  - Prevents marking as "Fixed" until linked quality issue is resolved
  - Notifies relevant users when issues are resolved
  - Updates approval status to "Pending" for re-approval after fixes

#### Automatic Quality Issue Creation
When panel status is set to:
- **OnHold**: Creates quality issue with type "Observation" and severity "Minor"
- **Rejected**: Creates quality issue with type "Defect" and severity "Major"

#### Notification Service
- **INotificationService** - Interface for notification management
- **NotificationService** - Implementation with:
  - `NotifyPanelIssueResolvedAsync()`: Notifies when quality issue is resolved
  - Creates in-app notifications
  - Sends real-time SignalR notifications
  - Updates unread notification count

#### API Endpoints
- `POST /api/boxes/panels/{boxPanelId}/workflow-status`
  - Updates panel workflow status
  - Request body:
    ```json
    {
      "boxPanelId": "guid",
      "workflowStatus": "InProgress|OnHold|Rejected|NotFixed|Fixed",
      "progress": 0-100,
      "notes": "string",
      "issueId": "guid (optional)"
    }
    ```

#### DTOs Updated
- **BoxPanelDto** - Added workflow fields:
  - `PreCastProgress`
  - `WorkflowStatus`
  - `QualityIssueId`

### 2. **Frontend Changes**

#### Models Updated
- **BoxPanel Interface** (`box.model.ts`)
  - Added `preCastProgress?: number`
  - Added `workflowStatus?: string`
  - Added `qualityIssueId?: string`

- **PanelStatus Enum** - Extended with new workflow statuses

#### Services Updated
- **PanelService** (`panel.service.ts`)
  - Added `UpdatePanelWorkflowRequest` interface
  - Added `updatePanelWorkflowStatus()` method

#### New Components

##### PanelWorkflowModalComponent
Location: `dubox-frontend/src/app/features/boxes/panel-workflow-modal/`

**Features:**
- Beautiful gradient UI with modern design
- Status selection with info tooltips
- Progress slider (0-100%) with visual progress bar
- Required notes field
- Real-time validation
- Success/error messaging
- Automatic refresh after update

**Status Options:**
1. **In Progress** (Blue) - Panel is being worked on
2. **Put On Hold** (Orange) - Automatically creates quality issue
3. **Rejected** (Red) - Automatically creates quality issue
4. **Not Fixed** (Purple) - Issue has not been fixed
5. **Fixed** (Green) - Issue resolved, ready for re-approval

**Status Info Tooltips:**
- Provides context for each status
- Warns about automatic quality issue creation
- Explains workflow implications

#### Updated Components
- **BoxPanelsComponent** (`box-panels.component.ts`)
  - Imported `PanelWorkflowModalComponent`
  - Added `showWorkflowModal` flag
  - Added `openWorkflowModal()` method
  - Added `closeWorkflowModal()` method
  - Added `onWorkflowUpdated()` method
  - Updated `getPanelStatusClass()` with new status badges

- **BoxPanelsComponent Template** (`box-panels.component.html`)
  - Added "Workflow" button in actions column
  - Integrated workflow modal
  - Button includes workflow icon

- **BoxPanelsComponent Styles** (`box-panels.component.scss`)
  - Added `.btn-workflow` with gradient styling
  - Added status badge classes:
    - `.status-in-progress` (Blue)
    - `.status-on-hold` (Orange)
    - `.status-rejected` (Red)
    - `.status-not-fixed` (Purple)
    - `.status-fixed` (Green)

## Workflow Process

### 1. **Pre-cast Location Stage**
User clicks "Workflow" button on any panel → Opens modal

### 2. **Status Update Options**

#### A. In Progress
- Update progress percentage
- Add notes about current work
- No automatic actions

#### B. Put On Hold
- **Automatic**: Creates quality issue (Type: Observation, Severity: Minor)
- Links issue to panel
- Records who put it on hold and when
- Requires notes explaining reason

#### C. Rejected
- **Automatic**: Creates quality issue (Type: Defect, Severity: Major)
- Links issue to panel
- Records who rejected and when
- Requires notes explaining rejection reason

#### D. Not Fixed
- Updates existing quality issue status to "Open"
- Adds notes about what is not fixed
- Keeps issue linked to panel

#### E. Fixed
- **Validation**: Checks if linked quality issue is resolved
- **Automatic**: Notifies the person who originally rejected/put on hold
- **Automatic**: Sets first approval status to "Pending" for re-approval
- Adds notes to approval explaining re-approval is required

### 3. **Notification Flow**
When issue is marked as Fixed:
1. System verifies quality issue is resolved
2. Creates notification for original status creator
3. Sends real-time SignalR notification
4. Updates notification count
5. Resets approval workflow for panel

### 4. **Re-approval Process**
After panel is fixed:
1. First approval status reset to "Pending"
2. Approval notes updated with reference to resolved issue
3. Panel can be re-approved through normal approval process

## Database Migration Required

You'll need to create a migration for the new BoxPanel fields:

```bash
# In Dubox.Infrastructure project
Add-Migration AddPanelWorkflowFields
Update-Database
```

Or using .NET CLI:
```bash
dotnet ef migrations add AddPanelWorkflowFields --project Dubox.Infrastructure --startup-project Dubox.Api
dotnet ef database update --project Dubox.Infrastructure --startup-project Dubox.Api
```

## UI/UX Features

### Modal Design
- **Gradient header** (Purple to violet)
- **Panel info card** with current status and progress
- **Status selection** with visual badges
- **Info tooltips** for each status explaining implications
- **Progress slider** with visual progress bar
- **Smooth animations** (fade in, slide up, slide down)
- **Responsive design** for all screen sizes
- **Loading states** with spinner
- **Success/error alerts** with icons

### Status Badges
Each status has a unique color scheme:
- **Not Started**: Gray
- **In Progress**: Blue (professional working status)
- **On Hold**: Orange (warning/pause)
- **Rejected**: Red (critical issue)
- **Not Fixed**: Purple (attention needed)
- **Fixed**: Green (success/resolved)

### Workflow Button
- Gradient styling matching modal theme
- Workflow icon (cross-arrows)
- Hover effects with shadow
- Positioned prominently in actions column

## Benefits

1. **Automatic Issue Tracking**: No manual quality issue creation needed
2. **Progress Visibility**: Real-time progress tracking at pre-cast stage
3. **Accountability**: All status changes tracked with user and timestamp
4. **Notification System**: Automatic notifications when issues are resolved
5. **Re-approval Workflow**: Ensures fixed panels are properly re-approved
6. **Audit Trail**: Complete history of status changes and notes
7. **User-Friendly**: Beautiful, intuitive UI with helpful tooltips
8. **Mobile-Ready**: Responsive design works on all devices

## Integration Points

- ✅ BoxPanel entity and database
- ✅ Quality issue management system
- ✅ Notification system (in-app + SignalR)
- ✅ Box-details component (via box-panels)
- ✅ User service for notifications
- ✅ Permission system (uses existing box permissions)

## Next Steps (Optional Enhancements)

1. Add workflow history timeline view
2. Export panel workflow reports
3. Dashboard widgets for workflow metrics
4. Bulk workflow updates for multiple panels
5. Workflow approval rules by role
6. Email notifications in addition to in-app
7. Mobile app QR code scanning for workflow updates
8. Workflow templates for common scenarios

## Files Modified

### Backend
- ✅ `Dubox.Domain/Entities/BoxPanel.cs`
- ✅ `Dubox.Domain/Enums/PanelStatusEnum.cs`
- ✅ `Dubox.Domain/Services/INotificationService.cs` (new)
- ✅ `Dubox.Application/Features/BoxPanels/Commands/UpdatePanelWorkflowStatusCommand.cs` (new)
- ✅ `Dubox.Application/Features/BoxPanels/Commands/UpdatePanelWorkflowStatusCommandHandler.cs` (new)
- ✅ `Dubox.Application/DTOs/BoxPanelDto.cs`
- ✅ `Dubox.Infrastructure/Services/NotificationService.cs` (new)
- ✅ `Dubox.Infrastructure/Bootstrap.cs`
- ✅ `Dubox.Api/Controllers/BoxesController.cs`

### Frontend
- ✅ `dubox-frontend/src/app/core/models/box.model.ts`
- ✅ `dubox-frontend/src/app/core/services/panel.service.ts`
- ✅ `dubox-frontend/src/app/features/boxes/panel-workflow-modal/` (new component)
  - ✅ `panel-workflow-modal.component.ts`
  - ✅ `panel-workflow-modal.component.html`
  - ✅ `panel-workflow-modal.component.scss`
- ✅ `dubox-frontend/src/app/features/boxes/box-panels/box-panels.component.ts`
- ✅ `dubox-frontend/src/app/features/boxes/box-panels/box-panels.component.html`
- ✅ `dubox-frontend/src/app/features/boxes/box-panels/box-panels.component.scss`

## Status: ✅ COMPLETE

All features have been implemented and are ready for testing. A database migration is required before deployment.

