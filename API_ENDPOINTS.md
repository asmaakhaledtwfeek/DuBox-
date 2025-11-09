# DuBox API Endpoints Documentation

Complete API endpoint documentation for the DuBox Tracking System following the 4-level hierarchy.

---

## Authentication

All endpoints require authentication using JWT Bearer token except login/register endpoints.

```
Authorization: Bearer {your-jwt-token}
```

---

## 📁 Level 1: PROJECTS

### **GET** `/api/projects`
Get all projects

**Response:**
```json
{
  "isSuccess": true,
  "value": [
    {
      "projectId": "guid",
      "projectCode": "J142-AMAALA",
      "projectName": "AMAALA Staff Village",
      "clientName": "Red Sea Development Company",
      "location": "NEOM, Saudi Arabia",
      "startDate": "2024-01-01T00:00:00Z",
      "plannedEndDate": "2025-06-30T00:00:00Z",
      "actualEndDate": null,
      "status": "Active",
      "description": "Modular staff village project",
      "totalBoxes": 1340,
      "isActive": true,
      "createdDate": "2024-11-04T00:00:00Z",
      "createdBy": "admin"
    }
  ]
}
```

---

### **GET** `/api/projects/{projectId}`
Get project by ID

---

### **POST** `/api/projects`
Create a new project

**Request Body:**
```json
{
  "projectCode": "J142-AMAALA",
  "projectName": "AMAALA Staff Village",
  "clientName": "Red Sea Development Company",
  "location": "NEOM, Saudi Arabia",
  "startDate": "2024-01-01T00:00:00Z",
  "plannedEndDate": "2025-06-30T00:00:00Z",
  "description": "Modular construction project"
}
```

---

### **PUT** `/api/projects/{projectId}`
Update an existing project

---

### **DELETE** `/api/projects/{projectId}`
Delete a project (only if no boxes exist)

---

## 📦 Level 2: BOXES

### **GET** `/api/boxes`
Get all boxes

---

### **GET** `/api/boxes/{boxId}`
Get box by ID

---

### **GET** `/api/boxes/project/{projectId}`
Get all boxes for a specific project

---

### **GET** `/api/boxes/qr/{qrCodeString}`
🔍 **Mobile Scanning Endpoint** - Get box by QR code string

**Example:** `/api/boxes/qr/J142-AMAALA_B2-FF-L3-2`

**Response:**
```json
{
  "isSuccess": true,
  "value": {
    "boxId": "guid",
    "projectId": "guid",
    "projectCode": "J142-AMAALA",
    "boxTag": "B2-FF-L3-2",
    "boxName": "Building 2, First Floor, Living Room 3-2",
    "boxType": "Living Room",
    "floor": "FF",
    "building": "Building 2",
    "zone": "Zone A",
    "qrCodeString": "J142-AMAALA_B2-FF-L3-2",
    "qrCodeImageUrl": "https://storage.blob.core.windows.net/qr/...",
    "progressPercentage": 45.5,
    "status": "In Progress",
    "length": 6000,
    "width": 3000,
    "height": 2800,
    "unitOfMeasure": "mm",
    "bimModelReference": "REV-2024-001",
    "actualStartDate": "2024-11-01T08:00:00Z",
    "plannedEndDate": "2024-12-15T00:00:00Z",
    "actualEndDate": null,
    "createdDate": "2024-11-04T00:00:00Z"
  }
}
```

---

### **POST** `/api/boxes`
Create a single box manually

**Request Body:**
```json
{
  "projectId": "guid",
  "boxTag": "B1-GF-BED-01",
  "boxName": "Ground Floor Bedroom 1",
  "boxType": "Bedroom",
  "floor": "GF",
  "building": "Building 1",
  "zone": "Zone A",
  "length": 5000,
  "width": 3000,
  "height": 2800,
  "bimModelReference": "REV-2024-001",
  "revitElementId": "123456",
  "assets": [
    {
      "assetType": "Precast Wall",
      "assetCode": "PCW-001",
      "assetName": "Exterior Wall Panel",
      "quantity": 4,
      "unit": "pcs",
      "specifications": "3m x 2.8m, 150mm thickness"
    }
  ]
}
```

