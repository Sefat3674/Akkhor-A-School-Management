import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';

import {
  TeacherDashboardService
} from '../../../../core/services/teacher-dashboard.service';

@Component({
  selector: 'app-teacher-dashboard',
  standalone: true,
  imports: [
    CommonModule
  ],
  templateUrl: './teacher-dashboard.component.html',
  styleUrls: ['./teacher-dashboard.component.scss']
})
export class TeacherDashboardComponent implements OnInit {

  // =====================================================
  // DASHBOARD DATA
  // =====================================================

  totalClasses = 0;
  totalAssignments = 0;
  publishedAssignments = 0;
  draftAssignments = 0;

  recentAssignments: any[] = [];


  // =====================================================
  // UI STATE
  // =====================================================

  isLoading = false;
  errorMessage = '';


  constructor(
    private dashboardService: TeacherDashboardService,
    private router: Router
  ) {}


  // =====================================================
  // INIT
  // =====================================================

  ngOnInit(): void {
    this.loadDashboard();
  }


  // =====================================================
  // LOAD DASHBOARD
  // =====================================================

  loadDashboard(): void {

    this.isLoading = true;
    this.errorMessage = '';

    this.dashboardService.getDashboard()
      .subscribe({
        next: (response) => {

          this.totalClasses =
            response.totalClasses ?? 0;

          this.totalAssignments =
            response.totalAssignments ?? 0;

          this.publishedAssignments =
            response.publishedAssignments ?? 0;

          this.draftAssignments =
            response.draftAssignments ?? 0;

          this.recentAssignments =
            response.recentAssignments ?? [];

          this.isLoading = false;
        },

        error: (error) => {

          console.error(
            'Teacher dashboard error:',
            error
          );

          this.errorMessage =
            error?.error?.message ||
            'Failed to load teacher dashboard.';

          this.isLoading = false;
        }
      });
  }


  // =====================================================
  // REFRESH
  // =====================================================

  refreshDashboard(): void {
    this.loadDashboard();
  }


  // =====================================================
  // NAVIGATION
  // =====================================================

  goToClasses(): void {
    this.router.navigate([
      '/teacher/classes'
    ]);
  }


  goToAssignments(): void {
    this.router.navigate([
      '/teacher/assignments'
    ]);
  }


  goToPublishedAssignments(): void {
    this.router.navigate([
      '/teacher/assignments'
    ], {
      queryParams: {
        status: 'published'
      }
    });
  }


  goToDraftAssignments(): void {
    this.router.navigate([
      '/teacher/assignments'
    ], {
      queryParams: {
        status: 'draft'
      }
    });
  }


  viewAssignment(id: string): void {

    if (!id) {
      return;
    }

    this.router.navigate([
      '/teacher/assignments',
      id
    ]);
  }
}