import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { ApiService } from './api.service';

export enum ProjectLocation {
  KSA = 1,
  UAE = 2
}

export enum FactorySectionType {
  Assembly = 1,
  Finishing = 2
}

export interface FreezingCell {
  freezingCellId?: string;
  factorySectionPartId: string;
  rowNumber: number;
}

export interface FactorySectionPart {
  partId?: string;
  sectionId?: string;
  partNumber: number;
  partName: string;
  minRow: number;
  maxRow: number;
  minBay: string;
  maxBay: string;
  capacity?: number;
  currentOccupancy?: number;
  availableCapacity?: number;
  isFull?: boolean;
  isActive?: boolean;
  rowCount?: number;
  bayCount?: number;
  freezingCells?: FreezingCell[];
}

export interface FactorySection {
  sectionId?: string;
  factoryId?: string;
  sectionType: FactorySectionType;
  sectionName: string;
  minRow: number;
  maxRow: number;
  minBay: string;
  maxBay: string;
  capacity?: number;
  currentOccupancy?: number;
  availableCapacity?: number;
  isFull?: boolean;
  isActive?: boolean;
  rowCount?: number;
  bayCount?: number;
  displayOrder?: number;
  parts?: FactorySectionPart[]; // Parts within this section
}

export interface Factory {
  factoryId: string;
  factoryCode: string;
  factoryName: string;
  location: ProjectLocation;
  capacity?: number;
  minRow?: number;
  maxRow?: number;
  minBay?: string;
  maxBay?: string;
  currentOccupancy: number;
  availableCapacity: number;
  isFull: boolean;
  isActive: boolean;
  dispatchedBoxesCount?: number; // Count of dispatched boxes in the factory
  sections?: FactorySection[]; // Factory sections (Assembly, Finishing)
}

export interface CreateFactorySectionRequest {
  sectionType: FactorySectionType;
  sectionName: string;
  minRow: number;
  maxRow: number;
  minBay: string;
  maxBay: string;
}

export interface CreateFactoryRequest {
  factoryCode: string;
  factoryName: string;
  location: ProjectLocation;
  capacity?: number;
  minRow: number;
  maxRow: number;
  minBay: string;
  maxBay: string;
  sections?: CreateFactorySectionRequest[]; // Optional sections configuration
}

@Injectable({
  providedIn: 'root'
})
export class FactoryService {
  private readonly endpoint = 'factories';

  constructor(private apiService: ApiService) {}

  /**
   * Transform backend factory section part response to frontend model
   */
  private transformFactorySectionPart(backendPart: any): FactorySectionPart {
    return {
      partId: backendPart.partId,
      sectionId: backendPart.sectionId,
      partNumber: backendPart.partNumber,
      partName: backendPart.partName,
      minRow: backendPart.minRow,
      maxRow: backendPart.maxRow,
      minBay: backendPart.minBay,
      maxBay: backendPart.maxBay,
      capacity: backendPart.capacity,
      currentOccupancy: backendPart.currentOccupancy || 0,
      availableCapacity: backendPart.availableCapacity || 0,
      isFull: backendPart.isFull || false,
      isActive: backendPart.isActive !== undefined ? backendPart.isActive : true,
      rowCount: backendPart.rowCount,
      bayCount: backendPart.bayCount,
      freezingCells: (backendPart.freezingCells || []) as FreezingCell[]
    };
  }

