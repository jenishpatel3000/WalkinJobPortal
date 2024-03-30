import { Time } from '@angular/common';

export interface IJob {
    jobId: number;
    jobName: string;
    fromTime: Date;
    toTime: Date;
    venue: string;
    thingsToRemember: string;
    locationId: number;
    special_message: string;
    location: {
        locationId: number;
        locationName: string;
    },
    jobRoles: jobRoles[];
    jobDescs: jobDesc[];
    jobSlots: jobSlots[];
}

interface role {
    roleId: number;
    roleName: string;
}

interface jobRoleDescs {
    id: number;
    descTitle: string;
    description: string;
}

export interface jobRoles {
    id: number;
    package: number;
    jobId: number;
    roleId: number;
    role: role;
    jobRolesDescs: jobRoleDescs[];
}

interface jobDesc {
    id: number;
    descTitle: string;
    description: string;
}

interface slot {
    slotId: number;
    fromTime: Time;
    toTime: Time;
}

interface jobSlots {
    slotId: number;
    jobId: number;
    slot: slot;
}

export interface IApplicationRequest {
    resume: string | null;
    userId: number;
    jobId: number;
    slotId: number;
    rolesid: number[];
}

export interface IApplication {
    applicationId: number;
    resume: string;
    userId: number;
    jobId: number;
    slotId: number;
    hallticket: string;
    job: {
        jobId: number;
        jobName: string;
        fromTime: Date;
        toTime: Date;
        venue: string;
        thingsToRemember: string;
        date: Date;
    };
    slot: slot;
    applicationRoles: {
        roleId: number;
        applicationId: number;
    }[] | null;
}

export interface College {
    collegeId: number;
    collegeName: string;
    locationId: number;
    location: Location;
}

export interface Location {
    locationId: number;
    locationName: string;
}

export interface Stream {
    streamId: number;
    streamName: string;
}

export interface Qualification {
    qualificationId: number;
    qualificationName: string;
}

export interface Tech {
    techId: number;
    techName: string;
}

export interface Role {
    roleId: number;
    roleName: string;
}

export interface ApplicationTypes {
    applicationTypeId: number;
    applicationTypeName: string;
}

export interface RegistrationData {
    college: College[];
    location: Location[];
    stream: Stream[];
    qualification: Qualification[];
    tech: Tech[];
    role: Role[];
    applicationTypes: ApplicationTypes[];
}

export interface UserRegistrationRequest {
    email: string;
    firstName: string;
    lastName: string;
    phoneNo: number | null;
    portfolioUrl: string;
    referalEmpName: string;
    sendMeUpdate: number | null;
    userId: number;
    countrycode: number | null;
    resume: string | null;
    resumeFileName: string;
    profilePhoto: string | null;
    percentage: number | null;
    passingYear: number | null;
    qualificationId: number;
    streamId: number;
    collegeId: number;
    otherCollege: string;
    otherCollegeLocation: string;
    expYear: number | null;
    currentCtc: number | null;
    expectedCtc: number | null;
    currentlyOnNoticePeriod: number | null;
    noticeEnd: Date | null;
    noticePeriodLength: number | null;
    appearedZeusTest: number | null;
    zeusTestRole: string;
    applicationTypeId: number;
    expertTechsId: number[];
    otherExpertTechs: string;
    familiarTechsId: number[];
    otherFamiliarTechs: string;
    rolesId: number[];
}

export interface LoginResponse {
    token: string;
    userId: number;
}