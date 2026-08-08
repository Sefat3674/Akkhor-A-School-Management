import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

import { StudentAssignment } from '../../../../core/models/student-assignment.model';

import { StudentAssignmentService } from '../../../../core/services/student-assignment.service';

// IMPORTANT:
// Make sure this model actually exports AssignmentSubmission.
// If your model uses another name, replace AssignmentSubmission below.
import { AssignmentSubmission } from '../../../../core/models/student-assignment-submission.model';

@Component({
  selector: 'app-student-assignment',
  standalone: true,

  imports: [
    CommonModule,
    FormsModule
  ],

  templateUrl: './student-assignment.component.html',

  styleUrls: [
    './student-assignment.component.scss'
  ]
})
export class StudentAssignmentComponent implements OnInit {

  // =====================================================
  // DATA
  // =====================================================

  assignments: StudentAssignment[] = [];

  submissions: AssignmentSubmission[] = [];


  // =====================================================
  // UI STATE
  // =====================================================

  loading = false;

  errorMessage = '';

  searchText = '';


  // =====================================================
  // FILTER
  // =====================================================

  selectedStatus = 'all';


  // =====================================================
  // PAGINATION
  // =====================================================

  currentPage = 1;

  pageSize = 10;


  // =====================================================
  // CONSTRUCTOR
  // =====================================================

  constructor(
    private assignmentService: StudentAssignmentService
  ) {}


  // =====================================================
  // INIT
  // =====================================================

  ngOnInit(): void {

    this.loadAssignments();

    this.loadMySubmissions();

  }


  // =====================================================
  // LOAD ASSIGNMENTS
  // =====================================================

  loadAssignments(): void {

    this.loading = true;

    this.errorMessage = '';

    this.assignmentService
      .getMyAssignments()
      .subscribe({

        next: (data: StudentAssignment[]) => {

          this.assignments = data ?? [];

          this.loading = false;

        },

        error: (error) => {

          console.error(
            'Error loading assignments:',
            error
          );

          this.assignments = [];

          this.errorMessage =
            'Unable to load assignments.';

          this.loading = false;

        }

      });

  }


  // =====================================================
  // LOAD MY SUBMISSIONS
  // =====================================================

  loadMySubmissions(): void {

    this.assignmentService
      .getMySubmissions()
      .subscribe({

        next: (
          data: AssignmentSubmission[]
        ) => {

          this.submissions = data ?? [];

        },

        error: (error) => {

          console.error(
            'Error loading submissions:',
            error
          );

          this.submissions = [];

        }

      });

  }


  // =====================================================
  // SEARCH
  // =====================================================

  onSearch(): void {

    this.currentPage = 1;

  }


  // =====================================================
  // CLEAR SEARCH
  // =====================================================

  clearSearch(): void {

    this.searchText = '';

    this.currentPage = 1;

  }


  // =====================================================
  // STATUS FILTER
  // =====================================================

  onStatusChange(): void {

    this.currentPage = 1;

  }


  // =====================================================
  // FILTERED ASSIGNMENTS
  // =====================================================

  get filteredAssignments(): StudentAssignment[] {

    let result = [
      ...this.assignments
    ];


    // ===================================================
    // SEARCH
    // ===================================================

    if (
      this.searchText &&
      this.searchText.trim()
    ) {

      const search =
        this.searchText
          .trim()
          .toLowerCase();


      result =
        result.filter(
          assignment => {

            const title =
              assignment.title
                ?.toLowerCase()
                .includes(search);

            const course =
              assignment.courseName
                ?.toLowerCase()
                .includes(search);

            const subject =
              assignment.subjectName
                ?.toLowerCase()
                .includes(search);

            const teacher =
              assignment.teacherName
                ?.toLowerCase()
                .includes(search);

            return !!(
              title ||
              course ||
              subject ||
              teacher
            );

          }
        );

    }


    // ===================================================
    // STATUS FILTER
    // ===================================================

    if (
      this.selectedStatus !== 'all'
    ) {

      result =
        result.filter(
          assignment =>
            this.getAssignmentStatus(
              assignment
            ) === this.selectedStatus
        );

    }


    return result;

  }


  // =====================================================
  // SUMMARY COUNTS
  // =====================================================

  get totalCount(): number {

    return this.assignments.length;

  }


