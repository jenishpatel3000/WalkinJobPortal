import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { JobPageComponent } from './job-page/job-page.component';
import { AuthGuard } from './shared/auth.guard';

const routes: Routes = [
  { path: '', pathMatch: 'full', redirectTo: '/login' },
  { path: 'registration', pathMatch: 'full', redirectTo: '/registration' },
  { path: 'admin', pathMatch: 'full', redirectTo: '/admin' },
  { path: 'jobs', component: JobPageComponent, canActivate: [AuthGuard] },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})
export class AppRoutingModule {}
