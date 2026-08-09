import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

import { StudentAssignment } from '../models/student-assignment.model';
import { AssignmentSubmission } from '../models/assignment-submission.model';

import { environment } from '../../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class StudentAssignmentService {

  // =====================================================
  // API URLS
  // =====================================================

  private readonly assignmentApiUrl =
    `${environment.apiUrl}/api/student-assignments`;

  private readonly submissionApiUrl =
    `${environment.apiUrl}/api/assignment-submissions`;

  // =====================================================
  // CONSTRUCTOR
  // =====================================================

  constructor(
    private http: HttpClient
  ) {}

  // =====================================================
  // GET MY ASSIGNMENTS
  // =====================================================

  getMyAssignments(): Observable<StudentAssignment[]> {

    return this.http.get<StudentAssignment[]>(
      `${this.assignmentApiUrl}/student`
    );

  }

  // =====================================================
  // GET ASSIGNMENT BY ID
  // =====================================================

  getById(
    id: string
  ): Observable<StudentAssignment> {

    return this.http.get<StudentAssignment>(
      `${this.assignmentApiUrl}/student/${id}`
    );

  }

  // =====================================================
  // GET MY SUBMISSIONS
  // =====================================================

  getMySubmissions(): Observable<AssignmentSubmission[]> {

    return this.http.get<AssignmentSubmission[]>(
      `${this.submissionApiUrl}/my`
    );

  }

  // =====================================================
  // GET SUBMISSION BY ID
  // =====================================================

  getSubmissionById(
    id: string
  ): Observable<AssignmentSubmission> {

    return this.http.get<AssignmentSubmission>(
      `${this.submissionApiUrl}/${id}`
    );

  }

  // =====================================================
  // GET MY SUBMISSION FOR ASSIGNMENT
  // =====================================================

  getMySubmissionByAssignment(
    assignmentId: string
  ): Observable<AssignmentSubmission> {

    return this.http.get<AssignmentSubmission>(
      `${this.submissionApiUrl}/my/assignment/${assignmentId}`
    );

  }

  // =====================================================
  // GET MY SUBMISSION FOR ASSIGNMENT
  // ALIAS
  // =====================================================

  getMySubmissionForAssignment(
    assignmentId: string
  ): Observable<AssignmentSubmission> {

    return this.getMySubmissionByAssignment(
      assignmentId
    );

  }

}