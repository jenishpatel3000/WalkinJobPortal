import { Component, OnInit } from '@angular/core';
import { DataService } from '../../common.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-forgot-password',
  templateUrl: './forgot-password.component.html',
  styleUrls: ['./forgot-password.component.scss'],
})
export class ForgotPasswordComponent implements OnInit {
  email!: string;

  constructor(private service: DataService, private router: Router) {}

  ngOnInit(): void {}
  resetPassword() {
    console.log(this.email);
    this.service.sendResetEmail(this.email).subscribe((response) => {
      console.log(response);
      if (response.message == 'Password reset email sent successfully') {
        // Reset form and show success message

        alert('Password reset email sent successfully');
      } else {
        // Handle other responses
        alert('Invalid User ID');
        this.router.navigate(['/forgot-password']);
      }
    });
  }
}
