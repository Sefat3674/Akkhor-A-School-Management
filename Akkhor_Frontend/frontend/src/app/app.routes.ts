import { Routes } from '@angular/router';
import { authGuard } from './core/guards/auth.guard';

export const routes: Routes = [

  // ==========================
  // AUTH
  // ==========================

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



  // ==========================
  // APPLICATION
  // ==========================

  {
    path: '',
    canActivate: [authGuard],

    children: [

      {
        path: '',
        redirectTo: 'dashboard',
        pathMatch: 'full'
      },



      // ==========================
      // Dashboard
      // ==========================

      {
        path: 'dashboard',
        loadComponent: () =>
          import('./features/dashboard/dashboard.component')
            .then(m => m.DashboardComponent)
      },



      // ==========================
      // ADMIN
      // ==========================

      // User List

      {
        path: 'admin/users',
        loadComponent: () =>
          import('./features/auth/Admin/user-management/user-list/user-list.component')
            .then(m => m.UserListComponent)
      },



      // Add User

      {
        path: 'admin/users/add',
        loadComponent: () =>
          import('./features/auth/Admin/user-management/add-user/add-user.component')
            .then(m => m.AddUserComponent)
      },



      // Edit User

      {
        path: 'admin/users/edit/:id',
        loadComponent: () =>
          import('./features/auth/Admin/user-management/edit-user/edit-user.component')
            .then(m => m.EditUserComponent)
      },



      // User Profile

      /*{
        path: 'admin/users/profile/:id',
        loadComponent: () =>
          import('./features/auth/Admin/user-management/user-profile/user-profile.component')
            .then(m => m.UserProfileComponent)
      }, */


      {
        path: 'admin/academic-years',
        loadComponent: () =>
          import('./features/auth/Admin/academic-years/academic-years.component')
            .then(m => m.AcademicYearsComponent)
      },

      {
      path:'admin/academic-years/create',
      loadComponent:()=>import(
      './features/auth/Admin/academic-years/create-academic-year/create-academic-year.component'
      )
      .then(m=>m.CreateAcademicYearComponent)
      },

      {
      path:'admin/academic-years/edit/:id',
      loadComponent:()=>import(
      './features/auth/Admin/academic-years/edit-academic-year/edit-academic-year.component'
      )
      .then(m=>m.EditAcademicYearComponent)
      },


      {
        path:'admin/classes',
        loadComponent:()=>import(
        './features/auth/Admin/Class/class-list/class-list.component'
        )
        .then(m=>m.ClassListComponent)
        },


        {
          path: 'admin/classes/create',
          loadComponent: () =>
            import('./features/auth/Admin/Class/class-form/class-form.component')
              .then(m => m.ClassFormComponent)
        },
        {
          path: 'admin/classes/edit/:id',
          loadComponent: () =>
            import('./features/auth/Admin/Class/class-form/class-form.component')
              .then(m => m.ClassFormComponent)
        },




        {
        path:'admin/sections',
        loadComponent:()=>import(
        './features/auth/Admin/Section/section-list/section-list.component'
        )
        .then(m=>m.SectionListComponent)
        },


        {
          path: 'admin/sections/create',
          loadComponent: () =>
            import('./features/auth/Admin/Section/section-form/section-form.component')
              .then(m => m.SectionFormComponent)
        },
        {
          path: 'admin/sections/edit/:id',
          loadComponent: () =>
            import('./features/auth/Admin/Section/section-form/section-form.component')
              .then(m => m.SectionFormComponent)
        },



        {
        path: 'admin/courses',
        loadComponent: () =>
          import('./features/auth/Admin/Course/course-list/course-list.component')
            .then(m => m.CourseListComponent)
      },

      { 
        path: 'admin/courses/create',
        loadComponent: () =>
          import('./features/auth/Admin/Course/course-form/course-form.component')
            .then(m => m.CourseFormComponent)
      },

      {
        path: 'admin/courses/edit/:id',
        loadComponent: () =>
          import('./features/auth/Admin/Course/course-form/course-form.component')
            .then(m => m.CourseFormComponent)
      },



      

      {
        path: 'admin/subjects',
        loadComponent: () =>
          import('./features/auth/Admin/Subject/subject-list/subject-list.component')
            .then(m => m.SubjectListComponent)
      },

      {
        path: 'admin/subjects/create',
        loadComponent: () =>
          import('./features/auth/Admin/Subject/subject-form/subject-form.component')
            .then(m => m.SubjectFormComponent)
      },

      {
        path: 'admin/subjects/edit/:id',
        loadComponent: () =>
          import('./features/auth/Admin/Subject/subject-form/subject-form.component')
            .then(m => m.SubjectFormComponent)
      },



      // ==========================
// COURSE SUBJECTS
// ==========================

{
  path: 'admin/course-subjects',
  loadComponent: () =>
    import('./features/auth/Admin/course-subject/course-subject-list/course-subject-list.component')
      .then(m => m.CourseSubjectListComponent)
},


{
  path: 'admin/course-subjects/create',
  loadComponent: () =>
    import('./features/auth/Admin/course-subject/course-subject-form/course-subject-form.component')
      .then(m => m.CourseSubjectFormComponent)
},


{
  path: 'admin/course-subjects/edit/:id',
  loadComponent: () =>
    import('./features/auth/Admin/course-subject/course-subject-form/course-subject-form.component')
      .then(m => m.CourseSubjectFormComponent)
},



{
  path: 'admin/student-enrollment',
  loadComponent: () =>
    import('./features/auth/Admin/student-enrollment/student-enrollment.component')
      .then(m => m.StudentEnrollmentComponent)
},


{
        path: 'admin/assign-teacher',
        loadComponent: () =>
          import('./features/auth/Admin/assign-teacher/assign-teacher.component')
            .then(m => m.AssignTeacherComponent)
  },
        

        



  {
  path: 'teacher/classes',
  loadComponent: () =>
    import(
      './features/auth/Teacher/my-classes/my-classes.component'
    )
    .then(m => m.MyClassesComponent)
},     


    // Assignment List

{
  path: 'teacher/assignments',
  loadComponent: () =>
    import(
      './features/auth/Teacher/assignments/assignment-list/assignment-list.component'
    ).then(m => m.AssignmentListComponent)
},


// Create Assignment

{
  path: 'teacher/assignments/create',
  loadComponent: () =>
    import(
      './features/auth/Teacher/assignments/assignment-form/assignment-form.component'
    ).then(m => m.AssignmentFormComponent)
},


// Edit Assignment

{
  path: 'teacher/assignments/edit/:id',
  loadComponent: () =>
    import(
      './features/auth/Teacher/assignments/assignment-form/assignment-form.component'
    ).then(m => m.AssignmentFormComponent)
},


// Review Assignment Submissions

{
  path: 'teacher/assignments/review/:id',
  loadComponent: () =>
    import(
      './features/auth/Teacher/assignments/review-assignment/review-assignment.component'
    ).then(m => m.ReviewAssignmentComponent)
},


// --------------------------------------------------
// SUBMISSIONS
// --------------------------------------------------

{
  path: 'teacher/submissions',
  loadComponent: () =>
    import(
      './features/auth/Teacher/assignments/review-assignment/review-assignment.component'
    ).then(m => m.ReviewAssignmentComponent)
},


// --------------------------------------------------
// MARKS & FEEDBACK
// --------------------------------------------------

{
  path: 'teacher/marks',
  loadComponent: () =>
    import(
      './features/auth/Teacher/assignments/marks-feedback/marks-feedback.component'
    ).then(m => m.MarksFeedbackComponent)
},








    {
        path: 'student/assignments',

        loadComponent: () =>
          import(
            './features/auth/Student/student-assignment/student-assignment.component'
          )
          .then(m => m.StudentAssignmentComponent)
      },

      
      /*
      Future Modules

      {
        path:'admin/classes',
        loadComponent:()=>import(...)
      },

      {
        path:'admin/subjects',
        loadComponent:()=>import(...)
      },

      {
        path:'admin/assign-teachers',
        loadComponent:()=>import(...)
      },

      {
        path:'admin/assignments',
        loadComponent:()=>import(...)
      },

      {
        path:'admin/submissions',
        loadComponent:()=>import(...)
      },

      {
        path:'admin/settings',
        loadComponent:()=>import(...)
      }
      */

    ]

  },



  {
    path: '**',
    redirectTo: 'dashboard'
  }

];