import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

import {
  AssignmentSubmission,
  CreateAssignmentSubmission,
  UpdateAssignmentSubmission,
  EvaluateAssignmentSubmission
} from '../models/assignment-submission.model';

import { environment } from '../../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class AssignmentSubmissionService {

  private apiUrl =
    `${environment.apiUrl}/api/assignment-submissions`;

  constructor(
    private http: HttpClient
  ) {}

  // =====================================================
  // GET ALL SUBMISSIONS
  // =====================================================

  getAll(): Observable<AssignmentSubmission[]> {

    return this.http.get<AssignmentSubmission[]>(
      this.apiUrl
    );

  }

  // =====================================================
  // GET BY ID
  // =====================================================

  getById(
    id: string
  ): Observable<AssignmentSubmission> {

    return this.http.get<AssignmentSubmission>(
      `${this.apiUrl}/${id}`
    );

  }

  // =====================================================
  // GET BY ASSIGNMENT
  // =====================================================

  getByAssignment(
    assignmentId: string
  ): Observable<AssignmentSubmission[]> {

    return this.http.get<AssignmentSubmission[]>(
      `${this.apiUrl}/assignment/${assignmentId}`
    );

  }

  // =====================================================
  // GET MY SUBMISSIONS
  // =====================================================

  getMySubmissions():
    Observable<AssignmentSubmission[]> {

    return this.http.get<AssignmentSubmission[]>(
      `${this.apiUrl}/my`
    );

  }

  // =====================================================
  // GET MY SUBMISSION FOR ASSIGNMENT
  // =====================================================

  getMySubmissionByAssignment(
    assignmentId: string
  ): Observable<AssignmentSubmission> {

    return this.http.get<AssignmentSubmission>(
      `${this.apiUrl}/my/assignment/${assignmentId}`
    );

  }

  // =====================================================
  // CREATE / SUBMIT
  // =====================================================

  create(
    data: CreateAssignmentSubmission,
    attachment?: File
  ): Observable<AssignmentSubmission> {

    const formData = new FormData();

    formData.append(
      'AssignmentId',
      data.assignmentId
    );

    if (data.submissionText) {

      formData.append(
        'SubmissionText',
        data.submissionText
      );

    }

    if (attachment) {

      formData.append(
        'Attachment',
        attachment
      );

    }

    return this.http.post<AssignmentSubmission>(
      this.apiUrl,
      formData
    );

  }

  // =====================================================
  // UPDATE
  // =====================================================

  update(
    id: string,
    data: UpdateAssignmentSubmission,
    attachment?: File
  ): Observable<AssignmentSubmission> {

    const formData = new FormData();

    if (
      data.submissionText !== undefined
    ) {

      formData.append(
        'SubmissionText',
        data.submissionText ?? ''
      );

    }

    if (attachment) {

      formData.append(
        'Attachment',
        attachment
      );

    }

    return this.http.put<AssignmentSubmission>(
      `${this.apiUrl}/${id}`,
      formData
    );

  }

  // =====================================================
  // DELETE
  // =====================================================

  delete(
    id: string
  ): Observable<void> {

    return this.http.delete<void>(
      `${this.apiUrl}/${id}`
    );

  }

  // =====================================================
  // EVALUATE
  // =====================================================

  evaluate(
    id: string,
    data: EvaluateAssignmentSubmission
  ): Observable<AssignmentSubmission> {

    return this.http.put<AssignmentSubmission>(
      `${this.apiUrl}/${id}/evaluate`,
      data
    );

  }

  // =====================================================
  // SUBMISSION COUNT
  // =====================================================

  getSubmissionCount(
    assignmentId: string
  ): Observable<number> {

    return this.http.get<number>(
      `${this.apiUrl}/assignment/${assignmentId}/count`
    );

  }

  // =====================================================
  // PENDING COUNT
  // =====================================================

  getPendingSubmissionCount(
    assignmentId: string
  ): Observable<number> {

    return this.http.get<number>(
      `${this.apiUrl}/assignment/${assignmentId}/pending-count`
    );

  }

}