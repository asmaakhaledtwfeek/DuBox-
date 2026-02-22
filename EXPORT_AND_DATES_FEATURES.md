# Export to Excel & Enhanced Dates Display Features

## Overview
Two new features have been added to the Schedule Dashboard:
1. **Export to Excel** - Export schedule activities in the same format as import template
2. **"Ongoing" Indicator** - Visual indicator for activities that have started but not finished

---

## Feature 1: Export to Excel

### Description
Export all schedule activities for a project to an Excel file with the same hierarchical structure as the import template (KJ-158 format).

### User Interface

**Button Location:** Next to "Import from Excel" button in the header

```
┌─────────────────────────────────────────────┐
│  [Import from Excel]  [Export to Excel]    │
└─────────────────────────────────────────────┘
```

**Button State:**
- **Enabled:** When project is selected and has activities
- **Disabled:** When no project selected, loading, or no activities

### Excel File Structure

The exported file matches the import template structure:

| Column | Description | Format |
|--------|-------------|--------|
| **Activity Code** | Activity identifier with indentation | Text (indented) |
| **Activity Name** | Full activity description | Text |
| **BL1 Start** | Planned start date | dd-mmm-yy |
| **BL1 Finish** | Planned finish date | dd-mmm-yy |
| **Original Duration** | Duration in days | Number |
| **Stage** | Activity stage | Text |
| **Status** | Current status | Text |
| **Progress %** | Completion percentage | Number (0-100%) |
| **Actual Start** | Actual start date | dd-mmm-yy |
| **Actual Finish** | Actual finish date | dd-mmm-yy |

### Hierarchical Structure

**Indentation Preserved:**
```
Activity Code                Activity Name
────────────────────────────────────────────
RSG                          Root Activity
  SUB-01                     Child Activity (2 spaces)
    SUB-01-A                 Grandchild Activity (4 spaces)
    SUB-01-B                 Grandchild Activity (4 spaces)
  SUB-02                     Child Activity (2 spaces)
```

**Visual Hierarchy:**
- **Root activities** - Bold, blue background
- **Child activities** - Indented (2 spaces per level)
- **Excel outline levels** - Set for expand/collapse in Excel

### File Naming Convention

```
{ProjectCode}_Schedule_{Timestamp}.xlsx

Examples:
- 0148_Schedule_2026-02-12T14-30-00.xlsx
- Dubuil123_Schedule_2026-02-12T09-15-30.xlsx
```

### Technical Implementation

#### Frontend

**File:** `schedule-dashboard.component.ts`

```typescript
exportToExcel(): void {
  this.http.get(
    `${environment.apiUrl}/schedule/activities/export/${projectId}`,
    { responseType: 'blob' }
  ).subscribe({
    next: (blob) => {
      // Create download link
      const url = window.URL.createObjectURL(blob);
      const link = document.createElement('a');
      link.href = url;
      link.download = `${projectCode}_Schedule_${timestamp}.xlsx`;
      link.click();
      window.URL.revokeObjectURL(url);
    }
  });
}
```

#### Backend

**Files Created:**
1. `ExportScheduleActivitiesToExcelCommand.cs`
2. `ExportScheduleActivitiesToExcelCommandHandler.cs`

**API Endpoint:**
```
GET /api/schedule/activities/export/{projectId}
```

**Features:**
- ✅ Preserves hierarchical structure with indentation
- ✅ Includes all activity data (planned, actual, progress)
- ✅ Formatted dates (dd-mmm-yy)
- ✅ Auto-fit columns
- ✅ Styled headers (teal background, white text)
- ✅ Bold root activities with background color
- ✅ Excel outline levels for expand/collapse
- ✅ Auto-filter enabled

---

## Feature 2: "Ongoing" Indicator for Actual Dates

