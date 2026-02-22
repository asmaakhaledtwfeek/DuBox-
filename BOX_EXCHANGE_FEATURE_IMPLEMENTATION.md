# Box Exchange Feature Implementation

## Overview
This document describes the implementation of the Box Exchange feature that allows users to request changes to a box's building or level (floor). When a user requests an exchange, a quality issue is automatically created and assigned to the project creator for approval.

## Feature Flow

### 1. User Requests Box Exchange
- User selects a box and requests to change:
  - Building number
  - Floor/Level
  - Or both
- User provides a reason for the exchange request

### 2. System Creates Exchange Request
- Creates a `BoxExchange` entity to store:
  - Old values (current building, floor, BoxTag)
  - New requested values
  - Request reason and status
- Automatically creates a Quality Issue with:
  - Type: `ExchangeRequest`
  - Assigned to: Project Creator
  - Description: Details of requested changes
  - Due date: 7 days from creation
- Generates a new BoxTag if building or floor changes
- Sends notification to project creator

### 3. Project Creator Reviews
- Project creator receives notification
- Reviews the exchange request via Quality Issue
- Can approve (Resolve/Close issue) or reject (with comments)

### 4. Automatic Application on Approval
- When Quality Issue is Resolved or Closed:
  - Box data is automatically updated with new values
  - BoxTag is updated to reflect new building/floor
  - BoxExchange status changes to `Applied`
  - Audit logs are created for tracking

## Backend Implementation

### 1. New Database Entities

#### BoxExchange Entity
**File:** `Dubox.Domain/Entities/BoxExchange.cs`

```csharp
public class BoxExchange
{
    public Guid BoxExchangeId { get; set; }
    public Guid BoxId { get; set; }
    public Guid QualityIssueId { get; set; }
    
    // Original values
    public string? OldBuildingNumber { get; set; }
    public string? OldFloor { get; set; }
    public string? OldBoxTag { get; set; }
    
    // Requested new values
    public string? NewBuildingNumber { get; set; }
    public string? NewFloor { get; set; }
    public string? NewBoxTag { get; set; }
    
    // Exchange details
    public string? RequestReason { get; set; }
    public ExchangeRequestStatusEnum Status { get; set; }
    public DateTime RequestedDate { get; set; }
    public Guid RequestedBy { get; set; }
    
    // Approval tracking
    public DateTime? ApprovedDate { get; set; }
    public Guid? ApprovedBy { get; set; }
    public DateTime? RejectedDate { get; set; }
    public Guid? RejectedBy { get; set; }
    public string? RejectionReason { get; set; }
    
    // Application tracking
    public DateTime? AppliedDate { get; set; }
    public Guid? AppliedBy { get; set; }
    
    // Navigation properties
    public Box Box { get; set; }
    public QualityIssue QualityIssue { get; set; }
    public User RequestedByUser { get; set; }
    // ... other navigation properties
}
```

#### ExchangeRequestStatusEnum
**File:** `Dubox.Domain/Enums/ExchangeRequestStatusEnum.cs`

```csharp
public enum ExchangeRequestStatusEnum
{
    Pending = 1,
    Approved,
    Rejected,
    Applied
}
```

#### IssueTypeEnum Update
**File:** `Dubox.Domain/Enums/IssueTypeEnum.cs`

Added new enum value:
```csharp
ExchangeRequest = 4
```

### 2. Database Migration

**File:** `Dubox.Infrastructure/Migrations/20260216130000_AddBoxExchangeEntity.cs`

Creates `BoxExchanges` table with:
- Primary key: `BoxExchangeId`
- Foreign keys to: `Boxes`, `QualityIssues`, `Users` (multiple)
- Proper cascade/restrict rules
- Indexes on all foreign keys

### 3. Application Layer

#### Command
**File:** `Dubox.Application/Features/Boxes/Commands/CreateBoxExchangeRequestCommand.cs`

```csharp
public record CreateBoxExchangeRequestCommand(
    Guid BoxId,
    string? NewBuildingNumber,
    string? NewFloor,
    string? RequestReason
) : IRequest<Result<BoxExchangeDto>>;
```

#### Command Handler
**File:** `Dubox.Application/Features/Boxes/Commands/CreateBoxExchangeRequestCommandHandler.cs`

Key responsibilities:
1. **Permission & Validation**
   - Checks user permissions
   - Validates box exists and user has access
   - Ensures at least one field is being changed

2. **BoxTag Generation**
   - Generates new BoxTag based on: `ProjectCode-Building-Floor-Type-SubType`
   - Validates uniqueness within project

3. **Quality Issue Creation**
   - Creates issue with type `ExchangeRequest`
   - Auto-generates issue number
   - Assigns to project creator
   - Sets 7-day due date

