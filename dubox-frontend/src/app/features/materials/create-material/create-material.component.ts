import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router, RouterModule } from '@angular/router';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { MaterialService } from '../../../core/services/material.service';
import { CreateMaterial } from '../../../core/models/material.model';
import { HeaderComponent } from '../../../shared/components/header/header.component';
import { SidebarComponent } from '../../../shared/components/sidebar/sidebar.component';

@Component({
  selector: 'app-create-material',
  standalone: true,
  imports: [CommonModule, RouterModule, ReactiveFormsModule, HeaderComponent, SidebarComponent],
  templateUrl: './create-material.component.html',
  styleUrls: ['./create-material.component.scss']
})
export class CreateMaterialComponent implements OnInit {
  materialForm!: FormGroup;
  loading = false;
  error = '';
  successMessage = '';

  categories = ['Precast', 'MEP', 'Finishing', 'Electrical', 'Plumbing', 'Structural', 'Other'];
  units = ['pcs', 'kg', 'm', 'm²', 'm³', 'L', 'box', 'roll', 'sheet', 'bag'];

  constructor(
    private fb: FormBuilder,
    private materialService: MaterialService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.materialForm = this.fb.group({
      materialCode: ['', [Validators.required, Validators.maxLength(50)]],
      materialName: ['', [Validators.required, Validators.maxLength(200)]],
      materialCategory: [''],
      unit: [''],
      unitCost: [null, [Validators.min(0)]],
      currentStock: [null, [Validators.min(0)]],
      minimumStock: [null, [Validators.min(0)]],
      reorderLevel: [null, [Validators.min(0)]],
      supplierName: ['']
    });
  }

  onSubmit(): void {
    if (this.materialForm.invalid) {
      this.markFormGroupTouched(this.materialForm);
      return;
    }

    this.loading = true;
    this.error = '';
    this.successMessage = '';

    const formValue = this.materialForm.value;
    const materialData: CreateMaterial = {
      materialCode: formValue.materialCode.trim(),
      materialName: formValue.materialName.trim(),
      materialCategory: formValue.materialCategory || undefined,
      unit: formValue.unit || undefined,
      unitCost: formValue.unitCost ? parseFloat(formValue.unitCost) : undefined,
      currentStock: formValue.currentStock ? parseFloat(formValue.currentStock) : undefined,
      minimumStock: formValue.minimumStock ? parseFloat(formValue.minimumStock) : undefined,
      reorderLevel: formValue.reorderLevel ? parseFloat(formValue.reorderLevel) : undefined,
      supplierName: formValue.supplierName?.trim() || undefined
    };

    this.materialService.createMaterial(materialData).subscribe({
      next: (material) => {
        this.loading = false;
        this.successMessage = 'Material created successfully!';
        setTimeout(() => {
          this.router.navigate(['/materials']);
        }, 1500);
      },
      error: (err) => {
        this.loading = false;
        this.error = err.error?.message || err.message || 'Failed to create material. Please try again.';
        console.error('Error creating material:', err);
      }
    });
  }

  onCancel(): void {
    this.router.navigate(['/materials']);
  }

  private markFormGroupTouched(formGroup: FormGroup): void {
    Object.keys(formGroup.controls).forEach(key => {
      const control = formGroup.get(key);
      control?.markAsTouched();
    });
  }
}


