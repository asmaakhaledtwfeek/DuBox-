export interface MaterialTemplate {
  materialTemplateId: string;
  templateName: string;
  templateCode: string;
  description?: string;
  category?: string;
  isActive: boolean;
  itemCount: number;
  items?: MaterialTemplateItem[];
  createdDate: Date;
  createdBy?: string;
  modifiedDate?: Date;
  modifiedBy?: string;
}

export interface MaterialTemplateItem {
  materialTemplateItemId: string;
  materialId: string;
  materialCode: string;
  materialName: string;
  materialCategory?: string;
  requiredBeforeDays: number;
  isRequired: boolean;
  notes?: string;
  displayOrder: number;
}

export interface ProjectMaterialTemplate {
  projectMaterialTemplateId: string;
  projectId: string;
  materialTemplateId: string;
  template: MaterialTemplate;
  templateName?: string;
  templateCode?: string;
  category?: string;
  itemCount?: number;
  assignedDate: Date;
  assignedBy?: string;
}

export interface BoxTypeMaterialTemplate {
  boxTypeMaterialTemplateId: string;
  projectBoxTypeId: number;
  boxTypeName: string;
  materialTemplateId: string;
  template: MaterialTemplate;
  templateName?: string;
  templateCode?: string;
  category?: string;
  itemCount?: number;
  assignedDate: Date;
  assignedBy?: string;
}

export interface CreateMaterialTemplateRequest {
  templateName: string;
  templateCode: string;
  description?: string;
  category?: string;
  items: MaterialTemplateItemDto[];
}

export interface MaterialTemplateItemDto {
  materialId: string;
  requiredBeforeDays: number;
  isRequired: boolean;
  notes?: string;
  displayOrder: number;
}

export interface UpdateMaterialTemplateRequest {
  templateName: string;
  description?: string;
  category?: string;
  items: MaterialTemplateItemDto[];
}

export interface AssignToProjectRequest {
  projectId: string;
}

export interface AssignToBoxTypeRequest {
  projectBoxTypeId: number;
}

