# Parent-Child Sync Logic

## Overview

Implements bidirectional sync with **data integrity guarantee**: parent progress is ALWAYS the true weighted average of its children, even after manual updates.

## Core Principle

**Parent progress is a computed property** that reflects the weighted average of its children. A manual update to a parent triggers a "bulk lift" operation on children, then the parent immediately recalculates from their actual values.

## Sync Flow

### Scenario: User updates SOIL INVESTIGATION to 39%

#### Before Update
```
SOIL INVESTIGATION: 0%
├─ Verification of Soil Bearing Capacity - Priority 1: 0%
├─ Verification of Soil Bearing Capacity - Priority 2: 0%
```

#### Step-by-Step Execution

**1. Manual Update (User Action)**
```csharp
activity.PercentComplete = 39%; // Temporary value
activity.Status = "In Progress";
```

**2. Top-Down Push (Only if parentValue > childValue)**
```csharp
// Push 39% to descendants where current progress < 39%
ExecuteUpdateAsync:
  - Priority 1: 0% → 39% ✅ (0 < 39)
  - Priority 2: 0% → 39% ✅ (0 < 39)
```

**Key Condition:** `a.PercentComplete < request.PercentComplete`
- If Priority 1 was already 50%, it stays 50% (50 > 39)
- If Priority 2 was 100%, it stays 100% (completed)

**3. Immediate Recalculation (Data Integrity)**
```csharp
// Reload direct children from DB (they now have updated values)
var directChildren = LoadFromDatabase();
// Priority 1: 39%, Weight: 1.0
// Priority 2: 39%, Weight: 1.0

// Recalculate parent as weighted average
activity.PercentComplete = (39 × 1.0 + 39 × 1.0) / (1.0 + 1.0) = 39%
activity.Status = "In Progress";
```

**Parent now reflects TRUE average of children.**

**4. Bottom-Up Propagation**
```csharp
// Walk up to ancestors and recalculate each
parent = EARLY DELIVERABLES
directChildren = [PREPARATION & SUBMISSION, SOIL INVESTIGATION, ...]
// Use in-memory value for SOIL INVESTIGATION (39%, recalculated from children)
parent.PercentComplete = weighted_average(all_children)
```

#### After Update
```
SOIL INVESTIGATION: 39% (computed from children)
├─ Verification Priority 1: 39%
├─ Verification Priority 2: 39%
```

## Example Scenarios

### Scenario 1: Parent "Lifts" Children

**Before:**
```
PREPARATION & SUBMISSION: 60%
├─ Construction Environmental: 100%
├─ Quality Management Plan: 15%
├─ Safety Program: 20%
├─ Baseline Programme: 50%
```

**User Action:** Set PREPARATION & SUBMISSION to 80%

**After Top-Down Push:**
```
├─ Construction Environmental: 100% (unchanged, 100 > 80)
├─ Quality Management Plan: 80% (updated, 15 < 80)
├─ Safety Program: 80% (updated, 20 < 80)
├─ Baseline Programme: 80% (updated, 50 < 80)
```

**After Recalculation (Weighted Average):**
```
Weight = 1.0 for all children (default)
PREPARATION & SUBMISSION = (100 × 1.0 + 80 × 1.0 + 80 × 1.0 + 80 × 1.0) / (1.0 + 1.0 + 1.0 + 1.0)
                         = 340 / 4
                         = 85%
```

**Final State:**
```
PREPARATION & SUBMISSION: 85% ← computed from children (not 80%)
├─ Construction Environmental: 100%
├─ Quality Management Plan: 80%
├─ Safety Program: 80%
├─ Baseline Programme: 80%
```

### Scenario 2: Weighted Children

**Before:**
```
PREPARATION & SUBMISSION: 60%
├─ Construction Environmental: 100% (Weight: 0.4 - 40% importance)
├─ Quality Management Plan: 60% (Weight: 0.6 - 60% importance)
```

**User Action:** Set PREPARATION & SUBMISSION to 80%

**After Top-Down Push:**
```
├─ Construction Environmental: 100% (unchanged, 100 > 80)
├─ Quality Management Plan: 80% (updated, 60 < 80)
```

**After Recalculation (Weighted Average):**
```
PREPARATION & SUBMISSION = (100 × 0.4 + 80 × 0.6) / (0.4 + 0.6)
                         = (40 + 48) / 1.0
                         = 88%
```

**Final State:**
```
PREPARATION & SUBMISSION: 88% ← weighted average (not 80%)
├─ Construction Environmental: 100% (Weight: 0.4)
├─ Quality Management Plan: 80% (Weight: 0.6)
```

### Scenario 3: Child Already Higher Than Parent

