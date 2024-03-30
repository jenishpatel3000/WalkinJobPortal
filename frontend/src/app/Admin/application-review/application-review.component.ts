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
  constructor(private service: DataService, private router: Router) {}

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
          // Perform accept action
          // this.acceptCard(card);
        } else {
          console.log('Rejected');
          // Perform reject action
          // this.rejectCard(card);
        }
        // this.resetCard(card);
      }
    }
  }

  // resetCard(card: HTMLElement) {
  //   if (card) {
  //     card.style.transition = "transform 0.3s ease, opacity 0.3s ease";
  //     card.style.transform = "none";
  //     card.style.opacity = "1";

  //     setTimeout(() => {
  //       if (card) {
  //         card.style.transition = "none";
  //       }
  //     }, 300);
  //   }
  // }

  acceptCard() {
    this.userEmail = this.applications[this.currentIndex]?.email;
    this.userName = this.applications[this.currentIndex]?.name;
    console.log(this.userEmail, this.userName);
    this.service
      .sendAcceptanceEmail(this.userEmail, this.userName)
      .subscribe(() => {
        this.moveToNextApplication();
      });
    // Perform accept action
    // For example, you could remove the card from the UI
    // this.moveToNextApplication();
  }

  rejectCard() {
    // Perform reject action
    // For example, you could reset the card position
    this.userEmail = this.applications[this.currentIndex]?.email;
    this.userName = this.applications[this.currentIndex]?.name;
    console.log(this.userEmail, this.userName);
    this.service
      .sendRejectEmail(this.userEmail, this.userName)
      .subscribe(() => {
        this.moveToNextApplication();
      });
    this.moveToNextApplication();
    // this.resetCard(card);
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
    // Add logic to handle when there are no more applications left
  }
  ngOnInit(): void {
    this.loadApplications();
  }
}
