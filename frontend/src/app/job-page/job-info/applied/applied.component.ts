import { Component, Input, OnInit } from '@angular/core';

import { DataService } from 'src/app/shared/data.service';
import { ApplicationService } from 'src/app/shared/application.service';
import { ActivatedRoute, Router } from '@angular/router';
import { IJobData, IApplicationInfo } from '../../JobData';
import { catchError } from 'rxjs/operators';
import { throwError } from 'rxjs';
import { IApplication } from 'src/interface/interfaces';
import { baseurl } from 'src/app/shared/url';
import { HttpClient } from '@angular/common/http';
import { Time } from '@angular/common';
@Component({
  selector: 'app-applied',
  templateUrl: './applied.component.html',
  styleUrls: ['./applied.component.scss'],
})
export class AppliedComponent implements OnInit {

  application: IApplication = {
    applicationId: 0,
    resume: '',
    userId: 0,
    jobId: 0,
    slotId: 0,
    hallticket: '',
    job: {
      jobId: 0,
      jobName: '',
      fromTime: new Date,
      toTime: new Date,
      venue: '',
      thingsToRemember: '',
      date: new Date
    },
    slot: {
      slotId: 0,
      fromTime: {
        hours: 0,
        minutes: 0
      },
      toTime: {
        hours: 0,
        minutes: 0
      }
    },
    applicationRoles: null
  };
  applicationId: number | undefined;

  constructor(
    private http: HttpClient,
    private route: ActivatedRoute,
    private dataService: DataService,
    private ApplicationService: ApplicationService
  ) { }
  applicationdetails: string = '';

  fetchData() {
    this.http
      .get<IApplication>(`${baseurl}/getapplication/${this.applicationId}`)
      .pipe(
        catchError((error) => {
          console.error('There was a problem with the fetch operation:', error);
          return throwError(error);
        })
      )
      .subscribe((data) => {
        this.application = data;
      });
  }
  ngOnInit(): void {
    let id: string | null;
    this.route.paramMap.subscribe(params => {
      id = params.get('applicationid');
      if (id) {
        this.applicationId = +id;
      }
    });
    this.fetchData();
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
    return formattedDateString;
  }

  convertTo12HourFormat(time24: Time): string {
    var timeString = time24.toString();
    timeString = timeString.substring(0, 5);
    return timeString;
  }

  downloadHallTicket() {
    if (this.application.hallticket) {
      var uint8Array = Uint8Array.from(atob(this.application.hallticket), char => char.charCodeAt(0));
      var pdfBlob = new Blob([uint8Array], { type: 'application/pdf' });
      var blobUrl = URL.createObjectURL(pdfBlob);
      var link = document.createElement('a');
      link.href = blobUrl;
      link.download = `hallticket_${this.application.applicationId}.pdf`;
      link.click();
      URL.revokeObjectURL(blobUrl);
    } else {
      console.error('Hallticket is empty');
    }
  }
  
}
