import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { DataService } from '../../common.service';
import { Router } from '@angular/router';
//  import { CommonService } from '../common.service';

@Component({
  selector: 'app-job',
  templateUrl: './job.component.html',
  styleUrls: ['./job.component.scss'],
})
export class JobComponent implements OnInit {
  jobRoles: any[] = [];
  timeSlots: any[] = [];

  userForm: FormGroup;
  constructor(
    public fb: FormBuilder,
    private service: DataService,
    private router: Router
  ) {
    this.userForm = this.fb.group({
      jobName: [''],
      jobTitle: [''],
      jobDescription: [''],
      jobRole: [''],
      roleTitle: [''],
      roleDescription: [''],
      startDate: [''],
      endDate: [''],
      venue: [''],
      thingsToRemember: [''],
      timeSlots: [''],
      package: [''],
    });
  }
  showJobDetails = false;
  showJobRoleDetails = false;
  newJob: any = {};

  toggleJobDetails() {
    this.showJobDetails = !this.showJobDetails;
  }

  toggleJobRoleDetails() {
    this.showJobRoleDetails = !this.showJobRoleDetails;
  }

  addJob() {
    // Implement your logic to add the job here
    console.log('New Job:', this.newJob);
    // Reset the form after adding the job
    this.newJob = {};
    this.showJobDetails = false;
    this.showJobRoleDetails = false;
  }
  showTitleAndDescription = {
    job: false,
    role: false,
  };

  toggleTitleAndDescription(type: string) {
    if (type === 'job') {
      this.showTitleAndDescription.job = !this.showTitleAndDescription.job;
    } else if (type === 'role') {
      this.showTitleAndDescription.role = !this.showTitleAndDescription.role;
    }
  }

  SubmitForm() {
    console.log("This form",this.userForm.value);
    this.service.AddUpdateUser(this.userForm.value).subscribe({
      next: (data) => {
        console.log('In Add Update User');
        // alert('Added');
        console.log(data);
      },
      error: (err) => {
        if (err.status === 404) {
          
          this.router.navigate(['admin/job-list']);
          console.error('Resource not found:', err);
          // Display a user-friendly error message to the user
          // You can also navigate to a different route or perform other actions here
        } else {
          console.error('An error occurred:', err);
          // Handle other types of errors here
        }
      },
    });
  }
  getJobRoles(){
    this.service.getRoles().subscribe((data) => {
      this.jobRoles = data;
      console.log("job roles",this.jobRoles); 
    });
    

  }
  getSLots(){
    this.service.getSlots().subscribe((data) => {
      this.timeSlots = data;
      console.log("Time slots",this.timeSlots); 
    });
    

  }

  ngOnInit(): void {
    this.getJobRoles();
    this.getSLots();
    
  }
}
