import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-dashboard-redirect',
  standalone: true,
  template: `
    <div class="dashboard-redirect">
      <div class="spinner"></div>
      <p>Loading dashboard...</p>
    </div>
  `,
  styles: [`
    .dashboard-redirect {
      min-height: 100vh;
      display: flex;
      flex-direction: column;
      align-items: center;
      justify-content: center;
      background: #f8fafc;
      color: #475569;
    }

    .spinner {
      width: 42px;
      height: 42px;
      border: 4px solid #e2e8f0;
      border-top-color: #2563eb;
      border-radius: 50%;
      animation: spin 0.8s linear infinite;
      margin-bottom: 15px;
    }

    p {
      margin: 0;
      font-size: 15px;
      font-weight: 500;
    }

    @keyframes spin {
      to {
        transform: rotate(360deg);
      }
    }
  `]
})
export class DashboardRedirectComponent implements OnInit {

  constructor(
    private router: Router
  ) {}

  ngOnInit(): void {
    const role =
      localStorage.getItem('role') ||
      localStorage.getItem('userRole');

    if (!role) {
      this.router.navigate(['/login']);
      return;
    }

    switch (role.toLowerCase()) {

      case 'admin':
      case 'superadmin':
        this.router.navigate(['/dashboard']);
        break;

      case 'teacher':
        this.router.navigate(['/teacher/dashboard']);
        break;

      case 'student':
        this.router.navigate(['/student/dashboard']);
        break;

      default:
        this.router.navigate(['/login']);
        break;
    }
  }
}