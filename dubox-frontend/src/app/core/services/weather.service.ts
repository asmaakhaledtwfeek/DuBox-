import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { environment } from '../../../environments/environment';
import { ProjectWeatherReport } from '../models/weather.model';

@Injectable({
  providedIn: 'root'
})
export class WeatherService {
  private apiUrl = `${environment.apiUrl}/projects/weather`;

  constructor(private http: HttpClient) { }

  getProjectWeather(projectId: string): Observable<ProjectWeatherReport | null> {
    return this.http.get<any>(`${this.apiUrl}/${projectId}`).pipe(
      map(response => {
        if (response.isSuccess && response.data) {
          return response.data;
        }
        return null;
      })
    );
  }

  getAllProjectsWeather(): Observable<ProjectWeatherReport[]> {
    return this.http.get<any>(`${this.apiUrl}/all`).pipe(
      map(response => {
        if (response.isSuccess && response.data) {
          return response.data;
        }
        return [];
      })
    );
  }
}
