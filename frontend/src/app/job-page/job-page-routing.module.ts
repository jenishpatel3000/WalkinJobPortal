import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { JobPageComponent } from './job-page.component';
import { JobInfoComponent } from './job-info/job-info.component';
import { AppliedComponent } from './job-info/applied/applied.component';
import { AuthGuard } from '../shared/auth.guard';

const routes: Routes = [
  { path: 'jobs', component: JobPageComponent,canActivate:[AuthGuard] },
  { path: 'jobs/:id', component: JobInfoComponent,canActivate:[AuthGuard] },
  { path: 'jobs/:id/application/:applicationid', component: AppliedComponent,canActivate:[AuthGuard] },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class JobPageRoutingModule {}
