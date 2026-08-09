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
  AssignmentSubmissionService
} from '../../../../core/services/assignment-submission.service';


@Component({
  selector: 'app-student-submissions',

  standalone: true,

  imports: [
    CommonModule,
    FormsModule
  ],

  templateUrl:
    './student-submissions.component.html',

  styleUrls: [
    './student-submissions.component.scss'
  ]
})
export class StudentSubmissionsComponent
  implements OnInit {


  // =====================================================
  // DATA
  // =====================================================

  submissions:
    AssignmentSubmission[] = [];

  filteredSubmissions:
    AssignmentSubmission[] = [];


  // =====================================================
  // UI STATE
  // =====================================================

  loading = false;

  errorMessage = '';

  searchText = '';

  selectedStatus = 'all';


  // =====================================================
  // MODAL
  // =====================================================

  showDetailsModal = false;

  selectedSubmission:
    AssignmentSubmission | null = null;


  // =====================================================
  // DELETE
  // =====================================================

  deleting = false;


  // =====================================================
  // CONSTRUCTOR
  // =====================================================

  constructor(
    private submissionService:
      AssignmentSubmissionService
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
      .getMySubmissions()
      .subscribe({

        next: (
          data: AssignmentSubmission[]
        ) => {

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

          this.submissions = [];

          this.filteredSubmissions = [];

          this.errorMessage =
            error?.error?.message ||
            'Unable to load your submissions.';

          this.loading = false;

        }

      });

  }


  // =====================================================
  // REFRESH
  // =====================================================

  refresh(): void {

    this.loadSubmissions();

  }


  // =====================================================
  // SEARCH
  // =====================================================

  onSearch(): void {

    this.applyFilters();

  }


  // =====================================================
  // CLEAR SEARCH
  // =====================================================

  clearSearch(): void {

    this.searchText = '';

    this.applyFilters();

  }


  // =====================================================
  // STATUS CHANGE
  // =====================================================

  onStatusChange(): void {

    this.applyFilters();

  }


  // =====================================================
  // APPLY FILTERS
  // =====================================================

  applyFilters(): void {

    let result =
      [
        ...this.submissions
      ];


    // ---------------------------------------------------
    // SEARCH
    // ---------------------------------------------------

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
          submission => {

            const assignment =
              submission.assignmentTitle
                ?.toLowerCase()
                .includes(search);


            const student =
              submission.studentName
                ?.toLowerCase()
                .includes(search);


            const text =
              submission.submissionText
                ?.toLowerCase()
                .includes(search);


            const status =
              submission.status
                ?.toLowerCase()
                .includes(search);


            return !!(
              assignment ||
              student ||
              text ||
              status
            );

          }
        );

    }


    // ---------------------------------------------------
    // STATUS
    // ---------------------------------------------------

    if (
      this.selectedStatus !== 'all'
    ) {

      result =
        result.filter(
          submission =>
            this.getStatus(
              submission
            ) ===
            this.selectedStatus
        );

    }


    // ---------------------------------------------------
    // SORT
    // ---------------------------------------------------

    result.sort(
      (a, b) =>
        new Date(
          b.submittedAt
        ).getTime()
        -
        new Date(
          a.submittedAt
        ).getTime()
    );


    this.filteredSubmissions =
      result;

  }


  // =====================================================
  // TOTAL
  // =====================================================

  get totalCount(): number {

    return this.submissions.length;

  }


  // =====================================================
  // SUBMITTED
  // =====================================================

  get submittedCount(): number {

    return this.submissions.filter(
      x =>
        this.getStatus(x) ===
        'submitted'
    ).length;

  }


  // =====================================================
  // EVALUATED
  // =====================================================

  get evaluatedCount(): number {

    return this.submissions.filter(
      x =>
        this.getStatus(x) ===
        'evaluated'
    ).length;

  }


  // =====================================================
  // PENDING
  // =====================================================

  get pendingCount(): number {

    return this.submissions.filter(
      x =>
        this.getStatus(x) ===
        'pending'
    ).length;

  }


  // =====================================================
  // STATUS
  // =====================================================

  getStatus(
    submission: AssignmentSubmission
  ): string {

    const status =
      submission.status
        ?.toLowerCase();


    if (
      status === 'evaluated'
    ) {

      return 'evaluated';

    }


    if (
      status === 'submitted'
    ) {

      return 'submitted';

    }


    return 'pending';

  }


  // =====================================================
  // STATUS LABEL
  // =====================================================

  getStatusLabel(
    submission: AssignmentSubmission
  ): string {

    switch (
      this.getStatus(
        submission
      )
    ) {

      case 'evaluated':

        return 'Evaluated';


      case 'submitted':

        return 'Submitted';


      default:

        return 'Pending';

    }

  }


  // =====================================================
  // STATUS CLASS
  // =====================================================

  getStatusClass(
    submission: AssignmentSubmission
  ): string {

    switch (
      this.getStatus(
        submission
      )
    ) {

      case 'evaluated':

        return 'status-evaluated';


      case 'submitted':

        return 'status-submitted';


      default:

        return 'status-pending';

    }

  }


  // =====================================================
  // VIEW
  // =====================================================

  viewSubmission(
    submission: AssignmentSubmission
  ): void {

    this.selectedSubmission =
      submission;

    this.showDetailsModal =
      true;

  }


  // =====================================================
  // CLOSE MODAL
  // =====================================================

  closeDetails(): void {

    this.showDetailsModal =
      false;

    this.selectedSubmission =
      null;

  }


  // =====================================================
  // DOWNLOAD
  // =====================================================

  // =====================================================
