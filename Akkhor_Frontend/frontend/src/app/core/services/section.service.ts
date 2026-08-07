import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

import { environment } from '../../../environments/environment';

import {
  SectionModel,
  CreateSection,
  UpdateSection
} from '../models/section.model';

@Injectable({
  providedIn: 'root'
})
export class SectionService {

  private apiUrl = `${environment.apiUrl}/api/sections`;

  constructor(
    private http: HttpClient
  ) { }

  getAll(): Observable<SectionModel[]> {

    return this.http.get<SectionModel[]>(this.apiUrl);

  }

  getById(id: string): Observable<SectionModel> {

    return this.http.get<SectionModel>(
      `${this.apiUrl}/${id}`
    );

  }

  create(model: CreateSection): Observable<SectionModel> {

    return this.http.post<SectionModel>(
      this.apiUrl,
      model
    );

  }

  update(
    id: string,
    model: UpdateSection
  ) {

    return this.http.put(
      `${this.apiUrl}/${id}`,
      model
    );

  }

  delete(id: string) {

    return this.http.delete(
      `${this.apiUrl}/${id}`
    );

  }

}