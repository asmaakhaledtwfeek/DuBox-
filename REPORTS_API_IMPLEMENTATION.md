# Reports API Implementation Guide

## 📋 Overview

This document describes the complete backend API implementation for the Reports module and its integration with the Angular frontend.

## 🎯 What Was Implemented

### Backend Components Created

1. **Report DTOs** (`Dubox.Application/DTOs/ReportDto.cs`)
   - `BoxProgressReportDto` - Progress distribution across buildings
   - `TeamProductivityReportDto` - Team performance metrics
   - `ReportSummaryDto` - Dashboard statistics
   - `PhaseReadinessReportDto` - Future implementation
   - `MissingMaterialsReportDto` - Future implementation

2. **Report Queries** (CQRS Pattern with MediatR)
   - `GetBoxProgressReportQuery` - Groups boxes by building and phase
   - `GetTeamProductivityReportQuery` - Calculates team efficiency metrics
   - `GetReportSummaryQuery` - Aggregates overall statistics

3. **Reports Controller** (`Dubox.Api/Controllers/ReportsController.cs`)
   - Three endpoints for report generation
   - Optional project filtering
   - Proper authorization and error handling

### Frontend Updates

1. **Reports Dashboard** - Now loads real summary statistics from API
2. **Box Progress Report** - Already configured to use API with fallback
3. **Team Productivity Report** - Updated to use API with fallback and project filtering

---

## 🔌 API Endpoints

### Base URL
```
https://localhost:7098/api/reports
```

### 1. Get Box Progress Report

**Endpoint:** `GET /api/reports/box-progress`

**Description:** Returns box progress data grouped by building, classified into different phases based on progress percentage.

**Query Parameters:**
- `projectId` (optional) - GUID - Filter by specific project

**Response:**
```json
{
  "data": [
    {
      "building": "KJ158-Building 1 (GF,FF,SF)",
      "projectId": "guid",
      "projectName": "DuBox Assembly Project",
      "nonAssembled": 48,
      "backing": 5,
      "released1stFix": 12,
      "released2ndFix": 18,
      "released3rdFix": 15,
      "total": 98,
      "progressPercentage": 51.02
    }
  ],
  "isSuccess": true,
  "isFailure": false,
  "message": "Success"
}
```

**Phase Classification:**
- **Non-Assembled**: Progress = 0%
- **Backing**: Progress > 0% and < 20%
- **Released 1st Fix**: Progress >= 20% and < 40% (MEP Phase 1)
- **Released 2nd Fix**: Progress >= 40% and < 80% (Finishing)
- **Released 3rd Fix**: Progress >= 80% (MEP Phase 2)

**Example Request:**
```bash
curl -X GET "https://localhost:7098/api/reports/box-progress" \
  -H "Authorization: Bearer YOUR_JWT_TOKEN"
```

**With Project Filter:**
```bash
curl -X GET "https://localhost:7098/api/reports/box-progress?projectId=your-project-guid" \
  -H "Authorization: Bearer YOUR_JWT_TOKEN"
```

---

### 2. Get Team Productivity Report

**Endpoint:** `GET /api/reports/team-productivity`

**Description:** Returns team productivity metrics including completed activities, efficiency, and average completion time.

**Query Parameters:**
- `projectId` (optional) - GUID - Filter by specific project

**Response:**
```json
{
  "data": [
    {
      "teamId": "guid",
      "teamName": "Civil Team A",
      "totalActivities": 45,
      "completedActivities": 38,
      "inProgress": 5,
      "pending": 2,
      "averageCompletionTime": 3.5,
      "efficiency": 84.44
    }
  ],
  "isSuccess": true,
  "isFailure": false,
  "message": "Success"
}
```

**Metrics Explained:**
- **Total Activities**: All activities assigned to the team
- **Completed Activities**: Activities with Status = Completed
- **In Progress**: Activities with Status = InProgress
- **Pending**: Activities with Status = NotStarted
- **Average Completion Time**: Average days to complete activities (calculated from ActualStartDate to ActualEndDate)
- **Efficiency**: Percentage of completed activities (Completed / Total * 100)

**Example Request:**
```bash
curl -X GET "https://localhost:7098/api/reports/team-productivity" \
  -H "Authorization: Bearer YOUR_JWT_TOKEN"
```

