
import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';

import {
  FormBuilder,
  FormGroup,
  ReactiveFormsModule,
  Validators
} from '@angular/forms';

import {
  ActivatedRoute,
  Router
} from '@angular/router';

import { AcademicYearService } from '../../../../../core/services/academic-year.service';
import { ClassService } from '../../../../../core/services/class.service';
import { SectionService } from '../../../../../core/services/section.service';
import { CourseService } from '../../../../../core/services/course.service';
import { CourseSubjectService } from '../../../../../core/services/course-subject.service';
import { AssignmentService } from '../../../../../core/services/assignment.service';

import {
  AcademicYear
} from '../../../../../core/models/academic-year.model';

import {
  ClassModel
} from '../../../../../core/models/class.model';

import {
  SectionModel
} from '../../../../../core/models/section.model';

import {
  CourseModel
} from '../../../../../core/models/course.model';

import {
  SubjectModel
} from '../../../../../core/models/subject.model';

import {
  CourseSubjectModel
} from '../../../../../core/models/course-subject.model';

@Component({
  selector: 'app-assignment-form',
  standalone: true,

  imports: [
    CommonModule,
    ReactiveFormsModule
  ],

  templateUrl: './assignment-form.component.html',
  styleUrls: ['./assignment-form.component.scss']
})
export class AssignmentFormComponent implements OnInit {

  // =====================================================
  // FORM
  // =====================================================

  assignmentForm!: FormGroup;

  // =====================================================
  // MODE
  // =====================================================

  isEditMode = false;

  assignmentId: string | null = null;

  // =====================================================
  // STATE
  // =====================================================

  loading = false;

  saving = false;

  errorMessage = '';

  // =====================================================
  // FILE
  // =====================================================

  selectedFile: File | null = null;

  existingFileName: string | null = null;

  // =====================================================
  // DROPDOWN DATA
  // =====================================================

  academicYears: AcademicYear[] = [];

  classes: ClassModel[] = [];

  sections: SectionModel[] = [];

  courses: CourseModel[] = [];

  subjects: SubjectModel[] = [];
  

  // =====================================================
  // ALL DATA
  // =====================================================

  allClasses: ClassModel[] = [];

  allSections: SectionModel[] = [];

  allCourses: CourseModel[] = [];

  allCourseSubjects: CourseSubjectModel[] = [];

  allSubjects: SubjectModel[] = [];

  // =====================================================
  // CONSTRUCTOR
  // =====================================================

  constructor(
    private fb: FormBuilder,

    private assignmentService: AssignmentService,

    private academicYearService: AcademicYearService,

    private classService: ClassService,

    private sectionService: SectionService,

    private courseService: CourseService,

    private courseSubjectService: CourseSubjectService,

    private router: Router,

    private route: ActivatedRoute
  ) {}

  // =====================================================
  // INIT
  // =====================================================

  ngOnInit(): void {

    this.createForm();

    // ===================================================
    // GET ASSIGNMENT ID
    // ===================================================

    this.assignmentId =
      this.route.snapshot.paramMap.get('id');

    if (this.assignmentId) {

      this.isEditMode = true;

    }

    // ===================================================
    // DROPDOWN LISTENERS
    // ===================================================

    this.setupDropdownListeners();

    // ===================================================
    // LOAD DATA
    // ===================================================

    this.loadDropdownData();
  }

  // =====================================================
  // CREATE FORM
  // =====================================================

  private createForm(): void {

    this.assignmentForm =
      this.fb.group({

        academicYearId: [
          '',
          Validators.required
        ],

        classId: [
          '',
          Validators.required
        ],

        sectionId: [
          ''
        ],

        courseId: [
          '',
          Validators.required
        ],

        subjectId: [
          '',
          Validators.required
        ],

        title: [
          '',
          [
            Validators.required,
            Validators.maxLength(250)
          ]
        ],

        description: [
          ''
        ],

        deadline: [
          '',
          Validators.required
        ],

        maximumMarks: [
          100,
          [
            Validators.required,
            Validators.min(1)
          ]
        ],

        attachmentUrl: [
          ''
        ],

        attachmentFileName: [
          ''
        ],

        attachmentContentType: [
          ''
        ],

        attachmentFileSize: [
          null
        ],

        isPublished: [
          false
        ],

        isActive: [
          true
        ]

      });
  }

