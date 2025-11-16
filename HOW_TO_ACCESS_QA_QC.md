# How to Access QA/QC Checklist

## ✅ Fixed Issue
The QA/QC Checklist button in the box details header was navigating to an incorrect route. This has been **removed** because QA/QC checklists are **activity-specific** (tied to WIR inspection activities).

## 📍 How to Access QA/QC Checklist

### Step 1: Navigate to Box Details
```
Projects → Click on a Project → View Boxes → Click on a Box
```

### Step 2: Go to Activities Tab
In the Box Details page, click on the **"Activities"** tab

### Step 3: Find WIR Activities
WIR activities are **highlighted in yellow** in the Process Flow Table. They include:
- **WIR-1**: Assembly Clearance
- **WIR-2**: Mechanical Clearance
- **WIR-3**: Ceiling Closure
- **WIR-4**: 3rd Fix Installation
- **WIR-5**: 3rd Fix MEP Installation
- **WIR-6**: Readiness for Dispatch

### Step 4: Click QA/QC Button
Each WIR activity has a **green "QA/QC" button**. Click it to access the inspection checklist for that specific activity.

## 🎯 Why This Design?

Each QA/QC checklist is specific to a manufacturing stage (WIR activity). Different activities have different checkpoints:
- WIR-1 has 5 checkpoints for assembly clearance
- WIR-5 has 8 checkpoints for MEP installation
- etc.

This ensures inspectors review the correct checklist for each stage.

## 🖼️ Visual Flow

```
Box Details Page
    ↓
Activities Tab
    ↓
Find WIR Activity (Yellow highlighted row)
    ↓
Click Green "QA/QC" Button
    ↓
QA/QC Checklist for that Activity
```

## ⚠️ Important Notes

1. **Only WIR activities** have QA/QC buttons (highlighted in yellow)
2. **Regular activities** don't have QA/QC checklists
3. Each WIR activity has its own specific checklist with different checkpoints
4. You must have **QCInspector**, **SiteEngineer**, **Foreman**, **ProjectManager**, or **SystemAdmin** role to access QA/QC checklists

## 🔐 Required Roles

To access and complete QA/QC checklists:
- ✅ SystemAdmin
- ✅ ProjectManager
- ✅ QCInspector (Primary role for QA/QC)
- ✅ SiteEngineer
- ✅ Foreman

## 📝 Example Workflow

1. Foreman completes "Box Closure" activity (which triggers WIR-1)
2. QC Inspector receives notification
3. QC Inspector navigates to Box Details → Activities Tab
4. QC Inspector finds WIR-1 (Assembly Clearance) - highlighted in yellow
5. QC Inspector clicks green "QA/QC" button
6. QC Inspector completes the 5-checkpoint inspection
7. QC Inspector signs and approves/rejects
8. System updates box status and notifies team

## 🆘 Troubleshooting

**Q: I don't see any QA/QC buttons**
- A: Make sure you're looking at the Activities tab in Box Details
- Check if there are WIR activities (they should be highlighted in yellow)
- Verify your user role has QA/QC permissions

**Q: The QA/QC button is disabled or missing**
- A: Only WIR activities (WIR-1 through WIR-6) have QA/QC buttons
- Regular activities don't require QA/QC inspection

**Q: I get an error when clicking QA/QC**
- A: Ensure the backend API is running on `https://localhost:7098`
- Check that the activity has a valid ID
- Verify your authentication token is valid

## ✅ Changes Made

### Removed from Box Details Header:
- ❌ Generic "QA/QC Checklist" button (was causing navigation errors)

### Added to Activities Table:
- ✅ Activity-specific QA/QC buttons for WIR activities
- ✅ Proper routing: `/projects/{projectId}/boxes/{boxId}/activities/{activityId}/qa-qc`
- ✅ Visual indicators (yellow highlight for WIR activities)
- ✅ Green "QA/QC" button with icon

---

**Updated**: November 16, 2025  
**Status**: ✅ Fixed - QA/QC now accessible via Activities Tab

