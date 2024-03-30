import { Component, OnInit } from '@angular/core';
import { DataService } from '../../common.service';
@Component({
  selector: 'app-joblist',
  templateUrl: './joblist.component.html',
  styleUrls: ['./joblist.component.scss'],
  providers: [DataService],
})
export class JoblistComponent implements OnInit {
  jobs: any[] = [];
  selectedJob: any = {};
  constructor(private service: DataService) {}

  ngOnInit(): void {
    // this.service.getJobs().subscribe((data) => {
    //   console.log(data);
    //   this.jobs = data;
    // });
  }
  GetAllUsers() {
    this.service.getJobs().subscribe((data) => {
      this.jobs = data;
    });
  }

  DeleteUserByID(ID: any) {
    console.log('ID', ID);
    this.service.DeleteUserByID(ID).subscribe((data) => {
      alert('User Deleted');
      this.GetAllUsers();
    });
  }
  GetUserByID(ID: any) {
    this.service.GetUserByID('1aaa').subscribe((data) => {
      alert('Get User');
      console.log('user detail', data);
      this.selectedJob = data;
      console.log('selectedJob', this.selectedJob);
    });
  }
}