**Before:**
```
PREPARATION & SUBMISSION: 40%
├─ Construction Environmental: 100%
├─ Quality Management Plan: 80%
├─ Safety Program: 20%
```

**User Action:** Set PREPARATION & SUBMISSION to 50%

**After Top-Down Push:**
```
├─ Construction Environmental: 100% (unchanged, 100 > 50)
├─ Quality Management Plan: 80% (unchanged, 80 > 50)
├─ Safety Program: 50% (updated, 20 < 50)
```

**After Recalculation:**
```
PREPARATION & SUBMISSION = (100 + 80 + 50) / 3 = 76.67%
```

**Final State:**
```
PREPARATION & SUBMISSION: 76.67% ← reflects actual children (not 50%)
├─ Construction Environmental: 100%
├─ Quality Management Plan: 80%
├─ Safety Program: 50%
```

## Key Benefits

### 1. Data Integrity
- Parent ALWAYS reflects true weighted average
- No manual overrides can create inconsistencies
- Mathematical correctness guaranteed

### 2. Intuitive Bulk Update
- User sets parent to 80% → all lagging children get "lifted" to 80%
- Children already ahead stay ahead (no regression)
- Useful for project managers: "All activities must be at least 50% by Friday"

### 3. No Rounding Gaps
- All calculations rounded to 2 decimal places
- Prevents drift and UI display issues

### 4. Auto-Sync in Both Directions
- **Bottom-Up:** Child update → parent recalculates
- **Top-Down:** Parent update → children lift → parent recalculates

## Implementation Details

### Top-Down Push Condition

**Previous (incorrect):**
```csharp
.Where(a => descendantIds.Contains(a.ScheduleActivityId) && a.PercentComplete < 100)
```
❌ Problem: Pushes to all incomplete children, even if they're ahead of parent

**Current (correct):**
```csharp
.Where(a => descendantIds.Contains(a.ScheduleActivityId) && a.PercentComplete < request.PercentComplete)
```
✅ Solution: Only pushes to children below parent's new value

### Parent Recalculation (Critical)

**After top-down push:**
```csharp
// Reload direct children from DB (they have updated values after ExecuteUpdateAsync)
var directChildren = await _context.ScheduleActivities
    .Where(a => a.ParentActivityId == activity.ScheduleActivityId)
    .ToListAsync(cancellationToken);

// Recalculate parent from actual children values
decimal weightedSum = 0;
decimal totalWeight = 0;
foreach (var child in directChildren)
{
    var childWeight = child.Weight > 0 ? child.Weight : 1.0m;
    weightedSum += child.PercentComplete * childWeight;
    totalWeight += childWeight;
}
activity.PercentComplete = totalWeight > 0 
    ? Math.Round(weightedSum / totalWeight, 2) 
    : 0;
```

**This ensures parent value is COMPUTED, not manually set.**

### Bottom-Up Propagation Fix

**Previous:**
```csharp
var childProgress = child.ScheduleActivityId == request.ScheduleActivityId
    ? request.PercentComplete  // ❌ Uses manual update value
    : child.PercentComplete;
```

**Current:**
```csharp
var childProgress = child.ScheduleActivityId == activity.ScheduleActivityId
    ? activity.PercentComplete  // ✅ Uses recalculated value
    : child.PercentComplete;
```

When walking up to ancestors, we use the **recalculated** parent value (computed from children), not the original manual update value.

## Edge Cases

### Case 1: All Children Already Ahead
```
Parent manual update: 30%
Children: 50%, 60%, 70%
Result: No push (all children > 30%), parent recalculates to 60%
```

### Case 2: All Children at 100%
```
Parent manual update: 80%
Children: 100%, 100%, 100%
Result: No push (all children > 80%), parent recalculates to 100%
```

### Case 3: Mixed Children
```
Parent manual update: 70%
Children: 50% (w:0.3), 80% (w:0.7)
Push: 50% → 70%
Recalculate: (70 × 0.3 + 80 × 0.7) / 1.0 = 77%
```

## Testing Checklist

- [x] Top-down only pushes if parentValue > childValue
- [x] Parent recalculates immediately after push
- [x] Parent always reflects weighted average (not manual value)
- [x] Bottom-up uses recalculated values
- [x] Weighted calculation respects child weights
- [x] Precision: 2 decimal places throughout
- [x] No linter errors

## Summary

The parent-child sync ensures:
1. **Manual updates trigger bulk lift** (children below parent → match parent)
2. **Parent immediately recalculates** (always reflects true average)
3. **Data integrity guaranteed** (parent = computed property)
4. **Ancestors cascade** (bottom-up recalculation)

This creates a **mathematically consistent hierarchy** where parent values are always trustworthy reflections of their children's progress.
