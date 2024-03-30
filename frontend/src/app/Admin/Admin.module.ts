import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AdminRoutingModule } from './Admin-routing.module';
import { AdminmoduleModule } from './adminmodule/adminmodule.module';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { HeaderComponent } from './header/header.component';
import { JobComponent } from './job/job.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { JoblistComponent } from './joblist/joblist.component';
import { EditFormComponent } from './edit-form/edit-form.component';
import { LoginComponent } from './AdminLogin/AdminLogin.component';
import { ForgotPasswordComponent } from './forgot-password/forgot-password.component';
import { ResetPasswordComponent } from './reset-password/reset-password.component';
import { ApplicationReviewComponent } from './application-review/application-review.component';

@NgModule({
  declarations: [
    HeaderComponent,
    JobComponent,
    JoblistComponent,
    EditFormComponent,
    LoginComponent,
    ForgotPasswordComponent,
    ResetPasswordComponent,
    ApplicationReviewComponent,
  ],
  imports: [
    BrowserModule,
    AdminRoutingModule,
    AdminmoduleModule,
    MatToolbarModule,
    MatIconModule,
    MatButtonModule,
    ReactiveFormsModule,
    HttpClientModule,
    FormsModule,

    ReactiveFormsModule,
  ],
})
export class Admin {}
