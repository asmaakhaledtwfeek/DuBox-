import { Component, Input, OnInit, OnChanges, SimpleChanges, Output, EventEmitter } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { IssueComment, CreateCommentRequest, UpdateCommentRequest, QualityIssueStatus } from '../../../core/models/issue-comment.model';
import { IssueCommentService } from '../../../core/services/issue-comment.service';
import { AuthService } from '../../../core/services/auth.service';
import { ToastService } from '../../../core/services/toast.service';

@Component({
  selector: 'app-issue-comments',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './issue-comments.component.html',
  styleUrls: ['./issue-comments.component.scss']
})
export class IssueCommentsComponent implements OnInit, OnChanges {
  @Input() issueId: string = '';
  @Input() issueNumber: string = '';
  @Input() issueTitle: string = '';
  @Output() commentAdded = new EventEmitter<void>();

  comments: IssueComment[] = [];
  loading: boolean = false;
  newCommentText: string = '';
  replyingTo: IssueComment | null = null;
  editingComment: IssueComment | null = null;
  editedCommentText: string = '';
  currentUserId: string = '';

  constructor(
    private commentService: IssueCommentService,
    public authService: AuthService,
    private toastService: ToastService
  ) {
    const userInfo = this.authService.getUserInfo();
    this.currentUserId = userInfo?.id || '';
  }

  ngOnInit(): void {
    if (this.issueId) {
      this.loadComments();
    }
  }

  ngOnChanges(changes: SimpleChanges): void {
    if (changes['issueId'] && !changes['issueId'].firstChange) {
      this.loadComments();
    }
  }

  loadComments(): void {
    if (!this.issueId) return;

    this.loading = true;
    this.commentService.getIssueComments(this.issueId).subscribe({
      next: (response) => {
        if (response.isSuccess && response.data) {
          this.comments = response.data;
        }
        this.loading = false;
      },
      error: (error) => {
        console.error('Error loading comments:', error);
        this.toastService.showError('Failed to load comments');
        this.loading = false;
      }
    });
  }

  addComment(): void {
    if (!this.newCommentText.trim()) {
      this.toastService.showWarning('Please enter a comment');
      return;
    }

    const request: CreateCommentRequest = {
      issueId: this.issueId,
      parentCommentId: this.replyingTo?.commentId,
      commentText: this.newCommentText.trim(),
      isStatusUpdateComment: false
    };

    this.commentService.addComment(request).subscribe({
      next: (response) => {
        if (response.isSuccess) {
          this.toastService.showSuccess('Comment added successfully');
          this.newCommentText = '';
          this.replyingTo = null;
          this.loadComments();
          this.commentAdded.emit();
        }
      },
      error: (error) => {
        console.error('Error adding comment:', error);
        this.toastService.showError('Failed to add comment');
      }
    });
  }

  startReply(comment: IssueComment): void {
    this.replyingTo = comment;
    this.editingComment = null;
    this.newCommentText = '';
  }

  cancelReply(): void {
    this.replyingTo = null;
    this.newCommentText = '';
  }

  startEdit(comment: IssueComment): void {
    this.editingComment = comment;
    this.editedCommentText = comment.commentText;
    this.replyingTo = null;
  }

  cancelEdit(): void {
    this.editingComment = null;
    this.editedCommentText = '';
  }

  saveEdit(): void {
    if (!this.editingComment || !this.editedCommentText.trim()) {
      this.toastService.showWarning('Please enter a comment');
      return;
    }

    const request: UpdateCommentRequest = {
      commentId: this.editingComment.commentId,
      commentText: this.editedCommentText.trim()
    };

    this.commentService.updateComment(this.issueId, request).subscribe({
      next: (response) => {
        if (response.isSuccess) {
          this.toastService.showSuccess('Comment updated successfully');
          this.editingComment = null;
          this.editedCommentText = '';
          this.loadComments();
        }
      },
      error: (error) => {
        console.error('Error updating comment:', error);
        this.toastService.showError('Failed to update comment');
      }
    });
  }

  deleteComment(comment: IssueComment): void {
    if (!confirm('Are you sure you want to delete this comment?')) {
      return;
    }

    this.commentService.deleteComment(this.issueId, comment.commentId).subscribe({
      next: (response) => {
        if (response.isSuccess) {
          this.toastService.showSuccess('Comment deleted successfully');
          this.loadComments();
        }
      },
      error: (error) => {
        console.error('Error deleting comment:', error);
        this.toastService.showError('Failed to delete comment');
      }
    });
  }

  canEdit(comment: IssueComment): boolean {
    return comment.authorId === this.currentUserId && !comment.isDeleted;
  }

  canDelete(comment: IssueComment): boolean {
    return comment.authorId === this.currentUserId && !comment.isDeleted;
  }

  formatDate(date: Date | string): string {
    if (!date) return '';
    const d = new Date(date);
    const now = new Date();
    const diff = now.getTime() - d.getTime();
    const seconds = Math.floor(diff / 1000);
    const minutes = Math.floor(seconds / 60);
    const hours = Math.floor(minutes / 60);
    const days = Math.floor(hours / 24);

    if (days > 7) {
      return d.toLocaleDateString();
    } else if (days > 0) {
      return `${days} day${days > 1 ? 's' : ''} ago`;
    } else if (hours > 0) {
      return `${hours} hour${hours > 1 ? 's' : ''} ago`;
    } else if (minutes > 0) {
      return `${minutes} minute${minutes > 1 ? 's' : ''} ago`;
    } else {
      return 'Just now';
    }
  }

  getStatusBadgeClass(status?: QualityIssueStatus): string {
    switch (status) {
      case QualityIssueStatus.Open:
        return 'badge-open';
      case QualityIssueStatus.InProgress:
        return 'badge-in-progress';
      case QualityIssueStatus.Resolved:
        return 'badge-resolved';
      case QualityIssueStatus.Closed:
        return 'badge-closed';
      default:
        return '';
    }
  }

  getStatusLabel(status?: QualityIssueStatus): string {
    switch (status) {
      case QualityIssueStatus.Open:
        return 'Open';
      case QualityIssueStatus.InProgress:
        return 'In Progress';
      case QualityIssueStatus.Resolved:
        return 'Resolved';
      case QualityIssueStatus.Closed:
        return 'Closed';
      default:
        return '';
    }
  }

  getInitials(name: string): string {
    if (!name) return '?';
    const parts = name.trim().split(' ');
    if (parts.length >= 2) {
      return (parts[0][0] + parts[1][0]).toUpperCase();
    }
    return name.substring(0, 2).toUpperCase();
  }
}

