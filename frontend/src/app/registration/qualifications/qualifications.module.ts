import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

import { QualificationsComponent } from './qualifications.component';
@NgModule({
  declarations: [
    QualificationsComponent,
  ],
  imports: [CommonModule, FormsModule],
  exports: [
    QualificationsComponent,
  ],
})
export class QualificationsModule {}
