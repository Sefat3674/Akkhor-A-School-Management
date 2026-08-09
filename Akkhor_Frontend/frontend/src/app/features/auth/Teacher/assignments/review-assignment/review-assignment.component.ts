import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';

import { Assignment } from '../../../../../core/models/assignment.model';
import { AssignmentService } from '../../../../../core/services/assignment.service';

import {
  AssignmentSubmission
} from '../../../../../core/models/assignment-submission.model';

import {
  AssignmentSubmissionService
} from '../../../../../core/services/assignment-submission.service';

@Component({
  selector: 'app-review-assignment',
  standalone: true,

  imports: [
    CommonModule,
    FormsModule
  ],

  templateUrl: './review-assignment.component.html',
  styleUrls: ['./review-assignment.component.scss']
})
export class ReviewAssignmentComponent implements OnInit {

  // =====================================================
  // ASSIGNMENT
  // =====================================================

  assignment: Assignment | null = null;

  assignmentId = '';

  // =====================================================
  // SUBMISSIONS
  // =====================================================

  submissions: AssignmentSubmission[] = [];

  filteredSubmissions: AssignmentSubmission[] = [];

  // =====================================================
  // SELECTED SUBMISSION
  // =====================================================

  selectedSubmission: AssignmentSubmission | null = null;

  // =====================================================
  // EVALUATION
  // =====================================================

  evaluationMarks: number | null = null;

  evaluationFeedback = '';

  // =====================================================
  // FILTER
  // =====================================================

  searchTerm = '';

  statusFilter = 'all';

  // =====================================================
  // SORT
  // =====================================================

  sortBy = 'latest';

  // =====================================================
  // UI STATE
  // =====================================================

  /**
   * Loading state for assignment details.
   * Used by review-assignment.component.html
   */
  loadingAssignment = false;

  /**
   * Loading state for submissions.
   * Used by review-assignment.component.html
   */
  loadingSubmissions = false;

  /**
   * Loading state while evaluating a submission.
   */
  evaluationLoading = false;

  deleting = false;

  errorMessage = '';

  successMessage = '';

  // =====================================================
  // CONSTRUCTOR
  // =====================================================

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private assignmentService: AssignmentService,
    private submissionService: AssignmentSubmissionService
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

