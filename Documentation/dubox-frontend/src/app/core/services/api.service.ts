import { Injectable } from '@angular/core';
import { HttpClient, HttpParams, HttpHeaders } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError, map } from 'rxjs/operators';
import { environment } from '../../../environments/environment';

export interface ApiResponse<T> {
  success: boolean;
  data: T;
  message?: string;
  errors?: any[];
}

export interface PaginatedResponse<T> {
  items: T[];
  totalCount: number;
  pageNumber: number;
  pageSize: number;
  totalPages: number;
}

@Injectable({
  providedIn: 'root'
})
export class ApiService {
  private readonly baseUrl = environment.apiUrl;

  constructor(private http: HttpClient) {}

  /**
   * GET request
   */
  get<T>(endpoint: string, params?: any): Observable<T> {
    const httpParams = this.buildParams(params);
    return this.http.get<any>(`${this.baseUrl}/${endpoint}`, { params: httpParams })
      .pipe(
        map(response => {
          console.log('🌐 GET API Response for', endpoint, ':', response);
          console.log('🔑 Response keys:', Object.keys(response));
          
          // Backend returns Result<T> with 'data' property (camelCase configured in Program.cs)
          // Try: data (camelCase) -> Data (PascalCase) -> value -> Value -> raw response
          const data = response.data || response.Data || response.value || response.Value || response;
          console.log('✅ Extracted data:', Array.isArray(data) ? `Array[${data.length}]` : data);
          return data;
        }),
        catchError(this.handleError)
      );
  }

  /**
   * POST request
   */
  post<T>(endpoint: string, body: any): Observable<T> {
    return this.http.post<any>(`${this.baseUrl}/${endpoint}`, body)
      .pipe(
        map(response => {
          console.log('🌐 POST API Response for', endpoint, ':', response);
          console.log('🔑 Response keys:', Object.keys(response));
          
          // Backend returns Result<T> with 'data' property (camelCase configured in Program.cs)
          // Try: data (camelCase) -> Data (PascalCase) -> value -> Value -> raw response
          const data = response.data || response.Data || response.value || response.Value || response;
          console.log('✅ Extracted data:', data);
          return data;
        }),
        catchError(this.handleError)
      );
  }

  /**
   * PUT request
   */
  put<T>(endpoint: string, body: any): Observable<T> {
    return this.http.put<any>(`${this.baseUrl}/${endpoint}`, body)
      .pipe(
        map(response => {
          console.log('🌐 PUT API Response for', endpoint, ':', response);
          console.log('🔑 Response keys:', Object.keys(response));
          
          // Backend returns Result<T> with 'data' property (camelCase configured in Program.cs)
          // Try: data (camelCase) -> Data (PascalCase) -> value -> Value -> raw response
          const data = response.data || response.Data || response.value || response.Value || response;
          console.log('✅ Extracted data:', data);
          return data;
        }),
        catchError(this.handleError)
      );
  }

  /**
   * POST request with FormData (for file uploads)
   * FormData will automatically set Content-Type with boundary
   * Important: Do NOT manually set Content-Type - let Angular/browser set it with boundary
   */
  postFormData<T>(endpoint: string, formData: FormData): Observable<T> {
    const url = `${this.baseUrl}/${endpoint}`;
    console.log('📤 POST FormData Request:', {
      url,
      endpoint,
      formDataKeys: Array.from(formData.keys()),
      hasFile: formData.has('file') || formData.has('Files')
    });
    
    return this.http.post<any>(url, formData, {
      reportProgress: false,
      observe: 'body'
    })
      .pipe(
        map(response => {
          console.log('🌐 POST FormData API Response for', endpoint, ':', response);
          const data = response.data || response.Data || response.value || response.Value || response;
          console.log('✅ Extracted data:', data);
          return data;
        }),
        catchError((error) => {
          console.error('❌ POST FormData Error for', endpoint, ':', {
            status: error.status,
            statusText: error.statusText,
            message: error.message,
            url: error.url,
            error: error.error
          });
          return this.handleError(error);
        })
      );
  }

