import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';

import { SectionService } from '../../../../../core/services/section.service';
import { ClassService } from '../../../../../core/services/class.service';

import {
  CreateSection,
  UpdateSection
} from '../../../../../core/models/section.model';

import { ClassModel } from '../../../../../core/models/class.model';

@Component({
  selector: 'app-section-form',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule
  ],
  templateUrl: './section-form.component.html',
  styleUrls: ['./section-form.component.scss']
})
export class SectionFormComponent implements OnInit {

  id: string | null = null;

  isEdit = false;

  saving = false;

  classes: ClassModel[] = [];

  model: CreateSection = {
  classId: '',
  sectionName: '',
  roomNumber: '',
  capacity: undefined
};

  isActive = true;

  constructor(
    private sectionService: SectionService,
    private classService: ClassService,
    private router: Router,
    private route: ActivatedRoute
  ) { }

  ngOnInit(): void {

    this.loadClasses();

    this.id = this.route.snapshot.paramMap.get('id');

    if (this.id) {

      this.isEdit = true;

      this.loadSection(this.id);

    }

  }

  loadClasses(): void {

    this.classService.getAll().subscribe({

      next: (res) => {

        this.classes = res;

      },

      error: (err) => {

        console.error(err);

      }

    });

  }

  loadSection(id: string): void {

    this.sectionService.getById(id).subscribe({

      next: (res) => {

        this.model = {

          classId: res.classId,
          sectionName: res.sectionName,
          roomNumber: res.roomNumber,
          capacity: res.capacity

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

      this.sectionService.create(this.model).subscribe({

        next: () => {

          this.router.navigate(['/admin/sections']);

        },

        error: (err) => {

          console.error(err);

          this.saving = false;

        }

      });

      return;

    }

    const updateModel: UpdateSection = {

      sectionName: this.model.sectionName,
      roomNumber: this.model.roomNumber,
      capacity: this.model.capacity,
      isActive: this.isActive

    };

    this.sectionService.update(this.id!, updateModel).subscribe({

      next: () => {

        this.router.navigate(['/admin/sections']);

      },

      error: (err) => {

        console.error(err);

        this.saving = false;

      }

    });

  }

  cancel(): void {

    this.router.navigate(['/admin/sections']);

  }

}