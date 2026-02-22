# Quality Issues Cascading Filter Implementation

## ✅ Changes Applied

I've successfully applied the same cascading filter logic to the **Quality Issues** section. Now when a user selects a Project Code, the Box Tag dropdown will show only box tags from that project.

---

## 📋 Backend Changes

### 1. Updated `QualityIssueFiltersDto.cs`
Added new properties for cascading filter:

```csharp
public class QualityIssueFiltersDto
{
    public List<string> IssueNumbers { get; set; } = new();
    public List<string> BoxTags { get; set; } = new();
    public List<string> ProjectCodes { get; set; } = new();           // NEW
    public List<BoxTagWithProject> BoxTagsWithProjects { get; set; } = new();  // NEW
}
```

### 2. Updated `GetQualityIssueFiltersQueryHandler.cs`
Added logic to populate the new properties:

```csharp
// Get distinct project codes
var projectCodes = issuesQuery
    .Where(qi => qi.Box.Project != null && !string.IsNullOrEmpty(qi.Box.Project.ProjectCode))
    .Select(qi => qi.Box.Project.ProjectCode)
    .Distinct()
    .OrderBy(p => p)
    .ToList();

// Get box tags with their project codes for cascading filter
var boxTagsWithProjects = issuesQuery
    .Where(qi => qi.Box.Project != null && !string.IsNullOrEmpty(qi.Box.Project.ProjectCode))
    .Select(qi => new BoxTagWithProject
    {
        BoxTag = qi.Box.BoxTag,
        ProjectCode = qi.Box.Project.ProjectCode
    })
    .Distinct()
    .OrderBy(b => b.ProjectCode)
    .ThenBy(b => b.BoxTag)
    .ToList();
```

---

## 🎨 Frontend Changes

### 1. Updated `wir.service.ts`
Enhanced `getQualityIssueFilters()` to handle new properties:

```typescript
getQualityIssueFilters(): Observable<{
  isSuccess: boolean;
  data: {
    issueNumbers: string[];
    boxTags: string[];
    projectCodes: string[];                                          // NEW
    boxTagsWithProjects?: Array<{ boxTag: string; projectCode: string }>  // NEW
  }
}> {
  return this.apiService.get<any>('qualityissues/filters').pipe(
    map(response => {
      // Normalize boxTagsWithProjects array and each item's properties
      const boxTagsWithProjectsRaw = rawData?.boxTagsWithProjects || rawData?.BoxTagsWithProjects || [];
      
      const normalizedBoxTagsWithProjects = Array.isArray(boxTagsWithProjectsRaw) 
        ? boxTagsWithProjectsRaw.map((item: any) => ({
            boxTag: item?.boxTag || item?.BoxTag || '',
            projectCode: item?.projectCode || item?.ProjectCode || ''
          }))
        : [];
      
      return {
        isSuccess: true,
        data: {
          issueNumbers: rawData?.issueNumbers || rawData?.IssueNumbers || [],
          boxTags: rawData?.boxTags || rawData?.BoxTags || [],
          projectCodes: rawData?.projectCodes || rawData?.ProjectCodes || [],
          boxTagsWithProjects: normalizedBoxTagsWithProjects
        }
      };
    })
  );
}
```

### 2. Updated `quality-control-dashboard.component.ts`

#### a. Added new properties:
```typescript
qualityIssueFilterOptions: {
  issueNumbers: string[];
  boxTags: string[];
  projectCodes: string[];  // NEW
} = { issueNumbers: [], boxTags: [], projectCodes: [] };

qualityIssueBoxTagsWithProjects: Array<{ boxTag: string; projectCode: string }> = [];  // NEW
```

#### b. Added project code change subscription:
```typescript
// Watch for project code changes to filter box tags
this.qualityIssuesFilterForm.get('projectCode')?.valueChanges.subscribe(projectCode => {
  console.log('🔄 Quality Issue Project Code changed to:', projectCode);
  // Reset box tag when project changes
  const currentBoxTag = this.qualityIssuesFilterForm.get('boxTag')?.value;
  if (currentBoxTag) {
    console.log('🔄 Resetting Quality Issue Box Tag from:', currentBoxTag);
    this.qualityIssuesFilterForm.patchValue({ boxTag: '' }, { emitEvent: false });
  }
});
```

