import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../../../environments/environment';
import { HeaderComponent } from '../../../shared/components/header/header.component';
import { SidebarComponent } from '../../../shared/components/sidebar/sidebar.component';

// Interface matching backend CostCodeListDto
interface CostCode {
  costCodeId: string;
  code: string;                          // Cost Code (Main identifier)
  costCodeLevel1?: string | null;        // Cost Code Level 1
  level1Description?: string | null;     // Level 1 Description
  costCodeLevel2?: string | null;        // Cost Code Level 2
  level2Description?: string | null;     // Level 2 Description
  costCodeLevel3?: string | null;        // Cost Code Level 3
  description: string;                   // Level 3 Description – as per CSI
  level3DescriptionAbbrev?: string | null; // Level 3 Description – abbreviation
  level3DescriptionAmana?: string | null;  // Level 3 Description – AMANA
  category?: string | null;
  unitOfMeasure?: string | null;
  unitRate?: number | null;
  currency: string;
  isActive: boolean;
}

// Interface matching backend HRCostDto
interface HRCost {
  hrCostRecordId: string;
  code?: string | null;
  chapter?: string | null;
  subChapter?: string | null;
  classification?: string | null;
  subClassification?: string | null;
  name: string;
  units?: string | null;
  type?: string | null;
  budgetLevel?: string | null;
  status?: string | null;
  job?: string | null;
  officeAccount?: string | null;
  jobCostAccount?: string | null;
  specialAccount?: string | null;
  idlAccount?: string | null;
}

interface HRCostFilterOptions {
  codes: string[];
  chapters: string[];
  subChapters: string[];
  classifications: string[];
  subClassifications: string[];
  units: string[];
  types: string[];
  statuses: string[];
}

interface Project {
  projectId: string;
  projectCode: string;
  projectName: string;
  clientName?: string;
  location?: string;
  status: string;
}

interface ProjectCost {
  projectCostId: string;
  boxId?: string;
  costCodeId?: string;
  hrCostRecordId?: string;
  cost: number;
  costType: string;
  // Cost Code Master fields
  costCodeLevel1?: string;
  costCodeLevel2?: string;
  costCodeLevel3?: string;
  // HRC Code fields
  chapter?: string;
  subChapter?: string;
  classification?: string;
  subClassification?: string;
  units?: string;
  type?: string;
  createdDate: string;
  createdBy?: string;
  boxTag?: string;
  hrCostCode?: string;
  hrCostName?: string;
}

interface Box {
  boxId: string;
  boxTag: string;
  boxName?: string;
}

interface NewCostForm {
  boxId: string | null;
  // Cost Code Master fields
  costCodeLevel1: string | null;
  costCodeLevel2: string | null;
  costCodeLevel3: string | null;
  // HRC Code fields
  chapter: string | null;
  subChapter: string | null;
  classification: string | null;
  subClassification: string | null;
  units: string | null;
  type: string | null;
  // Cost amount
  cost: number | null;
}

interface CostCodeFilterOptions {
  level1Options: string[];
  level2Options: string[];
  level3Options: string[];
}

interface NewCostCodeForm {
  code: string;
  costCodeLevel1: string | null;
  level1Description: string | null;
  costCodeLevel2: string | null;
  level2Description: string | null;
  costCodeLevel3: string | null;
  description: string;
  level3DescriptionAbbrev: string | null;
  level3DescriptionAmana: string | null;
  category: string | null;
  unitOfMeasure: string | null;
  unitRate: number | null;
  currency: string;
  isActive: boolean;
}

interface NewHRCostForm {
  code: string | null;
  chapter: string | null;
  subChapter: string | null;
  classification: string | null;
  subClassification: string | null;
  name: string;
  units: string | null;
  type: string | null;
  budgetLevel: string | null;
  status: string | null;
  job: string | null;
  officeAccount: string | null;
  jobCostAccount: string | null;
  specialAccount: string | null;
  idlAccount: string | null;
}

@Component({
  selector: 'app-cost-dashboard',
  standalone: true,
  imports: [CommonModule, RouterModule, FormsModule, HeaderComponent, SidebarComponent],
  templateUrl: './cost-dashboard.component.html',
  styleUrl: './cost-dashboard.component.scss'
})
export class CostDashboardComponent implements OnInit {
  activeTab: 'materials' | 'hr' | 'projectCost' = 'materials';
  
  // Material Costs
  costCodes: CostCode[] = [];
  costCodesTotalCount = 0;
  loading = false;
  error: string | null = null;
  searchCode = '';
  selectedCostLevel1: string | null = null;
  selectedCostLevel2: string | null = null;
  selectedCostLevel3: string | null = null;
  searchLevel1Description = '';
  searchLevel2Description = '';
  searchLevel3Description = '';
  selectedIsActive: boolean | null = null;
  
