import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

import { StudentEnrollmentService } from '../../../../core/services/student-enrollment.service';

import {
  StudentEnrollment,
  CreateStudentEnrollment,
  UpdateStudentEnrollment
} from '../../../../core/models/student-enrollment.model';


// =====================================================
// LOOKUP MODELS
// =====================================================

interface Student {
  id: string;
  userName: string;
}

interface ClassModel {
  id: string;
  name: string;
}

interface CourseModel {
  id: string;
  classId: string;
  courseName: string;
}

interface SectionModel {
  id: string;
  classId: string;
  sectionName: string;
}


@Component({
  selector: 'app-student-enrollment',
  standalone: true,

  imports: [
    CommonModule,
    FormsModule
  ],

  templateUrl: './student-enrollment.component.html',

  styleUrls: [
    './student-enrollment.component.scss'
  ]
})
export class StudentEnrollmentComponent implements OnInit {

  // =====================================================
  // ENROLLMENTS
  // =====================================================

  enrollments: StudentEnrollment[] = [];

  filteredEnrollments: StudentEnrollment[] = [];


  // =====================================================
  // LOOKUPS
  // =====================================================

  students: Student[] = [];

  classes: ClassModel[] = [];

  courses: CourseModel[] = [];

  sections: SectionModel[] = [];


  // =====================================================
  // UI STATE
  // =====================================================

  isModalOpen = false;

  isEditMode = false;

  isLoading = false;

  isSaving = false;

  isLoadingStudents = false;

  isLoadingClasses = false;

  isLoadingCourses = false;

  isLoadingSections = false;

  selectedId: string | null = null;


  // =====================================================
  // SEARCH
  // =====================================================

  searchText = '';


  // =====================================================
  // FORM
  // =====================================================

  formData: CreateStudentEnrollment = {
    studentId: '',
    classId: '',
    courseId: '',
    sectionId: null,
    rollNumber: '',
    enrollmentDate: this.getToday(),
    status: 'Active'
  };


  // =====================================================
  // CONSTRUCTOR
  // =====================================================

  constructor(
    private enrollmentService: StudentEnrollmentService
  ) {}


  // =====================================================
  // INIT
  // =====================================================

  ngOnInit(): void {

    this.loadEnrollments();

    this.loadStudents();

    this.loadClasses();
  }


  // =====================================================
  // LOAD ENROLLMENTS
  // =====================================================

  loadEnrollments(): void {

    this.isLoading = true;

    this.enrollmentService
      .getAll()
      .subscribe({

        next: (data) => {

          this.enrollments = data;

          this.filteredEnrollments = [...data];

          this.isLoading = false;
        },

        error: (error) => {

          console.error(
            'Error loading enrollments:',
            error
          );

          this.isLoading = false;

          this.showError(
            error,
            'Failed to load student enrollments.'
          );
        }

      });
  }


  // =====================================================
  // LOAD STUDENTS
  // Backend:
  // GET api/student-enrollments/students
  // =====================================================

  loadStudents(): void {

    this.isLoadingStudents = true;

    this.enrollmentService
      .getStudents()
      .subscribe({

        next: (data) => {

          this.students = data;

          this.isLoadingStudents = false;
        },

        error: (error) => {

          console.error(
            'Error loading students:',
            error
          );

          this.isLoadingStudents = false;

          this.showError(
            error,
            'Failed to load students.'
          );
        }

      });
  }


  // =====================================================
  // LOAD CLASSES
  // Backend:
  // GET api/student-enrollments/classes
  // =====================================================

  loadClasses(): void {

    this.isLoadingClasses = true;

    this.enrollmentService
      .getClasses()
      .subscribe({

        next: (data) => {

          this.classes = data;

          this.isLoadingClasses = false;
        },

        error: (error) => {

          console.error(
            'Error loading classes:',
            error
          );

          this.isLoadingClasses = false;

          this.showError(
            error,
            'Failed to load classes.'
          );
        }

      });
  }


  // =====================================================
  // CLASS CHANGE
  // =====================================================

