export interface IssueComment {
  commentId: string;
  issueId: string;
  parentCommentId?: string;
  authorId: string;
  authorName: string;
  authorAvatar?: string;
  commentText: string;
  createdDate: Date | string;
  updatedDate?: Date | string;
  updatedBy?: string;
  updatedByName?: string;
  isDeleted: boolean;
  isStatusUpdateComment: boolean;
  relatedStatus?: QualityIssueStatus;
  isReply: boolean;
  isEdited: boolean;
  replies: IssueComment[];
  replyCount: number;
}

export interface CreateCommentRequest {
  issueId: string;
  parentCommentId?: string;
  commentText: string;
  isStatusUpdateComment?: boolean;
  relatedStatus?: QualityIssueStatus;
}

export interface UpdateCommentRequest {
  commentId: string;
  commentText: string;
}

export enum QualityIssueStatus {
  Open = 'Open',
  InProgress = 'InProgress',
  Resolved = 'Resolved',
  Closed = 'Closed'
}

