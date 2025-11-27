import { Injectable } from '@angular/core';
import { Observable, shareReplay } from 'rxjs';
import { ApiService } from './api.service';
import { Department } from '../models/team.model';

@Injectable({
  providedIn: 'root'
})
export class DepartmentService {
  private departments$?: Observable<Department[]>;

  constructor(private apiService: ApiService) {}

  /**
   * Load departments from backend. Result is memoized to avoid duplicate calls.
   */
  getDepartments(forceRefresh = false): Observable<Department[]> {
    if (!this.departments$ || forceRefresh) {
      this.departments$ = this.apiService.get<Department[]>('Department').pipe(shareReplay(1));
    }

    return this.departments$;
  }
}



