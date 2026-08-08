import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';

import {
  TeacherAssignmentService
} from '../../../../core/services/teacher-assignment.service';

import {
  TeacherAssignment,
  TeacherDropdown,
  CreateTeacherAssignment,
  UpdateTeacherAssignment
} from '../../../../core/models/teacher-assignment.model';

import { AcademicYear } from '../../../../core/models/academic-year.model';
import { ClassModel } from '../../../../core/models/class.model';
import { SectionModel } from '../../../../core/models/section.model';
import { CourseModel } from '../../../../core/models/course.model';
import { SubjectModel } from '../../../../core/models/subject.model';


@Component({
  selector: 'app-assign-teacher',
  standalone: true,

  imports: [
    CommonModule,
    FormsModule
  ],

  templateUrl: './assign-teacher.component.html',
  styleUrls: ['./assign-teacher.component.scss']
})
export class AssignTeacherComponent implements OnInit {

  // =====================================================
  // DATA
  // =====================================================

  assignments: TeacherAssignment[] = [];

  teachers: TeacherDropdown[] = [];

  academicYears: AcademicYear[] = [];

  classes: ClassModel[] = [];

  sections: SectionModel[] = [];

  courses: CourseModel[] = [];

  subjects: SubjectModel[] = [];


  // =====================================================
  // FILTERED DROPDOWNS
  // =====================================================

  filteredSections: SectionModel[] = [];

  filteredCourses: CourseModel[] = [];

  filteredSubjects: SubjectModel[] = [];


  // =====================================================
  // SEARCH
  // =====================================================

  searchTerm = '';


  // =====================================================
  // LOADING
  // =====================================================

  loading = false;

  saving = false;

  deleting = false;


  // =====================================================
  // MESSAGES
  // =====================================================

  successMessage = '';

  errorMessage = '';


  // =====================================================
  // MODAL
  // =====================================================

  showModal = false;

  isEditMode = false;

  selectedAssignmentId: string | null = null;


  // =====================================================
  // FORM
  // =====================================================

  form: CreateTeacherAssignment = {
    teacherId: '',
    academicYearId: '',
    classId: '',
    sectionId: null,
    courseId: '',
    subjectId: '',
    isPrimary: true,
    isActive: true,
    createdBy: null
  };


  // =====================================================
  // CONSTRUCTOR
  // =====================================================

  constructor(
    private teacherAssignmentService: TeacherAssignmentService
  ) {}


  // =====================================================
  // INIT
  // =====================================================

  ngOnInit(): void {
    this.loadAllData();
  }


  // =====================================================
  // LOAD ALL DATA
  // =====================================================

  loadAllData(): void {

    this.loading = true;

    this.clearMessages();

    this.loadAssignments();

    this.loadTeachers();

    this.loadAcademicYears();

    this.loadClasses();

    this.loadSections();

    this.loadCourses();

    this.loadSubjects();
  }


  // =====================================================
  // LOAD ASSIGNMENTS
  // =====================================================

  loadAssignments(): void {

    this.teacherAssignmentService
      .getAll()
      .subscribe({

        next: (data) => {

          this.assignments = data;

          this.loading = false;
        },

        error: (error) => {

          console.error(
            'Failed to load teacher assignments:',
            error
          );

          this.errorMessage =
            error?.error?.message ||
            'Failed to load teacher assignments.';

          this.loading = false;
        }

      });
  }


  // =====================================================
  // LOAD TEACHERS
  // =====================================================

  loadTeachers(): void {

    this.teacherAssignmentService
      .getTeachers()
      .subscribe({

        next: (data) => {

          this.teachers = data;
        },

        error: (error) => {

          console.error(
            'Failed to load teachers:',
            error
          );

        }

      });
  }


  // =====================================================
  // LOAD ACADEMIC YEARS
  // =====================================================

  loadAcademicYears(): void {

    this.teacherAssignmentService
      .getAcademicYears()
      .subscribe({

        next: (data) => {

          this.academicYears = data;
        },

        error: (error) => {

          console.error(
            'Failed to load academic years:',
            error
          );

        }

      });
  }


  // =====================================================
  // LOAD CLASSES
  // =====================================================

  loadClasses(): void {

    this.teacherAssignmentService
      .getClasses()
      .subscribe({

        next: (data) => {

          this.classes = data;
        },

        error: (error) => {

          console.error(
            'Failed to load classes:',
            error
          );

        }

      });
  }


  // =====================================================
  // LOAD SECTIONS
  // =====================================================

