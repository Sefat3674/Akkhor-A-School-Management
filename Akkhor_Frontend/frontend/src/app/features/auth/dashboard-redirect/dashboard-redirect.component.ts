import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-dashboard-redirect',
  standalone: true,
  template: '',
})
export class DashboardRedirectComponent implements OnInit {

  constructor(private router: Router) {}

  ngOnInit(): void {

    const token = localStorage.getItem('token');

    if (!token) {
      this.router.navigate(['/login']);
      return;
    }

    try {

      const payload = JSON.parse(
        atob(token.split('.')[1])
      );

      const role =
        payload.role ||
        payload[
          'http://schemas.microsoft.com/ws/2008/06/identity/claims/role'
        ];

      switch (role) {

        // ADMIN
        case 'Admin':
        case 'SuperAdmin':
          this.router.navigateByUrl('/admin/dashboard', {
            replaceUrl: true
          });
          break;

        // TEACHER
        case 'Teacher':
          this.router.navigateByUrl('/teacher/dashboard', {
            replaceUrl: true
          });
          break;

        // STUDENT
        case 'Student':
          this.router.navigateByUrl('/student/dashboard', {
            replaceUrl: true
          });
          break;

        // NORMAL USER
        case 'Normal User':
        case 'NormalUser':
        case 'User':
          this.router.navigateByUrl('/user/dashboard', {
            replaceUrl: true
          });
          break;

        default:
          localStorage.removeItem('token');
          this.router.navigateByUrl('/login', {
            replaceUrl: true
          });
          break;
      }

    } catch (error) {

      console.error('Invalid JWT token:', error);

      localStorage.removeItem('token');

      this.router.navigateByUrl('/login', {
        replaceUrl: true
      });
    }
  }
}