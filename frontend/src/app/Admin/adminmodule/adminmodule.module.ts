import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AddjobComponent } from './addjob/addjob.component';



@NgModule({
  declarations: [
    AddjobComponent
  ],
  imports: [
    CommonModule
  ],
  exports:[
    AddjobComponent
  ]
})
export class AdminmoduleModule { }
