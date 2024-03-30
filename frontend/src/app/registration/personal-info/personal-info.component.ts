import { Component, OnInit, EventEmitter, Output, Input } from '@angular/core';
import { Role, UserRegistrationRequest } from 'src/interface/interfaces';
@Component({
  selector: 'app-personal-info',
  templateUrl: './personal-info.component.html',
  styleUrls: ['./personal-info.component.scss']
})
export class PersonalInfoComponent implements OnInit {
  selectedRoles: string[] = [];

  @Output() userDataUpdated = new EventEmitter<any>();
  @Input() roles: Role[] = [];
  @Input() userData!: UserRegistrationRequest;
  @Input() validations: any;

  updateUserData(): void {
    this.userDataUpdated.emit(this.userData);
    console.log(this.userData);
  }
  constructor() {}

  ngOnInit(): void {

  }

  toggleJobRole(role: Role): void {
    const i = this.userData.rolesId.indexOf(role.roleId);
    if(i>-1){
      this.userData.rolesId.splice(i, 1);
    }
    else{
      this.userData.rolesId.push(role.roleId);
    }
  }
  input = document.querySelector('#resume-input');
  inputFile: any;

  uploadResume(event: any): void {
    this.inputFile = event.target.files[0];
    if(this.inputFile){
      const blob = new Blob([this.inputFile],{type: this.inputFile.type});

      const reader = new FileReader();
      reader.readAsDataURL(blob);

      reader.onload = async (e: any) => {
        this.userData.resume = await e.target.result;
        console.log(this.userData.resume);
      };
    }
    this.userData.resumeFileName = this.inputFile ? this.inputFile.name : '';
  }

  photoinput = document.querySelector('.photo-input');
  inputPhoto: any;
  profilePhotoSrc: string = '../../../assets/images/default-profile-photo.png';

  uploadProfilePhoto(event: any): void {
    this.inputPhoto = event.target.files[0];
    if (this.inputPhoto) {
      const blob = new Blob([this.inputPhoto],{type: this.inputPhoto.type});

      const reader = new FileReader();
      reader.readAsDataURL(blob);

      reader.onload = async (e: any) => {
        this.userData.profilePhoto = await e.target.result;
        this.profilePhotoSrc = this.userData.profilePhoto ? this.userData.profilePhoto : this.profilePhotoSrc;
      };
    }
  }
}