### Description
When an activity has an actual start date but no actual finish date (meaning it's currently in progress), display a special "Ongoing" indicator.

### Visual Display

**Scenario 1: Not Started**
```
Actual Dates: -
```

**Scenario 2: In Progress (NEW!)**
```
Actual Dates: 12-Feb-26 → 🕐 Ongoing
              ↑ Green      ↑ Yellow badge
```

**Scenario 3: Completed**
```
Actual Dates: 12-Feb-26 → 24-Sep-25
              ↑ Green      ↑ Green
```

### "Ongoing" Badge Styling

**Colors:**
- **Background:** Yellow/Amber (#fef3c7)
- **Border:** Golden (#fbbf24)
- **Text:** Brown (#92400e)
- **Icon:** Orange (#f59e0b)

**Animations:**
- **Pulse effect:** Subtle opacity pulse (2s cycle)
- **Rotating clock:** Animated clock icon (3s rotation)

**Visual Example:**
```css
┌────────────────┐
│ 🕐 Ongoing     │ ← Yellow background
└────────────────┘  ← Pulsing animation
     ↑ Rotating clock icon
```

### Benefits

✅ **Immediate visual feedback** - Yellow stands out from green dates  
✅ **Clear status indication** - "Ongoing" text is self-explanatory  
✅ **Animated attention** - Rotating clock draws eye to in-progress items  
✅ **Professional appearance** - Matches modern UI/UX standards  
✅ **Consistent with status badges** - Similar style to "In Progress" status

### Technical Implementation

**File:** `activity-tree-node.component.ts`

```html
<!-- Start exists but finish doesn't - show "Ongoing" -->
<span class="date-item ongoing" *ngIf="actualStartDate && !actualFinishDate">
  <svg><!-- Clock icon --></svg>
  Ongoing
</span>
```

```css
.date-item.ongoing {
  background: #fef3c7;
  color: #92400e;
  border: 1px solid #fbbf24;
  animation: pulse-ongoing 2s ease-in-out infinite;
}

.date-item.ongoing svg {
  color: #f59e0b;
  animation: rotate-clock 3s linear infinite;
}
```

---

## Usage Guide

### Exporting Schedule

**Steps:**
1. Select a project from the dropdown
2. Ensure activities are loaded
3. Click **"Export to Excel"** button in header
4. Excel file downloads automatically
5. Open file in Excel/LibreOffice

**File Contents:**
- All activities in hierarchical order
- Indentation shows parent-child relationships
- All dates, progress, and status information
- Ready to be modified and re-imported

### Viewing Ongoing Activities

**Automatic Display:**
- When an activity has actual start date set
- But no actual finish date
- The "Ongoing" badge appears automatically
- No user action required

**Visual Cues:**
- Look for yellow badges in Actual Dates column
- Rotating clock icon indicates work in progress
- Arrow (→) always shown when activity has started

---

## Complete Workflow Example

### Project Lifecycle:

**1. Import baseline schedule:**
```
Action: Click "Import from Excel"
Result: 10,000+ activities loaded with hierarchy
```

**2. Start working on activities:**
```
Action: Click edit button → Set actual start date
Result: "12-Feb-26 → 🕐 Ongoing" appears
```

**3. Monitor progress:**
```
Visual: Yellow "Ongoing" badges show active work
Easy to scan: Rotating clocks draw attention
```

**4. Complete activities:**
```
Action: Click edit button → Set actual finish date
Result: "12-Feb-26 → 24-Sep-25" (both green)
```

**5. Export updated schedule:**
```
Action: Click "Export to Excel"
Result: Excel file with all current data downloads
Use case: Share with stakeholders, backup, reporting
```

**6. Re-import if needed:**
```
Action: Modify exported Excel → Import again
Result: Updates applied with preserved hierarchy
```

---

## Files Modified/Created

### Frontend
1. ✅ `schedule-dashboard.component.html` - Added export button
2. ✅ `schedule-dashboard.component.ts` - Added exportToExcel() method
3. ✅ `activity-tree-node.component.ts` - Added "Ongoing" indicator & animations

### Backend
4. ✅ `ExportScheduleActivitiesToExcelCommand.cs` (NEW)
5. ✅ `ExportScheduleActivitiesToExcelCommandHandler.cs` (NEW)
6. ✅ `ScheduleController.cs` - Added export endpoint

---

## API Endpoint

### Export Schedule Activities

**Method:** `GET`  
**URL:** `/api/schedule/activities/export/{projectId}`  
**Auth:** Required (Bearer token)

**Response:**
- **Content-Type:** `application/vnd.openxmlformats-officedocument.spreadsheetml.sheet`
- **Content-Disposition:** `attachment; filename="ProjectCode_Schedule_Timestamp.xlsx"`

**Example:**
```http
GET /api/schedule/activities/export/abc123-def456-789...
Authorization: Bearer {token}

Response: Binary Excel file
```

---

## Excel File Features

### Styling
✅ **Header row** - Teal background (#1b9aaa), white text, bold  
✅ **Root activities** - Bold text, light blue background  
✅ **Child activities** - Normal text, indented  
✅ **Auto-filter** - Enabled on all columns  
✅ **Borders** - Thin borders around all cells  
✅ **Date format** - dd-mmm-yy (e.g., 12-Feb-26)  
✅ **Progress format** - Percentage (e.g., 75%)

### Excel Features
✅ **Outline levels** - Expand/collapse hierarchy in Excel  
✅ **Auto-fit columns** - Optimal column widths  
✅ **Minimum widths** - Prevents columns from being too narrow  
✅ **Frozen header** - Can be added if needed

---

## Testing Checklist

### Export Testing
- [ ] Select project with activities
- [ ] Click "Export to Excel"
- [ ] Verify file downloads
- [ ] Open file in Excel
- [ ] Verify hierarchical structure (indentation)
- [ ] Verify all data is present and correct
- [ ] Verify date formatting (dd-mmm-yy)
- [ ] Verify progress percentages
- [ ] Try expand/collapse in Excel (outline levels)
- [ ] Verify root activities are bold

### "Ongoing" Indicator Testing
- [ ] Activity with no actual dates shows "-"
- [ ] Set actual start date → "Ongoing" badge appears
- [ ] Verify yellow background and rotating clock
- [ ] Set actual finish date → "Ongoing" disappears, both dates shown
- [ ] Verify animation is smooth
- [ ] Test on multiple activities

### Round-Trip Testing
- [ ] Export schedule
- [ ] Modify Excel file (update progress, dates)
- [ ] Re-import modified file
- [ ] Verify changes applied correctly
- [ ] Verify hierarchy preserved

---

## Future Enhancements

### Export Options
1. **Filtered Export** - Export only selected/visible activities
2. **Custom Columns** - Choose which columns to include
3. **Multiple Formats** - PDF, CSV export options
4. **Templates** - Different export templates for different purposes
5. **Scheduled Exports** - Auto-export on schedule

### "Ongoing" Enhancements
1. **Duration Display** - Show "Ongoing for X days"
2. **Color Coding** - Different colors for delayed vs on-time
3. **Tooltip** - Show more details on hover
4. **Click Action** - Quick complete when clicking "Ongoing"

---

**Features Completed:** February 12, 2026  
**Status:** ✅ Ready for Testing
