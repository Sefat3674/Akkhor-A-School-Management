import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';

import { SubjectService } from '../../../../../core/services/subject.service';
import { SubjectModel } from '../../../../../core/models/subject.model';

@Component({
  selector: 'app-subject-list',
  standalone: true,
  imports: [
    CommonModule
  ],
  templateUrl: './subject-list.component.html',
  styleUrls: [
    './subject-list.component.scss'
  ]
})
export class SubjectListComponent implements OnInit {

  subjects: SubjectModel[] = [];

  isLoading = false;

  constructor(
    private subjectService: SubjectService,
    private router: Router
  ) { }

  ngOnInit(): void {

    this.loadSubjects();

  }

  loadSubjects(): void {

    this.isLoading = true;

    this.subjectService.getAll().subscribe({

      next: (res) => {

        this.subjects = res;

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
      '/admin/subjects/create'
    ]);

  }

  edit(id: string): void {

    this.router.navigate([
      '/admin/subjects/edit',
      id
    ]);

  }

  delete(id: string): void {

    if (!confirm('Are you sure you want to delete this subject?')) {
      return;
    }

    this.subjectService.delete(id).subscribe({

      next: () => {

        this.loadSubjects();

      },

      error: (err) => {

        console.error(err);

      }

    });

  }

}