import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

import { Assignment } from '../models/assignment.model';
import { environment } from '../../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class AdminAssignmentService {

  private readonly apiUrl =
    `${environment.apiUrl}/api/admin/assignments`;

  constructor(
    private http: HttpClient
  ) {}


  // =====================================================
  // GET ALL
  // =====================================================

  getAll(): Observable<Assignment[]> {
    return this.http.get<Assignment[]>(
      this.apiUrl
    );
  }


  // =====================================================
  // GET BY ID
  // =====================================================

  getById(id: string): Observable<Assignment> {
    return this.http.get<Assignment>(
      `${this.apiUrl}/${id}`
    );
  }


  // =====================================================
  // GET BY CLASS
  // =====================================================

  getByClass(
    classId: string
  ): Observable<Assignment[]> {

    return this.http.get<Assignment[]>(
      `${this.apiUrl}/class/${classId}`
    );
  }


  // =====================================================
  // GET BY COURSE
  // =====================================================

  getByCourse(
    courseId: string
  ): Observable<Assignment[]> {

    return this.http.get<Assignment[]>(
      `${this.apiUrl}/course/${courseId}`
    );
  }


  // =====================================================
  // GET BY SUBJECT
  // =====================================================

  getBySubject(
    subjectId: string
  ): Observable<Assignment[]> {

    return this.http.get<Assignment[]>(
      `${this.apiUrl}/subject/${subjectId}`
    );
  }
}