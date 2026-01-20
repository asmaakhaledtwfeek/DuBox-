# DuBox Backend - Changes Log

## 📦 Panel System Enhancement (2026-01-20)

### Overview
Implemented comprehensive panel management system with dynamic panel types, barcode scanning, two-stage approval workflow, and delivery note tracking.

### ✅ New Entities Created

1. **PanelType** (`Dubox.Domain/Entities/PanelType.cs`)
   - Dynamic panel type configuration per project
   - Fields: PanelTypeId, ProjectId, PanelTypeName, PanelTypeCode, Description, IsActive, DisplayOrder
   - Enables Excel-like flexibility for panel configuration

2. **PanelDeliveryNote** (`Dubox.Domain/Entities/PanelDeliveryNote.cs`)
   - Delivery note management with QR codes
   - Fields: DeliveryNoteId, DeliveryNoteNumber, SupplierName, DriverName, VehicleNumber, DeliveryDate, QRCodeUrl, DocumentUrl, Status
   - Links to multiple panels

3. **PanelScanLog** (`Dubox.Domain/Entities/PanelScanLog.cs`)
   - Track all barcode scans with GPS
   - Fields: ScanLogId, BoxPanelId, Barcode, ScanType, ScanLocation, ScannedBy, ScannedDate, Latitude, Longitude

### ✅ Enhanced Entities

**BoxPanel** (`Dubox.Domain/Entities/BoxPanel.cs`)
- Added ~20 new fields:
  - Identification: Barcode, QRCodeUrl, PanelTypeId
  - Manufacturing: ManufacturerName, ManufacturedDate
  - Delivery: DispatchedDate, EstimatedArrivalDate, ActualArrivalDate, DeliveryNoteNumber, DeliveryNoteUrl
  - First Approval: FirstApprovalStatus, FirstApprovalBy, FirstApprovalDate, FirstApprovalNotes
  - Second Approval: SecondApprovalStatus, SecondApprovalBy, SecondApprovalDate, SecondApprovalNotes
  - Location: CurrentLocationStatus, ScannedAtFactory, InstalledDate
  - Physical: Weight, Dimensions

### ✅ Updated Enums

**PanelStatusEnum** (`Dubox.Domain/Enums/PanelStatusEnum.cs`)
```csharp
NotStarted = 1
Manufacturing = 2
ReadyForDispatch = 3
InTransit = 4 (Yellow - on way to factory)
ArrivedFactory = 5 (Green - arrived at factory)
FirstApprovalPending = 6
FirstApprovalApproved = 7
FirstApprovalRejected = 8
SecondApprovalPending = 9
SecondApprovalApproved = 10 (Ready for installation)
SecondApprovalRejected = 11
Installed = 12
Rejected = 13
// Backward compatibility
Yellow = InTransit
Green = ArrivedFactory
```

### ✅ New Services

**IBarcodeService** & **BarcodeService**
- `Dubox.Domain/Services/IBarcodeService.cs`
- `Dubox.Application/Services/BarcodeService.cs`
- Generate unique barcodes (Format: PNL-{ProjectCode}-{ShortGuid})
- Generate QR code images
- Validate and parse barcodes

### ✅ New DTOs

**PanelTypeDto, CreatePanelTypeDto, UpdatePanelTypeDto**
- `Dubox.Application/DTOs/PanelTypeDto.cs`

**Enhanced BoxPanelDto**
- `Dubox.Application/DTOs/BoxPanelDto.cs`
- Added all new tracking fields

### ✅ Panel Type Management (CRUD)

**Commands** (`Dubox.Application/Features/PanelTypes/Commands/`)
- `CreatePanelTypeCommand` + Handler
- `UpdatePanelTypeCommand` + Handler
- `DeletePanelTypeCommand` + Handler

**Queries** (`Dubox.Application/Features/PanelTypes/Queries/`)
- `GetPanelTypesByProjectQuery` + Handler
- `GetPanelTypeByIdQuery` + Handler

### ✅ Panel Scanning Functionality

**ScanPanelBarcodeCommand** (`Dubox.Application/Features/BoxPanels/Commands/`)
- Scan barcode to update panel status
- Create scan log with GPS coordinates
- Auto-update status based on scan type:
  - Dispatch → InTransit (Yellow)
  - FactoryArrival → ArrivedFactory → FirstApprovalPending
  - Installation → Installed

### ✅ Two-Stage Approval Workflow

**First Approval** (`ApprovePanelFirstApprovalCommand`)
- Quality check approval
- Records approver, date, notes
- Approved → Auto-set SecondApprovalPending
- Rejected → Mark as Rejected

**Second Approval** (`ApprovePanelSecondApprovalCommand`)
- Installation ready approval
- Validates first approval completed
- Approved → SecondApprovalApproved (Ready for installation)
- Rejected → Mark as Rejected

### 🔨 Next Steps

