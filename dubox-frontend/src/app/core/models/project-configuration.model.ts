export interface ProjectBuilding {
  id?: number;
  projectId?: string;
  buildingCode: string;
  buildingName?: string;
  displayOrder?: number;
  isActive?: boolean;
}

export interface ProjectLevel {
  id?: number;
  projectId?: string;
  levelCode: string;
  levelName?: string;
  displayOrder?: number;
  isActive?: boolean;
}

export interface ProjectBoxType {
  id?: number;
  projectId?: string;
  typeName: string;
  abbreviation?: string;
  hasSubTypes?: boolean;
  displayOrder?: number;
  isActive?: boolean;
  subTypes?: ProjectBoxSubType[];
}

export interface ProjectBoxSubType {
  id?: number;
  projectBoxTypeId?: number;
  subTypeName: string;
  abbreviation?: string;
  displayOrder?: number;
  isActive?: boolean;
}

export interface ProjectZone {
  id?: number;
  projectId?: string;
  zoneCode: string;
  zoneName?: string;
  displayOrder?: number;
  isActive?: boolean;
}

export interface ProjectBoxFunction {
  id?: number;
  projectId?: string;
  functionName: string;
  description?: string;
  displayOrder?: number;
  isActive?: boolean;
}

export interface ProjectConfiguration {
  projectId: string;
  buildings: ProjectBuilding[];
  levels: ProjectLevel[];
  boxTypes: ProjectBoxType[];
  zones: ProjectZone[];
  boxFunctions: ProjectBoxFunction[];
}

