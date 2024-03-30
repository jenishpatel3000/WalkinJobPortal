import { Component } from '@angular/core';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent {
  title = 'frontend';

  hasUserId(): boolean {
    return window.localStorage.getItem('userId') !== null;
  }

  hasAdminId(): boolean {
    return window.localStorage.getItem('adminId') !== null;
  }
}