4. **BoxExchange Entity Creation**
   - Stores old and new values
   - Sets status to `Pending`
   - Records requester information

5. **Notification**
   - Sends real-time notification to project creator via SignalR
   - Creates database notification record
   - Updates unread count

6. **Audit Logging**
   - Records the exchange request creation
   - Logs all changes for compliance

#### DTO
**File:** `Dubox.Application/DTOs/BoxExchangeDto.cs`

Complete DTO with:
- Box information
- Quality issue details
- Old and new values
- Status and tracking information
- User names for display

### 4. Quality Issue Handler Update

**File:** `Dubox.Application/Features/QualityIssues/Commands/UpdateQualityIssueStatusCommandHandler.cs`

Added logic to handle `ExchangeRequest` type issues:

```csharp
// When quality issue is resolved/closed
if (issue.IssueType == IssueTypeEnum.ExchangeRequest)
{
    var boxExchange = await _unitOfWork.Repository<BoxExchange>()
        .FirstOrDefaultAsync(be => be.QualityIssueId == issue.IssueId);

    if (boxExchange != null && boxExchange.Status == ExchangeRequestStatusEnum.Pending)
    {
        // Update box with new values
        box.BuildingNumber = boxExchange.NewBuildingNumber;
        box.Floor = boxExchange.NewFloor;
        box.BoxTag = boxExchange.NewBoxTag;
        
        // Update BoxExchange status to Applied
        boxExchange.Status = ExchangeRequestStatusEnum.Applied;
        boxExchange.AppliedDate = DateTime.UtcNow;
        boxExchange.AppliedBy = currentUserId;
        
        // Create audit logs
        // ...
    }
}
```

### 5. API Endpoint

**File:** `Dubox.Api/Controllers/BoxesController.cs`

```csharp
[HttpPost("{boxId}/exchange-request")]
[ProducesResponseType(StatusCodes.Status200OK)]
[ProducesResponseType(StatusCodes.Status400BadRequest)]
public async Task<IActionResult> CreateBoxExchangeRequest(
    Guid boxId,
    [FromBody] CreateBoxExchangeRequestCommand command,
    CancellationToken cancellationToken = default)
{
    if (boxId != command.BoxId)
        return BadRequest("Box ID mismatch");

    var result = await _mediator.Send(command, cancellationToken);
    return result.IsSuccess ? Ok(result) : BadRequest(result);
}
```

## API Usage

### Create Box Exchange Request

**Endpoint:** `POST /api/boxes/{boxId}/exchange-request`

**Request Body:**
```json
{
  "boxId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "newBuildingNumber": "B3",
  "newFloor": "L2",
  "requestReason": "Box was incorrectly assigned to Building B2"
}
```

**Response:**
```json
{
  "isSuccess": true,
  "data": {
    "boxExchangeId": "7b8f9e12-3456-7890-abcd-ef1234567890",
    "boxId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
    "boxTag": "PRJ-B2-L1-TYPE-SUBTYPE",
    "qualityIssueId": "9c8d7e6f-5432-1098-fedc-ba9876543210",
    "issueNumber": "00123",
    "oldBuildingNumber": "B2",
    "oldFloor": "L1",
    "oldBoxTag": "PRJ-B2-L1-TYPE-SUBTYPE",
    "newBuildingNumber": "B3",
    "newFloor": "L2",
    "newBoxTag": "PRJ-B3-L2-TYPE-SUBTYPE",
    "requestReason": "Box was incorrectly assigned to Building B2",
    "status": "Pending",
    "requestedDate": "2026-02-16T13:00:00Z",
    "requestedBy": "user-guid",
    "requestedByName": "John Doe"
  }
}
```

## Database Schema

### BoxExchanges Table

| Column | Type | Nullable | Description |
|--------|------|----------|-------------|
| BoxExchangeId | uniqueidentifier | No | Primary key |
| BoxId | uniqueidentifier | No | FK to Boxes |
| QualityIssueId | uniqueidentifier | No | FK to QualityIssues |
| OldBuildingNumber | nvarchar(100) | Yes | Original building |
| OldFloor | nvarchar(50) | Yes | Original floor |
| OldBoxTag | nvarchar(100) | Yes | Original BoxTag |
| NewBuildingNumber | nvarchar(100) | Yes | Requested building |
| NewFloor | nvarchar(50) | Yes | Requested floor |
| NewBoxTag | nvarchar(100) | Yes | Calculated new BoxTag |
| RequestReason | nvarchar(1000) | Yes | Reason for request |
| Status | int | No | ExchangeRequestStatusEnum |
| RequestedDate | datetime2 | No | When requested |
| RequestedBy | uniqueidentifier | No | FK to Users |
| ApprovedDate | datetime2 | Yes | When approved |
| ApprovedBy | uniqueidentifier | Yes | FK to Users |
| RejectedDate | datetime2 | Yes | When rejected |
| RejectedBy | uniqueidentifier | Yes | FK to Users |
| RejectionReason | nvarchar(1000) | Yes | Rejection reason |
| AppliedDate | datetime2 | Yes | When applied |
| AppliedBy | uniqueidentifier | Yes | FK to Users |
| CreatedDate | datetime2 | No | Creation timestamp |
| CreatedBy | uniqueidentifier | Yes | Creator user ID |