    this.loadSubmissions();
  }

  // =====================================================
  // LOAD ASSIGNMENT
  // =====================================================

  loadAssignment(): void {

    if (!this.assignmentId) {
      return;
    }

    this.loadingAssignment = true;

    this.assignmentService
      .getById(this.assignmentId)
      .subscribe({

        next: (data: Assignment) => {

          this.assignment = data;

          this.loadingAssignment = false;
        },

        error: (error) => {

          console.error(
            'Error loading assignment:',
            error
          );

          this.loadingAssignment = false;

          this.errorMessage =
            error?.error?.message ??
            'Failed to load assignment.';
        }

      });
  }

  // =====================================================
  // LOAD SUBMISSIONS
  // =====================================================

  loadSubmissions(): void {

    if (!this.assignmentId) {
      return;
    }

    this.loadingSubmissions = true;

    this.errorMessage = '';

    this.submissionService
      .getByAssignment(this.assignmentId)
      .subscribe({

        next: (data: AssignmentSubmission[]) => {

          this.submissions =
            Array.isArray(data)
              ? data
              : [];

          this.applyFilters();

          this.loadingSubmissions = false;
        },

        error: (error) => {

          console.error(
            'Error loading submissions:',
            error
          );

          this.loadingSubmissions = false;

          this.submissions = [];

          this.filteredSubmissions = [];

          this.errorMessage =
            error?.error?.message ??
            'Failed to load assignment submissions.';
        }

      });
  }

  // =====================================================
  // REFRESH
  // =====================================================

  refresh(): void {

    this.successMessage = '';

    this.errorMessage = '';

    this.loadAssignment();

    this.loadSubmissions();
  }

  // =====================================================
  // SEARCH
  // =====================================================

  onSearch(): void {

    this.applyFilters();
  }

  // =====================================================
  // STATUS FILTER
  // =====================================================

  onStatusFilterChange(): void {

    this.applyFilters();
  }

  // =====================================================
  // SORT
  // =====================================================

  onSortChange(): void {

    this.applyFilters();
  }

  // =====================================================
  // APPLY FILTERS
  // =====================================================

  applyFilters(): void {

    const search =
      this.searchTerm
        .trim()
        .toLowerCase();

    let result =
      this.submissions.filter(
        submission => {

          // ---------------------------------------------
          // SEARCH
          // ---------------------------------------------

          const studentName =
            submission.studentName
              ?.toLowerCase() ?? '';

          const studentId =
            submission.studentId
              ?.toLowerCase() ?? '';

          const submissionText =
            submission.submissionText
              ?.toLowerCase() ?? '';

          const matchesSearch =
            !search ||
            studentName.includes(search) ||
            studentId.includes(search) ||
            submissionText.includes(search);

          // ---------------------------------------------
          // STATUS
          // ---------------------------------------------

          const status =
            submission.status
              ?.trim()
              .toLowerCase() ?? '';

          let matchesStatus = true;

          switch (this.statusFilter) {

            case 'submitted':

              matchesStatus =
                status === 'submitted';

              break;

            case 'evaluated':

              matchesStatus =
                status === 'evaluated';

              break;

            case 'pending':

              matchesStatus =
                status === 'pending' ||
                status === '';

              break;

            case 'all':

            default:

              matchesStatus = true;

              break;
          }

          return (
            matchesSearch &&
            matchesStatus
          );
        }
      );

    // ===================================================
    // SORT
    // ===================================================

    result =
      [...result].sort(
        (a, b) => {

          // ---------------------------------------------
          // SORT BY STUDENT
          // ---------------------------------------------

          if (this.sortBy === 'student') {

            return (
              (a.studentName ?? '')
                .localeCompare(
                  b.studentName ?? '',
                  undefined,
                  {
                    sensitivity: 'base'
                  }
                )
            );
          }

          // ---------------------------------------------
          // SORT BY DATE
          // ---------------------------------------------

          const dateA =
            this.getSubmissionTime(
              a.submittedAt
            );

          const dateB =
            this.getSubmissionTime(
              b.submittedAt
            );

          // Oldest first
          if (this.sortBy === 'oldest') {

            return dateA - dateB;
          }

          // Latest first
          return dateB - dateA;
        }
      );

    this.filteredSubmissions =
      result;
  }

  // =====================================================
  // SUBMISSION DATE HELPER
  // =====================================================

  private getSubmissionTime(
    submittedAt: string | Date | null | undefined
  ): number {

    if (!submittedAt) {
      return 0;
    }

    const time =
      new Date(submittedAt).getTime();

    return Number.isNaN(time)
      ? 0
      : time;
  }

  // =====================================================
  // SELECT SUBMISSION
  // =====================================================

  selectSubmission(
    submission: AssignmentSubmission
  ): void {

    this.selectedSubmission =
      submission;

    this.evaluationMarks =
      submission.marksObtained ?? null;

    this.evaluationFeedback =
      submission.feedback ?? '';

    this.successMessage = '';

    this.errorMessage = '';
  }

  // =====================================================
  // CLOSE REVIEW
  // =====================================================

  closeReview(): void {

    if (this.evaluationLoading) {
      return;
    }

    this.selectedSubmission =
      null;

    this.evaluationMarks =
      null;

    this.evaluationFeedback =
      '';
  }

  // =====================================================
  // EVALUATE SUBMISSION
  // =====================================================

  evaluateSubmission(): void {

    if (!this.selectedSubmission) {

      this.errorMessage =
        'Please select a submission first.';

      return;
    }

    if (!this.selectedSubmission.id) {

      this.errorMessage =
        'Submission ID is missing.';

      return;
    }

    if (!this.assignment) {

      this.errorMessage =
        'Assignment information is unavailable.';

      return;
    }

    // ---------------------------------------------
    // VALIDATE MARKS
    // ---------------------------------------------

    if (
      this.evaluationMarks === null ||
      this.evaluationMarks === undefined
    ) {

      this.errorMessage =
        'Please enter marks.';

      return;
    }

    if (
      Number.isNaN(this.evaluationMarks)
    ) {

      this.errorMessage =
        'Please enter a valid mark.';

      return;
    }

    if (
      this.evaluationMarks < 0
    ) {

      this.errorMessage =
        'Marks cannot be negative.';

      return;
    }

    if (
      this.evaluationMarks >
      this.assignment.maximumMarks
    ) {

      this.errorMessage =
        `Marks cannot exceed ${this.assignment.maximumMarks}.`;

      return;
    }

    // ---------------------------------------------
    // SAVE
    // ---------------------------------------------

    this.evaluationLoading = true;

    this.errorMessage = '';

    this.successMessage = '';

    const dto = {

      marksObtained:
        this.evaluationMarks,

      feedback:
        this.evaluationFeedback
          ?.trim() || null
    };

    this.submissionService
      .evaluate(
        this.selectedSubmission.id,
        dto
      )
      .subscribe({

        next: (
          updated: AssignmentSubmission
        ) => {

          // -----------------------------------------
          // UPDATE LOCAL SUBMISSION
          // -----------------------------------------

          const index =
            this.submissions.findIndex(
              x =>
                x.id === updated.id
            );

          if (index !== -1) {

            this.submissions[index] =
              updated;
          }

          // -----------------------------------------
          // UPDATE SELECTED SUBMISSION
          // -----------------------------------------

          this.selectedSubmission =
            updated;

          this.evaluationMarks =
            updated.marksObtained ?? null;

          this.evaluationFeedback =
            updated.feedback ?? '';

          // -----------------------------------------
          // REFRESH FILTERED LIST
          // -----------------------------------------

          this.applyFilters();

          this.evaluationLoading = false;

          this.successMessage =
            'Submission evaluated successfully.';
        },

        error: (error) => {

          console.error(
            'Error evaluating submission:',
            error
          );

          this.evaluationLoading = false;

          this.errorMessage =
            error?.error?.message ??
            'Failed to evaluate submission.';
        }

      });
  }

  // =====================================================
  // DOWNLOAD ATTACHMENT
  // =====================================================

  downloadAttachment(
    submission: AssignmentSubmission
  ): void {

    if (!submission.attachmentUrl) {

      this.errorMessage =
        'No attachment available.';

      return;
    }

    window.open(
      submission.attachmentUrl,
      '_blank',
      'noopener,noreferrer'
    );
  }

  // =====================================================
  // OPEN ATTACHMENT
  // =====================================================

  openAttachment(
    submission: AssignmentSubmission
  ): void {

    if (!submission.attachmentUrl) {

      this.errorMessage =
        'No attachment available.';

      return;
    }

    window.open(
      submission.attachmentUrl,
      '_blank',
      'noopener,noreferrer'
    );
  }

  // =====================================================
  // BACK
  // =====================================================

  goBack(): void {

    this.router.navigate([
      '/admin/assignments'
    ]);
  }

  // =====================================================
  // EDIT ASSIGNMENT
  // =====================================================

  editAssignment(): void {

    if (!this.assignmentId) {
      return;
    }

    this.router.navigate([
      '/admin/assignments/edit',
      this.assignmentId
    ]);
  }

  // =====================================================
  // DEADLINE CHECK
  // =====================================================

  isExpired(
    deadline: string | Date | null | undefined
  ): boolean {

    if (!deadline) {
      return false;
    }

    const deadlineTime =
      new Date(deadline).getTime();

    if (Number.isNaN(deadlineTime)) {
      return false;
    }

    return (
      deadlineTime <
      new Date().getTime()
    );
  }

  // =====================================================
  // DEADLINE TEXT
  // =====================================================

  getDeadlineText(
    deadline: string | Date | null | undefined
  ): string {

    if (!deadline) {
      return 'No deadline';
    }

    if (
      this.isExpired(deadline)
    ) {

      return 'Expired';
    }

    return new Date(
      deadline
    ).toLocaleString();
  }

  // =====================================================
  // SUBMISSION STATUS
  // =====================================================

  getStatusText(
    submission: AssignmentSubmission
  ): string {

    const status =
      submission.status
        ?.trim()
        .toLowerCase() ?? '';

    if (status === 'evaluated') {
      return 'Evaluated';
    }

    if (status === 'submitted') {
      return 'Submitted';
    }

    if (status === 'pending') {
      return 'Pending';
    }

    return submission.status ||
      'Pending';
  }

  // =====================================================
  // STATUS CLASS
  // =====================================================

  getStatusClass(
    submission: AssignmentSubmission
  ): string {

    const status =
      submission.status
        ?.trim()
        .toLowerCase() ?? '';

    if (status === 'evaluated') {
      return 'evaluated';
    }

    if (status === 'submitted') {
      return 'submitted';
    }

    return 'pending';
  }

  // =====================================================
  // HAS BEEN EVALUATED
  // =====================================================

  isEvaluated(
    submission: AssignmentSubmission
  ): boolean {

    return (
      submission.status
        ?.trim()
        .toLowerCase() ===
      'evaluated'
    );
  }

  // =====================================================
  // SUBMISSION COUNT
  // =====================================================

  get totalSubmissions(): number {

    return this.submissions.length;
  }

  // =====================================================
  // EVALUATED COUNT
  // =====================================================

  get evaluatedCount(): number {

    return this.submissions.filter(
      submission =>
        this.isEvaluated(submission)
    ).length;
  }

  // =====================================================
  // PENDING COUNT
  // =====================================================

  get pendingCount(): number {

    return this.submissions.filter(
      submission => {

        const status =
          submission.status
            ?.trim()
            .toLowerCase() ?? '';

        return (
          status === 'pending' ||
          status === '' ||
          status === 'submitted'
        );
      }
    ).length;
  }

  // =====================================================
  // AVERAGE MARKS
  // =====================================================

  get averageMarks(): number {

    const evaluated =
      this.submissions.filter(
        submission =>
          submission.marksObtained !== null &&
          submission.marksObtained !== undefined
      );

    if (!evaluated.length) {
      return 0;
    }

    const total =
      evaluated.reduce(
        (
          sum,
          submission
        ) => {

          return (
            sum +
            Number(
              submission.marksObtained ?? 0
            )
          );
        },
        0
      );

    return (
      total /
      evaluated.length
    );
  }

  // =====================================================
  // MARKS PERCENTAGE
  // =====================================================

  getMarksPercentage(
    submission: AssignmentSubmission
  ): number {

    if (
      !this.assignment ||
      !this.assignment.maximumMarks ||
      submission.marksObtained === null ||
      submission.marksObtained === undefined
    ) {

      return 0;
    }

    return (
      Number(submission.marksObtained) /
      Number(this.assignment.maximumMarks)
    ) * 100;
  }

  // =====================================================
  // TRACK BY
  // =====================================================

  trackById(
    index: number,
    submission: AssignmentSubmission
  ): string | number {

    return submission.id || index;
  }
}