import { Component, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';


@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [
    CommonModule,
    RouterLink
  ],
  templateUrl: './user-dashboard.component.html'
})
export class DashboardComponent implements OnInit {


  totalStudents = signal(0);

  totalTeachers = signal(0);

  totalAssignments = signal(0);

  pendingSubmissions = signal(0);



  constructor()
  {

  }




  ngOnInit(): void {


    // Temporary data
    // Later connect with API

    this.totalStudents.set(2500);

    this.totalTeachers.set(180);

    this.totalAssignments.set(75);

    this.pendingSubmissions.set(25);


  }


}