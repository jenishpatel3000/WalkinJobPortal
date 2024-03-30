import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { catchError } from 'rxjs/operators';
import { throwError } from 'rxjs';
import { ActivatedRoute, Router } from '@angular/router';
import { DataService } from 'src/app/shared/data.service';
import { IApplicationRequest, IJob } from 'src/interface/interfaces';
import { baseurl } from 'src/app/shared/url';
import { Time } from '@angular/common';

@Component({
  selector: 'app-job-info',
  templateUrl: './job-info.component.html',
  styleUrls: ['./job-info.component.scss'],
})
export class JobInfoComponent implements OnInit {
  slotRequired: boolean = false;
  rolesRequired: boolean = false;
  Job: IJob = {
    jobId: 0,
    jobName: '',
    fromTime: new Date(),
    toTime: new Date(),
    venue: '',
    thingsToRemember: '',
    locationId: 0,
    special_message: '',
    location: {
      locationId: 0,
      locationName: ''
    },
    jobRoles: [],
    jobDescs: [],
    jobSlots: []
  };
  applicationId: number | undefined;
  application: IApplicationRequest = {
    resume: null,
    userId: 0,
    jobId: 0,
    slotId: 0,
    rolesid: []
  };
  remainingDays: number = 99;
  formData: FormData = new FormData();

  constructor(
    private http: HttpClient,
    private router: Router,
    private route: ActivatedRoute
  ) { }
  emitData() {
    this.slotRequired = this.application.slotId !== 0 ? false : true;
    this.rolesRequired = this.application.rolesid.length > 0 ? false : true;

    if (!this.slotRequired && !this.rolesRequired) {
      this.http
        .post<number>(`${baseurl}/apply`, this.application)
        .pipe(
          catchError((error) => {
            console.error('There was a problem with the apply operation:', error);
            return throwError(error);
          })
        )
        .subscribe((data) => {
          console.log(data);
          this.router.navigate([`/jobs/${this.jobid}/application/${data}`])
        });
    }
  }
  fetchData(jobid: number): void {
    this.http
      .get<IJob>(`${baseurl}/job/${jobid}`)
      .pipe(
        catchError((error) => {
          console.error('There was a problem with the fetch operation:', error);
          return throwError(error);
        })
      )
      .subscribe((data) => {
        this.Job = data;
        this.calculateRemainingDays();
      });
  }
  jobid: any;
  ngOnInit(): void {
    const id = this.route.snapshot.paramMap.get('id');
    if (id !== null) {
        this.jobid = +id;
        const userIdFromLocalStorage = localStorage.getItem('userId');

        if (userIdFromLocalStorage !== null) {
            this.application = {
                resume: '',
                jobId: this.jobid,
                slotId: 0,
                userId: +userIdFromLocalStorage,
                rolesid: []
            };
        } else {
            this.application.userId = 1;
        }
    } else {
        this.jobid = 1;
        this.application.jobId = 1;
    }
    this.fetchData(this.jobid);
}


  calculateRemainingDays(): void {
    const currentDate = new Date();
    if (this.Job?.toTime) {
      const toTime = new Date(this.Job.toTime);
      const timeDifference = toTime.getTime() - currentDate.getTime();
      this.remainingDays = Math.floor(timeDifference / (1000 * 3600 * 24));
    }
  }

  isNotExpanded = false;
  maxHeight: number = 0;

  toggleExpansion() {
    this.isNotExpanded = !this.isNotExpanded;
    if (this.isNotExpanded) {
      this.maxHeight = 0;
    } else {
      const contentElement = document.querySelector('.job-pre-requisite');
      this.maxHeight = contentElement ? contentElement.scrollHeight : 0;
    }
  }

  toggleJobRole(role: number): void {
    this.application.rolesid.push(role);
  }

  toggleSlot(slot: number) {
    this.application.slotId = slot;
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

  input = document.querySelector('#resume-input');
  fileName = document.querySelector('.file-name');
  inputFile: any;

  uploadResume(event: any): void {
    this.inputFile = event.target.files[0];

    if (this.inputFile) {
      const reader = new FileReader();
      reader.onloadend = (e: any) => {
        this.application.resume = e.target.result as string;
      };
      reader.readAsDataURL(this.inputFile);
    }

    this.fileName = this.inputFile ? this.inputFile.name : '';
  }

}