  // Dropdown options from database
  costLevel1Options: string[] = [];
  costLevel2Options: string[] = [];
  costLevel3Options: string[] = [];
  loadingCostLevel1Options = false;
  loadingCostLevel2Options = false;
  loadingCostLevel3Options = false;
  loadingCostCodeFilterOptions = false; // Prevent duplicate requests
  
  // HR Costs
  hrCosts: HRCost[] = [];
  hrCostsTotalCount = 0;
  hrLoading = false;
  hrError: string | null = null;
  
  // HR Cost Filters - Hierarchy: Type → Chapter → Sub Chapter → Classification → Sub Classification → Units
  hrFilterType: string | null = null;
  hrFilterChapter: string | null = null;
  hrFilterSubChapter: string | null = null;
  hrFilterClassification: string | null = null;
  hrFilterSubClassification: string | null = null;
  hrFilterUnits: string | null = null;
  hrFilterStatus: string | null = 'Active'; // Default to Active
  hrSearchName = '';
  
  // Hardcoded status options (always available) - matching database values
  hrStatusOptions: string[] = ['Active', 'InActive'];
  
  // Filter Options (for dropdowns)
  hrFilterOptions: HRCostFilterOptions = {
    codes: [],
    chapters: [],
    subChapters: [],
    classifications: [],
    subClassifications: [],
    units: [],
    types: [],
    statuses: []
  };
  
  // Project Costs
  projects: Project[] = [];
  selectedProject: Project | null = null;
  projectCosts: ProjectCost[] = [];
  projectCostsLoading = false;
  projectCostsError: string | null = null;
  projectsLoading = false;
  showCreateCostModal = false;
  
  // Create Cost Modal
  projectBoxes: Box[] = [];
  loadingBoxes = false;
  costCodeFilterOptions: CostCodeFilterOptions = {
    level1Options: [],
    level2Options: [],
    level3Options: []
  };
  newCost: NewCostForm = {
    boxId: null,
    costCodeLevel1: null,
    costCodeLevel2: null,
    costCodeLevel3: null,
    chapter: null,
    subChapter: null,
    classification: null,
    subClassification: null,
    units: null,
    type: null,
    cost: null
  };
  creatingCost = false;
  createCostError: string | null = null;
  createCostSuccess = false;
  
  // Create Cost Code Modal
  showCreateCostCodeModal = false;
  newCostCode: NewCostCodeForm = {
    code: '',
    costCodeLevel1: null,
    level1Description: null,
    costCodeLevel2: null,
    level2Description: null,
    costCodeLevel3: null,
    description: '',
    level3DescriptionAbbrev: null,
    level3DescriptionAmana: null,
    category: null,
    unitOfMeasure: null,
    unitRate: null,
    currency: 'SAR',
    isActive: true
  };
  creatingCostCode = false;
  createCostCodeError: string | null = null;
  createCostCodeSuccess = false;
  
  // Dropdown options for Create Cost Code Modal
  modalCostLevel1Options: string[] = [];
  modalCostLevel2Options: string[] = [];
  modalLevel1DescriptionOptions: string[] = [];
  modalLevel2DescriptionOptions: string[] = [];
  loadingModalCostLevel1Options = false;
  loadingModalCostLevel2Options = false;
  loadingModalLevel1DescOptions = false;
  loadingModalLevel2DescOptions = false;
  
  // Create HRC Cost Modal
  showCreateHRCostModal = false;
  newHRCost: NewHRCostForm = {
    code: null,
    chapter: null,
    subChapter: null,
    classification: null,
    subClassification: null,
    name: '',
    units: null,
    type: null,
    budgetLevel: null,
    status: 'Active',
    job: null,
    officeAccount: null,
    jobCostAccount: null,
    specialAccount: null,
    idlAccount: null
  };
  creatingHRCost = false;
  createHRCostError: string | null = null;
  createHRCostSuccess = false;
  
  // Pagination - Start with small page size for better performance
  currentPage = 1;
  pageSize = 10; // Reduced from 50 to 10 for faster initial load
  totalRecords = 0;

  constructor(private http: HttpClient) {
  }

  ngOnInit(): void {
    // Only load materials tab initially - true lazy loading
    this.loadCostCodes();
    // Don't load filter options until user interacts with dropdowns
    // Don't load other tabs until user switches to them
  }

