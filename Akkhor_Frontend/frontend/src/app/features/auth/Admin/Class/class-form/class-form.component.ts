import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';

import { ClassService } from '../../../../../core/services/class.service';
import { AcademicYearService } from '../../../../../core/services/academic-year.service';

import {
  CreateClass,
  UpdateClass
} from '../../../../../core/models/class.model';

@Component({
  selector: 'app-class-form',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule
  ],
  templateUrl: './class-form.component.html',
  styleUrls: ['./class-form.component.scss']
})
export class ClassFormComponent implements OnInit {

  id: string | null = null;

  isEdit = false;

  saving = false;

  academicYears: any[] = [];

  model: CreateClass = {
    academicYearId: '',
    name: '',
    code: '',
    description: '',
    displayOrder: 1
  };

  isActive = true;

  constructor(
    private classService: ClassService,
    private academicYearService: AcademicYearService,
    private router: Router,
    private route: ActivatedRoute
  ) { }

  ngOnInit(): void {

    this.loadAcademicYears();

    this.id = this.route.snapshot.paramMap.get('id');

    if (this.id) {

      this.isEdit = true;

      this.loadClass(this.id);

    }

  }

  loadAcademicYears(): void {

    this.academicYearService.getAll().subscribe({

      next: (res: any) => {

        this.academicYears = res;

      }

    });

  }

  loadClass(id: string): void {

    this.classService.getById(id).subscribe({

      next: (res: any) => {

        this.model = {

          academicYearId: res.academicYearId,
          name: res.name,
          code: res.code,
          description: res.description,
          displayOrder: res.displayOrder

        };

        this.isActive = res.isActive;

      }

    });

  }

  save(): void {

    this.saving = true;

    if (!this.isEdit) {

      this.classService.create(this.model).subscribe({

        next: () => {

          this.router.navigate(['/admin/classes']);

        },

        error: () => {

          this.saving = false;

        }

      });

      return;

    }

    const updateModel: UpdateClass = {

      name: this.model.name,
      code: this.model.code,
      description: this.model.description,
      displayOrder: this.model.displayOrder,
      isActive: this.isActive

    };

    this.classService.update(this.id!, updateModel).subscribe({

      next: () => {

        this.router.navigate(['/admin/classes']);

      },

      error: () => {

        this.saving = false;

      }

    });

  }

  cancel(): void {

    this.router.navigate(['/admin/classes']);

  }

}