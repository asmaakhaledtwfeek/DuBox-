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
 * - Completed projects:
 *   - Can change to OnHold or Closed
 *   - Can change to Archived only if all boxes are dispatched
 * - OnHold projects: 
 *   - If progress >= 100% AND all boxes dispatched, can change to Completed or Archived
 *   - If progress >= 100% but not all boxes dispatched, can change to Completed (but not Archived)
 *   - If progress < 100%, can change to Active or Closed
 * - Closed projects:
 *   - If progress < 100%, can change to OnHold or Active
 *   - If progress >= 100% AND all boxes completed/dispatched, can change to Completed
 * - All other statuses can transition to any non-Completed, non-Archived status
 */
export function getAvailableProjectStatuses(currentStatus: ProjectStatus, progress: number = 0, allBoxesCompletedOrDispatched: boolean = false, allBoxesDispatched: boolean = false): ProjectStatus[] {
  // If project is archived, no status changes are allowed
  if (currentStatus === ProjectStatus.Archived) {
    return [];
  }

  // Special handling for OnHold projects based on progress
  if (currentStatus === ProjectStatus.OnHold) {
    // Closed is always available for OnHold projects, regardless of progress
    const availableStatuses = [ProjectStatus.Closed];
    
    if (progress >= 100) {
      // Progress >= 100%, can change to Completed
      availableStatuses.push(ProjectStatus.Completed);
      
      // Can only change to Archived if all boxes are dispatched
      if (allBoxesDispatched) {
        availableStatuses.push(ProjectStatus.Archived);
      }
    } else {
      // Progress < 100%, can also change to Active
      availableStatuses.push(ProjectStatus.Active);
    }
    
    return availableStatuses;
  }
  
  // Special handling for Completed projects
  if (currentStatus === ProjectStatus.Completed) {
    // Can change to OnHold, Closed, or Archived (if all boxes dispatched)
    const availableStatuses = [ProjectStatus.OnHold, ProjectStatus.Closed];
    
    // Can only change to Archived if all boxes are dispatched
    if (allBoxesDispatched) {
      availableStatuses.push(ProjectStatus.Archived);
    }
    
    return availableStatuses;
  }

  // Special handling for Closed projects
  if (currentStatus === ProjectStatus.Closed) {
    if (progress >= 100 && allBoxesCompletedOrDispatched) {
      // Progress >= 100% AND all boxes completed/dispatched, can change to Completed
      return [ProjectStatus.Completed];
    } else if (progress < 100) {
      // Progress < 100%, can change to OnHold or Active
      return [ProjectStatus.OnHold, ProjectStatus.Active];
    } else {
      // Progress >= 100% but not all boxes completed/dispatched, no status change allowed
      return [];
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

