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
  Router
} from '@angular/router';

import {
  Assignment
} from '../../../../core/models/assignment.model';

import {
  AdminAssignmentService
} from '../../../../core/services/admin-assignment.service';


@Component({
  selector: 'app-admin-assignments',

  standalone: true,

  imports: [
    CommonModule,
    FormsModule
  ],

  templateUrl:
    './admin-assignments.component.html',

  styleUrls:
    ['./admin-assignments.component.scss']
})
export class AdminAssignmentsComponent
  implements OnInit {

  assignments: Assignment[] = [];

  filteredAssignments: Assignment[] = [];

  searchTerm = '';

  selectedStatus = 'all';

  loading = false;

  errorMessage = '';


  constructor(
    private assignmentService:
      AdminAssignmentService,

    private router: Router
  ) {}


  // =====================================================
  // INIT
  // =====================================================

  ngOnInit(): void {
    this.loadAssignments();
  }


  // =====================================================
  // LOAD ASSIGNMENTS
  // =====================================================

  loadAssignments(): void {

    this.loading = true;

    this.errorMessage = '';

    this.assignmentService
      .getAll()
      .subscribe({

        next: (data) => {

          this.assignments =
            data ?? [];

          this.filteredAssignments =
            [...this.assignments];

          this.loading = false;

          this.applyFilters();
        },

        error: (error) => {

          console.error(
            'Failed to load assignments',
            error
          );

          this.errorMessage =
            error?.error?.message ??
            'Failed to load assignments.';

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

    this.filteredAssignments =
      this.assignments.filter(
        assignment => {

          const matchesSearch =
            !search ||

            assignment.title
              ?.toLowerCase()
              .includes(search) ||

            assignment.teacherName
              ?.toLowerCase()
              .includes(search) ||

            assignment.className
              ?.toLowerCase()
              .includes(search) ||

            assignment.courseName
              ?.toLowerCase()
              .includes(search) ||

            assignment.subjectName
              ?.toLowerCase()
              .includes(search);


          const matchesStatus =
            this.selectedStatus === 'all' ||

            (
              this.selectedStatus === 'published' &&
              assignment.isPublished
            ) ||

            (
              this.selectedStatus === 'draft' &&
              !assignment.isPublished
            );


          return (
            matchesSearch &&
            matchesStatus
          );
        }
      );
  }


  // =====================================================
  // SEARCH
  // =====================================================

  onSearch(): void {
    this.applyFilters();
  }


  // =====================================================
  // STATUS FILTER
  // =====================================================

  onStatusChange(): void {
    this.applyFilters();
  }


  // =====================================================
  // VIEW DETAILS
  // =====================================================

  viewAssignment(
    assignment: Assignment
  ): void {

    this.router.navigate([
      '/admin/assignments',
      assignment.id
    ]);
  }


  // =====================================================
  // DOWNLOAD
  // =====================================================

  downloadAttachment(
    assignment: Assignment
  ): void {

    if (!assignment.attachmentUrl) {
      return;
    }

    window.open(
      assignment.attachmentUrl,
      '_blank'
    );
  }


  // =====================================================
  // CLEAR FILTER
  // =====================================================

  clearFilters(): void {

    this.searchTerm = '';

    this.selectedStatus = 'all';

    this.applyFilters();
  }


  // =====================================================
  // COUNTS
  // =====================================================

  get totalAssignments(): number {
    return this.assignments.length;
  }


  get publishedCount(): number {

    return this.assignments
      .filter(x => x.isPublished)
      .length;
  }


  get draftCount(): number {

    return this.assignments
      .filter(x => !x.isPublished)
      .length;
  }


  get activeCount(): number {

    return this.assignments
      .filter(x => x.isActive)
      .length;
  }
}