  // =====================================================
  // DROPDOWN CHANGE LISTENERS
  // =====================================================

  private setupDropdownListeners(): void {

    // ===================================================
    // ACADEMIC YEAR CHANGE
    // ===================================================

    this.assignmentForm
      .get('academicYearId')
      ?.valueChanges
      .subscribe((academicYearId) => {

        if (!academicYearId) {

          this.classes = [];
          this.sections = [];
          this.courses = [];
          this.subjects = [];

          this.assignmentForm.patchValue(
            {
              classId: '',
              sectionId: '',
              courseId: '',
              subjectId: ''
            },
            {
              emitEvent: false
            }
          );

          return;
        }

        // -----------------------------------------------
        // Academic Year → Class
        // -----------------------------------------------

        this.filterClassesByAcademicYear(
          academicYearId
        );

        // -----------------------------------------------
        // Clear dependent fields
        // -----------------------------------------------

        this.sections = [];
        this.courses = [];
        this.subjects = [];

        this.assignmentForm.patchValue(
          {
            classId: '',
            sectionId: '',
            courseId: '',
            subjectId: ''
          },
          {
            emitEvent: false
          }
        );

      });

    // ===================================================
    // CLASS CHANGE
    // ===================================================

    this.assignmentForm
      .get('classId')
      ?.valueChanges
      .subscribe((classId) => {

        if (!classId) {

          this.sections = [];
          this.courses = [];
          this.subjects = [];

          this.assignmentForm.patchValue(
            {
              sectionId: '',
              courseId: '',
              subjectId: ''
            },
            {
              emitEvent: false
            }
          );

          return;
        }

        // -----------------------------------------------
        // Class → Section
        // -----------------------------------------------

        this.filterSectionsByClass(
          classId
        );

        // -----------------------------------------------
        // Class → Course
        // -----------------------------------------------

        this.filterCoursesByClass(
          classId
        );

        // -----------------------------------------------
        // Clear dependent fields
        // -----------------------------------------------

        this.subjects = [];

        this.assignmentForm.patchValue(
          {
            sectionId: '',
            courseId: '',
            subjectId: ''
          },
          {
            emitEvent: false
          }
        );

      });

    // ===================================================
    // COURSE CHANGE
    // ===================================================

    this.assignmentForm
      .get('courseId')
      ?.valueChanges
      .subscribe((courseId) => {

        if (!courseId) {

          this.subjects = [];

          this.assignmentForm.patchValue(
            {
              subjectId: ''
            },
            {
              emitEvent: false
            }
          );

          return;
        }

        // -----------------------------------------------
        // Course → Subject
        //
        // IMPORTANT:
        // Subjects come from CourseSubjects
        // -----------------------------------------------

        this.filterSubjectsByCourse(
          courseId
        );

        this.assignmentForm.patchValue(
          {
            subjectId: ''
          },
          {
            emitEvent: false
          }
        );

      });

  }

  // =====================================================
  // LOAD DROPDOWN DATA
  // =====================================================

  private loadDropdownData(): void {

    this.loading = true;

    this.errorMessage = '';

    this.loadAcademicYears();
  }

  // =====================================================
  // LOAD ACADEMIC YEARS
  // =====================================================

  private loadAcademicYears(): void {

    this.academicYearService
      .getAll()
      .subscribe({

        next: (data: AcademicYear[]) => {

          console.log(
            'Academic Years:',
            data
          );

          this.academicYears =
            data ?? [];

          this.loadClasses();
        },

        error: (error) => {

          console.error(
            'Error loading academic years:',
            error
          );

          this.errorMessage =
            'Unable to load academic years.';

          this.loading = false;
        }

      });
  }

  // =====================================================
  // LOAD CLASSES
  // =====================================================

  private loadClasses(): void {

    this.classService
      .getAll()
      .subscribe({

        next: (data: ClassModel[]) => {

          console.log(
            'Classes:',
            data
          );

          this.allClasses =
            data ?? [];

          // Don't show anything until
          // Academic Year is selected.

          this.classes = [];

          this.loadSections();
        },

        error: (error) => {

          console.error(
            'Error loading classes:',
            error
          );

          this.errorMessage =
            'Unable to load classes.';

          this.loading = false;
        }

      });
  }

  // =====================================================
  // LOAD SECTIONS
  // =====================================================

