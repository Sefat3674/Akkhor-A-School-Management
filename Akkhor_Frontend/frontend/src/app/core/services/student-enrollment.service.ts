import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

import {
  StudentEnrollment,
  CreateStudentEnrollment,
  UpdateStudentEnrollment
} from '../models/student-enrollment.model';

// =====================================================
// LOOKUP MODELS
// =====================================================

export interface StudentLookup {
  id: string;
  userName: string;
}

export interface ClassLookup {
  id: string;
  name: string;
}

export interface CourseLookup {
  id: string;
  classId: string;
  courseName: string;
}

export interface SectionLookup {
  id: string;
  classId: string;
  sectionName: string;
}

@Injectable({
  providedIn: 'root'
})
export class StudentEnrollmentService {

  private apiUrl =
    'https://localhost:50268/api/student-enrollments';

  constructor(
    private http: HttpClient
  ) {}

  // =====================================================
  // GET ALL ENROLLMENTS
  // GET: api/student-enrollments
  // =====================================================

  getAll(): Observable<StudentEnrollment[]> {

    return this.http.get<StudentEnrollment[]>(
      this.apiUrl
    );
  }

  // =====================================================
  // GET ENROLLMENT BY ID
  // GET: api/student-enrollments/{id}
  // =====================================================

  getById(
    id: string
  ): Observable<StudentEnrollment> {

    return this.http.get<StudentEnrollment>(
      `${this.apiUrl}/${id}`
    );
  }

  // =====================================================
  // CREATE ENROLLMENT
  // POST: api/student-enrollments
  // =====================================================

  create(
    data: CreateStudentEnrollment
  ): Observable<StudentEnrollment> {

    return this.http.post<StudentEnrollment>(
      this.apiUrl,
      data
    );
  }

  // =====================================================
  // UPDATE ENROLLMENT
  // PUT: api/student-enrollments/{id}
  // =====================================================

  update(
    id: string,
    data: UpdateStudentEnrollment
  ): Observable<void> {

    return this.http.put<void>(
      `${this.apiUrl}/${id}`,
      data
    );
  }

  // =====================================================
  // DELETE ENROLLMENT
  // DELETE: api/student-enrollments/{id}
  // =====================================================

  delete(
    id: string
  ): Observable<void> {

    return this.http.delete<void>(
      `${this.apiUrl}/${id}`
    );
  }

  // =====================================================
  // GET STUDENTS
  // GET: api/student-enrollments/students
  // =====================================================

  getStudents(): Observable<StudentLookup[]> {

    return this.http.get<StudentLookup[]>(
      `${this.apiUrl}/students`
    );
  }

  // =====================================================
  // GET CLASSES
  // GET: api/student-enrollments/classes
  // =====================================================

  getClasses(): Observable<ClassLookup[]> {

    return this.http.get<ClassLookup[]>(
      `${this.apiUrl}/classes`
    );
  }

  // =====================================================
  // GET COURSES BY CLASS
  // GET: api/student-enrollments/classes/{classId}/courses
  // =====================================================

  getCoursesByClassId(
    classId: string
  ): Observable<CourseLookup[]> {

    return this.http.get<CourseLookup[]>(
      `${this.apiUrl}/classes/${classId}/courses`
    );
  }

  // =====================================================
  // GET SECTIONS BY CLASS
  // GET: api/student-enrollments/classes/{classId}/sections
  // =====================================================

  getSectionsByClassId(
    classId: string
  ): Observable<SectionLookup[]> {

    return this.http.get<SectionLookup[]>(
      `${this.apiUrl}/classes/${classId}/sections`
    );
  }

}