# Boxes Summary Report Implementation

## Overview

This document describes the implementation of the "Boxes Summary Report" feature as specified in the requirements. The report provides a comprehensive view of all boxes with filtering, KPIs, charts, tables, export functionality, and pagination.

## Implementation Status

### ✅ Completed

#### Backend (C#/.NET)

1. **Progress Formatting Helper** (`Dubox.Application/Utilities/ProgressFormatter.cs`)
   - Static utility class for consistent progress formatting
   - Formats to exactly 2 decimal places with "%" symbol
   - Returns "-" for null values
   - Uses `Math.Round(value, 2, MidpointRounding.AwayFromZero)` for consistent rounding

2. **DTOs** (`Dubox.Application/DTOs/ReportDto.cs`)
   - `BoxSummaryReportItemDto` - Individual box entry
   - `BoxSummaryReportKpisDto` - KPI metrics
   - `BoxSummaryReportAggregationsDto` - Chart/aggregation data
   - `ProjectBoxCountDto` - For top projects chart
   - `PaginatedBoxSummaryReportResponseDto` - Complete response with pagination

3. **Query & Handler** (`Dubox.Application/Features/Reports/Queries/GetBoxesSummaryReportQuery.cs`)
   - Comprehensive filtering support:
     - Project, Box Type (multi), Floor, Building, Zone
     - Status (multi-select), Progress Range (min/max)
     - Search (BoxTag, SerialNumber, BoxName)
     - Date range (LastUpdate or PlannedStartDate)
   - Server-side pagination (default: page=1, pageSize=25)
   - Server-side sorting by any column
   - Efficient EF Core queries with proper includes
   - KPIs calculation (Total, In Progress, Completed, Not Started, Avg Progress)
   - Aggregations for charts:
     - Status distribution
     - Progress range distribution (0-25, 26-50, 51-75, 76-100)
     - Top 5 projects by box count
   - LastUpdateDate calculated from ProgressUpdates

4. **API Endpoint** (`Dubox.Api/Controllers/ReportsController.cs`)
   - `GET /api/reports/boxes`
   - Accepts query parameters for filtering, pagination, sorting
   - Returns paginated response with KPIs and aggregations
   - Protected with `[Authorize]` attribute

#### Frontend (Angular)

1. **Progress Formatting Helper** (`dubox-frontend/src/app/core/utils/progress.util.ts`)
   - `formatProgress()` function matching backend logic
   - Consistent 2 decimal place formatting

2. **Models** (`dubox-frontend/src/app/core/models/boxes-summary-report.model.ts`)
   - TypeScript interfaces matching backend DTOs
   - Query parameters interface

3. **Service Methods** (`dubox-frontend/src/app/core/services/reports.service.ts`)
   - `getBoxesSummaryReport()` method
   - Response transformation handling both camelCase and PascalCase
   - Query parameter building

### 🚧 Remaining Work

#### Frontend Component

The frontend component (`boxes-summary-report.component.ts/html/scss`) needs to be created with:

1. **Filter Bar** (collapsible on small screens):
   - Project select dropdown
   - Box Type multi-select
   - Floor, Building, Zone selects
   - Status multi-select
   - Progress range slider or min/max inputs
   - Date range picker with filter type (LastUpdate/PlannedStart)
   - Search input (debounced 300ms)
   - Apply/Reset buttons

2. **KPI Cards**:
   - Total Boxes
   - In Progress count
   - Completed count
   - Not Started count
   - Average Progress % (formatted to 2 decimals)

3. **Charts Section** (using ng2-charts/Chart.js):
   - Pie/Donut chart: Status distribution
   - Bar chart: Progress ranges (0-25, 26-50, 51-75, 76-100)
   - Optional: Stacked bar chart for top 5 projects

4. **Main Table** (server-side paging & sorting):
   - Checkbox column (bulk selection)
   - Box Tag (clickable link to box details)
   - Serial Number
   - Project
   - Box Type
   - Floor
   - Building / Zone
   - Progress (2 decimals + %, horizontal progress bar)
   - Status (colored badge)
   - Current Location
   - Planned Start - Planned End (date range)
   - Duration (days)
   - Last Update (date)
   - Actions (View, Export row)

5. **Table Features**:
   - Server-side pagination controls
   - Column sorting (clickable headers)
   - Column visibility toggle
   - Column reorder (optional)
   - Row selection + bulk export
   - Row-level quick actions

6. **Export Functionality**:
   - Export visible rows or selected rows
   - CSV export
   - Excel export (using XLSX library)
   - Option: full details vs brief

7. **Responsive Design**:
   - Collapse less-important columns on small screens
   - Mobile-friendly filter bar
   - Responsive table with horizontal scroll

8. **Accessibility**:
   - Proper ARIA labels
   - Keyboard navigation
   - Color contrast for badges

9. **Route**:
   - Add `/reports/boxes` route to `app.routes.ts`

## API Endpoint Documentation

### GET /api/reports/boxes

Returns a paginated list of boxes with KPIs and aggregations.

#### Query Parameters

