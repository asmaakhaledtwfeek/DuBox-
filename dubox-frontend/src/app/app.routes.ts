import { Routes } from '@angular/router';
import { authGuard } from './core/guards/auth.guard';
import { roleGuard } from './core/guards/role.guard';
import { permissionGuard } from './core/guards/permission.guard';
import { UserRole } from './core/models/user.model';

export const routes: Routes = [
  {
    path: '',
    redirectTo: '/projects',
    pathMatch: 'full'
  },
  {
    path: 'login',
    loadComponent: () => import('./features/auth/login/login.component').then(m => m.LoginComponent)
  },
  {
    path: 'register',
    loadComponent: () => import('./features/auth/register/register.component').then(m => m.RegisterComponent)
  },
  {
    path: 'forgot-password',
    loadComponent: () => import('./features/auth/forgot-password/forgot-password.component').then(m => m.ForgotPasswordComponent)
  },
  {
    path: 'projects',
    canActivate: [authGuard],
    loadComponent: () => import('./features/projects/projects-list/projects-list.component').then(m => m.ProjectsListComponent)
  },
  {
    path: 'projects/create',
    canActivate: [authGuard, permissionGuard],
    data: { 
      permission: { module: 'projects', action: 'create' }
    },
    loadComponent: () => import('./features/projects/create-project/create-project.component').then(m => m.CreateProjectComponent)
  },
  {
    path: 'projects/:id/dashboard',
    canActivate: [authGuard],
    loadComponent: () => import('./features/projects/project-dashboard/project-dashboard.component').then(m => m.ProjectDashboardComponent)
  },
  {
    path: 'projects/:id/boxes',
    canActivate: [authGuard],
    loadComponent: () => import('./features/boxes/boxes-list/boxes-list.component').then(m => m.BoxesListComponent)
  },
  {
    path: 'boxes/create',
    canActivate: [authGuard, permissionGuard],
    data: { 
      permission: { module: 'boxes', action: 'create' }
    },
    loadComponent: () => import('./features/boxes/create-box/create-box.component').then(m => m.CreateBoxComponent)
  },
  {
    path: 'projects/:projectId/boxes/:boxId',
    canActivate: [authGuard],
    loadComponent: () => import('./features/boxes/box-details/box-details.component').then(m => m.BoxDetailsComponent)
  },
  {
    path: 'projects/:projectId/boxes/:boxId/edit',
    canActivate: [authGuard, permissionGuard],
    data: { 
      permission: { module: 'boxes', action: 'edit' }
    },
    loadComponent: () => import('./features/boxes/edit-box/edit-box.component').then(m => m.EditBoxComponent)
  },
  {
    path: 'projects/:projectId/boxes/:boxId/activities/:activityId',
    canActivate: [authGuard],
    loadComponent: () => import('./features/activities/activity-details/activity-details.component').then(m => m.ActivityDetailsComponent)
  },
  {
    path: 'qc',
    canActivate: [authGuard, permissionGuard],
    data: {
      permission: { module: 'quality-issues', action: 'view' }
    },
    loadComponent: () => import('./features/qc/quality-control-dashboard/quality-control-dashboard.component').then(m => m.QualityControlDashboardComponent)
  },
  {
    path: 'projects/:projectId/boxes/:boxId/activities/:activityId/qa-qc',
    canActivate: [authGuard, permissionGuard],
    data: { 
      permission: { module: 'wir', action: 'create' } // Allow access to add checklist items
    },
    loadComponent: () => import('./features/boxes/qa-qc-checklist/qa-qc-checklist.component').then(m => m.QaQcChecklistComponent)
  },
  {
    path: 'quality/projects/:projectId/boxes/:boxId/activities/:activityId/qa-qc',
    canActivate: [authGuard, permissionGuard],
    data: { 
      permission: { module: 'wir', action: 'create' } // Allow access to add checklist items
    },
    loadComponent: () => import('./features/boxes/qa-qc-checklist/qa-qc-checklist.component').then(m => m.QaQcChecklistComponent)
  },
  {
    path: 'projects/:projectId/boxes/:boxId/activities/:activityId/create-wir-checkpoint',
    canActivate: [authGuard, permissionGuard],
    data: { 
      permission: { module: 'wir', action: 'create' }
    },
    loadComponent: () => import('./features/boxes/create-wir-checkpoint/create-wir-checkpoint.component').then(m => m.CreateWIRCheckpointComponent)
  },
  {
    path: 'projects/:projectId/boxes/:boxId/activities/:activityId/wir-checkpoints/:wirId/add-checklist-items',
    canActivate: [authGuard, permissionGuard],
    data: { 
      permission: { module: 'wir', action: 'create' }
    },
    loadComponent: () => import('./features/boxes/add-checklist-items/add-checklist-items.component').then(m => m.AddChecklistItemsComponent)
  },
  {
    path: 'locations',
    canActivate: [authGuard],
    loadComponent: () => import('./features/locations/locations-management/locations-management.component').then(m => m.LocationsManagementComponent)
  },
  {
    path: 'locations/:locationId',
    canActivate: [authGuard],
    loadComponent: () => import('./features/locations/location-details/location-details.component').then(m => m.LocationDetailsComponent)
  },
  {
    path: 'admin',
    canActivate: [authGuard, permissionGuard],
    data: { 
      permission: { module: 'users', action: 'view' }
    },
    children: [
      {
        path: '',
        loadComponent: () => import('./features/admin/admin-panel/admin-panel.component').then(m => m.AdminPanelComponent)
      },
      {
        path: 'users',
        canActivate: [permissionGuard],
        data: { permission: { module: 'users', action: 'view' } },
        loadComponent: () => import('./features/admin/user-management/user-management.component').then(m => m.UserManagementComponent)
      },
      {
        path: 'users/:userId',
        canActivate: [permissionGuard],
        data: { permission: { module: 'users', action: 'view' } },
        loadComponent: () => import('./features/admin/user-management/user-details-page/user-details-page.component').then(m => m.UserDetailsPageComponent)
      },
      {
        path: 'users/:userId/audit-logs',
        canActivate: [permissionGuard],
        data: { permission: { module: 'audit-logs', action: 'view' } },
        loadComponent: () => import('./features/admin/user-management/user-audit-logs/user-audit-logs.component').then(m => m.UserAuditLogsComponent)
      },
      {
        path: 'audit-logs',
        canActivate: [permissionGuard],
        data: { permission: { module: 'audit-logs', action: 'view' } },
        loadComponent: () => import('./features/admin/audit-logs/audit-logs.component').then(m => m.AuditLogsComponent)
      }
    ]
  },
  {
    path: 'materials',
    canActivate: [authGuard],
    loadComponent: () => import('./features/materials/materials-dashboard/materials-dashboard.component').then(m => m.MaterialsDashboardComponent)
  },
  {
    path: 'materials/create',
    canActivate: [authGuard, permissionGuard],
    data: { 
      permission: { module: 'materials', action: 'create' }
    },
    loadComponent: () => import('./features/materials/create-material/create-material.component').then(m => m.CreateMaterialComponent)
  },
  {
    path: 'materials/:id',
    canActivate: [authGuard],
    loadComponent: () => import('./features/materials/material-details/material-details.component').then(m => m.MaterialDetailsComponent)
  },
  {
    path: 'materials/:id/edit',
    canActivate: [authGuard, permissionGuard],
    data: { 
      permission: { module: 'materials', action: 'edit' }
    },
    loadComponent: () => import('./features/materials/edit-material/edit-material.component').then(m => m.EditMaterialComponent)
  },
  {
    path: 'teams',
    canActivate: [authGuard],
    loadComponent: () => import('./features/teams/teams-dashboard/teams-dashboard.component').then(m => m.TeamsDashboardComponent)
  },
  {
    path: 'teams/create',
    canActivate: [authGuard, permissionGuard],
    data: { 
      permission: { module: 'teams', action: 'create' }
    },
    loadComponent: () => import('./features/teams/create-team/create-team.component').then(m => m.CreateTeamComponent)
  },
  {
    path: 'teams/:id/add-members',
    canActivate: [authGuard, permissionGuard],
    data: { 
      permission: { module: 'teams', action: 'manage-members' }
    },
    loadComponent: () => import('./features/teams/add-team-members/add-team-members.component').then(m => m.AddTeamMembersComponent)
  },
  {
    path: 'teams/:id/edit',
    canActivate: [authGuard, permissionGuard],
    data: { 
      permission: { module: 'teams', action: 'edit' }
    },
    loadComponent: () => import('./features/teams/edit-team/edit-team.component').then(m => m.EditTeamComponent)
  },
  {
    path: 'teams/:id',
    canActivate: [authGuard],
    loadComponent: () => import('./features/teams/team-details/team-details.component').then(m => m.TeamDetailsComponent)
  },
  {
    path: 'notifications',
    canActivate: [authGuard],
    loadComponent: () => import('./features/notifications/notifications-center/notifications-center.component').then(m => m.NotificationsCenterComponent)
  },
  {
    path: 'profile',
    canActivate: [authGuard],
    loadComponent: () => import('./features/user-profile/user-profile.component').then(m => m.UserProfileComponent)
  },
  {
    path: 'reports',
    canActivate: [authGuard],
    loadComponent: () => import('./features/reports/reports-dashboard/reports-dashboard.component').then(m => m.ReportsDashboardComponent)
  },
  {
    path: 'reports/projects',
    canActivate: [authGuard],
    loadComponent: () => import('./features/reports/projects-summary-report/projects-summary-report.component').then(m => m.ProjectsSummaryReportComponent)
  },
  {
    path: 'reports/boxes',
    canActivate: [authGuard],
    loadComponent: () => import('./features/reports/boxes-summary-report/boxes-summary-report.component').then(m => m.BoxesSummaryReportComponent)
  },
  {
    path: 'reports/activities',
    canActivate: [authGuard],
    loadComponent: () => import('./features/reports/activities-report/activities-report.component').then(m => m.ActivitiesReportComponent)
  },
  {
    path: 'reports/teams-performance',
    canActivate: [authGuard],
    loadComponent: () => import('./features/reports/teams-performance-report/teams-performance-report.component').then(m => m.TeamsPerformanceReportComponent)
  },
  {
    path: 'unauthorized',
    loadComponent: () => import('./shared/components/unauthorized/unauthorized.component').then(m => m.UnauthorizedComponent)
  },
  {
    path: '**',
    redirectTo: '/projects'
  }
];
