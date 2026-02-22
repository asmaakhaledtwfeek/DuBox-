import { Component, OnInit, OnDestroy, HostListener, ChangeDetectorRef, ChangeDetectionStrategy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../../../environments/environment';
import { HeaderComponent } from '../../../shared/components/header/header.component';
import { SidebarComponent } from '../../../shared/components/sidebar/sidebar.component';
import { TeamService } from '../../../core/services/team.service';
import { WeatherService } from '../../../core/services/weather.service';
import { Team, TeamMember } from '../../../core/models/team.model';
import { ProjectWeatherReport } from '../../../core/models/weather.model';
import { ActivityTreeNodeComponent } from './activity-tree-node.component';

interface ScheduleActivity {
  scheduleActivityId: string;
  activityName: string;
  activityCode: string;
  plannedStartDate: string;
  plannedFinishDate: string;
  actualStartDate?: string;
  actualFinishDate?: string;
  status: string;
  percentComplete: number;
  weight: number;
  teamCount: number;
  materialCount: number;
  parentActivityId?: string;
  children?: ScheduleActivity[];
  // UI state properties
  expanded?: boolean;
  visible?: boolean;
  level?: number;
}

interface CreateActivityRequest {
  activityName: string;
  activityCode: string;
  description?: string;
  plannedStartDate: string;
  plannedFinishDate: string;
  projectId?: string;
}

interface Project {
  projectId: string;
  projectCode: string;
  projectName: string;
  clientName?: string;
  location?: number; // 0 = KSA, 1 = UAE (ProjectLocationEnum)
  status: string;
}

@Component({
  selector: 'app-schedule-dashboard',
  standalone: true,
  imports: [CommonModule, RouterModule, FormsModule, HeaderComponent, SidebarComponent, ActivityTreeNodeComponent],
  templateUrl: './schedule-dashboard.component.html',
  styleUrl: './schedule-dashboard.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class ScheduleDashboardComponent implements OnInit, OnDestroy {
  activities: ScheduleActivity[] = [];
  flattenedActivities: ScheduleActivity[] = []; // Flattened list for display
  totalActivityCount = 0; // Total count including all nested children
  teams: Team[] = [];
  teamMembers: TeamMember[] = [];
  loading = false;
  loadingTeams = false;
  loadingMembers = false;
  error: string | null = null;
  
  // Level filtering
  selectedLevel: number = 0; // 0 means show all levels
  maxLevel: number = 0; // Maximum depth in the tree
  
  // Expand/Collapse all
  expandAllState: boolean = false;
  
  // Trigger for forcing updates
  treeUpdateTrigger: number = 0;
  
  // Activity selection for focused view
  selectedActivityIds: Set<string> = new Set<string>();
  focusedViewActive: boolean = false;
  
  // Project Selection
  projects: Project[] = [];
  selectedProject: Project | null = null;
  projectsLoading = false;
  
  // Weather
  weatherData: ProjectWeatherReport | null = null;
  loadingWeather = false;
  
  // Tree view only - no other views needed
  
  // Create Activity Form
  showCreateForm = false;
  createActivityError = '';
  creatingActivity = false;
  newActivity: CreateActivityRequest = {
    activityName: '',
    activityCode: '',
    description: '',
    plannedStartDate: '',
    plannedFinishDate: '',
    projectId: ''
  };

  // Assign Team Modal
  showAssignTeamModal = false;
  selectedActivityId: string | null = null;
  selectedTeamId: string = '';
  selectedMemberId: string = '';
  assignTeamNotes: string = '';
  assignTeamError: string = '';
  assigningTeam = false;

  // Success Modal
  showSuccessModal = false;
  successMessage = '';

  // Assign Material Modal
  showAssignMaterialModal = false;
  assignMaterialError = '';
  assigningMaterial = false;
  newMaterial = {
    materialName: '',
    materialCode: '',
    quantity: 0,
    unit: '',
    notes: ''
  };

  // Excel Import
  showImportModal = false;
  selectedFile: File | null = null;
  importing = false;
  importError = '';
  importResult: any = null;
  
  // Excel Export
  exporting = false;

  // Edit Progress Modal
  showEditProgressModal = false;
  editingActivity: ScheduleActivity | null = null;
  editProgressError = '';
  updatingProgress = false;
  editProgressData = {
    percentComplete: 0,
    status: 'Planned'
  };

  // Edit Dates Modal
  showEditDatesModal = false;
  editDatesError = '';
  updatingDates = false;
  editDatesData = {
    actualStartDate: '',
    actualFinishDate: ''
  };

  // Performance optimization
  private updateTimeout: any = null;
  private activityParentMap: Map<string, string> = new Map(); // Cache parent relationships
  private ancestorCache: Map<string, Set<string>> = new Map(); // Cache: activityId -> Set of ancestor IDs
  private descendantCache: Map<string, Set<string>> = new Map(); // Cache: activityId -> Set of descendant IDs
  
  // Cached computed values
  cachedTotalActivityCount: number = 0;
  cachedAvailableLevels: number[] = [0];

  // Preserve expanded state when reloading after progress/dates update (so parent doesn't collapse)
  private expandedActivityIdsToRestore: Set<string> | null = null;

  constructor(
    private http: HttpClient,
    private teamService: TeamService,
    private weatherService: WeatherService,
    private cdr: ChangeDetectorRef
  ) {}

  ngOnInit(): void {
    this.loadProjects();
    this.loadTeams();
  }

  ngOnDestroy(): void {
    // Clean up timeout to prevent memory leaks
    if (this.updateTimeout) {
      clearTimeout(this.updateTimeout);
    }
  }

  loadProjects(): void {
    this.projectsLoading = true;
    
    this.http.get<any>(`${environment.apiUrl}/projects`)
      .subscribe({
        next: (response) => {
          this.projects = response.data || response || [];
          this.projectsLoading = false;
        },
        error: (err) => {
          console.error('Error loading projects:', err);
          this.projectsLoading = false;
        }
      });
  }

  onProjectSelect(project: Project | null): void {
    this.selectedProject = project;
    if (this.selectedProject) {
      this.loadActivities();
      this.loadWeatherData();
    } else {
      this.activities = [];
      this.totalActivityCount = 0;
      this.weatherData = null;
    }
  }
  
  loadWeatherData(): void {
    if (!this.selectedProject) {
      return;
    }
    
    this.loadingWeather = true;
    this.weatherService.getProjectWeather(this.selectedProject.projectId).subscribe({
      next: (weather) => {
        this.weatherData = weather;
        this.loadingWeather = false;
      },
      error: (err) => {
        console.error('Error loading weather:', err);
        this.weatherData = null;
        this.loadingWeather = false;
      }
    });
  }
  
  formatTime(dateString?: string): string {
    if (!dateString) return '--:--';
    const date = new Date(dateString);
    return date.toLocaleTimeString('en-US', { hour: '2-digit', minute: '2-digit', hour12: false });
  }
  
  formatCoordinates(lat: number, lon: number): string {
    const latDir = lat >= 0 ? 'N' : 'S';
    const lonDir = lon >= 0 ? 'E' : 'W';
    
    const latDeg = Math.floor(Math.abs(lat));
    const latMin = Math.floor((Math.abs(lat) - latDeg) * 60);
    const latSec = Math.floor(((Math.abs(lat) - latDeg) * 60 - latMin) * 60);
    
    const lonDeg = Math.floor(Math.abs(lon));
    const lonMin = Math.floor((Math.abs(lon) - lonDeg) * 60);
    const lonSec = Math.floor(((Math.abs(lon) - lonDeg) * 60 - lonMin) * 60);
    
    return `${latDeg} ${latMin} ${latSec} ${latDir} ${lonDeg} ${lonMin} ${lonSec} ${lonDir}`;
  }
  
  getCurrentTime(): string {
    const now = new Date();
    const hours = now.getHours().toString().padStart(2, '0');
    const minutes = now.getMinutes().toString().padStart(2, '0');
    // location: 0 = KSA, 1 = UAE
    const location = this.selectedProject?.location === 1 ? 'UAE' : 'KSA';
    return `${hours}:${minutes} ${location}`;
  }

  loadActivities(): void {
    if (!this.selectedProject) {
      this.activities = [];
      this.flattenedActivities = [];
      this.totalActivityCount = 0;
      return;
    }

    this.loading = true;
    this.error = null;

    // Load activities filtered by project
    this.http.get<any>(`${environment.apiUrl}/schedule/activities/project/${this.selectedProject.projectId}`).subscribe({
      next: (response) => {
        // Detach change detection during heavy data processing
        this.cdr.detach();
        
        try {
          const rawActivities = response.data || [];
          
          // Transform to nested tree structure (API already returns hierarchical data)
          // Filter to get only root activities (those without parentActivityId)
          this.activities = this.getRootActivities(rawActivities);
          
          // Build parent-child relationship cache for performance
          this.buildParentMap(this.activities);
          
          // Calculate and cache values
          this.cachedTotalActivityCount = this.countAllActivities(this.activities);
          this.totalActivityCount = this.cachedTotalActivityCount;
          
          // Calculate maximum depth/level
          this.maxLevel = this.calculateMaxLevel(this.activities);
          this.cachedAvailableLevels = this.calculateAvailableLevels();
          
          // Initialize expanded state based on current level selection
          this.initializeExpandedState();

          // Restore expanded state after progress/dates update so parent rows stay expanded
          if (this.expandedActivityIdsToRestore) {
            this.restoreExpandedState(this.expandedActivityIdsToRestore);
            this.expandedActivityIdsToRestore = null;
          }
          
          this.loading = false;
        } finally {
          // Reattach and trigger change detection after processing is complete
          this.cdr.reattach();
          this.cdr.markForCheck();
        }
      },
      error: (err) => {
        this.error = 'Failed to load schedule activities';
        this.loading = false;
        this.totalActivityCount = 0;
        console.error('Error loading activities:', err);
        this.cdr.reattach();
        this.cdr.markForCheck();
      }
    });
  }

  // Filter to get only root activities (no parent)
  private getRootActivities(activities: ScheduleActivity[]): ScheduleActivity[] {
    return activities.filter(activity => !activity.parentActivityId);
  }

  // Build parent-child relationship map and caches for faster lookups
  private buildParentMap(activities: ScheduleActivity[], parentId: string = ''): void {
    // Clear caches before rebuilding
    this.activityParentMap.clear();
    this.ancestorCache.clear();
    this.descendantCache.clear();
    
    // Build maps recursively
    this.buildMapsRecursive(activities, parentId);
  }
  
  private buildMapsRecursive(activities: ScheduleActivity[], parentId: string = ''): void {
    activities.forEach(activity => {
      const activityId = activity.scheduleActivityId;
      
      // Build parent map
      if (parentId) {
        this.activityParentMap.set(activityId, parentId);
      }
      
      // Build ancestor cache (all parents, grandparents, etc.)
      const ancestors = new Set<string>();
      if (parentId) {
        ancestors.add(parentId);
        // Add all ancestors of parent
        const parentAncestors = this.ancestorCache.get(parentId);
        if (parentAncestors) {
          parentAncestors.forEach(ancestor => ancestors.add(ancestor));
        }
      }
      this.ancestorCache.set(activityId, ancestors);
      
      // Build descendant cache (all children, grandchildren, etc.)
      const descendants = new Set<string>();
      if (activity.children && activity.children.length > 0) {
        activity.children.forEach(child => {
          descendants.add(child.scheduleActivityId);
          // Recursively add all descendants of child (will be built in next iteration)
        });
        this.descendantCache.set(activityId, descendants);
        
        // Recurse to children
        this.buildMapsRecursive(activity.children, activityId);
        
        // After processing children, update descendants with grandchildren
        activity.children.forEach(child => {
          const childDescendants = this.descendantCache.get(child.scheduleActivityId);
          if (childDescendants) {
            childDescendants.forEach(desc => descendants.add(desc));
          }
        });
      } else {
        this.descendantCache.set(activityId, descendants);
      }
    });
  }

  // Get total activity count (including nested) for display - now returns cached value
  getTotalActivityCount(): number {
    return this.cachedTotalActivityCount;
  }

  // Helper method to recursively count all activities including nested children
  private countAllActivities(activities: ScheduleActivity[]): number {
    let count = activities.length;
    activities.forEach(activity => {
      if (activity.children && activity.children.length > 0) {
        count += this.countAllActivities(activity.children);
      }
    });
    return count;
  }

  // Calculate maximum depth/level in the tree
  private calculateMaxLevel(activities: ScheduleActivity[], currentLevel: number = 1): number {
    let maxLevel = currentLevel;
    activities.forEach(activity => {
      if (activity.children && activity.children.length > 0) {
        const childMaxLevel = this.calculateMaxLevel(activity.children, currentLevel + 1);
        maxLevel = Math.max(maxLevel, childMaxLevel);
      }
    });
    return maxLevel;
  }

  // Get available levels for filtering - now returns cached value
  getAvailableLevels(): number[] {
    return this.cachedAvailableLevels;
  }
  
  // Calculate available levels once
  private calculateAvailableLevels(): number[] {
    const levels = [0]; // 0 represents "All Levels"
    for (let i = 1; i <= this.maxLevel; i++) {
      levels.push(i);
    }
    return levels;
  }

  // Initialize expanded state when activities are loaded
  private initializeExpandedState(): void {
    console.log('Selected level:', this.selectedLevel);
    if (this.selectedLevel === 0) {
      // "All Levels" - make all visible but DON'T expand all by default
      // This prevents rendering thousands of DOM nodes at once
      this.setAllVisible(this.activities, true);
      
      // Start collapsed - user can expand as needed
      this.expandAllState = false;
      this.setExpandedStateRecursive(this.activities, false);
      
      // Only expand first level for better initial UX
      this.activities.forEach(activity => {
        activity.expanded = true; // Expand root nodes only
      });
      
      this.treeUpdateTrigger = Date.now();
    } else if (this.selectedLevel > 0) {
      // Specific level - expand to that level and set visibility
      this.expandAllState = false;
      this.expandToLevel(this.activities, 1, this.selectedLevel);
      this.treeUpdateTrigger = Date.now();
    }
  }

  // Change level filter
  onLevelChange(level: number): void {
    // Detach change detection during bulk visibility/expansion changes
    this.cdr.detach();
    
    try {
      this.selectedLevel = level;
      
      if (level === 0) {
        // If "All Levels" is selected, reset visibility for ALL nodes
        this.setAllVisible(this.activities, true);
        
        // Don't expand all - start collapsed with only root nodes expanded
        this.expandAllState = false;
        this.setExpandedStateRecursive(this.activities, false);
        
        // Expand only root level for better performance
        this.activities.forEach(activity => {
          activity.expanded = true;
        });
      } else if (level > 0) {
        // Automatically expand to the selected level and set visibility
        this.expandAllState = false;
        this.expandToLevel(this.activities, 1, level);
      }
    } finally {
      // Reattach and schedule update
      this.cdr.reattach();
      this.scheduleTreeUpdate();
    }
  }

  // Expand activities up to a specific level and control visibility
  private expandToLevel(activities: ScheduleActivity[], currentLevel: number, targetLevel: number): void {
    activities.forEach(activity => {
      // Set visibility based on level
      if (currentLevel <= targetLevel) {
        activity.visible = true; // Show activities up to and including target level
      } else {
        activity.visible = false; // Hide activities beyond target level
      }

      // Handle expansion and recurse to children
      if (activity.children && activity.children.length > 0) {
        if (currentLevel < targetLevel) {
          // Expand parents to show children at target level
          activity.expanded = true;
          this.expandToLevel(activity.children, currentLevel + 1, targetLevel);
        } else if (currentLevel === targetLevel) {
          // At target level, collapse but ensure children visibility is properly set
          activity.expanded = false;
          // Recursively hide all children beyond target level
          this.setAllVisible(activity.children, false);
        } else {
          // Beyond target level, collapse and hide
          activity.expanded = false;
          this.setAllVisible(activity.children, false);
        }
      }
    });
  }

  // Expand all activities
  expandAll(): void {
    // Detach change detection during bulk operation
    this.cdr.detach();
    
    try {
      this.expandAllState = true;
      this.setExpandedStateRecursive(this.activities, true);
    } finally {
      // Reattach and schedule update
      this.cdr.reattach();
      this.scheduleTreeUpdate();
    }
  }

  // Collapse all activities
  collapseAll(): void {
    // Detach change detection during bulk operation
    this.cdr.detach();
    
    try {
      this.expandAllState = false;
      this.setExpandedStateRecursive(this.activities, false);
    } finally {
      // Reattach and schedule update
      this.cdr.reattach();
      this.scheduleTreeUpdate();
    }
  }

  // Recursively set expanded state
  private setExpandedStateRecursive(activities: ScheduleActivity[], expanded: boolean): void {
    activities.forEach(activity => {
      activity.expanded = expanded;
      if (activity.children && activity.children.length > 0) {
        this.setExpandedStateRecursive(activity.children, expanded);
      }
    });
  }

  // Collect all activity IDs that are currently expanded (used before reload to preserve state)
  private collectExpandedActivityIds(): Set<string> {
    const ids = new Set<string>();
    const collect = (list: ScheduleActivity[]) => {
      list.forEach(activity => {
        if (activity.expanded) {
          ids.add(activity.scheduleActivityId);
        }
        if (activity.children && activity.children.length > 0) {
          collect(activity.children);
        }
      });
    };
    collect(this.activities);
    return ids;
  }

  // Restore expanded state for given IDs after a reload (e.g. after progress update)
  private restoreExpandedState(ids: Set<string>): void {
    const restore = (list: ScheduleActivity[]) => {
      list.forEach(activity => {
        if (ids.has(activity.scheduleActivityId)) {
          activity.expanded = true;
        }
        if (activity.children && activity.children.length > 0) {
          restore(activity.children);
        }
      });
    };
    restore(this.activities);
    this.treeUpdateTrigger = Date.now();
  }

  // Helper function to recursively set visibility state for all activities in the tree
  private setAllVisible(activities: ScheduleActivity[], state: boolean): void {
    activities.forEach(activity => {
      activity.visible = state;
      if (activity.children && activity.children.length > 0) {
        this.setAllVisible(activity.children, state);
      }
    });
  }

  // Toggle activity selection - Optimized version
  onActivitySelectionChange(activityId: string, selected: boolean): void {
    // Update selection without creating new Set (mutate in place for performance)
    if (selected) {
      this.selectedActivityIds.add(activityId);
    } else {
      this.selectedActivityIds.delete(activityId);
    }
    
    // NOTE: Don't clear ancestor/descendant caches - they're static relationship maps
    // Only selection state changes, not the tree structure
    
    // Activate focused view if any activities are selected
    this.focusedViewActive = this.selectedActivityIds.size > 0;
    
    // If focused view is active, expand selected activities (only the selected one)
    if (this.focusedViewActive && selected) {
      this.expandActivityById(activityId);
    }
    
    // Debounce tree update to batch multiple selections
    this.scheduleTreeUpdate();
  }

  // Debounced tree update for better performance
  private scheduleTreeUpdate(): void {
    if (this.updateTimeout) {
      clearTimeout(this.updateTimeout);
    }
    
    this.updateTimeout = setTimeout(() => {
      // Use requestAnimationFrame for smoother rendering
      requestAnimationFrame(() => {
        this.treeUpdateTrigger = Date.now();
        this.cdr.detectChanges();
      });
    }, 100); // 100ms debounce for better batching
  }

  // Expand only a specific activity by ID (much faster than full tree traversal)
  private expandActivityById(activityId: string): void {
    const expandRecursive = (activities: ScheduleActivity[]): boolean => {
      for (const activity of activities) {
        if (activity.scheduleActivityId === activityId) {
          activity.expanded = true;
          return true;
        }
        if (activity.children && activity.children.length > 0) {
          if (expandRecursive(activity.children)) {
            return true;
          }
        }
      }
      return false;
    };
    expandRecursive(this.activities);
  }

  // Check if an activity is selected
  isActivitySelected(activityId: string): boolean {
    return this.selectedActivityIds.has(activityId);
  }

  // Expand selected activities and their children
  private expandSelectedActivities(): void {
    this.expandActivitiesRecursive(this.activities);
  }

  private expandActivitiesRecursive(activities: ScheduleActivity[]): void {
    activities.forEach(activity => {
      if (this.selectedActivityIds.has(activity.scheduleActivityId)) {
        activity.expanded = true;
      }
      if (activity.children && activity.children.length > 0) {
        this.expandActivitiesRecursive(activity.children);
      }
    });
  }

  // Check if activity should be visible in focused view
  shouldShowActivityInFocusedView(activity: ScheduleActivity): boolean {
    if (!this.focusedViewActive) {
      return true; // Show all when not in focused view
    }
    
    // Show if this activity is selected
    if (this.selectedActivityIds.has(activity.scheduleActivityId)) {
      return true;
    }
    
    // Show if this activity is a child of a selected activity
    if (this.isChildOfSelectedActivity(activity)) {
      return true;
    }
    
    // Show if this activity is a parent of a selected activity
    if (this.isParentOfSelectedActivity(activity)) {
      return true;
    }
    
    return false;
  }

  private isChildOfSelectedActivity(activity: ScheduleActivity): boolean {
    // Use ancestor cache for O(1) lookup
    const ancestors = this.ancestorCache.get(activity.scheduleActivityId);
    if (!ancestors || ancestors.size === 0) {
      return false;
    }
    
    // Check if any ancestor is selected
    for (const ancestorId of ancestors) {
      if (this.selectedActivityIds.has(ancestorId)) {
        return true;
      }
    }
    return false;
  }

  private isParentOfSelectedActivity(activity: ScheduleActivity): boolean {
    // Use descendant cache for O(1) lookup
    const descendants = this.descendantCache.get(activity.scheduleActivityId);
    if (!descendants || descendants.size === 0) {
      return false;
    }
    
    // Check if any descendant is selected
    for (const selectedId of this.selectedActivityIds) {
      if (descendants.has(selectedId)) {
        return true;
      }
    }
    return false;
  }

  // Reset focused view - Optimized version
  resetFocusedView(): void {
    // Clear selection only (keep relationship caches)
    this.selectedActivityIds.clear();
    this.focusedViewActive = false;
    
    // Immediate update without debounce for reset action
    this.treeUpdateTrigger = Date.now();
    this.cdr.detectChanges();
  }

  // Check if an activity is an ancestor (parent/grandparent/etc.) of any selected activity
  // Uses pre-built descendant cache for O(1) lookup
  isActivityAncestorOfSelected(activity: ScheduleActivity): boolean {
    if (!this.focusedViewActive || this.selectedActivityIds.size === 0) {
      return false;
    }
    
    const activityId = activity.scheduleActivityId;
    
    // Get all descendants of this activity from cache
    const descendants = this.descendantCache.get(activityId);
    if (!descendants || descendants.size === 0) {
      return false;
    }
    
    // Check if any descendant is selected (O(n) where n = number of selected activities)
    for (const selectedId of this.selectedActivityIds) {
      if (descendants.has(selectedId)) {
        return true;
      }
    }
    
    return false;
  }

  loadTeams(): void {
    this.loadingTeams = true;
    this.teamService.getTeams().subscribe({
      next: (teams) => {
        this.teams = teams.filter(team => team.isActive);
        this.loadingTeams = false;
      },
      error: (err) => {
        console.error('Error loading teams:', err);
        this.loadingTeams = false;
      }
    });
  }

  loadTeamMembers(teamId: string): void {
    if (!teamId) {
      this.teamMembers = [];
      return;
    }

    this.loadingMembers = true;
    this.teamService.getTeamMembers(teamId).subscribe({
      next: (response) => {
        // Filter to only include members that have a userId
        this.teamMembers = (response.members || []).filter(member => member.userId && member.userId.trim() !== '');
        this.loadingMembers = false;
      },
      error: (err) => {
        console.error('Error loading team members:', err);
        this.teamMembers = [];
        this.loadingMembers = false;
      }
    });
  }

  onTeamChange(): void {
    // Reset member selection when team changes
    this.selectedMemberId = '';
    this.teamMembers = [];
    
    // Load team members if a team is selected
    if (this.selectedTeamId) {
      this.loadTeamMembers(this.selectedTeamId);
    }
  }

  openCreateForm(): void {
    if (!this.selectedProject) {
      return;
    }
    
    this.showCreateForm = true;
    this.createActivityError = '';
    this.newActivity = {
      activityName: '',
      activityCode: '',
      description: '',
      plannedStartDate: '',
      plannedFinishDate: '',
      projectId: this.selectedProject.projectId
    };
  }

  closeCreateForm(): void {
    this.showCreateForm = false;
    this.createActivityError = '';
  }

  createActivity(): void {
    if (!this.newActivity.activityName || !this.newActivity.activityCode || 
        !this.newActivity.plannedStartDate || !this.newActivity.plannedFinishDate) {
      this.createActivityError = 'Please fill in all required fields';
      return;
    }

    this.createActivityError = '';
    this.creatingActivity = true;

    this.http.post<any>(`${environment.apiUrl}/schedule/activities`, this.newActivity).subscribe({
      next: (response) => {
        this.creatingActivity = false;
        this.closeCreateForm();
        this.successMessage = 'Activity created successfully!';
        this.showSuccessModal = true;
        this.loadActivities();
      },
      error: (err) => {
        this.creatingActivity = false;
        const errorMessage = err.error?.error?.message || err.error?.message || err.message || 'Unknown error occurred';
        this.createActivityError = `Failed to create activity: ${errorMessage}`;
        console.error('Error creating activity:', err);
      }
    });
  }

  openAssignTeamModal(activityId: string): void {
    this.selectedActivityId = activityId;
    this.showAssignTeamModal = true;
    this.selectedTeamId = '';
    this.selectedMemberId = '';
    this.teamMembers = [];
    this.assignTeamNotes = '';
    this.assignTeamError = '';
    
    // Load teams if not already loaded
    if (this.teams.length === 0) {
      this.loadTeams();
    }
  }

  closeAssignTeamModal(): void {
    this.showAssignTeamModal = false;
    this.selectedActivityId = null;
    this.assignTeamError = '';
  }

  closeSuccessModal(): void {
    this.showSuccessModal = false;
    this.successMessage = '';
  }

  assignTeam(): void {
    if (!this.selectedTeamId) {
      this.assignTeamError = 'Please select a team';
      return;
    }

    this.assignTeamError = '';
    this.assigningTeam = true;

    const request = {
      teamId: this.selectedTeamId,
      memberId: this.selectedMemberId || null,
      notes: this.assignTeamNotes
    };

    this.http.post<any>(
      `${environment.apiUrl}/schedule/activities/${this.selectedActivityId}/assign-team`,
      request
    ).subscribe({
      next: (response) => {
        this.assigningTeam = false;
        this.closeAssignTeamModal();
        this.successMessage = 'Team assigned successfully!';
        this.showSuccessModal = true;
        this.loadActivities();
      },
      error: (err) => {
        this.assigningTeam = false;
        const errorMessage = err.error?.error?.message || err.error?.message || err.message || 'Unknown error occurred';
        this.assignTeamError = `Failed to assign team: ${errorMessage}`;
        console.error('Error assigning team:', err);
      }
    });
  }

  openAssignMaterialModal(activityId: string): void {
    this.selectedActivityId = activityId;
    this.showAssignMaterialModal = true;
    this.assignMaterialError = '';
    this.newMaterial = {
      materialName: '',
      materialCode: '',
      quantity: 0,
      unit: '',
      notes: ''
    };
  }

  closeAssignMaterialModal(): void {
    this.showAssignMaterialModal = false;
    this.selectedActivityId = null;
    this.assignMaterialError = '';
  }

  assignMaterial(): void {
    if (!this.newMaterial.materialName || this.newMaterial.quantity <= 0) {
      this.assignMaterialError = 'Please fill in material name and quantity';
      return;
    }

    this.assignMaterialError = '';
    this.assigningMaterial = true;

    this.http.post<any>(
      `${environment.apiUrl}/schedule/activities/${this.selectedActivityId}/assign-material`,
      this.newMaterial
    ).subscribe({
      next: (response) => {
        this.assigningMaterial = false;
        this.closeAssignMaterialModal();
        this.successMessage = 'Material assigned successfully!';
        this.showSuccessModal = true;
        this.loadActivities();
      },
      error: (err) => {
        this.assigningMaterial = false;
        const errorMessage = err.error?.error?.message || err.error?.message || err.message || 'Unknown error occurred';
        this.assignMaterialError = `Failed to assign material: ${errorMessage}`;
        console.error('Error assigning material:', err);
      }
    });
  }

  getStatusClass(status: string): string {
    switch (status.toLowerCase()) {
      case 'completed':
        return 'status-completed';
      case 'in progress':
        return 'status-in-progress';
      case 'on hold':
        return 'status-on-hold';
      default:
        return 'status-planned';
    }
  }

  // TrackBy function for better *ngFor performance
  trackByActivityId(index: number, activity: ScheduleActivity): string {
    return activity.scheduleActivityId;
  }

  @HostListener('document:click', ['$event'])
  onDocumentClick(event: Event): void {
    // Event handler for future use
  }

  // Excel Import/Export Methods
  openImportModal(): void {
    if (!this.selectedProject) {
      return;
    }
    this.showImportModal = true;
    this.selectedFile = null;
    this.importError = '';
    this.importResult = null;
  }

  exportToExcel(): void {
    if (!this.selectedProject) {
      return;
    }

    this.exporting = true;
    
    this.http.get(
      `${environment.apiUrl}/schedule/activities/export/${this.selectedProject.projectId}`,
      { responseType: 'blob' }
    ).subscribe({
      next: (blob) => {
        // Create download link
        const url = window.URL.createObjectURL(blob);
        const link = document.createElement('a');
        link.href = url;
        
        // Generate filename with project code and timestamp
        const timestamp = new Date().toISOString().replace(/[:.]/g, '-').slice(0, -5);
        const projectCode = this.selectedProject?.projectCode || 'Project';
        link.download = `${projectCode}_Schedule_${timestamp}.xlsx`;
        
        // Trigger download
        link.click();
        
        // Cleanup
        window.URL.revokeObjectURL(url);
        
        this.exporting = false;
        this.successMessage = 'Schedule exported successfully!';
        this.showSuccessModal = true;
      },
      error: (err) => {
        this.exporting = false;
        console.error('Error exporting to Excel:', err);
        // Show error in a more user-friendly way
        this.successMessage = 'Failed to export schedule. Please try again.';
        this.showSuccessModal = true;
      }
    });
  }

  closeImportModal(): void {
    this.showImportModal = false;
    this.selectedFile = null;
    this.importError = '';
    this.importResult = null;
  }

  onFileSelected(event: any): void {
    const file = event.target.files[0];
    if (file) {
      // Validate file type
      const validTypes = [
        'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet',  // .xlsx
        'application/vnd.ms-excel'  // .xls
      ];
      
      if (!validTypes.includes(file.type) && !file.name.endsWith('.xlsx') && !file.name.endsWith('.xls')) {
        this.importError = 'Please select a valid Excel file (.xlsx or .xls)';
        this.selectedFile = null;
        event.target.value = '';
        return;
      }

      // Validate file size (100MB limit)
      const maxSize = 100 * 1024 * 1024; // 100MB
      if (file.size > maxSize) {
        this.importError = 'File size exceeds 100MB limit';
        this.selectedFile = null;
        event.target.value = '';
        return;
      }

      this.selectedFile = file;
      this.importError = '';
    }
  }

  importExcelFile(): void {
    if (!this.selectedFile || !this.selectedProject) {
      this.importError = 'Please select a file';
      return;
    }

    this.importing = true;
    this.importError = '';
    this.importResult = null;

    const formData = new FormData();
    formData.append('excelFile', this.selectedFile);
    formData.append('projectId', this.selectedProject.projectId);
    // Optional: formData.append('sheetName', 'Sheet1'); // Specify sheet if needed

    this.http.post<any>(`${environment.apiUrl}/schedule/activities/import`, formData)
      .subscribe({
        next: (response) => {
          this.importing = false;
          
          if (response.success) {
            this.importResult = response.data;
            this.successMessage = response.message || 'Excel file imported successfully!';
            
            // Show detailed results if available
            if (response.data) {
              const msg = `Successfully imported ${response.data.successfullyImported} out of ${response.data.totalProcessed} activities`;
              if (response.data.skipped > 0) {
                this.importResult.message = msg + ` (${response.data.skipped} skipped)`;
              } else {
                this.importResult.message = msg;
              }
            }
            
            // Close import modal and show success
            this.closeImportModal();
            this.showSuccessModal = true;
            
            // Reload activities to show imported data
            setTimeout(() => {
              this.loadActivities();
            }, 1000);
          } else {
            this.importError = response.message || 'Import failed';
            if (response.errors && response.errors.length > 0) {
              this.importError += '\n' + response.errors.join('\n');
            }
          }
        },
        error: (err) => {
          this.importing = false;
          console.error('Import error:', err);
          
          let errorMessage = 'Failed to import Excel file. ';
          
          if (err.error?.message) {
            errorMessage += err.error.message;
          } else if (err.error?.errors && err.error.errors.length > 0) {
            errorMessage += err.error.errors.join(', ');
          } else if (err.message) {
            errorMessage += err.message;
          } else {
            errorMessage += 'Please check the file format and try again.';
          }
          
          this.importError = errorMessage;
        }
      });
  }

  // Helper method to convert wind direction degrees to text
  getWindDirectionText(degrees: number | null | undefined): string {
    if (degrees === null || degrees === undefined) {
      return 'N/A';
    }
    
    const directions = ['N', 'NNE', 'NE', 'ENE', 'E', 'ESE', 'SE', 'SSE', 'S', 'SSW', 'SW', 'WSW', 'W', 'WNW', 'NW', 'NNW'];
    const index = Math.round(degrees / 22.5) % 16;
    return directions[index];
  }

  // Edit Progress Methods
  openEditProgressModal(activity: ScheduleActivity): void {
    this.editingActivity = activity;
    this.showEditProgressModal = true;
    this.editProgressError = '';
    this.editProgressData = {
      percentComplete: activity.percentComplete || 0,
      status: activity.status || 'Planned'
    };
  }

  // Get auto-determined status based on progress
  getAutoStatus(percentComplete: number): string {
    if (percentComplete === 100) {
      return 'Completed ✓';
    } else if (percentComplete > 0) {
      return 'In Progress ⟳';
    } else {
      return 'Planned ○';
    }
  }

  closeEditProgressModal(): void {
    this.showEditProgressModal = false;
    this.editingActivity = null;
    this.editProgressError = '';
  }

  updateProgress(): void {
    if (!this.editingActivity) {
      return;
    }

    // Validation
    if (this.editProgressData.percentComplete < 0 || this.editProgressData.percentComplete > 100) {
      this.editProgressError = 'Progress must be between 0 and 100';
      return;
    }

    this.editProgressError = '';
    this.updatingProgress = true;

    // Prepare update request with auto-date and auto-status logic
    const updateRequest: any = {
      percentComplete: this.editProgressData.percentComplete,
      status: this.editProgressData.status
    };

    // Auto-set actual start date when progress > 0 and no actual start date exists
    if (this.editProgressData.percentComplete > 0 && !this.editingActivity.actualStartDate) {
      updateRequest.actualStartDate = new Date().toISOString();
    }

    // Auto-set status based on progress
    if (this.editProgressData.percentComplete === 100) {
      // 100% = Completed
      updateRequest.status = 'Completed';
      // Auto-set actual finish date if not already set
      if (!this.editingActivity.actualFinishDate) {
        updateRequest.actualFinishDate = new Date().toISOString();
      }
    } else if (this.editProgressData.percentComplete > 0 && this.editProgressData.percentComplete < 100) {
      // 1-99% = In Progress
      updateRequest.status = 'In Progress';
    }
    // 0% = Keep current status

    this.http.put<any>(
      `${environment.apiUrl}/schedule/activities/${this.editingActivity.scheduleActivityId}/progress`,
      updateRequest
    ).subscribe({
      next: (response) => {
        this.updatingProgress = false;
        this.closeEditProgressModal();
        this.successMessage = 'Progress updated successfully!';
        this.showSuccessModal = true;
        // Preserve expanded state so parent rows don't collapse after reload
        this.expandedActivityIdsToRestore = this.collectExpandedActivityIds();
        this.loadActivities(); // Reload to show updated data
      },
      error: (err) => {
        this.updatingProgress = false;
        const errorMessage = err.error?.error?.message || err.error?.message || err.message || 'Unknown error occurred';
        this.editProgressError = `Failed to update progress: ${errorMessage}`;
        console.error('Error updating progress:', err);
      }
    });
  }

  // Edit Dates Methods
  openEditDatesModal(activity: ScheduleActivity): void {
    this.editingActivity = activity;
    this.showEditDatesModal = true;
    this.editDatesError = '';
    
    // Convert dates to datetime-local format
    this.editDatesData = {
      actualStartDate: activity.actualStartDate ? this.toDateTimeLocal(activity.actualStartDate) : '',
      actualFinishDate: activity.actualFinishDate ? this.toDateTimeLocal(activity.actualFinishDate) : ''
    };
  }

  closeEditDatesModal(): void {
    this.showEditDatesModal = false;
    this.editingActivity = null;
    this.editDatesError = '';
  }

  // Convert ISO string to datetime-local format
  toDateTimeLocal(dateString: string): string {
    const date = new Date(dateString);
    const year = date.getFullYear();
    const month = String(date.getMonth() + 1).padStart(2, '0');
    const day = String(date.getDate()).padStart(2, '0');
    const hours = String(date.getHours()).padStart(2, '0');
    const minutes = String(date.getMinutes()).padStart(2, '0');
    return `${year}-${month}-${day}T${hours}:${minutes}`;
  }

  // Calculate progress based on actual dates
  getCalculatedProgress(): number {
    if (!this.editDatesData.actualStartDate && !this.editDatesData.actualFinishDate) {
      return 0; // No dates = 0%
    } else if (this.editDatesData.actualStartDate && this.editDatesData.actualFinishDate) {
      return 100; // Both dates = 100%
    } else if (this.editDatesData.actualStartDate) {
      return 50; // Only start date = 50%
    }
    return 0;
  }

  // Calculate status based on actual dates
  getCalculatedStatus(): string {
    if (!this.editDatesData.actualStartDate && !this.editDatesData.actualFinishDate) {
      return 'Planned ○';
    } else if (this.editDatesData.actualStartDate && this.editDatesData.actualFinishDate) {
      return 'Completed ✓';
    } else if (this.editDatesData.actualStartDate) {
      return 'In Progress ⟳';
    }
    return 'Planned ○';
  }

  updateDates(): void {
    if (!this.editingActivity) {
      return;
    }

    // Validation
    if (this.editDatesData.actualStartDate && this.editDatesData.actualFinishDate) {
      const startDate = new Date(this.editDatesData.actualStartDate);
      const finishDate = new Date(this.editDatesData.actualFinishDate);
      
      if (finishDate < startDate) {
        this.editDatesError = 'Finish date cannot be before start date';
        return;
      }
    }

    this.editDatesError = '';
    this.updatingDates = true;

    // Calculate progress and status based on dates
    const calculatedProgress = this.getCalculatedProgress();
    let calculatedStatus = 'Planned';
    
    if (calculatedProgress === 100) {
      calculatedStatus = 'Completed';
    } else if (calculatedProgress > 0) {
      calculatedStatus = 'In Progress';
    }

    // Prepare update request
    const updateRequest: any = {
      percentComplete: calculatedProgress,
      status: calculatedStatus,
      actualStartDate: this.editDatesData.actualStartDate ? new Date(this.editDatesData.actualStartDate).toISOString() : null,
      actualFinishDate: this.editDatesData.actualFinishDate ? new Date(this.editDatesData.actualFinishDate).toISOString() : null
    };

    this.http.put<any>(
      `${environment.apiUrl}/schedule/activities/${this.editingActivity.scheduleActivityId}/progress`,
      updateRequest
    ).subscribe({
      next: (response) => {
        this.updatingDates = false;
        this.closeEditDatesModal();
        this.successMessage = 'Actual dates updated successfully!';
        this.showSuccessModal = true;
        // Preserve expanded state so parent rows don't collapse after reload
        this.expandedActivityIdsToRestore = this.collectExpandedActivityIds();
        this.loadActivities(); // Reload to show updated data
      },
      error: (err) => {
        this.updatingDates = false;
        const errorMessage = err.error?.error?.message || err.error?.message || err.message || 'Unknown error occurred';
        this.editDatesError = `Failed to update dates: ${errorMessage}`;
        console.error('Error updating dates:', err);
      }
    });
  }
}