---

### **POST** `/api/boxes/import`
🚀 **Bulk Import** - Import multiple boxes from BIM or Excel

**Request Body:**
```json
{
  "projectId": "guid",
  "boxes": [
    {
      "boxTag": "B1-GF-BED-01",
      "boxName": "Ground Floor Bedroom 1",
      "boxType": "Bedroom",
      "floor": "GF",
      "building": "Building 1",
      "zone": "Zone A",
      "length": 5000,
      "width": 3000,
      "height": 2800,
      "assets": [...]
    },
    {
      "boxTag": "B1-GF-BED-02",
      ...
    }
  ]
}
```

**Features:**
- ✅ Auto-generates QR code strings
- ✅ Auto-copies activities from ActivityMaster
- ✅ Skips duplicate boxes
- ✅ Handles assets in bulk

---

### **PUT** `/api/boxes/{boxId}`
Update box details

---

### **DELETE** `/api/boxes/{boxId}`
Delete a box

---

## 🎯 Level 3: ACTIVITIES

### **GET** `/api/activities/masters`
Get all 36 activity master templates (seeded data)

**Response:**
```json
{
  "isSuccess": true,
  "value": [
    {
      "activityMasterId": "guid",
      "activityCode": "STAGE1-FAB",
      "activityName": "Fabrication of boxes",
      "stage": "Stage 1: Precast Production",
      "stageNumber": 1,
      "sequenceInStage": 1,
      "overallSequence": 1,
      "description": "Manufacturing and fabrication of precast box components",
      "estimatedDurationDays": 3,
      "isWIRCheckpoint": false,
      "wirCode": null,
      "applicableBoxTypes": null,
      "isActive": true
    },
    ...
  ]
}
```

---

### **GET** `/api/activities/masters/stage/{stageNumber}`
Get activities by stage (1-6)

**Example:** `/api/activities/masters/stage/2`

---

### **GET** `/api/activities/box/{boxId}`
Get all activities for a specific box

**Response:**
```json
{
  "isSuccess": true,
  "value": [
    {
      "boxActivityId": "guid",
      "boxId": "guid",
      "boxTag": "B2-FF-L3-2",
      "activityMasterId": "guid",
      "activityCode": "STAGE2-POD",
      "activityName": "PODS installation",
      "stage": "Stage 2: Module Assembly",
      "sequence": 5,
      "status": "In Progress",
      "progressPercentage": 75,
      "plannedStartDate": "2024-11-05T00:00:00Z",
      "plannedEndDate": "2024-11-06T00:00:00Z",
      "actualStartDate": "2024-11-05T08:00:00Z",
      "actualEndDate": null,
      "workDescription": "Installing bathroom PODs",
      "issuesEncountered": null,
      "assignedTeam": "Assembly Team A",
      "materialsAvailable": true,
      "isWIRCheckpoint": false,
      "wirCode": null
    },
    ...
  ]
}
```

---

### **PUT** `/api/activities/{boxActivityId}`
Update activity progress and status

**Request Body:**
```json
{
  "boxActivityId": "guid",
  "status": "In Progress",
  "progressPercentage": 75,
  "workDescription": "Installed 3 out of 4 PODs",
  "issuesEncountered": null,
  "assignedTeam": "Assembly Team A",
  "materialsAvailable": true
}
```

**Auto-triggers:**
- ✅ Updates box overall progress
- ✅ Sets ActualStartDate if status changes to "In Progress"
- ✅ Sets ActualEndDate if status changes to "Completed"
- ✅ Creates WIR record if activity is a checkpoint and completed

---

## 📊 Level 4: TRACK (Progress Updates)

### **POST** `/api/progressupdates`
📱 **Mobile/Web Update** - Create a progress update

**Request Body:**
```json
{
  "boxId": "guid",
  "boxActivityId": "guid",
  "progressPercentage": 100,
  "status": "Completed",
  "workDescription": "All PODs installed and sealed",
  "issuesEncountered": null,
  "latitude": 28.3949,
  "longitude": 34.9615,
  "locationDescription": "Assembly Bay 3",
  "photoUrls": [
    "https://storage.blob.core.windows.net/photos/photo1.jpg",
    "https://storage.blob.core.windows.net/photos/photo2.jpg"
  ],
  "updateMethod": "Mobile",
  "deviceInfo": "iPhone 14 Pro - iOS 17.0"
}
```

