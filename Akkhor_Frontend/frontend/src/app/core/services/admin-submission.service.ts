import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

import { AssignmentSubmission }
  from '../models/assignment-submission.model';

import { environment }
  from '../../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class AdminSubmissionService {

  private readonly apiUrl =
    `${environment.apiUrl}/api/admin/submissions`;

  constructor(
    private http: HttpClient
  ) {}


  // =====================================================
  // GET ALL
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
}