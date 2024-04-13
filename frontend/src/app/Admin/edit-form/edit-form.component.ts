import { Component, OnInit } from '@angular/core';
import { DataService } from '../../common.service';
import { ActivatedRoute, Router } from '@angular/router';
import { FormBuilder, FormGroup } from '@angular/forms';

@Component({
  selector: 'app-edit-form',
  templateUrl: './edit-form.component.html',
  styleUrls: ['./edit-form.component.scss'],
})
export class EditFormComponent implements OnInit {
  selectedJob: any = {};
  jobRoles: any[] = [];
  timeSlots1: any[] = [];
  userForm: FormGroup;
  jobId: any;
  constructor(
    private service: DataService,
    private route: ActivatedRoute,
    private fb: FormBuilder,
    private router: Router
  ) {
    this.userForm = this.fb.group({
      jobName: [''],
      jobTitle: [''],
      jobDescription: [''],
      jobRole: [''],

      startDate: [''],
      endDate: [''],
      venue: [''],
      thingsToRemember: [''],
      timeSlots: [''],
      package: [''],
    });
  }

  ngOnInit(): void {
   
    this.route.params.subscribe((params) => {
      this.jobId = params['id']; // Assuming the route parameter is 'id'
      console.log(this.jobId);
      this.GetUserByID(this.jobId);
    });
     this.populateForm();
     this.getJobRoles();
     this.getSLots();
  }
  GetUserByID(ID: any) {
    this.service.GetUserByID(ID).subscribe((data) => {
     
      this.populateForm();
    });
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
  updateJob() {
    const jobId = this.jobId;
    const updatedJobData = this.userForm.value;
    console.log('ID in update Job', this.jobId);
    console.log(updatedJobData);
    this.service.updateJob(jobId, updatedJobData).subscribe({
      next: (data) => {
        console.log('In Add Update User');
       
        this.router.navigate(['admin/job-list']);
        console.log(data);
      },
      error: (err) => {
        if (err.status === 404) {
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
  populateForm() {
    // Assuming selectedJob contains the data of the job to be edited
    console.log('In populateForm');
    this.userForm.patchValue({
      jobName: this.selectedJob.jobName,
      jobTitle: this.selectedJob.jobTitle,
      jobDescription: this.selectedJob.jobDescription,
      jobRole: this.selectedJob.jobRole,

      startDate: this.selectedJob.startDate,
      endDate: this.selectedJob.endDate,
      venue: this.selectedJob.venue,
      thingsToRemember: this.selectedJob.thingsToRemember,
      timeSlots: this.selectedJob.timeSlots,
      package: this.selectedJob.package,
    });
    console.log('Form Value:', this.userForm.value);
  }
  getJobRoles(){
    this.service.getRoles().subscribe((data) => {
      this.jobRoles = data;
      console.log("job roles",this.jobRoles); 
    });
    

  }
  getSLots(){
    this.service.getSlots().subscribe((data) => {
      this.timeSlots1 = data;
      console.log("Time slots",this.timeSlots1); 
    });
}
}
