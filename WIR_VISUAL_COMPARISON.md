# 🎨 WIR Display - Visual Comparison

## 📊 Process Flow: How It Should Work

```
┌──────────────────────────────────────────────────────────────────┐
│                       ACTIVITY WORKFLOW                          │
└──────────────────────────────────────────────────────────────────┘

Step 1: Complete Checkpoint Activity
┌────────────────────────────────────────────────────┐
│  Activity #4: Box Closure                         │
│  Progress: 0% → 50% → 100% ✓                     │
│  Status: Not Started → In Progress → Completed   │
└────────────────────────────────────────────────────┘
                      ↓
                 (100% Reached)
                      ↓
Step 2: Backend Auto-Creates WIR Record
┌────────────────────────────────────────────────────┐
│  CREATE WIR-1 Record:                             │
│  - BoxActivityId: Activity #4                     │
│  - WIRCode: "WIR-1"                               │
│  - Status: Pending                                │
│  - RequestedBy: Current User                      │
│  - RequestedDate: Now                             │
└────────────────────────────────────────────────────┘
                      ↓
Step 3: Frontend Displays WIR Row
┌────────────────────────────────────────────────────┐
│  🟡 WIR-1 Row (Yellow Highlighted)                │
│  Team: QA/QC                                      │
│  Activity: ⚠️ Clearance/WIR                      │
│  Status: Pending                                  │
│  Action: [QA/QC Button]                           │
└────────────────────────────────────────────────────┘
                      ↓
Step 4: QC Engineer Approves/Rejects
┌────────────────────────────────────────────────────┐
│  QC Engineer clicks "QA/QC" button               │
│  Reviews checklist items                          │
│  → Approves: Status = Approved ✓                 │
│  → Rejects: Status = Rejected ✗                  │
└────────────────────────────────────────────────────┘
```

---

## 🎯 Frontend Display Comparison

### ❌ BEFORE FIX (Incorrect)

```
┌─────┬────────┬────────────────────────┬──────────┬────────┬──────────┬─────────┐
│ ID  │ Team   │ Activity               │ Assigned │ Prog.  │ Status   │ Actions │
├─────┼────────┼────────────────────────┼──────────┼────────┼──────────┼─────────┤
│ 1   │ Civil  │ Assembly & joints      │ Foreman  │ 100%   │ Complete │ [Edit]  │
│ 2   │ Civil  │ PODS installation      │ Foreman  │ 100%   │ Complete │ [Edit]  │
│ 3   │ MEP    │ MEP Cage installation  │ Foreman  │ 80%    │ Progress │ [Edit]  │
│ 4   │ Civil  │ Box Closure            │ Foreman  │ 80%    │ Progress │ [Edit]  │
│ 5   │ QA/QC  │ WIR-1                  │ Engineer │ 0%     │ Pending  │ [Edit]  │ ← WRONG
│ 6   │ MEP    │ Ducts & Insulation     │ Foreman  │ 0%     │ NotStart │ [Edit]  │
│ 7   │ MEP    │ Drainage piping        │ Foreman  │ 0%     │ NotStart │ [Edit]  │
│ 8   │ MEP    │ Water Piping           │ Foreman  │ 0%     │ NotStart │ [Edit]  │
│ 9   │ MEP    │ Fire Fighting Piping   │ Foreman  │ 0%     │ NotStart │ [Edit]  │
│ 10  │ QA/QC  │ WIR-2                  │ Engineer │ 0%     │ Pending  │ [Edit]  │ ← WRONG
└─────┴────────┴────────────────────────┴──────────┴────────┴──────────┴─────────┘

Problems:
❌ WIR appears as regular activity (ID 5, 10, etc.)
❌ WIR has white background (not distinctive)
❌ WIR can be edited like regular activity
❌ WIR appears even when Activity 4 is not 100% complete
❌ Total 43 activities (too many)
```

### ✅ AFTER FIX (Correct)

