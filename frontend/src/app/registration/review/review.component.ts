import { Component, OnInit, Input, EventEmitter, Output } from '@angular/core';
import { RegistrationData, UserRegistrationRequest } from 'src/interface/interfaces';

@Component({
  selector: 'app-review',
  templateUrl: './review.component.html',
  styleUrls: ['./SCSS/style.scss'],
})
export class ReviewComponent implements OnInit {
  @Input() userData!: UserRegistrationRequest;
  @Input() registrationData!: RegistrationData;

  fileName: string = '';
  profilePhotoSrc: string = '../../../assets/images/default-profile-photo.png';
  constructor() { }

  uploadResume(event: any): void {
    const inputFile = event.target.files[0];
    console.log(inputFile);
    this.fileName = inputFile ? inputFile.name : '';
  }

  ngOnInit(): void {

    if (
      this.userData
    ) {
      const applicantType =
        this.userData.applicationTypeId;
      console.log('ApplicantType:', applicantType);

      if (applicantType === 1) {
        this.showFresherQualifications();
      } else if (applicantType === 2) {
        this.showProfessionalQualifications();
      } else {
        this.hideAllQualifications();
      }
    }

    this.showPhoto();
  }
  showPhoto() {
    this.userData.profilePhoto;
    if (this.userData.profilePhoto) {
      this.profilePhotoSrc = this.userData.profilePhoto;
    }
  }

  getRoleName(roleId: number): string {
    const role = this.registrationData.role.find(role => role.roleId == roleId);
    return role ? role.roleName : 'Unknown Role';
  }

  getCollegeName(collegeId: number): string {
    console.log(this.registrationData)
    const college = this.registrationData.college.find(college => college.collegeId == collegeId);
    return college ? college.collegeName : 'Unknown Role';
  }

  getStreamName(streamId: number): string {
    const stream = this.registrationData.stream.find(stream => stream.streamId == streamId);
    return stream ? stream.streamName : 'Unknown Role';
  }

  getTechName(techId: number): string {
    const tech = this.registrationData.tech.find(tech => tech.techId == techId);
    return tech ? tech.techName : 'Unknown Role';
  }

  getApplicationTypeName(id: number): string{
    const applicationTypes = this.registrationData.applicationTypes.find(applicationTypes => applicationTypes.applicationTypeId == id);
    return applicationTypes ? applicationTypes.applicationTypeName : 'Unknown Role';
  }

  getQualificationName(qualificationId: number): string{
    const qualification = this.registrationData.qualification.find(qualification => qualification.qualificationId == qualificationId);
    return qualification ? qualification.qualificationName : 'Unknown Role';
  }

  private showFresherQualifications(): void {
    document.querySelector('.proqualifications')?.classList.add('hidden');
    document.querySelector('.fresherqualifications')?.classList.remove('hidden');
  }

  private showProfessionalQualifications(): void {
    document.querySelector('.proqualifications')?.classList.remove('hidden');
    document.querySelector('.fresherqualifications')?.classList.add('hidden');
  }

  private hideAllQualifications(): void {
    document.querySelector('.proqualifications')?.classList.add('hidden');
    document.querySelector('.fresherqualifications')?.classList.add('hidden');
  }
}
