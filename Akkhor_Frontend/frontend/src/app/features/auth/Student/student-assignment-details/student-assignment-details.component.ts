import {
  Component,
  OnInit
} from '@angular/core';

import {
  CommonModule
} from '@angular/common';

import {
  ActivatedRoute,
  Router
} from '@angular/router';

import {
  StudentAssignment
} from '../../../../core/models/student-assignment.model';

import {
  AssignmentSubmission
} from '../../../../core/models/assignment-submission.model';

import {
  StudentAssignmentService
} from '../../../../core/services/student-assignment.service';

import {
  AssignmentSubmissionService
} from '../../../../core/services/assignment-submission.service';


@Component({
  selector: 'app-student-assignment-details',

  standalone: true,

  imports: [
    CommonModule
  ],

  templateUrl:
    './student-assignment-details.component.html',

  styleUrls: [
    './student-assignment-details.component.scss'
  ]
})
export class StudentAssignmentDetailsComponent
  implements OnInit {


  // =====================================================
  // DATA
  // =====================================================

  assignment:
    StudentAssignment | null = null;

  submission:
    AssignmentSubmission | null = null;


  // =====================================================
  // UI STATE
  // =====================================================

  loading = false;

  errorMessage = '';


  // =====================================================
  // ASSIGNMENT ID
  // =====================================================

  assignmentId = '';


  // =====================================================
  // CONSTRUCTOR
  // =====================================================

  constructor(
    private route: ActivatedRoute,

    private router: Router,

    private assignmentService:
      StudentAssignmentService,

    private submissionService:
      AssignmentSubmissionService
  ) {}


  // =====================================================
  // INIT
  // =====================================================

  ngOnInit(): void {

    this.assignmentId =
      this.route.snapshot.paramMap.get('id') ?? '';

    if (!this.assignmentId) {

      this.errorMessage =
        'Assignment ID is missing.';

      return;

    }

    this.loadAssignment();

  }


  // =====================================================
  // LOAD ASSIGNMENT
  // =====================================================

  loadAssignment(): void {

    if (!this.assignmentId) {

      this.errorMessage =
        'Assignment ID is missing.';

      return;

    }

    this.loading = true;

    this.errorMessage = '';

    this.assignment = null;

    this.submission = null;


    // ===================================================
    // LOAD ASSIGNMENT
    // ===================================================

    this.assignmentService
      .getById(this.assignmentId)
      .subscribe({

        next: (
          assignment: StudentAssignment
        ) => {

          this.assignment =
            assignment;

          this.loading = false;

          this.loadSubmission();

        },

        error: (error) => {

          console.error(
            'Error loading assignment:',
            error
          );

          this.assignment = null;

          this.loading = false;

          this.errorMessage =
            error?.error?.message ||
            'Unable to load assignment.';

        }

      });

  }


  // =====================================================
  // LOAD MY SUBMISSION
  // =====================================================

  loadSubmission(): void {

    if (!this.assignmentId) {

      return;

    }

    this.submissionService
      .getMySubmissionByAssignment(
        this.assignmentId
      )
      .subscribe({

        next: (
          submission: AssignmentSubmission
        ) => {

          this.submission =
            submission;

        },

        error: (error) => {

          console.log(
            'No submission found for this assignment.',
            error
          );

          this.submission = null;

        }

      });

  }


  // =====================================================
  // STATUS
  // =====================================================

  get status(): string {

    if (this.submission) {

      if (
        this.submission.status &&
        this.submission.status
          .toLowerCase() === 'evaluated'
      ) {

        return 'evaluated';

      }

      return 'submitted';

    }

    if (this.isOverdue()) {

      return 'overdue';

    }

    return 'pending';

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
  // SUBMIT ASSIGNMENT
  // =====================================================

  submitAssignment(): void {

    if (!this.assignment) {

      return;

    }

    if (this.submission) {

      return;

    }

    if (this.isOverdue()) {

      return;

    }

    this.router.navigate([
      '/student/assignments',
      this.assignment.id,
      'submit'
    ]);

  }


  // =====================================================
  // EDIT SUBMISSION
  // =====================================================

  editSubmission(): void {

    if (!this.assignment) {

      return;

    }

    if (!this.submission) {

      return;

    }

    if (this.isEvaluated()) {

      return;

    }

    this.router.navigate([
      '/student/assignments',
      this.assignment.id,
      'submit'
    ]);

  }


  // =====================================================
  // CHECK SUBMITTED
  // =====================================================

  isSubmitted(): boolean {

    return !!this.submission;

  }


  // =====================================================
  // CHECK EVALUATED
  // =====================================================

  isEvaluated(): boolean {

    if (!this.submission) {

      return false;

    }

    return (
      this.submission.status
        ?.toLowerCase() ===
      'evaluated'
    );

  }


  // =====================================================
  // CHECK OVERDUE
  // =====================================================

  isOverdue(): boolean {

    if (
      !this.assignment?.deadline
    ) {

      return false;

    }

    return (
      new Date(
        this.assignment.deadline
      ).getTime()
      <
      new Date().getTime()
    );

  }


  // =====================================================
  // STATUS LABEL
  // =====================================================

  getStatusLabel(): string {

    switch (this.status) {

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

  getStatusClass(): string {

    switch (this.status) {

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
  // SUBMISSION STATUS LABEL
  // =====================================================

  getSubmissionStatusLabel(): string {

    if (!this.submission) {

      return 'Not Submitted';

    }

    if (this.isEvaluated()) {

      return 'Evaluated';

    }

    return 'Submitted';

  }


  // =====================================================
  // SUBMISSION STATUS CLASS
  // =====================================================

  getSubmissionStatusClass(): string {

    if (!this.submission) {

      return 'status-pending';

    }

    if (this.isEvaluated()) {

      return 'status-evaluated';

    }

    return 'status-submitted';

  }


  // =====================================================
  // DEADLINE
  // =====================================================

  formatDeadline(
    deadline?: string | null
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
  // DATE
  // =====================================================

  formatDate(
    value?: string | null
  ): string {

    if (!value) {

      return '-';

    }

    return new Date(
      value
    ).toLocaleDateString(
      'en-BD',
      {
        day: '2-digit',
        month: 'short',
        year: 'numeric'
      }
    );

  }


  // =====================================================
  // DOWNLOAD ASSIGNMENT FILE
  // =====================================================

  downloadAssignmentFile(): void {

    if (
      !this.assignment?.attachmentUrl
    ) {

      alert(
        'Assignment file is not available.'
      );

      return;

    }

    this.downloadFile(
      this.assignment.attachmentUrl,
      this.assignment.attachmentFileName ||
      'assignment-file'
    );

  }


  // =====================================================
  // DOWNLOAD SUBMISSION FILE
  // =====================================================

  downloadSubmissionFile(): void {

    if (
      !this.submission?.attachmentUrl
    ) {

      alert(
        'Your submission file is not available.'
      );

      return;

    }

    this.downloadFile(
      this.submission.attachmentUrl,
      this.submission.attachmentFileName ||
      'submission-file'
    );

  }


  // =====================================================
  // COMMON DOWNLOAD
  // =====================================================

  private downloadFile(
    url: string,
    fileName: string
  ): void {

    try {

      const link =
        document.createElement('a');

      link.href = url;

      link.download = fileName;

      link.target = '_blank';

      link.rel = 'noopener noreferrer';

      document.body.appendChild(link);

      link.click();

      document.body.removeChild(link);

    }
    catch (error) {

      console.error(
        'File download error:',
        error
      );

      window.open(
        url,
        '_blank'
      );

    }

  }

}