  loadSections(): void {

    this.teacherAssignmentService
      .getSections()
      .subscribe({

        next: (data) => {

          this.sections = data;
        },

        error: (error) => {

          console.error(
            'Failed to load sections:',
            error
          );

        }

      });
  }


  // =====================================================
  // LOAD COURSES
  // =====================================================

  loadCourses(): void {

    this.teacherAssignmentService
      .getCourses()
      .subscribe({

        next: (data) => {

          this.courses = data;
        },

        error: (error) => {

          console.error(
            'Failed to load courses:',
            error
          );

        }

      });
  }


  // =====================================================
  // LOAD SUBJECTS
  // =====================================================

  loadSubjects(): void {

    this.teacherAssignmentService
      .getSubjects()
      .subscribe({

        next: (data) => {

          this.subjects = data;
        },

        error: (error) => {

          console.error(
            'Failed to load subjects:',
            error
          );

        }

      });
  }


  // =====================================================
  // SEARCH
  // =====================================================

  get filteredAssignments(): TeacherAssignment[] {

    const search =
      this.searchTerm
        .trim()
        .toLowerCase();

    if (!search) {
      return this.assignments;
    }

    return this.assignments.filter(x =>

      (x.teacherName || '')
        .toLowerCase()
        .includes(search)

      ||

      (x.teacherEmail || '')
        .toLowerCase()
        .includes(search)

      ||

      (x.academicYearName || '')
        .toLowerCase()
        .includes(search)

      ||

      (x.className || '')
        .toLowerCase()
        .includes(search)

      ||

      (x.sectionName || '')
        .toLowerCase()
        .includes(search)

      ||

      (x.courseName || '')
        .toLowerCase()
        .includes(search)

      ||

      (x.subjectName || '')
        .toLowerCase()
        .includes(search)

    );
  }


  // =====================================================
  // OPEN CREATE
  // =====================================================

  openCreate(): void {

    this.isEditMode = false;

    this.selectedAssignmentId = null;

    this.resetForm();

    this.showModal = true;

    this.clearMessages();
  }


  // =====================================================
  // OPEN EDIT
  // =====================================================

  openEdit(
    assignment: TeacherAssignment
  ): void {

    this.isEditMode = true;

    this.selectedAssignmentId =
      assignment.id;


    this.form = {

      teacherId:
        assignment.teacherId,

      academicYearId:
        assignment.academicYearId,

      classId:
        assignment.classId,

      sectionId:
        assignment.sectionId,

      courseId:
        assignment.courseId,

      subjectId:
        assignment.subjectId,

      isPrimary:
        assignment.isPrimary,

      isActive:
        assignment.isActive,

      createdBy:
        assignment.createdBy
    };


    // Populate dependent dropdowns

    this.onClassChange();

    this.onCourseChange();


    this.showModal = true;

    this.clearMessages();
  }


  // =====================================================
  // CLOSE MODAL
  // =====================================================

  closeModal(): void {

    if (this.saving) {
      return;
    }

    this.showModal = false;

    this.selectedAssignmentId = null;

    this.resetForm();

    this.clearMessages();
  }


  // =====================================================
  // RESET FORM
  // =====================================================

  resetForm(): void {

    this.form = {

      teacherId: '',

      academicYearId: '',

      classId: '',

      sectionId: null,

      courseId: '',

      subjectId: '',

      isPrimary: true,

      isActive: true,

      createdBy: null
    };


    this.filteredSections = [];

    this.filteredCourses = [];

    this.filteredSubjects = [];
  }


  // =====================================================
  // CLASS CHANGE
  // =====================================================

  onClassChange(): void {

    const classId =
      this.form.classId;


    // Reset dependent values

    this.form.sectionId = null;

    this.form.courseId = '';

    this.form.subjectId = '';

    this.filteredSubjects = [];


    if (!classId) {

      this.filteredSections = [];

      this.filteredCourses = [];

      return;
    }


    // -------------------------------------------------
    // Filter Sections by Class
    // -------------------------------------------------

    this.filteredSections =
      this.sections.filter(
        section =>
          section.classId === classId
      );


    // -------------------------------------------------
    // Filter Courses by Class
    // -------------------------------------------------

    this.filteredCourses =
      this.courses.filter(
        course =>
          course.classId === classId
      );
  }


  // =====================================================
  // COURSE CHANGE
  // =====================================================

