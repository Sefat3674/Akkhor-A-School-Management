
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

import {
  ApplicationSetting,
  CreateApplicationSetting,
  UpdateApplicationSetting
} from '../models/application-setting.model';

import { environment } from '../../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class ApplicationSettingService {

  private readonly apiUrl =
    `${environment.apiUrl}/api/application-settings`;

  constructor(
    private readonly http: HttpClient
  ) {}

  // =====================================================
  // GET ALL
  // =====================================================

  getAll(): Observable<ApplicationSetting[]> {
    return this.http.get<ApplicationSetting[]>(
      this.apiUrl
    );
  }

  // =====================================================
  // GET BY ID
  // =====================================================

  getById(id: string): Observable<ApplicationSetting> {
    return this.http.get<ApplicationSetting>(
      `${this.apiUrl}/${id}`
    );
  }

  // =====================================================
  // GET BY KEY
  // =====================================================

  getByKey(key: string): Observable<ApplicationSetting> {
    return this.http.get<ApplicationSetting>(
      `${this.apiUrl}/key/${encodeURIComponent(key)}`
    );
  }

  // =====================================================
  // GET BY CATEGORY
  // =====================================================

  getByCategory(
    category: string
  ): Observable<ApplicationSetting[]> {
    return this.http.get<ApplicationSetting[]>(
      `${this.apiUrl}/category/${encodeURIComponent(category)}`
    );
  }

  // =====================================================
  // CREATE
  // =====================================================

  create(
    setting: CreateApplicationSetting
  ): Observable<ApplicationSetting> {
    return this.http.post<ApplicationSetting>(
      this.apiUrl,
      setting
    );
  }

  // =====================================================
  // UPDATE
  // =====================================================

  update(
    id: string,
    setting: UpdateApplicationSetting
  ): Observable<ApplicationSetting> {
    return this.http.put<ApplicationSetting>(
      `${this.apiUrl}/${id}`,
      setting
    );
  }

  // =====================================================
  // DELETE
  // =====================================================

  delete(id: string): Observable<void> {
    return this.http.delete<void>(
      `${this.apiUrl}/${id}`
    );
  }
}

