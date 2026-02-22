# Cascading Filter Implementation - Project Code → Box Tag

## Overview
Implemented cascading filter where selecting a Project Code automatically filters the Box Tag dropdown to show only box tags from that project.

## Changes Made

### 1. Filter Order Changed
**Before:** Stage Number → Box Tag → Project Code
**After:** Stage Number → **Project Code** → **Box Tag**

This makes more sense from a UX perspective - users first select a project, then see only relevant box tags.

---

### 2. Backend Changes

#### DTO Updated: `WIRCheckpointFiltersDto.cs`
Added new property to include box tags with their associated project codes:

```csharp
public class WIRCheckpointFiltersDto
{
    public List<string> StageNumbers { get; set; } = new();
    public List<string> BoxTags { get; set; } = new();
    public List<string> ProjectCodes { get; set; } = new();
    public List<BoxTagWithProject> BoxTagsWithProjects { get; set; } = new(); // NEW
}

public class BoxTagWithProject
{
    public string BoxTag { get; set; } = string.Empty;
    public string ProjectCode { get; set; } = string.Empty;
}
```

#### New Specification: `GetWIRCheckpointFiltersSpecification.cs`
Created dedicated specification for loading filter data:
- No pagination (loads all records)
- Includes Box and Project relationships
- Filters out inactive boxes/projects
- Filters out on-hold/closed/archived projects
- Respects user visibility permissions

#### Handler Updated: `GetWIRCheckpointFiltersQueryHandler.cs`
Added query to fetch box tags with their project codes:

```csharp
var boxTagsWithProjects = checkpointsQuery
    .Where(w => w.Box.Project != null && !string.IsNullOrEmpty(w.Box.Project.ProjectCode))
    .Select(w => new BoxTagWithProject
    {
        BoxTag = w.Box.BoxTag,
        ProjectCode = w.Box.Project.ProjectCode
    })
    .Distinct()
    .OrderBy(b => b.ProjectCode)
    .ThenBy(b => b.BoxTag)
    .ToList();
```

---

### 3. Frontend Changes

#### Service Updated: `wir.service.ts`
Enhanced response transformation to:
- Handle both camelCase and PascalCase property names
- Support the new `boxTagsWithProjects` property
- Add comprehensive logging for debugging

```typescript
const normalizedData = {
  stageNumbers: rawData?.stageNumbers || rawData?.StageNumbers || [],
  boxTags: rawData?.boxTags || rawData?.BoxTags || [],
  projectCodes: rawData?.projectCodes || rawData?.ProjectCodes || [],
  boxTagsWithProjects: rawData?.boxTagsWithProjects || rawData?.BoxTagsWithProjects || []
};
```

#### Component TypeScript: `quality-control-dashboard.component.ts`

**Added Properties:**
```typescript
// Store box tags with project codes for cascading filter
checkpointBoxTagsWithProjects: Array<{ boxTag: string; projectCode: string }> = [];
```

**Added Constructor Logic:**
```typescript
// Watch for project code changes to filter box tags
this.filterForm.get('projectCode')?.valueChanges.subscribe(projectCode => {
  // Reset box tag when project changes
  if (this.filterForm.get('boxTag')?.value) {
    this.filterForm.patchValue({ boxTag: '' }, { emitEvent: false });
  }
});
```

**Added Method:**
```typescript
/**
 * Get filtered box tags based on selected project code
 */
getFilteredCheckpointBoxTags(): string[] {
  const selectedProjectCode = this.filterForm.get('projectCode')?.value;
  
  // If no project selected, return all box tags
  if (!selectedProjectCode) {
    return this.checkpointFilterOptions.boxTags;
  }
  
  // If we have detailed box tag info with projects, filter by project
  if (this.checkpointBoxTagsWithProjects.length > 0) {
    return this.checkpointBoxTagsWithProjects
      .filter(item => item.projectCode === selectedProjectCode)
      .map(item => item.boxTag);
  }
  
  // Fallback: return all box tags if we don't have project mapping
  return this.checkpointFilterOptions.boxTags;
}
```

#### Component HTML: `quality-control-dashboard.component.html`

**Filter Order Changed:**
```html
<!-- Stage Number (unchanged) -->
<div class="field">
  <label>Stage Number</label>
  <select formControlName="wirNumber">...</select>
</div>

<!-- Project Code (moved before Box Tag) -->
<div class="field">
  <label>Project Code</label>
  <select formControlName="projectCode">
    <option value="">All</option>
    <option *ngFor="let code of checkpointFilterOptions.projectCodes" [value]="code">
      {{ code }}
    </option>
  </select>
</div>

<!-- Box Tag (now cascades based on Project Code) -->
<div class="field">
  <label>Box Tag</label>
  <select formControlName="boxTag">
    <option value="">All</option>
    <option *ngFor="let tag of getFilteredCheckpointBoxTags()" [value]="tag">
      {{ tag }}
    </option>
  </select>
</div>
```