  switchTab(tab: 'materials' | 'hr' | 'projectCost'): void {
    this.activeTab = tab;
    
    // Lazy load data only when switching to that tab
    if (tab === 'hr' && this.hrCosts.length === 0) {
      console.log('⏳ Loading HR costs tab for first time');
      this.loadHRCosts();
      // Load Types (first filter in hierarchy) when switching to HR tab
      this.loadHRFilterOptions();
    }
    
    if (tab === 'projectCost' && this.projects.length === 0) {
      console.log('⏳ Loading projects for first time');
      this.loadProjects();
    }
    
    if (tab === 'materials' && this.costCodes.length === 0) {
      console.log('⏳ Loading materials tab for first time');
      this.loadCostCodes();
    }
  }

  loadCostCodes(): void {
    // Prevent duplicate requests
    if (this.loading) {
      console.log('⏳ Cost codes already loading, skipping duplicate request');
      return;
    }
    
    this.loading = true;
    this.error = null;

    const params: any = {
      pageNumber: this.currentPage,
      pageSize: this.pageSize
    };

    // Add search filters based on new backend parameters
    if (this.searchCode) {
      params.code = this.searchCode.trim();
    }

    if (this.selectedCostLevel1) {
      params.costCodeLevel1 = this.selectedCostLevel1;
    }

    if (this.selectedCostLevel2) {
      params.costCodeLevel2 = this.selectedCostLevel2;
    }

    if (this.selectedCostLevel3) {
      params.costCodeLevel3 = this.selectedCostLevel3;
    }

    if (this.searchLevel1Description) {
      params.level1Description = this.searchLevel1Description.trim();
    }

    if (this.searchLevel2Description) {
      params.level2Description = this.searchLevel2Description.trim();
    }

    if (this.searchLevel3Description) {
      params.level3Description = this.searchLevel3Description.trim();
    }

    if (this.selectedIsActive !== null) {
      params.isActive = this.selectedIsActive;
    }

    // Debug logging
    console.log('🔍 Loading Cost Codes with params:', params);

    this.http.get<any>(`${environment.apiUrl}/cost/codes`, { params })
      .subscribe({
        next: (response) => {
          const responseData = response.data || response;
          this.costCodes = responseData?.data || responseData || [];
          this.costCodesTotalCount = responseData?.totalCount || this.costCodes.length;
          this.loading = false;
        },
        error: (err) => {
          this.error = 'Failed to load cost codes master. Please try again.';
          this.loading = false;
          console.error('Error loading cost codes:', err);
        }
      });
  }

  onSearch(): void {
    console.log('🔎 Search triggered - filters:', {
      code: this.searchCode,
      level1: this.selectedCostLevel1,
      level2: this.selectedCostLevel2,
      level3: this.selectedCostLevel3,
      level1Desc: this.searchLevel1Description,
      level2Desc: this.searchLevel2Description,
      level3Desc: this.searchLevel3Description
    });
    this.currentPage = 1;
    this.loadCostCodes();
  }

  clearFilters(): void {
    this.searchCode = '';
    this.selectedCostLevel1 = null;
    this.selectedCostLevel2 = null;
    this.selectedCostLevel3 = null;
    this.searchLevel1Description = '';
    this.searchLevel2Description = '';
    this.searchLevel3Description = '';
    this.selectedIsActive = null;
    this.currentPage = 1;
    this.loadCostCodes();
  }

  hasActiveFilters(): boolean {
    return !!(
      this.searchCode || 
      this.selectedCostLevel1 || 
      this.selectedCostLevel2 || 
      this.selectedCostLevel3 || 
      this.searchLevel1Description || 
      this.searchLevel2Description || 
      this.searchLevel3Description || 
      this.selectedIsActive !== null
    );
  }

  previousPage(): void {
    if (this.currentPage > 1) {
      this.currentPage--;
      if (this.activeTab === 'materials') {
        this.loadCostCodes();
      } else if (this.activeTab === 'hr') {
        this.loadHRCosts();
      } else if (this.activeTab === 'projectCost') {
        this.loadProjectCosts();
      }
    }
  }

  nextPage(): void {
    this.currentPage++;
    if (this.activeTab === 'materials') {
      this.loadCostCodes();
    } else if (this.activeTab === 'hr') {
      this.loadHRCosts();
    } else if (this.activeTab === 'projectCost') {
      this.loadProjectCosts();
    }
  }

  onPageSizeChange(): void {
    console.log('📄 Page size changed to:', this.pageSize);
    this.currentPage = 1; // Reset to first page
    
    // Reload current tab data with new page size
    if (this.activeTab === 'materials') {
      this.loadCostCodes();
    } else if (this.activeTab === 'hr') {
      this.loadHRCosts();
    } else if (this.activeTab === 'projectCost') {
      this.loadProjectCosts();
    }
  }

