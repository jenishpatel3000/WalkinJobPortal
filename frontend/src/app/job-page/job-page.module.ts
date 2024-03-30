import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

import { JobPageRoutingModule } from './job-page-routing.module';

import { JobPageComponent } from './job-page.component';
import { JobInfoComponent } from './job-info/job-info.component';
import { RolesOfferedComponent } from './job-info/roles-offered/roles-offered.component';
import { AppliedComponent } from './job-info/applied/applied.component';

@NgModule({
  declarations: [
    JobPageComponent,
    JobInfoComponent,
    RolesOfferedComponent,
    AppliedComponent,
  ],
  imports: [CommonModule, JobPageRoutingModule, FormsModule],
  exports: [
    JobPageComponent,
    JobInfoComponent,
    RolesOfferedComponent,
    AppliedComponent,
  ],
})
export class JobModule {}
