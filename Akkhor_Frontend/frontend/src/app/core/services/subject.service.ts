import { Injectable } from '@angular/core';

import { HttpClient } from '@angular/common/http';

import { Observable } from 'rxjs';

import { environment } from '../../../environments/environment';

import {
  SubjectModel,
  CreateSubject,
  UpdateSubject
} from '../models/subject.model';

@Injectable({
  providedIn: 'root'
})
export class SubjectService {

  private apiUrl =
    `${environment.apiUrl}/api/subjects`;

  constructor(
    private http: HttpClient
  ) { }

  // GET ALL
  getAll(): Observable<SubjectModel[]> {

    return this.http.get<SubjectModel[]>(
      this.apiUrl
    );

  }

  // GET BY ID
  getById(id: string): Observable<SubjectModel> {

    return this.http.get<SubjectModel>(
      `${this.apiUrl}/${id}`
    );

  }

  // CREATE
  create(model: CreateSubject): Observable<any> {

    return this.http.post(
      this.apiUrl,
      model
    );

  }

  // UPDATE
  update(
    id: string,
    model: UpdateSubject
  ): Observable<any> {

    return this.http.put(
      `${this.apiUrl}/${id}`,
      model
    );

  }

  // DELETE
  delete(id: string): Observable<any> {

    return this.http.delete(
      `${this.apiUrl}/${id}`
    );

  }

}