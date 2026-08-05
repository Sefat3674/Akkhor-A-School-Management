import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';


import {
  UserManagementService,
  UpdateUserDto,
  UserDto,
  RoleDto

} from '../../../../../core/services/user-management.service';



@Component({

  selector: 'app-edit-user',

  standalone: true,

  imports: [
    CommonModule,
    FormsModule
  ],

  templateUrl: './edit-user.component.html',

  styleUrl: './edit-user.component.scss'

})


export class EditUserComponent implements OnInit {



  id = '';

  loading = false;



  roles: RoleDto[] = [];




  user: UpdateUserDto = {


    fullName: '',

    email: '',

    phoneNumber: '',

    isActive: true,

    role: ''

  };







  constructor(

    private route: ActivatedRoute,

    private router: Router,

    private userService: UserManagementService

  ) {}







  ngOnInit(): void {


    this.id =
      this.route.snapshot.paramMap.get('id')!;


    this.loadRoles();


    this.loadUser();


  }








  // =====================================
  // LOAD ALL AVAILABLE ROLES
  // =====================================


  loadRoles(): void {


    this.userService
      .getRoles()

      .subscribe({

        next: (response: RoleDto[]) => {


          this.roles = response;


        },


        error: (error: any) => {


          console.error(
            'Role loading failed',
            error
          );


        }


      });


  }








  // =====================================
  // LOAD USER INFORMATION
  // =====================================


  loadUser(): void {


    this.userService
      .getUserById(this.id)

      .subscribe({

        next: (response: UserDto) => {



          this.user.fullName =
            response.fullName;



          this.user.email =
            response.email;



          this.user.phoneNumber =
            response.phoneNumber ?? '';



          this.user.isActive =
            response.isActive;



          this.loadUserRole();


        },


        error: (error: any) => {


          console.error(
            'User loading failed',
            error
          );


        }


      });


  }









  // =====================================
  // LOAD CURRENT USER ROLE
  // =====================================


  loadUserRole(): void {


    this.userService
      .getUserRoles(this.id)

      .subscribe({

        next: (roles: string[]) => {



          if(roles.length > 0)
          {

            this.user.role =
              roles[0];

          }



        },


        error:(error:any)=>{


          console.error(
            'User role loading failed',
            error
          );


        }


      });


  }










  // =====================================
  // UPDATE USER
  // =====================================


  updateUser(): void {



    this.loading = true;



    this.userService

      .updateUser(

        this.id,

        this.user

      )


      .subscribe({



        next:()=>{


          alert(
            'User updated successfully'
          );



          this.router.navigate([
            '/admin/users'
          ]);



        },





        error:(error:any)=>{


          console.error(
            'User update failed',
            error
          );



          this.loading = false;



        }



      });



  }









  // =====================================
  // CANCEL
  // =====================================


  cancel():void{


    this.router.navigate([

      '/admin/users'

    ]);


  }



}