---

## How It Works

### User Flow:

1. **User opens QA/QC Workspace**
   - API calls `/api/wircheckpoints/filters`
   - Backend returns all stage numbers, box tags, project codes, AND box tags with their project mappings

2. **User sees all filter options**
   - Stage Number: Shows all available stages
   - Project Code: Shows all available project codes
   - Box Tag: Shows all available box tags

3. **User selects a Project Code (e.g., "PROJ-001")**
   - Project Code dropdown changes to "PROJ-001"
   - Box Tag automatically resets to "All"
   - Box Tag dropdown now shows ONLY box tags from "PROJ-001"

4. **User selects a Box Tag from the filtered list**
   - Only box tags from the selected project are available
   - User applies filter

5. **User clears Project Code (selects "All")**
   - Box Tag dropdown shows all box tags again

### Data Flow:

```
Backend Query
    ↓
Get all WIRCheckpoints
    ↓
Extract:
  - Distinct Stage Numbers: ["WIR-1", "WIR-2", ...]
  - Distinct Box Tags: ["B-001", "B-002", "B-003", ...]
  - Distinct Project Codes: ["PROJ-001", "PROJ-002", ...]
  - Box Tags with Projects: [
      { boxTag: "B-001", projectCode: "PROJ-001" },
      { boxTag: "B-002", projectCode: "PROJ-001" },
      { boxTag: "B-003", projectCode: "PROJ-002" },
      ...
    ]
    ↓
Frontend receives data
    ↓
User selects "PROJ-001"
    ↓
getFilteredCheckpointBoxTags() filters:
  boxTagsWithProjects
    .filter(item => item.projectCode === "PROJ-001")
    .map(item => item.boxTag)
    ↓
Box Tag dropdown shows: ["B-001", "B-002"]
```

---

## Benefits

1. ✅ **Better UX**: Users see only relevant box tags for selected project
2. ✅ **Faster Selection**: Reduced dropdown options when project is selected
3. ✅ **Logical Flow**: Project → Box Tag makes more sense than Box Tag → Project
4. ✅ **Auto-Reset**: Box tag resets when project changes (prevents invalid combinations)
5. ✅ **Backwards Compatible**: If project not selected, all box tags are shown

---

## API Response Structure

### Example Response from `/api/wircheckpoints/filters`:

```json
{
  "isSuccess": true,
  "data": {
    "stageNumbers": ["WIR-1", "WIR-2", "WIR-3"],
    "boxTags": ["02156-B01-FF-L3-1", "170-B01-FF-S2-A-002", "161-B04-FF-S2-C-003"],
    "projectCodes": ["PROJ-001", "PROJ-002"],
    "boxTagsWithProjects": [
      { "boxTag": "02156-B01-FF-L3-1", "projectCode": "PROJ-001" },
      { "boxTag": "170-B01-FF-S2-A-002", "projectCode": "PROJ-001" },
      { "boxTag": "161-B04-FF-S2-C-003", "projectCode": "PROJ-002" }
    ]
  }
}
```

---

## Testing

### Scenario 1: No Project Selected
- **Project Code**: "All"
- **Box Tag Options**: Shows ALL box tags (e.g., 50 tags)

### Scenario 2: Project Selected
- **Project Code**: "PROJ-001"
- **Box Tag Options**: Shows ONLY box tags from PROJ-001 (e.g., 20 tags)
- **Previous Box Tag Selection**: Automatically cleared

### Scenario 3: Change Project
- **Initial**: Project = "PROJ-001", Box Tag = "B-001"
- **User changes Project to**: "PROJ-002"
- **Result**: Box Tag automatically resets to "All", shows tags from PROJ-002

---

## Files Modified

### Backend:
1. ✅ `Dubox.Application/DTOs/WIRCheckpointFiltersDto.cs` - Added BoxTagWithProject class
2. ✅ `Dubox.Application/Specifications/GetWIRCheckpointFiltersSpecification.cs` - New specification
3. ✅ `Dubox.Application/Features/WIRCheckpoints/Queries/GetWIRCheckpointFiltersQueryHandler.cs` - Added box tags with projects query

### Frontend:
1. ✅ `dubox-frontend/src/app/core/services/wir.service.ts` - Updated response type and normalization
2. ✅ `dubox-frontend/src/app/features/qc/quality-control-dashboard/quality-control-dashboard.component.ts` - Added cascading logic
3. ✅ `dubox-frontend/src/app/features/qc/quality-control-dashboard/quality-control-dashboard.component.html` - Reordered filters

---

## Next Steps

1. **Rebuild backend** to compile the new specification and handler
2. **Test the API** endpoint to verify it returns boxTagsWithProjects
3. **Rebuild frontend** to get the new changes
4. **Test in browser**:
   - Open QA/QC Workspace
   - Check console logs for filter data
   - Select a project code
   - Verify box tag dropdown updates to show only relevant tags

The cascading filter is now ready! 🎯
