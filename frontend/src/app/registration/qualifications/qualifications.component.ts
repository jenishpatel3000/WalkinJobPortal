import { registerLocaleData } from '@angular/common';
import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { RegistrationData, Tech, UserRegistrationRequest } from 'src/interface/interfaces';

@Component({
  selector: 'app-qualifications',
  templateUrl: './qualifications.component.html',
  styleUrls: ['./SCSS/style.scss'],
})
export class QualificationsComponent implements OnInit {
  @Output() userDataUpdated = new EventEmitter<any>();
  @Input() userData!: UserRegistrationRequest;
  @Input() registrationData!: RegistrationData;
  @Input() validations: any;

  selectedApplicantType: string = '';
  currentYear = new Date().getFullYear();
  yearsList = Array.from({ length: 51 }, (_, index) => this.currentYear - index);

  handleApplicantTypeChange(applicantType: string): void {
    this.selectedApplicantType = applicantType;
    this.updateComponentsVisibility();
  }
  
  updateComponentsVisibility(): void {
    const fresherQualifications = document.querySelector('.fresherqualifications') as HTMLInputElement;
    const proQualifications = document.querySelector('.proqualifications') as HTMLInputElement;
  
    if (this.selectedApplicantType === 'Fresher') {
      fresherQualifications.style.display = 'block';
      proQualifications.style.display = 'none';
    } else if (this.selectedApplicantType === 'Experienced') {
      fresherQualifications.style.display = 'none';
      proQualifications.style.display = 'block';
    } else {
      fresherQualifications.style.display = 'none';
      proQualifications.style.display = 'none';
    }
  }

  toggleFamiliarTech(tech: Tech): void {
    const i = this.userData.familiarTechsId.indexOf(tech.techId)
    if(i>-1){
      this.userData.familiarTechsId.splice(i,1);
    }
    else{
      this.userData.familiarTechsId.push(tech.techId);
    }
  }
  toggleExpertiseTech(tech: Tech): void {
    const i = this.userData.expertTechsId.indexOf(tech.techId)
    if(i>-1){
      this.userData.expertTechsId.splice(i,1);
    }
    else{
      this.userData.expertTechsId.push(tech.techId);
    }
  }

  constructor() { }
  updateUserData(): void {
    this.userDataUpdated.emit(this.userData);
    console.log(this.userData);
  }

  ngOnInit(): void {
    console.log(this.userData);
  }
}
