import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router, RouterModule, ActivatedRoute } from '@angular/router';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { MaterialService } from '../../../core/services/material.service';
import { Material, MaterialTransaction, RestockMaterial } from '../../../core/models/material.model';
import { HeaderComponent } from '../../../shared/components/header/header.component';
import { SidebarComponent } from '../../../shared/components/sidebar/sidebar.component';

@Component({
  selector: 'app-material-details',
  standalone: true,
  imports: [CommonModule, RouterModule, ReactiveFormsModule, HeaderComponent, SidebarComponent],
  templateUrl: './material-details.component.html',
  styleUrls: ['./material-details.component.scss']
})
export class MaterialDetailsComponent implements OnInit {
  material: Material | null = null;
  transactions: MaterialTransaction[] = [];
  materialId: string = '';
  loading = true;
  error = '';
  activeTab: 'overview' | 'transactions' = 'overview';
  
  showRestockModal = false;
  restockForm!: FormGroup;
  restocking = false;
  restockMessage = '';
  restockMessageType: 'success' | 'error' | null = null;

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private materialService: MaterialService,
    private fb: FormBuilder
  ) {}

  ngOnInit(): void {
    this.materialId = this.route.snapshot.params['id'];
    
    this.restockForm = this.fb.group({
      quantity: [null, [Validators.required, Validators.min(0.01)]],
      reference: [''],
      remarks: ['']
    });
    
    this.loadMaterial();
    this.loadTransactions();
  }

  loadMaterial(): void {
    this.materialService.getMaterialById(this.materialId).subscribe({
      next: (material) => {
        this.material = material;
        this.loading = false;
      },
      error: (err) => {
        this.error = err.error?.message || err.message || 'Failed to load material';
        this.loading = false;
        console.error('Error loading material:', err);
      }
    });
  }

  loadTransactions(): void {
    this.materialService.getMaterialTransactions(this.materialId).subscribe({
      next: (transactions) => {
        this.transactions = transactions.sort((a, b) => 
          new Date(b.transactionDate).getTime() - new Date(a.transactionDate).getTime()
        );
      },
      error: (err) => {
        console.error('Error loading transactions:', err);
      }
    });
  }

  openRestockModal(): void {
    this.restockForm.reset({
      quantity: null,
      reference: '',
      remarks: ''
    });
    this.restockMessage = '';
    this.restockMessageType = null;
    this.showRestockModal = true;
  }

  closeRestockModal(): void {
    this.showRestockModal = false;
    this.restockForm.reset();
    this.restockMessage = '';
    this.restockMessageType = null;
  }

  onRestock(): void {
    if (this.restockForm.invalid) {
      this.markFormGroupTouched(this.restockForm);
      return;
    }

    this.restocking = true;
    const formValue = this.restockForm.value;
    
    const restockData: RestockMaterial = {
      materialId: this.materialId,
      quantity: parseFloat(formValue.quantity),
      reference: formValue.reference?.trim() || undefined,
      remarks: formValue.remarks?.trim() || undefined
    };

    this.restockMessage = '';
    this.restockMessageType = null;

    this.materialService.restockMaterial(this.materialId, restockData).subscribe({
      next: (result) => {
        this.restocking = false;
        const newStock = result.currentStock || this.material?.currentStock;
        this.restockMessage = `Material restocked successfully! New stock: ${newStock} ${this.material?.unit || ''}`;
        this.restockMessageType = 'success';
        
        // Reload material and transactions
        this.loadMaterial();
        this.loadTransactions();
        
        // Auto-close modal after 2 seconds on success
        setTimeout(() => {
          this.closeRestockModal();
        }, 2000);
      },
      error: (err) => {
        this.restocking = false;
        this.restockMessage = err.error?.message || err.message || 'Failed to restock material. Please try again.';
        this.restockMessageType = 'error';
        console.error('Error restocking material:', err);
      }
    });
  }

  editMaterial(): void {
    this.router.navigate(['/materials', this.materialId, 'edit']);
  }

  backToList(): void {
    this.router.navigate(['/materials']);
  }

  setActiveTab(tab: 'overview' | 'transactions'): void {
    this.activeTab = tab;
  }

  getStockStatusClass(): string {
    if (!this.material) return '';
    if (this.material.needsReorder) return 'status-critical';
    if (this.material.isLowStock) return 'status-warning';
    return 'status-ok';
  }

  getStockStatusText(): string {
    if (!this.material) return '';
    if (this.material.needsReorder) return 'Critical';
    if (this.material.isLowStock) return 'Low';
    return 'OK';
  }

  formatDate(date: Date | string): string {
    if (!date) return '-';
    const d = new Date(date);
    if (isNaN(d.getTime())) return '-';
    return d.toLocaleDateString('en-US', { 
      year: 'numeric', 
      month: 'short', 
      day: 'numeric',
      hour: '2-digit',
      minute: '2-digit'
    });
  }

  getTransactionTypeClass(type?: string): string {
    if (!type) return '';
    switch (type.toLowerCase()) {
      case 'receipt': return 'type-receipt';
      case 'issue': return 'type-issue';
      case 'adjustment': return 'type-adjustment';
      case 'return': return 'type-return';
      default: return '';
    }
  }

  private markFormGroupTouched(formGroup: FormGroup): void {
    Object.keys(formGroup.controls).forEach(key => {
      const control = formGroup.get(key);
      control?.markAsTouched();
    });
  }
}