  // HR Cost Methods
  loadHRCosts(): void {
    // Prevent duplicate requests
    if (this.hrLoading) {
      console.log('⏳ HR costs already loading, skipping duplicate request');
      return;
    }
    
    this.hrLoading = true;
    this.hrError = null;

    const params: any = {
      pageNumber: this.currentPage,
      pageSize: this.pageSize
    };

    // Add filters - hierarchy: Type → Chapter → Sub Chapter → Classification → Sub Classification → Units
    if (this.hrFilterType) params.type = this.hrFilterType;
    if (this.hrFilterChapter) params.chapter = this.hrFilterChapter;
    if (this.hrFilterSubChapter) params.subChapter = this.hrFilterSubChapter;
    if (this.hrFilterClassification) params.classification = this.hrFilterClassification;
    if (this.hrFilterSubClassification) params.subClassification = this.hrFilterSubClassification;
    if (this.hrFilterUnits) params.units = this.hrFilterUnits;
    if (this.hrFilterStatus) params.status = this.hrFilterStatus;
    if (this.hrSearchName) params.name = this.hrSearchName.trim();

    console.log('🔍 Loading HR Costs with params:', params);

    this.http.get<any>(`${environment.apiUrl}/cost/hr-costs`, { params })
      .subscribe({
        next: (response) => {
          const responseData = response.data || response;
          this.hrCosts = responseData?.data || responseData || [];
          this.hrCostsTotalCount = responseData?.totalCount || this.hrCosts.length;
          this.hrLoading = false;
        },
        error: (err) => {
          this.hrError = 'Failed to load HR costs. Please try again.';
          this.hrLoading = false;
          console.error('Error loading HR costs:', err);
        }
      });
  }

  loadHRFilterOptions(): void {
    const params: any = {};
    
    // Cascading hierarchy: Type → Chapter → Sub Chapter → Classification → Sub Classification → Units
    // Send current filter values to get dependent options
    if (this.hrFilterType) params.type = this.hrFilterType;
    if (this.hrFilterChapter) params.chapter = this.hrFilterChapter;
    if (this.hrFilterSubChapter) params.subChapter = this.hrFilterSubChapter;
    if (this.hrFilterClassification) params.classification = this.hrFilterClassification;
    if (this.hrFilterSubClassification) params.subClassification = this.hrFilterSubClassification;

    this.http.get<any>(`${environment.apiUrl}/cost/hr-costs/filter-options`, { params })
      .subscribe({
        next: (response) => {
          const data = response.data || response;
          this.hrFilterOptions = data;
          console.log('📊 HRC filter options loaded:', {
            types: this.hrFilterOptions.types.length,
            chapters: this.hrFilterOptions.chapters.length,
            subChapters: this.hrFilterOptions.subChapters.length,
            classifications: this.hrFilterOptions.classifications.length,
            subClassifications: this.hrFilterOptions.subClassifications.length,
            units: this.hrFilterOptions.units.length
          });
        },
        error: (err) => {
          console.error('Error loading filter options:', err);
        }
      });
  }

  onHRFilterChange(filterName: string): void {
    console.log('🔄 HRC filter changed:', filterName, 'value:', (this as any)['hrFilter' + filterName.charAt(0).toUpperCase() + filterName.slice(1)]);
    
    // Clear dependent filters based on hierarchy: Type → Chapter → Sub Chapter → Classification → Sub Classification → Units
    if (filterName === 'type') {
      // Type is the first filter - clear all dependent filters
      this.hrFilterChapter = null;
      this.hrFilterSubChapter = null;
      this.hrFilterClassification = null;
      this.hrFilterSubClassification = null;
      this.hrFilterUnits = null;
      
      this.hrFilterOptions.chapters = [];
      this.hrFilterOptions.subChapters = [];
      this.hrFilterOptions.classifications = [];
      this.hrFilterOptions.subClassifications = [];
      this.hrFilterOptions.units = [];
      
      if (this.hrFilterType) {
        this.loadHRFilterOptions();
      }
    } else if (filterName === 'chapter') {
      // Clear dependent filters
      this.hrFilterSubChapter = null;
      this.hrFilterClassification = null;
      this.hrFilterSubClassification = null;
      this.hrFilterUnits = null;
      
      this.hrFilterOptions.subChapters = [];
      this.hrFilterOptions.classifications = [];
      this.hrFilterOptions.subClassifications = [];
      this.hrFilterOptions.units = [];
      
      if (this.hrFilterChapter) {
        this.loadHRFilterOptions();
      }
    } else if (filterName === 'subChapter') {
      this.hrFilterClassification = null;
      this.hrFilterSubClassification = null;
      this.hrFilterUnits = null;
      
      this.hrFilterOptions.classifications = [];
      this.hrFilterOptions.subClassifications = [];
      this.hrFilterOptions.units = [];
      
      if (this.hrFilterSubChapter) {
        this.loadHRFilterOptions();
      }
    } else if (filterName === 'classification') {
      this.hrFilterSubClassification = null;
      this.hrFilterUnits = null;
      
      this.hrFilterOptions.subClassifications = [];
      this.hrFilterOptions.units = [];
      
      if (this.hrFilterClassification) {
        this.loadHRFilterOptions();
      }
    } else if (filterName === 'subClassification') {
      this.hrFilterUnits = null;
      this.hrFilterOptions.units = [];
      
      if (this.hrFilterSubClassification) {
        this.loadHRFilterOptions();
      }
    } else if (filterName === 'units' || filterName === 'status') {
      // These don't have dependents in the cascade
      // Status is independent with hardcoded options
    }
  }

