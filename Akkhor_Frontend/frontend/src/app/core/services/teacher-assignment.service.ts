import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

import { environment } from '../../../environments/environment';

import {
  TeacherAssignment,
  CreateTeacherAssignment,
  UpdateTeacherAssignment,
  TeacherDropdown
} from '../models/teacher-assignment.model';

import { AcademicYear } from '../models/academic-year.model';
import { ClassModel } from '../models/class.model';
import { SectionModel } from '../models/section.model';
import { CourseModel } from '../models/course.model';
import { SubjectModel } from '../models/subject.model';

@Injectable({
  providedIn: 'root'
})
export class TeacherAssignmentService {

  private apiUrl =
    `${environment.apiUrl}/api/teacher-assignments`;

  constructor(
    private http: HttpClient
  ) {}


  // =====================================================
  // TEACHER ASSIGNMENTS
  // =====================================================

  getAll(): Observable<TeacherAssignment[]> {

    return this.http.get<TeacherAssignment[]>(
      this.apiUrl
    );
  }


  getById(
    id: string
  ): Observable<TeacherAssignment> {

    return this.http.get<TeacherAssignment>(
      `${this.apiUrl}/${id}`
    );
  }


  create(
    data: CreateTeacherAssignment
  ): Observable<TeacherAssignment> {

    return this.http.post<TeacherAssignment>(
      this.apiUrl,
      data
    );
  }


  // =====================================================
  // UPDATE
  // =====================================================

  update(
    id: string,
    data: UpdateTeacherAssignment
  ): Observable<TeacherAssignment> {

    return this.http.put<TeacherAssignment>(
      `${this.apiUrl}/${id}`,
      data
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
  // TEACHERS
  // =====================================================

  getTeachers(): Observable<TeacherDropdown[]> {

    return this.http.get<TeacherDropdown[]>(
      `${this.apiUrl}/teachers`
    );
  }


  // =====================================================
  // ACADEMIC YEARS
  // =====================================================

  getAcademicYears(): Observable<AcademicYear[]> {

    return this.http.get<AcademicYear[]>(
      `${environment.apiUrl}/api/academic-years`
    );
  }


  // =====================================================
  // CLASSES
  // =====================================================

  getClasses(): Observable<ClassModel[]> {

    return this.http.get<ClassModel[]>(
      `${environment.apiUrl}/api/classes`
    );
  }


  // =====================================================
  // SECTIONS
  // =====================================================

  getSections(): Observable<SectionModel[]> {

    return this.http.get<SectionModel[]>(
      `${environment.apiUrl}/api/sections`
    );
  }


  // =====================================================
  // COURSES
  // =====================================================

  getCourses(): Observable<CourseModel[]> {

    return this.http.get<CourseModel[]>(
      `${environment.apiUrl}/api/courses`
    );
  }


  // =====================================================
  // SUBJECTS
  // =====================================================

  getSubjects(): Observable<SubjectModel[]> {

    return this.http.get<SubjectModel[]>(
      `${environment.apiUrl}/api/subjects`
    );
  }


  // =====================================================
  // SUBJECTS BY COURSE
  // =====================================================

  getSubjectsByCourse(
    courseId: string
  ): Observable<SubjectModel[]> {

    return this.http.get<SubjectModel[]>(
      `${environment.apiUrl}/api/courses/${courseId}/subjects`
    );
  }

}