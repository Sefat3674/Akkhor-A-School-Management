import { Component, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import {
  FormBuilder,
  ReactiveFormsModule,
  Validators
} from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { finalize } from 'rxjs';
import { AuthService } from '../../../core/services/auth.service';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    RouterLink
  ],
  templateUrl: './login.component.html'
})
export class LoginComponent {

  // =====================================================
  // LOGIN FORM
  // =====================================================

  form = this.fb.nonNullable.group({
    email: [
      '',
      [
        Validators.required,
        Validators.email
      ]
    ],

    password: [
      '',
      [
        Validators.required,
        Validators.minLength(8)
      ]
    ]
  });

  // =====================================================
  // UI STATE
  // =====================================================

  loading = signal(false);

  errorMessage = signal<string | null>(null);

  showPassword = signal(false);

  // =====================================================
  // CONSTRUCTOR
  // =====================================================

  constructor(
    private readonly fb: FormBuilder,
    private readonly auth: AuthService,
    private readonly router: Router
  ) {}

  // =====================================================
  // LOGIN
  // =====================================================

  submit(): void {

    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    this.loading.set(true);
    this.errorMessage.set(null);

    const {
      email,
      password
    } = this.form.getRawValue();

    this.auth
      .login(
        email.trim(),
        password
      )
      .pipe(
        finalize(() => {
          this.loading.set(false);
        })
      )
      .subscribe({

        // ===============================================
        // LOGIN SUCCESS
        // ===============================================

        next: () => {

          const role = this.auth
            .getRole()
            .toLowerCase();

          console.log('Logged in role:', role);

          switch (role) {

            // ===========================================
            // ADMIN
            // ===========================================

            case 'admin':
            case 'superadmin':

              this.router.navigate([
                '/admin/dashboard'
              ]);

              break;

            // ===========================================
            // TEACHER
            // ===========================================

            case 'teacher':

              this.router.navigate([
                '/teacher/dashboard'
              ]);

              break;

            // ===========================================
            // STUDENT
            // ===========================================

            case 'student':

              this.router.navigate([
                '/student/dashboard'
              ]);

              break;

            // ===========================================
            // NORMAL USER
            // ===========================================

            case 'normal user':
            case 'normaluser':
            case 'user':

              this.router.navigate([
                '/dashboard'
              ]);

              break;

            // ===========================================
            // UNKNOWN ROLE
            // ===========================================

            default:

              console.error(
                'Unknown user role:',
                role
              );

              this.errorMessage.set(
                'Unable to determine your user role.'
              );

              break;
          }
        },

        // ===============================================
        // LOGIN ERROR
        // ===============================================

        error: err => {

          console.error(
            'Login error:',
            err
          );

          this.errorMessage.set(
            err?.error?.message ??
            'Invalid email address or password.'
          );
        }

      });
  }

  // =====================================================
  // PASSWORD VISIBILITY
  // =====================================================

  togglePassword(): void {

    this.showPassword.update(
      value => !value
    );

  }

}