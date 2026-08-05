import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

import { Observable } from 'rxjs';

import { environment } from '../../../environments/environment';

import {
  AcademicYear,
  CreateAcademicYear,
  UpdateAcademicYear
}
from '../models/academic-year.model';



@Injectable({
  providedIn:'root'
})
export class AcademicYearService {


private apiUrl =
environment.apiUrl + '/api/academic-years';



constructor(
 private http:HttpClient
){}



getAll():Observable<AcademicYear[]>
{
 return this.http.get<AcademicYear[]>(
   this.apiUrl
 );
}



getById(id:string)
{
 return this.http.get<AcademicYear>(
   `${this.apiUrl}/${id}`
 );
}



create(data:CreateAcademicYear)
{
 return this.http.post(
   this.apiUrl,
   data
 );
}



update(
 id:string,
 data:UpdateAcademicYear
)
{
 return this.http.put(
   `${this.apiUrl}/${id}`,
   data
 );
}



delete(id:string)
{
 return this.http.delete(
   `${this.apiUrl}/${id}`
 );
}


}