| Parameter | Type | Required | Default | Description |
|-----------|------|----------|---------|-------------|
| `pageNumber` | int | No | 1 | Page number (1-based) |
| `pageSize` | int | No | 25 | Items per page (max 100) |
| `sortBy` | string | No | "boxtag" | Column to sort by |
| `sortDir` | string | No | "asc" | Sort direction: "asc" or "desc" |
| `projectId` | Guid | No | - | Filter by project ID |
| `boxType` | string[] | No | - | Filter by box types (multi-select) |
| `floor` | string | No | - | Filter by floor |
| `building` | string | No | - | Filter by building |
| `zone` | string | No | - | Filter by zone |
| `status` | int[] | No | - | Filter by status (BoxStatusEnum values: 1-6) |
| `progressMin` | decimal | No | - | Minimum progress percentage (0-100) |
| `progressMax` | decimal | No | - | Maximum progress percentage (0-100) |
| `search` | string | No | - | Search in BoxTag, SerialNumber, BoxName |
| `dateFrom` | DateTime | No | - | Start date for date filter |
| `dateTo` | DateTime | No | - | End date for date filter |
| `dateFilterType` | string | No | "LastUpdate" | Date filter type: "LastUpdate" or "PlannedStart" |

#### Sortable Columns

- `boxtag` (default)
- `serialnumber`
- `project`
- `boxtype`
- `floor`
- `building`
- `zone`
- `progress`
- `status`
- `location`
- `plannedstart`
- `plannedend`
- `lastupdate`

#### Response Structure

```json
{
  "data": {
    "items": [
      {
        "boxId": "guid",
        "projectId": "guid",
        "projectCode": "string",
        "projectName": "string",
        "boxTag": "string",
        "serialNumber": "string",
        "boxName": "string",
        "boxType": "string",
        "floor": "string",
        "building": "string",
        "zone": "string",
        "progressPercentage": 0.00,
        "progressPercentageFormatted": "0.00%",
        "status": "string",
        "currentLocationId": "guid",
        "currentLocationName": "string",
        "plannedStartDate": "datetime",
        "plannedEndDate": "datetime",
        "actualStartDate": "datetime",
        "actualEndDate": "datetime",
        "duration": 0,
        "lastUpdateDate": "datetime",
        "activitiesCount": 0,
        "assetsCount": 0
      }
    ],
    "totalCount": 0,
    "pageNumber": 1,
    "pageSize": 25,
    "totalPages": 0,
    "kpis": {
      "totalBoxes": 0,
      "inProgressCount": 0,
      "completedCount": 0,
      "notStartedCount": 0,
      "averageProgress": 0.00,
      "averageProgressFormatted": "0.00%"
    },
    "aggregations": {
      "statusDistribution": {
        "NotStarted": 0,
        "InProgress": 0,
        "Completed": 0
      },
      "progressRangeDistribution": {
        "0-25": 0,
        "26-50": 0,
        "51-75": 0,
        "76-100": 0
      },
      "topProjects": [
        {
          "projectId": "guid",
          "projectCode": "string",
          "projectName": "string",
          "boxCount": 0
        }
      ]
    }
  }
}
```

#### Example Requests

```
GET /api/reports/boxes?pageNumber=1&pageSize=25&sortBy=progress&sortDir=desc
GET /api/reports/boxes?projectId=guid&status=2&progressMin=50
GET /api/reports/boxes?search=B2-FF&boxType=Bedroom&boxType=Living%20Room
GET /api/reports/boxes?dateFrom=2025-01-01&dateTo=2025-01-31&dateFilterType=LastUpdate
```

## Progress Formatting

Both backend and frontend use consistent progress formatting:

- **Format**: Exactly 2 decimal places + "%" symbol
- **Null handling**: Returns "-" for null/undefined values
- **Rounding**: Uses "AwayFromZero" midpoint rounding (1.855 → 1.86%)
- **Backend**: `ProgressFormatter.FormatProgress(decimal? value)`
- **Frontend**: `formatProgress(value: number | null | undefined)`

## Testing

### Backend Tests (Recommended)

1. Unit tests for `ProgressFormatter` covering:
   - Normal values (1.85185 → "1.85%")
   - Rounding edge cases (1.855 → "1.86%")
   - Null/zero values
   - Boundary values (0, 100)

2. Integration tests for `GET /api/reports/boxes`:
   - Filtering combinations
   - Pagination
   - Sorting
   - KPI calculations
   - Aggregation accuracy

### Frontend Tests (Recommended)

1. Component tests:
   - Filter application
   - Pagination
   - Export functionality
   - Progress formatting

## Next Steps

1. Create the frontend component (`boxes-summary-report.component.ts/html/scss`)
2. Add route `/reports/boxes` to `app.routes.ts`
3. Implement charts using ng2-charts
4. Implement export functionality (CSV/Excel)
5. Add responsive styling
6. Implement accessibility features
7. Write tests

## Notes

- The backend is fully functional and ready for testing
- Progress formatting is consistent across backend and frontend
- The API endpoint follows existing patterns in the codebase
- Server-side filtering/pagination ensures good performance with large datasets
- All aggregations are calculated from filtered data (before pagination)

