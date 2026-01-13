import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { ApiService } from './api.service';
import { Material, CreateMaterial, UpdateMaterial, RestockMaterial, MaterialTransaction, MaterialDetails } from '../models/material.model';

@Injectable({
  providedIn: 'root'
})
export class MaterialService {
  private readonly endpoint = 'materials';

  constructor(private apiService: ApiService) {}

  /**
   * Get all materials
   */
  getMaterials(): Observable<Material[]> {
    return this.apiService.get<Material[]>(this.endpoint).pipe(
      map(materials => materials.map(m => this.transformMaterial(m)))
    );
  }

  /**
   * Get low stock materials
   */
  getLowStockMaterials(): Observable<Material[]> {
    return this.apiService.get<Material[]>(`${this.endpoint}/low-stock`).pipe(
      map(materials => materials.map(m => this.transformMaterial(m)))
    );
  }

  /**
   * Get material by ID
   */
  getMaterialById(materialId: string): Observable<Material> {
    return this.apiService.get<Material>(`${this.endpoint}/${materialId}`).pipe(
      map(m => this.transformMaterial(m))
    );
  }

  /**
   * Get material transactions
   */
  getMaterialTransactions(materialId: string): Observable<MaterialTransaction[]> {
    return this.apiService.get<any>(`${this.endpoint}/transactions/${materialId}`).pipe(
      map(response => {
        // Backend returns GetAllMaterialTransactionsDto with a 'transactions' property
        // Handle both success response with data.transactions and direct transactions array
        let transactions: any[] = [];
        
        if (response && typeof response === 'object') {
          // If response has transactions property (from GetAllMaterialTransactionsDto)
          transactions = response.transactions || response.Transactions || [];
          
          // If response is an array directly
          if (Array.isArray(response)) {
            transactions = response;
          }
        }
        
        return transactions.map((t: any) => this.transformTransaction(t));
      })
    );
  }

  /**
   * Create new material
   */
  createMaterial(material: CreateMaterial): Observable<Material> {
    return this.apiService.post<Material>(this.endpoint, material).pipe(
      map(m => this.transformMaterial(m))
    );
  }

  /**
   * Update material
   */
  updateMaterial(materialId: string, material: UpdateMaterial): Observable<Material> {
    return this.apiService.put<Material>(`${this.endpoint}/update/${materialId}`, {
      ...material,
      materialId
    }).pipe(
      map(m => this.transformMaterial(m))
    );
  }

  /**
   * Restock material
   */
  restockMaterial(materialId: string, restock: RestockMaterial): Observable<any> {
    return this.apiService.put<any>(`${this.endpoint}/restock/${materialId}`, {
      ...restock,
      materialId
    });
  }

  /**
   * Transform backend material to frontend model
   */
  private transformMaterial(backendMaterial: any): Material {
    return {
      materialId: backendMaterial.materialId || backendMaterial.id,
      materialCode: backendMaterial.materialCode || backendMaterial.code,
      materialName: backendMaterial.materialName || backendMaterial.name,
      materialCategory: backendMaterial.materialCategory,
      unit: backendMaterial.unit,
      unitCost: backendMaterial.unitCost,
      currentStock: backendMaterial.currentStock,
      minimumStock: backendMaterial.minimumStock,
      reorderLevel: backendMaterial.reorderLevel,
      supplierName: backendMaterial.supplierName,
      isActive: backendMaterial.isActive !== undefined ? backendMaterial.isActive : true,
      isLowStock: backendMaterial.isLowStock || false,
      needsReorder: backendMaterial.needsReorder || false
    };
  }

  /**
   * Transform backend transaction to frontend model
   */
  private transformTransaction(backendTransaction: any): MaterialTransaction {
    return {
      transactionId: backendTransaction.transactionId || backendTransaction.id,
      materialId: backendTransaction.materialId,
      materialCode: backendTransaction.materialCode || '',
      materialName: backendTransaction.materialName || '',
      quantity: backendTransaction.quantity,
      transactionDate: backendTransaction.transactionDate ? new Date(backendTransaction.transactionDate) : new Date(),
      transactionType: backendTransaction.transactionType,
      reference: backendTransaction.reference,
      remarks: backendTransaction.remarks,
      performedById: backendTransaction.performedById,
      performedByUserName: backendTransaction.performedByUserName || '',
      boxId: backendTransaction.boxId,
      boxCode: backendTransaction.boxCode,
      boxActivityId: backendTransaction.boxActivityId,
      activityCode: backendTransaction.activityCode,
      activityName: backendTransaction.activityName
    };
  }
}

