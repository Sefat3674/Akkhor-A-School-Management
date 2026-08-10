import { Routes } from '@angular/router';
import { authGuard } from './core/guards/auth.guard';

export const routes: Routes = [

  // =====================================================
  // AUTH
  // =====================================================

  {
    path: 'login',
    loadComponent: () =>
      import('./features/auth/login/login.component')
        .then(m => m.LoginComponent)
  },

  {
    path: 'register',
    loadComponent: () =>
      import('./features/auth/register/register.component')
        .then(m => m.RegisterComponent)
  },


  // =====================================================
  // APPLICATION
  // =====================================================

  {
    path: '',
    canActivate: [authGuard],

    children: [

      // =================================================
      // DEFAULT
      // =================================================

      {
        path: '',
        redirectTo: 'admin/dashboard',
        pathMatch: 'full'
      },


      // =================================================
      // ADMIN DASHBOARD
      // =================================================

      {
        path: 'admin/dashboard',
        loadComponent: () =>
          import(
            './features/auth/Admin/admin-dashboard/admin-dashboard.component'
          ).then(
            m => m.AdminDashboardComponent
          )
      },


      // =================================================
      // ADMIN - USER MANAGEMENT
      // =================================================

      {
        path: 'admin/users',
        loadComponent: () =>
          import(
            './features/auth/Admin/user-management/user-list/user-list.component'
          ).then(
            m => m.UserListComponent
          )
      },

      {
        path: 'admin/users/add',
        loadComponent: () =>
          import(
            './features/auth/Admin/user-management/add-user/add-user.component'
          ).then(
            m => m.AddUserComponent
          )
      },

      {
        path: 'admin/users/edit/:id',
        loadComponent: () =>
          import(
            './features/auth/Admin/user-management/edit-user/edit-user.component'
          ).then(
            m => m.EditUserComponent
          )
      },


      // =================================================
      // ADMIN - ACADEMIC YEARS
      // =================================================

      {
        path: 'admin/academic-years',
        loadComponent: () =>
          import(
            './features/auth/Admin/academic-years/academic-years.component'
          ).then(
            m => m.AcademicYearsComponent
          )
      },

      {
        path: 'admin/academic-years/create',
        loadComponent: () =>
          import(
            './features/auth/Admin/academic-years/create-academic-year/create-academic-year.component'
          ).then(
            m => m.CreateAcademicYearComponent
          )
      },

      {
        path: 'admin/academic-years/edit/:id',
        loadComponent: () =>
          import(
            './features/auth/Admin/academic-years/edit-academic-year/edit-academic-year.component'
          ).then(
            m => m.EditAcademicYearComponent
          )
      },


      // =================================================
      // ADMIN - CLASSES
      // =================================================

      {
        path: 'admin/classes',
        loadComponent: () =>
          import(
            './features/auth/Admin/Class/class-list/class-list.component'
          ).then(
            m => m.ClassListComponent
          )
      },

      {
        path: 'admin/classes/create',
        loadComponent: () =>
          import(
            './features/auth/Admin/Class/class-form/class-form.component'
          ).then(
            m => m.ClassFormComponent
          )
      },

      {
        path: 'admin/classes/edit/:id',
        loadComponent: () =>
          import(
            './features/auth/Admin/Class/class-form/class-form.component'
          ).then(
            m => m.ClassFormComponent
          )
      },


      // =================================================
      // ADMIN - SECTIONS
      // =================================================

      {
        path: 'admin/sections',
        loadComponent: () =>
          import(
            './features/auth/Admin/Section/section-list/section-list.component'
          ).then(
            m => m.SectionListComponent
          )
      },

      {
        path: 'admin/sections/create',
        loadComponent: () =>
          import(
            './features/auth/Admin/Section/section-form/section-form.component'
          ).then(
            m => m.SectionFormComponent
          )
      },

      {
        path: 'admin/sections/edit/:id',
        loadComponent: () =>
          import(
            './features/auth/Admin/Section/section-form/section-form.component'
          ).then(
            m => m.SectionFormComponent
          )
      },


      // =================================================
      // ADMIN - COURSES
      // =================================================

      {
        path: 'admin/courses',
        loadComponent: () =>
          import(
            './features/auth/Admin/Course/course-list/course-list.component'
          ).then(
            m => m.CourseListComponent
          )
      },

      {
        path: 'admin/courses/create',
        loadComponent: () =>
          import(
            './features/auth/Admin/Course/course-form/course-form.component'
          ).then(
            m => m.CourseFormComponent
          )
      },

      {
        path: 'admin/courses/edit/:id',
        loadComponent: () =>
          import(
            './features/auth/Admin/Course/course-form/course-form.component'
          ).then(
            m => m.CourseFormComponent
          )
      },


      // =================================================
      // ADMIN - SUBJECTS
      // =================================================

      {
        path: 'admin/subjects',
        loadComponent: () =>
          import(
            './features/auth/Admin/Subject/subject-list/subject-list.component'
          ).then(
            m => m.SubjectListComponent
          )
      },

      {
        path: 'admin/subjects/create',
        loadComponent: () =>
          import(
            './features/auth/Admin/Subject/subject-form/subject-form.component'
          ).then(
            m => m.SubjectFormComponent
          )
      },

      {
        path: 'admin/subjects/edit/:id',
        loadComponent: () =>
          import(
            './features/auth/Admin/Subject/subject-form/subject-form.component'
          ).then(
            m => m.SubjectFormComponent
          )
      },


      // =================================================
      // ADMIN - COURSE SUBJECTS
      // =================================================

      {
        path: 'admin/course-subjects',
        loadComponent: () =>
          import(
            './features/auth/Admin/course-subject/course-subject-list/course-subject-list.component'
          ).then(
            m => m.CourseSubjectListComponent
          )
      },

      {
        path: 'admin/course-subjects/create',
        loadComponent: () =>
          import(
            './features/auth/Admin/course-subject/course-subject-form/course-subject-form.component'
          ).then(
            m => m.CourseSubjectFormComponent
          )
      },

      {
        path: 'admin/course-subjects/edit/:id',
        loadComponent: () =>
          import(
            './features/auth/Admin/course-subject/course-subject-form/course-subject-form.component'
          ).then(
            m => m.CourseSubjectFormComponent
          )
      },


      // =================================================
      // ADMIN - STUDENT ENROLLMENT
      // =================================================

      {
        path: 'admin/student-enrollment',
        loadComponent: () =>
          import(
            './features/auth/Admin/student-enrollment/student-enrollment.component'
          ).then(
            m => m.StudentEnrollmentComponent
          )
      },


      // =================================================
      // ADMIN - ASSIGN TEACHER
      // =================================================

      {
        path: 'admin/assign-teacher',
        loadComponent: () =>
          import(
            './features/auth/Admin/assign-teacher/assign-teacher.component'
          ).then(
            m => m.AssignTeacherComponent
          )
      },


      // =================================================
      // ADMIN - ALL ASSIGNMENTS
      // =================================================

      {
        path: 'admin/assignments',
        loadComponent: () =>
          import(
            './features/auth/Admin/assignments/admin-assignments.component'
          ).then(
            m => m.AdminAssignmentsComponent
          )
      },


      // =================================================
      // ADMIN - ALL SUBMISSIONS
      // =================================================

      {
        path: 'admin/submissions',
        loadComponent: () =>
          import(
            './features/auth/Admin/submissions/admin-submissions.component'
          ).then(
            m => m.AdminSubmissionsComponent
          )
      },


      // =================================================
      // ADMIN - SETTINGS
      // =================================================

      // Add this only when the settings component exists.
      {
        path: 'admin/settings',
        loadComponent: () =>
          import('./features/auth/Admin/application-setting/application-settings.component')
            .then(m => m.ApplicationSettingsComponent)
      },


      // =================================================
      // TEACHER DASHBOARD
      // =================================================

      {
        path: 'teacher/dashboard',
        loadComponent: () =>
          import(
            './features/auth/Teacher/teacher-dashboard/teacher-dashboard.component'
          ).then(
            m => m.TeacherDashboardComponent
          )
      },


      // =================================================
      // TEACHER - CLASSES
      // =================================================

      {
        path: 'teacher/classes',
        loadComponent: () =>
          import(
            './features/auth/Teacher/my-classes/my-classes.component'
          ).then(
            m => m.MyClassesComponent
          )
      },


      // =================================================
      // TEACHER - ASSIGNMENTS
      // =================================================

      {
        path: 'teacher/assignments',
        loadComponent: () =>
          import(
            './features/auth/Teacher/assignments/assignment-list/assignment-list.component'
          ).then(
            m => m.AssignmentListComponent
          )
      },

      {
        path: 'teacher/assignments/create',
        loadComponent: () =>
          import(
            './features/auth/Teacher/assignments/assignment-form/assignment-form.component'
          ).then(
            m => m.AssignmentFormComponent
          )
      },

      {
        path: 'teacher/assignments/edit/:id',
        loadComponent: () =>
          import(
            './features/auth/Teacher/assignments/assignment-form/assignment-form.component'
          ).then(
            m => m.AssignmentFormComponent
          )
      },

      {
        path: 'teacher/assignments/review/:id',
        loadComponent: () =>
          import(
            './features/auth/Teacher/assignments/review-assignment/review-assignment.component'
          ).then(
            m => m.ReviewAssignmentComponent
          )
      },


      // =================================================
      // TEACHER - SUBMISSIONS
      // =================================================

      {
        path: 'teacher/submissions',
        loadComponent: () =>
          import(
            './features/auth/Teacher/assignments/review-assignment/review-assignment.component'
          ).then(
            m => m.ReviewAssignmentComponent
          )
      },


      // =================================================
      // TEACHER - ASSIGNMENT PREVIEW
      // =================================================

      {
        path: 'teacher/teacher-assignment-preview',
        loadComponent: () =>
          import(
            './features/auth/Teacher/teacher-assignment-preview/teacher-assignment-preview.component'
          ).then(
            m => m.TeacherAssignmentPreviewComponent
          )
      },


      // =================================================
      // TEACHER - MARKS & FEEDBACK
      // =================================================

      {
        path: 'teacher/marks',
        loadComponent: () =>
          import(
            './features/auth/Teacher/assignments/marks-feedback/marks-feedback.component'
          ).then(
            m => m.MarksFeedbackComponent
          )
      },


      // =================================================
      // STUDENT DASHBOARD
      // =================================================

      {
        path: 'student/dashboard',
        loadComponent: () =>
          import(
            './features/auth/Student/student-dashboard/student-dashboard.component'
          ).then(
            m => m.StudentDashboardComponent
          )
      },


      // =================================================
      // STUDENT - ASSIGNMENTS
      // =================================================

      {
        path: 'student/assignments',
        loadComponent: () =>
          import(
            './features/auth/Student/student-assignment/student-assignment.component'
          ).then(
            m => m.StudentAssignmentComponent
          )
      },

      {
        path: 'student/assignments/:id',
        loadComponent: () =>
          import(
            './features/auth/Student/student-assignment-details/student-assignment-details.component'
          ).then(
            m => m.StudentAssignmentDetailsComponent
          )
      },

      {
        path: 'student/assignments/:id/submit',
        loadComponent: () =>
          import(
            './features/auth/Student/student-submission-page/student-submission-page.component'
          ).then(
            m => m.StudentSubmissionPageComponent
          )
      },


      // =================================================
      // STUDENT - SUBMISSIONS
      // =================================================

      {
        path: 'student/submissions',
        loadComponent: () =>
          import(
            './features/auth/Student/student-submissions/student-submissions.component'
          ).then(
            m => m.StudentSubmissionsComponent
          )
      },


      // =================================================
      // STUDENT - RESULTS
      // =================================================

      {
        path: 'student/results',
        loadComponent: () =>
          import(
            './features/auth/Student/marks-feedback/marks-feedback.component'
          ).then(
            m => m.MarksFeedbackComponent
          )
      }

    ]
  },


  // =====================================================
  // UNKNOWN ROUTE
  // =====================================================

  {
    path: '**',
    redirectTo: 'dashboard'
  }

];