import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { HeaderComponent } from '../../../shared/components/header/header.component';
import { SidebarComponent } from '../../../shared/components/sidebar/sidebar.component';

interface HelpModule {
  id: string;
  name: string;
  icon: string;
  description: string;
  topics: string[];
  route?: string;
}

interface FAQ {
  question: string;
  answer: string;
  category: string;
  expanded?: boolean;
}

@Component({
  selector: 'app-help-dashboard',
  standalone: true,
  imports: [CommonModule, RouterModule, FormsModule, HeaderComponent, SidebarComponent],
  templateUrl: './help-dashboard.component.html',
  styleUrl: './help-dashboard.component.scss'
})
export class HelpDashboardComponent implements OnInit {
  // PowerPoint Manual Link
  manualUrl = 'https://amanaholdings-my.sharepoint.com/:p:/g/personal/asmaa_hassan_amanabuildings_com/IQBflonM2AphTbRMgTY23214Ad1Ht3BDJqhdzgEAJQI0wf4?e=54bI6q';

  searchTerm = '';
  selectedCategory = 'all';

  // Navigation sections state (expanded/collapsed)
  navSections: { [key: string]: boolean } = {
    'about': false,
    'system': true, // Current page, expanded by default
    'modules': false
  };

  modules: HelpModule[] = [
    {
      id: 'projects',
      name: 'Projects',
      icon: '📁',
      description: 'Create and manage construction projects',
      topics: ['Create Project', 'Edit Details', 'Project Dashboard', 'Documents', 'Timeline'],
      route: '/projects'
    },
    {
      id: 'quality',
      name: 'Quality Control',
      icon: '✓',
      description: 'Stage, RFI, and quality inspections',
      topics: ['Create Stage', 'Submit RFI', 'Inspections', 'Approvals', 'Reports'],
      route: '/quality-control'
    },
    {
      id: 'teams',
      name: 'Teams',
      icon: '👥',
      description: 'Manage project teams and assignments',
      topics: ['Create Team', 'Add Members', 'Assign Tasks', 'Team Performance'],
      route: '/teams'
    },
    {
      id: 'materials',
      name: 'Materials',
      icon: '📦',
      description: 'Track materials inventory and usage',
      topics: ['Material Catalog', 'Stock Management', 'Orders', 'Consumption'],
      route: '/materials'
    },
    {
      id: 'cost',
      name: 'Cost Management',
      icon: '💰',
      description: 'Track project costs and budgets',
      topics: ['Cost Codes', 'HR Costs', 'Budget Tracking', 'Cost Reports'],
      route: '/cost'
    },
    {
      id: 'schedule',
      name: 'Schedule Activities',
      icon: '📅',
      description: 'Plan and track construction activities',
      topics: ['Create Activity', 'Assign Teams', 'Assign Materials', 'Progress Tracking'],
      route: '/schedule'
    },
    {
      id: 'reports',
      name: 'Reports',
      icon: '📊',
      description: 'Generate project reports and analytics',
      topics: ['Progress Reports', 'Cost Reports', 'Quality Reports', 'Export PDF'],
      route: '/reports'
    },
    {
      id: 'factories',
      name: 'Factories',
      icon: '🏭',
      description: 'Manage factory layouts and production',
      topics: ['Factory Setup', 'Layout Design', 'Production Tracking'],
      route: '/factories'
    },
    {
      id: 'bim',
      name: 'BIM',
      icon: '🏗️',
      description: 'BIM models with 5D and 4D capabilities',
      topics: ['Upload Models', 'BIM 5D (Quantity)', 'BIM 4D (Schedule)', '3D Viewer'],
      route: '/bim'
    },
    {
      id: 'admin',
      name: 'Admin',
      icon: '⚙️',
      description: 'System administration and settings',
      topics: ['User Management', 'Roles & Permissions', 'System Settings', 'Audit Logs'],
      route: '/admin'
    }
  ];