  onClassChange(): void {

    const classId = this.formData.classId;

    // Clear old course and section
    this.formData.courseId = '';

    this.formData.sectionId = null;

    this.courses = [];

    this.sections = [];


    // No class selected
    if (!classId) {

      return;
    }


    // Load courses for selected class
    this.loadCoursesByClass(classId);

    // Load sections for selected class
    this.loadSectionsByClass(classId);
  }


  // =====================================================
  // LOAD COURSES BY CLASS
  // Backend:
  // GET api/student-enrollments/classes/{classId}/courses
  // =====================================================

  loadCoursesByClass(classId: string): void {

    this.isLoadingCourses = true;

    this.enrollmentService
      .getCoursesByClassId(classId)
      .subscribe({

        next: (data) => {

          this.courses = data;

          this.isLoadingCourses = false;
        },

        error: (error) => {

          console.error(
            'Error loading courses:',
            error
          );

          this.courses = [];

          this.isLoadingCourses = false;

          this.showError(
            error,
            'Failed to load courses.'
          );
        }

      });
  }


  // =====================================================
  // LOAD SECTIONS BY CLASS
  // Backend:
  // GET api/student-enrollments/classes/{classId}/sections
  // =====================================================

  loadSectionsByClass(classId: string): void {

    this.isLoadingSections = true;

    this.enrollmentService
      .getSectionsByClassId(classId)
      .subscribe({

        next: (data) => {

          this.sections = data;

          this.isLoadingSections = false;
        },

        error: (error) => {

          console.error(
            'Error loading sections:',
            error
          );

          this.sections = [];

          this.isLoadingSections = false;

          this.showError(
            error,
            'Failed to load sections.'
          );
        }

      });
  }


  // =====================================================
  // SEARCH
  // =====================================================

  search(): void {

    const search =
      this.searchText
        .trim()
        .toLowerCase();


    if (!search) {

      this.filteredEnrollments =
        [...this.enrollments];

      return;
    }


    this.filteredEnrollments =
      this.enrollments.filter(x =>

        (x.studentName || '')
          .toLowerCase()
          .includes(search)

        ||

        (x.studentId || '')
          .toLowerCase()
          .includes(search)

        ||

        (x.className || '')
          .toLowerCase()
          .includes(search)

        ||

        (x.courseName || '')
          .toLowerCase()
          .includes(search)

        ||

        (x.sectionName || '')
          .toLowerCase()
          .includes(search)

        ||

        (x.rollNumber || '')
          .toLowerCase()
          .includes(search)

        ||

        (x.status || '')
          .toLowerCase()
          .includes(search)

      );
  }


  // =====================================================
  // GET INITIALS
  // =====================================================

  getInitials(
    name: string | null | undefined
  ): string {

    if (!name) {
      return 'NA';
    }


    const parts =
      name
        .trim()
        .split(/\s+/)
        .filter(Boolean);


    if (parts.length === 1) {

      return parts[0]
        .substring(0, 2)
        .toUpperCase();
    }


    return (
      parts[0][0] +
      parts[parts.length - 1][0]
    ).toUpperCase();
  }


  // =====================================================
  // OPEN CREATE
  // =====================================================

  openCreate(): void {

    this.isEditMode = false;

    this.selectedId = null;


    this.formData = {

      studentId: '',

      classId: '',

      courseId: '',

      sectionId: null,

      rollNumber: '',

      enrollmentDate: this.getToday(),

      status: 'Active'
    };


    // Clear dependent dropdowns
    this.courses = [];

    this.sections = [];


    this.isModalOpen = true;
  }


  // =====================================================
  // OPEN EDIT
  // =====================================================

  openEdit(
    enrollment: StudentEnrollment
  ): void {

    this.isEditMode = true;

    this.selectedId = enrollment.id;


    this.formData = {

      studentId: enrollment.studentId,

      classId: enrollment.classId,

      courseId: enrollment.courseId,

      sectionId: enrollment.sectionId,

      rollNumber: enrollment.rollNumber || '',

      enrollmentDate:
        this.formatDateForInput(
          enrollment.enrollmentDate
        ),

      status: enrollment.status
    };


    this.isModalOpen = true;


    // Important:
    // When editing, load courses and sections
    // for the existing class.
    if (enrollment.classId) {

      this.loadCoursesByClass(
        enrollment.classId
      );

      this.loadSectionsByClass(
        enrollment.classId
      );
    }
  }


