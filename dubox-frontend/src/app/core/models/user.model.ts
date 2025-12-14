export interface User {
  id: string;
  email: string;
  firstName: string;
  lastName: string;
  department?: string;
  directRoles?: UserRole[];
  groups?: UserGroup[];
  allRoles?: UserRole[]; 
  createdAt?: Date;
  updatedAt?: Date;
}

// Group AMANA Roles
export enum UserRole {
  SystemAdmin = 'SystemAdmin',
  ProjectManager = 'ProjectManager',
  SiteEngineer = 'SiteEngineer',
  Foreman = 'Foreman',
  QCInspector = 'QCInspector',
  ProcurementOfficer = 'ProcurementOfficer',
  HSEOfficer = 'HSEOfficer',
  CostEstimator = 'CostEstimator',
  Viewer = 'Viewer'
}

export interface UserGroup {
  id: string;
  name: string;
  description?: string;
  roles: UserRole[];
}

export interface Permission {
  id: string;
  name: string;
  description: string;
  module: string;
}

export interface LoginRequest {
  email: string;
  password: string;
}

export interface LoginResponse {
  token: string;
  refreshToken: string;
  user: User;
  expiresIn: number;
}

export interface RegisterRequest {
  email: string;
  password: string;
  fullName: string;
  departmentId: string;
}

export interface AuthState {
  user: User | null;
  token: string | null;
  isAuthenticated: boolean;
  loading: boolean;
  error: string | null;
}

// Helper function to check if user has a specific role (direct or inherited)
export function userHasRole(user: User | null, role: UserRole): boolean {
  if (!user || !user.allRoles) return false;
  return user.allRoles.includes(role);
}

// Helper function to check if user has any of the specified roles
export function userHasAnyRole(user: User | null, roles: UserRole[]): boolean {
  if (!user || !user.allRoles) return false;
  return roles.some(role => user.allRoles?.includes(role) ?? false);
}

// Helper function to check if user has all specified roles
export function userHasAllRoles(user: User | null, roles: UserRole[]): boolean {
  if (!user || !user.allRoles) return false;
  return roles.every(role => user.allRoles?.includes(role) ?? false);
}

// Helper function to get user's primary role (highest permission level)
export function getUserPrimaryRole(user: User | null): UserRole | null {
  if (!user || !user.allRoles || !user.allRoles.length) return null;
  
  // Priority order (highest to lowest)
  const rolePriority = [
    UserRole.SystemAdmin,
    UserRole.ProjectManager,
    UserRole.SiteEngineer,
    UserRole.CostEstimator,
    UserRole.QCInspector,
    UserRole.Foreman,
    UserRole.ProcurementOfficer,
    UserRole.HSEOfficer,
    UserRole.Viewer
  ];
  
  for (const role of rolePriority) {
    if (user.allRoles.includes(role)) {
      return role;
    }
  }
  
  return null;
}
