# Visual Guide: Import Excel Button Location

## Button Placement

The **Import Excel** button is placed immediately after the **Export Excel** button for each box type section.

---

## Before (Original Layout)

```
┌────────────────────────────────────────────────────────────────────┐
│  Project Materials Management                                      │
├────────────────────────────────────────────────────────────────────┤
│                                                                     │
│  📊 Statistics: 3 Box Types | 49 Materials | 5 Delivered          │
│                                                                     │
│  ┌────────────────────────────────────────────────────────────┐  │
│  │  📦 Loose Element (17 materials)     [Export Excel ↓]      │  │
│  ├────────────────────────────────────────────────────────────┤  │
│  │  Material Table...                                          │  │
│  └────────────────────────────────────────────────────────────┘  │
│                                                                     │
│  ┌────────────────────────────────────────────────────────────┐  │
│  │  📦 S1 (15 materials)                [Export Excel ↓]      │  │
│  ├────────────────────────────────────────────────────────────┤  │
│  │  Material Table...                                          │  │
│  └────────────────────────────────────────────────────────────┘  │
│                                                                     │
│  ┌────────────────────────────────────────────────────────────┐  │
│  │  📦 S2 (17 materials)                [Export Excel ↓]      │  │
│  ├────────────────────────────────────────────────────────────┤  │
│  │  Material Table...                                          │  │
│  └────────────────────────────────────────────────────────────┘  │
│                                                                     │
└────────────────────────────────────────────────────────────────────┘
```

---

## After (With Import Button Added)

```
┌────────────────────────────────────────────────────────────────────┐
│  Project Materials Management                                      │
├────────────────────────────────────────────────────────────────────┤
│                                                                     │
│  📊 Statistics: 3 Box Types | 49 Materials | 5 Delivered          │
│                                                                     │
│  ┌────────────────────────────────────────────────────────────┐  │
│  │  📦 Loose Element (17 materials)                            │  │
│  │                     [Export Excel ↓]  [Import Excel ↑] ◄── NEW!│
│  ├────────────────────────────────────────────────────────────┤  │
│  │  Material Table...                                          │  │
│  └────────────────────────────────────────────────────────────┘  │
│                                                                     │
│  ┌────────────────────────────────────────────────────────────┐  │
│  │  📦 S1 (15 materials)                                        │  │
│  │                     [Export Excel ↓]  [Import Excel ↑] ◄── NEW!│
│  ├────────────────────────────────────────────────────────────┤  │
│  │  Material Table...                                          │  │
│  └────────────────────────────────────────────────────────────┘  │
│                                                                     │
│  ┌────────────────────────────────────────────────────────────┐  │
│  │  📦 S2 (17 materials)                                        │  │
│  │                     [Export Excel ↓]  [Import Excel ↑] ◄── NEW!│
│  ├────────────────────────────────────────────────────────────┤  │
│  │  Material Table...                                          │  │
│  └────────────────────────────────────────────────────────────┘  │
│                                                                     │
└────────────────────────────────────────────────────────────────────┘
```

---

## Detailed Button View

### Box Type Header Section (Expanded View)

```
┌──────────────────────────────────────────────────────────────────────┐
│  ▼  📦 Loose Element (17 materials)                                  │
│                                                                       │
│                 ┌─────────────────┐  ┌─────────────────┐           │
│                 │  Export Excel ↓ │  │  Import Excel ↑ │  ◄── HERE │
│                 │   (Green Btn)   │  │   (Blue Btn)    │           │
│                 └─────────────────┘  └─────────────────┘           │
├──────────────────────────────────────────────────────────────────────┤
│  📊 2 boxes | 0 delivered items (0 materials × 2 boxes)             │
├──────────────────────────────────────────────────────────────────────┤
│                                                                       │
│  Material Table (when expanded):                                     │
│  ┌─────┬───────┬──────────┬─────────┬──────────┬─────────┬────────┐│
│  │ St. │ Code  │   Name   │ Qty/Box │ Delivered│  Total  │ Actions││
│  ├─────┼───────┼──────────┼─────────┼──────────┼─────────┼────────┤│
│  │ ○ P │ MAT-1 │ Material │    5    │    -     │   10    │ [Edit] ││
│  │ ○ P │ MAT-2 │ Material │    3    │    -     │    6    │ [Edit] ││
│  └─────┴───────┴──────────┴─────────┴──────────┴─────────┴────────┘│
│                                                                       │
└──────────────────────────────────────────────────────────────────────┘
```

---

## Button Comparison

### Export Excel Button
```
┌─────────────────────┐
│  ↓                  │  ← Download icon
│  Export Excel       │  ← Green background
└─────────────────────┘
     (Downloads current data)
```

### Import Excel Button
```
┌─────────────────────┐
│  ↑                  │  ← Upload icon
│  Import Excel       │  ← Blue background
└─────────────────────┘
     (Uploads edited data)
```

---

## User Interaction Flow

### 1. Click Export Button
```
User clicks: [Export Excel ↓]
    ↓
Downloads: S1_Materials_2026-02-08.xlsx
```

### 2. Edit Excel File Offline
```
Opens file in Excel/LibreOffice
    ↓
Edits columns:
  • Quantity Per Box
  • Delivered Quantity
  • Notes
    ↓
Saves file
```

### 3. Click Import Button
```
User clicks: [Import Excel ↑]
    ↓
File dialog appears
    ↓
Selects edited Excel file
    ↓
File uploads to server
    ↓
Shows result:
  ✅ "Import completed. 12 material(s) updated successfully."
  OR
  ⚠️ "Row 5: Delivered Quantity exceeds Total Quantity"
    ↓
Materials auto-refresh
```

---

## Button States

### Normal State
```
[Export Excel ↓]  [Import Excel ↑]
   (Clickable)        (Clickable)
```

### Loading State (during upload)
```
[Export Excel ↓]  [Import Excel ↑]
   (Disabled)        (Disabled + Spinner)
```

### Success State (after import)
```
✅ Import completed. 12 material(s) updated successfully.

[Export Excel ↓]  [Import Excel ↑]
   (Clickable)        (Clickable)
```

### Error State
```
⚠️ Import failed: Row 5: Delivered Quantity (100) cannot exceed Total Quantity (80)

[Export Excel ↓]  [Import Excel ↑]
   (Clickable)        (Clickable)
```

---

## Mobile/Responsive View

### Desktop (both buttons visible)
```
┌────────────────────────────────────────────┐
│  📦 S1 (15 materials)                      │
│        [Export ↓]  [Import ↑]              │
└────────────────────────────────────────────┘
```

### Tablet (buttons stack if needed)
```
┌──────────────────────────┐
│  📦 S1 (15 materials)    │
│     [Export Excel ↓]     │
│     [Import Excel ↑]     │
└──────────────────────────┘
```

---

## Screenshot Reference

The implementation matches the screenshot provided by the user, where each box type section shows:

1. **Box type name** with collapse toggle
2. **Material count** in parentheses
3. **Export Excel button** (green, on the right)
4. **Import Excel button** (blue, next to Export) ← **NEW**

The buttons are aligned to the right side of the box type header, maintaining the same visual style and spacing as other action buttons in the interface.

---

**Visual Guide Version:** 1.0  
**Last Updated:** February 8, 2026