---

### 3. Get Report Summary

**Endpoint:** `GET /api/reports/summary`

**Description:** Returns overall statistics for the reports dashboard.

**Query Parameters:**
- `projectId` (optional) - GUID - Filter by specific project

**Response:**
```json
{
  "data": {
    "totalBoxes": 330,
    "averageProgress": 51.25,
    "pendingActivities": 145,
    "activeTeams": 8,
    "totalProjects": 3,
    "completedActivities": 287
  },
  "isSuccess": true,
  "isFailure": false,
  "message": "Success"
}
```

**Example Request:**
```bash
curl -X GET "https://localhost:7098/api/reports/summary" \
  -H "Authorization: Bearer YOUR_JWT_TOKEN"
```

---

## 🖥️ Frontend Integration

### Reports Service Usage

The frontend `ReportsService` (`dubox-frontend/src/app/core/services/reports.service.ts`) provides methods to call these APIs:

```typescript
import { ReportsService } from './core/services/reports.service';

constructor(private reportsService: ReportsService) {}

// Get box progress report
this.reportsService.getBoxProgressReport(projectId).subscribe({
  next: (data) => console.log('Box progress:', data),
  error: (err) => console.error('Error:', err)
});

// Get team productivity report
this.reportsService.getTeamProductivityReport(projectId).subscribe({
  next: (data) => console.log('Team productivity:', data),
  error: (err) => console.error('Error:', err)
});

// Get report summary
this.reportsService.getReportSummary(projectId).subscribe({
  next: (data) => console.log('Summary:', data),
  error: (err) => console.error('Error:', err)
});
```

### Component Updates

1. **Reports Dashboard** (`/reports`)
   - Loads summary statistics on initialization
   - Displays: Total Boxes, Average Progress, Pending Activities, Active Teams

2. **Box Progress Report** (`/reports/box-progress`)
   - Fetches data from API with fallback to mock data
   - Project filter dropdown
   - Excel export functionality
   - Phase distribution charts

3. **Team Productivity Report** (`/reports/team-productivity`)
   - Fetches data from API with fallback to mock data
   - Project filter dropdown
   - Efficiency color coding
   - Excel export functionality

---

## 🚀 Testing the APIs

### Method 1: Using Swagger UI

1. Start your backend: `dotnet run` in `Dubox.Api` directory
2. Navigate to: `https://localhost:7098/swagger`
3. Authorize with JWT token
4. Test each endpoint under "Reports" section

### Method 2: Using Postman

1. **Login first** to get JWT token:
   ```
   POST https://localhost:7098/api/auth/login
   Body: {
     "email": "admin@groupamana.com",
     "password": "AMANA@2024"
   }
   ```

2. **Copy the token** from response

3. **Call Reports API**:
   ```
   GET https://localhost:7098/api/reports/box-progress
   Headers: 
     Authorization: Bearer YOUR_TOKEN
   ```

### Method 3: Using Frontend

1. Start backend: `cd Dubox.Api && dotnet run`
2. Start frontend: `cd dubox-frontend && npm start`
3. Login at: `http://localhost:4200/login`
4. Navigate to: `http://localhost:4200/reports`
5. Check browser console for API calls

---

## 📊 Data Flow

```
Frontend Component
    ↓
Reports Service (Angular)
    ↓
HTTP Request → /api/reports/*
    ↓
Reports Controller (ASP.NET)
    ↓
MediatR → Report Query Handler
    ↓
Database (via DbContext)
    ↓
Aggregate & Transform Data
    ↓
Return Result<ReportDto>
    ↓
Frontend displays data
```

---

## 🔧 Configuration

### Backend Configuration

No additional configuration needed. The reports endpoints use:
- Existing database context (`IDbContext`)
- Existing authentication/authorization
- Standard MediatR pipeline

### Frontend Configuration

The API base URL is configured in:
```typescript
// dubox-frontend/src/app/environments/environment.ts
export const environment = {
  apiUrl: 'https://localhost:7098/api'
};
```

---

## 🎨 Frontend Features

### 1. Automatic Fallback
If the API is unavailable, components automatically load mock data and display a warning in console.

