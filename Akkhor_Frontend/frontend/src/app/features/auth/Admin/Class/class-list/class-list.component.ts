import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';

import { ClassService } from '../../../../../core/services/class.service';
import { ClassModel } from '../../../../../core/models/class.model';

@Component({
  selector: 'app-class-list',
  standalone: true,
  imports: [
    CommonModule
  ],
  templateUrl: './class-list.component.html',
  styleUrls: ['./class-list.component.scss']
})
export class ClassListComponent implements OnInit {

  classes: ClassModel[] = [];

  isLoading = false;

  constructor(
    private service: ClassService,
    private router: Router
  ) { }

  ngOnInit(): void {
    this.load();
  }

  load(): void {

    this.isLoading = true;

    this.service.getAll().subscribe({

      next: (res) => {

        this.classes = res;
        this.isLoading = false;

      },

      error: (err) => {

        console.error('Error loading classes:', err);
        this.isLoading = false;

      }

    });

  }

  addClass(): void {

    this.router.navigate(['/admin/classes/create']);

  }

  editClass(id: string): void {

    this.router.navigate(['/admin/classes/edit', id]);

  }

  deleteClass(id: string): void {

    if (!confirm('Are you sure you want to delete this class?')) {
      return;
    }

    this.service.delete(id).subscribe({

      next: () => {

        this.load();

      },

      error: (err) => {

        console.error('Delete failed:', err);
        alert('Failed to delete class.');

      }

    });

  }

}