import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class DataService {
readonly url="https://localhost:7232"

  constructor(private http: HttpClient) { }
  AddUpdateUser(data:any):Observable<any>{
    return this.http.post('https://localhost:7232/AddJob',data)
  }
  

  

  getJobs(): Observable<any[]> {
    return this.http.get<any[]>('https://localhost:7232/jobs');
  }
  getApplications(): Observable<any[]> {
    return this.http.get<any[]>('https://localhost:7232/getApplications');
  }
  DeleteUserByID(ID:any):Observable<any>{
    return this.http.delete<any[]>('https://localhost:7232/Delete'+'/'+ID);
  }
  GetUserByID(ID:any):Observable<any>{
    return this.http.get<any[]>('https://localhost:7232/Get'+'/'+ID);
  }
  updateJob(ID:any,data:any):Observable<any>{
    console.log("In common service updateJob ",ID,data)
    return this.http.put<any>('https://localhost:7232/Update'+'/' + ID, data);
  }
  sendResetEmail(Email:any):Observable<any>
  {
    console.log("In CommonService Email",Email);
    return this.http.post('https://localhost:7232/forgot-password',{Email});
  }
  sendAcceptanceEmail(Email:any,userName:any):Observable<any>
  {
    console.log("In CommonService Email",Email,userName);
    return this.http.post('https://localhost:7232/select-application',{Email,userName});
  }
  sendRejectEmail(Email:any,userName:any):Observable<any>
  {
    console.log("In CommonService Email",Email,userName);
    return this.http.post('https://localhost:7232/reject-application',{Email,userName});
  }
  updatePassword(Email:string,data:string):Observable<any>{
    console.log("In common service updateJob ",Email,data)
    const requestBody = {
      email: Email,
      newPassword: data
    };
    console.log("RequestBody",requestBody)
    return this.http.put<any>('https://localhost:7232/reset-password', requestBody);
  }
}