```
┌─────┬────────┬────────────────────────┬──────────┬────────┬──────────┬─────────┐
│ ID  │ Team   │ Activity               │ Assigned │ Prog.  │ Status   │ Actions │
├─────┼────────┼────────────────────────┼──────────┼────────┼──────────┼─────────┤
│ 1   │ Civil  │ Assembly & joints      │ Foreman  │ 100%   │ Complete │ [Edit]  │
│ 2   │ Civil  │ PODS installation      │ Foreman  │ 100%   │ Complete │ [Edit]  │
│ 3   │ MEP    │ MEP Cage installation  │ Foreman  │ 80%    │ Progress │ [Edit]  │
│ 4   │ Civil  │ Box Closure            │ Foreman  │ 100%   │ Complete │ [Edit]  │
├═════╪════════╪════════════════════════╪══════════╪════════╪══════════╪═════════┤
│🔶🟡 │ QA/QC  │ ⚠️ Clearance/WIR       │ Engineer │   -    │ Pending  │  [QC]   │ ← WIR-1
│WIR-1│        │                        │          │        │          │         │   (Yellow)
├═════╪════════╪════════════════════════╪══════════╪════════╪══════════╪═════════┤
│ 5   │ MEP    │ Ducts & Insulation     │ Foreman  │ 0%     │ NotStart │ [Edit]  │
│ 6   │ MEP    │ Drainage piping        │ Foreman  │ 0%     │ NotStart │ [Edit]  │
│ 7   │ MEP    │ Water Piping           │ Foreman  │ 0%     │ NotStart │ [Edit]  │
│ 8   │ MEP    │ Fire Fighting Piping   │ Foreman  │ 0%     │ NotStart │ [Edit]  │
├═════╪════════╪════════════════════════╪══════════╪════════╪══════════╪═════════┤
│🔶🟡 │ QA/QC  │ ⚠️ Clearance/WIR       │ Engineer │   -    │ Pending  │  [QC]   │ ← WIR-2
│WIR-2│        │                        │          │        │          │         │   (Yellow)
├═════╪════════╪════════════════════════╪══════════╪════════╪══════════╪═════════┤
│ ...  │ ...   │ ...                    │ ...      │ ...    │ ...      │ ...     │
└─────┴────────┴────────────────────────┴──────────┴────────┴──────────┴─────────┘

Improvements:
✅ WIR appears ONLY after checkpoint activity reaches 100%
✅ WIR has yellow/orange gradient background (distinctive)
✅ WIR shows as separate inspection step (not activity)
✅ WIR has "QA/QC" button (not "Edit")
✅ WIR has no progress percentage (inspection is binary: pending/approved/rejected)
✅ Total 28 activities + dynamic WIR records
```

---

## 🎨 Color Scheme

### Regular Activity Rows:
```
Background: White (#FFFFFF) / Light Gray (#F9F9F9) alternating
Border: Light Gray (#DDD)
Text: Dark Gray (#333)
```

### WIR Checkpoint Rows:
```
Background: Yellow/Orange Gradient
  - Start: #FFE082 (Light Yellow)
  - End: #FFCC80 (Light Orange)
Left Border: 4px Solid Orange (#FF9800)
Box Shadow: 0 2px 4px rgba(255, 152, 0, 0.2)
Text: Black (#000) - Bold (700)

Hover State:
  - Background: Darker Yellow/Orange (#FFD54F → #FFB74D)
  - Shadow: 0 3px 8px rgba(255, 152, 0, 0.3)
```

### WIR Status Badges:

**Pending:**
```css
Background: #fff3cd (Light Yellow)
Color: #856404 (Dark Brown)
Border: 2px solid #ffc107 (Amber)
Text: "PENDING" (uppercase, bold)
```

**Approved:**
```css
Background: #d4edda (Light Green)
Color: #155724 (Dark Green)
Border: 2px solid #28a745 (Green)
Text: "APPROVED" (uppercase, bold)
Icon: ✓
```

**Rejected:**
```css
Background: #f8d7da (Light Red)
Color: #721c24 (Dark Red)
Border: 2px solid #dc3545 (Red)
Text: "REJECTED" (uppercase, bold)
Icon: ✗
```

---

## 📋 Data Structure Comparison