#### c. Added cascading filter method:
```typescript
/**
 * Get filtered box tags based on selected project code (for Quality Issues)
 */
getFilteredQualityIssueBoxTags(): string[] {
  const selectedProjectCode = this.qualityIssuesFilterForm.get('projectCode')?.value;
  
  console.log('🔍 getFilteredQualityIssueBoxTags called');
  console.log('   - Selected Project Code:', selectedProjectCode);
  console.log('   - Total Box Tags:', this.qualityIssueFilterOptions.boxTags?.length || 0);
  console.log('   - Box Tags with Projects:', this.qualityIssueBoxTagsWithProjects.length);
  
  // If no project selected, return all box tags
  if (!selectedProjectCode || selectedProjectCode.trim() === '') {
    console.log('   → Returning ALL box tags (no project filter)');
    return this.qualityIssueFilterOptions.boxTags || [];
  }
  
  // If we have detailed box tag info with projects, filter by project
  if (this.qualityIssueBoxTagsWithProjects.length > 0) {
    const filteredTags = this.qualityIssueBoxTagsWithProjects
      .filter(item => item.projectCode === selectedProjectCode)
      .map(item => item.boxTag);
    
    console.log('   → Filtered to', filteredTags.length, 'box tags for project:', selectedProjectCode);
    console.log('   → Filtered tags:', filteredTags);
    return filteredTags;
  }
  
  // Fallback: return all box tags if we don't have project mapping
  console.warn('   ⚠️ No project mapping available, returning all box tags');
  return this.qualityIssueFilterOptions.boxTags || [];
}
```

#### d. Updated `loadQualityIssueFilterOptions()`:
```typescript
private loadQualityIssueFilterOptions(): void {
  this.wirService.getQualityIssueFilters().subscribe({
    next: (response) => {
      if (response.isSuccess && response.data) {
        this.qualityIssueFilterOptions = response.data;
        
        // Store box tags with project codes for cascading filter
        if (response.data.boxTagsWithProjects && response.data.boxTagsWithProjects.length > 0) {
          this.qualityIssueBoxTagsWithProjects = response.data.boxTagsWithProjects;
          console.log('✅ Stored quality issue box tags with projects for cascading filter');
        }
        
        console.log('✅ Loaded quality issue filter options:');
        console.log('   - Issue Numbers:', this.qualityIssueFilterOptions.issueNumbers?.length || 0, 'items');
        console.log('   - Box Tags:', this.qualityIssueFilterOptions.boxTags?.length || 0, 'items');
        console.log('   - Project Codes:', this.qualityIssueFilterOptions.projectCodes?.length || 0, 'items');
        console.log('   - Box Tags with Projects:', this.qualityIssueBoxTagsWithProjects.length, 'items');
      }
    }
  });
}
```

### 3. Updated `quality-control-dashboard.component.html`

Changed the Quality Issues filters:

```html
<!-- OLD: Used uniqueProjects from actual data -->
<div class="field">
  <label>Project </label>
  <select formControlName="projectCode">
    <option [ngValue]="''">All</option>
    <option *ngFor="let project of uniqueProjects" [ngValue]="project.id">
      {{project.name}} - {{ project.code }}
    </option>
  </select>
</div>

<!-- NEW: Uses projectCodes from filters API -->
<div class="field">
  <label>Project Code</label>
  <select formControlName="projectCode">
    <option value="">All</option>
    <option *ngFor="let code of qualityIssueFilterOptions.projectCodes" [value]="code">{{ code }}</option>
  </select>
</div>

<!-- OLD: Shows all box tags -->
<div class="field">
  <label>Box Tag</label>
  <select formControlName="boxTag">
    <option value="">All</option>
    <option *ngFor="let tag of qualityIssueFilterOptions.boxTags" [value]="tag">{{ tag }}</option>
  </select>
</div>

<!-- NEW: Shows only box tags from selected project -->
<div class="field">
  <label>Box Tag</label>
  <select formControlName="boxTag">
    <option value="">All</option>
    <option *ngFor="let tag of getFilteredQualityIssueBoxTags()" [value]="tag">{{ tag }}</option>
  </select>
</div>
```

---

## 🎯 How It Works

### User Experience Flow:

1. **Initial Load**
   - All dropdowns populated
   - Project Code shows all available project codes
   - Box Tag shows ALL box tags

2. **User Selects Project Code**
   - Console logs: `🔄 Quality Issue Project Code changed to: PROJ-001`
   - If a box tag was selected, it automatically resets to "All"
   - Box Tag dropdown updates to show only tags from PROJ-001
   - Data refreshes automatically (auto-apply)

