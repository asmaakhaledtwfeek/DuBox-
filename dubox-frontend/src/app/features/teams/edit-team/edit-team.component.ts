import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router, RouterModule, ActivatedRoute } from '@angular/router';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { TeamService } from '../../../core/services/team.service';
import { UpdateTeam, Team, Department } from '../../../core/models/team.model';
import { HeaderComponent } from '../../../shared/components/header/header.component';
import { SidebarComponent } from '../../../shared/components/sidebar/sidebar.component';

@Component({
  selector: 'app-edit-team',
  standalone: true,
  imports: [CommonModule, RouterModule, ReactiveFormsModule, HeaderComponent, SidebarComponent],
  templateUrl: './edit-team.component.html',
  styleUrls: ['./edit-team.component.scss']
})
export class EditTeamComponent implements OnInit {
  teamForm!: FormGroup;
  teamId: string = '';
  loading = true;
  saving = false;
  error = '';
  successMessage = '';
  departments: Department[] = [];
  loadingDepartments = false;
  teamHasMembers = false;
  originalDepartmentId: string = '';

  trades = ['Assembly', 'Mechanical', 'Electrical', 'Finishing', 'Structural', 'MEP', 'Other'];

  constructor(
    private fb: FormBuilder,
    private teamService: TeamService,
    private router: Router,
    private route: ActivatedRoute
  ) {}

  ngOnInit(): void {
    this.teamId = this.route.snapshot.params['id'];
    
    this.teamForm = this.fb.group({
      teamCode: ['', [Validators.required, Validators.maxLength(50)]],
      teamName: ['', [Validators.required, Validators.maxLength(200)]],
      departmentId: ['', [Validators.required]],
      trade: [''],
      isActive: [true]
    });

    // Load departments first, then load team to ensure department dropdown is populated
    this.teamService.getDepartments().subscribe({
      next: (departments) => {
        this.departments = departments.filter(d => d.isActive);
        this.loadingDepartments = false;
        // Now load team data after departments are loaded
        this.loadTeam();
      },
      error: (err) => {
        const errorMsg = err.error?.message || err.message || 'Failed to load departments';
        if (!this.error) {
          this.error = errorMsg;
        }
        this.loadingDepartments = false;
        console.error('Error loading departments:', err);
        // Even if departments fail, still try to load team
        this.loadTeam();
      }
    });
  }


  loadTeam(): void {
    this.loading = true;
    this.teamService.getTeamById(this.teamId).subscribe({
      next: (team) => {
        console.log('📋 Loaded team:', team);
        console.log('📋 Team departmentId:', team.departmentId);
        console.log('📋 Available departments:', this.departments);
        
        // Convert departmentId to string
        const departmentIdStr = team.departmentId ? String(team.departmentId) : '';
        this.originalDepartmentId = departmentIdStr;
        this.teamHasMembers = (team.teamSize || 0) > 0;
        
        // Find matching department by comparing string values
        const matchingDept = this.departments.find(d => {
          const deptId = String(d.departmentId);
          return deptId === departmentIdStr;
        });
        
        // Use the exact departmentId from the departments array
        const finalDepartmentId = matchingDept ? String(matchingDept.departmentId) : departmentIdStr;
        
        console.log('✅ Matching department found:', matchingDept?.departmentName || 'None');
        console.log('✅ Final departmentId to set:', finalDepartmentId);
        
        // Set all form values
        this.teamForm.patchValue({
          teamCode: team.teamCode || '',
          teamName: team.teamName || '',
          departmentId: finalDepartmentId,
          trade: team.trade || '',
          isActive: team.isActive !== undefined ? team.isActive : true
        });

        // Wait a tick for Angular to update the DOM
        setTimeout(() => {
          const formControl = this.teamForm.get('departmentId');
          const formValue = formControl?.value;
          console.log('✅ Form departmentId after patchValue:', formValue);
          console.log('✅ Form departmentId type:', typeof formValue);
          console.log('✅ Expected departmentId:', finalDepartmentId);
          console.log('✅ Values match?', String(formValue) === String(finalDepartmentId));
          
          // If still not set, try setValue
          if (!formValue && finalDepartmentId) {
            console.log('⚠️ Value not set, trying setValue...');
            formControl?.setValue(finalDepartmentId, { emitEvent: false });
          }
        }, 0);

        // Disable department field if team has members (but do it after setting value)
        if (this.teamHasMembers) {
          setTimeout(() => {
            this.teamForm.get('departmentId')?.disable();
          }, 100);
        }

        this.loading = false;
      },
      error: (err) => {
        this.error = err.error?.message || err.message || 'Failed to load crew';
        this.loading = false;
        console.error('Error loading team:', err);
      }
    });
  }

  onSubmit(): void {
    if (this.teamForm.invalid) {
      this.markFormGroupTouched(this.teamForm);
      return;
    }

    this.saving = true;
    this.error = '';
    this.successMessage = '';

    const formValue = this.teamForm.value;
    
    // If team has members, don't allow department change
    const departmentId = this.teamHasMembers 
      ? this.originalDepartmentId 
      : (formValue.departmentId || undefined);

    const teamData: UpdateTeam = {
      teamId: this.teamId,
      teamCode: formValue.teamCode.trim(),
      teamName: formValue.teamName.trim(),
      departmentId: departmentId,
      trade: formValue.trade || undefined,
      isActive: formValue.isActive
    };

    this.teamService.updateTeam(this.teamId, teamData).subscribe({
      next: (team) => {
        this.saving = false;
        this.successMessage = 'Crew updated successfully!';
        setTimeout(() => {
          this.router.navigate(['/teams', this.teamId]);
        }, 1500);
      },
      error: (err) => {
        this.saving = false;
        this.error = err.error?.message || err.message || 'Failed to update crew. Please try again.';
        console.error('Error updating team:', err);
      }
    });
  }

  onCancel(): void {
    this.router.navigate(['/teams', this.teamId]);
  }

  private markFormGroupTouched(formGroup: FormGroup): void {
    Object.keys(formGroup.controls).forEach(key => {
      const control = formGroup.get(key);
      control?.markAsTouched();
    });
  }
}

