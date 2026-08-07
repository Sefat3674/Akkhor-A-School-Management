import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';

import { SubjectService } from '../../../../../core/services/subject.service';

import {
  CreateSubject,
  UpdateSubject
} from '../../../../../core/models/subject.model';

@Component({
  selector: 'app-subject-form',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule
  ],
  templateUrl: './subject-form.component.html',
  styleUrls: [
    './subject-form.component.scss'
  ]
})
export class SubjectFormComponent implements OnInit {

  id: string | null = null;

  isEdit = false;

  saving = false;

  model: CreateSubject = {

    name: '',

    code: '',

    description: '',

    creditHours: undefined

  };

  isActive = true;

  constructor(

    private subjectService: SubjectService,

    private router: Router,

    private route: ActivatedRoute

  ) { }

  ngOnInit(): void {

    this.id = this.route.snapshot.paramMap.get('id');

    if (this.id) {

      this.isEdit = true;

      this.loadSubject(this.id);

    }

  }

  loadSubject(id: string): void {

    this.subjectService.getById(id).subscribe({

      next: (res) => {

        this.model = {

          name: res.name,

          code: res.code,

          description: res.description,

          creditHours: res.creditHours

        };

        this.isActive = res.isActive;

      },

      error: (err) => {

        console.error(err);

      }

    });

  }

  save(): void {

    this.saving = true;

    if (!this.isEdit) {

      this.subjectService.create(this.model).subscribe({

        next: () => {

          this.router.navigate([
            '/admin/subjects'
          ]);

        },

        error: (err) => {

          console.error(err);

          this.saving = false;

        }

      });

      return;

    }

    const updateModel: UpdateSubject = {

      name: this.model.name,

      code: this.model.code,

      description: this.model.description,

      creditHours: this.model.creditHours,

      isActive: this.isActive

    };

    this.subjectService.update(
      this.id!,
      updateModel
    ).subscribe({

      next: () => {

        this.router.navigate([
          '/admin/subjects'
        ]);

      },

      error: (err) => {

        console.error(err);

        this.saving = false;

      }

    });

  }

  cancel(): void {

    this.router.navigate([
      '/admin/subjects'
    ]);

  }

}