import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { IJobData, IApplicationInfo } from './JobData';
import { catchError } from 'rxjs/operators';
import { throwError } from 'rxjs';

import { DataService } from 'src/app/shared/data.service';
import { ApplicationService } from 'src/app/shared/application.service';
import { IJob } from 'src/interface/interfaces';
import { baseurl } from '../shared/url';

@Component({
  selector: 'app-job-page',
  templateUrl: './job-page.component.html',
  styleUrls: ['./SCSS/style.scss'],
})
export class JobPageComponent implements OnInit {

  Job: IJob[] = [];
  applicationArray: IApplicationInfo[] = [];
  remainingDays: number = 99;

  constructor(
    private http: HttpClient,
    private dataService: DataService,
    private ApplicationService: ApplicationService
  ) {}

  ngOnInit(): void {
    this.fetchData();
    this.dataService.data$.subscribe((data) => {
      this.applicationArray = this.applicationArray + data;
      // console.log(data);
      console.log(
        this.applicationArray +
          'jobpage application data stored in application array'
      );
    });
    // console.log(this.receivedData);
    this.emitData();
  }
  emitData() {
    this.ApplicationService.sendData(this.applicationArray);
  }

  fetchData(): void {
    const jwttoken=localStorage.getItem("access_token");
    
    console.log(jwttoken);
    this.http
      .get<IJob[]>(`${baseurl}/jobs`)
      .pipe(
        catchError((error) => {
          console.error('There was a problem with the fetch operation:', error);
          return throwError(error);
        })
      )
      .subscribe( (data) => {
        this.Job = data;
        this.calculateRemainingDays();
        // console.log(data);
      });
  }

  calculateRemainingDays(): void {
    const currentDate = new Date();

    this.Job.forEach((job) => {
      const toTime = new Date(job.toTime);
      const timeDifference = toTime.getTime() - currentDate.getTime();
      this.remainingDays = Math.floor(timeDifference / (1000 * 3600 * 24));
    });
  }

  formatdate(date: Date): string {
    const originalDate = new Date(date);
    const monthNames = [
      'Jan',
      'Feb',
      'Mar',
      'Apr',
      'May',
      'Jun',
      'Jul',
      'Aug',
      'Sep',
      'Oct',
      'Nov',
      'Dec',
    ];
    const day = originalDate.getDate();
    const month = monthNames[originalDate.getMonth()];
    const year = originalDate.getFullYear();
    const formattedDateString = `${day}-${month}-${year}`;
    // console.log(formattedDateString);
    return formattedDateString;
  }
}
