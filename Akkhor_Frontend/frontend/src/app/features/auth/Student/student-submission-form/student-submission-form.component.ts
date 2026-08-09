import {
  Component,
  EventEmitter,
  Input,
  Output,
  OnInit
} from '@angular/core';

import {
  CommonModule
} from '@angular/common';

import {
  FormsModule
} from '@angular/forms';

import {
  StudentAssignment
} from '../../../../core/models/student-assignment.model';

import {
  AssignmentSubmission
} from '../../../../core/models/assignment-submission.model';

import {
  AssignmentSubmissionService
} from '../../../../core/services/assignment-submission.service';


@Component({
  selector: 'app-student-submission-form',

  standalone: true,

  imports: [
    CommonModule,
    FormsModule
  ],

  templateUrl:
    './student-submission-form.component.html',

  styleUrls: [
    './student-submission-form.component.scss'
  ]
})
export class StudentSubmissionFormComponent
  implements OnInit {


  // =====================================================
  // INPUT
  // =====================================================

  @Input()
  assignment:
    StudentAssignment | null = null;


  @Input()
  existingSubmission:
    AssignmentSubmission | null = null;


  // =====================================================
  // OUTPUT
  // =====================================================

  @Output()
  close =
    new EventEmitter<void>();


  @Output()
  submitted =
    new EventEmitter<AssignmentSubmission>();


  // =====================================================
  // FORM
  // =====================================================

  submissionText = '';

  selectedFile:
    File | null = null;


  // =====================================================
  // UI STATE
  // =====================================================

  loading = false;

  errorMessage = '';

  successMessage = '';


  // =====================================================
  // FILE
  // =====================================================

  maxFileSize =
    10 * 1024 * 1024;


  allowedFileTypes = [

    'application/pdf',

    'application/msword',

    'application/vnd.openxmlformats-officedocument.wordprocessingml.document',

    'application/vnd.ms-powerpoint',

    'application/vnd.openxmlformats-officedocument.presentationml.presentation',

    'application/vnd.ms-excel',

    'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet',

    'image/jpeg',

    'image/png',

    'image/jpg'

  ];


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

    this.initializeForm();

  }


  // =====================================================
  // INITIALIZE
  // =====================================================

  initializeForm(): void {

    this.errorMessage = '';

    this.successMessage = '';

    this.selectedFile = null;


    if (
      this.existingSubmission
    ) {

      this.submissionText =
        this.existingSubmission
          .submissionText ?? '';

    }
    else {

      this.submissionText = '';

    }

  }


  // =====================================================
  // FILE SELECT
  // =====================================================

  onFileSelected(
    event: Event
  ): void {

    const input =
      event.target as HTMLInputElement;


    if (
      !input.files ||
      input.files.length === 0
    ) {

      this.selectedFile = null;

      return;

    }


    const file =
      input.files[0];


    // ---------------------------------------------------
    // SIZE
    // ---------------------------------------------------

    if (
      file.size >
      this.maxFileSize
    ) {

      this.errorMessage =
        'File size cannot exceed 10 MB.';

      input.value = '';

      this.selectedFile = null;

      return;

    }


    // ---------------------------------------------------
    // TYPE
    // ---------------------------------------------------

    if (
      !this.allowedFileTypes
        .includes(file.type)
    ) {

      this.errorMessage =
        'This file type is not supported.';

      input.value = '';

      this.selectedFile = null;

      return;

    }


    this.errorMessage = '';

    this.selectedFile = file;

  }


  // =====================================================
  // REMOVE FILE
  // =====================================================

  removeFile(): void {

    this.selectedFile = null;

  }


  // =====================================================
  // SUBMIT
  // =====================================================

  submitAssignment(): void {

    this.errorMessage = '';

    this.successMessage = '';


    if (
      !this.assignment
    ) {

      this.errorMessage =
        'Assignment information is missing.';

      return;

    }


    // ---------------------------------------------------
    // EXISTING SUBMISSION
    // ---------------------------------------------------

    if (
      this.existingSubmission
    ) {

      this.updateSubmission();

      return;

    }


    // ---------------------------------------------------
    // DEADLINE
    // ---------------------------------------------------

    if (
      this.isOverdue()
    ) {

      this.errorMessage =
        'The deadline for this assignment has passed.';

      return;

    }


    // ---------------------------------------------------
    // TEXT / FILE
    // ---------------------------------------------------

    const text =
      this.submissionText
        ?.trim() ?? '';


    if (
      !text &&
      !this.selectedFile
    ) {

      this.errorMessage =
        'Please enter your submission or attach a file.';

      return;

    }


    this.loading = true;


    const data = {

      assignmentId:
        this.assignment.id,

      submissionText:
        text

    };


    this.submissionService
      .create(
        data,
        this.selectedFile ?? undefined
      )
      .subscribe({

        next: (
          response:
            AssignmentSubmission
        ) => {

          this.loading = false;

          this.successMessage =
            'Assignment submitted successfully.';

          this.submitted.emit(
            response
          );

        },


        error: (error) => {

          console.error(
            'Assignment submission error:',
            error
          );

          this.loading = false;

          this.errorMessage =
            error?.error?.message ||
            'Failed to submit assignment. Please try again.';

        }

      });

  }


  // =====================================================
  // UPDATE
  // =====================================================

  private updateSubmission(): void {

    if (
      !this.existingSubmission
    ) {

      return;

    }


    const text =
      this.submissionText
        ?.trim() ?? '';


    if (
      !text &&
      !this.selectedFile
    ) {

      this.errorMessage =
        'Please enter your submission or attach a file.';

      return;

    }


    this.loading = true;


    const data = {

      submissionText:
        text

    };


    this.submissionService
      .update(
        this.existingSubmission.id,
        data,
        this.selectedFile ?? undefined
      )
      .subscribe({

        next: (
          response:
            AssignmentSubmission
        ) => {

          this.loading = false;

          this.successMessage =
            'Submission updated successfully.';

          this.submitted.emit(
            response
          );

        },


        error: (error) => {

          console.error(
            'Update submission error:',
            error
          );

          this.loading = false;

          this.errorMessage =
            error?.error?.message ||
            'Failed to update submission. Please try again.';

        }

      });

  }


  // =====================================================
  // CANCEL
  // =====================================================

  cancel(): void {

    if (
      this.loading
    ) {

      return;

    }

    this.close.emit();

  }


  // =====================================================
  // DEADLINE
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
  // DEADLINE FORMAT
  // =====================================================

  formatDeadline(): string {

    if (
      !this.assignment?.deadline
    ) {

      return '-';

    }


    return new Date(
      this.assignment.deadline
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
    bytes: number
  ): string {

    if (
      !bytes ||
      bytes <= 0
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
  // FILE EXTENSION
  // =====================================================

  getFileExtension(
    file: File
  ): string {

    const parts =
      file.name.split('.');


    if (
      parts.length < 2
    ) {

      return '';

    }


    return parts[
      parts.length - 1
    ].toUpperCase();

  }

}