  private loadSections(): void {

    this.sectionService
      .getAll()
      .subscribe({

        next: (data: SectionModel[]) => {

          console.log(
            'Sections:',
            data
          );

          this.allSections =
            data ?? [];

          // Sections will be loaded
          // after Class is selected.

          this.sections = [];

          this.loadCourses();
        },

        error: (error) => {

          console.error(
            'Error loading sections:',
            error
          );

          this.errorMessage =
            'Unable to load sections.';

          this.loading = false;
        }

      });
  }

  // =====================================================
  // LOAD COURSES
  // =====================================================

  private loadCourses(): void {

    this.courseService
      .getAll()
      .subscribe({

        next: (data: CourseModel[]) => {

          console.log(
            'Courses:',
            data
          );

          this.allCourses =
            data ?? [];

          // IMPORTANT:
          // Courses depend on CLASS,
          // not directly on Academic Year.

          this.courses = [];

          this.loadCourseSubjects();
        },

        error: (error) => {

          console.error(
            'Error loading courses:',
            error
          );

          this.errorMessage =
            'Unable to load courses.';

          this.loading = false;
        }

      });
  }

  // =====================================================
  // LOAD COURSE SUBJECT RELATIONSHIPS
  // =====================================================

  private loadCourseSubjects(): void {

    this.courseSubjectService
      .getAll()
      .subscribe({

        next: (data: CourseSubjectModel[]) => {

          console.log(
            'Course Subjects:',
            data
          );

          this.allCourseSubjects =
            data ?? [];

          this.subjects = [];

          // =============================================
          // LOAD SUBJECTS
          //
          // We still load the subject master data so that
          // we can resolve SubjectId → Subject details.
          // =============================================

          this.loadSubjects();
        },

        error: (error) => {

          console.error(
            'Error loading course subjects:',
            error
          );

          this.errorMessage =
            'Unable to load course subjects.';

          this.loading = false;
        }

      });
  }

  // =====================================================
  // LOAD SUBJECTS
  // =====================================================

  private loadSubjects(): void {

    /*
     * We need the Subject master data because
     * CourseSubjects normally contains:
     *
     * CourseId
     * SubjectId
     *
     * Therefore:
     *
     * Course
     *   ↓
     * CourseSubject
     *   ↓
     * Subject
     */

    // Lazy import is NOT required.
    // The SubjectService is intentionally not injected
    // because the subject list is resolved through
    // CourseSubjects.
    //
    // If your CourseSubject API already returns
    // SubjectName / Subject object, this method can
    // be simplified.

    this.loading = false;

    // =================================================
    // EDIT MODE
    // =================================================

    if (
      this.isEditMode &&
      this.assignmentId
    ) {

      this.loadAssignment(
        this.assignmentId
      );
    }
  }

  // =====================================================
  // FILTER CLASSES BY ACADEMIC YEAR
  // =====================================================

  private filterClassesByAcademicYear(
    academicYearId: string
  ): void {

    const selectedId =
      String(academicYearId);

    this.classes =
      this.allClasses
        .filter((item: any) =>
          String(item.academicYearId) ===
          selectedId
        )
        .sort((a: any, b: any) =>
          String(a.name ?? '')
            .localeCompare(
              String(b.name ?? ''),
              undefined,
              {
                numeric: true
              }
            )
        );

    console.log(
      'Classes filtered by Academic Year:',
      this.classes
    );
  }

  // =====================================================
  // FILTER SECTIONS BY CLASS
  // =====================================================

  private filterSectionsByClass(
    classId: string
  ): void {

    const selectedId =
      String(classId);

    this.sections =
      this.allSections
        .filter((item: any) =>
          String(item.classId) ===
          selectedId
        )
        .sort((a: any, b: any) =>
          String(a.sectionName ?? '')
            .localeCompare(
              String(b.sectionName ?? '')
            )
        );

    console.log(
      'Sections filtered by Class:',
      this.sections
    );
  }

  // =====================================================
  // FILTER COURSES BY CLASS
  // =====================================================

  private filterCoursesByClass(
    classId: string
  ): void {

    const selectedId =
      String(classId);

    this.courses =
      this.allCourses
        .filter((item: any) =>
          String(item.classId) ===
          selectedId
        )
        .sort((a: any, b: any) =>
          String(
            a.courseName ??
            a.name ??
            ''
          ).localeCompare(
            String(
              b.courseName ??
              b.name ??
              ''
            )
          )
        );

    console.log(
      'Courses filtered by Class:',
      this.courses
    );
  }

