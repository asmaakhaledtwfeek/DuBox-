# Updated Panel Workflow System

## 🎯 New Workflow Flow

### Stage 1: Pre-cast Location Workflow (REQUIRED FIRST)
Panels **MUST** complete workflow before any approval can happen.

#### Workflow Statuses:
1. **Not Started** → Initial state
2. **In Progress** → Panel being worked on (0-99% progress)
3. **Completed** → Panel finished (auto-sets 100% progress, enables First Approval)

### Stage 2: First Approval (Pre-cast Location QA)
Only available after workflow status = "Completed"

- ✅ **Approve** → Moves to Second Approval stage
- ❌ **Reject** → Creates quality issue, sends back to workflow with "Rejected" status

### Stage 3: Second Approval (Dubox Delivery)
Only available after First Approval is approved

- ✅ **Approve** → Panel fully approved, ready for installation
- ❌ **Reject** → Creates quality issue, sends back to workflow

## 🔄 Rejection & Issue Resolution Flow

### When Panel is Rejected:
1. **Quality issue created automatically** with:
   - Issue assigned to the person who rejected it
   - Status: Open
   - Severity: Major (First Approval) or Critical (Second Approval)

2. **Panel status changed to "Rejected"**
3. **Panel sent back to workflow stage**

### Resolving Issues:
- Issue **MUST** be resolved by:
  - The person who created the issue (rejector), OR
  - System administrator

- Once issue is resolved → Update workflow to "Completed" again
- Then re-approval can happen

### Re-approval Process:
1. Resolve the quality issue (status: Resolved/Closed)
2. Update panel workflow to "Completed"
3. Approval buttons become available again
4. Approve the panel

## 📋 Workflow Modal Options

### When opening "Workflow" button:

**1. In Progress**
- Use for ongoing work
- Update progress slider (0-99%)
- Add notes about current work

**2. Completed (Ready for Approval)**
- Use when panel work is done
- **Auto-sets progress to 100%**
- **Enables First Approval buttons**
- Panel now ready for QA review

**3. Rejected (Return to Workflow)**
- Use when issue found during workflow
- Sends panel back to rework
- Does NOT create quality issue (only approval rejection creates issues)

## 🚫 Approval Validation Rules

### First Approval Cannot Happen If:
- ❌ Workflow status is NOT "Completed"
- ❌ Panel has open quality issue
- ❌ Box is dispatched (read-only)

### Second Approval Cannot Happen If:
- ❌ First Approval is not "Approved"
- ❌ Panel has open quality issue
- ❌ Box is dispatched (read-only)

## 🎨 Status Colors

| Status | Color | Badge |
|--------|-------|-------|
| Not Started | Gray | ⚪ |
| In Progress | Blue | 🔵 |
| Completed | Green | 🟢 |
| Rejected | Red | 🔴 |
| First Approval Pending | Orange | 🟠 |
| First Approval Approved | Green | 🟢 |
| Second Approval Pending | Orange | 🟠 |
| Second Approval Approved | Green | ✅ |
| Second Approval Rejected | Red | ❌ |

## 💡 Example Workflow

### Happy Path:
1. Panel created → Status: **Not Started**
2. Worker opens "Workflow" → selects "In Progress"
3. Updates progress: 25% → 50% → 75%
4. Work done → selects "Completed" (progress → 100%)
5. QA Inspector clicks **"✓ 1st"** → First Approval: **Approved**
6. Delivery Inspector clicks **"✓ 2nd"** → Second Approval: **Approved**
7. Panel ready for installation! ✅

### Rejection Path:
1. Panel at workflow → Status: **Completed** (100%)
2. QA Inspector reviews → finds issue
3. Clicks **"✗ 1st"** with notes: "Crack in corner"
4. **Quality Issue #00001 created automatically**
5. Panel status → **Rejected**
6. Issue assigned to QA Inspector
7. Worker fixes issue → Updates in quality issue
8. QA Inspector marks issue as **Resolved**
9. Worker updates workflow → **Completed** again
10. QA Inspector clicks **"✓ 1st"** → **Approved**
11. Continue to Second Approval

## 🔧 Technical Implementation

### Backend Changes:
- ✅ `PanelStatusEnum` extended with workflow stages
- ✅ `ApprovePanelFirstApprovalCommandHandler` - validates workflow completion
- ✅ `ApprovePanelSecondApprovalCommandHandler` - validates previous approval
- ✅ `UpdatePanelWorkflowStatusCommandHandler` - simplified to 3 statuses
- ✅ Automatic quality issue creation on rejection
- ✅ Issue assignment to rejector

### Frontend Changes:
- ✅ Workflow modal with 3 status options
- ✅ Auto-complete at 100% progress
- ✅ Updated status badges and colors
- ✅ Clearer status labels

## 📝 Database Migration

Run this migration to update the database:

```bash
dotnet ef migrations add UpdatePanelWorkflowSystem --project Dubox.Infrastructure --startup-project Dubox.Api
dotnet ef database update --project Dubox.Infrastructure --startup-project Dubox.Api
```

## ✅ Benefits

1. **Enforced Workflow Order** - No skipping stages
2. **Clear Progress Tracking** - 0-100% visibility
3. **Automatic Issue Creation** - No manual tracking needed
4. **Issue Accountability** - Assigned to rejector
5. **Prevents Premature Approval** - Validation checks
6. **Complete Audit Trail** - All status changes logged
7. **User-Friendly** - Clear, intuitive workflow

## 🎯 Status: READY FOR TESTING

All code updated and ready for deployment!

