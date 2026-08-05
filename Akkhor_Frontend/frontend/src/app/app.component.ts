import { Component } from '@angular/core';
import {
  Router,
  RouterOutlet,
  RouterLink,
  RouterLinkActive
} from '@angular/router';

import { CommonModule } from '@angular/common';

import { AuthService } from './core/services/auth.service';



@Component({
  selector: 'app-root',

  standalone: true,

  imports: [
    CommonModule,
    RouterOutlet,
    RouterLink,
    RouterLinkActive
  ],

  templateUrl: './app.component.html'
})
export class AppComponent {



  constructor(
    public auth: AuthService,
    private router: Router
  ) {}





  initials(): string {


    const name =
      this.auth.currentUser()
      ?.fullName ?? '';



    const parts =
      name
      .trim()
      .split(/\s+/)
      .filter(Boolean);



    if(parts.length === 0)
    {
      return '?';
    }



    if(parts.length === 1)
    {
      return parts[0]
        .substring(0,2)
        .toUpperCase();
    }



    return (
      parts[0][0] +
      parts[parts.length - 1][0]
    )
    .toUpperCase();

  }






  logout(): void {


    this.auth.logout();


    this.router.navigate([
      '/login'
    ]);

  }



}