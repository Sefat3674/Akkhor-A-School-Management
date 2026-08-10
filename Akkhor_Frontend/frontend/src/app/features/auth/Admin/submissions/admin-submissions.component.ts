import {
  Component,
  OnInit
} from '@angular/core';

import {
  CommonModule
} from '@angular/common';

import {
  FormsModule
} from '@angular/forms';

import {
  AssignmentSubmission
} from '../../../../core/models/assignment-submission.model';

import {
  AdminSubmissionService
} from '../../../../core/services/admin-submission.service';


@Component({
  selector: 'app-admin-submissions',

  standalone: true,

  imports: [
    CommonModule,
    FormsModule
  ],

  templateUrl:
    './admin-submissions.component.html',

  styleUrls:
    ['./admin-submissions.component.scss']
})
export class AdminSubmissionsComponent
  implements OnInit {

  submissions: AssignmentSubmission[] = [];

  filteredSubmissions:
    AssignmentSubmission[] = [];

  searchTerm = '';

  selectedStatus = 'all';

  loading = false;

  errorMessage = '';


  constructor(
    private submissionService:
      AdminSubmissionService
  ) {}


  // =====================================================
  // INIT
  // =====================================================

  ngOnInit(): void {
    this.loadSubmissions();
  }


  // =====================================================
  // LOAD
  // =====================================================

  loadSubmissions(): void {

    this.loading = true;

    this.errorMessage = '';

    this.submissionService
      .getAll()
      .subscribe({

        next: (data) => {

          this.submissions =
            data ?? [];

          this.filteredSubmissions =
            [...this.submissions];

          this.loading = false;

          this.applyFilters();
        },

        error: (error) => {

          console.error(
            'Failed to load submissions',
            error
          );

          this.errorMessage =
            error?.error?.message ??
            'Failed to load submissions.';

          this.loading = false;
        }

      });
  }


  // =====================================================
  // FILTER
  // =====================================================

  applyFilters(): void {

    const search =
      this.searchTerm
        .trim()
        .toLowerCase();

    this.filteredSubmissions =
      this.submissions.filter(
        submission => {

          const matchesSearch =
            !search ||

            submission.assignmentTitle
              ?.toLowerCase()
              .includes(search) ||

            submission.studentName
              ?.toLowerCase()
              .includes(search);


          const matchesStatus =
            this.selectedStatus === 'all' ||

            submission.status
              ?.toLowerCase() ===
              this.selectedStatus.toLowerCase();


          return (
            matchesSearch &&
            matchesStatus
          );
        }
      );
  }


  onSearch(): void {
    this.applyFilters();
  }


  onStatusChange(): void {
    this.applyFilters();
  }


  clearFilters(): void {

    this.searchTerm = '';

    this.selectedStatus = 'all';

    this.applyFilters();
  }


  // =====================================================
  // DOWNLOAD
  // =====================================================

  downloadAttachment(
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
  // COUNTS
  // =====================================================

  get totalSubmissions(): number {
    return this.submissions.length;
  }


  get evaluatedCount(): number {

    return this.submissions
      .filter(x =>
        x.status
          ?.toLowerCase() === 'evaluated'
      )
      .length;
  }


  get pendingCount(): number {

    return this.submissions
      .filter(x =>
        x.status
          ?.toLowerCase() === 'submitted'
      )
      .length;
  }


  get totalMarks(): number {

    return this.submissions
      .reduce(
        (total, item) =>
          total + (item.marksObtained ?? 0),
        0
      );
  }
}