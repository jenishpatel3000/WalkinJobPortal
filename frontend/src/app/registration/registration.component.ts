import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { ActivatedRoute, Router } from '@angular/router';
import {
  RegistrationData,
  UserRegistrationRequest,
} from 'src/interface/interfaces';
import { baseurl } from '../shared/url';
import { catchError, throwError } from 'rxjs';
@Component({
  selector: 'app-registration',
  templateUrl: './registration.component.html',
  styleUrls: ['./registration.component.scss'],
  encapsulation: ViewEncapsulation.None,
})
export class RegistrationComponent implements OnInit {
  personalShow: boolean = true;
  qualificationShow: boolean = false;
  reviewShow: boolean = false;
  registrationData: RegistrationData = {
    college: [],
    location: [],
    stream: [],
    qualification: [],
    tech: [],
    role: [],
    applicationTypes: [],
  };
  userData: UserRegistrationRequest = {
    email: '',
    firstName: '',
    lastName: '',
    phoneNo: null,
    portfolioUrl: '',
    referalEmpName: '',
    sendMeUpdate: null,
    userId: 0,
    countrycode: null,
    resume: null,
    profilePhoto: null,
    percentage: null,
    passingYear: null,
    qualificationId: 0,
    streamId: 0,
    collegeId: 0,
    expYear: null,
    currentCtc: null,
    expectedCtc: null,
    currentlyOnNoticePeriod: null,
    noticeEnd: null,
    noticePeriodLength: null,
    appearedZeusTest: null,
    zeusTestRole: '',
    applicationTypeId: 0,
    expertTechsId: [],
    familiarTechsId: [],
    resumeFileName: '',
    rolesId: [],
    otherCollege: '',
    otherExpertTechs: '',
    otherFamiliarTechs: '',
    otherCollegeLocation: '',
  };
  nameRegex = /^[A-Z][a-zA-Z]{1,40}$/;
  emailRegex = /^[a-zA-Z0-9._-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,4}$/;
  countryCodeRegex = /^[0-9]{1,4}$/;
  phonenoRegex = /^[1-9][0-9]{9}$/;
  percentRegex = /^((100)|(\d{1,2}(.\d{1.2})?))$/;
  expRegex = /^[0-9]{1,2}$/;
  ctcRegex = /^[0-9]{4,8}$/;
  Validations: any = {
    firstNameInalid: false,
    lastNameInalid: false,
    emailInalid: false,
    countroCodeInvalid: false,
    phonenoInvalid: false,
    resumeRequired: false,
    rolesRequired: false,
    percentInvalid: false,
    passingYearRequired: false,
    qualificationRequired: false,
    streamRequired: false,
    collegeRequired: false,
    locationRequired: false,
    applicationTypeRequired: false,
    expInvalid: false,
    currentCtcInvalid: false,
    expectedCtcInvalid: false,
    expertTechRequired: false,
    currentlyOnNoticeRequired: false,
    noticePeriodEndRequired: false,
    givenZeusTestRequired: false,
  };
  constructor(
    private http: HttpClient,
    private router: Router,
    private route: ActivatedRoute
  ) {}

  updateUserData(userData: UserRegistrationRequest): void {
    this.userData = userData;
  }

