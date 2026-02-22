# Activity Checklist Review - Visual Example

## Before (Flat List)

```
┌────────────────────────────────────────────────────────────────┐
│ ✓ Activity Checklist Review                               ✕   │
├────────────────────────────────────────────────────────────────┤
│ Activity: Assembly & joints                    STAGEA-ASM      │
├────────────────────────────────────────────────────────────────┤
│ Review Progress                                           0%   │
│ ▓▓▓▓▓▓▓▓▓░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░      │
├───┬──────────────────────────┬──────────┬──────────┬──────────┤
│ # │ Description              │ Reference│ Mandatory│ Status   │
├───┼──────────────────────────┼──────────┼──────────┼──────────┤
│ 1 │ All joints sealed...     │ REF-0013 │ Yes      │ Pending  │
│ 2 │ Erection of walls...     │ REF-0018 │ Yes      │ Pending  │
│ 3 │ Check height of box...   │ REF-0010 │ Yes      │ Pending  │
│ 4 │ Panel connections...     │ REF-0019 │ Yes      │ Pending  │
│ 5 │ Dimensions and level...  │ REF-0020 │ Yes      │ Pending  │
│...│ (continues...)           │          │          │          │
└───┴──────────────────────────┴──────────┴──────────┴──────────┘
```

## After (Grouped & Collapsible - Default Collapsed State)

```
┌────────────────────────────────────────────────────────────────┐
│ ✓ Activity Checklist Review                               ✕   │
├────────────────────────────────────────────────────────────────┤
│ Activity: Assembly & joints                    STAGEA-ASM      │
├────────────────────────────────────────────────────────────────┤
│ Review Progress                                           0%   │
│ ▓▓▓▓▓▓▓▓▓░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░      │
├────────────────────────────────────────────────────────────────┤
│                                   Expand All  Collapse All     │ ← Control Buttons
│                                                                │
│ ┌──────────────────────────────────────────────────────────┐  │
│ │ ▶ CL-32  Pre-Construction Checklist                     │  │ ← COLLAPSED (chevron →)
│ └──────────────────────────────────────────────────────────┘  │
│                                                                │
│ ┌──────────────────────────────────────────────────────────┐  │
│ │ ▶ CL-18  Another Checklist                              │  │ ← COLLAPSED
│ └──────────────────────────────────────────────────────────┘  │
│                                                                │
├────────────────────────────────────────────────────────────────┤
│                                           Cancel  Submit Review│
└────────────────────────────────────────────────────────────────┘
```

## After (Grouped & Collapsible - Expanded State)

```
┌────────────────────────────────────────────────────────────────┐
│ ✓ Activity Checklist Review                               ✕   │
├────────────────────────────────────────────────────────────────┤
│ Activity: Assembly & joints                    STAGEA-ASM      │
├────────────────────────────────────────────────────────────────┤
│ Review Progress                                           0%   │
│ ▓▓▓▓▓▓▓▓▓░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░      │
├────────────────────────────────────────────────────────────────┤
│                                   Expand All  Collapse All     │
│                                                                │
│ ┌──────────────────────────────────────────────────────────┐  │
│ │ ▼ CL-32  Pre-Construction Checklist                     │  │ ← EXPANDED (chevron ▼)
│ └──────────────────────────────────────────────────────────┘  │
│                                                                │
│ ┌──────────────────────────────────────────────────────────┐  │
│ │ ▼ GENERAL                                                │  │ ← EXPANDED SECTION
│ ├───┬──────────────────────────┬──────────┬──────────────┬──┤  │
│ │ # │ Description              │ Reference│ Mandatory    │ S│  │
│ ├───┼──────────────────────────┼──────────┼──────────────┼──┤  │
│ │ 1 │ Ensure method statement..│ REF-0001 │ Yes      │✓✕○│  │ ← New sequence: 1
│ │ 2 │ Ensure materials stored..│ REF-0002 │ Yes      │✓✕○│  │ ← New sequence: 2
│ │ 3 │ Check the expiry date... │ REF-0003 │ Yes      │✓✕○│  │ ← New sequence: 3
│ └───┴──────────────────────────┴──────────┴──────────────┴──┘  │
│                                                                │
│ ┌──────────────────────────────────────────────────────────┐  │
│ │ ▶ STRUCTURAL                                             │  │ ← COLLAPSED SECTION
│ └──────────────────────────────────────────────────────────┘  │
│                                                                │
│ ┌──────────────────────────────────────────────────────────┐  │
│ │ ▼ PREPARATION & SETTING OUT                             │  │ ← EXPANDED SECTION
│ ├───┬──────────────────────────┬──────────┬──────────────┬──┤  │
│ │ # │ Description              │ Reference│ Mandatory    │ S│  │
│ ├───┼──────────────────────────┼──────────┼──────────────┼──┤  │
│ │ 1 │ Ensure Drawing Stamp...  │ REF-0004 │ Yes      │✓✕○│  │ ← Restarts at 1
│ │ 2 │ Floor Setting Out...     │ REF-0005 │ Yes      │✓✕○│  │ ← New sequence: 2
│ └───┴──────────────────────────┴──────────┴──────────────┴──┘  │
│                                                                │
│ ┌──────────────────────────────────────────────────────────┐  │
│ │ ▶ CL-18  Another Checklist                              │  │ ← COLLAPSED CHECKLIST
│ └──────────────────────────────────────────────────────────┘  │
│                                                                │
├────────────────────────────────────────────────────────────────┤
│                                           Cancel  Submit Review│
└────────────────────────────────────────────────────────────────┘
```

