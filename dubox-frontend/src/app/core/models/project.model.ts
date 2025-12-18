export interface Project {
  id: string;
  name: string;
  code: string;
  location: string;
  clientName?: string;
  description?: string;
  categoryId?: number;
  categoryName?: string;
  startDate?: Date;
  endDate?: Date;
  plannedStartDate?: Date;
  plannedEndDate?: Date;
  actualStartDate?: Date;
  compressionStartDate?: Date;
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
  Active = 'Active',
  OnHold = 'OnHold',
  Completed = 'Completed',
  Archived = 'Archived',
  Closed = 'Closed'
}

export interface ProjectStats {
  totalProjects: number;
  activeProjects: number;
  completedProjects: number;
  totalBoxes: number;
}