  // =====================================================
  // FILTER SUBJECTS BY COURSE
  // =====================================================

  // =====================================================
// FILTER SUBJECTS BY COURSE
// =====================================================

private filterSubjectsByCourse(
  courseId: string
): void {

  const selectedCourseId =
    String(courseId);

  // ===================================================
  // CourseSubject belongs to selected Course
  // ===================================================

  const courseSubjectRows =
    this.allCourseSubjects
      .filter((item: CourseSubjectModel) =>
        String(item.courseId) === selectedCourseId
      )
      .sort((a, b) =>
        (a.displayOrder ?? 0) -
        (b.displayOrder ?? 0)
      );

  console.log(
    'Selected Course ID:',
    selectedCourseId
  );

  console.log(
    'CourseSubject rows for selected course:',
    courseSubjectRows
  );

  // ===================================================
  // Convert CourseSubject → Subject dropdown data
  // ===================================================

  this.subjects =
    courseSubjectRows.map(
      (item: CourseSubjectModel) => ({
        id: item.subjectId,
        name: item.subjectName
      } as SubjectModel)
    );

  console.log(
    'Filtered Subjects:',
    this.subjects
  );
}

  // =====================================================
  // LOAD ASSIGNMENT
  // =====================================================

  private loadAssignment(
    id: string
  ): void {

    this.loading = true;

    this.errorMessage = '';

    this.assignmentService
      .getById(id)
      .subscribe({

        next: (assignment: any) => {

          console.log(
            'Assignment:',
            assignment
          );

          this.patchAssignment(
            assignment
          );

          this.loading = false;
        },

        error: (error) => {

          console.error(
            'Error loading assignment:',
            error
          );

          this.errorMessage =
            'Unable to load assignment.';

          this.loading = false;
        }

      });
  }

  // =====================================================
  // PATCH ASSIGNMENT
  // =====================================================

  private patchAssignment(
    assignment: any
  ): void {

    if (!assignment) {
      return;
    }

    // =================================================
    // EXISTING FILE
    // =================================================

    this.existingFileName =
      assignment.attachmentFileName ??
      null;

    // =================================================
    // IDS
    // =================================================

    const academicYearId =
      assignment.academicYearId ?? '';

    const classId =
      assignment.classId ?? '';

    const sectionId =
      assignment.sectionId ?? '';

    const courseId =
      assignment.courseId ?? '';

    const subjectId =
      assignment.subjectId ?? '';

    // =================================================
    // FILTER DEPENDENT DROPDOWNS
    // =================================================

    if (academicYearId) {

      this.filterClassesByAcademicYear(
        academicYearId
      );
    }

    if (classId) {

      this.filterSectionsByClass(
        classId
      );

      this.filterCoursesByClass(
        classId
      );
    }

    if (courseId) {

      this.filterSubjectsByCourse(
        courseId
      );
    }

    // =================================================
    // PATCH FORM
    // =================================================

    this.assignmentForm.patchValue(
      {

        academicYearId:
          academicYearId,

        classId:
          classId,

        sectionId:
          sectionId,

        courseId:
          courseId,

        subjectId:
          subjectId,

        title:
          assignment.title ?? '',

        description:
          assignment.description ?? '',

        deadline:
          this.formatDateTimeLocal(
            assignment.deadline
          ),

        maximumMarks:
          assignment.maximumMarks ?? 100,

        attachmentUrl:
          assignment.attachmentUrl ?? '',

        attachmentFileName:
          assignment.attachmentFileName ?? '',

        attachmentContentType:
          assignment.attachmentContentType ?? '',

        attachmentFileSize:
          assignment.attachmentFileSize ?? null,

        isPublished:
          assignment.isPublished ?? false,

        isActive:
          assignment.isActive ?? true

      },

      {
        emitEvent: false
      }
    );
  }

  // =====================================================
  // SUBMIT
  // =====================================================