## Sequential Numbering

**Important:** The sequence numbers shown in the `#` column are **re-numbered sequentially within each section**, starting from 1.

- **GENERAL Section**: Items numbered 1, 2, 3...
- **PREPARATION & SETTING OUT Section**: Items numbered 1, 2... (restarts)
- **ERECTION Section**: Items numbered 1, 2... (restarts)

This provides cleaner, more intuitive numbering for users reviewing the checklist, regardless of the original sequence numbers in the database.

## Key Visual Elements

### Checklist Header
- **Color**: Blue gradient background (#1e40af → #3b82f6)
- **Content**: Checklist code badge + Checklist name
- **Typography**: Bold, 18px, white text
- **Example**: "CL-32 Pre-Construction Checklist"

### Section Header
- **Color**: Light gray gradient background (#f8fafc → #e2e8f0)
- **Border**: 4px blue left border (#3b82f6)
- **Content**: Section title in uppercase
- **Typography**: Semi-bold, 15px, dark text
- **Example**: "GENERAL", "STRUCTURAL", "MEP"

### Table
- **Border**: 1px solid border, rounded corners
- **Position**: Directly under section header
- **Content**: All checklist items for that section
- **Interaction**: Same as before (status buttons, remarks input)

## Interactive Features

### Collapse/Expand Behavior

**Chevron Icons:**
- ▶ (Right-pointing) = Collapsed
- ▼ (Down-pointing) = Expanded

**Click Interactions:**
1. **Click Checklist Header** → Toggles all sections within that checklist
2. **Click Section Header** → Toggles items table for that section
3. **Click "Expand All"** → Opens all checklists and all sections
4. **Click "Collapse All"** → Closes all checklists (sections also close)

**Visual Feedback:**
- Headers have hover effects (darker background, slight lift)
- Chevron icon rotates smoothly when toggling
- Content slides down/up with smooth animation
- Cursor changes to pointer on headers

### Default State
- **All Collapsed**: Users see only checklist names initially
- **Clean View**: Reduces overwhelming amount of information
- **Progressive Disclosure**: Users expand only what they need to review

## Benefits of This Layout

1. **Clear Hierarchy**: 
   - Level 1: Checklist (blue header)
   - Level 2: Section (gray header with blue accent)
   - Level 3: Items (table rows)

2. **Easy Scanning**: 
   - Users can quickly identify which checklist and section they're reviewing
   - Related items are grouped together
   - Collapsed by default prevents information overload

3. **Professional Look**: 
   - Color-coded headers
   - Smooth animations and transitions
   - Interactive chevron indicators
   - Consistent spacing and alignment
   - Modern, clean design

4. **Flexibility**: 
   - Can handle any number of checklists
   - Can handle any number of sections per checklist
   - Gracefully handles missing checklist/section information
   - Users control what they want to see

5. **Better UX**:
   - Expand All / Collapse All for quick access
   - Progressive disclosure reduces cognitive load
   - Clear visual indicators of expand/collapse state
