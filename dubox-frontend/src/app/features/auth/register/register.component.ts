import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators, ValidatorFn, AbstractControl, ValidationErrors } from '@angular/forms';
import { Router, RouterModule } from '@angular/router';
import { AuthService } from '../../../core/services/auth.service';
import { DepartmentService } from '../../../core/services/department.service';
import { Department } from '../../../core/models/team.model';
import { RegisterRequest } from '@core/models/user.model';

const disallowValueValidator = (value: string): ValidatorFn => {
  return (control: AbstractControl): ValidationErrors | null => {
    if (!control.value) {
      return null;
    }

    return control.value?.toString().trim().toLowerCase() === value.toLowerCase()
      ? { disallowedValue: true }
      : null;
  };
};

const passwordStrengthValidator: ValidatorFn = (control: AbstractControl): ValidationErrors | null => {
  const value = control.value as string;
  if (!value) {
    return null;
  }

  const errors: ValidationErrors = {};
  if (!/[A-Z]/.test(value)) {
    errors['requireUppercase'] = true;
  }
  if (!/[a-z]/.test(value)) {
    errors['requireLowercase'] = true;
  }
  if (!/[0-9]/.test(value)) {
    errors['requireNumber'] = true;
  }

  return Object.keys(errors).length ? errors : null;
};

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, RouterModule],
  templateUrl: './register.component.html',
  styleUrl: './register.component.scss'
})
export class RegisterComponent implements OnInit {
  registerForm: FormGroup;
  loading = false;
  serverError = '';
  showPassword = false;

  departments: Department[] = [];
  departmentsLoading = false;
  departmentsError = '';

  constructor(
    private fb: FormBuilder,
    private authService: AuthService,
    private departmentService: DepartmentService,
    private router: Router
  ) {
    this.registerForm = this.fb.group({
      fullName: ['', [Validators.required, disallowValueValidator('string')]],
      email: ['', [Validators.required, Validators.email, Validators.maxLength(100)]],
      password: [
        '',
        [
          Validators.required,
          Validators.minLength(8),
          Validators.maxLength(50),
          passwordStrengthValidator
        ]
      ],
      departmentId: ['', [Validators.required]]
    });
  }

  ngOnInit(): void {
    this.loadDepartments();
  }

  get f() {
    return this.registerForm.controls;
  }

  togglePasswordVisibility(): void {
    this.showPassword = !this.showPassword;
  }

  loadDepartments(): void {
    this.departmentsLoading = true;
    this.departmentsError = '';

    this.departmentService.getDepartments().subscribe({
      next: (departments) => {
        this.departments = (departments || []).sort((a, b) =>
          a.departmentName.localeCompare(b.departmentName)
        );
        this.departmentsLoading = false;
      },
      error: () => {
        this.departmentsError = 'Unable to load departments. Please try again.';
        this.departmentsLoading = false;
      }
    });
  }

  onSubmit(): void {
    if (this.registerForm.invalid) {
      this.registerForm.markAllAsTouched();
      return;
    }

    this.loading = true;
    this.serverError = '';

    const payload = this.registerForm.value as RegisterRequest;

    this.authService.register(payload).subscribe({
      next: () => {
        this.loading = false;
        document.dispatchEvent(
          new CustomEvent('app-toast', {
            detail: {
              message: 'Registration successful. Please sign in to continue.',
              type: 'success'
            }
          })
        );
        this.router.navigate(['/login'], {
          queryParams: { email: payload.email }
        });
      },
      error: (error) => {
        this.serverError =
          error?.error?.message || 'Registration failed. Please review your entries and try again.';
        this.loading = false;
      }
    });
  }
}