  /**
   * Transform backend factory section response to frontend model
   */
  private transformFactorySection(backendSection: any): FactorySection {
    // Backend returns enum as string ("Assembly" or "Finishing") or numeric value (1 or 2)
    // Convert to numeric enum value (1 or 2)
    let sectionType: FactorySectionType;
    console.log(`🔍 Transforming section "${backendSection.sectionName}" - Backend type: ${backendSection.sectionType} (${typeof backendSection.sectionType})`);
    
    if (typeof backendSection.sectionType === 'string') {
      sectionType = backendSection.sectionType === 'Assembly' 
        ? FactorySectionType.Assembly 
        : FactorySectionType.Finishing;
    } else {
      sectionType = backendSection.sectionType;
    }
    
    console.log(`  ➡️ Transformed to: ${sectionType} (Assembly=${FactorySectionType.Assembly}, Finishing=${FactorySectionType.Finishing})`);
    
    // Transform parts if they exist
    const parts = backendSection.parts ? backendSection.parts.map((p: any) => 
      this.transformFactorySectionPart(p)
    ) : [];
    
    return {
      sectionId: backendSection.sectionId,
      factoryId: backendSection.factoryId,
      sectionType: sectionType,
      sectionName: backendSection.sectionName,
      minRow: backendSection.minRow,
      maxRow: backendSection.maxRow,
      minBay: backendSection.minBay,
      maxBay: backendSection.maxBay,
      capacity: backendSection.capacity,
      currentOccupancy: backendSection.currentOccupancy || 0,
      availableCapacity: backendSection.availableCapacity || 0,
      isFull: backendSection.isFull || false,
      isActive: backendSection.isActive !== undefined ? backendSection.isActive : true,
      rowCount: backendSection.rowCount,
      bayCount: backendSection.bayCount,
      displayOrder: backendSection.displayOrder || 0,
      parts: parts
    };
  }

  /**
   * Transform backend factory response to frontend model
   */
  private transformFactory(backendFactory: any): Factory {
    const sections = backendFactory.sections ? backendFactory.sections.map((s: any) => 
      this.transformFactorySection(s)
    ) : [];
    
    return {
      factoryId: backendFactory.factoryId || backendFactory.id || '',
      factoryCode: backendFactory.factoryCode || backendFactory.code || '',
      factoryName: backendFactory.factoryName || backendFactory.name || '',
      location: backendFactory.location !== undefined ? backendFactory.location : ProjectLocation.UAE,
      capacity: backendFactory.capacity,
      minRow: backendFactory.minRow,
      maxRow: backendFactory.maxRow,
      minBay: backendFactory.minBay,
      maxBay: backendFactory.maxBay,
      currentOccupancy: backendFactory.currentOccupancy || 0,
      availableCapacity: backendFactory.availableCapacity || 0,
      isFull: backendFactory.isFull || false,
      isActive: backendFactory.isActive !== undefined ? backendFactory.isActive : true,
      dispatchedBoxesCount: backendFactory.dispatchedBoxesCount || 0,
      sections: sections
    };
  }

  /**
   * Get all factories
   */
  getAllFactories(): Observable<Factory[]> {
    return this.apiService.get<any[]>(this.endpoint).pipe(
      map(factories => (factories || []).map(f => this.transformFactory(f)))
    );
  }

  /**
   * Get factory by ID
   */
  getFactoryById(factoryId: string): Observable<Factory> {
    return this.apiService.get<any>(`${this.endpoint}/${factoryId}`).pipe(
      map(f => this.transformFactory(f))
    );
  }

  /**
   * Create a new factory
   */
  createFactory(request: CreateFactoryRequest): Observable<Factory> {
    return this.apiService.post<any>(this.endpoint, request).pipe(
      map(f => this.transformFactory(f))
    );
  }

  /**
   * Get factories by location
   */
  getFactoriesByLocation(location: ProjectLocation): Observable<Factory[]> {
    return this.apiService.get<any[]>(`${this.endpoint}/location/${location}`).pipe(
      map(factories => (factories || []).map(f => this.transformFactory(f)))
    );
  }

  // ── Freezing Cells ────────────────────────────────────────────────────────

  /**
   * Get all freezing cells for a specific factory section part
   */
  getFreezingCellsByPart(factorySectionPartId: string): Observable<FreezingCell[]> {
    return this.apiService.get<FreezingCell[]>(`freezingcells/part/${factorySectionPartId}`);
  }

  /**
   * Freeze a row in a factory section part
   */
  createFreezingCell(factorySectionPartId: string, rowNumber: number): Observable<FreezingCell> {
    return this.apiService.post<FreezingCell>('freezingcells', { factorySectionPartId, rowNumber });
  }

  /**
   * Unfreeze a row by deleting the freezing cell record
   */
  deleteFreezingCell(freezingCellId: string): Observable<any> {
    return this.apiService.delete<any>(`freezingcells/${freezingCellId}`);
  }
}