3. **User Changes Project Code**
   - Previous box tag selection is cleared
   - Box Tag dropdown updates with new project's tags
   - Data refreshes automatically

4. **User Clears Project (Select "All")**
   - Box Tag dropdown shows ALL tags again

---

## 🧪 Testing Checklist

### Backend Testing
- [ ] Build backend successfully: `dotnet build --no-restore`
- [ ] Start/restart backend API
- [ ] Test endpoint: `GET /api/qualityissues/filters`
- [ ] Verify response includes:
  - `issueNumbers` array
  - `boxTags` array
  - `projectCodes` array
  - `boxTagsWithProjects` array with `{ boxTag, projectCode }` objects

### Frontend Testing
- [ ] Rebuild frontend: `ng serve`
- [ ] Open browser console (F12)
- [ ] Navigate to QA/QC Workspace → Quality Issues tab

**Test 1: Initial Load**
- [ ] All filters show options
- [ ] Console shows: `boxTagsWithProjects count: [number > 0]`

**Test 2: Project Selection**
- [ ] Select a Project Code
- [ ] Box Tag dropdown updates immediately
- [ ] Only shows tags from that project
- [ ] Console shows: `→ Filtered to X box tags for project: [code]`
- [ ] Data auto-refreshes (no Apply button needed)

**Test 3: Project Change**
- [ ] Select Project Code: "PROJ-001"
- [ ] Select Box Tag: "B-001"
- [ ] Change Project Code to: "PROJ-002"
- [ ] Box Tag resets to "All"
- [ ] Box Tag dropdown shows tags from PROJ-002

**Test 4: Clear Project**
- [ ] Select a Project Code
- [ ] Change to "All"
- [ ] Box Tag shows all tags again

---

## 📊 Console Logs to Verify

When you load the page, you should see:

```
🔄 Loading quality issue filter options...
📥 Quality Issue Filters Response: {...}
✅ Normalized filter data:
   - issueNumbers: [...]
   - boxTags: [...]
   - projectCodes: [...]
   - boxTagsWithProjects: [...]
   - boxTagsWithProjects count: 25
✅ Loaded quality issue filter options:
   - Issue Numbers: 15 items
   - Box Tags: 25 items
   - Project Codes: 5 items
   - Box Tags with Projects: 25 items
```

When you select a project:

```
🔄 Quality Issue Project Code changed to: PROJ-001
🔍 getFilteredQualityIssueBoxTags called
   - Selected Project Code: PROJ-001
   - Total Box Tags: 25
   - Box Tags with Projects: 25
   → Filtered to 8 box tags for project: PROJ-001
   → Filtered tags: ["B-001", "B-002", ...]
🔄 Filter changed, auto-applying filters...
```

---

## 🔧 Troubleshooting

### Problem: Box Tags Still Showing All Tags

**Check Console:**
```
- Box Tags with Projects: 0 items  ← Should NOT be 0
```

**Solutions:**
1. Verify backend is restarted after changes
2. Check Network tab → `qualityissues/filters` response
3. Ensure `boxTagsWithProjects` array exists and has data
4. Clear browser cache (Ctrl + Shift + Delete)

### Problem: Property Name Mismatch

The service already handles both camelCase and PascalCase:
- `boxTagsWithProjects` OR `BoxTagsWithProjects`
- `item.boxTag` OR `item.BoxTag`
- `item.projectCode` OR `item.ProjectCode`

If still not working, check the actual response format in Network tab.

---

## 📄 Files Modified

### Backend:
1. `Dubox.Application\DTOs\QualityIssueFiltersDto.cs`
2. `Dubox.Application\Features\QualityIssues\Queries\GetQualityIssueFiltersQueryHandler.cs`

### Frontend:
1. `dubox-frontend\src\app\core\services\wir.service.ts`
2. `dubox-frontend\src\app\features\qc\quality-control-dashboard\quality-control-dashboard.component.ts`
3. `dubox-frontend\src\app\features\qc\quality-control-dashboard\quality-control-dashboard.component.html`

---

## ✅ Summary

Both **Stage (Checkpoints)** and **Quality Issues** sections now have:

| Feature | Checkpoints | Quality Issues |
|---------|-------------|----------------|
| Auto-apply filters | ✅ | ✅ |
| Cascading Project → Box Tag | ✅ | ✅ |
| Auto-reset Box Tag on Project change | ✅ | ✅ |
| Comprehensive logging | ✅ | ✅ |
| PascalCase/camelCase handling | ✅ | ✅ |

Ready for testing! 🚀