  // =====================================================
  // SAVE
  // =====================================================

  save(): void {

    if (!this.validateForm()) {

      return;
    }


    this.isSaving = true;


    // ===================================================
    // UPDATE
    // ===================================================

    if (
      this.isEditMode &&
      this.selectedId
    ) {

      const updateData:
        UpdateStudentEnrollment = {

        classId:
          this.formData.classId,

        courseId:
          this.formData.courseId,

        sectionId:
          this.formData.sectionId,

        rollNumber:
          this.formData.rollNumber,

        status:
          this.formData.status
      };


      this.enrollmentService
        .update(
          this.selectedId,
          updateData
        )
        .subscribe({

          next: () => {

            this.isSaving = false;

            this.closeModal();

            this.loadEnrollments();
          },

          error: (error) => {

            this.isSaving = false;

            console.error(
              'Error updating enrollment:',
              error
            );

            this.showError(
              error,
              'Failed to update enrollment.'
            );
          }

        });


      return;
    }


    // ===================================================
    // CREATE
    // ===================================================

    const createData:
      CreateStudentEnrollment = {

      studentId:
        this.formData.studentId,

      classId:
        this.formData.classId,

      courseId:
        this.formData.courseId,

      sectionId:
        this.formData.sectionId,

      rollNumber:
        this.formData.rollNumber,

      enrollmentDate:
        this.formData.enrollmentDate,

      status:
        this.formData.status
    };


    this.enrollmentService
      .create(createData)
      .subscribe({

        next: () => {

          this.isSaving = false;

          this.closeModal();

          this.loadEnrollments();
        },

        error: (error) => {

          this.isSaving = false;

          console.error(
            'Error creating enrollment:',
            error
          );

          this.showError(
            error,
            'Failed to create enrollment.'
          );
        }

      });
  }


  // =====================================================
  // DELETE
  // =====================================================

  delete(id: string): void {

    const confirmed =
      window.confirm(
        'Are you sure you want to delete this enrollment?'
      );


    if (!confirmed) {
      return;
    }


    this.enrollmentService
      .delete(id)
      .subscribe({

        next: () => {

          this.loadEnrollments();
        },

        error: (error) => {

          console.error(
            'Error deleting enrollment:',
            error
          );

          this.showError(
            error,
            'Failed to delete enrollment.'
          );
        }

      });
  }


  // =====================================================
  // CLOSE MODAL
  // =====================================================

  closeModal(): void {

    this.isModalOpen = false;

    this.isEditMode = false;

    this.selectedId = null;

    this.isSaving = false;

    this.courses = [];

    this.sections = [];
  }

  onBackdropClick(event: MouseEvent): void {

  if (event.target === event.currentTarget) {

    this.closeModal();

  }

}


  // =====================================================
  // VALIDATION
  // =====================================================

  private validateForm(): boolean {

    if (!this.formData.studentId) {

      alert(
        'Please select a student.'
      );

      return false;
    }


    if (!this.formData.classId) {

      alert(
        'Please select a class.'
      );

      return false;
    }


    if (!this.formData.courseId) {

      alert(
        'Please select a course.'
      );

      return false;
    }


    if (!this.formData.enrollmentDate) {

      alert(
        'Please select enrollment date.'
      );

      return false;
    }


    if (!this.formData.status) {

      alert(
        'Please select status.'
      );

      return false;
    }


    return true;
  }


  // =====================================================
  // ERROR
  // =====================================================

  private showError(
    error: any,
    defaultMessage: string
  ): void {

    const message =
      error?.error?.message ||
      error?.error?.title ||
      defaultMessage;


    alert(message);
  }


  // =====================================================
  // TODAY
  // =====================================================

  private getToday(): string {

    const date = new Date();


    const year =
      date.getFullYear();


    const month =
      String(
        date.getMonth() + 1
      ).padStart(2, '0');


    const day =
      String(
        date.getDate()
      ).padStart(2, '0');


    return `${year}-${month}-${day}`;
  }


  // =====================================================
  // DATE FORMAT
  // =====================================================

  private formatDateForInput(
    value: string
  ): string {

    if (!value) {

      return this.getToday();
    }


    return value.substring(
      0,
      10
    );
  }

}