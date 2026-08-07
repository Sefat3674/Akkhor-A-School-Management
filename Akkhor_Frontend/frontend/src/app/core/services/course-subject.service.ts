import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

import {
  CourseSubjectModel,
  CreateCourseSubjectModel,
  UpdateCourseSubjectModel
} from '../models/course-subject.model';

import { environment } from '../../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class CourseSubjectService {

  private apiUrl = `${environment.apiUrl}/api/course-subjects`;

  constructor(
    private http: HttpClient
  ) { }

  // Get All
  getAll(): Observable<CourseSubjectModel[]> {
    return this.http.get<CourseSubjectModel[]>(this.apiUrl);
  }

  // Get By Id
  getById(id: string): Observable<CourseSubjectModel> {
    return this.http.get<CourseSubjectModel>(`${this.apiUrl}/${id}`);
  }

  // Create
  create(data: CreateCourseSubjectModel): Observable<CourseSubjectModel> {
    return this.http.post<CourseSubjectModel>(this.apiUrl, data);
  }

  // Update
  update(id: string, data: UpdateCourseSubjectModel): Observable<void> {
    return this.http.put<void>(`${this.apiUrl}/${id}`, data);
  }

  // Delete
  delete(id: string): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }

}