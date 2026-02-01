import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { FormBuilder, FormGroup, FormArray, Validators, ReactiveFormsModule } from '@angular/forms';
import { ProjectService } from '../../../core/services/project.service';
import { HeaderComponent } from '../../../shared/components/header/header.component';
import { SidebarComponent } from '../../../shared/components/sidebar/sidebar.component';
import { 
  ProjectConfiguration, 
  ProjectBuilding, 
  ProjectLevel, 
  ProjectBoxType, 
  ProjectBoxSubType,
  ProjectZone, 
  ProjectBoxFunction 
} from '../../../core/models/project-configuration.model';

@Component({
  selector: 'app-project-configuration',
  standalone: true,
  imports: [CommonModule, RouterModule, ReactiveFormsModule, HeaderComponent, SidebarComponent],
  templateUrl: './project-configuration.component.html',
  styleUrl: './project-configuration.component.scss'
})
export class ProjectConfigurationComponent implements OnInit {
  projectId!: string;
  projectName: string = '';
  loading = false;
  saving = false;
  error = '';
  successMessage = '';
  
  // Configuration arrays
  buildings: ProjectBuilding[] = [];
  levels: ProjectLevel[] = [];
  boxTypes: ProjectBoxType[] = [];
  zones: ProjectZone[] = [];
  boxFunctions: ProjectBoxFunction[] = [];

  // Temp forms for adding new items
  newBuilding = '';
  newLevel = '';
  newBoxType = '';
  newBoxTypeName = '';
  newBoxSubType = '';
  selectedTypeForSubType = -1;
  newZone = '';
  newBoxFunction = '';

  constructor(
    private fb: FormBuilder,
    private projectService: ProjectService,
    private router: Router,
    private route: ActivatedRoute
  ) {}

  ngOnInit(): void {
    this.route.params.subscribe(params => {
      this.projectId = params['projectId'];
      if (this.projectId) {
        this.loadProjectConfiguration();
      } else {
        this.error = 'No project ID provided';
      }
    });
  }

  private loadProjectConfiguration(): void {
    this.loading = true;
    this.projectService.getProjectConfiguration(this.projectId).subscribe({
      next: (config) => {
        this.buildings = config.buildings || [];
        this.levels = config.levels || [];
        this.boxTypes = config.boxTypes || [];
        this.zones = config.zones || [];
        this.boxFunctions = config.boxFunctions || [];
        this.loading = false;
      },
      error: (err) => {
        console.log('No existing configuration or error loading:', err);
        // It's okay if there's no configuration yet
        this.loading = false;
      }
    });
  }

  // Building methods
  addBuilding(): void {
    if (this.newBuilding.trim()) {
      // Split by comma and add each item
      const items = this.newBuilding.split(',').map(item => item.trim()).filter(item => item);
      items.forEach(item => {
        // Check if building already exists
        const exists = this.buildings.some(b => b.buildingCode.toLowerCase() === item.toLowerCase());
        if (!exists) {
          this.buildings.push({
            buildingCode: item,
            buildingName: item
          });
        }
      });
      this.newBuilding = '';
    }
  }

  removeBuilding(index: number): void {
    this.buildings.splice(index, 1);
  }

  // Level methods
  addLevel(): void {
    if (this.newLevel.trim()) {
      // Split by comma and add each item
      const items = this.newLevel.split(',').map(item => item.trim()).filter(item => item);
      items.forEach(item => {
        // Check if level already exists
        const exists = this.levels.some(l => l.levelCode.toLowerCase() === item.toLowerCase());
        if (!exists) {
          this.levels.push({
            levelCode: item,
            levelName: item
          });
        }
      });
      this.newLevel = '';
    }
  }

  removeLevel(index: number): void {
    this.levels.splice(index, 1);
  }