**Database:**
- Create migration for new tables: PanelTypes, PanelDeliveryNotes, PanelScanLogs
- Alter BoxPanels table with new columns

**API:**
- Create PanelTypesController
- Add panel endpoints to BoxesController
- Create DeliveryNotesController

**Frontend:**
- Update factory-walls-status component
- Create panel type management UI
- Create barcode scanner component
- Create approval workflow UI
- Create delivery note management UI

### 📚 Documentation
- `PANEL_SYSTEM_IMPLEMENTATION.md` - Full implementation plan
- `PANEL_IMPLEMENTATION_SUMMARY.md` - Progress summary

---

## ✅ Role Integration Changes (Previous)

## ✅ Changes Made

### 1. Updated `UserDto.cs`
**Location:** `Dubox.Application/DTOs/UserDto.cs`

**Changes:**
- Added `Department` property (string) for department name
- Added `DirectRoles` property (List<string>) for roles directly assigned to user
- Added `Groups` property (List<GroupWithRolesDto>) for groups with their roles
- Added `AllRoles` property (List<string>) for combined direct + inherited roles

**Before:**
```csharp
public record UserDto
{
    public Guid UserId { get; init; }
    public string Email { get; init; } = string.Empty;
    public string? FullName { get; init; }
    public Guid? DepartmentId { get; init; }
    public bool IsActive { get; init; }
    public DateTime? LastLoginDate { get; init; }
    public DateTime CreatedDate { get; init; }
}
```

**After:**
```csharp
public record UserDto
{
    public Guid UserId { get; init; }
    public string Email { get; init; } = string.Empty;
    public string? FullName { get; init; }
    public Guid? DepartmentId { get; init; }
    public string? Department { get; init; }
    public bool IsActive { get; init; }
    public DateTime? LastLoginDate { get; init; }
    public DateTime CreatedDate { get; init; }
    
    // Role and Group information for frontend
    public List<string> DirectRoles { get; init; } = new();
    public List<GroupWithRolesDto> Groups { get; init; } = new();
    public List<string> AllRoles { get; init; } = new();
}
```

---

### 2. Updated `AuthDto.cs`
**Location:** `Dubox.Application/DTOs/AuthDto.cs`

**Changes:**
- Added `RefreshToken` property
- Added `ExpiresIn` property (default 3600 seconds = 1 hour)

**Before:**
```csharp
public record LoginResponseDto
{
    public string Token { get; init; } = string.Empty;
    public UserDto User { get; init; } = null!;
}
```

**After:**
```csharp
public record LoginResponseDto
{
    public string Token { get; init; } = string.Empty;
    public string RefreshToken { get; init; } = string.Empty;
    public int ExpiresIn { get; init; } = 3600;
    public UserDto User { get; init; } = null!;
}
```

---

### 3. Updated `LoginCommandHandler.cs`
**Location:** `Dubox.Application/Features/Auth/Commands/LoginCommandHandler.cs`

**Major Changes:**
1. **Added Entity Includes**: Load user with Department, UserRoles, UserGroups relationships
2. **Extract Direct Roles**: Get roles directly assigned to the user
3. **Extract Groups**: Get user's groups with their associated roles
4. **Calculate All Roles**: Combine direct roles + group roles (distinct)
5. **Populate UserDto**: Include all role and group information

**Key Code Added:**
```csharp
// Load user with all relationships
var user = await _context.Users
    .Include(u => u.Department)
    .Include(u => u.UserRoles)
        .ThenInclude(ur => ur.Role)
    .Include(u => u.UserGroups)
        .ThenInclude(ug => ug.Group)
            .ThenInclude(g => g.GroupRoles)
                .ThenInclude(gr => gr.Role)
    .FirstOrDefaultAsync(u => u.Email == request.Email, cancellationToken);

// Extract direct roles
var directRoles = user.UserRoles
    .Where(ur => ur.Role != null)
    .Select(ur => ur.Role!.RoleName)
    .ToList();

// Extract groups with their roles
var groups = user.UserGroups
    .Where(ug => ug.Group != null)
    .Select(ug => new GroupWithRolesDto { ... })
    .ToList();

// Combine all roles (direct + from groups)
var allRoles = directRoles
    .Concat(groups.SelectMany(g => g.Roles.Select(r => r.RoleName)))
    .Distinct()
    .ToList();
```

---

## 🔍 What the Response Looks Like Now

### Example Login Response

