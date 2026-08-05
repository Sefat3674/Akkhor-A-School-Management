import { Injectable, signal } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, tap } from 'rxjs';
import { environment } from '../../../environments/environment';

import {
  AuthResponse,
  CurrentUser,
  RegisterUser
} from '../models/models';


const TOKEN_KEY = 'akkhor_token';
const USER_KEY = 'akkhor_user';



@Injectable({
  providedIn: 'root'
})
export class AuthService {


  currentUser = signal<CurrentUser | null>(
    this.readStoredUser()
  );



  constructor(
    private http: HttpClient
  ) {}




  // =========================
  // LOGIN
  // =========================

  login(
    email: string,
    password: string
  ): Observable<AuthResponse> {


    return this.http.post<AuthResponse>(
      `${environment.apiUrl}/api/Auth/login`,
      {
        email,
        password
      }

    ).pipe(

      tap(response => {

        this.storeSession(response);

      })

    );

  }





  // =========================
  // REGISTER
  // =========================

  register(
    dto: RegisterUser
  ): Observable<AuthResponse> {


    return this.http.post<AuthResponse>(
      `${environment.apiUrl}/api/Auth/register`,
      dto

    ).pipe(

      tap(response => {

        this.storeSession(response);

      })

    );

  }





  // =========================
  // LOGOUT
  // =========================

  logout(): void {


    localStorage.removeItem(
      TOKEN_KEY
    );


    localStorage.removeItem(
      USER_KEY
    );


    this.currentUser.set(null);

  }






  // =========================
  // GET TOKEN
  // =========================

  getToken(): string | null {

    return localStorage.getItem(
      TOKEN_KEY
    );

  }






  // =========================
  // LOGIN STATUS
  // =========================

  isLoggedIn(): boolean {

    return !!this.getToken();

  }






  // =========================
  // SAVE SESSION
  // =========================

  private storeSession(
    response: AuthResponse
  ): void {


    localStorage.setItem(
      TOKEN_KEY,
      response.token
    );



    const user: CurrentUser = {

      userId: response.userId,

      fullName: response.fullName,

      email: response.email,

      role: response.role

    };



    localStorage.setItem(
      USER_KEY,
      JSON.stringify(user)
    );



    this.currentUser.set(user);

  }







  // =========================
  // LOAD USER FROM STORAGE
  // =========================

  private readStoredUser(): CurrentUser | null {


    const raw =
      localStorage.getItem(
        USER_KEY
      );



    if (!raw) {

      return null;

    }



    try {


      const user: CurrentUser =
        JSON.parse(raw);



      return user;


    }
    catch {


      localStorage.removeItem(
        USER_KEY
      );


      return null;

    }

  }







  // =========================
  // ROLE
  // =========================

  getRole(): string {


    return (
      this.currentUser()
        ?.role
        ?.trim()
        ?? ''
    );

  }







  // =========================
  // SUPER ADMIN
  // =========================

  isSuperAdmin(): boolean {


    return this.getRole()
      .toLowerCase()
      === 'superadmin';

  }







  // =========================
  // ADMIN
  // =========================

  isAdmin(): boolean {


    const role =
      this.getRole()
      .toLowerCase();



    return (

      role === 'admin' ||

      role === 'superadmin'

    );

  }







  // =========================
  // TEACHER
  // =========================

  isTeacher(): boolean {


    return this.getRole()
      .toLowerCase()
      === 'teacher';

  }







  // =========================
  // STUDENT
  // =========================

  isStudent(): boolean {


    return this.getRole()
      .toLowerCase()
      === 'student';

  }







  // =========================
  // NORMAL USER
  // =========================

  isNormalUser(): boolean {


    return this.getRole()
      .toLowerCase()
      === 'normal user';

  }

}