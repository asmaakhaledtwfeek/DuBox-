export interface ProjectMaterialDto {
  projectMaterialId: string;
  projectId: string;
  materialId: string;
  materialCode: string;
  materialName: string;
  materialCategory?: string;
  defaultRequiredBeforeDays: number;
  isSelected: boolean;
}






