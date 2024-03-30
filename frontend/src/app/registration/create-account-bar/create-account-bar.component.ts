import { HttpClient } from '@angular/common/http';
import { Component, Input, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { UserRegistrationRequest } from 'src/interface/interfaces';
import { userData } from '../userRegistrationData';
import { baseurl } from 'src/app/shared/url';
import { catchError, throwError } from 'rxjs';

@Component({
  selector: 'app-create-account-bar',
  templateUrl: './create-account-bar.component.html',
  styleUrls: ['./SCSS/style.scss'],
})
export class CreateAccountBarComponent implements OnInit {
  @Input() createshow: boolean = false;
  @Input() userData!: UserRegistrationRequest;
  
  constructor(private router: Router,
    private http: HttpClient
  ) {}

  ngOnInit(): void {}

  // Add a method to dynamically set the class based on the reviewShow status
  getCreateButtonClass() {
    return this.createshow
      ? 'create-account-button create-button active'
      : 'create-account-button create-button inactive';
  }
  onCreateClick() {
    this.http
      .post(`${baseurl}/user`, this.userData)
      .pipe(
        catchError((error) => {
          console.error('There was a problem with the fetch operation:', error);
          return throwError(error);
        })
      )
      .subscribe((data) => {
        if (this.createshow) this.router.navigate(['/login']);
      });
  }
}
