import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router, RouterModule } from '@angular/router';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { TeamService } from '../../../core/services/team.service';
import { CreateTeam } from '../../../core/models/team.model';
import { Department } from '../../../core/models/team.model';
import { HeaderComponent } from '../../../shared/components/header/header.component';
import { SidebarComponent } from '../../../shared/components/sidebar/sidebar.component';

@Component({
  selector: 'app-create-team',
  standalone: true,
  imports: [CommonModule, RouterModule, ReactiveFormsModule, HeaderComponent, SidebarComponent],
  templateUrl: './create-team.component.html',
  styleUrls: ['./create-team.component.scss']
})
export class CreateTeamComponent implements OnInit {
  teamForm!: FormGroup;
  loading = false;
  error = '';
  successMessage = '';
  departments: Department[] = [];
  loadingDepartments = false;

  trades = ['Assembly', 'Mechanical', 'Electrical', 'Finishing', 'Structural', 'MEP', 'Other'];

  constructor(
    private fb: FormBuilder,
    private teamService: TeamService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.teamForm = this.fb.group({
      teamCode: ['', [Validators.required, Validators.maxLength(50)]],
      teamName: ['', [Validators.required, Validators.maxLength(200)]],
      departmentId: ['', [Validators.required]],
      trade: ['']
    });

    this.loadDepartments();
  }

  loadDepartments(): void {
    this.loadingDepartments = true;
    this.teamService.getDepartments().subscribe({
      next: (departments) => {
        this.departments = departments.filter(d => d.isActive);
        this.loadingDepartments = false;
      },
      error: (err) => {
        this.error = 'Failed to load departments';
        this.loadingDepartments = false;
        console.error('Error loading departments:', err);
      }
    });
  }

  onSubmit(): void {
    if (this.teamForm.invalid) {
      this.markFormGroupTouched(this.teamForm);
      return;
    }

    this.loading = true;
    this.error = '';
    this.successMessage = '';

    const formValue = this.teamForm.value;
    const teamData: CreateTeam = {
      teamCode: formValue.teamCode.trim(),
      teamName: formValue.teamName.trim(),
      departmentId: formValue.departmentId,
      trade: formValue.trade || undefined
    };

    this.teamService.createTeam(teamData).subscribe({
      next: (team) => {
        this.loading = false;
        this.successMessage = 'Team created successfully!';
        setTimeout(() => {
          this.router.navigate(['/teams']);
        }, 1500);
      },
      error: (err) => {
        this.loading = false;
        this.error = err.error?.message || err.message || 'Failed to create team. Please try again.';
        console.error('Error creating team:', err);
      }
    });
  }

  onCancel(): void {
    this.router.navigate(['/teams']);
  }

  private markFormGroupTouched(formGroup: FormGroup): void {
    Object.keys(formGroup.controls).forEach(key => {
      const control = formGroup.get(key);
      control?.markAsTouched();
    });
  }
}