  clearHRFilters(): void {
    this.hrFilterType = null;
    this.hrFilterChapter = null;
    this.hrFilterSubChapter = null;
    this.hrFilterClassification = null;
    this.hrFilterSubClassification = null;
    this.hrFilterUnits = null;
    this.hrFilterStatus = 'Active'; // Reset to default Active
    this.hrSearchName = '';
    
    // Clear cascading filter options (except types which are always loaded)
    this.hrFilterOptions.chapters = [];
    this.hrFilterOptions.subChapters = [];
    this.hrFilterOptions.classifications = [];
    this.hrFilterOptions.subClassifications = [];
    this.hrFilterOptions.units = [];
    
    // Reload HR costs with default Active status filter and reload Types
    this.currentPage = 1;
    this.loadHRCosts();
    this.loadHRFilterOptions();
  }


  // Project Cost Methods
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
    this.currentPage = 1;
    if (this.selectedProject) {
      this.loadProjectCosts();
    } else {
      this.projectCosts = [];
    }
  }

  loadProjectCosts(): void {
    if (!this.selectedProject) return;

    // Prevent duplicate requests
    if (this.projectCostsLoading) {
      console.log('⏳ Project costs already loading, skipping duplicate request');
      return;
    }

    this.projectCostsLoading = true;
    this.projectCostsError = null;

    const params: any = {
      pageNumber: this.currentPage,
      pageSize: this.pageSize
    };

    this.http.get<any>(`${environment.apiUrl}/cost/project-costs/project/${this.selectedProject.projectId}`, { params })
      .subscribe({
        next: (response) => {
          this.projectCosts = response.value || response.data || [];
          this.projectCostsLoading = false;
        },
        error: (err) => {
          this.projectCostsError = 'Failed to load project costs. Please try again.';
          this.projectCostsLoading = false;
          console.error('Error loading project costs:', err);
        }
      });
  }

  openCreateCostModal(): void {
    if (!this.selectedProject) return;
    
    this.showCreateCostModal = true;
    this.createCostError = null;
    this.createCostSuccess = false;
    this.newCost = {
      boxId: null,
      costCodeLevel1: null,
      costCodeLevel2: null,
      costCodeLevel3: null,
      chapter: null,
      subChapter: null,
      classification: null,
      subClassification: null,
      units: null,
      type: null,
      cost: null
    };
    this.loadProjectBoxes();
    this.loadCostCodeFilterOptions();
    this.loadHRFilterOptions();
  }

  closeCreateCostModal(): void {
    this.showCreateCostModal = false;
    this.createCostError = null;
    this.createCostSuccess = false;
    this.newCost = {
      boxId: null,
      costCodeLevel1: null,
      costCodeLevel2: null,
      costCodeLevel3: null,
      chapter: null,
      subChapter: null,
      classification: null,
      subClassification: null,
      units: null,
      type: null,
      cost: null
    };
  }

  loadProjectBoxes(): void {
    if (!this.selectedProject) return;

    this.loadingBoxes = true;

    this.http.get<any>(`${environment.apiUrl}/boxes/project/${this.selectedProject.projectId}`)
      .subscribe({
        next: (response) => {
          this.projectBoxes = response.data || response || [];
          this.loadingBoxes = false;
        },
        error: (err) => {
          console.error('Error loading project boxes:', err);
          this.createCostError = 'Failed to load project boxes';
          this.loadingBoxes = false;
        }
      });
  }

  loadCostCodeFilterOptions(): void {
    const params: any = {};
    if (this.newCost.costCodeLevel1) params.level1 = this.newCost.costCodeLevel1;
    if (this.newCost.costCodeLevel2) params.level2 = this.newCost.costCodeLevel2;

    this.http.get<any>(`${environment.apiUrl}/cost/codes/filter-options`, { params })
      .subscribe({
        next: (response) => {
          const data = response.data || response;
          this.costCodeFilterOptions = data;
          console.log('📊 Loaded cost code filter options:', this.costCodeFilterOptions);
        },
        error: (err) => {
          console.error('Error loading cost code filter options:', err);
        }
      });
  }

  onProjectCostCodeChange(level: string): void {
    if (level === 'level1') {
      this.newCost.costCodeLevel2 = null;
      this.newCost.costCodeLevel3 = null;
    } else if (level === 'level2') {
      this.newCost.costCodeLevel3 = null;
    }
    this.loadCostCodeFilterOptions();
  }

  onProjectHRCostChange(field: string): void {
    // Reset dependent fields when parent changes
    // Cascade order: type → chapter → subChapter → classification → subClassification → units
    const fields = ['type', 'chapter', 'subChapter', 'classification', 'subClassification', 'units'];
    const index = fields.indexOf(field);
    if (index >= 0) {
      for (let i = index + 1; i < fields.length; i++) {
        (this.newCost as any)[fields[i]] = null;
      }
    }
    this.loadHRFilterOptions();
  }

  createProjectCost(): void {
    // Validate required fields (box is optional)
    if (!this.selectedProject || !this.newCost.cost ||
        !this.newCost.costCodeLevel1 || !this.newCost.costCodeLevel2 ||
        !this.newCost.chapter || !this.newCost.subChapter || !this.newCost.classification ||
        !this.newCost.subClassification || !this.newCost.units || !this.newCost.type) {
      this.createCostError = 'Please fill in all required fields';
      return;
    }

    this.creatingCost = true;
    this.createCostError = null;
    this.createCostSuccess = false;

    const payload = {
      projectId: this.selectedProject.projectId, // Include selected project ID
      boxId: this.newCost.boxId,
      cost: this.newCost.cost,
      // Cost Code Master
      costCodeLevel1: this.newCost.costCodeLevel1,
      costCodeLevel2: this.newCost.costCodeLevel2,
      costCodeLevel3: this.newCost.costCodeLevel3,
      // HRC Code
      chapter: this.newCost.chapter,
      subChapter: this.newCost.subChapter,
      classification: this.newCost.classification,
      subClassification: this.newCost.subClassification,
      units: this.newCost.units,
      type: this.newCost.type
    };

    this.http.post<any>(`${environment.apiUrl}/cost/project-costs`, payload)
      .subscribe({
        next: (response) => {
          this.creatingCost = false;
          this.createCostSuccess = true;
          
          // Reload project costs
          setTimeout(() => {
            this.loadProjectCosts();
            this.closeCreateCostModal();
          }, 1500);
        },
        error: (err) => {
          this.creatingCost = false;
          this.createCostError = err.error?.error || err.error?.message || 'Failed to create project cost. Please try again.';
          console.error('Error creating project cost:', err);
        }
      });
  }

  getTotalProjectCost(): number {
    return this.projectCosts.reduce((sum, cost) => sum + cost.cost, 0);
  }

  getCostTypeCount(costType: string): number {
    return this.projectCosts.filter(c => c.costType === costType).length;
  }

  // TrackBy functions for performance optimization
  trackByCostCodeId(index: number, item: CostCode): string {
    return item.costCodeId;
  }

  trackByHRCostId(index: number, item: HRCost): string {
    return item.hrCostRecordId;
  }

  trackByProjectCostId(index: number, item: ProjectCost): string {
    return item.projectCostId;
  }

  trackByProjectId(index: number, item: Project): string {
    return item.projectId;
  }

  trackByBoxId(index: number, item: Box): string {
    return item.boxId;
  }

  trackByValue(index: number, item: string): string {
    return item;
  }

  // Cost Code Modal Functions
  openCreateCostCodeModal(): void {
    this.showCreateCostCodeModal = true;
    this.createCostCodeError = null;
    this.createCostCodeSuccess = false;
    this.resetCostCodeForm();
    
    // Load dropdown options for modal using efficient endpoint
    this.loadModalCostCodeFilterOptions();
  }

  closeCreateCostCodeModal(): void {
    this.showCreateCostCodeModal = false;
    this.resetCostCodeForm();
    this.createCostCodeError = null;
    this.createCostCodeSuccess = false;
  }

  resetCostCodeForm(): void {
    this.newCostCode = {
      code: '',
      costCodeLevel1: null,
      level1Description: null,
      costCodeLevel2: null,
      level2Description: null,
      costCodeLevel3: null,
      description: '',
      level3DescriptionAbbrev: null,
      level3DescriptionAmana: null,
      category: null,
      unitOfMeasure: null,
      unitRate: null,
      currency: 'SAR',
      isActive: true
    };
  }

  createCostCode(): void {
    if (!this.newCostCode.code || !this.newCostCode.description) {
      this.createCostCodeError = 'Please fill in all required fields (Code and Description).';
      return;
    }

    this.creatingCostCode = true;
    this.createCostCodeError = null;
    this.createCostCodeSuccess = false;

    const payload = {
      code: this.newCostCode.code,
      costCodeLevel1: this.newCostCode.costCodeLevel1 || null,
      level1Description: this.newCostCode.level1Description || null,
      costCodeLevel2: this.newCostCode.costCodeLevel2 || null,
      level2Description: this.newCostCode.level2Description || null,
      costCodeLevel3: this.newCostCode.costCodeLevel3 || null,
      description: this.newCostCode.description,
      level3DescriptionAbbrev: this.newCostCode.level3DescriptionAbbrev || null,
      level3DescriptionAmana: this.newCostCode.level3DescriptionAmana || null,
      category: this.newCostCode.category || null,
      unitOfMeasure: this.newCostCode.unitOfMeasure || null,
      unitRate: this.newCostCode.unitRate || null,
      currency: this.newCostCode.currency,
      isActive: this.newCostCode.isActive
    };

    this.http.post<any>(`${environment.apiUrl}/cost/codes`, payload)
      .subscribe({
        next: (response) => {
          this.creatingCostCode = false;
          this.createCostCodeSuccess = true;
          
          // Reload cost codes
          setTimeout(() => {
            this.loadCostCodes();
            this.closeCreateCostCodeModal();
          }, 1500);
        },
        error: (err) => {
          this.creatingCostCode = false;
          this.createCostCodeError = err.error?.error || err.error?.message || 'Failed to create cost code. Please try again.';
          console.error('Error creating cost code:', err);
        }
      });
  }

  // HRC Cost Modal Functions
  openCreateHRCostModal(): void {
    this.showCreateHRCostModal = true;
    this.createHRCostError = null;
    this.createHRCostSuccess = false;
    this.resetHRCostForm();
    
    // Load filter options for dropdowns if not already loaded
    if (this.hrFilterOptions.chapters.length === 0) {
      this.loadHRFilterOptions();
    }
  }

  closeCreateHRCostModal(): void {
    this.showCreateHRCostModal = false;
    this.resetHRCostForm();
    this.createHRCostError = null;
    this.createHRCostSuccess = false;
  }

  resetHRCostForm(): void {
    this.newHRCost = {
      code: null,
      chapter: null,
      subChapter: null,
      classification: null,
      subClassification: null,
      name: '',
      units: null,
      type: null,
      budgetLevel: null,
      status: 'Active',
      job: null,
      officeAccount: null,
      jobCostAccount: null,
      specialAccount: null,
      idlAccount: null
    };
  }

  createHRCost(): void {
    if (!this.newHRCost.name) {
      this.createHRCostError = 'Please fill in the required field (Name).';
      return;
    }

    this.creatingHRCost = true;
    this.createHRCostError = null;
    this.createHRCostSuccess = false;

    const payload = {
      code: this.newHRCost.code || null,
      chapter: this.newHRCost.chapter || null,
      subChapter: this.newHRCost.subChapter || null,
      classification: this.newHRCost.classification || null,
      subClassification: this.newHRCost.subClassification || null,
      name: this.newHRCost.name,
      units: this.newHRCost.units || null,
      type: this.newHRCost.type || null,
      budgetLevel: this.newHRCost.budgetLevel || null,
      status: this.newHRCost.status || 'Active',
      job: this.newHRCost.job || null,
      officeAccount: this.newHRCost.officeAccount || null,
      jobCostAccount: this.newHRCost.jobCostAccount || null,
      specialAccount: this.newHRCost.specialAccount || null,
      idlAccount: this.newHRCost.idlAccount || null
    };

    this.http.post<any>(`${environment.apiUrl}/cost/hr-costs`, payload)
      .subscribe({
        next: (response) => {
          this.creatingHRCost = false;
          this.createHRCostSuccess = true;
          
          // Reload HR costs
          setTimeout(() => {
            this.loadHRCosts();
            this.closeCreateHRCostModal();
          }, 1500);
        },
        error: (err) => {
          this.creatingHRCost = false;
          this.createHRCostError = err.error?.error || err.error?.message || 'Failed to create HRC cost. Please try again.';
          console.error('Error creating HRC cost:', err);
        }
      });
  }



  // Lazy load filter options - only load what's needed based on selection
  loadCostCodeFilterOptionsForSearch(): void {
    // Prevent duplicate requests
    if (this.loadingCostCodeFilterOptions) {
      console.log('⏳ Filter options already loading, skipping duplicate request');
      return;
    }

    this.loadingCostCodeFilterOptions = true;

    const params: any = {};
    
    // Cascading: only request options based on current selections
    if (this.selectedCostLevel1) {
      params.level1 = this.selectedCostLevel1;
      this.loadingCostLevel2Options = true;
      
      if (this.selectedCostLevel2) {
        params.level2 = this.selectedCostLevel2;
        this.loadingCostLevel3Options = true;
      }
    } else {
      // Only load level 1 initially
      this.loadingCostLevel1Options = true;
    }

    this.http.get<any>(`${environment.apiUrl}/cost/codes/filter-options`, { params })
      .subscribe({
        next: (response) => {
          const data = response.data || response;
          
          // Only populate the options that were requested
          if (!this.selectedCostLevel1) {
            this.costLevel1Options = data.level1Options || [];
            console.log('✅ Level 1 options loaded:', this.costLevel1Options.length);
          }
          
          if (this.selectedCostLevel1) {
            this.costLevel2Options = data.level2Options || [];
            console.log('✅ Level 2 options loaded:', this.costLevel2Options.length);
          }
          
          if (this.selectedCostLevel2) {
            this.costLevel3Options = data.level3Options || [];
            console.log('✅ Level 3 options loaded:', this.costLevel3Options.length);
          }
          
          this.loadingCostCodeFilterOptions = false;
          this.loadingCostLevel1Options = false;
          this.loadingCostLevel2Options = false;
          this.loadingCostLevel3Options = false;
        },
        error: (err) => {
          console.error('Error loading cost code filter options:', err);
          this.loadingCostCodeFilterOptions = false;
          this.loadingCostLevel1Options = false;
          this.loadingCostLevel2Options = false;
          this.loadingCostLevel3Options = false;
        }
      });
  }

  // Lazy load Level 1 options when dropdown is clicked
  onCostLevel1DropdownClick(): void {
    if (this.costLevel1Options.length === 0 && !this.loadingCostLevel1Options) {
      console.log('🔄 Lazy loading Level 1 options...');
      this.loadCostCodeFilterOptionsForSearch();
    }
  }

  // Handle cascading filter updates - load dependent options on demand
  onCostCodeFilterChange(level: string): void {
    if (level === 'level1') {
      // Clear dependent selections
      this.selectedCostLevel2 = null;
      this.selectedCostLevel3 = null;
      this.costLevel2Options = [];
      this.costLevel3Options = [];
      
      // Load level 2 options based on selected level 1
      if (this.selectedCostLevel1) {
        console.log('🔄 Loading Level 2 options for:', this.selectedCostLevel1);
        this.loadCostCodeFilterOptionsForSearch();
      }
    } else if (level === 'level2') {
      // Clear dependent selections
      this.selectedCostLevel3 = null;
      this.costLevel3Options = [];
      
      // Load level 3 options based on selected level 2
      if (this.selectedCostLevel2) {
        console.log('🔄 Loading Level 3 options for:', this.selectedCostLevel2);
        this.loadCostCodeFilterOptionsForSearch();
      }
    }
  }

  // Load dropdown options for Create Cost Code Modal using efficient endpoint
  loadModalCostLevel1Options(): void {
    // Modal options will be loaded together with loadModalCostCodeFilterOptions
    this.loadModalCostCodeFilterOptions();
  }

  loadModalCostLevel2Options(): void {
    // Modal options will be loaded together with loadModalCostCodeFilterOptions
    this.loadModalCostCodeFilterOptions();
  }

  loadModalLevel1DescriptionOptions(): void {
    // Descriptions are loaded as part of the main filter options
    // This is now a placeholder for compatibility
    this.loadingModalLevel1DescOptions = false;
  }

  loadModalLevel2DescriptionOptions(): void {
    // Descriptions are loaded as part of the main filter options
    // This is now a placeholder for compatibility
    this.loadingModalLevel2DescOptions = false;
  }

  // Centralized method to load modal filter options efficiently
  loadModalCostCodeFilterOptions(): void {
    this.loadingModalCostLevel1Options = true;
    this.loadingModalCostLevel2Options = true;

    const params: any = {};
    if (this.newCostCode.costCodeLevel1) params.level1 = this.newCostCode.costCodeLevel1;
    if (this.newCostCode.costCodeLevel2) params.level2 = this.newCostCode.costCodeLevel2;

    this.http.get<any>(`${environment.apiUrl}/cost/codes/filter-options`, { params })
      .subscribe({
        next: (response) => {
          const data = response.data || response;
          this.modalCostLevel1Options = data.level1Options || [];
          this.modalCostLevel2Options = data.level2Options || [];
          
          console.log('✅ Modal Cost Code filter options loaded:', {
            level1: this.modalCostLevel1Options.length,
            level2: this.modalCostLevel2Options.length
          });
          
          this.loadingModalCostLevel1Options = false;
          this.loadingModalCostLevel2Options = false;
        },
        error: (err) => {
          console.error('Error loading modal cost code filter options:', err);
          this.modalCostLevel1Options = [];
          this.modalCostLevel2Options = [];
          this.loadingModalCostLevel1Options = false;
          this.loadingModalCostLevel2Options = false;
        }
      });
  }
}

