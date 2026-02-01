import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { ApiService } from './api.service';

export interface FactoryLocation {
  locationId: string;
  locationCode: string;
  locationName: string;
  locationType?: string;
  bay?: string;
  row?: string;
  position?: string;
  capacity?: number;
  currentOccupancy: number;
  availableCapacity: number;
  isFull: boolean;
  isActive: boolean;
}

export interface CreateLocationRequest {
  locationCode: string;
  locationName: string;
  locationType?: string;
  bay?: string;
  row?: string;
  position?: string;
  capacity?: number;
}

export interface BoxLocationHistory {
  historyId: string;
  boxId: string;
  boxTag: string;
  serialNumber?: string;
  locationId: string;
  locationCode: string;
  locationName: string;
  movedFromLocationId?: string;
  movedFromLocationCode?: string;
  movedFromLocationName?: string;
  movedDate: Date;
  reason?: string;
  movedBy?: string;
  movedByUsername?: string;
  movedByFullName?: string;
}

export interface MoveBoxToLocationRequest {
  boxId: string;
  toLocationId: string;
  reason?: string;
}

export interface LocationBoxStatusCount {
  status: string;
  count: number;
}

export interface LocationBoxes {
  locationId: string;
  locationCode: string;
  locationName: string;
  boxes: any[];
  statusCounts: LocationBoxStatusCount[];
  totalBoxes: number;
}

@Injectable({
  providedIn: 'root'
})
export class LocationService {
  private readonly endpoint = 'locations';

  constructor(private apiService: ApiService) {}

  /**
   * Transform backend location response to frontend model
   */
  private transformLocation(backendLocation: any): FactoryLocation {
    return {
      locationId: backendLocation.locationId || backendLocation.id || '',
      locationCode: backendLocation.locationCode || backendLocation.code || '',
      locationName: backendLocation.locationName || backendLocation.name || '',
      locationType: backendLocation.locationType,
      bay: backendLocation.bay,
      row: backendLocation.row,
      position: backendLocation.position,
      capacity: backendLocation.capacity,
      currentOccupancy: backendLocation.currentOccupancy || 0,
      availableCapacity: backendLocation.availableCapacity || 0,
      isFull: backendLocation.isFull || false,
      isActive: backendLocation.isActive !== undefined ? backendLocation.isActive : true
    };
  }

  /**
   * Get all locations
   */
  getLocations(): Observable<FactoryLocation[]> {
    return this.apiService.get<any[]>(this.endpoint).pipe(
      map(locations => (locations || []).map(l => this.transformLocation(l)))
    );
  }

  /**
   * Get location by ID
   */
  getLocation(locationId: string): Observable<FactoryLocation> {
    return this.apiService.get<any>(`${this.endpoint}/${locationId}`).pipe(
      map(l => this.transformLocation(l))
    );
  }

  /**
   * Create a new location
   */
  createLocation(payload: CreateLocationRequest): Observable<FactoryLocation> {
    return this.apiService.post<any>(this.endpoint, payload).pipe(
      map(l => this.transformLocation(l))
    );
  }

  /**
   * Check if location code exists
   */
  checkLocationExists(locationCode: string): Observable<boolean> {
    return this.apiService.get<{ exists: boolean }>(`${this.endpoint}/check-exists`, { locationCode }).pipe(
      map(response => response.exists)
    );
  }

  /**
   * Move box to location
   */
  moveBoxToLocation(payload: MoveBoxToLocationRequest): Observable<BoxLocationHistory> {
    return this.apiService.post<any>(`${this.endpoint}/move-box`, payload).pipe(
      map(history => ({
        historyId: history.historyId || history.id || '',
        boxId: history.boxId || '',
        boxTag: history.boxTag || '',
        serialNumber: history.serialNumber,
        locationId: history.locationId || '',
        locationCode: history.locationCode || '',
        locationName: history.locationName || '',
        movedFromLocationId: history.movedFromLocationId,
        movedFromLocationCode: history.movedFromLocationCode,
        movedFromLocationName: history.movedFromLocationName,
        movedDate: new Date(history.movedDate || history.movedInDate),
        reason: history.reason,
        movedBy: history.movedBy,
        movedByUsername: history.movedByUsername,
        movedByFullName: history.movedByFullName
      }))
    );
  }

  /**
   * Get box location history
   */
  getBoxLocationHistory(boxId: string): Observable<BoxLocationHistory[]> {
    return this.apiService.get<any[]>(`${this.endpoint}/boxes/${boxId}/history`).pipe(
      map(history => (history || []).map(h => ({
        historyId: h.historyId || h.id || '',
        boxId: h.boxId || '',
        boxTag: h.boxTag || '',
        serialNumber: h.serialNumber,
        locationId: h.locationId || '',
        locationCode: h.locationCode || '',
        locationName: h.locationName || '',
        movedFromLocationId: h.movedFromLocationId,
        movedFromLocationCode: h.movedFromLocationCode,
        movedFromLocationName: h.movedFromLocationName,
        movedDate: new Date(h.movedDate || h.movedInDate),
        reason: h.reason,
        movedBy: h.movedBy,
        movedByUsername: h.movedByUsername,
        movedByFullName: h.movedByFullName
      })))
    );
  }

  /**
   * Get boxes by location
   */
  getBoxesByLocation(locationId: string): Observable<LocationBoxes> {
    return this.apiService.get<any>(`${this.endpoint}/${locationId}/boxes`).pipe(
      map(data => ({
        locationId: data.locationId || '',
        locationCode: data.locationCode || '',
        locationName: data.locationName || '',
        boxes: data.boxes || [],
        statusCounts: (data.statusCounts || []).map((sc: any) => ({
          status: sc.status || '',
          count: sc.count || 0
        })),
        totalBoxes: data.totalBoxes || 0
      }))
    );
  }
}

