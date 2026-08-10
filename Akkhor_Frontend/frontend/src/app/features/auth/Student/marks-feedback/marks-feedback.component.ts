import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';

import { StudentAssignment } from '../../../../core/models/student-assignment.model';
import { StudentAssignmentService } from '../../../../core/services/student-assignment.service';

@Component({
  selector: 'app-marks-feedback',
  standalone: true,

  imports: [
    CommonModule,
    FormsModule
  ],

  templateUrl: './marks-feedback.component.html',

  styleUrls: [
    './marks-feedback.component.scss'
  ]
})
export class MarksFeedbackComponent implements OnInit {

  // =====================================================
  // ASSIGNMENTS
  // =====================================================

  assignments: StudentAssignment[] = [];

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
    private assignmentService: StudentAssignmentService,
    private router: Router
  ) {}


  // =====================================================
  // INIT
  // =====================================================

  ngOnInit(): void {

    this.loadAssignments();

  }


  // =====================================================
  // LOAD STUDENT ASSIGNMENTS
  // =====================================================

  loadAssignments(): void {

    this.loading = true;

    this.errorMessage = '';


    this.assignmentService
      .getMyAssignments()
      .subscribe({

        next: (data: StudentAssignment[]) => {

          console.log(
            'Student assignments:',
            data
          );


          this.assignments =
            Array.isArray(data)
              ? data
              : [];


          this.loading = false;

        },


        error: (error) => {

          console.error(
            'Error loading student assignments:',
            error
          );


          this.loading = false;

          this.assignments = [];


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


    this.openAssignment(
      this.selectedAssignmentId
    );

  }


  // =====================================================
  // OPEN ASSIGNMENT
  // =====================================================

  openAssignment(
    assignmentId: string
  ): void {

    if (!assignmentId) {

      return;

    }


    this.router.navigate([
      '/student/assignments',
      assignmentId
    ]);

  }


  // =====================================================
  // BACK
  // =====================================================

  goBack(): void {

    this.router.navigate([
      '/student/assignments'
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
    assignment: StudentAssignment
  ): string | number {

    return assignment.id || index;

  }

}