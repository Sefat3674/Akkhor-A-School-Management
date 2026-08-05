import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';


import {
  UserManagementService,
  UserDto,
  RoleDto
}
from '../../../../../core/services/user-management.service';



@Component({

  selector: 'app-user-list',

  standalone: true,

  imports: [
    CommonModule,
    FormsModule
  ],

  templateUrl: './user-list.component.html',

  styleUrl: './user-list.component.scss'

})


export class UserListComponent implements OnInit {


  users: UserDto[] = [];

  filteredUsers: UserDto[] = [];

  roles: RoleDto[] = [];


  loading = false;



  searchText = '';



  selectedRole = '';

  selectedStatus = '';



  page = 1;

  pageSize = 10;

showPasswordModal = false;

selectedUserId = '';

newPassword = '';

confirmPassword = '';

showPassword = false;



  constructor(

    private userService: UserManagementService,

    private router: Router

  ) {}






  ngOnInit(): void {


    this.loadUsers();

    this.loadRoles();


  }








  // ===============================
  // LOAD USERS
  // ===============================


  loadUsers(): void {


    this.loading = true;



    this.userService.getUsers()

    .subscribe({


      next:(response: UserDto[])=>{


        this.users = response;


        this.filteredUsers = response;


        this.loading = false;


      },


      error:(error:any)=>{


        console.error(
          'User loading error',
          error
        );


        this.loading = false;


      }


    });


  }









  // ===============================
  // LOAD ROLES
  // ===============================


  loadRoles():void{


    this.userService.getRoles()

    .subscribe({


      next:(response: RoleDto[])=>{


        this.roles = response;


      },


      error:(error:any)=>{


        console.error(
          'Role loading error',
          error
        );


      }


    });


  }









  // ===============================
  // SEARCH + FILTER
  // ===============================


  filterUsers():void{


    const search =

    this.searchText

    .toLowerCase()

    .trim();





    this.filteredUsers =


    this.users.filter(user=>{





      const searchMatch =


      !search

      ||

      (user.fullName ?? '')
      .toLowerCase()
      .includes(search)


      ||

      (user.email ?? '')
      .toLowerCase()
      .includes(search)


      ||

      (user.phoneNumber ?? '')
      .includes(search);








      const roleMatch =


      !this.selectedRole


      ||


      user.roles?.some(role=>


        role.toLowerCase().trim()

        ===

        this.selectedRole
        .toLowerCase()
        .trim()


      );








      const statusMatch =


      this.selectedStatus === ''


      ||


      user.isActive ===

      (
        this.selectedStatus === 'true'
      );








      return (

        searchMatch

        &&

        roleMatch

        &&

        statusMatch

      );


    });




    this.page = 1;


  }









  // ===============================
  // PAGINATION
  // ===============================


  get paginatedUsers():UserDto[]{


    const start =

    (this.page - 1)

    *

    this.pageSize;




    return this.filteredUsers.slice(

      start,

      start + this.pageSize

    );


  }







  get startIndex():number{


    return (

      (this.page - 1)

      *

      this.pageSize

    );


  }






  get endIndex():number{


    return Math.min(

      this.page * this.pageSize,

      this.filteredUsers.length

    );


  }







  get totalPages():number{


    return Math.max(

      1,

      Math.ceil(

        this.filteredUsers.length /

        this.pageSize

      )

    );


  }







  nextPage():void{


    if(this.page < this.totalPages)

    {

      this.page++;

    }


  }







  previousPage():void{


    if(this.page > 1)

    {

      this.page--;

    }


  }









  // ===============================
  // ROUTING
  // ===============================


  addUser():void{


    this.router.navigate([

      '/admin/users/add'

    ]);


  }








  editUser(id:string):void{


    this.router.navigate([

      '/admin/users/edit',

      id

    ]);


  }









  // ===============================
  // ACTIVE / INACTIVE
  // ===============================


  toggleStatus(user:UserDto):void{


    const newStatus =

    !user.isActive;



    const action =

    newStatus

    ?

    'activate'

    :

    'deactivate';





    if(!confirm(

      `Are you sure you want to ${action} ${user.fullName}?`

    ))

    {

      return;

    }







    this.userService

    .updateUserStatus(

      user.id,

      newStatus

    )

    .subscribe({



      next:()=>{


        user.isActive = newStatus;


      },



      error:(error:any)=>{


        console.error(

          'Status update failed',

          error

        );


      }


    });


  }









  // ===============================
  // DELETE USER
  // ===============================


  deleteUser(id:string):void{


    if(!confirm(

      'Are you sure you want to delete this user?'

    ))

    {

      return;

    }







    this.userService

    .deleteUser(id)

    .subscribe({



      next:()=>{


        this.loadUsers();


      },



      error:(error:any)=>{


        console.error(

          'Delete failed',

          error

        );


      }


    });


  }









  // ===============================
  // ROLE STYLE
  // ===============================


  getRoleClass(role:string):string{


    switch(role.toLowerCase())

    {


      case 'admin':

        return 'role-admin';



      case 'teacher':

        return 'role-teacher';



      case 'student':

        return 'role-student';



      default:

        return 'role-user';


    }


  }
// ===============================
// RESET PASSWORD
// ===============================
// ===============================
// OPEN RESET PASSWORD MODAL
// ===============================

resetPassword(userId:string):void{

  this.selectedUserId = userId;

  this.newPassword = '';

  this.confirmPassword = '';

  this.showPassword = false;

  this.showPasswordModal = true;

}
// ===============================
// CONFIRM RESET PASSWORD
// ===============================

confirmResetPassword():void{


  if(!this.newPassword)
  {

    alert('Please enter new password');

    return;

  }



  if(this.newPassword.length < 6)
  {

    alert('Password must be minimum 6 characters');

    return;

  }



  if(this.newPassword !== this.confirmPassword)
  {

    alert('Password does not match');

    return;

  }



  this.userService
  .resetPassword(
    this.selectedUserId,
    this.newPassword
  )
  .subscribe({

    next:()=>{

      alert(
        'Password reset successfully'
      );

      this.closePasswordModal();

    },


    error:(error:any)=>{

      console.error(
        'Password reset failed',
        error
      );

      alert(
        'Password reset failed'
      );

    }

  });


}

// ===============================
// CLOSE PASSWORD MODAL
// ===============================

closePasswordModal():void{


  this.showPasswordModal = false;

  this.selectedUserId = '';

  this.newPassword = '';

  this.confirmPassword = '';

  this.showPassword = false;


}











  getRoleIcon(role:string):string{


    switch(role.toLowerCase())

    {


      case 'admin':

        return 'bi-shield-fill-check';



      case 'teacher':

        return 'bi-person-workspace';



      case 'student':

        return 'bi-mortarboard-fill';



      default:

        return 'bi-person-fill';


    }


  }



}