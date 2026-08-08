import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

import { Assignment } from '../models/assignment.model';

@Injectable({
  providedIn: 'root'
})
export class AssignmentService {

  // =====================================================
  // API URL
  // =====================================================

  private apiUrl =
    'https://localhost:50268/api/assignments';

  constructor(
    private http: HttpClient
  ) {}

  // =====================================================
  // GET ALL ASSIGNMENTS
  // =====================================================

  getAll(): Observable<Assignment[]> {

    return this.http.get<Assignment[]>(
      this.apiUrl
    );

  }

  // =====================================================
  // GET ASSIGNMENT BY ID
  // =====================================================

  getById(
    id: string
  ): Observable<Assignment> {

    return this.http.get<Assignment>(
      `${this.apiUrl}/${id}`
    );

  }

  // =====================================================
  // GET MY ASSIGNMENTS
  // =====================================================

  getMyAssignments(): Observable<Assignment[]> {

    return this.http.get<Assignment[]>(
      `${this.apiUrl}/my`
    );

  }

  // =====================================================
  // GET MY ASSIGNMENT BY ID
  // =====================================================

  getMyAssignmentById(
    id: string
  ): Observable<Assignment> {

    return this.http.get<Assignment>(
      `${this.apiUrl}/my/${id}`
    );

  }

  // =====================================================
  // GET ASSIGNMENTS BY CLASS
  // =====================================================

  getByClass(
    classId: string
  ): Observable<Assignment[]> {

    return this.http.get<Assignment[]>(
      `${this.apiUrl}/class/${classId}`
    );

  }

  // =====================================================
  // GET ASSIGNMENTS BY COURSE
  // =====================================================

  getByCourse(
    courseId: string
  ): Observable<Assignment[]> {

    return this.http.get<Assignment[]>(
      `${this.apiUrl}/course/${courseId}`
    );

  }

  // =====================================================
  // GET ASSIGNMENTS BY SUBJECT
  // =====================================================

  getBySubject(
    subjectId: string
  ): Observable<Assignment[]> {

    return this.http.get<Assignment[]>(
      `${this.apiUrl}/subject/${subjectId}`
    );

  }

  // =====================================================
  // GET ASSIGNMENTS BY TEACHER
  // =====================================================

  getByTeacher(
    teacherId: string
  ): Observable<Assignment[]> {

    return this.http.get<Assignment[]>(
      `${this.apiUrl}/teacher/${teacherId}`
    );

  }

  // =====================================================
  // CREATE ASSIGNMENT
  // =====================================================
  // Uses FormData because assignments can contain
  // PDF, Word, image or other file attachments.
  // =====================================================

  create(
    formData: FormData
  ): Observable<Assignment> {

    return this.http.post<Assignment>(
      this.apiUrl,
      formData
    );

  }

  // =====================================================
  // UPDATE ASSIGNMENT
  // =====================================================
  // Uses FormData because assignments can contain
  // attachments.
  // =====================================================

  update(
    id: string,
    formData: FormData
  ): Observable<Assignment> {

    return this.http.put<Assignment>(
      `${this.apiUrl}/${id}`,
      formData
    );

  }

  // =====================================================
  // DELETE ASSIGNMENT
  // =====================================================

  delete(
    id: string
  ): Observable<boolean> {

    return this.http.delete<boolean>(
      `${this.apiUrl}/${id}`
    );

  }

  // =====================================================
  // PUBLISH ASSIGNMENT
  // =====================================================

  publish(
    id: string
  ): Observable<Assignment> {

    return this.http.patch<Assignment>(
      `${this.apiUrl}/${id}/publish`,
      {}
    );

  }

  // =====================================================
  // UNPUBLISH / MOVE TO DRAFT
  // =====================================================

  unpublish(
    id: string
  ): Observable<Assignment> {

    return this.http.patch<Assignment>(
      `${this.apiUrl}/${id}/unpublish`,
      {}
    );

  }

  // =====================================================
  // DOWNLOAD ASSIGNMENT ATTACHMENT
  // =====================================================

  downloadAttachment(
    id: string
  ): Observable<Blob> {

    return this.http.get(
      `${this.apiUrl}/${id}/attachment`,
      {
        responseType: 'blob'
      }
    );

  }

}