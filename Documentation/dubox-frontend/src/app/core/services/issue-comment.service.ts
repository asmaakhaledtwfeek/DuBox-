import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { IssueComment, CreateCommentRequest, UpdateCommentRequest } from '../models/issue-comment.model';

@Injectable({
  providedIn: 'root'
})
export class IssueCommentService {
  private readonly apiUrl = `${environment.apiUrl}/issues`;

  constructor(private http: HttpClient) {}

  /**
   * Get all comments for a quality issue (with threaded structure)
   */
  getIssueComments(issueId: string, includeDeleted: boolean = false): Observable<any> {
    return this.http.get<any>(`${this.apiUrl}/${issueId}/comments`, {
      params: { includeDeleted: includeDeleted.toString() }
    });
  }

  /**
   * Add a new comment to a quality issue
   */
  addComment(request: CreateCommentRequest): Observable<any> {
    return this.http.post<any>(`${this.apiUrl}/${request.issueId}/comments`, request);
  }

  /**
   * Update an existing comment (author only)
   */
  updateComment(issueId: string, request: UpdateCommentRequest): Observable<any> {
    return this.http.put<any>(`${this.apiUrl}/${issueId}/comments/${request.commentId}`, request);
  }

  /**
   * Delete a comment (soft delete, author only)
   */
  deleteComment(issueId: string, commentId: string): Observable<any> {
    return this.http.delete<any>(`${this.apiUrl}/${issueId}/comments/${commentId}`);
  }
}

