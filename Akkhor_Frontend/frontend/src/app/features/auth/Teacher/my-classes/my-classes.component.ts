import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';

import { TeacherClassService } from '../../../../core/services/teacher-class.service';
import { TeacherClass } from '../../../../core/models/teacher-class.model';

@Component({
  selector: 'app-my-classes',
  standalone: true,

  imports: [
    CommonModule,
    FormsModule
  ],

  templateUrl: './my-classes.component.html',
  styleUrls: ['./my-classes.component.scss']
})
export class MyClassesComponent implements OnInit {

  // =====================================================
  // DATA
  // =====================================================

  classes: TeacherClass[] = [];

  // =====================================================
  // SEARCH
  // =====================================================

  searchTerm = '';

  // =====================================================
  // FILTER
  // =====================================================

  selectedAcademicYear = '';

  selectedClass = '';

  selectedSection = '';

  // =====================================================
  // LOADING
  // =====================================================

  loading = false;

  // =====================================================
  // MESSAGES
  // =====================================================

  errorMessage = '';

  // =====================================================
  // CONSTRUCTOR
  // =====================================================

  constructor(
    private teacherClassService: TeacherClassService
  ) {}

  // =====================================================
  // INIT
  // =====================================================

  ngOnInit(): void {
    this.loadMyClasses();
  }

  // =====================================================
  // LOAD MY CLASSES
  // =====================================================

  loadMyClasses(): void {

    this.loading = true;

    this.errorMessage = '';

    this.teacherClassService
      .getMyClasses()
      .subscribe({

        next: (data) => {

          this.classes = data || [];

          this.loading = false;
        },

        error: (error) => {

          console.error(
            'Failed to load teacher classes:',
            error
          );

          this.errorMessage =
            error?.error?.message ||
            'Failed to load your classes.';

          this.loading = false;
        }

      });
  }

  // =====================================================
  // FILTERED CLASSES
  // =====================================================

  get filteredClasses(): TeacherClass[] {

    const search =
      this.searchTerm
        .trim()
        .toLowerCase();

    return this.classes.filter(item => {

      // -------------------------------------------------
      // Search
      // -------------------------------------------------

      const matchesSearch =
        !search ||

        (item.academicYearName || '')
          .toLowerCase()
          .includes(search) ||

        (item.className || '')
          .toLowerCase()
          .includes(search) ||

        (item.sectionName || '')
          .toLowerCase()
          .includes(search) ||

        (item.roomNumber || '')
          .toLowerCase()
          .includes(search) ||

        (item.courseName || '')
          .toLowerCase()
          .includes(search) ||

        (item.subjectName || '')
          .toLowerCase()
          .includes(search);


      // -------------------------------------------------
      // Academic Year
      // -------------------------------------------------

      const matchesAcademicYear =
        !this.selectedAcademicYear ||
        item.academicYearId ===
          this.selectedAcademicYear;


      // -------------------------------------------------
      // Class
      // -------------------------------------------------

      const matchesClass =
        !this.selectedClass ||
        item.classId ===
          this.selectedClass;


      // -------------------------------------------------
      // Section
      // -------------------------------------------------

      const matchesSection =
        !this.selectedSection ||
        item.sectionId ===
          this.selectedSection;


      return (
        matchesSearch &&
        matchesAcademicYear &&
        matchesClass &&
        matchesSection
      );
    });
  }

  // =====================================================
  // ACADEMIC YEARS
  // =====================================================

  get academicYears(): TeacherClass[] {

    return this.classes.filter(
      (item, index, array) =>
        array.findIndex(
          x =>
            x.academicYearId ===
            item.academicYearId
        ) === index
    );
  }

  // =====================================================
  // CLASSES
  // =====================================================

  get availableClasses(): TeacherClass[] {

    let data = this.classes;

    if (this.selectedAcademicYear) {

      data = data.filter(
        item =>
          item.academicYearId ===
          this.selectedAcademicYear
      );
    }

    return data.filter(
      (item, index, array) =>
        array.findIndex(
          x =>
            x.classId ===
            item.classId
        ) === index
    );
  }

  // =====================================================
  // SECTIONS
  // =====================================================

  get availableSections(): TeacherClass[] {

    let data = this.classes;

    if (this.selectedAcademicYear) {

      data = data.filter(
        item =>
          item.academicYearId ===
          this.selectedAcademicYear
      );
    }

    if (this.selectedClass) {

      data = data.filter(
        item =>
          item.classId ===
          this.selectedClass
      );
    }

    return data.filter(
      (item, index, array) =>
        array.findIndex(
          x =>
            x.sectionId ===
            item.sectionId
        ) === index
    );
  }

  // =====================================================
  // ACADEMIC YEAR CHANGE
  // =====================================================

  onAcademicYearChange(): void {

    this.selectedClass = '';

    this.selectedSection = '';
  }

  // =====================================================
  // CLASS CHANGE
  // =====================================================

  onClassChange(): void {

    this.selectedSection = '';
  }

  // =====================================================
  // CLEAR FILTERS
  // =====================================================

  clearFilters(): void {

    this.searchTerm = '';

    this.selectedAcademicYear = '';

    this.selectedClass = '';

    this.selectedSection = '';
  }

  // =====================================================
  // RECORD COUNT
  // =====================================================

  get recordCount(): number {

    return this.filteredClasses.length;
  }

  // =====================================================
  // PRIMARY COUNT
  // =====================================================

  get primaryCount(): number {

    return this.filteredClasses.filter(
      item => item.isPrimary
    ).length;
  }

  // =====================================================
  // ACTIVE COUNT
  // =====================================================

  get activeCount(): number {

    return this.filteredClasses.filter(
      item => item.isActive
    ).length;
  }

  // =====================================================
  // TRACK BY
  // =====================================================

  trackByAssignmentId(
    index: number,
    item: TeacherClass
  ): string {

    return item.assignmentId;
  }
}