  onSubmit(): void {

    this.errorMessage = '';

    // =================================================
    // VALIDATION
    // =================================================

    if (
      this.assignmentForm.invalid
    ) {

      this.assignmentForm.markAllAsTouched();

      return;
    }

    // =================================================
    // PREVENT DOUBLE SUBMIT
    // =================================================

    if (this.saving) {
      return;
    }

    this.saving = true;

    const formValue =
      this.assignmentForm.getRawValue();

    // =================================================
    // FORM DATA
    // =================================================

    const formData =
      new FormData();

    // =================================================
    // BASIC INFORMATION
    // =================================================

    if (formValue.academicYearId) {

      formData.append(
        'AcademicYearId',
        String(
          formValue.academicYearId
        )
      );
    }

    if (formValue.classId) {

      formData.append(
        'ClassId',
        String(
          formValue.classId
        )
      );
    }

    if (formValue.sectionId) {

      formData.append(
        'SectionId',
        String(
          formValue.sectionId
        )
      );
    }

    if (formValue.courseId) {

      formData.append(
        'CourseId',
        String(
          formValue.courseId
        )
      );
    }

    if (formValue.subjectId) {

      formData.append(
        'SubjectId',
        String(
          formValue.subjectId
        )
      );
    }

    // =================================================
    // ASSIGNMENT INFORMATION
    // =================================================

    formData.append(
      'Title',
      (
        formValue.title ?? ''
      ).trim()
    );

    if (
      formValue.description &&
      formValue.description.trim()
    ) {

      formData.append(
        'Description',
        formValue.description.trim()
      );
    }

    // =================================================
    // DEADLINE
    // =================================================

    if (formValue.deadline) {

      const deadline =
        new Date(
          formValue.deadline
        );

      if (
        !isNaN(
          deadline.getTime()
        )
      ) {

        formData.append(
          'Deadline',
          deadline.toISOString()
        );
      }
    }

    // =================================================
    // MAXIMUM MARKS
    // =================================================

    formData.append(
      'MaximumMarks',
      String(
        formValue.maximumMarks
      )
    );

    // =================================================
    // PUBLICATION
    // =================================================

    formData.append(
      'IsPublished',
      formValue.isPublished
        ? 'true'
        : 'false'
    );

    formData.append(
      'IsActive',
      formValue.isActive
        ? 'true'
        : 'false'
    );

    // =================================================
    // FILE
    // =================================================

    if (this.selectedFile) {

      formData.append(
        'Attachment',
        this.selectedFile,
        this.selectedFile.name
      );
    }

    // =================================================
    // DEBUG
    // =================================================

    console.log(
      'Assignment Form Data:'
    );

    formData.forEach(
      (value, key) => {

        console.log(
          key,
          value
        );

      }
    );

    // =================================================
    // CREATE / UPDATE
    // =================================================

    if (
      this.isEditMode &&
      this.assignmentId
    ) {

      this.updateAssignment(
        this.assignmentId,
        formData
      );

    }
    else {

      this.createAssignment(
        formData
      );
    }
  }

  // =====================================================
  // CREATE ASSIGNMENT
  // =====================================================

  private createAssignment(
    formData: FormData
  ): void {

    this.assignmentService
      .create(formData)
      .subscribe({

        next: (response) => {

          console.log(
            'Assignment created:',
            response
          );

          this.saving = false;

          this.router.navigate([
            '/admin/assignments'
          ]);
        },

        error: (error) => {

          console.error(
            'Error creating assignment:',
            error
          );

          this.handleError(
            error,
            'Unable to create assignment.'
          );
        }

      });
  }

  // =====================================================
  // UPDATE ASSIGNMENT
  // =====================================================

  private updateAssignment(
    id: string,
    formData: FormData
  ): void {

    this.assignmentService
      .update(
        id,
        formData
      )
      .subscribe({

        next: (response) => {

          console.log(
            'Assignment updated:',
            response
          );

          this.saving = false;

          this.router.navigate([
            '/admin/assignments'
          ]);
        },

        error: (error) => {

          console.error(
            'Error updating assignment:',
            error
          );

          this.handleError(
            error,
            'Unable to update assignment.'
          );
        }

      });
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

      return;
    }

    const file =
      input.files[0];

    this.selectedFile =
      file;

    this.existingFileName =
      null;

    this.assignmentForm.patchValue({

      attachmentFileName:
        file.name,

      attachmentContentType:
        file.type,

      attachmentFileSize:
        file.size

    });

