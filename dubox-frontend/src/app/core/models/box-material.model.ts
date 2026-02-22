export interface BoxMaterial {
  boxMaterialId: string;
  boxId: string;
  boxTag?: string;
  materialId: string;
  materialCode: string;
  materialName: string;
  materialCategory?: string;
  requiredBeforeDays: number;
  requiredByDate: Date;
  isArrived: boolean;
  arrivedDate?: Date;
  arrivedByUserName?: string;
  qualityIssueId?: string;
  daysUntilRequired: number;
  isOverdue: boolean;
  status: 'Arrived' | 'Overdue' | 'Approaching' | 'Pending';
  deliveredQuantity?: number;
  quantityPerBox?: number;
}

export interface ProjectMaterialSelection {
  projectId: string;
  materialIds: string[];
  selectAll: boolean;
}

