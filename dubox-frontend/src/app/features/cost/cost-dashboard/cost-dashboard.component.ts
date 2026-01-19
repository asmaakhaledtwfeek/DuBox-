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
  name: string;
  units?: string | null;        // Units: Ls, Hr, Day, Month
  costType?: string | null;     // Type: Manpower, etc.
  trade?: string | null;
  position?: string | null;
  hourlyRate?: number | null;
  dailyRate?: number | null;
  monthlyRate?: number | null;
  overtimeRate?: number | null;
  currency: string;
  isActive: boolean;
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
  boxId: string;
  hrCostRecordId?: string;
  cost: number;
  costType: string;
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
  boxId: string;
  costType: string;
  hrCostRecordId: string;
  cost: number | null;
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
  name: string;
  units: string | null;
  costType: string | null;
  isActive: boolean;
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
  
  // HR Costs
  hrCosts: HRCost[] = [];
  hrCostsTotalCount = 0;
  hrLoading = false;
  hrError: string | null = null;
  hrSearchCode = '';
  hrSearchName = '';
  hrSearchUnits = '';
  selectedHRCostType: string | null = null;
  selectedHRIsActive: boolean | null = null;
  hrCostTypes: string[] = [];
  loadingHRCostTypes = false;
  hrUnitsOptions: string[] = [];
  loadingHRUnitsOptions = false;
  
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
  costTypes: string[] = [];
  loadingCostTypes = false;
  filteredHRCosts: HRCost[] = [];
  loadingFilteredHRCosts = false;
  newCost: NewCostForm = {
    boxId: '',
    costType: '',
    hrCostRecordId: '',
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
    name: '',
    units: null,
    costType: null,
    isActive: true
  };
  creatingHRCost = false;
  createHRCostError: string | null = null;
  createHRCostSuccess = false;
  
  // Pagination
  currentPage = 1;
  pageSize = 50;
  totalRecords = 0;

  constructor(private http: HttpClient) {}

  ngOnInit(): void {
    this.loadCostCodes();
    this.loadHRCosts();
    this.loadProjects();
    this.loadCostTypes();
    this.loadCostLevel1Options();
    this.loadCostLevel2Options();
    this.loadCostLevel3Options();
    this.loadHRUnitsOptions();
  }

  switchTab(tab: 'materials' | 'hr' | 'projectCost'): void {
    this.activeTab = tab;
    if (tab === 'projectCost' && this.projects.length === 0) {
      this.loadProjects();
    }
    if (tab === 'hr' && this.hrCostTypes.length === 0) {
      this.loadHRCostTypes();
    }
  }

  loadCostCodes(): void {
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

  // HR Cost Methods
  loadHRCosts(): void {
    this.hrLoading = true;
    this.hrError = null;

    const params: any = {
      pageNumber: this.currentPage,
      pageSize: this.pageSize
    };

    // Add search filters based on new backend parameters
    if (this.hrSearchCode) {
      params.code = this.hrSearchCode.trim();
    }

    if (this.hrSearchName) {
      params.name = this.hrSearchName.trim();
    }

    if (this.hrSearchUnits) {
      params.units = this.hrSearchUnits.trim();
    }

    if (this.selectedHRCostType) {
      params.costType = this.selectedHRCostType;
    }

    if (this.selectedHRIsActive !== null) {
      params.isActive = this.selectedHRIsActive;
    }

    // Debug logging
    console.log('🔍 Loading HRC Costs with params:', params);

    this.http.get<any>(`${environment.apiUrl}/cost/hr-costs`, { params })
      .subscribe({
        next: (response) => {
          const responseData = response.data || response;
          this.hrCosts = responseData?.data || responseData || [];
          this.hrCostsTotalCount = responseData?.totalCount || this.hrCosts.length;
          this.hrLoading = false;
        },
        error: (err) => {
          this.hrError = 'Failed to load HRC codes. Please try again.';
          this.hrLoading = false;
          console.error('Error loading HRC codes:', err);
        }
      });
  }

  onHRSearch(): void {
    console.log('🔎 HR Search triggered - code:', this.hrSearchCode, 'name:', this.hrSearchName, 'units:', this.hrSearchUnits);
    this.currentPage = 1;
    this.loadHRCosts();
  }

  clearHRFilters(): void {
    this.hrSearchCode = '';
    this.hrSearchName = '';
    this.hrSearchUnits = '';
    this.selectedHRCostType = null;
    this.selectedHRIsActive = null;
    this.currentPage = 1;
    this.loadHRCosts();
  }

  hasActiveHRFilters(): boolean {
    return !!(this.hrSearchCode || this.hrSearchName || this.hrSearchUnits || this.selectedHRCostType || this.selectedHRIsActive !== null);
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
    this.filteredHRCosts = [];
    this.newCost = {
      boxId: '',
      costType: '',
      hrCostRecordId: '',
      cost: null
    };
    this.loadProjectBoxes();
    
    // Load cost types if not already loaded
    if (this.costTypes.length === 0) {
      this.loadCostTypes();
    }
  }

  closeCreateCostModal(): void {
    this.showCreateCostModal = false;
    this.createCostError = null;
    this.createCostSuccess = false;
    this.filteredHRCosts = [];
    this.newCost = {
      boxId: '',
      costType: '',
      hrCostRecordId: '',
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

  onCostTypeChange(): void {
    // Reset HRC cost selection when cost type changes
    this.newCost.hrCostRecordId = '';
    this.filteredHRCosts = [];
    
    if (this.newCost.costType) {
      this.loadHRCostsByCostType(this.newCost.costType);
    }
  }

  loadHRCostsByCostType(costType: string): void {
    this.loadingFilteredHRCosts = true;

    const params: any = {
      costType: costType,
      isActive: true,
      pageSize: 1000 // Get all active HRC costs for this type
    };

    this.http.get<any>(`${environment.apiUrl}/cost/hr-costs`, { params })
      .subscribe({
        next: (response) => {
          const responseData = response.data || response;
          this.filteredHRCosts = responseData?.data || responseData || [];
          this.loadingFilteredHRCosts = false;
          console.log('✅ Loaded HRC costs for cost type:', costType, this.filteredHRCosts.length);
        },
        error: (err) => {
          console.error('Error loading HRC costs by cost type:', err);
          this.filteredHRCosts = [];
          this.loadingFilteredHRCosts = false;
        }
      });
  }

  createProjectCost(): void {
    if (!this.selectedProject || !this.newCost.boxId || !this.newCost.costType || !this.newCost.hrCostRecordId || !this.newCost.cost) {
      return;
    }

    this.creatingCost = true;
    this.createCostError = null;
    this.createCostSuccess = false;

    const payload = {
      boxId: this.newCost.boxId,
      costType: this.newCost.costType,
      hrCostRecordId: this.newCost.hrCostRecordId,
      cost: this.newCost.cost
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

  // Cost Code Modal Functions
  openCreateCostCodeModal(): void {
    this.showCreateCostCodeModal = true;
    this.createCostCodeError = null;
    this.createCostCodeSuccess = false;
    this.resetCostCodeForm();
    
    // Load dropdown options for modal
    this.loadModalCostLevel1Options();
    this.loadModalCostLevel2Options();
    this.loadModalLevel1DescriptionOptions();
    this.loadModalLevel2DescriptionOptions();
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
    
    // Load cost types for dropdown if not already loaded
    if (this.hrCostTypes.length === 0) {
      this.loadHRCostTypes();
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
      name: '',
      units: null,
      costType: null,
      isActive: true
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
      name: this.newHRCost.name,
      units: this.newHRCost.units || null,
      costType: this.newHRCost.costType || null,
      isActive: this.newHRCost.isActive
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

  loadCostTypes(): void {
    this.loadingCostTypes = true;

    // Use the new distinct cost-types endpoint
    this.http.get<any>(`${environment.apiUrl}/cost/cost-types`)
      .subscribe({
        next: (response) => {
          const costTypesData = response.data || response || [];
          
          // Extract just the type names from the response
          this.costTypes = costTypesData.map((ct: any) => ct.costType).sort();
          
        
          this.loadingCostTypes = false;
        },
        error: (err) => {
          console.error('Error loading cost types:', err);
          // Use default list on error
          this.costTypes = ['Direct Labor', 'Indirect', 'Overhead', 'Material', 'Equipment', 'Subcontract', 'Other'];
          this.loadingCostTypes = false;
        }
      });
  }

  loadHRCostTypes(): void {
    this.loadingHRCostTypes = true;

    // Use the cost-types endpoint to get HRC cost types
    this.http.get<any>(`${environment.apiUrl}/cost/cost-types`)
      .subscribe({
        next: (response) => {
          const costTypesData = response.data || response || [];
          
          // Extract just the type names from the response and sort
          this.hrCostTypes = costTypesData.map((ct: any) => ct.costType).sort();
          
          console.log('✅ HRC Cost Types loaded:', this.hrCostTypes);
          this.loadingHRCostTypes = false;
        },
        error: (err) => {
          console.error('Error loading HRC cost types:', err);
          // Use default list on error
          this.hrCostTypes = ['Manpower', 'Equipment', 'Labor', 'Supervision'];
          this.loadingHRCostTypes = false;
        }
      });
  }

  loadHRUnitsOptions(): void {
    this.loadingHRUnitsOptions = true;

    // Fetch all HRC costs and extract distinct units values
    const params: any = {
      pageSize: 10000 // Get all records to extract distinct values
    };

    this.http.get<any>(`${environment.apiUrl}/cost/hr-costs`, { params })
      .subscribe({
        next: (response) => {
          const responseData = response.data || response;
          const hrCosts = responseData?.data || responseData || [];
          
          // Extract distinct units values and filter out nulls/empty
          const distinctUnits = new Set<string>();
          hrCosts.forEach((cost: HRCost) => {
            if (cost.units) {
              distinctUnits.add(cost.units);
            }
          });
          
          this.hrUnitsOptions = Array.from(distinctUnits).sort();
          console.log('✅ HR Units options loaded:', this.hrUnitsOptions.length);
          this.loadingHRUnitsOptions = false;
        },
        error: (err) => {
          console.error('Error loading HR units options:', err);
          // Use default list on error
          this.hrUnitsOptions = ['Ls', 'Hr', 'Day', 'Month'];
          this.loadingHRUnitsOptions = false;
        }
      });
  }

  loadCostLevel1Options(): void {
    this.loadingCostLevel1Options = true;

    // Fetch all cost codes and extract distinct costCodeLevel1 values
    const params: any = {
      pageSize: 10000 // Get all records to extract distinct values
    };

    this.http.get<any>(`${environment.apiUrl}/cost/codes`, { params })
      .subscribe({
        next: (response) => {
          const responseData = response.data || response;
          const costCodes = responseData?.data || responseData || [];
          
          // Extract distinct costCodeLevel1 values and filter out nulls/empty
          const distinctLevels = new Set<string>();
          costCodes.forEach((code: CostCode) => {
            if (code.costCodeLevel1) {
              distinctLevels.add(code.costCodeLevel1);
            }
          });
          
          this.costLevel1Options = Array.from(distinctLevels).sort();
          console.log('✅ Cost Level 1 options loaded:', this.costLevel1Options.length);
          this.loadingCostLevel1Options = false;
        },
        error: (err) => {
          console.error('Error loading cost level 1 options:', err);
          this.costLevel1Options = [];
          this.loadingCostLevel1Options = false;
        }
      });
  }

  loadCostLevel2Options(): void {
    this.loadingCostLevel2Options = true;

    // Fetch all cost codes and extract distinct costCodeLevel2 values
    const params: any = {
      pageSize: 10000 // Get all records to extract distinct values
    };

    this.http.get<any>(`${environment.apiUrl}/cost/codes`, { params })
      .subscribe({
        next: (response) => {
          const responseData = response.data || response;
          const costCodes = responseData?.data || responseData || [];
          
          // Extract distinct costCodeLevel2 values and filter out nulls/empty
          const distinctLevels = new Set<string>();
          costCodes.forEach((code: CostCode) => {
            if (code.costCodeLevel2) {
              distinctLevels.add(code.costCodeLevel2);
            }
          });
          
          this.costLevel2Options = Array.from(distinctLevels).sort();
          console.log('✅ Cost Level 2 options loaded:', this.costLevel2Options.length);
          this.loadingCostLevel2Options = false;
        },
        error: (err) => {
          console.error('Error loading cost level 2 options:', err);
          this.costLevel2Options = [];
          this.loadingCostLevel2Options = false;
        }
      });
  }

  loadCostLevel3Options(): void {
    this.loadingCostLevel3Options = true;

    // Fetch all cost codes and extract distinct costCodeLevel3 values
    const params: any = {
      pageSize: 10000 // Get all records to extract distinct values
    };

    this.http.get<any>(`${environment.apiUrl}/cost/codes`, { params })
      .subscribe({
        next: (response) => {
          const responseData = response.data || response;
          const costCodes = responseData?.data || responseData || [];
          
          // Extract distinct costCodeLevel3 values and filter out nulls/empty
          const distinctLevels = new Set<string>();
          costCodes.forEach((code: CostCode) => {
            if (code.costCodeLevel3) {
              distinctLevels.add(code.costCodeLevel3);
            }
          });
          
          this.costLevel3Options = Array.from(distinctLevels).sort();
          console.log('✅ Cost Level 3 options loaded:', this.costLevel3Options.length);
          this.loadingCostLevel3Options = false;
        },
        error: (err) => {
          console.error('Error loading cost level 3 options:', err);
          this.costLevel3Options = [];
          this.loadingCostLevel3Options = false;
        }
      });
  }

  // Load dropdown options for Create Cost Code Modal
  loadModalCostLevel1Options(): void {
    this.loadingModalCostLevel1Options = true;

    const params: any = {
      pageSize: 10000
    };

    this.http.get<any>(`${environment.apiUrl}/cost/codes`, { params })
      .subscribe({
        next: (response) => {
          const responseData = response.data || response;
          const costCodes = responseData?.data || responseData || [];
          
          const distinctLevels = new Set<string>();
          costCodes.forEach((code: CostCode) => {
            if (code.costCodeLevel1) {
              distinctLevels.add(code.costCodeLevel1);
            }
          });
          
          this.modalCostLevel1Options = Array.from(distinctLevels).sort();
          console.log('✅ Modal Cost Level 1 options loaded:', this.modalCostLevel1Options.length);
          this.loadingModalCostLevel1Options = false;
        },
        error: (err) => {
          console.error('Error loading modal cost level 1 options:', err);
          this.modalCostLevel1Options = [];
          this.loadingModalCostLevel1Options = false;
        }
      });
  }

  loadModalCostLevel2Options(): void {
    this.loadingModalCostLevel2Options = true;

    const params: any = {
      pageSize: 10000
    };

    this.http.get<any>(`${environment.apiUrl}/cost/codes`, { params })
      .subscribe({
        next: (response) => {
          const responseData = response.data || response;
          const costCodes = responseData?.data || responseData || [];
          
          const distinctLevels = new Set<string>();
          costCodes.forEach((code: CostCode) => {
            if (code.costCodeLevel2) {
              distinctLevels.add(code.costCodeLevel2);
            }
          });
          
          this.modalCostLevel2Options = Array.from(distinctLevels).sort();
          console.log('✅ Modal Cost Level 2 options loaded:', this.modalCostLevel2Options.length);
          this.loadingModalCostLevel2Options = false;
        },
        error: (err) => {
          console.error('Error loading modal cost level 2 options:', err);
          this.modalCostLevel2Options = [];
          this.loadingModalCostLevel2Options = false;
        }
      });
  }

  loadModalLevel1DescriptionOptions(): void {
    this.loadingModalLevel1DescOptions = true;

    const params: any = {
      pageSize: 10000
    };

    this.http.get<any>(`${environment.apiUrl}/cost/codes`, { params })
      .subscribe({
        next: (response) => {
          const responseData = response.data || response;
          const costCodes = responseData?.data || responseData || [];
          
          const distinctDescriptions = new Set<string>();
          costCodes.forEach((code: CostCode) => {
            if (code.level1Description) {
              distinctDescriptions.add(code.level1Description);
            }
          });
          
          this.modalLevel1DescriptionOptions = Array.from(distinctDescriptions).sort();
          console.log('✅ Modal Level 1 Description options loaded:', this.modalLevel1DescriptionOptions.length);
          this.loadingModalLevel1DescOptions = false;
        },
        error: (err) => {
          console.error('Error loading modal level 1 description options:', err);
          this.modalLevel1DescriptionOptions = [];
          this.loadingModalLevel1DescOptions = false;
        }
      });
  }

  loadModalLevel2DescriptionOptions(): void {
    this.loadingModalLevel2DescOptions = true;

    const params: any = {
      pageSize: 10000
    };

    this.http.get<any>(`${environment.apiUrl}/cost/codes`, { params })
      .subscribe({
        next: (response) => {
          const responseData = response.data || response;
          const costCodes = responseData?.data || responseData || [];
          
          const distinctDescriptions = new Set<string>();
          costCodes.forEach((code: CostCode) => {
            if (code.level2Description) {
              distinctDescriptions.add(code.level2Description);
            }
          });
          
          this.modalLevel2DescriptionOptions = Array.from(distinctDescriptions).sort();
          console.log('✅ Modal Level 2 Description options loaded:', this.modalLevel2DescriptionOptions.length);
          this.loadingModalLevel2DescOptions = false;
        },
        error: (err) => {
          console.error('Error loading modal level 2 description options:', err);
          this.modalLevel2DescriptionOptions = [];
          this.loadingModalLevel2DescOptions = false;
        }
      });
  }
}

