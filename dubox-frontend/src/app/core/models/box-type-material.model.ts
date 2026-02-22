export interface BoxTypeMaterial {
  boxTypeMaterialId: string;
  projectBoxTypeId: number;
  boxTypeName: string;
  boxCount: number; // Number of boxes under this box type
  materialId: string;
  materialCode: string;
  materialName: string;
  materialCategory?: string;
  unit?: string;
  quantityPerBox?: number; // Editable quantity per box
  requiredBeforeDays: number;
  deliveryProgress: number;
  isArrived: boolean;
  arrivedDate?: Date;
  arrivedBy?: string;
  arrivedByName?: string;
  arrivedQuantity?: number; // Same as deliveredQuantity
  deliveredQuantity?: number; // User enters when marking as delivered
  notes?: string;
  createdDate: Date;
}

export interface MarkMaterialArrivedRequest {
  deliveryProgress?: number;
  arrivedQuantity?: number;
  deliveredQuantity?: number;
  notes?: string;
}






