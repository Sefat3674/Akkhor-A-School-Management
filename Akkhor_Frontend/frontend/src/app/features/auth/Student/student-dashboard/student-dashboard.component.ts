import {
  Component,
  OnInit
} from '@angular/core';

import {
  CommonModule
} from '@angular/common';

import {
  Router,
  RouterModule
} from '@angular/router';

import {
  StudentDashboardService
} from '../../../../core/services/student-dashboard.service';

import {
  StudentDashboard,
  StudentDashboardStatistics,
  StudentDashboardAssignment,
  StudentDashboardSubmission
} from '../../../../core/models/student-dashboard.model';


@Component({
  selector: 'app-student-dashboard',

  standalone: true,

  imports: [
    CommonModule,
    RouterModule
  ],

  templateUrl:
    './student-dashboard.component.html',

  styleUrls: [
    './student-dashboard.component.scss'
  ]
})
export class StudentDashboardComponent
  implements OnInit {


  // =====================================================
  // DATA
  // =====================================================

  dashboard: StudentDashboard | null = null;


  statistics: StudentDashboardStatistics = {

    totalAssignments: 0,

    pendingAssignments: 0,

    submittedAssignments: 0,

    gradedAssignments: 0,

    overdueAssignments: 0,

    submissionRate: 0,

    averageMarks: 0

  };


  recentAssignments:
    StudentDashboardAssignment[] = [];


  upcomingAssignments:
    StudentDashboardAssignment[] = [];


  recentSubmissions:
    StudentDashboardSubmission[] = [];


  // =====================================================
  // STATE
  // =====================================================

  loading = false;

  errorMessage = '';


  // =====================================================
  // CONSTRUCTOR
  // =====================================================

  constructor(
    private dashboardService:
      StudentDashboardService,

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

    this.loading = true;

    this.errorMessage = '';


    this.dashboardService
      .getDashboard()
      .subscribe({

        // =================================================
        // SUCCESS
        // =================================================

        next: (
          response: StudentDashboard
        ) => {

          console.log(
            'Student Dashboard Response:',
            response
          );


          this.dashboard = response;


          // ===============================================
          // MAP FLAT API RESPONSE TO STATISTICS
          // ===============================================

          this.statistics = {

            totalAssignments:
              response.totalAssignments ?? 0,

            pendingAssignments:
              response.pendingAssignments ?? 0,

            submittedAssignments:
              response.submittedAssignments ?? 0,

            gradedAssignments:
              response.gradedAssignments ?? 0,

            overdueAssignments:
              response.overdueAssignments ?? 0

          };


          // ===============================================
          // RECENT ASSIGNMENTS
          // ===============================================

          this.recentAssignments =
            response.recentAssignments ?? [];


          // ===============================================
          // UPCOMING ASSIGNMENTS
          // ===============================================

          this.upcomingAssignments =
            response.upcomingAssignments ?? [];


          // ===============================================
          // RECENT SUBMISSIONS
          // ===============================================

          this.recentSubmissions =
            response.recentSubmissions ?? [];


          this.loading = false;

        },


        // =================================================
        // ERROR
        // =================================================

        error: (
          error
        ) => {

          console.error(
            'Student dashboard error:',
            error
          );


          this.loading = false;


          this.errorMessage =
            error?.error?.message ??
            'Failed to load dashboard.';

        }

      });

  }


  // =====================================================
  // REFRESH
  // =====================================================

  refresh(): void {

    this.loadDashboard();

  }


  // =====================================================
  // STUDENT NAME
  // =====================================================

  getStudentName(): string {

    const name =
      this.dashboard?.studentName?.trim();


    return name || 'Student';

  }


  // =====================================================
  // STUDENT EMAIL
  // =====================================================

  getStudentEmail(): string {

    return (
      this.dashboard?.email ||
      '-'
    );

  }


  // =====================================================
  // CLASS NAME
  // =====================================================

  getClassName(): string {

    return (
      this.dashboard?.className ||
      '-'
    );

  }


  // =====================================================
  // SECTION NAME
  // =====================================================

  getSectionName(): string {

    return (
      this.dashboard?.sectionName ||
      '-'
    );

  }


  // =====================================================
  // ACADEMIC YEAR
  // =====================================================

  getAcademicYearName(): string {

    return (
      this.dashboard?.academicYearName ||
      '-'
    );

  }


  // =====================================================
  // VIEW ASSIGNMENT
  // =====================================================

  viewAssignment(
    assignment: StudentDashboardAssignment
  ): void {

    if (!assignment.id) {

      return;

    }


    this.router.navigate([
      '/student/assignments',
      assignment.id
    ]);

  }


  // =====================================================
  // VIEW SUBMISSION
  // =====================================================

  viewSubmission(
    submission: StudentDashboardSubmission
  ): void {

    if (!submission.assignmentId) {

      return;

    }


    this.router.navigate([
      '/student/assignments',
      submission.assignmentId
    ]);

  }


  // =====================================================
  // SUBMIT ASSIGNMENT
  // =====================================================

  submitAssignment(
    assignment: StudentDashboardAssignment
  ): void {

    if (!assignment.id) {

      return;

    }


    this.router.navigate([
      '/student/assignments',
      assignment.id,
      'submit'
    ]);

  }


  // =====================================================
  // DATE FORMAT
  // =====================================================

  formatDate(
    date?: string | null
  ): string {

    if (!date) {

      return '-';

    }


    const parsedDate =
      new Date(date);


    if (
      isNaN(
        parsedDate.getTime()
      )
    ) {

      return '-';

    }


    return parsedDate.toLocaleDateString(
      'en-GB',
      {
        day: '2-digit',
        month: 'short',
        year: 'numeric'
      }
    );

  }


  // =====================================================
  // DATE + TIME FORMAT
  // =====================================================

  formatDateTime(
    date?: string | null
  ): string {

    if (!date) {

      return '-';

    }


    const parsedDate =
      new Date(date);


    if (
      isNaN(
        parsedDate.getTime()
      )
    ) {

      return '-';

    }


    return parsedDate.toLocaleString(
      'en-GB',
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
  // ASSIGNMENT STATUS
  // =====================================================

  getAssignmentStatus(
    assignment: StudentDashboardAssignment
  ): string {


    // ===============================================
    // API STATUS
    // ===============================================

    if (
      assignment.status
    ) {

      return assignment.status;

    }


    // ===============================================
    // GRADED
    // ===============================================

    if (
      assignment.isGraded === true
    ) {

      return 'Graded';

    }


    // ===============================================
    // SUBMITTED
    // ===============================================

    if (
      assignment.isSubmitted === true
    ) {

      return 'Submitted';

    }


    // ===============================================
    // OVERDUE
    // ===============================================

    if (
      assignment.isOverdue === true
    ) {

      return 'Overdue';

    }


    // ===============================================
    // DATE BASED OVERDUE
    // ===============================================

    if (
      assignment.dueDate
    ) {

      const dueDate =
        new Date(
          assignment.dueDate
        );


      if (
        dueDate < new Date()
      ) {

        return 'Overdue';

      }

    }


    return 'Pending';

  }


  // =====================================================
  // STATUS CLASS
  // =====================================================

  getStatusClass(
    status?: string
  ): string {

    if (!status) {

      return 'status-default';

    }


    switch (
      status.toLowerCase()
    ) {

      case 'submitted':

        return 'status-submitted';


      case 'completed':

        return 'status-completed';


      case 'graded':

        return 'status-graded';


      case 'pending':

        return 'status-pending';


      case 'overdue':

        return 'status-overdue';


      default:

        return 'status-default';

    }

  }


  // =====================================================
  // TRACK ASSIGNMENT
  // =====================================================

  trackAssignment(
    index: number,
    assignment: StudentDashboardAssignment
  ): string {

    return (
      assignment.id ??
      index.toString()
    );

  }


  // =====================================================
  // TRACK SUBMISSION
  // =====================================================

  trackSubmission(
    index: number,
    submission: StudentDashboardSubmission
  ): string {

    return (
      submission.id ??
      index.toString()
    );

  }

}