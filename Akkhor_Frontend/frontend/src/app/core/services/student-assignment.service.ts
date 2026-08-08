import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

import {
  Assignment
} from '../models/student-assignment.model';

import {
  AssignmentSubmission,
  CreateAssignmentSubmission,
  UpdateAssignmentSubmission
} from '../models/student-assignment-submission.model';

@Injectable({
  providedIn: 'root'
})
export class StudentAssignmentService {

  private readonly assignmentApiUrl =
    'https://localhost:50268/api/student-assignments';

  private readonly submissionApiUrl =
    'https://localhost:50268/api/assignment-submissions';


  constructor(
    private http: HttpClient
  ) {}


  // =====================================================
  // STUDENT ASSIGNMENTS
  // =====================================================

  /**
   * Get all assignments available for logged-in student
   */
  getMyAssignments(): Observable<Assignment[]> {

    return this.http.get<Assignment[]>(
      `${this.assignmentApiUrl}/student`
    );
  }


  /**
   * Get single assignment for logged-in student
   */
  getAssignmentById(
    id: string
  ): Observable<Assignment> {

    return this.http.get<Assignment>(
      `${this.assignmentApiUrl}/student/${id}`
    );
  }


  // =====================================================
  // ASSIGNMENT SUBMISSIONS
  // =====================================================

  /**
   * Get all submissions of logged-in student
   */
  getMySubmissions(): Observable<AssignmentSubmission[]> {

    return this.http.get<AssignmentSubmission[]>(
      `${this.submissionApiUrl}/my`
    );
  }


  /**
   * Get submission by ID
   */
  getSubmissionById(
    id: string
  ): Observable<AssignmentSubmission> {

    return this.http.get<AssignmentSubmission>(
      `${this.submissionApiUrl}/${id}`
    );
  }


  /**
   * Get submission for a specific assignment
   */
  getMySubmissionForAssignment(
    assignmentId: string
  ): Observable<AssignmentSubmission | null> {

    return this.http.get<AssignmentSubmission | null>(
      `${this.submissionApiUrl}/assignment/${assignmentId}/my`
    );
  }


  /**
   * Submit assignment
   */
  submitAssignment(
    data: CreateAssignmentSubmission
  ): Observable<AssignmentSubmission> {

    return this.http.post<AssignmentSubmission>(
      this.submissionApiUrl,
      data
    );
  }


  /**
   * Update assignment submission
   */
  updateSubmission(
    id: string,
    data: UpdateAssignmentSubmission
  ): Observable<AssignmentSubmission> {

    return this.http.put<AssignmentSubmission>(
      `${this.submissionApiUrl}/${id}`,
      data
    );
  }


  /**
   * Delete assignment submission
   */
  deleteSubmission(
    id: string
  ): Observable<boolean> {

    return this.http.delete<boolean>(
      `${this.submissionApiUrl}/${id}`
    );
  }
}