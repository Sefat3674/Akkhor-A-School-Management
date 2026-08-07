import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';


import { CourseService } from '../../../../../core/services/course.service';

import { CourseModel } from '../../../../../core/models/course.model';



@Component({

selector:'app-course-list',

standalone:true,

imports:[
 CommonModule
],

templateUrl:'./course-list.component.html',

styleUrls:[
 './course-list.component.scss'
]

})


export class CourseListComponent implements OnInit{


courses:CourseModel[]=[];


isLoading=false;



constructor(

private courseService:CourseService,

private router:Router

){}



ngOnInit():void{

 this.loadCourses();

}




loadCourses(){

this.isLoading=true;


this.courseService.getAll()
.subscribe({

next:(res)=>{

this.courses=res;

this.isLoading=false;

},


error:(err)=>{

console.error(err);

this.isLoading=false;

}


});


}





add(){

this.router.navigate([
'/admin/courses/create'
]);

}



edit(id:string){

this.router.navigate([
'/admin/courses/edit',
id
]);

}




delete(id:string){


if(confirm('Delete this course?')){


this.courseService.delete(id)
.subscribe(()=>{

this.loadCourses();

});


}


}



}