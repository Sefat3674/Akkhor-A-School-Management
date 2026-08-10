import { Injectable } from '@angular/core';

import {
  HttpClient
} from '@angular/common/http';

import {
  Observable
} from 'rxjs';

import {
  environment
} from '../../../environments/environment';

import {
  AdminDashboard
} from '../models/admin-dashboard.model';


@Injectable({
  providedIn: 'root'
})
export class AdminDashboardService {

  private readonly apiUrl =
    `${environment.apiUrl}/api/admin/dashboard`;


  constructor(
    private http: HttpClient
  ) {}


  // =====================================================
  // GET ADMIN DASHBOARD
  // =====================================================

  getDashboard():
    Observable<AdminDashboard> {

    return this.http.get<AdminDashboard>(
      this.apiUrl
    );

  }

}