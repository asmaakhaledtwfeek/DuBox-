import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router, RouterModule } from '@angular/router';
import { HeaderComponent } from '../../../shared/components/header/header.component';
import { SidebarComponent } from '../../../shared/components/sidebar/sidebar.component';

interface WIROption {
  code: string;
  name: string;
  description: string;
  icon: string;
}

@Component({
  selector: 'app-manage-predefined-checklists',
  standalone: true,
  imports: [CommonModule, RouterModule, HeaderComponent, SidebarComponent],
  templateUrl: './manage-predefined-checklists.component.html',
  styleUrls: ['./manage-predefined-checklists.component.scss']
})
export class ManagePredefinedChecklistsComponent implements OnInit {
  wirOptions: WIROption[] = [
    { code: 'WIR-1', name: 'WIR-1', description: 'Box Closure Inspection', icon: '📦' },
    { code: 'WIR-2', name: 'WIR-2', description: 'MEP', icon: '🏗️' },
    { code: 'WIR-3', name: 'WIR-3', description: 'Electrical Inspection', icon: '⚡' },
    { code: 'WIR-4', name: 'WIR-4', description: 'Plumbing Inspection', icon: '🔧' },
    { code: 'WIR-5', name: 'WIR-5', description: 'HVAC Inspection', icon: '❄️' },
    { code: 'WIR-6', name: 'WIR-6', description: 'Final Inspection', icon: '✅' }
  ];

  constructor(private router: Router) {}

  ngOnInit(): void {}

  selectWIR(wirCode: string): void {
    this.router.navigate(['/qc/predefined-checklists', wirCode]);
  }

  goBack(): void {
    this.router.navigate(['/qc']);
  }
}