### 2. Project Filtering
Both report components support filtering by project:
```html
<select [(ngModel)]="selectedProject" (change)="onProjectChange()">
  <option value="">All Projects</option>
  <option *ngFor="let project of projects" [value]="project.id">
    {{ project.name }}
  </option>
</select>
```

### 3. Excel Export
Export functionality works with both real and mock data:
```typescript
exportToExcel(): void {
  // Generates Excel file with date-stamped filename
  // Example: Box_Progress_Report_2024-11-27.xlsx
}
```

### 4. Loading States
```html
<div *ngIf="loading" class="loading-state">
  <div class="spinner"></div>
  <p>Loading report data...</p>
</div>
```

### 5. Error Handling
```html
<div *ngIf="error && !loading" class="error-state">
  <p>{{ error }}</p>
  <button (click)="loadReportData()">Retry</button>
</div>
```

---

## 🐛 Troubleshooting

### Issue: "No data returned from API"

**Possible Causes:**
1. No boxes/teams in database
2. All data filtered out by project filter
3. Database connection issue

**Solution:**
- Check if you have seeded data
- Try without project filter (select "All Projects")
- Check backend console for errors

### Issue: "401 Unauthorized"

**Solution:**
- Ensure you're logged in
- Check if JWT token is valid
- Token might be expired (login again)

### Issue: "API endpoint not found (404)"

**Solution:**
- Verify backend is running
- Check API URL in environment.ts
- Ensure ReportsController.cs is compiled

### Issue: "CORS Error"

**Solution:**
Add to `Program.cs`:
```csharp
app.UseCors(builder => 
    builder.AllowAnyOrigin()
           .AllowAnyMethod()
           .AllowAnyHeader());
```

---

## 📈 Performance Considerations

### Backend Optimizations

1. **Lazy Loading**: Only load required data
2. **Efficient Queries**: Use LINQ aggregation at database level
3. **Caching**: Consider adding response caching for summary data

### Frontend Optimizations

1. **Fallback Strategy**: Mock data loads if API fails
2. **Project Filtering**: Reduces data transfer
3. **Loading States**: Better UX during data fetch

---

## 🔮 Future Enhancements

### Planned Features

1. **Phase Readiness Report**
   - Track phase dependencies
   - Identify blocking issues
   - Ready vs Pending boxes per phase

2. **Missing Materials Report**
   - Material shortage tracking
   - Affected boxes count
   - Expected delivery dates

3. **Additional Filters**
   - Date range selection
   - Building filter
   - Team filter

4. **Export Options**
   - PDF export
   - Scheduled reports
   - Email distribution

5. **Real-time Updates**
   - WebSocket integration
   - Live dashboard updates
   - Push notifications

---

## 📝 Testing Checklist

- [ ] Backend compiles without errors
- [ ] Swagger UI shows Reports endpoints
- [ ] Login and get JWT token
- [ ] Call `/api/reports/box-progress` - returns data
- [ ] Call `/api/reports/team-productivity` - returns data
- [ ] Call `/api/reports/summary` - returns data
- [ ] Test with projectId filter
- [ ] Frontend connects to backend
- [ ] Reports Dashboard loads summary
- [ ] Box Progress Report displays data
- [ ] Team Productivity Report displays data
- [ ] Project filters work correctly
- [ ] Excel export works for both reports
- [ ] Error handling works (stop backend and check fallback)

---

## 🎉 Summary

**Backend:**
- ✅ 3 DTOs for report data transfer
- ✅ 3 Query handlers with CQRS pattern
- ✅ 1 Controller with 3 endpoints
- ✅ Optional project filtering
- ✅ Proper error handling
- ✅ Authorization required

**Frontend:**
- ✅ Reports Dashboard with real summary data
- ✅ Box Progress Report with API integration
- ✅ Team Productivity Report with API integration
- ✅ Project filtering on both reports
- ✅ Automatic fallback to mock data
- ✅ Loading and error states
- ✅ Excel export functionality

---

## 📞 Support

For issues or questions:
1. Check this documentation
2. Review `REPORTS_IMPLEMENTATION.md` for frontend details
3. Check console logs for error messages
4. Verify backend is running and accessible

---

**Created:** November 27, 2024  
**Last Updated:** November 27, 2024  
**Version:** 1.0.0

© 2024 Group AMANA - DuBox Platform