```json
{
  "data": {
    "token": "eyJhbGci...",
    "refreshToken": "eyJhbGci...",
    "expiresIn": 3600,
    "user": {
      "userId": "f1111111-1111-1111-...",
      "email": "admin@groupamana.com",
      "fullName": "System Administrator",
      "departmentId": "d1000000-0000-...",
      "department": "IT",
      "isActive": true,
      "lastLoginDate": "2025-11-16T...",
      "createdDate": "2024-11-01T...",
      "directRoles": [
        "SystemAdmin"
      ],
      "groups": [
        {
          "groupId": "g1000000-0000-...",
          "groupName": "Management",
          "roles": [
            {
              "roleId": "r1000000-0000-...",
              "roleName": "SystemAdmin",
              "description": "Full system administration access",
              "isActive": true,
              "createdDate": "2024-11-01T..."
            },
            {
              "roleId": "r2000000-0000-...",
              "roleName": "ProjectManager",
              "description": "Manage projects and teams",
              "isActive": true,
              "createdDate": "2024-11-01T..."
            }
          ]
        }
      ],
      "allRoles": [
        "SystemAdmin",
        "ProjectManager"
      ]
    }
  },
  "isSuccess": true,
  "isFailure": false,
  "message": "Success"
}
```

---

## 🧪 Testing

### Test 1: Login as Admin
```bash
POST https://localhost:7098/api/auth/login
{
  "email": "admin@groupamana.com",
  "password": "AMANA@2024"
}
```

**Expected:**
- `directRoles`: Should contain direct role assignments
- `groups`: Should contain all groups with their roles
- `allRoles`: Should contain combined distinct roles

### Test 2: Login as User with Group Roles
```bash
POST https://localhost:7098/api/auth/login
{
  "email": "mohammed.hassan@groupamana.com",
  "password": "AMANA@2024"
}
```

**Expected:**
- `directRoles`: ["ProjectManager"] (direct assignment)
- `groups[0].roles`: Should include ["SiteEngineer", "DesignEngineer", "Viewer"] from Engineering group
- `allRoles`: ["ProjectManager", "SiteEngineer", "DesignEngineer", "Viewer"]

---

## ✅ Frontend Integration

### What Changed for Frontend

1. **Response Structure**: Now matches what Angular expects
2. **Role Information**: Includes `allRoles`, `directRoles`, and `groups`
3. **Department Name**: Includes department name (not just ID)
4. **Token Info**: Includes refresh token and expiration time

### Frontend Can Now:

1. **Remove Temporary Workaround**: Delete the email-based role assignment code
2. **Use Real Roles**: Access `user.allRoles` for permission checks
3. **Show Role Details**: Display direct vs. inherited roles
4. **Show Groups**: Display user's group memberships

---

## 🔄 Role Inheritance Flow

```
User Login
    ↓
Load User Entity (with includes)
    ↓
Extract Direct Roles
    ├── UserRoles → Role.RoleName
    └── Example: ["ProjectManager"]
    ↓
Extract Groups & Their Roles
    ├── UserGroups → Group
    ├── Group → GroupRoles → Role.RoleName
    └── Example: Management group → ["SystemAdmin", "ProjectManager"]
    ↓
Combine & Deduplicate
    ├── DirectRoles + Group Roles
    ├── .Distinct()
    └── AllRoles: ["SystemAdmin", "ProjectManager"]
    ↓
Return to Frontend
```

---

## 📋 Database Requirements

Make sure these relationships exist:

1. **User → UserRoles → Role**
2. **User → UserGroups → Group**
3. **Group → GroupRoles → Role**
4. **User → Department**

All should be properly seeded with the Group AMANA data.

---

## 🚀 Next Steps

1. **Build the Backend**: Rebuild the solution
2. **Test Login**: Use Postman/Swagger to test login endpoint
3. **Verify Response**: Check that roles and groups are included
4. **Update Frontend**: Remove temporary workaround in `auth.service.ts`
5. **Test Frontend**: Login and verify permissions work

---

## 🐛 Troubleshooting

### Issue: Roles are null or empty

**Solution**: Check that:
1. Users have been assigned roles via `AssignRolesToUserCommand`
2. Users have been assigned to groups via `AssignUserToGroupsCommand`
3. Groups have roles assigned via `AssignRolesToGroupCommand`
4. Database relationships are properly configured

### Issue: Navigation properties are null

**Solution**: Verify Entity Framework Core configuration:
- Check that entities have proper navigation properties
- Ensure DbContext includes the relationships
- Verify migrations have created foreign keys

---

## 📝 Code Review Checklist

- [x] Updated DTOs with role information
- [x] Updated login handler to load roles and groups
- [x] Added Entity Framework includes for relationships
- [x] Extract direct roles from UserRoles
- [x] Extract groups and their roles from UserGroups
- [x] Combine and deduplicate all roles
- [x] Return department name (not just ID)
- [x] Added refresh token to response
- [x] Added expiration time to response

---

## 💡 Additional Notes

- **Performance**: The includes add some overhead but are necessary for one query
- **Caching**: Consider caching user roles if performance becomes an issue
- **Refresh Token**: Currently returns same as access token - implement proper refresh token logic if needed
- **Expiration**: Set to 1 hour (3600 seconds) - adjust as needed

---

© 2024 Group AMANA - DuBox Platform

