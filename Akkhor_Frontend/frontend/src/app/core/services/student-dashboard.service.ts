import { Injectable } from '@angular/core';

import {
  HttpClient
} from '@angular/common/http';

import {
  Observable
} from 'rxjs';

import {
  environment
} from '../../../environments/environment';

import {
  StudentDashboard,
  StudentDashboardStatistics,
  StudentDashboardAssignment,
  StudentDashboardSubmission
} from '../models/student-dashboard.model';


@Injectable({
  providedIn: 'root'
})
export class StudentDashboardService {

  // =====================================================
  // API URL
  // =====================================================

  private readonly apiUrl =
    `${environment.apiUrl}/api/student/dashboard`;


  // =====================================================
  // CONSTRUCTOR
  // =====================================================

  constructor(
    private http: HttpClient
  ) {}


  // =====================================================
  // GET COMPLETE DASHBOARD
  // GET:
  // api/student/dashboard
  // =====================================================

  getDashboard(): Observable<StudentDashboard> {

    return this.http.get<StudentDashboard>(
      this.apiUrl
    );

  }


  // =====================================================
  // GET STATISTICS
  // GET:
  // api/student/dashboard/statistics
  // =====================================================

  getStatistics(): Observable<StudentDashboardStatistics> {

    return this.http.get<StudentDashboardStatistics>(
      `${this.apiUrl}/statistics`
    );

  }


  // =====================================================
  // GET RECENT ASSIGNMENTS
  // GET:
  // api/student/dashboard/recent-assignments
  // =====================================================

  getRecentAssignments(
    limit: number = 5
  ): Observable<StudentDashboardAssignment[]> {

    return this.http.get<StudentDashboardAssignment[]>(
      `${this.apiUrl}/recent-assignments`,
      {
        params: {
          limit: limit.toString()
        }
      }
    );

  }


  // =====================================================
  // GET UPCOMING ASSIGNMENTS
  // GET:
  // api/student/dashboard/upcoming-assignments
  // =====================================================

  getUpcomingAssignments(
    limit: number = 5
  ): Observable<StudentDashboardAssignment[]> {

    return this.http.get<StudentDashboardAssignment[]>(
      `${this.apiUrl}/upcoming-assignments`,
      {
        params: {
          limit: limit.toString()
        }
      }
    );

  }


  // =====================================================
  // GET RECENT SUBMISSIONS
  // GET:
  // api/student/dashboard/recent-submissions
  // =====================================================

  getRecentSubmissions(
    limit: number = 5
  ): Observable<StudentDashboardSubmission[]> {

    return this.http.get<StudentDashboardSubmission[]>(
      `${this.apiUrl}/recent-submissions`,
      {
        params: {
          limit: limit.toString()
        }
      }
    );

  }

}