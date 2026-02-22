# Testing Guide - Cascading Filters & Auto-Apply

## Changes Completed

### ✅ Backend
1. Added `BoxTagsWithProjects` to the filter response
2. Created `GetWIRCheckpointFiltersSpecification` for efficient filter data loading
3. Enhanced handler to return box tag → project code mappings

### ✅ Frontend
1. Filters now **auto-apply** without clicking "Apply Filters" button
2. **Project Code** filter moved before Box Tag filter
3. **Cascading filter**: Selecting a project filters box tags
4. **Auto-reset**: Box tag clears when project changes
5. Comprehensive logging for debugging

---

## Testing Steps

### Step 1: Build Backend
```bash
cd "C:\Users\asmaa.hassan\source\repos\Digital Engineering"
dotnet build --no-restore
```

### Step 2: Start/Restart Backend
Make sure your backend API is running with the latest changes.

### Step 3: Rebuild Frontend
```bash
cd dubox-frontend
npm install  # if needed
ng serve     # or npm start
```

### Step 4: Open Browser Console
1. Open Chrome/Edge
2. Press **F12** to open Developer Tools
3. Go to **Console** tab
4. Clear console (Ctrl+L or click clear button)

### Step 5: Navigate to QA/QC Workspace
Go to the QA/QC Workspace page.

---

## What to Check in Console

### On Page Load:
You should see these logs in order:

```
🔄 Loading WIR checkpoint filter options...
🔍 Raw WIR Checkpoint Filters Response: {object}
🔍 Response type: object
🔍 Response keys: ["data", "isSuccess", ...] or ["stageNumbers", "boxTags", ...]
✅ Response format: [detected format]
🔍 Extracted rawData: {object}
🔍 RawData keys: ["stageNumbers", "boxTags", "projectCodes", "boxTagsWithProjects"]
✅ Normalized filter data:
   - stageNumbers: (5) ["WIR-1", "WIR-2", ...]
   - boxTags: (20) ["B-001", "B-002", ...]
   - projectCodes: (3) ["PROJ-001", "PROJ-002", ...]
   - boxTagsWithProjects: (20) [{boxTag: "B-001", projectCode: "PROJ-001"}, ...]
   - boxTagsWithProjects count: 20
📥 WIR Checkpoint Filters Response: {object}
✅ Loaded checkpoint filter options:
   - Stage Numbers: 5 items
   - Box Tags: 20 items
   - Project Codes: 3 items
   - Box Tags with Projects: 20 items
```

### When You Select a Project Code:
```
🔄 Project Code changed to: PROJ-001
🔍 getFilteredCheckpointBoxTags called
   - Selected Project Code: PROJ-001
   - Total Box Tags: 20
   - Box Tags with Projects: 20
   → Filtered to 8 box tags for project: PROJ-001
   → Filtered tags: ["B-001", "B-002", ...]
🔄 Filter changed, auto-applying filters...
```

---

## Troubleshooting

### Problem 1: All Box Tags Still Showing

**Check Console for:**
```
- Box Tags with Projects: 0 items  ← This should NOT be 0
```

**If it's 0:**
1. Check the backend response in Network tab
2. Look for `wircheckpoints/filters` request
3. Check if `boxTagsWithProjects` array exists in response
4. Verify it's not empty

**Possible Causes:**
- Backend didn't compile with new changes
- Backend not restarted after changes
- Specification not including Project relationship

**Solution:**
```bash
# Rebuild backend
dotnet clean
dotnet build
# Restart backend
```

---

### Problem 2: Property Name Mismatch

**Check Console for:**
```
🔍 RawData keys: ["StageNumbers", "BoxTags", ...]  ← PascalCase
```

**If properties are PascalCase:**
The code already handles this! Check if the normalization logs show:
```
✅ Normalized filter data:
   - boxTagsWithProjects: (20) [{...}]  ← Should have data
```

**If still empty after normalization:**
The property might have a different name. Check the actual response in Network tab.

---

### Problem 3: Box Tags Not Filtering

**Check Console When Selecting Project:**
```
🔍 getFilteredCheckpointBoxTags called
   - Selected Project Code: [empty or undefined]  ← Should have value
```

**If Project Code is empty:**
- Form control might not be binding correctly
- Check that formControlName="projectCode" exists in HTML

**If Box Tags with Projects is 0:**
- Backend didn't return the mapping data
- Check backend logs and database

---

### Problem 4: Filters Not Auto-Applying