  // Box Type methods
  addBoxType(): void {
    if (this.newBoxType.trim()) {
      // Split by comma and add each item
      const items = this.newBoxType.split(',').map(item => item.trim()).filter(item => item);
      items.forEach(item => {
        // Check if box type already exists
        const exists = this.boxTypes.some(t => t.typeName.toLowerCase() === item.toLowerCase());
        if (!exists) {
          this.boxTypes.push({
            typeName: item,
            hasSubTypes: false,
            subTypes: []
          });
        }
      });
      this.newBoxType = '';
    }
  }

  removeBoxType(index: number): void {
    this.boxTypes.splice(index, 1);
  }

  toggleHasSubTypes(index: number): void {
    const type = this.boxTypes[index];
    type.hasSubTypes = !type.hasSubTypes;
    if (!type.hasSubTypes) {
      type.subTypes = [];
    }
  }

  // Box SubType methods
  addSubType(typeIndex: number): void {
    if (this.newBoxSubType.trim()) {
      if (!this.boxTypes[typeIndex].subTypes) {
        this.boxTypes[typeIndex].subTypes = [];
      }
      // Split by comma and add each item
      const items = this.newBoxSubType.split(',').map(item => item.trim()).filter(item => item);
      items.forEach(item => {
        // Check if subtype already exists
        const exists = this.boxTypes[typeIndex].subTypes?.some(s => s.subTypeName.toLowerCase() === item.toLowerCase());
        if (!exists) {
          this.boxTypes[typeIndex].subTypes!.push({
            subTypeName: item
          });
        }
      });
      this.newBoxSubType = '';
      this.selectedTypeForSubType = -1;
    }
  }

  removeSubType(typeIndex: number, subTypeIndex: number): void {
    this.boxTypes[typeIndex].subTypes?.splice(subTypeIndex, 1);
  }

  // Zone methods
  addZone(): void {
    if (this.newZone.trim()) {
      // Split by comma and add each item
      const items = this.newZone.split(',').map(item => item.trim()).filter(item => item);
      items.forEach(item => {
        // Check if zone already exists
        const exists = this.zones.some(z => z.zoneCode.toLowerCase() === item.toLowerCase());
        if (!exists) {
          this.zones.push({
            zoneCode: item,
            zoneName: item
          });
        }
      });
      this.newZone = '';
    }
  }

  removeZone(index: number): void {
    this.zones.splice(index, 1);
  }

  // Box Function methods
  addBoxFunction(): void {
    if (this.newBoxFunction.trim()) {
      // Split by comma and add each item
      const items = this.newBoxFunction.split(',').map(item => item.trim()).filter(item => item);
      items.forEach(item => {
        // Check if function already exists
        const exists = this.boxFunctions.some(f => f.functionName.toLowerCase() === item.toLowerCase());
        if (!exists) {
          this.boxFunctions.push({
            functionName: item
          });
        }
      });
      this.newBoxFunction = '';
    }
  }

  removeBoxFunction(index: number): void {
    this.boxFunctions.splice(index, 1);
  }

  // Save all configuration
  saveConfiguration(): void {
    this.saving = true;
    this.error = '';
    this.successMessage = '';

    const configuration: ProjectConfiguration = {
      projectId: this.projectId,
      buildings: this.buildings,
      levels: this.levels,
      boxTypes: this.boxTypes,
      zones: this.zones,
      boxFunctions: this.boxFunctions
    };

    this.projectService.saveProjectConfiguration(this.projectId, configuration).subscribe({
      next: () => {
        this.saving = false;
        this.successMessage = 'Project configuration saved successfully!';
        setTimeout(() => {
          this.router.navigate(['/projects', this.projectId, 'dashboard']);
        }, 1500);
      },
      error: (err) => {
        this.saving = false;
        this.error = err.error?.message || 'Failed to save configuration. Please try again.';
        console.error('Error saving configuration:', err);
      }
    });
  }

  cancel(): void {
    this.router.navigate(['/projects', this.projectId, 'dashboard']);
  }

  skipConfiguration(): void {
    this.router.navigate(['/projects', this.projectId, 'dashboard']);
  }
}

