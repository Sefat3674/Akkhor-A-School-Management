import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, Validators } from '@angular/forms';
import { Router } from '@angular/router';

import { AcademicYearService } from '../../../../../core/services/academic-year.service';


@Component({
  selector: 'app-create-academic-year',
  standalone: true,
  imports:[
    CommonModule,
    ReactiveFormsModule
  ],
  templateUrl:'./create-academic-year.component.html',
  styleUrls:[
    './create-academic-year.component.scss'
  ]
})
export class CreateAcademicYearComponent {


constructor(
 private fb:FormBuilder,
 private service:AcademicYearService,
 private router:Router
){}



form=this.fb.group({

 name:[
  '',
  Validators.required
 ],

 startDate:[
  '',
  Validators.required
 ],

 endDate:[
  '',
  Validators.required
 ],

 isActive:[
  true
 ]

});




save(){


if(this.form.invalid)
{
 return;
}



this.service.create(
 this.form.value as any
)
.subscribe({

next:()=>{

 this.router.navigate(
 ['/admin/academic-years']
 );

},

error:(err)=>{

 console.log(err);

}

});


}




cancel(){

this.router.navigate(
 ['/admin/academic-years']
);

}


}