### ❌ BEFORE (Incorrect)

**ActivityMaster Table:**
```json
[
  { "id": 1, "name": "Assembly & joints", "sequence": 1 },
  { "id": 2, "name": "PODS installation", "sequence": 2 },
  { "id": 3, "name": "MEP Cage", "sequence": 3 },
  { "id": 4, "name": "Box Closure", "sequence": 4, "isWIRCheckpoint": false },
  { "id": 5, "name": "WIR-1", "sequence": 5, "isWIRCheckpoint": true }, // ❌ WRONG
  { "id": 6, "name": "Ducts & Insulation", "sequence": 6 },
  ...
  // Total: 43 activities
]
```

**BoxActivities Table (for Box B1-001):**
```json
[
  { "id": 101, "activityMasterId": 1, "boxId": "B1-001", "progress": 100 },
  { "id": 102, "activityMasterId": 2, "boxId": "B1-001", "progress": 100 },
  { "id": 103, "activityMasterId": 3, "boxId": "B1-001", "progress": 80 },
  { "id": 104, "activityMasterId": 4, "boxId": "B1-001", "progress": 80 },
  { "id": 105, "activityMasterId": 5, "boxId": "B1-001", "progress": 0 }, // ❌ WIR as activity
  ...
  // Total: 43 box activities
]
```

**WIRRecords Table:**
```json
[] // Empty - WIR not created
```

---

### ✅ AFTER (Correct)

**ActivityMaster Table:**
```json
[
  { "id": 1, "name": "Assembly & joints", "sequence": 1 },
  { "id": 2, "name": "PODS installation", "sequence": 2 },
  { "id": 3, "name": "MEP Cage", "sequence": 3 },
  { "id": 4, "name": "Box Closure", "sequence": 4, "isWIRCheckpoint": true, "wirCode": "WIR-1" }, // ✅
  { "id": 5, "name": "Ducts & Insulation", "sequence": 5 },
  ...
  // Total: 28 activities (no WIR activities)
]
```

**BoxActivities Table (for Box B1-001):**
```json
[
  { "id": 101, "activityMasterId": 1, "boxId": "B1-001", "progress": 100 },
  { "id": 102, "activityMasterId": 2, "boxId": "B1-001", "progress": 100 },
  { "id": 103, "activityMasterId": 3, "boxId": "B1-001", "progress": 80 },
  { "id": 104, "activityMasterId": 4, "boxId": "B1-001", "progress": 100 }, // ✅ 100% triggers WIR
  { "id": 105, "activityMasterId": 5, "boxId": "B1-001", "progress": 0 },
  ...
  // Total: 28 box activities
]
```

**WIRRecords Table:**
```json
[
  {
    "wirRecordId": "wir-001",
    "boxActivityId": "104", // Links to Activity #4
    "wirCode": "WIR-1",
    "status": "Pending",
    "requestedDate": "2024-11-19T10:30:00Z",
    "requestedBy": "user-001",
    "checklistItems": [
      { "description": "Box dimensions verified", "status": "Pending" },
      { "description": "Joint connections secure", "status": "Pending" },
      ...
    ]
  }
  // WIR-2, WIR-3, etc. created dynamically when checkpoints reached
]
```

---

## 🔄 Frontend Flow Diagram

