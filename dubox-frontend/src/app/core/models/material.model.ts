export interface Material {
  materialId: string;
  materialCode: string;
  materialName: string;
  materialCategory?: string;
  unit?: string;
  unitCost?: number;
  currentStock?: number;
  minimumStock?: number;
  reorderLevel?: number;
  supplierName?: string;
  isActive: boolean;
  isLowStock: boolean;
  needsReorder: boolean;
}

export interface CreateMaterial {
  materialCode: string;
  materialName: string;
  materialCategory?: string;
  unit?: string;
  unitCost?: number;
  currentStock?: number;
  minimumStock?: number;
  reorderLevel?: number;
  supplierName?: string;
}

export interface UpdateMaterial {
  materialId: string;
  materialCode?: string;
  materialName?: string;
  materialCategory?: string;
  unit?: string;
  unitCost?: number;
  minimumStock?: number;
  reorderLevel?: number;
  supplierName?: string;
  isActive?: boolean;
}

export interface RestockMaterial {
  materialId: string;
  quantity: number;
  reference?: string;
  remarks?: string;
}

export interface MaterialTransaction {
  transactionId: string;
  materialId: string;
  materialCode: string;
  materialName: string;
  quantity?: number;
  transactionDate: Date;
  transactionType?: string;
  reference?: string;
  remarks?: string;
  performedById?: string;
  performedByUserName?: string;
  boxId?: string;
  boxCode?: string;
  boxActivityId?: string;
  activityCode?: string;
  activityName?: string;
}

export interface MaterialDetails {
  material: Material;
  transactions: MaterialTransaction[];
}

export enum MaterialTransactionType {
  Receipt = 'Receipt',
  Issue = 'Issue',
  Adjustment = 'Adjustment',
  Return = 'Return'
}