  /**
   * PUT request with FormData (for file uploads)
   * FormData will automatically set Content-Type with boundary
   * Important: Do NOT manually set Content-Type - let Angular/browser set it with boundary
   */
  putFormData<T>(endpoint: string, formData: FormData): Observable<T> {
    // Angular's HttpClient automatically sets Content-Type for FormData with boundary
    // We must NOT set it manually, otherwise the boundary won't be included
    return this.http.put<any>(`${this.baseUrl}/${endpoint}`, formData)
      .pipe(
        map(response => {
          console.log('🌐 PUT FormData API Response for', endpoint, ':', response);
          
          // Backend returns Result<T> with 'data' property (camelCase configured in Program.cs)
          const data = response.data || response.Data || response.value || response.Value || response;
          console.log('✅ Extracted data:', data);
          return data;
        }),
        catchError(this.handleError)
      );
  }

  /**
   * PATCH request
   */
  patch<T>(endpoint: string, body: any): Observable<T> {
    return this.http.patch<any>(`${this.baseUrl}/${endpoint}`, body)
      .pipe(
        map(response => response.data || response.Data || response),
        catchError(this.handleError)
      );
  }

  /**
   * DELETE request
   */
  delete<T>(endpoint: string): Observable<T> {
    return this.http.delete<any>(`${this.baseUrl}/${endpoint}`)
      .pipe(
        map(response => {
          console.log('🌐 DELETE API Response for', endpoint, ':', response);
          console.log('🔑 Response keys:', Object.keys(response));
          
          // Backend returns Result<T> with 'data' property (camelCase configured in Program.cs)
          // Try: data (camelCase) -> Data (PascalCase) -> value -> Value -> raw response
          const data = response.data || response.Data || response.value || response.Value || response;
          console.log('✅ Extracted data:', data);
          return data;
        }),
        catchError(this.handleError)
      );
  }

  /**
   * Upload file
   */
  upload<T>(endpoint: string, file: File, additionalData?: any): Observable<T> {
    const formData = new FormData();
    formData.append('file', file);
    
    if (additionalData) {
      Object.keys(additionalData).forEach(key => {
        formData.append(key, additionalData[key]);
      });
    }

    return this.http.post<any>(`${this.baseUrl}/${endpoint}`, formData)
      .pipe(
        map(response => {
          console.log('🌐 UPLOAD API Response for', endpoint, ':', response);
          console.log('🔑 Response keys:', Object.keys(response));
          
          // Backend returns Result<T> with 'data' property (camelCase configured in Program.cs)
          // Try: data (camelCase) -> Data (PascalCase) -> value -> Value -> raw response
          const data = response.data || response.Data || response.value || response.Value || response;
          console.log('✅ Extracted upload data:', data);
          return data;
        }),
        catchError(this.handleError)
      );
  }

  /**
   * Download file
   */
  download(endpoint: string): Observable<Blob> {
    return this.http.get(`${this.baseUrl}/${endpoint}`, {
      responseType: 'blob'
    }).pipe(
      catchError(this.handleError)
    );
  }

  /**
   * Download file with full response (including headers)
   */
  downloadWithResponse(endpoint: string): Observable<any> {
    return this.http.get(`${this.baseUrl}/${endpoint}`, {
      responseType: 'blob',
      observe: 'response'
    }).pipe(
      catchError(this.handleError)
    );
  }

  /**
   * Build HTTP params from object
   */
  private buildParams(params?: any): HttpParams {
    let httpParams = new HttpParams();
    
    if (params) {
      Object.keys(params).forEach(key => {
        if (params[key] !== null && params[key] !== undefined) {
          if (Array.isArray(params[key])) {
            params[key].forEach((value: any) => {
              httpParams = httpParams.append(key, value.toString());
            });
          } else {
            httpParams = httpParams.set(key, params[key].toString());
          }
        }
      });
    }
    
    return httpParams;
  }

  /**
   * Handle HTTP errors
   */
  private handleError(error: any): Observable<never> {
    let errorMessage = 'An error occurred';
    
    if (error.error instanceof ErrorEvent) {
      // Client-side error
      errorMessage = `Error: ${error.error.message}`;
    } else {
      // Server-side error
      errorMessage = error.error?.message || `Error Code: ${error.status}\nMessage: ${error.message}`;
    }
    
    console.error('API Error:', errorMessage, error);
    return throwError(() => error);
  }
}
