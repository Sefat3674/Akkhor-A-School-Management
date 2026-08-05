import { Component, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { AuthService } from '../../../core/services/auth.service';



@Component({
 selector:'app-register',
 standalone:true,
 imports:[
  CommonModule,
  ReactiveFormsModule,
  RouterLink
 ],
 templateUrl:'./register.component.html'
})
export class RegisterComponent {



form = this.fb.group({

 fullName:[
  '',
  Validators.required
 ],


 email:[
  '',
  [
   Validators.required,
   Validators.email
  ]
 ],


 password:[
  '',
  [
   Validators.required,
   Validators.minLength(8)
  ]
 ]


});



loading = signal(false);

errorMessage = signal<string|null>(null);



constructor(
 private fb:FormBuilder,
 private auth:AuthService,
 private router:Router
){}



submit(){


if(this.form.invalid)
{
 this.form.markAllAsTouched();
 return;
}


this.loading.set(true);

this.errorMessage.set(null);



this.auth.register(
 this.form.value as any
)
.subscribe({

next:(res)=>{


this.loading.set(false);


this.router.navigate([
 '/login'
]);


},


error:(err)=>{


this.loading.set(false);


this.errorMessage.set(
 err?.error?.message ??
 err?.error?.errors?.join(', ') ??
 "Registration failed"
);


}


});

}


}