## Security & Permissions

- **Create Exchange Request**: Requires `Boxes.Edit` permission
- **Approve/Reject**: Automatic via Quality Issue resolution (assigned to project creator)
- **Access Control**: Project-level access validation
- **Audit Trail**: Complete logging of all actions

## Frontend Integration (Next Steps)

### 1. UI Components Needed

- **Exchange Request Dialog/Modal**
  - Building dropdown/input
  - Floor dropdown/input
  - Reason textarea
  - Preview of new BoxTag
  - Submit button

- **Box Details Page Integration**
  - "Request Exchange" button
  - Exchange history display
  - Link to related Quality Issue

### 2. API Integration

```typescript
// Service method
async createBoxExchangeRequest(
  boxId: string, 
  request: BoxExchangeRequest
): Promise<BoxExchangeDto> {
  return this.http.post<BoxExchangeDto>(
    `${this.apiUrl}/api/boxes/${boxId}/exchange-request`,
    request
  ).toPromise();
}

// Component usage
onSubmitExchangeRequest() {
  const request = {
    boxId: this.box.boxId,
    newBuildingNumber: this.form.value.building,
    newFloor: this.form.value.floor,
    requestReason: this.form.value.reason
  };
  
  this.boxService.createBoxExchangeRequest(
    this.box.boxId, 
    request
  ).subscribe(
    result => {
      this.showSuccessMessage(
        `Exchange request created. Quality Issue ${result.issueNumber} assigned to project creator.`
      );
      this.closeDialog();
    },
    error => {
      this.showErrorMessage(error.message);
    }
  );
}
```

### 3. Notification Handling

```typescript
// Listen for box exchange notifications
this.notificationService.notifications$.subscribe(notification => {
  if (notification.notificationType === 'BoxExchangeRequest') {
    this.showNotificationToast(notification);
  }
});
```

## Testing Checklist

- [ ] Create exchange request with new building only
- [ ] Create exchange request with new floor only
- [ ] Create exchange request with both building and floor
- [ ] Verify BoxTag uniqueness validation
- [ ] Verify Quality Issue is created with correct type
- [ ] Verify project creator receives notification
- [ ] Resolve Quality Issue and verify box is updated
- [ ] Close Quality Issue and verify box is updated
- [ ] Verify audit logs are created
- [ ] Test permission checks
- [ ] Test with invalid box ID
- [ ] Test with user who doesn't have access

## Benefits

1. **Approval Workflow**: Changes require project creator approval
2. **Audit Trail**: Complete history of all exchange requests
3. **Automatic Updates**: Box data updated automatically when approved
4. **Notifications**: Real-time alerts to relevant stakeholders
5. **Quality Integration**: Leverages existing Quality Issue system
6. **Data Integrity**: BoxTag uniqueness validation prevents conflicts

## Notes

- Exchange requests create Quality Issues that can be tracked like any other issue
- Project creator can add comments or request more information via Quality Issue comments
- If Quality Issue is rejected or cancelled, BoxExchange status remains `Pending`
- Only when Quality Issue is `Resolved` or `Closed` does the exchange apply
- All changes are logged in `AuditLogs` table for compliance

## Migration Instructions

1. Ensure latest code is pulled from repository
2. Run migration: 
   ```bash
   dotnet ef database update --project Dubox.Infrastructure --startup-project Dubox.Api
   ```
3. Verify `BoxExchanges` table is created
4. Test API endpoint with Postman/Swagger
5. Implement frontend components

### Note on Cascade Delete
To avoid SQL Server cascade path conflicts, all foreign keys to the `Users` table use `ON DELETE NO ACTION`. This prevents multiple cascade paths that SQL Server doesn't allow. The application layer handles data cleanup when needed.

## Future Enhancements

- [ ] Add ability to change other box properties (Zone, Function, etc.)
- [ ] Add bulk exchange requests
- [ ] Add exchange request history view in UI
- [ ] Add email notifications in addition to in-app notifications
- [ ] Add exchange request dashboard for project creators
- [ ] Add ability to withdraw/cancel pending exchange requests