  onCourseChange(): void {

    const courseId =
      this.form.courseId;


    this.form.subjectId = '';


    if (!courseId) {

      this.filteredSubjects = [];

      return;
    }


    /*
     * Current Subject API returns all subjects.
     *
     * For now we use the CourseSubjects relationship
     * only if CourseModel contains Subject information.
     *
     * If your Course model does not contain subjects,
     * the next backend endpoint should be:
     *
     * GET /api/courses/{courseId}/subjects
     *
     * Then we can load only subjects belonging to
     * this course.
     */


    this.filteredSubjects =
      this.subjects;
  }


  // =====================================================
  // SAVE
  // =====================================================

  save(): void {

    this.clearMessages();


    // -------------------------------------------------
    // Validation
    // -------------------------------------------------

    if (!this.form.teacherId) {

      this.errorMessage =
        'Please select a teacher.';

      return;
    }


    if (!this.form.academicYearId) {

      this.errorMessage =
        'Please select an academic year.';

      return;
    }


    if (!this.form.classId) {

      this.errorMessage =
        'Please select a class.';

      return;
    }


    if (!this.form.courseId) {

      this.errorMessage =
        'Please select a course.';

      return;
    }


    if (!this.form.subjectId) {

      this.errorMessage =
        'Please select a subject.';

      return;
    }


    this.saving = true;


    // =================================================
    // CREATE
    // =================================================

    if (!this.isEditMode) {

      const data: CreateTeacherAssignment = {

        teacherId:
          this.form.teacherId,

        academicYearId:
          this.form.academicYearId,

        classId:
          this.form.classId,

        sectionId:
          this.form.sectionId,

        courseId:
          this.form.courseId,

        subjectId:
          this.form.subjectId,

        isPrimary:
          this.form.isPrimary,

        isActive:
          this.form.isActive,

        createdBy:
          this.form.createdBy
      };


      this.teacherAssignmentService
        .create(data)
        .subscribe({

          next: () => {

            this.saving = false;

            this.successMessage =
              'Teacher assignment created successfully.';

            this.showModal = false;

            this.resetForm();

            this.loadAssignments();
          },

          error: (error) => {

            this.saving = false;

            this.errorMessage =
              error?.error?.message ||
              'Failed to create teacher assignment.';
          }

        });


      return;
    }


    // =================================================
    // UPDATE
    // =================================================

    if (!this.selectedAssignmentId) {

      this.saving = false;

      this.errorMessage =
        'Assignment ID is missing.';

      return;
    }


    const updateData: UpdateTeacherAssignment = {

      teacherId:
        this.form.teacherId,

      academicYearId:
        this.form.academicYearId,

      classId:
        this.form.classId,

      sectionId:
        this.form.sectionId,

      courseId:
        this.form.courseId,

      subjectId:
        this.form.subjectId,

      isPrimary:
        this.form.isPrimary,

      isActive:
        this.form.isActive,

      updatedBy:
        null
    };


    this.teacherAssignmentService
      .update(
        this.selectedAssignmentId,
        updateData
      )
      .subscribe({

        next: () => {

          this.saving = false;

          this.successMessage =
            'Teacher assignment updated successfully.';

          this.showModal = false;

          this.resetForm();

          this.loadAssignments();
        },

        error: (error) => {

          this.saving = false;

          this.errorMessage =
            error?.error?.message ||
            'Failed to update teacher assignment.';
        }

      });
  }


  // =====================================================
  // DELETE
  // =====================================================

  deleteAssignment(
    assignment: TeacherAssignment
  ): void {

    if (this.deleting) {
      return;
    }


    const teacherName =
      assignment.teacherName ||
      'this teacher';


    const subjectName =
      assignment.subjectName ||
      'this subject';


    const confirmed =
      window.confirm(
        `Are you sure you want to delete the assignment of ${teacherName} for ${subjectName}?`
      );


    if (!confirmed) {
      return;
    }


    this.deleting = true;

    this.clearMessages();


    this.teacherAssignmentService
      .delete(assignment.id)
      .subscribe({

        next: () => {

          this.deleting = false;

          this.successMessage =
            'Teacher assignment deleted successfully.';

          this.loadAssignments();
        },

        error: (error) => {

          this.deleting = false;

          this.errorMessage =
            error?.error?.message ||
            'Failed to delete teacher assignment.';
        }

      });
  }


  // =====================================================
  // CLEAR MESSAGES
  // =====================================================

  clearMessages(): void {

    this.successMessage = '';

    this.errorMessage = '';
  }


  // =====================================================
  // TRACK BY
  // =====================================================

  trackById(
    index: number,
    item: TeacherAssignment
  ): string {

    return item.id;
  }
}