**Auto-triggers:**
- ✅ Creates ProgressUpdate audit record
- ✅ Updates BoxActivity (progress, status, dates)
- ✅ Recalculates Box overall progress
- ✅ Updates Box status (if all activities 100% → Completed)
- ✅ Checks for WIR checkpoints (creates WIRRecord if needed)
- ✅ Tracks GPS location
- ✅ Stores photos
- ✅ Records update method (Mobile/Web)

**Response:**
```json
{
  "isSuccess": true,
  "value": {
    "progressUpdateId": "guid",
    "boxId": "guid",
    "boxTag": "B2-FF-L3-2",
    "boxActivityId": "guid",
    "activityName": "PODS installation",
    "updateDate": "2024-11-04T16:00:00Z",
    "updatedBy": "guid",
    "updatedByName": "Ahmed Mohammed",
    "progressPercentage": 100,
    "status": "Completed",
    "workDescription": "All PODs installed and sealed",
    "issuesEncountered": null,
    "latitude": 28.3949,
    "longitude": 34.9615,
    "locationDescription": "Assembly Bay 3",
    "photoUrls": "https://...,https://...",
    "updateMethod": "Mobile"
  }
}
```

---

### **GET** `/api/progressupdates/box/{boxId}`
Get all progress updates for a specific box

---

### **GET** `/api/progressupdates/activity/{boxActivityId}`
Get all progress updates for a specific activity

---

## ✅ WIR Records (Work Inspection Requests)

### **GET** `/api/wirrecords`
Get all WIR records

---

### **GET** `/api/wirrecords/{wirRecordId}`
Get WIR record by ID

---

### **GET** `/api/wirrecords/box/{boxId}`
Get all WIR records for a specific box

---

### **POST** `/api/wirrecords`
Create a new WIR record

**Request Body:**
```json
{
  "boxActivityId": "guid",
  "wirCode": "WIR-1",
  "photoUrls": "https://storage.blob.core.windows.net/wir/photo1.jpg"
}
```

---

### **POST** `/api/wirrecords/{wirRecordId}/approve`
✅ **QC Inspector** - Approve a WIR record

**Request Body:**
```json
{
  "wirRecordId": "guid",
  "inspectionNotes": "All work completed to specification. Approved.",
  "photoUrls": "https://storage.blob.core.windows.net/wir/inspection1.jpg"
}
```

---

### **POST** `/api/wirrecords/{wirRecordId}/reject`
❌ **QC Inspector** - Reject a WIR record

**Request Body:**
```json
{
  "wirRecordId": "guid",
  "rejectionReason": "Gaps found in POD sealing. Rework required.",
  "inspectionNotes": "See attached photos for details."
}
```

**Auto-triggers:**
- ✅ Updates BoxActivity status to "On Hold"
- ✅ Adds rejection reason to IssuesEncountered

---

## 📈 Dashboard & Statistics

### **GET** `/api/dashboard/statistics`
Get overall system statistics

**Response:**
```json
{
  "isSuccess": true,
  "value": {
    "totalProjects": 5,
    "activeProjects": 3,
    "totalBoxes": 2500,
    "boxesNotStarted": 450,
    "boxesInProgress": 1800,
    "boxesCompleted": 250,
    "boxesDelayed": 0,
    "overallProgress": 62.5,
    "pendingWIRs": 12,
    "totalActivities": 90000,
    "completedActivities": 56250
  }
}
```

---

### **GET** `/api/dashboard/projects`
Get dashboard data for all projects

**Response:**
```json
{
  "isSuccess": true,
  "value": [
    {
      "projectId": "guid",
      "projectCode": "J142-AMAALA",
      "projectName": "AMAALA Staff Village",
      "totalBoxes": 1340,
      "boxesNotStarted": 200,
      "boxesInProgress": 950,
      "boxesCompleted": 190,
      "progressPercentage": 68.5,
      "pendingWIRs": 5,
      "startDate": "2024-01-01T00:00:00Z",
      "plannedEndDate": "2025-06-30T00:00:00Z",
      "status": "Active"
    },
    ...
  ]
}
```

