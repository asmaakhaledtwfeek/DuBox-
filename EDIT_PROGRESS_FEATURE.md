# Edit Progress Feature Documentation

## Overview
This feature allows users to update activity progress with automatic actual date tracking. When progress is updated, the system automatically sets actual start and finish dates based on the progress value.

---

## Features

### 1. Edit Button on Each Activity
- **Location:** Next to the progress bar on each activity row
- **Visibility:** Appears on hover over the progress column
- **Icon:** Pencil/edit icon for intuitive interaction

### 2. Progress Update Modal
Shows when clicking the edit button with:
- **Activity Information:** Name and code of the selected activity
- **Progress Input:** Number field (0-100%)
- **Visual Progress Bar:** Live preview of the entered percentage
- **Status Dropdown:** Planned, In Progress, Completed, On Hold
- **Auto-Date Information:** Info box explaining automatic date setting

### 3. Automatic Date Setting

#### Auto Start Date
**Trigger:** When progress goes from 0% to any value > 0%  
**Action:** Automatically sets `actualStartDate` to current date/time  
**Condition:** Only if no actual start date already exists

```typescript
// Example: User sets progress to 25%
if (percentComplete > 0 && !activity.actualStartDate) {
  activity.actualStartDate = new Date(); // ✅ Auto-set
}
```

#### Auto Finish Date
**Trigger:** When progress reaches 100%  
**Action:** 
1. Automatically sets `actualFinishDate` to current date/time
2. Updates status to "Completed"

**Condition:** Only if no actual finish date already exists

```typescript
// Example: User sets progress to 100%
if (percentComplete === 100 && !activity.actualFinishDate) {
  activity.actualFinishDate = new Date(); // ✅ Auto-set
  activity.status = 'Completed'; // ✅ Auto-update status
}
```

---

## User Workflow

### Scenario 1: Starting an Activity
1. Activity starts at 0% progress, status "Planned"
2. User clicks edit button
3. User sets progress to 10%
4. Clicks "Update Progress"
5. **Result:**
   - Progress: 10%
   - Actual Start Date: [Current Date/Time] ✅ Auto-set
   - Status: Can be changed to "In Progress"

### Scenario 2: Updating Progress
1. Activity is at 50% progress
2. User clicks edit button
3. User updates progress to 75%
4. Clicks "Update Progress"
5. **Result:**
   - Progress: 75%
   - Actual Start Date: [Unchanged - already set]
   - Actual Finish Date: [None - not 100% yet]

### Scenario 3: Completing an Activity
1. Activity is at 90% progress
2. User clicks edit button
3. User sets progress to 100%
4. Clicks "Update Progress"
5. **Result:**
   - Progress: 100%
   - Actual Finish Date: [Current Date/Time] ✅ Auto-set
   - Status: "Completed" ✅ Auto-update

---

## Technical Implementation

### Frontend Components

#### 1. Edit Button (activity-tree-node.component.ts)
```typescript
// Added to progress column
<button class="btn-edit-progress" (click)="onEditProgress($event)">
  <svg><!-- Edit icon --></svg>
</button>

// Event emission
@Output() editProgress = new EventEmitter<ActivityTreeNode>();

onEditProgress(event: Event): void {
  event.stopPropagation();
  this.editProgress.emit(this.activity);
}
```

#### 2. Modal (schedule-dashboard.component.html)
- Modal overlay with click-outside-to-close
- Form with progress input and status dropdown
- Visual progress bar preview
- Info box explaining auto-date behavior

#### 3. Update Logic (schedule-dashboard.component.ts)
```typescript
updateProgress(): void {
  const updateRequest: any = {
    percentComplete: this.editProgressData.percentComplete,
    status: this.editProgressData.status
  };

  // Auto-set start date if progress > 0
  if (percentComplete > 0 && !activity.actualStartDate) {
    updateRequest.actualStartDate = new Date().toISOString();
  }

  // Auto-set finish date if progress = 100
  if (percentComplete === 100 && !activity.actualFinishDate) {
    updateRequest.actualFinishDate = new Date().toISOString();
    updateRequest.status = 'Completed';
  }

  // API call
  this.http.put(`/api/schedule/activities/${id}/progress`, updateRequest)
    .subscribe(...);
}
```

### Backend Implementation

