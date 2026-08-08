import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

import {
  AssignmentSubmission,
  EvaluateAssignmentSubmission
} from '../models/assignment-submission.model';

@Injectable({
  providedIn: 'root'
})
export class AssignmentSubmissionService {

  // =====================================================
  // API URL
  // =====================================================

  private apiUrl =
    'https://localhost:50268/api/assignment-submissions';

  // =====================================================
  // CONSTRUCTOR
  // =====================================================

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
  // GET SUBMISSION BY ID
  // =====================================================

  getById(
    id: string
  ): Observable<AssignmentSubmission> {

    return this.http.get<AssignmentSubmission>(
      `${this.apiUrl}/${id}`
    );
  }

  // =====================================================
  // GET SUBMISSIONS BY ASSIGNMENT
  // =====================================================

  getByAssignment(
    assignmentId: string
  ): Observable<AssignmentSubmission[]> {

    return this.http.get<AssignmentSubmission[]>(
      `${this.apiUrl}/assignment/${assignmentId}`
    );
  }

  // =====================================================
  // GET SUBMISSION BY ASSIGNMENT + STUDENT
  // =====================================================

  getByAssignmentAndStudent(
    assignmentId: string,
    studentId: string
  ): Observable<AssignmentSubmission | null> {

    return this.http.get<AssignmentSubmission | null>(
      `${this.apiUrl}/assignment/${assignmentId}/student/${studentId}`
    );
  }

  // =====================================================
  // GET MY SUBMISSIONS
  // =====================================================

  getMySubmissions(): Observable<AssignmentSubmission[]> {

    return this.http.get<AssignmentSubmission[]>(
      `${this.apiUrl}/my`
    );
  }

  // =====================================================
  // CREATE / SUBMIT ASSIGNMENT
  // =====================================================
  //
  // FormData is used because students can submit:
  // - Submission text
  // - Attachment
  //
  // =====================================================

  create(
    formData: FormData
  ): Observable<AssignmentSubmission> {

    return this.http.post<AssignmentSubmission>(
      this.apiUrl,
      formData
    );
  }

  // =====================================================
  // UPDATE SUBMISSION
  // =====================================================
  //
  // FormData allows the student to update:
  // - Submission text
  // - Attachment
  //
  // =====================================================

  update(
    id: string,
    formData: FormData
  ): Observable<AssignmentSubmission> {

    return this.http.put<AssignmentSubmission>(
      `${this.apiUrl}/${id}`,
      formData
    );
  }

  // =====================================================
  // DELETE SUBMISSION
  // =====================================================

  delete(
    id: string
  ): Observable<boolean> {

    return this.http.delete<boolean>(
      `${this.apiUrl}/${id}`
    );
  }

  // =====================================================
  // EVALUATE / GRADE SUBMISSION
  // =====================================================
  //
  // Teacher sends:
  // - marksObtained
  // - feedback
  //
  // Backend sets:
  // - Status = Evaluated
  // - EvaluatedAt
  // - EvaluatedBy
  //
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
  // GET SUBMISSION COUNT
  // =====================================================

  getSubmissionCount(
    assignmentId: string
  ): Observable<number> {

    return this.http.get<number>(
      `${this.apiUrl}/assignment/${assignmentId}/count`
    );
  }

  // =====================================================
  // GET PENDING SUBMISSION COUNT
  // =====================================================

  getPendingSubmissionCount(
    assignmentId: string
  ): Observable<number> {

    return this.http.get<number>(
      `${this.apiUrl}/assignment/${assignmentId}/pending-count`
    );
  }

}