import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

import { RegistrationRoutingModule } from './registration-routing.module';

import { QualificationsModule } from './qualifications/qualifications.module';

import { CreateAccountBarComponent } from './create-account-bar/create-account-bar.component';
import { ProgressBarComponent } from './progress-bar/progress-bar.component';
import { PersonalInfoComponent } from './personal-info/personal-info.component';
import { RegistrationComponent } from './registration.component';
import { ReviewComponent } from './review/review.component';
// import { userData } from './userRegistrationData';
@NgModule({
  declarations: [
    CreateAccountBarComponent,
    ProgressBarComponent,
    PersonalInfoComponent,
    RegistrationComponent,
    ReviewComponent,
  ],
  imports: [
    CommonModule,
    QualificationsModule,
    FormsModule,
    RegistrationRoutingModule,
  ],
  exports: [
    CreateAccountBarComponent,
    ProgressBarComponent,
    PersonalInfoComponent,
    ReviewComponent,
    RegistrationComponent,
  ],
})
export class RegistrationModule {}
