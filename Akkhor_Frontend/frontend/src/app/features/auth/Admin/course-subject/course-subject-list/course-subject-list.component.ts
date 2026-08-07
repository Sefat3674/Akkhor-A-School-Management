import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';

import { CourseSubjectService } from '../../../../../core/services/course-subject.service';
import { CourseSubjectModel } from '../../../../../core/models/course-subject.model';

@Component({
  selector: 'app-course-subject-list',
  standalone: true,
  imports: [
    CommonModule
  ],
  templateUrl: './course-subject-list.component.html',
  styleUrls: [
    './course-subject-list.component.scss'
  ]
})
export class CourseSubjectListComponent implements OnInit {

  courseSubjects: CourseSubjectModel[] = [];

  isLoading = false;

  constructor(
    private courseSubjectService: CourseSubjectService,
    private router: Router
  ) { }

  ngOnInit(): void {
    this.loadCourseSubjects();
  }

  loadCourseSubjects(): void {

    this.isLoading = true;

    this.courseSubjectService.getAll()
      .subscribe({

        next: (res) => {

          this.courseSubjects = res;
          this.isLoading = false;

        },

        error: (err) => {

          console.error(err);
          this.isLoading = false;

        }

      });

  }

  add(): void {

    this.router.navigate([
      '/admin/course-subjects/create'
    ]);

  }

  edit(id: string): void {

    this.router.navigate([
      '/admin/course-subjects/edit',
      id
    ]);

  }

  delete(id: string): void {

    if (!confirm('Delete this course subject?')) {
      return;
    }

    this.courseSubjectService.delete(id)
      .subscribe({

        next: () => {

          this.loadCourseSubjects();

        },

        error: err => {

          console.error(err);

        }

      });

  }

}