    console.log(
      'Selected file:',
      file
    );
  }

  // =====================================================
  // REMOVE FILE
  // =====================================================

  removeFile(): void {

    this.selectedFile =
      null;

    this.existingFileName =
      null;

    this.assignmentForm.patchValue({

      attachmentUrl:
        '',

      attachmentFileName:
        '',

      attachmentContentType:
        '',

      attachmentFileSize:
        null

    });
  }

  // =====================================================
  // FILE ICON
  // =====================================================

  getFileIcon(
    fileName: string
  ): string {

    if (!fileName) {

      return 'fa-file';
    }

    const extension =
      fileName
        .split('.')
        .pop()
        ?.toLowerCase();

    switch (extension) {

      case 'pdf':
        return 'fa-file-pdf';

      case 'doc':
      case 'docx':
        return 'fa-file-word';

      case 'xls':
      case 'xlsx':
        return 'fa-file-excel';

      case 'ppt':
      case 'pptx':
        return 'fa-file-powerpoint';

      case 'jpg':
      case 'jpeg':
      case 'png':
      case 'gif':
      case 'webp':
        return 'fa-file-image';

      case 'zip':
      case 'rar':
      case '7z':
        return 'fa-file-archive';

      case 'txt':
        return 'fa-file-alt';

      default:
        return 'fa-file';
    }
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

    const safeIndex =
      Math.min(
        index,
        units.length - 1
      );

    return (
      parseFloat(
        (
          bytes /
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
  // FIELD VALIDATION
  // =====================================================

  isFieldInvalid(
    fieldName: string
  ): boolean {

    const control =
      this.assignmentForm.get(
        fieldName
      );

    return !!(
      control &&
      control.invalid &&
      (
        control.touched ||
        control.dirty
      )
    );
  }

  // =====================================================
  // FORMAT DATETIME LOCAL
  // =====================================================

  private formatDateTimeLocal(
    value:
      string |
      Date |
      null |
      undefined
  ): string {

    if (!value) {

      return '';
    }

    const date =
      new Date(value);

    if (
      isNaN(
        date.getTime()
      )
    ) {

      return '';
    }

    const year =
      date.getFullYear();

    const month =
      String(
        date.getMonth() + 1
      ).padStart(
        2,
        '0'
      );

    const day =
      String(
        date.getDate()
      ).padStart(
        2,
        '0'
      );

    const hours =
      String(
        date.getHours()
      ).padStart(
        2,
        '0'
      );

    const minutes =
      String(
        date.getMinutes()
      ).padStart(
        2,
        '0'
      );

    return (
      `${year}-${month}-${day}` +
      `T${hours}:${minutes}`
    );
  }

  // =====================================================
  // ERROR HANDLING
  // =====================================================

  private handleError(
    error: any,
    defaultMessage: string
  ): void {

    this.saving = false;

    console.error(
      'Assignment error:',
      error
    );

    // =================================================
    // API MESSAGE
    // =================================================

    if (
      error?.error?.message
    ) {

      this.errorMessage =
        error.error.message;

      return;
    }

    // =================================================
    // STRING ERROR
    // =================================================

    if (
      typeof error?.error === 'string'
    ) {

      this.errorMessage =
        error.error;

      return;
    }

    // =================================================
    // VALIDATION ERRORS
    // =================================================

    if (
      error?.error?.errors
    ) {

      const errors =
        error.error.errors;

      const messages: string[] = [];

      Object.keys(errors)
        .forEach(
          key => {

            const fieldErrors =
              errors[key];

            if (
              Array.isArray(
                fieldErrors
              )
            ) {

              messages.push(
                ...fieldErrors
              );
            }

          }
        );

      if (
        messages.length > 0
      ) {

        this.errorMessage =
          messages.join(' ');

        return;
      }
    }

    // =================================================
    // STATUS 400
    // =================================================

    if (
      error?.status === 400
    ) {

      this.errorMessage =
        'Invalid assignment data. Please check all required fields.';

      return;
    }

    // =================================================
    // STATUS 404
    // =================================================

    if (
      error?.status === 404
    ) {

      this.errorMessage =
        'Assignment was not found.';

      return;
    }

    // =================================================
    // STATUS 500
    // =================================================

    if (
      error?.status >= 500
    ) {

      this.errorMessage =
        'Server error. Please try again later.';

      return;
    }

    // =================================================
    // DEFAULT
    // =================================================

    this.errorMessage =
      defaultMessage;
  }

  // =====================================================
  // BACK
  // =====================================================

  goBack(): void {

    this.router.navigate([
      '/admin/assignments'
    ]);
  }

}

