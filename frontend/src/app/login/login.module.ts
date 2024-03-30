import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { LoginFormComponent } from './login-form/login-form.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { LoginRoutingModule } from './login-routing.module';
@NgModule({
  declarations: [LoginFormComponent],
  imports: [CommonModule, LoginRoutingModule, FormsModule, ReactiveFormsModule],
  exports: [LoginFormComponent],
})
export class LoginModule {}
