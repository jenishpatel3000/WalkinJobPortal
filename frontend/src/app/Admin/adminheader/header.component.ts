import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { AuthService } from '../../auth.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-admin-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.scss'],
})
export class AdminHeaderComponent implements OnInit {

  @ViewChild('addEmployeeTab') addEmployeeTab!: ElementRef;
  @ViewChild('jobListTab') jobListTab!: ElementRef;

  constructor(private authservice: AuthService, private router: Router) {}

  isLoggedIn(): boolean {
    return this.authservice.isLoggedIn();
  }
  
  logout(): void {
    localStorage.removeItem('access_token');
    this.router.navigate(['/login']);
  }

  ngOnInit(): void {}
}
