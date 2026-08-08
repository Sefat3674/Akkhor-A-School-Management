import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';

import {
  Assignment
} from '../../../../../core/models/assignment.model';

import {
  AssignmentService
} from '../../../../../core/services/assignment.service';

@Component({
  selector: 'app-assignment-list',
  standalone: true,

  imports: [
    CommonModule,
    FormsModule
  ],

  templateUrl: './assignment-list.component.html',
  styleUrls: ['./assignment-list.component.scss']
})
export class AssignmentListComponent implements OnInit {

  // =====================================================
  // DATA
  // =====================================================

  assignments: Assignment[] = [];

  filteredAssignments: Assignment[] = [];


  // =====================================================
  // UI STATE
  // =====================================================

  loading = false;

  errorMessage = '';

  searchTerm = '';

  statusFilter = 'all';


  // =====================================================
  // CONSTRUCTOR
  // =====================================================

  constructor(
    private assignmentService: AssignmentService,
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

        next: (data: Assignment[]) => {

          this.assignments = data ?? [];

          this.applyFilters();

          this.loading = false;
        },

        error: (error) => {

          console.error(
            'Error loading assignments:',
            error
          );

          this.errorMessage =
            'Failed to load assignments.';

          this.loading = false;
        }

      });
  }


  // =====================================================
  // SEARCH
  // =====================================================

  onSearch(): void {

    this.applyFilters();
  }


  // =====================================================
  // FILTER
  // =====================================================

  onStatusFilterChange(): void {

    this.applyFilters();
  }


  // =====================================================
  // APPLY FILTERS
  // =====================================================

  applyFilters(): void {

    const search =
      this.searchTerm
        .trim()
        .toLowerCase();


    this.filteredAssignments =
      this.assignments.filter(
        assignment => {

          // ---------------------------------------------
          // Search
          // ---------------------------------------------

          const matchesSearch =
            !search ||

            assignment.title
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


          // ---------------------------------------------
          // Status
          // ---------------------------------------------

          let matchesStatus = true;


          if (this.statusFilter === 'published') {

            matchesStatus =
              assignment.isPublished === true;
          }


          if (this.statusFilter === 'draft') {

            matchesStatus =
              assignment.isPublished === false;
          }


          if (this.statusFilter === 'active') {

            matchesStatus =
              assignment.isActive === true;
          }


          if (this.statusFilter === 'inactive') {

            matchesStatus =
              assignment.isActive === false;
          }


          return (
            matchesSearch &&
            matchesStatus
          );
        }
      );
  }


  // =====================================================
  // CREATE
  // =====================================================

  openCreate(): void {

    this.router.navigate([
      '/teacher/assignments/create'
    ]);
  }


  // =====================================================
  // EDIT
  // =====================================================

  openEdit(
    assignment: Assignment
  ): void {

    this.router.navigate([
      '/teacher/assignments/edit',
      assignment.id
    ]);
  }


  // =====================================================
  // VIEW
  // =====================================================

  openView(
    assignment: Assignment
  ): void {

    this.router.navigate([
      '/teacher/assignments/view',
      assignment.id
    ]);
  }


  // =====================================================
  // REVIEW SUBMISSIONS
  // =====================================================

  reviewAssignment(
    assignment: Assignment
  ): void {

    this.router.navigate([
      '/teacher/assignments/review',
      assignment.id
    ]);
  }


  // =====================================================
  // PUBLISH
  // =====================================================

  publish(
    assignment: Assignment
  ): void {

    if (!assignment.id) {
      return;
    }


    const confirmed =
      confirm(
        `Publish "${assignment.title}"?`
      );


    if (!confirmed) {
      return;
    }


    this.assignmentService
      .publish(assignment.id)
      .subscribe({

        next: (updated) => {

          const index =
            this.assignments.findIndex(
              x => x.id === updated.id
            );


          if (index !== -1) {

            this.assignments[index] =
              updated;
          }


          this.applyFilters();
        },

        error: (error) => {

          console.error(
            'Error publishing assignment:',
            error
          );

          alert(
            'Failed to publish assignment.'
          );
        }

      });
  }


  // =====================================================
  // UNPUBLISH
  // =====================================================

  unpublish(
    assignment: Assignment
  ): void {

    if (!assignment.id) {
      return;
    }


    const confirmed =
      confirm(
        `Move "${assignment.title}" back to draft?`
      );


    if (!confirmed) {
      return;
    }


    this.assignmentService
      .unpublish(assignment.id)
      .subscribe({

        next: (updated) => {

          const index =
            this.assignments.findIndex(
              x => x.id === updated.id
            );


          if (index !== -1) {

            this.assignments[index] =
              updated;
          }


          this.applyFilters();
        },

        error: (error) => {

          console.error(
            'Error unpublishing assignment:',
            error
          );

          alert(
            'Failed to move assignment to draft.'
          );
        }

      });
  }


  // =====================================================
  // DELETE
  // =====================================================

  deleteAssignment(
    assignment: Assignment
  ): void {

    if (!assignment.id) {
      return;
    }


    const confirmed =
      confirm(
        `Are you sure you want to delete "${assignment.title}"?`
      );


    if (!confirmed) {
      return;
    }


    this.assignmentService
      .delete(assignment.id)
      .subscribe({

        next: () => {

          this.assignments =
            this.assignments.filter(
              x => x.id !== assignment.id
            );


          this.applyFilters();
        },

        error: (error) => {

          console.error(
            'Error deleting assignment:',
            error
          );

          alert(
            'Failed to delete assignment.'
          );
        }

      });
  }


  // =====================================================
  // TOGGLE ACTIVE
  // =====================================================

  // =====================================================
