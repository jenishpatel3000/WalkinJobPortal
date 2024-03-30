import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, Router, RouterStateSnapshot, UrlTree } from '@angular/router';
import { Observable } from 'rxjs';
import { AuthService } from './auth.service';

@Injectable({
  providedIn: 'root'
})

export class AuthGuard implements CanActivate {
  constructor(private authservice:AuthService,private router:Router)
  {

  }
  canActivate(
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot): Observable<boolean | UrlTree> | Promise<boolean | UrlTree> | boolean | UrlTree {
      console.log("AuthGuard",this.authservice.isLoggedIn())
      const ans=this.authservice.isLoggedIn();
      if(this.authservice.isLoggedIn())
      {
        console.log("In if");
    return true;
      }
  else{
    console.log("In else");
    this.router.navigate(['/login']);
    return false;
  }
  
}
}