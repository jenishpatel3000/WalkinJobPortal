import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { catchError, throwError } from 'rxjs';
import { LoginResponse } from 'src/interface';

@Component({
  selector: 'app-login',
  templateUrl: './AdminLogin.component.html',
  styleUrls: ['./SCSS/style.scss']
})
export class LoginComponent implements OnInit {

  passwordFieldType: string = 'password';
  emailRegex = /[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}/;
  emailInalid: boolean = false;
  emailRequired: boolean = false;
  passwordRequired: boolean = false;
  loginData = {
    username: '',
    password: '',
    rememberMe: false,
  };

  constructor(private router: Router, private http: HttpClient) {
  }

  ngOnInit(): void {

  }

  login() {
    console.log(this.loginData);
    if (this.loginData.username.trim() == '') {
      this.emailRequired = true;
    }
    else {
      this.emailRequired = false;
    }
    if (this.emailRegex.test(this.loginData.username.trim()) == false) {
      this.emailInalid = true;
    }
    else {
      this.emailInalid = false;
    }
    if (this.loginData.password.trim() == '') {
      this.passwordRequired = true;
    }
    else {
      this.passwordRequired = false;
    }
    if (!this.emailInalid && !this.emailRequired && !this.passwordRequired) {
      console.log(this.loginData)
      this.http
        .post<LoginResponse>(`https://localhost:7232/Login`, this.loginData)
        .pipe(
          catchError((error) => {
            alert("Invalid User");
            console.error('There was a problem with the fetch operation:', error);
            return throwError(error);
          })
        )
        .subscribe((data:LoginResponse) => {
          

          console.log(data)
          this.setToken(data.token);
          this.router.navigate(['/job-list']);
        });
    }
    
    
  }
  setToken(token:string)
  {
    localStorage.setItem("access_token",token)
  }

  onRememberMeChange(event: Event): void {
    this.loginData.rememberMe = (event.target as HTMLInputElement).checked;
  }

  togglePassword(): void {
    this.passwordFieldType =
      this.passwordFieldType === 'password' ? 'text' : 'password';
  }
}
