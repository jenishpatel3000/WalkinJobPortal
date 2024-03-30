import { Component, OnInit } from '@angular/core';
import { DataService } from '../../common.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-application-review',
  templateUrl: './application-review.component.html',
  styleUrls: ['./application-review.component.scss'],
})
export class ApplicationReviewComponent implements OnInit {
  startX: number = 0;
  isDragging: boolean = false;
  applications: any[] = [];
  currentIndex: number = 0;
  userName: string = '';
  userEmail: string = '';
  profilePic: string = '';
  constructor(private service: DataService, private router: Router) { }

  ngOnInit(): void {
    this.loadApplications();
  }

  handleMouseDown(event: MouseEvent) {
    this.isDragging = true;
    this.startX = event.clientX;
  }

  handleMouseUp() {
    this.isDragging = false;
  }

  handleMouseMove(event: MouseEvent) {
    const card = event.currentTarget as HTMLElement;
    if (this.isDragging && card) {
      const deltaX: number = event.clientX - this.startX;
      const direction: string = deltaX > 0 ? 'right' : 'left';
      const opacity: number = Math.abs(deltaX) / (card.clientWidth / 2);

      card.style.transform = `translateX(${deltaX}px)`;
      card.style.opacity = String(1 - opacity);

      if (Math.abs(deltaX) > card.clientWidth / 2) {
        if (direction === 'right') {
          console.log('Accepted');
        } else {
          console.log('Rejected');
        }
      }
    }
  }

  acceptCard() {
    this.userEmail = this.applications[this.currentIndex]?.email;
    this.userName = this.applications[this.currentIndex]?.name;
    console.log(this.userEmail, this.userName);
    this.service
      .sendAcceptanceEmail(this.userEmail, this.userName)
      .subscribe(() => {
        this.profilePic = this.applications[this.currentIndex].profilePhoto;
      });
    this.moveToNextApplication();
  }

  rejectCard() {
    this.userEmail = this.applications[this.currentIndex]?.email;
    this.userName = this.applications[this.currentIndex]?.name;
    console.log(this.userEmail, this.userName);
    this.service
      .sendRejectEmail(this.userEmail, this.userName)
      .subscribe(() => {
      });
    this.moveToNextApplication();
  }

  loadApplications() {
    this.service.getApplications().subscribe((data) => {
      this.applications = data;
      this.profilePic = data[this.currentIndex].profilePhoto;
      console.log(data);
    });
  }
  moveToNextApplication() {
    this.currentIndex++;
  }

  ViewResume() {
    const pdfData = Uint8Array.from(atob(this.applications[this.currentIndex].resume.substring(28,this.applications[this.currentIndex].resume.size)), c => c.charCodeAt(0));
    const pdfBlobObject = new Blob([pdfData], { type: 'application/pdf' });
    const pdfUrl = URL.createObjectURL(pdfBlobObject);
    window.open(pdfUrl, '_blank');
  }
}
