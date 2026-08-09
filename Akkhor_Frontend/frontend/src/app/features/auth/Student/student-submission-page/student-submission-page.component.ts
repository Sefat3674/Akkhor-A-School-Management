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

import {
  StudentSubmissionFormComponent
} from '../student-submission-form/student-submission-form.component';


@Component({
  selector: 'app-student-submission-page',

  standalone: true,

  imports: [
    CommonModule,
    StudentSubmissionFormComponent
  ],

  templateUrl:
    './student-submission-page.component.html',

  styleUrls: [
    './student-submission-page.component.scss'
  ]
})
export class StudentSubmissionPageComponent
  implements OnInit {


  // =====================================================
  // ASSIGNMENT
  // =====================================================

  assignment:
    StudentAssignment | null = null;


  // =====================================================
  // EXISTING SUBMISSION
  // =====================================================

  existingSubmission:
    AssignmentSubmission | null = null;


  // =====================================================
  // UI STATE
  // =====================================================

  loading = false;

  errorMessage = '';


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

    const assignmentId =
      this.route.snapshot.paramMap.get('id');


    console.log(
      'Submission Assignment ID:',
      assignmentId
    );


    if (!assignmentId) {

      this.errorMessage =
        'Assignment ID is missing.';

      return;

    }


    this.loadAssignment(
      assignmentId
    );

  }


  // =====================================================
  // LOAD ASSIGNMENT
  // =====================================================

  loadAssignment(
    assignmentId: string
  ): void {

    this.loading = true;

    this.errorMessage = '';


    this.assignmentService
      .getMyAssignments()
      .subscribe({

        next: (
          assignments: StudentAssignment[]
        ) => {

          console.log(
            'Student assignments:',
            assignments
          );


          const foundAssignment =
            assignments.find(
              assignment =>
                assignment.id ===
                assignmentId
            );


          // -----------------------------------------------
          // NOT FOUND
          // -----------------------------------------------

          if (!foundAssignment) {

            this.assignment = null;

            this.errorMessage =
              'Assignment not found.';

            this.loading = false;

            return;

          }


          // -----------------------------------------------
          // ASSIGNMENT FOUND
          // -----------------------------------------------

          this.assignment =
            foundAssignment;


          console.log(
            'Selected assignment:',
            this.assignment
          );


          // -----------------------------------------------
          // LOAD EXISTING SUBMISSION
          // -----------------------------------------------

          this.loadExistingSubmission(
            assignmentId
          );

        },


        error: (error) => {

          console.error(
            'Error loading assignment:',
            error
          );


          this.assignment = null;

          this.errorMessage =
            'Unable to load assignment.';

          this.loading = false;

        }

      });

  }


  // =====================================================
  // LOAD EXISTING SUBMISSION
  // =====================================================

  loadExistingSubmission(
    assignmentId: string
  ): void {

    this.submissionService
      .getMySubmissions()
      .subscribe({

        next: (
          submissions: AssignmentSubmission[]
        ) => {

          console.log(
            'My submissions:',
            submissions
          );


          this.existingSubmission =
            submissions.find(
              submission =>
                submission.assignmentId ===
                assignmentId
            ) ?? null;


          console.log(
            'Existing submission:',
            this.existingSubmission
          );


          this.loading = false;

        },


        error: (error) => {

          console.error(
            'Error loading submissions:',
            error
          );


          // No existing submission is okay.
          // Student can create a new submission.

          this.existingSubmission = null;

          this.loading = false;

        }

      });

  }


  // =====================================================
  // BACK
  // =====================================================

  cancel(): void {

    if (this.loading) {

      return;

    }


    this.router.navigate([
      '/student/assignments'
    ]);

  }


  // =====================================================
  // SUBMITTED
  // =====================================================

  onSubmitted(
    submission: AssignmentSubmission
  ): void {

    console.log(
      'Submission completed:',
      submission
    );


    this.existingSubmission =
      submission;

  }

}