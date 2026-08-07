import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';

import {
  ClassModel,
  CreateClass,
  UpdateClass
} from '../models/class.model';


@Injectable({
  providedIn:'root'
})
export class ClassService {


private apiUrl = `${environment.apiUrl}/api/classes`;



constructor(
 private http:HttpClient
){}



getAll():Observable<ClassModel[]>{

 return this.http.get<ClassModel[]>(
   this.apiUrl
 );

}



getById(id:string):Observable<ClassModel>{

 return this.http.get<ClassModel>(
   `${this.apiUrl}/${id}`
 );

}



create(data:CreateClass):Observable<ClassModel>{

 return this.http.post<ClassModel>(
   this.apiUrl,
   data
 );

}



update(
 id:string,
 data:UpdateClass
){

 return this.http.put(
   `${this.apiUrl}/${id}`,
   data
 );

}



delete(id:string){

 return this.http.delete(
   `${this.apiUrl}/${id}`
 );

}


}