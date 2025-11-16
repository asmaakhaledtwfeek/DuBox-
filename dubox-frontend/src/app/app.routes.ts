import { Routes } from '@angular/router';
import { authGuard } from './core/guards/auth.guard';
import { roleGuard } from './core/guards/role.guard';
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
    canActivate: [authGuard, roleGuard],
    data: { 
      roles: [UserRole.SystemAdmin, UserRole.ProjectManager, UserRole.DesignEngineer] 
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
    canActivate: [authGuard, roleGuard],
    data: { 
      roles: [UserRole.SystemAdmin, UserRole.ProjectManager, UserRole.DesignEngineer, UserRole.SiteEngineer] 
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
    canActivate: [authGuard, roleGuard],
    data: { 
      roles: [UserRole.SystemAdmin, UserRole.ProjectManager, UserRole.DesignEngineer, UserRole.SiteEngineer] 
    },
    loadComponent: () => import('./features/boxes/edit-box/edit-box.component').then(m => m.EditBoxComponent)
  },
  {
    path: 'projects/:projectId/boxes/:boxId/activities/:activityId',
    canActivate: [authGuard],
    loadComponent: () => import('./features/activities/activity-details/activity-details.component').then(m => m.ActivityDetailsComponent)
  },
  {
    path: 'projects/:projectId/boxes/:boxId/activities/:activityId/qa-qc',
    canActivate: [authGuard, roleGuard],
    data: { 
      roles: [
        UserRole.SystemAdmin, 
        UserRole.ProjectManager, 
        UserRole.QCInspector, 
        UserRole.SiteEngineer,
        UserRole.Foreman
      ] 
    },
    loadComponent: () => import('./features/boxes/qa-qc-checklist/qa-qc-checklist.component').then(m => m.QaQcChecklistComponent)
  },
  {
    path: 'admin',
    canActivate: [authGuard, roleGuard],
    data: { 
      roles: [UserRole.SystemAdmin, UserRole.ProjectManager] 
    },
    children: [
      {
        path: '',
        loadComponent: () => import('./features/admin/admin-panel/admin-panel.component').then(m => m.AdminPanelComponent)
      },
      {
        path: 'users',
        canActivate: [roleGuard],
        data: { roles: [UserRole.SystemAdmin] },
        loadComponent: () => import('./features/admin/user-management/user-management.component').then(m => m.UserManagementComponent)
      }
    ]
  },
  {
    path: 'notifications',
    canActivate: [authGuard],
    loadComponent: () => import('./features/notifications/notifications-center/notifications-center.component').then(m => m.NotificationsCenterComponent)
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
