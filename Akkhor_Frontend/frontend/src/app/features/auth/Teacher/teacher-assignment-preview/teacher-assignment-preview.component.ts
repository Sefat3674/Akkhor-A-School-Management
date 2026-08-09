import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';

import { Assignment } from '../../../../core/models/assignment.model';
import { AssignmentService } from '../../../../core/services/assignment.service';

@Component({
  selector: 'app-teacher-assignment-preview',
  standalone: true,

  imports: [
    CommonModule,
    FormsModule
  ],

  templateUrl: './teacher-assignment-preview.component.html',
  styleUrls: ['./teacher-assignment-preview.component.scss']
})
export class TeacherAssignmentPreviewComponent implements OnInit {

  // =====================================================
  // ASSIGNMENTS
  // =====================================================

  assignments: Assignment[] = [];

  publishedAssignments: Assignment[] = [];

  selectedAssignmentId = '';

  // =====================================================
  // UI STATE
  // =====================================================

  loading = false;

  errorMessage = '';

  // =====================================================
  // CONSTRUCTOR
  // =====================================================

  constructor(
    private assignmentService: AssignmentService,
    private router: Router
  ) {}

  // =====================================================
  // INIT
  // =====================================================

  ngOnInit(): void {
    this.loadAssignments();
  }

  // =====================================================
  // LOAD TEACHER ASSIGNMENTS
  // =====================================================

  loadAssignments(): void {

    this.loading = true;

    this.errorMessage = '';

    this.assignmentService
      .getMyAssignments()
      .subscribe({

        next: (data: Assignment[]) => {

          this.assignments =
            Array.isArray(data)
              ? data
              : [];

          // -------------------------------------------------
          // ONLY PUBLISHED ASSIGNMENTS
          // -------------------------------------------------

          this.publishedAssignments =
            this.assignments.filter(
              assignment =>
                assignment.isPublished === true
            );

          this.loading = false;
        },

        error: (error) => {

          console.error(
            'Error loading teacher assignments:',
            error
          );

          this.loading = false;

          this.assignments = [];

          this.publishedAssignments = [];

          this.errorMessage =
            error?.error?.message ??
            'Failed to load assignments.';
        }

      });
  }

  // =====================================================
  // ASSIGNMENT SELECT
  // =====================================================

  onAssignmentChange(): void {

    if (!this.selectedAssignmentId) {
      return;
    }

    this.openReview(
      this.selectedAssignmentId
    );
  }

  // =====================================================
  // OPEN REVIEW
  // =====================================================

  openReview(
    assignmentId: string
  ): void {

    if (!assignmentId) {
      return;
    }

    this.router.navigate([
      '/teacher/assignments/review',
      assignmentId
    ]);
  }

  // =====================================================
  // BACK
  // =====================================================

  goBack(): void {

    this.router.navigate([
      '/teacher/assignments'
    ]);
  }

  // =====================================================
  // REFRESH
  // =====================================================

  refresh(): void {

    this.selectedAssignmentId = '';

    this.loadAssignments();
  }

  // =====================================================
  // TRACK BY
  // =====================================================

  trackByAssignmentId(
    index: number,
    assignment: Assignment
  ): string | number {

    return assignment.id || index;
  }
}