```
┌────────────────────────────────────────────────────────────────┐
│  Component: activity-table.component.ts                       │
└────────────────────────────────────────────────────────────────┘
                           │
                           ├─► Step 1: Load Activities
                           │   ┌──────────────────────────────┐
                           │   │ boxActivityService           │
                           │   │ .getActivitiesByBox(boxId)   │
                           │   │ → Returns 28 activities      │
                           │   └──────────────────────────────┘
                           │
                           ├─► Step 2: Load WIR Records
                           │   ┌──────────────────────────────┐
                           │   │ wirService                   │
                           │   │ .getWIRRecordsByBox(boxId)   │
                           │   │ → Returns WIR records        │
                           │   └──────────────────────────────┘
                           │
                           ├─► Step 3: Build Table Rows
                           │   ┌──────────────────────────────┐
                           │   │ buildTableRows()             │
                           │   │ 1. Sort activities by seq    │
                           │   │ 2. For each activity:        │
                           │   │    - Add activity row        │
                           │   │    - Check if WIR exists     │
                           │   │    - If yes, add WIR row     │
                           │   │ 3. Result: Combined array    │
                           │   └──────────────────────────────┘
                           │
                           ├─► Step 4: Render Table
                           │   ┌──────────────────────────────┐
                           │   │ *ngFor="let row of tableRows"│
                           │   │                              │
                           │   │ *ngIf="isActivityRow(row)"   │
                           │   │   → Render activity row      │
                           │   │                              │
                           │   │ *ngIf="isWIRRow(row)"        │
                           │   │   → Render WIR row (yellow)  │
                           │   └──────────────────────────────┘
                           │
                           └─► Step 5: Handle Actions
                               ┌──────────────────────────────┐
                               │ Activity Row:                │
                               │   [Edit] → Progress Modal    │
                               │                              │
                               │ WIR Row:                     │
                               │   [QA/QC] → Inspection Modal │
                               └──────────────────────────────┘
```

---

## 📊 Sequence Comparison

### ❌ BEFORE (43 Activities)
```
1  - Assembly & joints
2  - PODS installation
3  - MEP Cage
4  - Box Closure
5  - WIR-1 ← Activity (wrong)
6  - Ducts & Insulation
...
15 - WIR-2 ← Activity (wrong)
...
20 - WIR-3 ← Activity (wrong)
...
27 - WIR-4 ← Activity (wrong)
...
36 - WIR-5 ← Activity (wrong)
...
40 - WIR-6 ← Activity (wrong)
...
43 - Box Completion
```

### ✅ AFTER (28 Activities + Dynamic WIRs)
```
1  - Assembly & joints
2  - PODS installation
3  - MEP Cage
4  - Box Closure (triggers WIR-1) ✓
     └─► WIR-1 (dynamic record, yellow row)
5  - Ducts & Insulation
6  - Drainage piping
7  - Water Piping
8  - Fire Fighting Piping (triggers WIR-2) ✓
     └─► WIR-2 (dynamic record, yellow row)
9  - Electrical Containment
10 - Electrical Wiring
11 - Dry Wall Framing
12 - DB and ONU Panel (triggers WIR-3) ✓
     └─► WIR-3 (dynamic record, yellow row)
13 - False Ceiling
14 - Tile Fixing
15 - Painting
16 - Kitchenette
17 - Doors
18 - Windows (triggers WIR-4) ✓
     └─► WIR-4 (dynamic record, yellow row)
19 - Switches & Sockets
20 - Light Fittings
21 - Copper Piping
22 - Sanitary Fittings
23 - Thermostats
24 - Air Outlet
25 - Sprinkler
26 - Smoke Detector (triggers WIR-5) ✓
     └─► WIR-5 (dynamic record, yellow row)
27 - Iron Mongeries
28 - Inspection & Wrapping (triggers WIR-6) ✓
     └─► WIR-6 (dynamic record, yellow row)
```

---

## ✅ Key Takeaways

### 1. **WIR is NOT an Activity**
- WIR records are inspection checkpoints, not work activities
- They should never appear in the ActivityMaster table
- They are created dynamically in the WIRRecords table

### 2. **Checkpoint Activities Trigger WIRs**
- Activities 4, 8, 12, 18, 26, 28 have `IsWIRCheckpoint = true`
- When these activities reach 100%, backend auto-creates WIR record
- Frontend displays WIR row AFTER the checkpoint activity

### 3. **Visual Distinction is Critical**
- WIR rows MUST have yellow/orange gradient background
- WIR rows MUST have orange left border
- This matches the yellow highlighting in your Process Flow Table

### 4. **Correct Activity Count**
- Total: 28 activities (not 43)
- 6 are checkpoint activities
- WIRs are NOT counted in the activity total

---

**Created:** November 19, 2024  
**Purpose:** Visual reference for WIR display comparison  
**Status:** ✅ Complete

