import { Component, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { jwtDecode } from 'jwt-decode';
import { DataService } from '../../common.service';
@Component({
  selector: 'app-reset-password',
  templateUrl: './reset-password.component.html',
  styleUrls: ['./reset-password.component.scss'],
})
export class ResetPasswordComponent implements OnInit {
  token!: string;
  expirationDate!: Date;
  email!: string;
  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private service: DataService
  ) {}
  newPassword: string = '';
  confirmPassword: string = '';
  error: string = ''; // No initial error message

  ngOnInit(): void {
    console.log('hi');
    this.route.queryParams.subscribe((params) => {
      this.token = params['token'];
      console.log('Token:', this.token);
      this.email = this.getEmailFromToken(this.token);
      const [, payloadBase64] = this.token.split('.');
      const payload = JSON.parse(atob(payloadBase64));

      // Extract and convert the expiration timestamp to a Date object
      const expirationTimestamp = payload.exp;
      this.expirationDate = new Date(expirationTimestamp * 1000);
      console.log(this.expirationDate);
      // Check if the token is expired
      if (this.expirationDate.getTime() < Date.now()) {
        // Token expired, redirect to an error page or show a message
        alert('Link Expired');
      }
    });
    // Initialize component properties or perform any necessary setup here
  }

  resetPassword(form: NgForm) {
    console.log('hi');
    console.log(this.newPassword);
    console.log(this.confirmPassword);
    if (this.newPassword !== this.confirmPassword) {
      alert('Password should match confirm password');
    } else {
      this.updateJob();
      // Passwords match, perform reset password action
      // For example, call your API to reset the password
      // You can also reset the form or clear the input fields here
    }
  }
  getEmailFromToken(token: string): string {
    try {
      const decodedToken: any = jwtDecode(token);
      console.log('In Decode', decodedToken);
      return decodedToken.Email;
    } catch (error) {
      console.error('Error decoding token:', error);
      return '';
    }
  }
  updateJob() {
    console.log('In Update Job', this.newPassword);
    this.service.updatePassword(this.email, this.newPassword).subscribe({
      next: (data) => {
        console.log('In Add Update User');
        alert('Updated');
        // this.router.navigate(['/job-list'])
        console.log(data);
      },
      error: (err) => {
        if (err.status === 404) {
          console.error('Resource not found:', err);
          // Display a user-friendly error message to the user
          // You can also navigate to a different route or perform other actions here
        } else {
          alert('Password Changed');
          this.router.navigate(['admin/login']);
          console.error('An error occurred:', err);
          // Handle other types of errors here
        }
      },
    });
  }
}
