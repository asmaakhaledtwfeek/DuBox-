export interface Project {
  id: string;
  name: string;
  code: string;
  location: string;
  clientName?: string;
  description?: string;
  bimLink?: string;
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

/**
 * Get available project status transitions based on current status
 * Business Rules:
 * - Archived projects cannot be changed to any other status (locked)
 * - Completed status is set automatically and cannot be manually selected
 * - OnHold projects: 
 *   - If progress >= 100%, can only change to Completed or Archived
 *   - If progress < 100%, can only change to Active
 * - All other statuses can transition to any non-Completed, non-Archived status
 */
export function getAvailableProjectStatuses(currentStatus: ProjectStatus, progress: number = 0): ProjectStatus[] {
  // If project is archived, no status changes are allowed
  if (currentStatus === ProjectStatus.Archived) {
    return [];
  }

  // Special handling for OnHold projects based on progress
  if (currentStatus === ProjectStatus.OnHold) {
    if (progress >= 100) {
      // Progress >= 100%, can only change to Completed or Archived
      return [ProjectStatus.Completed, ProjectStatus.Archived];
    } else {
      // Progress < 100%, can only change to Active
      return [ProjectStatus.Active];
    }
  }

  // Return all statuses except Completed (auto-set) and current status
  const allStatuses = Object.values(ProjectStatus);
  return allStatuses.filter(status => 
    status !== ProjectStatus.Completed && 
    status !== currentStatus
  );
}

/**
 * Check if a project status can be changed
 */
export function canChangeProjectStatus(currentStatus: ProjectStatus): boolean {
  return currentStatus !== ProjectStatus.Archived;
}

export interface ProjectStats {
  totalProjects: number;
  activeProjects: number;
  completedProjects: number;
  totalBoxes: number;
}

