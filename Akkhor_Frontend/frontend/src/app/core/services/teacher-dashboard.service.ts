import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

import { environment } from '../../../environments/environment';

export interface TeacherDashboardResponse {
  totalClasses: number;
  totalAssignments: number;
  publishedAssignments: number;
  draftAssignments: number;
  recentAssignments: any[];
}

@Injectable({
  providedIn: 'root'
})
export class TeacherDashboardService {

  private readonly apiUrl =
    `${environment.apiUrl}/api/teacher-dashboard`;


  constructor(
    private http: HttpClient
  ) {}


  // =====================================================
  // GET DASHBOARD
  // =====================================================

  getDashboard():
    Observable<TeacherDashboardResponse> {

    return this.http.get<TeacherDashboardResponse>(
      this.apiUrl
    );
  }


  // =====================================================
  // GET SUMMARY
  // =====================================================

  getSummary(): Observable<any> {

    return this.http.get(
      `${this.apiUrl}/summary`
    );
  }


  // =====================================================
  // GET RECENT ASSIGNMENTS
  // =====================================================

  getRecentAssignments(): Observable<any[]> {

    return this.http.get<any[]>(
      `${this.apiUrl}/recent-assignments`
    );
  }


  // =====================================================
  // GET PUBLISHED ASSIGNMENTS
  // =====================================================

  getPublishedAssignments(): Observable<any[]> {

    return this.http.get<any[]>(
      `${this.apiUrl}/published-assignments`
    );
  }


  // =====================================================
  // GET DRAFT ASSIGNMENTS
  // =====================================================

  getDraftAssignments(): Observable<any[]> {

    return this.http.get<any[]>(
      `${this.apiUrl}/draft-assignments`
    );
  }


  // =====================================================
  // GET TEACHER CLASSES
  // =====================================================

  getClasses(): Observable<any[]> {

    return this.http.get<any[]>(
      `${this.apiUrl}/classes`
    );
  }
}