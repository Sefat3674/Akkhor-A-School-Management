import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';

import { 
  UserManagementService,
  CreateUserDto
} from '../../../../../core/services/user-management.service';


@Component({
  selector: 'app-add-user',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule
  ],
  templateUrl: './add-user.component.html',
  styleUrl: './add-user.component.scss'
})
export class AddUserComponent {


  user: CreateUserDto = {

    fullName: '',

    email: '',

    password: '',

    role: ''

  };



  loading = false;



  roles = [
    'Admin',
    'Teacher',
    'Student'
  ];




  constructor(
    private userService: UserManagementService,
    private router: Router
  ) {}





  saveUser(){


    this.loading = true;


    this.userService.createUser(this.user)
    .subscribe({

      next:(response)=>{


        alert('User created successfully');


        this.router.navigate([
          '/admin/users'
        ]);


      },


      error:(error)=>{


        console.error(error);


        alert('User creation failed');


        this.loading = false;


      }


    });


  }





  cancel(){

    this.router.navigate([
      '/admin/users'
    ]);

  }


}