---

### **GET** `/api/dashboard/projects/{projectId}`
Get dashboard data for a specific project

---

## 🔐 Authentication Endpoints

### **POST** `/api/auth/register`
Register a new user

**Request Body:**
```json
{
  "email": "ahmed@example.com",
  "password": "SecurePass123!",
  "fullName": "Ahmed Mohammed",
  "department": "Production"
}
```

---

### **POST** `/api/auth/login`
Login

**Request Body:**
```json
{
  "email": "ahmed@example.com",
  "password": "SecurePass123!"
}
```

**Response:**
```json
{
  "isSuccess": true,
  "value": {
    "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
    "email": "ahmed@example.com",
    "fullName": "Ahmed Mohammed"
  }
}
```

---

### **POST** `/api/auth/change-password`
Change password

---

## 👥 User Management

### **GET** `/api/users`
Get all users

---

### **GET** `/api/users/{userId}`
Get user by ID

---

### **PUT** `/api/users/{userId}`
Update user

---

### **GET** `/api/users/{userId}/roles`
Get user roles

---

### **POST** `/api/users/{userId}/roles`
Assign roles to user

---

### **POST** `/api/users/{userId}/groups`
Assign user to groups

---

## 🏢 Groups & Roles

### **GET** `/api/groups`
Get all groups

---

### **POST** `/api/groups`
Create group

---

### **POST** `/api/groups/{groupId}/roles`
Assign roles to group

---

### **GET** `/api/roles`
Get all roles

---

### **POST** `/api/roles`
Create role

---

## 📱 Mobile App Integration

### Typical Mobile Scanning Flow

1. **Scan QR Code** → Extract QR string (e.g., `J142-AMAALA_B2-FF-L3-2`)

2. **Get Box Details:**
   ```
   GET /api/boxes/qr/J142-AMAALA_B2-FF-L3-2
   ```

3. **Get Box Activities:**
   ```
   GET /api/activities/box/{boxId}
   ```

4. **Create Progress Update:**
   ```
   POST /api/progressupdates
   {
     "boxId": "guid",
     "boxActivityId": "guid",
     "progressPercentage": 100,
     "status": "Completed",
     "workDescription": "Work completed",
     "updateMethod": "Mobile",
     "latitude": 28.3949,
     "longitude": 34.9615,
     "photoUrls": ["https://..."]
   }
   ```

5. **System Auto-Actions:**
   - ✅ Updates BoxActivity
   - ✅ Recalculates Box progress
   - ✅ Creates WIR if checkpoint
   - ✅ Updates timestamps
   - ✅ Stores audit trail

---

## 📊 Key Features Implemented

### ✅ Level 1: Projects (CRUD)
- Full CRUD operations
- Project statistics
- Box count tracking

### ✅ Level 2: Boxes (Import)
- Bulk import from Excel/BIM
- QR code generation
- Manual entry
- Asset tracking

### ✅ Level 3: Activities (Auto-Copy)
- 36 pre-seeded activity templates
- Auto-copy to boxes on creation
- Box type filtering
- Activity progress tracking

### ✅ Level 4: Track (Real-time)
- Mobile QR scanning
- GPS location tracking
- Photo documentation
- Progress audit trail
- Auto-update cascading
- WIR checkpoint automation

### ✅ Additional Features
- Dashboard statistics
- WIR approval workflow
- User management
- Role-based access
- Authentication & Authorization

---

## 🎯 Next Steps

1. **Test all endpoints** using Postman or Swagger
2. **Add Swagger documentation** (already configured in Program.cs)
3. **Implement QR Code generation service**
4. **Add file upload for photos**
5. **Implement SignalR for real-time notifications**
6. **Create Excel import template parser**
7. **Build mobile app** (Flutter/React Native)

---

## 📝 Notes

- All endpoints return standardized `Result<T>` wrapper
- All dates are in UTC format
- All endpoints require authentication except auth endpoints
- Progress updates trigger multiple auto-actions
- Box activities are automatically created from templates
- WIR records are auto-created at checkpoints

