import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../../environments/environment';

import { Observable } from 'rxjs';

import {
  CourseModel,
  CreateCourse,
  UpdateCourse
} from '../models/course.model';


@Injectable({
  providedIn: 'root'
})
export class CourseService {


  private apiUrl = `${environment.apiUrl}/api/courses`;



  constructor(
    private http: HttpClient
  ) {}



  getAll(): Observable<CourseModel[]> {

    return this.http.get<CourseModel[]>(this.apiUrl);

  }



  getById(id:string):Observable<CourseModel>{

    return this.http.get<CourseModel>(
      `${this.apiUrl}/${id}`
    );

  }



  create(
    model:CreateCourse
  ):Observable<CourseModel>{

    return this.http.post<CourseModel>(
      this.apiUrl,
      model
    );

  }



  update(
    id:string,
    model:UpdateCourse
  ):Observable<void>{

    return this.http.put<void>(
      `${this.apiUrl}/${id}`,
      model
    );

  }



  delete(id:string):Observable<void>{

    return this.http.delete<void>(
      `${this.apiUrl}/${id}`
    );

  }

}