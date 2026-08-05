import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

import { environment } from '../../../environments/environment';



export interface UserDto {

  id: string;

  fullName: string;

  email: string;

  phoneNumber?: string;

  isActive: boolean;

  roles: string[];

}
export interface RoleDto {

  id:string;

  name:string;

}


export interface CreateUserDto {

  fullName: string;

  email: string;

  password: string;

  role: string;

}



export interface UpdateUserDto {


 fullName:string;


 email:string;


 phoneNumber?:string;


 isActive:boolean;


 role:string;


}



export interface AssignRoleDto {

  userId: string;

  role: string;

}



@Injectable({
  providedIn: 'root'
})
export class UserManagementService {



  // Backend Controller:
  // [Route("api/users")]
  //
  // Example:
  // https://localhost:50268/api/users

  private apiUrl = `${environment.apiUrl}/api/users`;



  constructor(
    private http: HttpClient
  ) {}


getUserRoles(id:string):Observable<string[]>{

 return this.http.get<string[]>(
   `${this.apiUrl}/${id}/roles`
 );

}

getRoles(): Observable<RoleDto[]> {

 return this.http.get<RoleDto[]>(
   `${this.apiUrl}/roles`
 );

}

  // =====================================================
  // GET ALL USERS
  // GET: api/users
  // =====================================================

  getUsers(): Observable<UserDto[]> {

    return this.http.get<UserDto[]>(
      this.apiUrl
    );

  }





  // =====================================================
  // GET USER BY ID
  // GET: api/users/{id}
  // =====================================================

  getUserById(
    id: string
  ): Observable<UserDto> {


    return this.http.get<UserDto>(
      `${this.apiUrl}/${id}`
    );

  }





  // =====================================================
  // CREATE USER
  // POST: api/users
  // =====================================================

  createUser(
    user: CreateUserDto
  ): Observable<any> {


    return this.http.post(
      this.apiUrl,
      user
    );

  }





  // =====================================================
  // UPDATE USER
  // PUT: api/users/{id}
  // =====================================================

  updateUser(
    id: string,
    user: UpdateUserDto
  ): Observable<any> {


    return this.http.put(
      `${this.apiUrl}/${id}`,
      user
    );

  }





  // =====================================================
  // DELETE / DEACTIVATE USER
  // DELETE: api/users/{id}
  // =====================================================

  deleteUser(
    id: string
  ): Observable<any> {


    return this.http.delete(
      `${this.apiUrl}/${id}`
    );

  }





  // =====================================================
  // ASSIGN ROLE
  // PUT: api/users/assign-role
  // =====================================================

  assignRole(
    data: AssignRoleDto
  ): Observable<any> {


    return this.http.put(
      `${this.apiUrl}/assign-role`,
      data
    );

  }

// ========================================
// ACTIVE / INACTIVE USER
// PUT api/users/{id}/status
// ========================================


updateUserStatus(
  id:string,
  isActive:boolean
):Observable<any>{


  return this.http.put(

    `${this.apiUrl}/${id}/status`,

    isActive

  );


}



  // =====================================================
  // RESET PASSWORD
  // PUT: api/users/{id}/reset-password
  // =====================================================

  // ===============================
// RESET PASSWORD
// ===============================

resetPassword(
  userId:string,
  newPassword:string
)
{
  return this.http.put<any>(
    `${this.apiUrl}/${userId}/reset-password`,
    JSON.stringify(newPassword),
    {
      headers:{
        'Content-Type':'application/json'
      }
    }
  );
}



}