// DOWNLOAD / VIEW ATTACHMENT
// =====================================================

downloadAttachment(
  submission: AssignmentSubmission
): void {

  if (!submission.attachmentUrl) {

    alert('Attachment file is not available.');

    return;
  }

  const attachmentUrl =
    submission.attachmentUrl.startsWith('http')
      ? submission.attachmentUrl
      : `https://localhost:50268${submission.attachmentUrl}`;

  window.open(
    attachmentUrl,
    '_blank'
  );
}


  // =====================================================
  // EDIT
  // =====================================================

  editSubmission(
    submission: AssignmentSubmission
  ): void {

    if (
      this.getStatus(
        submission
      ) === 'evaluated'
    ) {

      alert(
        'An evaluated submission cannot be modified.'
      );

      return;

    }


    console.log(
      'Edit submission:',
      submission
    );

  }


  // =====================================================
  // DELETE
  // =====================================================

  deleteSubmission(
    submission: AssignmentSubmission
  ): void {

    if (
      this.getStatus(
        submission
      ) === 'evaluated'
    ) {

      alert(
        'An evaluated submission cannot be deleted.'
      );

      return;

    }


    const confirmed =
      window.confirm(
        `Are you sure you want to delete your submission for "${submission.assignmentTitle ?? 'this assignment'}"?`
      );


    if (
      !confirmed
    ) {

      return;

    }


    this.deleting = true;


    this.submissionService
      .delete(
        submission.id
      )
      .subscribe({

        next: () => {

          this.submissions =
            this.submissions.filter(
              x =>
                x.id !==
                submission.id
            );


          this.applyFilters();

          this.deleting = false;

          this.closeDetails();

        },


        error: (error) => {

          console.error(
            'Delete submission error:',
            error
          );

          alert(
            error?.error?.message ||
            'Failed to delete submission.'
          );

          this.deleting = false;

        }

      });

  }


  // =====================================================
  // FORMAT DATE
  // =====================================================

  formatDate(
    value?: string | null
  ): string {

    if (
      !value
    ) {

      return '-';

    }


    return new Date(
      value
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
  // FILE SIZE
  // =====================================================

  formatFileSize(
    bytes?: number | null
  ): string {

    if (
      !bytes ||
      bytes <= 0
    ) {

      return '-';

    }


    const units = [
      'Bytes',
      'KB',
      'MB',
      'GB'
    ];


    const index =
      Math.floor(
        Math.log(bytes) /
        Math.log(1024)
      );


    return (
      parseFloat(
        (
          bytes /
          Math.pow(
            1024,
            index
          )
        ).toFixed(2)
      )
      +
      ' ' +
      units[index]
    );

  }


  // =====================================================
  // TRACK BY
  // =====================================================

  trackBySubmissionId(
    index: number,
    submission: AssignmentSubmission
  ): string {

    return submission.id;

  }

}