  get pendingCount(): number {

    return this.countByStatus('pending');

  }


  get submittedCount(): number {

    return this.countByStatus('submitted');

  }


  get evaluatedCount(): number {

    return this.countByStatus('evaluated');

  }


  get overdueCount(): number {

    return this.countByStatus('overdue');

  }


  // =====================================================
  // COUNT BY STATUS
  // =====================================================

  private countByStatus(
    status: string
  ): number {

    let count = 0;


    for (
      const assignment of this.assignments
    ) {

      if (
        this.getAssignmentStatus(
          assignment
        ) === status
      ) {

        count++;

      }

    }


    return count;

  }


  // =====================================================
  // ASSIGNMENT STATUS
  // =====================================================

  getAssignmentStatus(
    assignment: StudentAssignment
  ): string {

    const submission =
      this.getSubmission(
        assignment.id
      );


    // ---------------------------------------------------
    // Submitted / Evaluated
    // ---------------------------------------------------

    if (submission) {

      if (
        submission.status &&
        submission.status
          .toLowerCase() === 'evaluated'
      ) {

        return 'evaluated';

      }


      return 'submitted';

    }


    // ---------------------------------------------------
    // Overdue
    // ---------------------------------------------------

    if (
      this.isOverdue(
        assignment
      )
    ) {

      return 'overdue';

    }


    // ---------------------------------------------------
    // Pending
    // ---------------------------------------------------

    return 'pending';

  }


  // =====================================================
  // GET SUBMISSION
  // =====================================================

  getSubmission(
    assignmentId: string
  ): AssignmentSubmission | undefined {

    return this.submissions.find(
      submission =>
        submission.assignmentId ===
        assignmentId
    );

  }


  // =====================================================
  // CHECK SUBMITTED
  // =====================================================

  isSubmitted(
    assignmentId: string
  ): boolean {

    return !!this.getSubmission(
      assignmentId
    );

  }


  // =====================================================
  // DEADLINE
  // =====================================================

  isOverdue(
    assignment: StudentAssignment
  ): boolean {

    if (!assignment.deadline) {

      return false;

    }


    return (
      new Date(
        assignment.deadline
      ).getTime()
      <
      new Date().getTime()
    );

  }


  // =====================================================
  // DEADLINE FORMAT
  // =====================================================

  formatDeadline(
    deadline: string
  ): string {

    if (!deadline) {

      return '-';

    }


    return new Date(
      deadline
    ).toLocaleString(
      'en-BD',
      {
        day: '2-digit',
        month: 'short',
        year: 'numeric',
        hour: '2-digit',
        minute: '2-digit'
      }
    );

  }


  // =====================================================
  // STATUS LABEL
  // =====================================================

  getStatusLabel(
    assignment: StudentAssignment
  ): string {

    const status =
      this.getAssignmentStatus(
        assignment
      );


    switch (status) {

      case 'submitted':

        return 'Submitted';


      case 'evaluated':

        return 'Evaluated';


      case 'overdue':

        return 'Overdue';


      default:

        return 'Pending';

    }

  }


  // =====================================================
  // STATUS CLASS
  // =====================================================

  getStatusClass(
    assignment: StudentAssignment
  ): string {

    const status =
      this.getAssignmentStatus(
        assignment
      );


    switch (status) {

      case 'submitted':

        return 'status-submitted';


      case 'evaluated':

        return 'status-evaluated';


      case 'overdue':

        return 'status-overdue';


      default:

        return 'status-pending';

    }

  }


  // =====================================================
  // REFRESH
  // =====================================================

  refresh(): void {

    this.loadAssignments();

    this.loadMySubmissions();

  }


  // =====================================================
  // VIEW ASSIGNMENT
  // =====================================================

  viewAssignment(
    assignment: StudentAssignment
  ): void {

    console.log(
      'View assignment:',
      assignment
    );

  }


  // =====================================================
  // SUBMIT ASSIGNMENT
  // =====================================================

  submitAssignment(
    assignment: StudentAssignment
  ): void {

    console.log(
      'Submit assignment:',
      assignment
    );

  }


  // =====================================================
  // TRACK BY
  // =====================================================

  trackByAssignmentId(
    index: number,
    assignment: StudentAssignment
  ): string {

    return assignment.id;

  }

}