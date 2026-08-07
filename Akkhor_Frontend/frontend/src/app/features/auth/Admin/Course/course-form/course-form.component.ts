import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

import {
  ActivatedRoute,
  Router
} from '@angular/router';


import { CourseService } from '../../../../../core/services/course.service';

import { ClassService } from '../../../../../core/services/class.service';

import {
  CreateCourse,
  UpdateCourse
} from '../../../../../core/models/course.model';


import {
  ClassModel
} from '../../../../../core/models/class.model';



@Component({

selector:'app-course-form',

standalone:true,

imports:[
 CommonModule,
 FormsModule
],

templateUrl:'./course-form.component.html',

styleUrls:[
 './course-form.component.scss'
]

})


export class CourseFormComponent implements OnInit{


id:string|null=null;


isEdit=false;


saving=false;


classes:ClassModel[]=[];



model:CreateCourse={

classId:'',

courseName:'',

courseCode:'',

description:'',

durationMonths:undefined

};



isActive=true;




constructor(

private courseService:CourseService,

private classService:ClassService,

private router:Router,

private route:ActivatedRoute

){}




ngOnInit():void{


this.loadClasses();


this.id=this.route.snapshot.paramMap.get('id');


if(this.id){

this.isEdit=true;

this.loadCourse(this.id);

}


}





loadClasses(){


this.classService.getAll()
.subscribe({

next:(res)=>{

this.classes=res;

},


error:(err)=>{

console.error(err);

}

});


}






loadCourse(id:string){


this.courseService.getById(id)
.subscribe({

next:(res)=>{


this.model={


classId:res.classId,

courseName:res.courseName,

courseCode:res.courseCode,

description:res.description,

durationMonths:res.durationMonths


};


this.isActive=res.isActive;


},


error:(err)=>{

console.error(err);

}


});


}







save(){


this.saving=true;



if(!this.isEdit){



this.courseService.create(this.model)
.subscribe({

next:()=>{


this.router.navigate([
'/admin/courses'
]);


},


error:(err)=>{


console.error(err);

this.saving=false;


}


});


return;


}






const updateModel:UpdateCourse={


courseName:this.model.courseName,


courseCode:this.model.courseCode,


description:this.model.description,


durationMonths:this.model.durationMonths,


isActive:this.isActive


};




this.courseService.update(
this.id!,
updateModel
)
.subscribe({

next:()=>{


this.router.navigate([
'/admin/courses'
]);


},


error:(err)=>{


console.error(err);

this.saving=false;


}


});



}







cancel(){


this.router.navigate([
'/admin/courses'
]);


}


}