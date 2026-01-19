import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../../../environments/environment';
import { HeaderComponent } from '../../../shared/components/header/header.component';
import { SidebarComponent } from '../../../shared/components/sidebar/sidebar.component';
import { SafePipe } from '../../../shared/pipes/safe.pipe';

interface BIMModel {
  bimModelId: string;
  modelName: string;
  discipline?: string;
  category?: string;
  revitFamily?: string;
  type?: string;
  instance?: string;
  quantity?: number;
  unit?: string;
  plannedStartDate?: string;
  plannedFinishDate?: string;
  thumbnailPath?: string;
}

@Component({
  selector: 'app-bim-dashboard',
  standalone: true,
  imports: [CommonModule, RouterModule, FormsModule, HeaderComponent, SidebarComponent, SafePipe],
  templateUrl: './bim-dashboard.component.html',
  styleUrl: './bim-dashboard.component.scss'
})
export class BIMDashboardComponent implements OnInit {
  models: BIMModel[] = [];
  loading = false;
  error: string | null = null;

  // Default 3D Model URL (when no model is selected)
  // Using working Sketchfab embed - Aristocratic Mansion with autoplay
  defaultModelUrl = 'https://sketchfab.com/models/f495ed0ed0d14788a1319200e884f3cf/embed?autostart=1&autospin=0.2&ui_infos=0';
  defaultModelName = 'Aristocratic Mansion';

  // Left Panel - Create Form
  newModel = {
    modelName: '',
    discipline: '',
    category: '',
    revitFamily: '',
    type: '',
    instance: '',
    quantity: 0,
    unit: '',
    plannedStartDate: '',
    plannedFinishDate: '',
    actualStartDate: '',
    actualFinishDate: '',
    description: ''
  };

  // Disciplines dropdown
  disciplines = ['Structure', 'Architecture', 'MEP', 'Civil', 'Landscaping'];

  // Selected category for filtering
  selectedCategory: string = 'all';
  categories = [
    'Air Terminals',
    'Cable Tray Fittings',
    'Cable Trays',
    'Casework',
    'Ceilings',
    'Columns',
    'Communication Devices',
    'Conduit Fittings',
    'Conduits',
    'Curtain Panels',
    'Data Devices',,
    'Doors',
    'Duct Accessories',
    
    'Duct Placeholders',
    'Ducts',
    'Electrical Equipment',
    'Electrical Fixtures',
    'Entourage',
    'Fire Alarm Devices',
    'Fire Protection',
    'Flex Ducts',
    'Flex Pipes',
    'Floors',
    'Furniture',
    'Furniture Systems',
    'Generic Models',
    'Lighting Devices',
    'Lighting Fixtures',
    'Mechanical Control Devices',
    'Mechanical Equipment',
    
    'Pipe Accessories',
    'Pipe Fittings',
    'Pipe Insulations',
    'Pipe Placeholders',
    'Pipes',
    'Planting',
    'Plumbing Equipment',
    'Plumbing Fixtures',
    'Railings',
    'Ramps',
    'Roads',
    'Roofs',
    'Security Devices',
    'Shaft Openings',
    'Signage',
    'Site',
    'Specialty Equipment',
    'Sprinklers',
    'Stairs',
    'Structural Area Reinforcement',
    'Structural Beam Systems',
    'Structural Columns',
    'Structural Connections',
    'Structural Fabric Areas',
    'Structural Fabric Reinforcement',
    'Structural Foundations',
    'Structural Framing',
    'Structural Path Reinforcement',
    'Structural Rebar',
    'Structural Rebar Couplers',
    'Structural Stiffeners',
    'Structural Trusses',
    'Telephone Devices',
    'Temporary Structures',
    'Topography',
   
    'Vertical Circulation',
    'Walls',
    'Windows',
    'Wires'
  ];

  // BIM 5D  - Quantity
  showBIM5D = true;

  // BIM 4D - Schedule
  showBIM4D = true;

  // 3D Viewer State
  currentModelIndex = 0;

  constructor(private http: HttpClient) {}

  ngOnInit(): void {
    this.loadModels();
  }

  loadModels(): void {
    this.loading = true;
    this.error = null;

    this.http.get<any>(`${environment.apiUrl}/bim/models`).subscribe({
      next: (response) => {
        this.models = response.data || [];
        this.loading = false;
      },
      error: (err) => {
        this.error = 'Failed to load BIM models';
        this.loading = false;
        console.error('Error loading BIM models:', err);
      }
    });
  }

  createModel(): void {
    if (!this.newModel.modelName) {
      alert('Please enter model name');
      return;
    }

    this.http.post<any>(`${environment.apiUrl}/bim/models`, this.newModel).subscribe({
      next: (response) => {
        alert('BIM Model created successfully!');
        this.resetForm();
        this.loadModels();
      },
      error: (err) => {
        alert('Failed to create model: ' + (err.error?.message || 'Unknown error'));
        console.error('Error creating model:', err);
      }
    });
  }

  resetForm(): void {
    this.newModel = {
      modelName: '',
      discipline: '',
      category: '',
      revitFamily: '',
      type: '',
      instance: '',
      quantity: 0,
      unit: '',
      plannedStartDate: '',
      plannedFinishDate: '',
      actualStartDate: '',
      actualFinishDate: '',
      description: ''
    };
  }

  getFilteredModels(): BIMModel[] {
    if (this.selectedCategory === 'all') {
      return this.models;
    }
    return this.models.filter(m => m.category === this.selectedCategory);
  }

  selectModel(index: number): void {
    this.currentModelIndex = index;
  }

  toggleBIM5D(): void {
    this.showBIM5D = !this.showBIM5D;
  }

  toggleBIM4D(): void {
    this.showBIM4D = !this.showBIM4D;
  }
}

