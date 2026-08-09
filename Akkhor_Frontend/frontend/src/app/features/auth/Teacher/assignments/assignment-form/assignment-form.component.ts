import { CommonModule } from '@angular/common';

import {
  Component,
  OnDestroy,
  OnInit
} from '@angular/core';

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

import {
  Subject,
  forkJoin,
  takeUntil
} from 'rxjs';

import { AcademicYearService }
  from '../../../../../core/services/academic-year.service';

import { ClassService }
  from '../../../../../core/services/class.service';

import { SectionService }
  from '../../../../../core/services/section.service';

import { CourseService }
  from '../../../../../core/services/course.service';

import { CourseSubjectService }
  from '../../../../../core/services/course-subject.service';

import { AssignmentService }
  from '../../../../../core/services/assignment.service';

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

  templateUrl:
    './assignment-form.component.html',

  styleUrls: [
    './assignment-form.component.scss'
  ]
})
export class AssignmentFormComponent
  implements OnInit, OnDestroy {


  // =====================================================
  // DESTROY
  // =====================================================

  private destroy$ =
    new Subject<void>();


  // =====================================================
  // FORM
  // =====================================================

  assignmentForm!: FormGroup;


  // =====================================================
  // MODE
  // =====================================================

  isEditMode = false;

  assignmentId:
    string | null = null;


  // =====================================================
  // STATE
  // =====================================================

  loading = false;

  saving = false;

  errorMessage = '';


  // =====================================================
  // FILE
  // =====================================================

  selectedFile:
    File | null = null;

  existingFileName:
    string | null = null;


  // =====================================================
  // DROPDOWN DATA
  // =====================================================

  academicYears:
    AcademicYear[] = [];

  classes:
    ClassModel[] = [];

  sections:
    SectionModel[] = [];

  courses:
    CourseModel[] = [];

  subjects:
    SubjectModel[] = [];


  // =====================================================
  // ALL DATA
  // =====================================================

  allClasses:
    ClassModel[] = [];

  allSections:
    SectionModel[] = [];

  allCourses:
    CourseModel[] = [];

  allCourseSubjects:
    CourseSubjectModel[] = [];


  // =====================================================
  // CONSTRUCTOR
  // =====================================================

  constructor(

    private fb: FormBuilder,

    private assignmentService:
      AssignmentService,

    private academicYearService:
      AcademicYearService,

    private classService:
      ClassService,

    private sectionService:
      SectionService,

    private courseService:
      CourseService,

    private courseSubjectService:
      CourseSubjectService,

    private router: Router,

    private route: ActivatedRoute

  ) {}


  // =====================================================
  // INIT
  // =====================================================

  ngOnInit(): void {

    this.createForm();


    this.assignmentId =
      this.route.snapshot.paramMap.get('id');


    this.isEditMode =
      !!this.assignmentId;


    this.setupDropdownListeners();


    this.loadAllDropdownData();

  }


  // =====================================================
  // DESTROY
  // =====================================================

  ngOnDestroy(): void {

    this.destroy$.next();

    this.destroy$.complete();

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
  // DROPDOWN LISTENERS
  // =====================================================

  private setupDropdownListeners(): void {


    // ===================================================
    // ACADEMIC YEAR
    // ===================================================

    this.assignmentForm
      .get('academicYearId')
      ?.valueChanges
      .pipe(
        takeUntil(this.destroy$)
      )
      .subscribe(
        (academicYearId: string) => {

          if (!academicYearId) {

            this.classes = [];

            this.sections = [];

            this.courses = [];

            this.subjects = [];

            this.clearFields([
              'classId',
              'sectionId',
              'courseId',
              'subjectId'
            ]);

            return;
          }


          this.filterClassesByAcademicYear(
            academicYearId
          );

        }
      );


    // ===================================================
    // CLASS
    // ===================================================

    this.assignmentForm
      .get('classId')
      ?.valueChanges
      .pipe(
        takeUntil(this.destroy$)
      )
      .subscribe(
        (classId: string) => {

          if (!classId) {

            this.sections = [];

            this.courses = [];

            this.subjects = [];

            this.clearFields([
              'sectionId',
              'courseId',
              'subjectId'
            ]);

            return;
          }


          this.filterSectionsByClass(
            classId
          );


          this.filterCoursesByClass(
            classId
          );

        }
      );


    // ===================================================
    // COURSE
    // ===================================================

    this.assignmentForm
      .get('courseId')
      ?.valueChanges
      .pipe(
        takeUntil(this.destroy$)
      )
      .subscribe(
        (courseId: string) => {

          if (!courseId) {

            this.subjects = [];

            this.clearFields([
              'subjectId'
            ]);

            return;
          }


          this.filterSubjectsByCourse(
            courseId
          );

        }
      );

  }


  // =====================================================
  // CLEAR FIELDS
  // =====================================================

  private clearFields(
    fields: string[]
  ): void {

    const patch: Record<string, any> = {};


    fields.forEach(
      field => {

        patch[field] = '';

      }
    );


    this.assignmentForm.patchValue(
      patch,
      {
        emitEvent: false
      }
    );

  }


  // =====================================================
  // LOAD ALL DROPDOWN DATA
  // =====================================================

  private loadAllDropdownData(): void {

    this.loading = true;

    this.errorMessage = '';


    forkJoin({

      academicYears:
        this.academicYearService.getAll(),

      classes:
        this.classService.getAll(),

      sections:
        this.sectionService.getAll(),

      courses:
        this.courseService.getAll(),

      courseSubjects:
        this.courseSubjectService.getAll()

    })
      .pipe(
        takeUntil(this.destroy$)
      )
      .subscribe({

        next: (result) => {

          this.academicYears =
            result.academicYears ?? [];


          this.allClasses =
            result.classes ?? [];


          this.allSections =
            result.sections ?? [];


          this.allCourses =
            result.courses ?? [];


          this.allCourseSubjects =
            result.courseSubjects ?? [];


          this.loading = false;


          if (
            this.isEditMode &&
            this.assignmentId
          ) {

            this.loadAssignment(
              this.assignmentId
            );

          }

        },

        error: (error) => {

          console.error(
            'Error loading dropdown data:',
            error
          );

          this.errorMessage =
            'Unable to load assignment dropdown data.';

          this.loading = false;

        }

      });

  }


  // =====================================================
  // FILTER CLASSES
  // =====================================================

  private filterClassesByAcademicYear(
    academicYearId: string
  ): void {

    const selectedId =
      String(academicYearId);


    this.classes =
      this.allClasses
        .filter(
          (item: any) =>
            String(item.academicYearId) ===
            selectedId
        )
        .sort(
          (a: any, b: any) =>
            String(a.name ?? '')
              .localeCompare(
                String(b.name ?? ''),
                undefined,
                {
                  numeric: true
                }
              )
        );

  }


  // =====================================================
  // FILTER SECTIONS
  // =====================================================

  private filterSectionsByClass(
    classId: string
  ): void {

    const selectedId =
      String(classId);


    this.sections =
      this.allSections
        .filter(
          (item: any) =>
            String(item.classId) ===
            selectedId
        )
        .sort(
          (a: any, b: any) =>
            String(
              a.sectionName ?? ''
            ).localeCompare(
              String(
                b.sectionName ?? ''
              ),
              undefined,
              {
                numeric: true
              }
            )
        );

  }


  // =====================================================
  // FILTER COURSES
  // =====================================================

  private filterCoursesByClass(
    classId: string
  ): void {

    const selectedId =
      String(classId);


    this.courses =
      this.allCourses
        .filter(
          (item: any) =>
            String(item.classId) ===
            selectedId
        )
        .sort(
          (a: any, b: any) => {

            const nameA =
              String(
                a.courseName ??
                a.name ??
                ''
              );


            const nameB =
              String(
                b.courseName ??
                b.name ??
                ''
              );


            return nameA.localeCompare(
              nameB,
              undefined,
              {
                numeric: true
              }
            );

          }
        );

  }


  // =====================================================
  // FILTER SUBJECTS
  // =====================================================

  private filterSubjectsByCourse(
    courseId: string
  ): void {

    const selectedCourseId =
      String(courseId);


    const rows =
      this.allCourseSubjects
        .filter(
          (item: any) =>
            String(item.courseId) ===
            selectedCourseId
        )
        .sort(
          (a: any, b: any) =>
            Number(
              a.displayOrder ?? 0
            ) -
            Number(
              b.displayOrder ?? 0
            )
        );


    this.subjects =
      rows
        .map(
          (item: any) => {

            const subjectId =
              item.subjectId ??
              item.subject?.id;


            const subjectName =
              item.subjectName ??
              item.subject?.name ??
              item.name ??
              `Subject ${subjectId}`;


            return {
              id: subjectId,
              name: subjectName
            } as SubjectModel;

          }
        )
        .filter(
          item => !!item.id
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
      .pipe(
        takeUntil(this.destroy$)
      )
      .subscribe({

        next: (assignment: any) => {

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


    // Existing attachment

    this.existingFileName =
      assignment.attachmentFileName ??
      null;


    this.selectedFile =
      null;


    const academicYearId =
      assignment.academicYearId ??
      '';


    const classId =
      assignment.classId ??
      '';


    const sectionId =
      assignment.sectionId ??
      '';


    const courseId =
      assignment.courseId ??
      '';


    const subjectId =
      assignment.subjectId ??
      '';


    // Filter dropdowns

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


    // Patch form

    this.assignmentForm.patchValue(

      {

        academicYearId,

        classId,

        sectionId,

        courseId,

        subjectId,

        title:
          assignment.title ??
          '',

        description:
          assignment.description ??
          '',

        deadline:
          this.formatDateTimeLocal(
            assignment.deadline
          ),

        maximumMarks:
          assignment.maximumMarks ??
          100,

        attachmentUrl:
          assignment.attachmentUrl ??
          '',

        attachmentFileName:
          assignment.attachmentFileName ??
          '',

        attachmentContentType:
          assignment.attachmentContentType ??
          '',

        attachmentFileSize:
          assignment.attachmentFileSize ??
          null,

        isPublished:
          assignment.isPublished ??
          false,

        isActive:
          assignment.isActive ??
          true

      },

      {
        emitEvent: false
      }

    );

  }


  // =====================================================
  // OPEN FILE PICKER
  // =====================================================

  openFilePicker(
    input: HTMLInputElement
  ): void {

    input.click();

  }


  // =====================================================
  // FILE SELECTED
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


    console.log(
      'Selected file:',
      file
    );


    // Optional file size limit: 20 MB

    const maxSize =
      20 * 1024 * 1024;


    if (file.size > maxSize) {

      this.errorMessage =
        'File size cannot exceed 20 MB.';

      input.value = '';

      this.selectedFile = null;

      return;

    }


    // Clear previous error

    this.errorMessage = '';


    // Store actual File object

    this.selectedFile =
      file;


    // New file replaces old file

    this.existingFileName =
      null;


    // Update form metadata

    this.assignmentForm.patchValue({

      attachmentUrl:
        '',

      attachmentFileName:
        file.name,

      attachmentContentType:
        file.type ||
        'application/octet-stream',

      attachmentFileSize:
        file.size

    });


    this.assignmentForm
      .get('attachmentFileName')
      ?.markAsDirty();


    console.log(
      'Selected file name:',
      this.selectedFile.name
    );

    console.log(
      'Selected file size:',
      this.selectedFile.size
    );

    console.log(
      'Selected file type:',
      this.selectedFile.type
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


    const input =
      document.getElementById(
        'assignmentAttachment'
      ) as HTMLInputElement | null;


    if (input) {

      input.value = '';

    }

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

      )

      +

      ' ' +

      units[safeIndex]

    );

  }


  // =====================================================
  // SUBMIT
  // =====================================================

  onSubmit(): void {

    this.errorMessage = '';


    if (
      this.assignmentForm.invalid
    ) {

      this.assignmentForm.markAllAsTouched();

      return;

    }


    if (this.saving) {

      return;

    }


    this.saving = true;


    const value =
      this.assignmentForm.getRawValue();


    const formData =
      new FormData();


    // ===================================================
    // IDS
    // ===================================================

    this.appendIfValue(
      formData,
      'AcademicYearId',
      value.academicYearId
    );


    this.appendIfValue(
      formData,
      'ClassId',
      value.classId
    );


    this.appendIfValue(
      formData,
      'SectionId',
      value.sectionId
    );


    this.appendIfValue(
      formData,
      'CourseId',
      value.courseId
    );


    this.appendIfValue(
      formData,
      'SubjectId',
      value.subjectId
    );


    // ===================================================
    // TITLE
    // ===================================================

    formData.append(
      'Title',
      String(
        value.title ?? ''
      ).trim()
    );


    // ===================================================
    // DESCRIPTION
    // ===================================================

    if (
      value.description &&
      String(value.description).trim()
    ) {

      formData.append(
        'Description',
        String(
          value.description
        ).trim()
      );

    }


    // ===================================================
    // DEADLINE
    // ===================================================

    if (value.deadline) {

      const deadline =
        new Date(
          value.deadline
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


    // ===================================================
    // MAXIMUM MARKS
    // ===================================================

    formData.append(
      'MaximumMarks',
      String(
        value.maximumMarks
      )
    );


    // ===================================================
    // PUBLISHED
    // ===================================================

    formData.append(
      'IsPublished',
      value.isPublished
        ? 'true'
        : 'false'
    );


    // ===================================================
    // ACTIVE
    // ===================================================

    formData.append(
      'IsActive',
      value.isActive
        ? 'true'
        : 'false'
    );


    // ===================================================
    // ACTUAL FILE
    // ===================================================

    if (this.selectedFile) {

      console.log(
        'Uploading actual file:',
        this.selectedFile.name
      );


      console.log(
        'File size:',
        this.selectedFile.size
      );


      console.log(
        'File type:',
        this.selectedFile.type
      );


      formData.append(
        'Attachment',
        this.selectedFile,
        this.selectedFile.name
      );

    }


    // ===================================================
    // DEBUG
    // ===================================================

    console.log(
      '========== FORM DATA =========='
    );


    formData.forEach(
      (item, key) => {

        if (item instanceof File) {

          console.log(
            key,
            'FILE:',
            item.name,
            item.size,
            item.type
          );

        }
        else {

          console.log(
            key,
            item
          );

        }

      }
    );


    // ===================================================
    // CREATE / UPDATE
    // ===================================================

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
  // APPEND VALUE
  // =====================================================

  private appendIfValue(
    formData: FormData,
    key: string,
    value: any
  ): void {

    if (
      value !== null &&
      value !== undefined &&
      String(value).trim() !== ''
    ) {

      formData.append(
        key,
        String(value)
      );

    }

  }


  // =====================================================
  // CREATE
  // =====================================================

  private createAssignment(
    formData: FormData
  ): void {

    this.assignmentService
      .create(formData)
      .pipe(
        takeUntil(this.destroy$)
      )
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
            'Create assignment error:',
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
  // UPDATE
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
      .pipe(
        takeUntil(this.destroy$)
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
            'Update assignment error:',
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
  // DATETIME LOCAL
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


    if (
      error?.error?.message
    ) {

      this.errorMessage =
        error.error.message;

      return;

    }


    if (
      typeof error?.error === 'string'
    ) {

      this.errorMessage =
        error.error;

      return;

    }


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


    if (
      error?.status === 400
    ) {

      this.errorMessage =
        'Invalid assignment data. Please check all required fields.';

      return;

    }


    if (
      error?.status === 404
    ) {

      this.errorMessage =
        'Assignment was not found.';

      return;

    }


    if (
      error?.status >= 500
    ) {

      this.errorMessage =
        'Server error. Please try again later.';

      return;

    }


    this.errorMessage =
      defaultMessage;

  }


  // =====================================================
  // BACK
  // =====================================================

  goBack(): void {

    if (this.saving) {

      return;

    }


    this.router.navigate([
      '/admin/assignments'
    ]);

  }

}