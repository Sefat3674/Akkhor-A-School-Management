import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';

import { AcademicYearService } 
from '../../../../core/services/academic-year.service';

import { AcademicYear } 
from '../../../../core/models/academic-year.model';

import { EditAcademicYearComponent } 
from './edit-academic-year/edit-academic-year.component';



@Component({

selector:'app-academic-years',

standalone:true,

imports:[

    CommonModule,

    EditAcademicYearComponent

],

templateUrl:'./academic-years.component.html',

styleUrls:[
    './academic-years.component.scss'
]

})


export class AcademicYearsComponent implements OnInit {



years:AcademicYear[]=[];


loading=false;



// EDIT MODAL

showEditModal=false;


selectedAcademicYearId:string='';






constructor(

private academicYearService:AcademicYearService,

private router:Router

){}







ngOnInit():void{


this.loadAcademicYears();


}








loadAcademicYears():void{


this.loading=true;



this.academicYearService
.getAll()

.subscribe({


next:(response)=>{


this.years=response;


this.loading=false;


},



error:(error)=>{


console.error(
'Failed to load academic years',
error
);


this.loading=false;


}



});



}









// CREATE PAGE


addAcademicYear():void{


this.router.navigate(

[
'/admin/academic-years/create'
]

);


}









// OPEN EDIT MODAL


editAcademicYear(id:string):void{


this.selectedAcademicYearId=id;


this.showEditModal=true;



}









// CLOSE EDIT MODAL


closeEditModal():void{


this.showEditModal=false;


this.selectedAcademicYearId='';



}








// AFTER UPDATE


reloadAcademicYears():void{


this.closeEditModal();


this.loadAcademicYears();


}









// DELETE


delete(id:string):void{


const confirmDelete = confirm(

'Are you sure you want to delete this academic year?'

);



if(!confirmDelete)
{

return;

}






this.academicYearService

.delete(id)

.subscribe({



next:()=>{


this.loadAcademicYears();


},




error:(error)=>{


console.error(

'Delete failed',

error

);


}



});



}



}