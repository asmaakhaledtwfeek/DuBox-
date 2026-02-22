# Collapse/Expand Feature for Activity Checklist Review

## Overview
Added collapse/expand functionality to the Activity Checklist Review modal to improve usability and reduce visual clutter. All checklists and sections are **collapsed by default**.

## Implementation Summary

### 1. Component Changes (`activity-checklist-review-modal.component.ts`)

**Added Properties:**
- `isExpanded?: boolean` to both `GroupedChecklistItem` and `GroupedSection` interfaces
- Default value: `false` (collapsed)

**New Methods:**
```typescript
toggleChecklist(checklist)      // Toggle individual checklist
toggleSection(section)          // Toggle individual section
expandAllChecklists()          // Expand everything
collapseAllChecklists()        // Collapse everything
```

### 2. Template Changes (`activity-checklist-review-modal.component.html`)

**Added:**
- Expand All / Collapse All control buttons at the top
- Chevron icons in headers (rotate based on state)
- Click handlers on checklist and section headers
- Conditional rendering based on `isExpanded` state
- Wrapper divs for animation effects

**Chevron Indicators:**
- ▶ (Right) = Collapsed
- ▼ (Down) = Expanded

### 3. Style Changes (`activity-checklist-review-modal.component.scss`)

**New Styles:**
- `.collapse-controls` - Control buttons styling
- `.btn-collapse-control` - Individual button styles
- `.chevron-icon` - Rotating chevron with smooth transition
- `cursor: pointer` on headers
- Hover effects for interactive headers
- `@keyframes slideDown` - Smooth expand animation
- `.collapsed` class for collapsed state

**Visual Effects:**
- Smooth rotation of chevron icons (90deg)
- Slide-down animation when expanding
- Hover effects (darker background, slight lift)
- Rounded corners adjust based on state

## User Experience

### Default Behavior
1. Modal opens with all checklists **collapsed**
2. Only checklist names are visible (CHK-PC-001, etc.)
3. Clean, uncluttered initial view

### Interaction Flow
```
1. User sees collapsed checklists
   ↓
2. Clicks on a checklist header
   ↓
3. Checklist expands, showing section headers (collapsed)
   ↓
4. Clicks on a section header
   ↓
5. Section expands, showing table with items
   ↓
6. User reviews items, changes status, adds remarks
   ↓
7. Clicks section or checklist header to collapse
```

### Quick Actions
- **Expand All**: Opens all checklists and all sections at once
- **Collapse All**: Closes everything back to initial state

## Visual Example

### Initial State (All Collapsed)
```
┌─────────────────────────────────┐
│ [Expand All] [Collapse All]    │
│                                 │
│ ▶ CHK-PC-001 Construction of... │
│ ▶ CHK-PC-002 Another Checklist │
└─────────────────────────────────┘
```

### One Checklist Expanded
```
┌─────────────────────────────────┐
│ [Expand All] [Collapse All]    │
│                                 │
│ ▼ CHK-PC-001 Construction of... │
│   ▶ GENERAL                    │
│   ▶ PREPARATION & SETTING OUT  │
│   ▶ ERECTION / ASSEMBLING      │
│                                 │
│ ▶ CHK-PC-002 Another Checklist │
└─────────────────────────────────┘
```

### Fully Expanded
```
┌─────────────────────────────────┐
│ [Expand All] [Collapse All]    │
│                                 │
│ ▼ CHK-PC-001 Construction of... │
│   ▼ GENERAL                    │
│   ┌──────────────────────────┐ │
│   │ # | Description | Status │ │
│   │ 1 | Ensure...   | ✓✕○   │ │ ← Sequential: 1, 2, 3...
│   │ 2 | Check...    | ✓✕○   │ │
│   └──────────────────────────┘ │
│   ▼ PREPARATION & SETTING OUT  │
│   ┌──────────────────────────┐ │
│   │ # | Description | Status │ │
│   │ 1 | Drawing...  | ✓✕○   │ │ ← Restarts at 1
│   └──────────────────────────┘ │
│   ▶ ERECTION / ASSEMBLING      │
└─────────────────────────────────┘
```

**Note:** Sequence numbers are re-numbered within each section (1, 2, 3...) for better readability.

## Benefits

### 1. Reduced Cognitive Load
- Users see only high-level structure initially
- Progressive disclosure of details
- Less overwhelming for checklists with many items

### 2. Better Navigation
- Quickly scan checklist names
- Expand only relevant sections
- Collapse reviewed sections to stay organized

### 3. Improved Performance
- Fewer DOM elements rendered initially
- Smoother scrolling
- Faster initial load

### 4. Flexible Review Process
- Expand All: Review everything sequentially
- Selective: Expand only what needs attention
- Collapse All: Reset to overview

## Technical Details

### Animation
- **Duration**: 0.3s
- **Easing**: ease
- **Effect**: slideDown (opacity + max-height)

### Chevron Rotation
- **Duration**: 0.2s
- **Easing**: ease
- **Angle**: 0deg (collapsed) → 90deg (expanded)

### State Management
- State stored in component data structure
- No external state management needed
- Persists during user interaction
- Resets on modal close/reopen

## Testing Checklist

- [ ] All checklists start collapsed
- [ ] Clicking checklist header toggles it
- [ ] Clicking section header toggles it
- [ ] Chevron icons rotate correctly
- [ ] Expand All button works
- [ ] Collapse All button works
- [ ] Hover effects appear on headers
- [ ] Animations are smooth
- [ ] Can still review items when expanded
- [ ] Status changes work correctly
- [ ] Remarks input works correctly
- [ ] Submit review includes all items (even collapsed)

## Browser Compatibility
- Chrome: ✅
- Firefox: ✅
- Edge: ✅
- Safari: ✅

All modern browsers support CSS transitions and animations used.
