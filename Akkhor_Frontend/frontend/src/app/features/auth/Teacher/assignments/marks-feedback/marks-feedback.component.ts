import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';

import {
  AssignmentSubmission
} from '../../../../../core/models/assignment-submission.model';

import {
  AssignmentSubmissionService
} from '../../../../../core/services/assignment-submission.service';

import {
  AssignmentService
} from '../../../../../core/services/assignment.service';

import {
  Assignment
} from '../../../../../core/models/assignment.model';

@Component({
  selector: 'app-marks-feedback',
  standalone: true,

  imports: [
    CommonModule,
    FormsModule
  ],

  templateUrl: './marks-feedback.component.html',
  styleUrls: ['./marks-feedback.component.scss']
})
export class MarksFeedbackComponent implements OnInit {

  // =====================================================
  // ROUTE ID
  // =====================================================

  submissionId = '';

  // =====================================================
  // DATA
  // =====================================================

  submission: AssignmentSubmission | null = null;

  assignment: Assignment | null = null;

  // =====================================================
  // FORM
  // =====================================================

  marksObtained: number | null = null;

  feedback = '';

  // =====================================================
  // UI STATE
  // =====================================================

  loading = false;

  saving = false;

  errorMessage = '';

  successMessage = '';

  // =====================================================
  // CONSTRUCTOR
  // =====================================================

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private submissionService: AssignmentSubmissionService,
    private assignmentService: AssignmentService
  ) {}

  // =====================================================
  // INIT
  // =====================================================

  ngOnInit(): void {

    this.submissionId =
      this.route.snapshot.paramMap.get('id') ?? '';

    if (!this.submissionId) {

      this.errorMessage =
        'Submission ID is missing.';

      return;
    }

    this.loadSubmission();
  }

  // =====================================================
  // LOAD SUBMISSION
  // =====================================================

  loadSubmission(): void {

    this.loading = true;

    this.errorMessage = '';

    this.submissionService
      .getById(this.submissionId)
      .subscribe({

        next: (data: AssignmentSubmission) => {

          this.submission = data;

          // ---------------------------------------------
          // Load existing marks
          // ---------------------------------------------

          this.marksObtained =
            data.marksObtained ?? null;

          // ---------------------------------------------
          // Load existing feedback
          // ---------------------------------------------

          this.feedback =
            data.feedback ?? '';

          // ---------------------------------------------
          // Load assignment details
          // ---------------------------------------------

          if (data.assignmentId) {

            this.loadAssignment(
              data.assignmentId
            );
          }

          this.loading = false;
        },

        error: (error) => {

          console.error(
            'Error loading submission:',
            error
          );

          this.errorMessage =
            'Failed to load submission.';

          this.loading = false;
        }

      });
  }

  // =====================================================
  // LOAD ASSIGNMENT
  // =====================================================

  loadAssignment(
    assignmentId: string
  ): void {

    this.assignmentService
      .getById(assignmentId)
      .subscribe({

        next: (data: Assignment) => {

          this.assignment = data;
        },

        error: (error) => {

          console.error(
            'Error loading assignment:',
            error
          );

          // Assignment information is optional
          // because submission already contains
          // assignment title.
        }

      });
  }

  // =====================================================
  // GET MAXIMUM MARKS
  // =====================================================

  get maximumMarks(): number {

    if (this.assignment) {

      return this.assignment.maximumMarks;
    }

    return 0;
  }

  // =====================================================
  // MARKS VALIDATION
  // =====================================================

  isMarksValid(): boolean {

    if (
      this.marksObtained === null ||
      this.marksObtained === undefined
    ) {

      return false;
    }

    if (this.marksObtained < 0) {

      return false;
    }

    if (
      this.maximumMarks > 0 &&
      this.marksObtained > this.maximumMarks
    ) {

      return false;
    }

    return true;
  }

  // =====================================================
  // MARKS ERROR
  // =====================================================

  getMarksError(): string {

    if (
      this.marksObtained === null ||
      this.marksObtained === undefined
    ) {

      return 'Marks are required.';
    }

    if (this.marksObtained < 0) {

      return 'Marks cannot be negative.';
    }

    if (
      this.maximumMarks > 0 &&
      this.marksObtained > this.maximumMarks
    ) {

      return `Marks cannot exceed ${this.maximumMarks}.`;
    }

    return '';
  }

  // =====================================================
  // MARKS CHANGE
  // =====================================================

  onMarksChange(): void {

    this.successMessage = '';

    this.errorMessage = '';

    if (
      this.marksObtained === null ||
      this.marksObtained === undefined
    ) {

      return;
    }

    // Prevent negative values

    if (this.marksObtained < 0) {

      this.marksObtained = 0;
    }

    // Prevent marks greater than maximum

    if (
      this.maximumMarks > 0 &&
      this.marksObtained > this.maximumMarks
    ) {

      this.marksObtained =
        this.maximumMarks;
    }
  }

  // =====================================================
  // FEEDBACK CHANGE
  // =====================================================

  onFeedbackChange(): void {

    this.successMessage = '';

    this.errorMessage = '';
  }

  // =====================================================
  // SAVE EVALUATION
  // =====================================================

  saveEvaluation(): void {

    this.errorMessage = '';

    this.successMessage = '';

    // ---------------------------------------------
    // Submission validation
    // ---------------------------------------------

    if (!this.submission) {

      this.errorMessage =
        'Submission information is unavailable.';

      return;
    }

    // ---------------------------------------------
    // Marks validation
    // ---------------------------------------------

    if (!this.isMarksValid()) {

      this.errorMessage =
        this.getMarksError();

      return;
    }

    // ---------------------------------------------
    // Prevent double submit
    // ---------------------------------------------

    if (this.saving) {

      return;
    }

    // ---------------------------------------------
    // Confirm evaluation
    // ---------------------------------------------

    const confirmed =
      confirm(
        `Save marks for ${this.submission.studentName ?? 'this student'}?`
      );

    if (!confirmed) {

      return;
    }

    // ---------------------------------------------
    // Prepare request
    // ---------------------------------------------

    const data = {

      marksObtained:
        this.marksObtained as number,

      feedback:
        this.feedback?.trim() || null

    };

    // ---------------------------------------------
    // Start saving
    // ---------------------------------------------

    this.saving = true;

    this.submissionService
      .evaluate(
        this.submission.id,
        data
      )
      .subscribe({

        next: (
          updated: AssignmentSubmission
        ) => {

          // -----------------------------------------
          // Update local submission
          // -----------------------------------------

          this.submission =
            updated;

          this.marksObtained =
            updated.marksObtained ?? null;

          this.feedback =
            updated.feedback ?? '';

          // -----------------------------------------
          // Success
          // -----------------------------------------

          this.successMessage =
            'Marks and feedback saved successfully.';

          this.saving = false;
        },

        error: (error) => {

          console.error(
            'Error evaluating submission:',
            error
          );

          // -----------------------------------------
          // Backend error
          // -----------------------------------------

          if (
            error?.error?.message
          ) {

            this.errorMessage =
              error.error.message;

          } else if (
            typeof error?.error === 'string'
          ) {

            this.errorMessage =
              error.error;

          } else {

            this.errorMessage =
              'Failed to save marks and feedback.';
          }

          this.saving = false;
        }

      });
  }

  // =====================================================
  // RESET FORM
  // =====================================================

  resetForm(): void {

    if (!this.submission) {

      return;
    }

    this.marksObtained =
      this.submission.marksObtained ?? null;

    this.feedback =
      this.submission.feedback ?? '';

    this.errorMessage = '';

    this.successMessage = '';
  }

  // =====================================================
  // DOWNLOAD ATTACHMENT
  // =====================================================

  downloadAttachment(): void {

    if (
      !this.submission?.attachmentUrl
    ) {

      return;
    }

    window.open(
      this.submission.attachmentUrl,
      '_blank'
    );
  }

  // =====================================================
  // HAS ATTACHMENT
  // =====================================================

  hasAttachment(): boolean {

    return !!(
      this.submission?.attachmentUrl
    );
  }

  // =====================================================
  // SUBMISSION STATUS
  // =====================================================

  getStatusText(): string {

    if (!this.submission) {

      return '';
    }

    return this.submission.status || 'Pending';
  }

  // =====================================================
  // STATUS CLASS
  // =====================================================

  getStatusClass(): string {

    if (!this.submission) {

      return 'pending';
    }

    switch (
      this.submission.status
        ?.toLowerCase()
    ) {

      case 'evaluated':
        return 'evaluated';

      case 'submitted':
        return 'submitted';

      case 'pending':
        return 'pending';

      default:
        return 'pending';
    }
  }

  // =====================================================
  // PERCENTAGE
  // =====================================================

  getMarksPercentage(): number {

    if (
      this.marksObtained === null ||
      this.marksObtained === undefined ||
      this.maximumMarks <= 0
    ) {

      return 0;
    }

    return (
      (this.marksObtained /
        this.maximumMarks) *
      100
    );
  }

  // =====================================================
  // GRADE
  // =====================================================

  getGrade(): string {

    const percentage =
      this.getMarksPercentage();

    if (percentage >= 80) {

      return 'A+';
    }

    if (percentage >= 70) {

      return 'A';
    }

    if (percentage >= 60) {

      return 'A-';
    }

    if (percentage >= 50) {

      return 'B';
    }

    if (percentage >= 40) {

      return 'C';
    }

    if (percentage >= 33) {

      return 'D';
    }

    return 'F';
  }

  // =====================================================
  // IS ALREADY EVALUATED
  // =====================================================

  isEvaluated(): boolean {

    return (
      this.submission?.status
        ?.toLowerCase() ===
      'evaluated'
    );
  }

  // =====================================================
  // GO BACK
  // =====================================================

  goBack(): void {

    if (
      this.submission?.assignmentId
    ) {

      this.router.navigate([
        '/admin/assignments/review',
        this.submission.assignmentId
      ]);

      return;
    }

    this.router.navigate([
      '/admin/assignments'
    ]);
  }

  // =====================================================
  // CANCEL
  // =====================================================

  cancel(): void {

    this.goBack();
  }

  // =====================================================
  // FORMAT FILE SIZE
  // =====================================================

  formatFileSize(
  size?: number | null
): string {

  if (
    size === undefined ||
    size === null ||
    size <= 0
  ) {
    return '0 Bytes';
  }

  const units = [
    'Bytes',
    'KB',
    'MB',
    'GB'
  ];

  const index =
    Math.floor(
      Math.log(size) /
      Math.log(1024)
    );

  const safeIndex =
    Math.min(
      index,
      units.length - 1
    );

  return (
    parseFloat(
      (
        size /
        Math.pow(
          1024,
          safeIndex
        )
      ).toFixed(2)
    ) +
    ' ' +
    units[safeIndex]
  );
}

  // =====================================================
  // FORMAT DATE
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
      Number.isNaN(
        parsedDate.getTime()
      )
    ) {

      return '-';
    }

    return parsedDate.toLocaleString(
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
  // TRACK
  // =====================================================

  trackById(
    index: number,
    item: AssignmentSubmission
  ): string {

    return item.id;
  }
}