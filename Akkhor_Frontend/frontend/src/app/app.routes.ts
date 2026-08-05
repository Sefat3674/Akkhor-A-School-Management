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