import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { AuthService } from '../../auth.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.scss'],
})
export class HeaderComponent implements OnInit {
  @ViewChild('addEmployeeTab') addEmployeeTab!: ElementRef;
  @ViewChild('jobListTab') jobListTab!: ElementRef;
  constructor(private authservice: AuthService, private router: Router) {}
  isLoggedIn(): boolean {
    return this.authservice.isLoggedIn(); // Call the isLoggedIn method from the AuthService
  }
  logout(): void {
    // Remove token from localStorage
    localStorage.removeItem('access_token');
    
    // Redirect to the login page
    this.router.navigate(['/login']);
  }

  ngOnInit(): void {}
  // selectTab(tabRef: ElementRef) {
  //   const tab: HTMLElement = tabRef.nativeElement as HTMLElement;
  //   // Remove 'active' class from all tabs
  //   document.querySelectorAll('.nav-link').forEach(el => {
  //     el.classList.remove('active');
  //   });

  //   // Add 'active' class to the clicked tab
  //   tab.classList.add('active');
  // }
}
