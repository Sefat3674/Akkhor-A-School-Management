import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';

import { SectionService } from '../../../../../core/services/section.service';
import { SectionModel } from '../../../../../core/models/section.model';

@Component({
  selector: 'app-section-list',
  standalone: true,
  imports: [
    CommonModule
  ],
  templateUrl: './section-list.component.html',
  styleUrls: ['./section-list.component.scss']
})
export class SectionListComponent implements OnInit {

  sections: SectionModel[] = [];

  isLoading = false;

  constructor(
    private sectionService: SectionService,
    private router: Router
  ) { }

  ngOnInit(): void {
    this.loadSections();
  }

  loadSections(): void {

    this.isLoading = true;

    this.sectionService.getAll().subscribe({

      next: (res) => {

        this.sections = res;
        this.isLoading = false;

      },

      error: (err) => {

        console.error('Error loading sections:', err);
        this.isLoading = false;

      }

    });

  }

  addSection(): void {

    this.router.navigate(['/admin/sections/create']);

  }

  editSection(id: string): void {

    this.router.navigate(['/admin/sections/edit', id]);

  }

  deleteSection(id: string): void {

    if (!confirm('Are you sure you want to delete this section?')) {
      return;
    }

    this.sectionService.delete(id).subscribe({

      next: () => {

        this.loadSections();

      },

      error: (err) => {

        console.error(err);
        alert('Failed to delete section.');

      }

    });

  }

}