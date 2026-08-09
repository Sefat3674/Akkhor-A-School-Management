
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
  // FILE CONFIGURATION
  // =====================================================

  maxFileSize =
    10 * 1024 * 1024;


  allowedFileTypes: string[] = [

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


  allowedFileExtensions: string[] = [

    '.pdf',

    '.doc',

    '.docx',

    '.ppt',

    '.pptx',

    '.xls',

    '.xlsx',

    '.jpg',

    '.jpeg',

    '.png'

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
  // INITIALIZE FORM
  // =====================================================

  initializeForm(): void {

    this.errorMessage = '';

    this.successMessage = '';

    this.selectedFile = null;


    // ---------------------------------------------------
    // Existing submission
    // ---------------------------------------------------

    if (this.existingSubmission) {

      this.submissionText =
        this.existingSubmission.submissionText ?? '';

    }

    else {

      this.submissionText = '';

    }

  }


  // =====================================================
  // FILE PICKER
  // =====================================================

  openFilePicker(
    fileInput: HTMLInputElement
  ): void {

    if (this.loading) {
      return;
    }

    if (this.isOverdue()) {
      return;
    }

    fileInput.click();

  }


  // =====================================================
  // FILE SELECT
  // =====================================================

  onFileSelected(
    event: Event
  ): void {

    this.errorMessage = '';


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
    // FILE SIZE
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
    // FILE TYPE
    // ---------------------------------------------------

    const extension =
      this.getFileExtension(file)
        .toLowerCase();


    const validMimeType =
      this.allowedFileTypes
        .includes(file.type);


    const validExtension =
      this.allowedFileExtensions
        .includes(`.${extension}`);


    /*
     * Some browsers do not always provide
     * the correct MIME type.
     *
     * Therefore we check both MIME type
     * and file extension.
     */

    if (
      !validMimeType &&
      !validExtension
    ) {

      this.errorMessage =
        'This file type is not supported.';

      input.value = '';

      this.selectedFile = null;

      return;

    }


    // ---------------------------------------------------
    // VALID FILE
    // ---------------------------------------------------

    this.selectedFile =
      file;

  }


  // =====================================================
  // REMOVE FILE
  // =====================================================

  removeFile(): void {

    if (this.loading) {
      return;
    }

    this.selectedFile = null;

  }


  // =====================================================
  // SUBMIT ASSIGNMENT
  // =====================================================

  submitAssignment(): void {

    this.errorMessage = '';

    this.successMessage = '';


    // ---------------------------------------------------
    // ASSIGNMENT CHECK
    // ---------------------------------------------------

    if (!this.assignment) {

      this.errorMessage =
        'Assignment information is missing.';

      return;

    }


    // ---------------------------------------------------
    // DEADLINE CHECK
    // ---------------------------------------------------

    if (this.isOverdue()) {

      this.errorMessage =
        'The deadline for this assignment has passed.';

      return;

    }


    // ---------------------------------------------------
    // EXISTING SUBMISSION
    // ---------------------------------------------------

    if (this.existingSubmission) {

      this.updateSubmission();

      return;

    }


    // ---------------------------------------------------
    // SUBMISSION TEXT
    // ---------------------------------------------------

    const text =
      this.submissionText
        ?.trim() ?? '';


    // ---------------------------------------------------
    // TEXT / FILE VALIDATION
    // ---------------------------------------------------

    if (
      !text &&
      !this.selectedFile
    ) {

      this.errorMessage =
        'Please enter your submission or attach a file.';

      return;

    }


    // ---------------------------------------------------
    // LOADING
    // ---------------------------------------------------

    this.loading = true;


    // ---------------------------------------------------
    // REQUEST DATA
    // ---------------------------------------------------

    const data = {

      assignmentId:
        this.assignment.id,

      submissionText:
        text

    };


    // ---------------------------------------------------
    // CREATE SUBMISSION
    // ---------------------------------------------------

    this.submissionService
      .create(
        data,
        this.selectedFile ?? undefined
      )
      .subscribe({

        // ===============================================
        // SUCCESS
        // ===============================================

        next: (
          response: AssignmentSubmission
        ) => {

          this.loading = false;

          this.successMessage =
            'Assignment submitted successfully.';


          this.submitted.emit(
            response
          );

        },


        // ===============================================
        // ERROR
        // ===============================================

        error: (error) => {

          console.error(
            'Assignment submission error:',
            error
          );


          this.loading = false;


          this.errorMessage =
            error?.error?.message ||
            error?.error?.title ||
            'Failed to submit assignment. Please try again.';

        }

      });

  }


  // =====================================================
  // UPDATE EXISTING SUBMISSION
  // =====================================================

  private updateSubmission(): void {

    if (
      !this.existingSubmission
    ) {

      return;

    }


    // ---------------------------------------------------
    // DEADLINE
    // ---------------------------------------------------

    if (this.isOverdue()) {

      this.errorMessage =
        'The deadline for this assignment has passed.';

      return;

    }


    // ---------------------------------------------------
    // TEXT
    // ---------------------------------------------------

    const text =
      this.submissionText
        ?.trim() ?? '';


    // ---------------------------------------------------
    // VALIDATION
    // ---------------------------------------------------

    if (
      !text &&
      !this.selectedFile
    ) {

      this.errorMessage =
        'Please enter your submission or attach a file.';

      return;

    }


    // ---------------------------------------------------
    // LOADING
    // ---------------------------------------------------

    this.loading = true;


    // ---------------------------------------------------
    // REQUEST DATA
    // ---------------------------------------------------

    const data = {

      submissionText:
        text

    };


    // ---------------------------------------------------
    // UPDATE
    // ---------------------------------------------------

    this.submissionService
      .update(
        this.existingSubmission.id,

        data,

        this.selectedFile ?? undefined

      )
      .subscribe({

        // ===============================================
        // SUCCESS
        // ===============================================

        next: (
          response: AssignmentSubmission
        ) => {

          this.loading = false;

          this.successMessage =
            'Submission updated successfully.';


          this.submitted.emit(
            response
          );

        },


        // ===============================================
        // ERROR
        // ===============================================

        error: (error) => {

          console.error(
            'Update submission error:',
            error
          );


          this.loading = false;


          this.errorMessage =
            error?.error?.message ||
            error?.error?.title ||
            'Failed to update submission. Please try again.';

        }

      });

  }


  // =====================================================
  // CANCEL
  // =====================================================

  cancel(): void {

    if (this.loading) {

      return;

    }


    this.close.emit();

  }


  // =====================================================
  // DEADLINE CHECK
  // =====================================================

  isOverdue(): boolean {

    if (
      !this.assignment?.deadline
    ) {

      return false;

    }


    const deadline =
      new Date(
        this.assignment.deadline
      ).getTime();


    const now =
      new Date().getTime();


    return deadline < now;

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


    const date =
      new Date(
        this.assignment.deadline
      );


    return date.toLocaleString(
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

    if (!file?.name) {

      return '';

    }


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


  // =====================================================
  // FILE ICON
  // =====================================================

  getFileIcon(
    file: File
  ): string {

    if (!file) {

      return 'fa-file';

    }


    const extension =
      this.getFileExtension(file)
        .toLowerCase();


    switch (extension) {

      case 'pdf':

        return 'fa-file-pdf';


      case 'doc':

      case 'docx':

        return 'fa-file-word';


      case 'ppt':

      case 'pptx':

        return 'fa-file-powerpoint';


      case 'xls':

      case 'xlsx':

        return 'fa-file-excel';


      case 'jpg':

      case 'jpeg':

      case 'png':

        return 'fa-file-image';


      default:

        return 'fa-file';

    }

  }

}

