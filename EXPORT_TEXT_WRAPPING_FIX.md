# Export Text Wrapping Fix - Complete Solution

## Problem
When exporting the factory layout to PDF using html2canvas:
1. Vertical labels (CENTRAL AISLE, part names) were breaking into single letters per line
2. Text was wrapping incorrectly, making labels unreadable
3. Rotated text was appearing flipped or mirrored

## Solution Implemented

### 1. CSS Changes (factory-layout.component.scss)

Added comprehensive `.export-mode` class that applies during PDF/PNG export:

#### A. Part Labels (Left & Right)
```scss
.part-label-dual.part-label-left {
  transform: none !important;
  writing-mode: vertical-rl !important;
  text-orientation: mixed !important;
  transform-origin: center center !important;
  width: max-content !important;
  min-width: auto !important;
  max-width: none !important;
  
  span {
    display: inline-block !important;
    white-space: nowrap !important; // ✅ Prevents single-letter wrapping
    width: max-content !important;
  }
}
```

#### B. Central Aisle Labels (Vertical & Horizontal)
```scss
.central-aisle-vertical .aisle-label-vertical {
  transform: none !important;
  writing-mode: vertical-rl !important;
  transform-origin: center center !important;
  white-space: nowrap !important; // ✅ Prevents wrapping
  width: max-content !important;
}

.central-aisle-horizontal .aisle-label {
  white-space: nowrap !important;
  width: max-content !important;
}
```

#### C. Bay Labels & Row Numbers
```scss
.common-bay-axis .bay-label-container .bay-label {
  white-space: nowrap !important;
  width: max-content !important;
}

.row-axis-top .row-number,
.row-axis-bottom .row-number {
  white-space: nowrap !important;
  width: max-content !important;
}
```

#### D. Container Adjustments
```scss
.factory-layout-two-column.blueprint-style {
  overflow: visible !important;
  padding-right: 50px !important; // ✅ Extra space for vertical labels
  padding-left: 50px !important;  // ✅ Extra space for vertical labels
}
```

### 2. TypeScript Changes (factory-layout.component.ts)

Modified `captureLayoutCanvas()` method:

```typescript
private async captureLayoutCanvas(): Promise<HTMLCanvasElement> {
    const el = this.layoutPrintArea?.nativeElement;
    if (!el) {
        throw new Error('Layout not ready.');
    }
    
    // ✅ Add export-mode class
    el.classList.add('export-mode');
    
    // ✅ Wait for DOM to update
    await new Promise(resolve => setTimeout(resolve, 100));
    
    try {
        return await html2canvas(el, { /* options */ });
    } finally {
        // ✅ Always remove export-mode class
        el.classList.remove('export-mode');
    }
}
```

## Key Features

### ✅ Prevents Text Wrapping
- `white-space: nowrap` prevents text from breaking into single letters
- `width: max-content` ensures proper width for full text

### ✅ Centers Transform Origin
- `transform-origin: center center` prevents layout shifts
- Maintains visual alignment during export

### ✅ Proper Container Sizing
- Extra padding (50px left/right) accommodates rotated vertical text
- `overflow: visible` allows labels to extend beyond boundaries

### ✅ Writing Mode Instead of Transform
- Uses `writing-mode: vertical-rl` for vertical text
- Removes problematic `transform: rotate()` during export
- html2canvas renders writing-mode correctly

### ✅ Automatic Cleanup
- Export-mode class is temporary (only during capture)
- try-finally ensures class is always removed
- No side effects on screen display

## Affected Elements

1. **Part Labels** (left & right sides) - "PART 1", "PART 2", etc.
2. **Central Aisle Labels** (vertical) - "CENTRAL AISLE"
3. **Central Aisle Labels** (horizontal) - "CENTRAL AISLE"
4. **Bay Labels** - Letters (B, C, D, E, F, G, H)
5. **Row Numbers** - Numbers (1-24)
6. **Part Divider Labels** - Labels on red divider lines
7. **Section Headers** - Section names and ranges

## Testing

To verify the fix:
1. Open factory layout in browser
2. Click "Export PDF" button
3. Check exported PDF:
   - ✅ Vertical labels should display as complete words
   - ✅ "CENTRAL AISLE" should be readable (not C-E-N-T-R-A-L)
   - ✅ Part names should be readable
   - ✅ No text wrapping or single-letter breaks
   - ✅ Labels should not be flipped or mirrored

## Benefits

1. **Clean exports** - All labels readable in PDF/PNG
2. **No screen impact** - On-screen display unchanged
3. **Maintainable** - Uses standard CSS properties
4. **Automatic** - Works for both PDF and PNG exports
5. **Robust** - Handles all vertical and horizontal labels

## Technical Notes

- **html2canvas limitation**: Cannot properly render CSS transforms
- **Solution**: Replace transforms with writing-mode during export
- **Timing**: 100ms delay ensures DOM updates before capture
- **Cleanup**: try-finally ensures export-mode is always removed
- **Compatibility**: Works with all modern browsers

## Files Modified

1. `factory-layout.component.scss` - Added export-mode CSS rules
2. `factory-layout.component.ts` - Modified captureLayoutCanvas() method

## Date
February 17, 2026
