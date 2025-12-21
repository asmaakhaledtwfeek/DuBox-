import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { ApiService } from './api.service';

export enum ProjectLocation {
  KSA = 1,
  UAE = 2
}

export interface Factory {
  factoryId: string;
  factoryCode: string;
  factoryName: string;
  location: ProjectLocation;
  capacity?: number;
  currentOccupancy: number;
  availableCapacity: number;
  isFull: boolean;
  isActive: boolean;
}

export interface CreateFactoryRequest {
  factoryCode: string;
  factoryName: string;
  location: ProjectLocation;
  capacity?: number;
}

@Injectable({
  providedIn: 'root'
})
export class FactoryService {
  private readonly endpoint = 'factories';

  constructor(private apiService: ApiService) {}

  /**
   * Transform backend factory response to frontend model
   */
  private transformFactory(backendFactory: any): Factory {
    return {
      factoryId: backendFactory.factoryId || backendFactory.id || '',
      factoryCode: backendFactory.factoryCode || backendFactory.code || '',
      factoryName: backendFactory.factoryName || backendFactory.name || '',
      location: backendFactory.location !== undefined ? backendFactory.location : ProjectLocation.UAE,
      capacity: backendFactory.capacity,
      currentOccupancy: backendFactory.currentOccupancy || 0,
      availableCapacity: backendFactory.availableCapacity || 0,
      isFull: backendFactory.isFull || false,
      isActive: backendFactory.isActive !== undefined ? backendFactory.isActive : true
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
}

