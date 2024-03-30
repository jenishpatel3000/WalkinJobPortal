// export interface IJobData {
//   JobId: number;
//   JobTitle: string;
//   JobDateTime: string;
//   JobLocation: string;
//   JobRoles: string[];
//   JobSpecialMessage: string;
//   JobExpiringRemainingDays: number;
// }
export interface IJobData {
  JobId: number;
  JobTitle: string;
  JobStartDate: Date;
  JobEndDate: Date;
  JobLocation: string;
  JobSpecialMessage: string;
  JobExpiringRemainingDays: number;
  JobVenue: string;
  Things_to_remember: string;
  JobRoles: {
    JobRoleName: string;
    RolePackage: number;
    RoleDescription: {
      DescriptionTitle: string;
      Description: string;
    }[];
  }[];
  JobDescription: {
    DescriptionTitle: string;
    Description: string;
  }[];
  JobTimeSlots: {
    StartTime: string;
    EndTime: string;
  }[];
}
export interface IApplicationInfo {
  ApplicationJobId: number;
  ApplicationId: number;
  SelectedTimeSlots: string;
  selectedRoles: string[];
  UploadedResume: File | null;
}
export enum Jobrole {
  val1 = 'Instructional Designer',
  val2 = 'Software Engineer',
  val3 = 'Software Quality Engineer',
}

export interface IJob {
  job_id: string;
  job_name: string;
  from_time: string;
  to_time: string;
  venue: string;
  things_to_remember: string;
  location_name: string;
  roles: {
    role_name: string;
    package: number;
    role_desc: {
      desc_title: string;
      desc: string;
    }[];
  }[];
  job_desc: {
    desc_title: string;
    desc: string;
  }[];
  slots: {
    from_time: string;
    to_time: string;
  }[];
}
