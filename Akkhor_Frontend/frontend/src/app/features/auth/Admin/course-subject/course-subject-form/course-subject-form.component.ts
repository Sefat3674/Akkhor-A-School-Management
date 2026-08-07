import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';

import { CourseSubjectService } from '../../../../../core/services/course-subject.service';
import { CourseService } from '../../../../../core/services/course.service';
import { SubjectService } from '../../../../../core/services/subject.service';

import { CourseModel } from '../../../../../core/models/course.model';
import { SubjectModel } from '../../../../../core/models/subject.model';

@Component({
  selector: 'app-course-subject-form',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule
  ],
  templateUrl: './course-subject-form.component.html',
  styleUrls: ['./course-subject-form.component.scss']
})
export class CourseSubjectFormComponent implements OnInit {

  isEdit = false;

  id = '';

  isSaving = false;

  courses: CourseModel[] = [];

  subjects: SubjectModel[] = [];

  form = this.fb.group({

    courseId: ['', Validators.required],

    subjectId: ['', Validators.required],

    isMandatory: [true],

    displayOrder: [0, Validators.required]

  });

  constructor(

    private fb: FormBuilder,

    private courseSubjectService: CourseSubjectService,

    private courseService: CourseService,

    private subjectService: SubjectService,

    private router: Router,

    private route: ActivatedRoute

  ) { }

  ngOnInit(): void {

    this.loadCourses();

    this.loadSubjects();

    this.id = this.route.snapshot.paramMap.get('id') ?? '';

    if (this.id) {

      this.isEdit = true;

      this.loadData();

    }

  }

  loadCourses(): void {

    this.courseService.getAll().subscribe({

      next: res => this.courses = res,

      error: err => console.error(err)

    });

  }

  loadSubjects(): void {

    this.subjectService.getAll().subscribe({

      next: res => this.subjects = res,

      error: err => console.error(err)

    });

  }

  loadData(): void {

    this.courseSubjectService.getById(this.id)
      .subscribe({

        next: res => {

          this.form.patchValue({

            courseId: res.courseId,

            subjectId: res.subjectId,

            isMandatory: res.isMandatory,

            displayOrder: res.displayOrder

          });

        },

        error: err => console.error(err)

      });

  }

  save(): void {

  if (this.form.invalid) {

    this.form.markAllAsTouched();

    return;

  }

  this.isSaving = true;

  const dto = {

    courseId: this.form.controls.courseId.value!,

    subjectId: this.form.controls.subjectId.value!,

    isMandatory: this.form.controls.isMandatory.value ?? true,

    displayOrder: this.form.controls.displayOrder.value ?? 0

  };

  if (this.isEdit) {

    this.courseSubjectService
      .update(this.id, dto)
      .subscribe({

        next: () => {

          this.router.navigate(['/admin/course-subjects']);

        },

        error: err => {

          console.error(err);

          this.isSaving = false;

        }

      });

  } else {

    this.courseSubjectService
      .create(dto)
      .subscribe({

        next: () => {

          this.router.navigate(['/admin/course-subjects']);

        },

        error: err => {

          console.error(err);

          this.isSaving = false;

        }

      });

  }

}

  cancel(): void {

    this.router.navigate([
      '/admin/course-subjects'
    ]);

  }

}