export interface Project {
  id: string;
  name: string;
  code: string;
  location: string;
  clientName?: string;
  description?: string;
  startDate?: Date;
  endDate?: Date;
  plannedStartDate?: Date;
  plannedEndDate?: Date;
  duration?: number;
  status: ProjectStatus;
  totalBoxes: number;
  completedBoxes: number;
  inProgressBoxes: number;
  readyForDeliveryBoxes: number;
  progress: number;
  createdBy?: string;
  updatedBy?: string;
  createdAt?: Date;
  updatedAt?: Date;
}

export enum ProjectStatus {
  Planning = 'Planning',
  Active = 'Active',
  OnHold = 'OnHold',
  Completed = 'Completed',
  Cancelled = 'Cancelled'
}

export interface ProjectStats {
  totalProjects: number;
  activeProjects: number;
  completedProjects: number;
  totalBoxes: number;
}

