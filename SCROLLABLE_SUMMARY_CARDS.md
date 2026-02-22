# Scrollable Summary Cards Implementation

## Overview
Made the "Dispatched Boxes" and "Boxes by Building" summary cards scrollable to handle cases with many entries while keeping the "Total Boxes Overview" card at its natural height.

## Implementation Details

### 1. **Selective Scrolling**
Only cards with table headers (thead) are made scrollable:
- ✅ Card 2: Dispatched Boxes - **Scrollable**
- ✅ Card 3: Boxes by Building - **Scrollable**
- ❌ Card 1: Total Boxes Overview - **Not scrollable** (remains at natural height)

### 2. **Max Height & Overflow**
```scss
.card-body:has(table thead) {
  max-height: 300px;          // Desktop height
  overflow-y: auto;           // Vertical scrolling
  overflow-x: hidden;         // No horizontal scroll
  scroll-behavior: smooth;    // Smooth scrolling
}
```

Mobile adjustment:
```scss
@media (max-width: 768px) {
  max-height: 240px;          // Reduced for mobile
}
```

### 3. **Sticky Headers & Footers**

#### Sticky Table Headers (Top)
```scss
thead th {
  position: sticky;
  top: 0;
  background: #f8fafc;
  z-index: 10;
}
```
- Headers stay visible while scrolling
- Column labels always accessible

#### Sticky Total Row (Bottom)
```scss
tfoot td {
  position: sticky;
  bottom: 0;
  background: #f8fafc;
  z-index: 9;
}
```
- Total row always visible at bottom
- Quick access to summary totals

### 4. **Custom Scrollbar**
Beautiful, minimal scrollbar styling:
```scss
&::-webkit-scrollbar {
  width: 6px;                 // Thin scrollbar
}

&::-webkit-scrollbar-track {
  background: #f1f5f9;        // Light track
  border-radius: 3px;
}

&::-webkit-scrollbar-thumb {
  background: #cbd5e1;        // Gray thumb
  border-radius: 3px;
  
  &:hover {
    background: #94a3b8;      // Darker on hover
  }
}
```

### 5. **Scroll Shadow Indicators**
Visual cues for scrollable content:
```scss
background:
  // Fade at top
  linear-gradient(white 30%, rgba(255, 255, 255, 0)),
  // Fade at bottom
  linear-gradient(rgba(255, 255, 255, 0), white 70%) 0 100%,
  // Shadow at top
  radial-gradient(farthest-side at 50% 0, rgba(0, 0, 0, 0.1), rgba(0, 0, 0, 0)),
  // Shadow at bottom
  radial-gradient(farthest-side at 50% 100%, rgba(0, 0, 0, 0.1), rgba(0, 0, 0, 0)) 0 100%;
```
- Subtle shadows indicate more content above/below
- Automatic visual feedback

### 6. **Export Mode Compatibility**
Full content shown in PDF/PNG exports:
```scss
.layout-print-area.export-mode .summary-card .card-body {
  max-height: none !important;      // Remove height limit
  overflow: visible !important;      // Show all content
  background: white !important;      // Remove shadows
}
```
- Ensures complete data in exports
- No truncation in PDFs

## Features

### ✅ User Experience
1. **Consistent Layout** - Cards maintain uniform height
2. **Quick Scanning** - Headers and totals always visible
3. **Smooth Scrolling** - Native smooth scroll behavior
4. **Visual Feedback** - Shadow indicators show scroll state
5. **Touch Optimized** - Works great on mobile devices

### ✅ Responsive Design
- **Desktop**: 300px max height
- **Mobile**: 240px max height
- **Export**: Full height (no truncation)

### ✅ Accessibility
- Keyboard scrolling supported
- Touch gestures enabled
- Clear visual boundaries

### ✅ Performance
- Hardware-accelerated scrolling
- Efficient CSS-only implementation
- No JavaScript overhead

## Use Cases

### When It Helps
1. **Many Floors** - Multiple dispatch locations (10+ floors)
2. **Many Buildings** - Large projects (5+ buildings)
3. **Data Growth** - As project scales over time
4. **Limited Screen Space** - Keeps UI compact

### Example Scenarios
```
Dispatched Boxes:
- Floor 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12...
  ↓ Scroll to see more ↓

Boxes by Building:
- AP01, AP02, AP03, BG01, BG02, RO01...
  ↓ Scroll to see more ↓
```

## Browser Support

- ✅ Chrome/Edge (full support)
- ✅ Firefox (full support)
- ✅ Safari (full support)
- ✅ Mobile browsers (iOS/Android)

## Technical Notes

### CSS Selector
Uses `:has()` pseudo-class for smart targeting:
```scss
.card-body:has(table thead) {
  // Only applied to cards with table headers
}
```

### Z-Index Layers
- Header: `z-index: 10` (highest)
- Footer: `z-index: 9` (middle)
- Content: default (lowest)

### Background Attachment
```scss
background-attachment: local, local, scroll, scroll;
```
- First two layers: scroll with content
- Last two layers: fixed (shadow indicators)

## Files Modified

1. `factory-layout.component.scss`
   - Added `.card-body:has(table thead)` scrolling rules
   - Sticky headers and footers
   - Custom scrollbar styling
   - Scroll shadow indicators
   - Mobile responsiveness
   - Export mode overrides

## Testing Checklist

- [ ] Cards scroll when content exceeds 300px
- [ ] Headers stay at top when scrolling
- [ ] Totals stay at bottom when scrolling
- [ ] Scrollbar appears and is styled correctly
- [ ] Shadow indicators show/hide appropriately
- [ ] Mobile view uses 240px max height
- [ ] Export PDF shows full content (no truncation)
- [ ] Total Boxes Overview card doesn't scroll (stays at natural height)
- [ ] Smooth scrolling works on all devices

## Date
February 17, 2026
