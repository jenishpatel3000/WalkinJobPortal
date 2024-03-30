import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { IApplicationInfo } from '../job-page/JobData';

@Injectable({
  providedIn: 'root',
})
export class ApplicationService {
  private applicationData: IApplicationInfo[] = [];
  private applicationDataSubject = new BehaviorSubject<IApplicationInfo[]>(
    this.applicationData
  );

  constructor() {}

  sendData(data: IApplicationInfo[]) {
    this.applicationData = data;
    this.applicationDataSubject.next(this.applicationData);
  }

  getApplicationData(): IApplicationInfo[] {
    return this.applicationData;
  }
}