  faqs: FAQ[] = [
    {
      question: 'How do I create a new project?',
      answer: 'Navigate to Projects > Click "Create Project" button > Fill in project details including name, code, client, location, and dates > Click "Save" to create the project.',
      category: 'Projects',
      expanded: false
    },
    {
      question: 'How do I submit a Work Inspection Request (WIR)?',
      answer: 'Go to Quality Control > WIR tab > Click "Create WIR" > Select project, activity, and inspection type > Fill in details > Upload photos if needed > Submit for approval.',
      category: 'Quality',
      expanded: false
    },
    {
      question: 'How do I assign a team to an activity?',
      answer: 'Go to Schedule > Select an activity > Click "Assign Team" > Choose from the dropdown list of available teams > Add notes if needed > Click "Assign Team".',
      category: 'Schedule',
      expanded: false
    },
    {
      question: 'How do I track material costs?',
      answer: 'Navigate to Cost Management > Material & Activity Costs tab > Use search and filters to find materials > View unit rates and quantities > Export to Excel if needed.',
      category: 'Cost',
      expanded: false
    },
    {
      question: 'How do I upload a BIM model?',
      answer: 'Go to BIM module > Click "Create BIM Model" > Enter model details (Category, Revit Family, Type) > Add BIM 5D (quantity) and BIM 4D (schedule) data > Save the model.',
      category: 'BIM',
      expanded: false
    },
    {
      question: 'How do I generate a project report?',
      answer: 'Navigate to Reports > Select report type (Progress, Cost, Quality) > Choose date range and filters > Click "Generate Report" > Export to PDF or Excel.',
      category: 'Reports',
      expanded: false
    },
    {
      question: 'How do I manage user permissions?',
      answer: 'Go to Admin > Users > Select a user > Click "Edit" > Assign roles (System Admin, Project Manager, etc.) > Specific permissions are automatically assigned based on role.',
      category: 'Admin',
      expanded: false
    },
    {
      question: 'Can I work offline?',
      answer: 'Currently, DuBox requires an internet connection for most features. We are working on offline capabilities for mobile inspections.',
      category: 'General',
      expanded: false
    },
    {
      question: 'How do I edit project details?',
      answer: 'Navigate to Projects > Select the project > Click "Edit Details" > Update required fields > Click "Save" to apply changes.',
      category: 'Projects',
      expanded: false
    },
    {
      question: 'How do I create teams?',
      answer: 'Go to Teams module > Click "Create Team" > Enter team name and details > Add team members > Assign team leader > Click "Save Team".',
      category: 'Schedule',
      expanded: false
    },
    {
      question: 'How do I manage material inventory?',
      answer: 'Navigate to Materials > Select material catalog > Add or update materials > Set stock levels and unit prices > Track consumption and orders.',
      category: 'Cost',
      expanded: false
    },
    {
      question: 'What BIM formats are supported?',
      answer: 'DuBox supports Revit families and models. You can upload BIM 5D data for quantities and BIM 4D data for scheduling information.',
      category: 'BIM',
      expanded: false
    },
    {
      question: 'How do I export reports?',
      answer: 'After generating a report, click the "Export" button > Choose format (PDF or Excel) > Select export options > Click "Download" to save the report.',
      category: 'Reports',
      expanded: false
    },
    {
      question: 'How do I reset user passwords?',
      answer: 'Go to Admin > Users > Select the user > Click "Reset Password" > Send reset link via email or set a temporary password.',
      category: 'Admin',
      expanded: false
    }
  ];

  ngOnInit(): void {
    // Initialization if needed
  }

  openManual(): void {
    window.open(this.manualUrl, '_blank');
  }

  navigateToModule(route?: string): void {
    if (route) {
      window.location.href = route;
    }
  }

  toggleFAQ(faq: FAQ): void {
    faq.expanded = !faq.expanded;
  }

  getFilteredModules(): HelpModule[] {
    if (!this.searchTerm) {
      return this.modules;
    }
    
    const search = this.searchTerm.toLowerCase();
    return this.modules.filter(m => 
      m.name.toLowerCase().includes(search) ||
      m.description.toLowerCase().includes(search) ||
      m.topics.some(t => t.toLowerCase().includes(search))
    );
  }

  getFilteredFAQs(): FAQ[] {
    let filtered = this.faqs;

    if (this.selectedCategory !== 'all') {
      filtered = filtered.filter(f => f.category === this.selectedCategory);
    }

    if (this.searchTerm) {
      const search = this.searchTerm.toLowerCase();
      filtered = filtered.filter(f => 
        f.question.toLowerCase().includes(search) ||
        f.answer.toLowerCase().includes(search)
      );
    }

    return filtered;
  }

  getFAQCategories(): string[] {
    const categories = new Set(this.faqs.map(f => f.category));
    return Array.from(categories);
  }

  toggleNavSection(section: string): void {
    this.navSections[section] = !this.navSections[section];
  }

  scrollToSection(sectionId: string): void {
    const element = document.getElementById(sectionId);
    if (element) {
      element.scrollIntoView({ behavior: 'smooth', block: 'start' });
    }
  }
}



