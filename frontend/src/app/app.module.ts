import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { HttpClientModule } from '@angular/common/http';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { HeaderComponent } from './header/header.component';
import { ContactComponent } from './contact/contact.component';

import { LoginModule } from './login/login.module';
import { Admin } from './Admin/Admin.module';
import { RegistrationModule } from './registration/registration.module';
import { JobModule } from './job-page/job-page.module';
import { AdminHeaderComponent } from './Admin/adminheader/header.component';
@NgModule({
  declarations: [AppComponent, HeaderComponent, AdminHeaderComponent, ContactComponent, ],
  imports: [
    BrowserModule,
    HttpClientModule,
    AppRoutingModule,
    LoginModule,
    RegistrationModule,
    JobModule,
    Admin,
  ],
  providers: [],
  bootstrap: [AppComponent],
})
export class AppModule {}
