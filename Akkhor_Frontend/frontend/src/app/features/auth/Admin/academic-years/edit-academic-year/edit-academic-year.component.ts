import { 
Component, 
Input, 
Output, 
EventEmitter, 
OnChanges,
SimpleChanges
} from '@angular/core';


import { CommonModule } 
from '@angular/common';


import { FormsModule } 
from '@angular/forms';


import { AcademicYearService } 
from '../../../../../core/services/academic-year.service';



@Component({

selector:'app-edit-academic-year',

standalone:true,

imports:[
CommonModule,
FormsModule
],

templateUrl:'./edit-academic-year.component.html',

styleUrls:[
'./edit-academic-year.component.scss'
]

})


export class EditAcademicYearComponent 
implements OnChanges {



@Input() academicYearId!:string;



@Output() close =
new EventEmitter<void>();



@Output() updated =
new EventEmitter<void>();






model={


name:'',

startDate:'',

endDate:'',

isActive:true


};





loading=false;





constructor(

private service:AcademicYearService

){}







ngOnChanges(changes:SimpleChanges){


if(
changes['academicYearId']
&& this.academicYearId
){

this.loadData();

}


}








loadData(){


this.loading=true;



this.service
.getById(this.academicYearId)

.subscribe({



next:(res)=>{



this.model={


name:res.name,



startDate:
res.startDate
?
new Date(res.startDate)
.toISOString()
.substring(0,10)
:
'',



endDate:
res.endDate
?
new Date(res.endDate)
.toISOString()
.substring(0,10)
:
'',



isActive:res.isActive


};




this.loading=false;



},




error:(err)=>{


console.error(
'Failed loading academic year',
err
);


this.loading=false;



}



});



}









update(){



if(!this.model.name){


alert(
'Academic year name required'
);


return;


}




if(
!this.model.startDate ||
!this.model.endDate
){


alert(
'Please select dates'
);


return;


}







if(
this.model.endDate <=
this.model.startDate
){


alert(
'End date must be greater than start date'
);


return;


}






this.loading=true;






this.service.update(

this.academicYearId,

this.model

)

.subscribe({



next:()=>{


this.loading=false;



this.updated.emit();


this.close.emit();



},




error:(err)=>{


console.error(
'Update failed',
err
);


this.loading=false;


}



});



}








cancel(){


this.close.emit();


}



}