#### 1. Command (UpdateActivityProgressCommand.cs)
```csharp
public record UpdateActivityProgressCommand(
    Guid ScheduleActivityId,
    decimal PercentComplete,
    string Status,
    DateTime? ActualStartDate,
    DateTime? ActualFinishDate
) : IRequest<Result<bool>>;
```

#### 2. Handler (UpdateActivityProgressCommandHandler.cs)
- Validates progress (0-100)
- Updates activity entity
- Applies auto-date logic on backend as well (double protection)
- Saves changes to database

#### 3. API Endpoint (ScheduleActivitiesController.cs)
```csharp
[HttpPut("{id}/progress")]
public async Task<IActionResult> UpdateActivityProgress(
    Guid id,
    [FromBody] UpdateActivityProgressRequest request,
    CancellationToken cancellationToken)
{
    var command = new UpdateActivityProgressCommand(...);
    var result = await _mediator.Send(command, cancellationToken);
    return result.IsSuccess ? Ok(result) : BadRequest(result);
}
```

---

## Validation Rules

### Frontend Validation
- ✅ Progress must be 0-100
- ✅ Progress input is type="number" with min/max attributes
- ✅ Error message shown if validation fails

### Backend Validation
- ✅ Progress must be between 0 and 100
- ✅ Activity must exist
- ✅ Returns appropriate error messages

---

## UI/UX Features

### Visual Feedback
1. **Edit Button:**
   - Hidden by default
   - Appears on hover (opacity transition)
   - Hover effect (background color change, scale)

2. **Progress Preview:**
   - Live visual progress bar in modal
   - Updates as user types
   - Shows percentage text

3. **Auto-Date Info Box:**
   - Blue info box with icon
   - Explains automatic behavior
   - Helps users understand the feature

### Success Feedback
- Success modal shown after update
- Activities list automatically refreshes
- Updated progress visible immediately

---

## Files Modified/Created

### Frontend
1. ✅ `activity-tree-node.component.ts` - Added edit button and event
2. ✅ `schedule-dashboard.component.html` - Added edit progress modal
3. ✅ `schedule-dashboard.component.ts` - Added update logic

### Backend
4. ✅ `UpdateActivityProgressCommand.cs` - Command definition (NEW)
5. ✅ `UpdateActivityProgressCommandHandler.cs` - Business logic (NEW)
6. ✅ `ScheduleActivitiesController.cs` - API endpoint (UPDATED)

---

## API Endpoint

### Update Activity Progress

**Method:** `PUT`  
**URL:** `/api/schedule/activities/{id}/progress`  
**Auth:** Required (Bearer token)

**Request Body:**
```json
{
  "percentComplete": 75,
  "status": "In Progress",
  "actualStartDate": null,      // Optional - auto-set if needed
  "actualFinishDate": null       // Optional - auto-set if needed
}
```

**Response (Success):**
```json
{
  "isSuccess": true,
  "data": true,
  "error": null
}
```

**Response (Error):**
```json
{
  "isSuccess": false,
  "data": false,
  "error": {
    "code": "Progress.Invalid",
    "message": "Progress must be between 0 and 100"
  }
}
```

---

## Testing Checklist

### Manual Testing

- [ ] Click edit button on an activity with 0% progress
- [ ] Set progress to 25% → Verify actual start date is set
- [ ] Update progress to 50% → Verify actual start date unchanged
- [ ] Set progress to 100% → Verify actual finish date is set and status = "Completed"
- [ ] Try to set progress to -10 → Verify error message shown
- [ ] Try to set progress to 150 → Verify error message shown
- [ ] Edit button appears on hover
- [ ] Modal closes on cancel
- [ ] Modal closes on backdrop click
- [ ] Progress bar preview updates live
- [ ] Success message shown after update
- [ ] Activities list refreshes after update

### Edge Cases

- [ ] Update activity that already has actual start date
- [ ] Update activity that already has actual finish date
- [ ] Set progress from 100% back to 50% (finish date should remain)
- [ ] Rapid clicking of edit button
- [ ] Network error handling

---

## Future Enhancements

### Potential Improvements
1. **Bulk Update:** Select multiple activities and update progress
2. **Progress History:** Track progress changes over time
3. **Progress Charts:** Visualize progress trends
4. **Notifications:** Notify team when activity reaches certain milestones
5. **Comments:** Add notes when updating progress
6. **Undo:** Undo recent progress changes

---

**Feature Completed:** February 12, 2026  
**Status:** ✅ Ready for Testing
