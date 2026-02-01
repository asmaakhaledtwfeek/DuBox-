import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router, RouterModule, ActivatedRoute } from '@angular/router';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { MaterialService } from '../../../core/services/material.service';
import { UpdateMaterial, Material } from '../../../core/models/material.model';
import { HeaderComponent } from '../../../shared/components/header/header.component';
import { SidebarComponent } from '../../../shared/components/sidebar/sidebar.component';

@Component({
  selector: 'app-edit-material',
  standalone: true,
  imports: [CommonModule, RouterModule, ReactiveFormsModule, HeaderComponent, SidebarComponent],
  templateUrl: './edit-material.component.html',
  styleUrls: ['./edit-material.component.scss']
})
export class EditMaterialComponent implements OnInit {
  materialForm!: FormGroup;
  materialId: string = '';
  loading = true;
  saving = false;
  error = '';
  successMessage = '';

  categories = ['Precast', 'MEP', 'Finishing', 'Electrical', 'Plumbing', 'Structural', 'Other'];
  units = ['pcs', 'kg', 'm', 'm²', 'm³', 'L', 'box', 'roll', 'sheet', 'bag'];

  constructor(
    private fb: FormBuilder,
    private materialService: MaterialService,
    private router: Router,
    private route: ActivatedRoute
  ) {}

  ngOnInit(): void {
    this.materialId = this.route.snapshot.params['id'];
    
    this.materialForm = this.fb.group({
      materialCode: ['', [Validators.required, Validators.maxLength(50)]],
      materialName: ['', [Validators.required, Validators.maxLength(200)]],
      materialCategory: [''],
      unit: [''],
      unitCost: [null, [Validators.min(0)]],
      minimumStock: [null, [Validators.min(0)]],
      reorderLevel: [null, [Validators.min(0)]],
      supplierName: [''],
      isActive: [true]
    });

    this.loadMaterial();
  }

  loadMaterial(): void {
    this.loading = true;
    this.materialService.getMaterialById(this.materialId).subscribe({
      next: (material) => {
        this.materialForm.patchValue({
          materialCode: material.materialCode,
          materialName: material.materialName,
          materialCategory: material.materialCategory || '',
          unit: material.unit || '',
          unitCost: material.unitCost || null,
          minimumStock: material.minimumStock || null,
          reorderLevel: material.reorderLevel || null,
          supplierName: material.supplierName || '',
          isActive: material.isActive
        });
        this.loading = false;
      },
      error: (err) => {
        this.error = err.error?.message || err.message || 'Failed to load material';
        this.loading = false;
        console.error('Error loading material:', err);
      }
    });
  }

  onSubmit(): void {
    if (this.materialForm.invalid) {
      this.markFormGroupTouched(this.materialForm);
      return;
    }

    this.saving = true;
    this.error = '';
    this.successMessage = '';

    const formValue = this.materialForm.value;
    const materialData: UpdateMaterial = {
      materialId: this.materialId,
      materialCode: formValue.materialCode.trim(),
      materialName: formValue.materialName.trim(),
      materialCategory: formValue.materialCategory || undefined,
      unit: formValue.unit || undefined,
      unitCost: formValue.unitCost ? parseFloat(formValue.unitCost) : undefined,
      minimumStock: formValue.minimumStock ? parseFloat(formValue.minimumStock) : undefined,
      reorderLevel: formValue.reorderLevel ? parseFloat(formValue.reorderLevel) : undefined,
      supplierName: formValue.supplierName?.trim() || undefined,
      isActive: formValue.isActive
    };

    this.materialService.updateMaterial(this.materialId, materialData).subscribe({
      next: (material) => {
        this.saving = false;
        this.successMessage = 'Material updated successfully!';
        setTimeout(() => {
          this.router.navigate(['/materials', this.materialId]);
        }, 1500);
      },
      error: (err) => {
        this.saving = false;
        this.error = err.error?.message || err.message || 'Failed to update material. Please try again.';
        console.error('Error updating material:', err);
      }
    });
  }

  onCancel(): void {
    this.router.navigate(['/materials', this.materialId]);
  }

  private markFormGroupTouched(formGroup: FormGroup): void {
    Object.keys(formGroup.controls).forEach(key => {
      const control = formGroup.get(key);
      control?.markAsTouched();
    });
  }
}


