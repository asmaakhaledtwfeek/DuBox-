export interface User {
  id: string;
  email: string;
  firstName: string;
  lastName: string;
  role: UserRole;
  permissions: Permission[];
  createdAt?: Date;
  updatedAt?: Date;
}

export enum UserRole {
  Admin = 'Admin',
  Factory = 'Factory',
  Site = 'Site',
  Viewer = 'Viewer'
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

export interface AuthState {
  user: User | null;
  token: string | null;
  isAuthenticated: boolean;
  loading: boolean;
  error: string | null;
}

