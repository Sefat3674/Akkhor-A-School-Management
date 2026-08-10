import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { HttpClient } from '@angular/common/http';

import { environment } from '../../../../../environments/environment';


// =====================================================
// ADMIN DASHBOARD MODELS
// =====================================================

export interface AdminDashboard {
  totalUsers: number;
  totalStudents: number;
  totalTeachers: number;

  totalClasses: number;
  totalSections: number;
  totalCourses: number;
  totalSubjects: number;

  totalEnrollments: number;

  totalAssignments: number;
  totalSubmissions: number;
  pendingSubmissions: number;

  totalAcademicYears: number;

  recentAssignments: RecentAssignment[];
  recentSubmissions: RecentSubmission[];
}


// =====================================================
// RECENT ASSIGNMENT
// =====================================================

export interface RecentAssignment {
  id?: string;

  title: string;

  teacherName?: string | null;

  courseName?: string | null;

  subjectName?: string | null;

  dueDate?: string | Date | null;

  status?: string | null;
}


// =====================================================
// RECENT SUBMISSION
// =====================================================

export interface RecentSubmission {
  id?: string;

  studentName: string;

  assignmentTitle: string;

  submittedAt?: string | Date | null;

  status?: string | null;
}


// =====================================================
// COMPONENT
// =====================================================

@Component({
  selector: 'app-admin-dashboard',

  standalone: true,

  imports: [
    CommonModule,
    RouterModule
  ],

  templateUrl: './admin-dashboard.component.html',

  styleUrls: [
    './admin-dashboard.component.scss'
  ]
})
export class AdminDashboardComponent implements OnInit {

  // ===================================================
  // API URL
  // ===================================================

  private readonly apiUrl =
    `${environment.apiUrl}/api/admin/dashboard`;


  // ===================================================
  // STATE
  // ===================================================

  dashboard: AdminDashboard | null = null;

  loading = false;

  errorMessage = '';


  // ===================================================
  // CONSTRUCTOR
  // ===================================================

  constructor(
    private readonly http: HttpClient
  ) {}


  // ===================================================
  // INIT
  // ===================================================

  ngOnInit(): void {

    this.loadDashboard();

  }


  // ===================================================
  // LOAD DASHBOARD
  // ===================================================

  loadDashboard(): void {

    this.loading = true;

    this.errorMessage = '';


    this.http
      .get<AdminDashboard>(this.apiUrl)
      .subscribe({

        next: (data) => {

          this.dashboard =
            this.normalizeDashboard(data);

          this.loading = false;

        },

        error: (error) => {

          console.error(
            'Failed to load admin dashboard:',
            error
          );

          this.errorMessage =
            error?.error?.message ||
            'Failed to load dashboard data. Please try again.';

          this.loading = false;

        }

      });

  }


  // ===================================================
  // NORMALIZE DASHBOARD
  // ===================================================

  private normalizeDashboard(
    data: AdminDashboard
  ): AdminDashboard {

    return {

      totalUsers:
        data?.totalUsers ?? 0,

      totalStudents:
        data?.totalStudents ?? 0,

      totalTeachers:
        data?.totalTeachers ?? 0,

      totalClasses:
        data?.totalClasses ?? 0,

      totalSections:
        data?.totalSections ?? 0,

      totalCourses:
        data?.totalCourses ?? 0,

      totalSubjects:
        data?.totalSubjects ?? 0,

      totalEnrollments:
        data?.totalEnrollments ?? 0,

      totalAssignments:
        data?.totalAssignments ?? 0,

      totalSubmissions:
        data?.totalSubmissions ?? 0,

      pendingSubmissions:
        data?.pendingSubmissions ?? 0,

      totalAcademicYears:
        data?.totalAcademicYears ?? 0,

      recentAssignments:
        data?.recentAssignments ?? [],

      recentSubmissions:
        data?.recentSubmissions ?? []

    };

  }


  // ===================================================
  // GET INITIALS
  // ===================================================

  getInitials(
    name?: string | null
  ): string {

    if (!name?.trim()) {

      return 'U';

    }


    const parts =
      name
        .trim()
        .split(/\s+/)
        .filter(Boolean);


    if (parts.length === 1) {

      return parts[0]
        .substring(0, 2)
        .toUpperCase();

    }


    return (
      parts[0].charAt(0) +
      parts[parts.length - 1].charAt(0)
    ).toUpperCase();

  }

}