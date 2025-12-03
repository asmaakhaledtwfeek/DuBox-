import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators, AbstractControl, ValidationErrors } from '@angular/forms';
import { AuthService } from '../../core/services/auth.service';
import { HeaderComponent } from '../../shared/components/header/header.component';
import { SidebarComponent } from '../../shared/components/sidebar/sidebar.component';
import { User } from '../../core/models/user.model';

@Component({
  selector: 'app-user-profile',
  standalone: true,
  imports: [CommonModule, RouterModule, ReactiveFormsModule, HeaderComponent, SidebarComponent],
  templateUrl: './user-profile.component.html',
  styleUrls: ['./user-profile.component.scss']
})
export class UserProfileComponent implements OnInit {
  currentUser: User | null = null;
  changePasswordForm!: FormGroup;
  isChangingPassword = false;
  passwordChangeError = '';
  passwordChangeSuccess = false;
  showCurrentPassword = false;
  showNewPassword = false;
  showConfirmPassword = false;

  constructor(
    private authService: AuthService,
    private fb: FormBuilder
  ) {
    this.changePasswordForm = this.fb.group({
      currentPassword: ['', [Validators.required]],
      newPassword: ['', [
        Validators.required,
        Validators.minLength(8),
        this.passwordStrengthValidator
      ]],
      confirmPassword: ['', [Validators.required]]
    }, {
      validators: this.passwordMatchValidator
    });
  }

  ngOnInit(): void {
    this.authService.authState$.subscribe(state => {
      this.currentUser = state.user;
    });
  }

  passwordStrengthValidator(control: AbstractControl): ValidationErrors | null {
    const value = control.value;
    if (!value) return null;

    const hasUpperCase = /[A-Z]/.test(value);
    const hasLowerCase = /[a-z]/.test(value);
    const hasNumeric = /[0-9]/.test(value);
    const hasSpecialChar = /[!@#$%^&*(),.?":{}|<>]/.test(value);

    const errors: ValidationErrors = {};
    if (!hasUpperCase) errors['noUpperCase'] = true;
    if (!hasLowerCase) errors['noLowerCase'] = true;
    if (!hasNumeric) errors['noNumeric'] = true;
    if (!hasSpecialChar) errors['noSpecialChar'] = true;

    return Object.keys(errors).length > 0 ? errors : null;
  }

  passwordMatchValidator(control: AbstractControl): ValidationErrors | null {
    const newPassword = control.get('newPassword');
    const confirmPassword = control.get('confirmPassword');

    if (!newPassword || !confirmPassword) return null;

    return newPassword.value === confirmPassword.value ? null : { passwordMismatch: true };
  }

  getPasswordStrengthErrors(): string[] {
    const errors: string[] = [];
    const newPasswordControl = this.changePasswordForm.get('newPassword');

    if (newPasswordControl?.errors) {
      if (newPasswordControl.errors['noUpperCase']) {
        errors.push('At least one uppercase letter');
      }
      if (newPasswordControl.errors['noLowerCase']) {
        errors.push('At least one lowercase letter');
      }
      if (newPasswordControl.errors['noNumeric']) {
        errors.push('At least one number');
      }
      if (newPasswordControl.errors['noSpecialChar']) {
        errors.push('At least one special character');
      }
      if (newPasswordControl.errors['minlength']) {
        errors.push('Minimum 8 characters');
      }
    }

    return errors;
  }

  toggleCurrentPasswordVisibility(): void {
    this.showCurrentPassword = !this.showCurrentPassword;
  }

  toggleNewPasswordVisibility(): void {
    this.showNewPassword = !this.showNewPassword;
  }

  toggleConfirmPasswordVisibility(): void {
    this.showConfirmPassword = !this.showConfirmPassword;
  }

  onChangePassword(): void {
    if (this.changePasswordForm.invalid || !this.currentUser) {
      this.markFormGroupTouched(this.changePasswordForm);
      return;
    }

    this.isChangingPassword = true;
    this.passwordChangeError = '';
    this.passwordChangeSuccess = false;

    const { currentPassword, newPassword } = this.changePasswordForm.value;

    this.authService.changePassword(
      this.currentUser!.id,
      currentPassword,
      newPassword
    ).subscribe({
      next: () => {
        this.passwordChangeSuccess = true;
        this.isChangingPassword = false;
        this.changePasswordForm.reset();
        this.showCurrentPassword = false;
        this.showNewPassword = false;
        this.showConfirmPassword = false;

        // Show success message
        document.dispatchEvent(new CustomEvent('app-toast', {
          detail: {
            message: 'Password changed successfully!',
            type: 'success'
          }
        }));

        // Hide success message after 5 seconds
        setTimeout(() => {
          this.passwordChangeSuccess = false;
        }, 5000);
      },
      error: (err) => {
        this.isChangingPassword = false;
        this.passwordChangeError = err?.error?.message || err?.message || 'Failed to change password. Please check your current password and try again.';
      }
    });
  }

  markFormGroupTouched(formGroup: FormGroup): void {
    Object.keys(formGroup.controls).forEach(key => {
      const control = formGroup.get(key);
      control?.markAsTouched();

      if (control instanceof FormGroup) {
        this.markFormGroupTouched(control);
      }
    });
  }

  getFieldError(fieldName: string): string {
    const control = this.changePasswordForm.get(fieldName);
    if (!control || !control.errors || !control.touched) return '';

    if (control.errors['required']) {
      return `${this.getFieldLabel(fieldName)} is required`;
    }
    if (control.errors['minlength']) {
      return `${this.getFieldLabel(fieldName)} must be at least ${control.errors['minlength'].requiredLength} characters`;
    }
    if (fieldName === 'confirmPassword' && this.changePasswordForm.errors?.['passwordMismatch']) {
      return 'Passwords do not match';
    }

    return '';
  }

  getFieldLabel(fieldName: string): string {
    const labels: Record<string, string> = {
      currentPassword: 'Current password',
      newPassword: 'New password',
      confirmPassword: 'Confirm password'
    };
    return labels[fieldName] || fieldName;
  }
}