// TOGGLE ACTIVE
// =====================================================

toggleActive(
  assignment: Assignment
): void {

  if (!assignment.id) {
    return;
  }


  // ===================================================
  // CREATE FORM DATA
  // ===================================================

  const formData = new FormData();


  // ===================================================
  // FOREIGN KEYS
  // ===================================================

  formData.append(
    'academicYearId',
    assignment.academicYearId
  );

  formData.append(
    'classId',
    assignment.classId
  );

  if (assignment.sectionId) {

    formData.append(
      'sectionId',
      assignment.sectionId
    );

  }

  formData.append(
    'courseId',
    assignment.courseId
  );

  formData.append(
    'subjectId',
    assignment.subjectId
  );


  // ===================================================
  // ASSIGNMENT INFORMATION
  // ===================================================

  formData.append(
    'title',
    assignment.title
  );

  formData.append(
    'description',
    assignment.description ?? ''
  );

  formData.append(
    'deadline',
    assignment.deadline
  );

  formData.append(
    'maximumMarks',
    assignment.maximumMarks.toString()
  );


  // ===================================================
  // ATTACHMENT
  // ===================================================

  if (assignment.attachmentUrl) {

    formData.append(
      'attachmentUrl',
      assignment.attachmentUrl
    );

  }

  if (assignment.attachmentFileName) {

    formData.append(
      'attachmentFileName',
      assignment.attachmentFileName
    );

  }

  if (assignment.attachmentContentType) {

    formData.append(
      'attachmentContentType',
      assignment.attachmentContentType
    );

  }

  if (
    assignment.attachmentFileSize !== null &&
    assignment.attachmentFileSize !== undefined
  ) {

    formData.append(
      'attachmentFileSize',
      assignment.attachmentFileSize.toString()
    );

  }


  // ===================================================
  // PUBLICATION
  // ===================================================

  formData.append(
    'isPublished',
    assignment.isPublished.toString()
  );


  // ===================================================
  // ACTIVE STATUS
  // ===================================================

  formData.append(
    'isActive',
    (!assignment.isActive).toString()
  );


  // ===================================================
  // UPDATE
  // ===================================================

  this.assignmentService
    .update(
      assignment.id,
      formData
    )
    .subscribe({

      next: (updated: Assignment) => {

        const index =
          this.assignments.findIndex(
            x => x.id === updated.id
          );


        if (index !== -1) {

          this.assignments[index] =
            updated;

        }


        this.applyFilters();

      },


      error: (error) => {

        console.error(
          'Error updating assignment:',
          error
        );

        alert(
          'Failed to update assignment.'
        );

      }

    });

}


  // =====================================================
  // DOWNLOAD ATTACHMENT
  // =====================================================

 downloadAttachment(
  assignment: Assignment
): void {

  if (!assignment.id) {
    return;
  }

  if (!assignment.attachmentFileName) {
    alert('No attachment found.');
    return;
  }

  this.assignmentService
    .downloadAttachment(assignment.id)
    .subscribe({

      next: (blob: Blob) => {

        // Create temporary URL for the downloaded file
        const url =
          window.URL.createObjectURL(blob);

        // Create temporary download link
        const link =
          document.createElement('a');

        link.href = url;

        link.download =
          assignment.attachmentFileName
          || 'assignment-attachment';

        // Start download
        document.body.appendChild(link);

        link.click();

        // Cleanup
        document.body.removeChild(link);

        window.URL.revokeObjectURL(url);

      },

      error: (error) => {

        console.error(
          'Error downloading attachment:',
          error
        );

        alert(
          'Failed to download attachment.'
        );

      }

    });

}


  // =====================================================
  // STATUS TEXT
  // =====================================================

  getStatusText(
    assignment: Assignment
  ): string {

    if (!assignment.isActive) {
      return 'Inactive';
    }


    if (assignment.isPublished) {
      return 'Published';
    }


    return 'Draft';
  }


  // =====================================================
  // STATUS CLASS
  // =====================================================

  getStatusClass(
    assignment: Assignment
  ): string {

    if (!assignment.isActive) {
      return 'inactive';
    }


    if (assignment.isPublished) {
      return 'published';
    }


    return 'draft';
  }


  // =====================================================
  // DEADLINE CHECK
  // =====================================================

  isExpired(
    deadline: string
  ): boolean {

    return new Date(deadline).getTime()
      < new Date().getTime();
  }


  // =====================================================
  // TOTAL ASSIGNMENTS
  // =====================================================

  get totalAssignments(): number {

    return this.assignments.length;
  }


  // =====================================================
  // PUBLISHED COUNT
  // =====================================================

  get publishedCount(): number {

    return this.assignments.filter(
      x => x.isPublished
    ).length;
  }


  // =====================================================
  // DRAFT COUNT
  // =====================================================

  get draftCount(): number {

    return this.assignments.filter(
      x => !x.isPublished
    ).length;
  }


  // =====================================================
  // ACTIVE COUNT
  // =====================================================

  get activeCount(): number {

    return this.assignments.filter(
      x => x.isActive
    ).length;
  }


  // =====================================================
  // TRACK BY
  // =====================================================

  trackById(
    index: number,
    assignment: Assignment
  ): string {

    return assignment.id;
  }

}