**Symptoms:**
- Selecting a filter value doesn't trigger search
- Need to click Apply button (which is now hidden)

**Check Console:**
Should see this when you change any filter:
```
🔄 Filter changed, auto-applying filters...
```

**If you don't see this:**
- The valueChanges subscription might not be working
- Check for JavaScript errors in console

---

## Network Tab Debugging

### Step 1: Open Network Tab
1. F12 → Network tab
2. Filter by "filters" in search box

### Step 2: Find the Request
Look for: `wircheckpoints/filters`

### Step 3: Check Response
Click on the request → **Response** tab

**Expected Response:**
```json
{
  "isSuccess": true,
  "data": {
    "stageNumbers": ["WIR-1", "WIR-2"],
    "boxTags": ["B-001", "B-002", "B-003"],
    "projectCodes": ["PROJ-001", "PROJ-002"],
    "boxTagsWithProjects": [
      { "boxTag": "B-001", "projectCode": "PROJ-001" },
      { "boxTag": "B-002", "projectCode": "PROJ-001" },
      { "boxTag": "B-003", "projectCode": "PROJ-002" }
    ]
  }
}
```

**Or (PascalCase):**
```json
{
  "IsSuccess": true,
  "Data": {
    "StageNumbers": ["WIR-1", "WIR-2"],
    "BoxTags": ["B-001", "B-002", "B-003"],
    "ProjectCodes": ["PROJ-001", "PROJ-002"],
    "BoxTagsWithProjects": [
      { "BoxTag": "B-001", "ProjectCode": "PROJ-001" },
      ...
    ]
  }
}
```

### Step 4: Verify boxTagsWithProjects
- Should be an array
- Should have objects with `boxTag` and `projectCode` properties
- Should have the same number of items as `boxTags` array

---

## Manual Testing Checklist

### Test 1: Auto-Apply Filters
- [ ] Open QA/QC Workspace
- [ ] Change any filter (Stage Number, Project Code, etc.)
- [ ] Verify table updates immediately WITHOUT clicking "Apply Filters"
- [ ] Verify no "Apply Filters" button is visible

### Test 2: Cascading Filter - Initial Load
- [ ] All dropdowns should be populated
- [ ] Project Code dropdown has values
- [ ] Box Tag dropdown shows ALL box tags

### Test 3: Cascading Filter - Select Project
- [ ] Select a Project Code (e.g., "PROJ-001")
- [ ] Box Tag dropdown should update immediately
- [ ] Box Tag should show only tags from PROJ-001
- [ ] If you had a box tag selected, it should reset to "All"

### Test 4: Cascading Filter - Change Project
- [ ] Select Project Code: "PROJ-001"
- [ ] Select Box Tag: "B-001" (from PROJ-001)
- [ ] Change Project Code to: "PROJ-002"
- [ ] Box Tag should reset to "All"
- [ ] Box Tag dropdown should show only tags from PROJ-002

### Test 5: Cascading Filter - Clear Project
- [ ] Select Project Code: "PROJ-001"
- [ ] Verify Box Tag shows filtered tags
- [ ] Change Project Code to: "All"
- [ ] Box Tag dropdown should show ALL tags again

### Test 6: Reset Button
- [ ] Apply various filters
- [ ] Click "Reset" button
- [ ] All filters should reset to "All"
- [ ] Data should refresh

---

## Quick Debug Commands

### Check if backend is running:
```bash
curl http://localhost:5000/api/wircheckpoints/filters
```

### Check frontend dev server:
```bash
cd dubox-frontend
ng serve
```

### Clear browser cache:
- Press **Ctrl + Shift + Delete**
- Select "Cached images and files"
- Click "Clear data"
- Refresh page (**Ctrl + F5**)

---

## Expected Behavior Summary

| Action | Expected Result |
|--------|----------------|
| Page loads | All filter dropdowns populated |
| Select Project Code | Box Tag dropdown filters to that project |
| Change filter value | Data auto-refreshes (no Apply button needed) |
| Select Stage Number | Data filters immediately |
| Select Box Tag | Data filters immediately |
| Click Reset | All filters clear, data refreshes |
| Change Project | Box Tag resets and shows new project's tags |

---

## If Issues Persist

**Copy and paste these from the console:**

1. The "Raw WIR Checkpoint Filters Response" log
2. The "RawData keys" log
3. The "Normalized filter data" log
4. Any error messages

This will help identify the exact issue!
