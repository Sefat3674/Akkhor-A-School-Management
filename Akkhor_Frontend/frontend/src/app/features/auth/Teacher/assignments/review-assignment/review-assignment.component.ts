import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';

import {
  Assignment
} from '../../../../../core/models/assignment.model';

import {
  AssignmentService
} from '../../../../../core/services/assignment.service';

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

  loading = false;

  assignmentLoading = false;

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

    this.assignmentLoading = true;

    this.assignmentService
      .getById(this.assignmentId)
      .subscribe({

        next: (data: Assignment) => {

          this.assignment = data;

          this.assignmentLoading = false;
        },

        error: (error) => {

          console.error(
            'Error loading assignment:',
            error
          );

          this.assignmentLoading = false;

          this.errorMessage =
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

    this.loading = true;

    this.errorMessage = '';

    this.submissionService
      .getByAssignment(this.assignmentId)
      .subscribe({

        next: (data: AssignmentSubmission[]) => {

          this.submissions =
            data ?? [];

          this.applyFilters();

          this.loading = false;
        },

        error: (error) => {

          console.error(
            'Error loading submissions:',
            error
          );

          this.loading = false;

          this.errorMessage =
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

          const matchesSearch =
            !search ||

            submission.studentName
              ?.toLowerCase()
              .includes(search) ||

            submission.studentId
              ?.toLowerCase()
              .includes(search) ||

            submission.submissionText
              ?.toLowerCase()
              .includes(search);


          // ---------------------------------------------
          // STATUS
          // ---------------------------------------------

          let matchesStatus = true;

          if (
            this.statusFilter === 'submitted'
          ) {

            matchesStatus =
              submission.status
                ?.toLowerCase() === 'submitted';
          }

          if (
            this.statusFilter === 'evaluated'
          ) {

            matchesStatus =
              submission.status
                ?.toLowerCase() === 'evaluated';
          }

          if (
            this.statusFilter === 'pending'
          ) {

            matchesStatus =
              !submission.status ||
              submission.status
                ?.toLowerCase() === 'pending';
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

          const dateA =
            new Date(
              a.submittedAt
            ).getTime();

          const dateB =
            new Date(
              b.submittedAt
            ).getTime();

          if (
            this.sortBy === 'oldest'
          ) {

            return dateA - dateB;
          }

          if (
            this.sortBy === 'student'
          ) {

            return (
              (a.studentName ?? '')
                .localeCompare(
                  b.studentName ?? ''
                )
            );
          }

          return dateB - dateA;
        }
      );

    this.filteredSubmissions =
      result;
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
      return;
    }

    if (!this.selectedSubmission.id) {
      return;
    }

    if (!this.assignment) {

      this.errorMessage =
        'Assignment information is unavailable.';

      return;
    }

    // ---------------------------------------------
    // Validate Marks
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

    this.evaluationLoading =
      true;

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

          const index =
            this.submissions.findIndex(
              x =>
                x.id === updated.id
            );

          if (index !== -1) {

            this.submissions[index] =
              updated;
          }

          this.selectedSubmission =
            updated;

          this.evaluationMarks =
            updated.marksObtained ?? null;

          this.evaluationFeedback =
            updated.feedback ?? '';

          this.applyFilters();

          this.evaluationLoading =
            false;

          this.successMessage =
            'Submission evaluated successfully.';
        },

        error: (error) => {

          console.error(
            'Error evaluating submission:',
            error
          );

          this.evaluationLoading =
            false;

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
      '_blank'
    );
  }

  // =====================================================
  // OPEN ATTACHMENT
  // =====================================================

  openAttachment(
    submission: AssignmentSubmission
  ): void {

    if (!submission.attachmentUrl) {
      return;
    }

    window.open(
      submission.attachmentUrl,
      '_blank'
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

    return (
      new Date(deadline).getTime() <
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

    if (
      submission.status
        ?.toLowerCase() === 'evaluated'
    ) {

      return 'Evaluated';
    }

    if (
      submission.status
        ?.toLowerCase() === 'submitted'
    ) {

      return 'Submitted';
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

    if (
      submission.status
        ?.toLowerCase() === 'evaluated'
    ) {

      return 'evaluated';
    }

    if (
      submission.status
        ?.toLowerCase() === 'submitted'
    ) {

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
        ?.toLowerCase() === 'evaluated'
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
      x =>
        x.status
          ?.toLowerCase() === 'evaluated'
    ).length;
  }

  // =====================================================
  // PENDING COUNT
  // =====================================================

  get pendingCount(): number {

    return this.submissions.filter(
      x =>
        x.status
          ?.toLowerCase() !== 'evaluated'
    ).length;
  }

  // =====================================================
  // AVERAGE MARKS
  // =====================================================

  get averageMarks(): number {

    const evaluated =
      this.submissions.filter(
        x =>
          x.marksObtained !== null &&
          x.marksObtained !== undefined
      );

    if (!evaluated.length) {
      return 0;
    }

    const total =
      evaluated.reduce(
        (
          sum,
          submission
        ) =>
          sum +
          (submission.marksObtained ?? 0),
        0
      );

    return total /
      evaluated.length;
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
      submission.marksObtained /
      this.assignment.maximumMarks
    ) * 100;
  }

  // =====================================================
  // TRACK BY
  // =====================================================

  trackById(
    index: number,
    submission: AssignmentSubmission
  ): string {

    return submission.id;
  }
}