  previous(): void {
    if (!this.qualificationShow && this.reviewShow) {
      this.personalShow = false;
      this.qualificationShow = true;
      this.reviewShow = false;
    } else if (!this.personalShow && this.qualificationShow) {
      this.personalShow = true;
      this.qualificationShow = false;
      this.reviewShow = false;
    }
  }
  next(): void {
    if (!this.qualificationShow && this.personalShow) {
      if (this.nameRegex.test(this.userData.firstName)) {
        this.Validations.firstNameInalid = false;
      } else {
        this.Validations.firstNameInalid = true;
      }
      if (this.nameRegex.test(this.userData.lastName)) {
        this.Validations.lastNameInalid = false;
      } else {
        this.Validations.lastNameInalid = true;
      }
      if (this.emailRegex.test(this.userData.email)) {
        this.Validations.emailInalid = false;
      } else {
        this.Validations.emailInalid = true;
      }
      if (this.userData.countrycode !== null) {
        const countryCodeString: string = this.userData.countrycode.toString();
        if (this.countryCodeRegex.test(countryCodeString)) {
          this.Validations.countroCodeInvalid = false;
        } else {
          this.Validations.countroCodeInvalid = true;
        }
      } else {
        this.Validations.countroCodeInvalid = true;
      }
      if (this.userData.phoneNo !== null) {
        const String: string = this.userData.phoneNo.toString();
        if (this.phonenoRegex.test(String)) {
          this.Validations.phonenoInvalid = false;
        } else {
          this.Validations.phonenoInvalid = true;
        }
      } else {
        this.Validations.phonenoInvalid = true;
      }
      if (this.userData.resume !== null || this.userData.resume === '') {
        this.Validations.resumeRequired = false;
      } else {
        this.Validations.resumeRequired = true;
      }
      if (this.userData.rolesId.length > 0) {
        this.Validations.rolesRequired = false;
      } else {
        this.Validations.rolesRequired = true;
      }
      if (
        !this.Validations.firstNameInalid &&
        !this.Validations.lastNameInalid &&
        !this.Validations.emailInalid &&
        !this.Validations.countroCodeInvalid &&
        !this.Validations.phonenoInvalid &&
        !this.Validations.resumeRequired &&
        !this.Validations.rolesRequired
      ) {
        this.personalShow = false;
        this.qualificationShow = true;
        this.reviewShow = false;
      }
    } else if (!this.reviewShow && this.qualificationShow) {
      if (this.userData.percentage !== null) {
        const String: string = this.userData.percentage.toString();
        if (this.countryCodeRegex.test(String)) {
          this.Validations.percentInvalid = false;
        } else {
          this.Validations.percentInvalid = true;
        }
      } else {
        this.Validations.percentInvalid = true;
      }
      if (this.userData.passingYear) {
        this.Validations.passingYearRequired = false;
      } else {
        this.Validations.passingYearRequired = true;
      }
      if (this.userData.qualificationId != 0) {
        this.Validations.qualificationRequired = false;
      } else {
        this.Validations.qualificationRequired = true;
      }
      if (this.userData.streamId != 0) {
        this.Validations.streamRequired = false;
      } else {
        this.Validations.streamRequired = true;
      }
      if (this.userData.collegeId != 0) {
        this.Validations.collegeRequired = false;
      } else {
        this.Validations.collegeRequired = true;
      }
      if (this.nameRegex.test(this.userData.otherCollegeLocation)) {
        this.Validations.locationRequired = false;
      } else {
        this.Validations.locationRequired = true;
      }
      if (this.userData.applicationTypeId != 0) {
        this.Validations.applicationTypeRequired = false;
        if (this.userData.expYear !== null) {
          const String: string = this.userData.expYear.toString();
          if (this.expRegex.test(String)) {
            this.Validations.expInvalid = false;
          } else {
            this.Validations.expInvalid = true;
          }
        } else {
          this.Validations.expInvalid = true;
        }
        if (this.userData.currentCtc !== null) {
          const String: string = this.userData.currentCtc.toString();
          if (this.ctcRegex.test(String)) {
            this.Validations.currentCtcInvalid = false;
          } else {
            this.Validations.currentCtcInvalid = true;
          }
        } else {
          this.Validations.currentCtcInvalid = true;
        }
        if (this.userData.expectedCtc !== null) {
          const String: string = this.userData.expectedCtc.toString();
          if (this.ctcRegex.test(String)) {
            this.Validations.expectedCtcInvalid = false;
          } else {
            this.Validations.expectedCtcInvalid = true;
          }
        } else {
          this.Validations.expectedCtcInvalid = true;
        }
        if (this.userData.expertTechsId.length > 0) {
          this.Validations.expertTechRequired = false;
        } else {
          this.Validations.expertTechRequired = true;
        }
        if (this.userData.currentlyOnNoticePeriod !== null) {
          this.Validations.currentlyOnNoticeRequired = false;
          if (this.userData.noticeEnd !== null) {
            this.Validations.noticePeriodEndRequired = false;
          } else {
            this.Validations.noticePeriodEndRequired = true;
          }
        } else {
          this.Validations.currentlyOnNoticeRequired = true;
        }
      } else {
        this.Validations.applicationTypeRequired = true;
      }
      if (this.userData.appearedZeusTest !== null) {
        this.Validations.givenZeusTestRequired = false;
      } else {
        this.Validations.givenZeusTestRequired = true;
      }
      if (
        !this.Validations.percentInvalid &&
        !this.Validations.passingYearRequired &&
        !this.Validations.qualificationRequired &&
        !this.Validations.streamRequired &&
        !this.Validations.collegeRequired &&
        !this.Validations.locationRequired &&
        (this.userData.applicationTypeId == 1 ||
          (this.userData.applicationTypeId == 2 &&
            !this.Validations.expInvalid &&
            !this.Validations.currentCtcInvalid &&
            !this.Validations.expectedCtcInvalid &&
            !this.Validations.expertTechRequired &&
            !this.Validations.currentlyOnNoticeRequired &&
            !this.Validations.noticePeriodEndRequired &&
            !this.Validations.givenZeusTestRequired))
      ) {
        this.personalShow = false;
        this.qualificationShow = false;
        this.reviewShow = true;
      }
    }
  }

  fetchData() {
    this.http
      .get<RegistrationData>(`${baseurl}/getregistrationdata`)
      .pipe(
        catchError((error) => {
          console.error('There was a problem with the fetch operation:', error);
          return throwError(error);
        })
      )
      .subscribe((data) => {
        this.registrationData = data;
      });
  }

  ngOnInit(): void {
    this.fetchData();
  }
}
