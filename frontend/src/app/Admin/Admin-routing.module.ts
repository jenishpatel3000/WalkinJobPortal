import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { JobComponent } from './job/job.component';
import { JoblistComponent } from './joblist/joblist.component';
import { EditFormComponent } from './edit-form/edit-form.component';
import { LoginComponent } from './AdminLogin/AdminLogin.component';
import { AuthGuard } from '../auth.guard';
import { ForgotPasswordComponent } from './forgot-password/forgot-password.component';
import { ResetPasswordComponent } from './reset-password/reset-password.component';
import { ApplicationReviewComponent } from './application-review/application-review.component';

const routes: Routes = [
  { path: 'admin', component: LoginComponent }, // Redirect to login initially
  { path: 'admin/add-job', component: JobComponent, canActivate: [AuthGuard] },
  {
    path: 'admin/job-list',
    component: JoblistComponent,
  },
  // {
  //   path: 'admin/job-list',
  //   component: JoblistComponent,
  //   canActivate: [AuthGuard],
  // },
  {
    path: 'admin/edit-job/:id',
    component: EditFormComponent,
  },
  // {
  //   path: 'admin/edit-job/:id',
  //   component: EditFormComponent,
  //   canActivate: [AuthGuard],
  // },
  { path: 'forgot-password', component: ForgotPasswordComponent },
  { path: 'reset-password', component: ResetPasswordComponent },
  {
    path: 'admin/application',
    component: ApplicationReviewComponent,
  },
  // {
  //   path: 'admin/application',
  //   component: ApplicationReviewComponent,
  //   canActivate: [AuthGuard],
  // },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class AdminRoutingModule {}
