import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

import { environment } from '../../../environments/environment';

import { TeacherClass } from '../models/teacher-class.model';

@Injectable({
  providedIn: 'root'
})
export class TeacherClassService {

  private apiUrl =
    `${environment.apiUrl}/api/teacher-classes`;

  constructor(
    private http: HttpClient
  ) {}

  // =====================================================
  // GET MY CLASSES
  // =====================================================

  getMyClasses(): Observable<TeacherClass[]> {

    return this.http.get<TeacherClass[]>(
      `${this.apiUrl}/my-classes`
    );
  }
}