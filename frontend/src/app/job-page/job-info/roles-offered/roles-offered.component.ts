import { Component, Input, OnInit } from '@angular/core';
import { jobRoles } from 'src/interface/interfaces';

@Component({
  selector: 'app-roles-offered',
  templateUrl: './roles-offered.component.html',
  styleUrls: ['./roles-offered.component.scss']
})
export class RolesOfferedComponent implements OnInit {

  @Input() role!: jobRoles;

  constructor() { }

  ngOnInit(): void {
  }

  isNotExpanded = false;

  toggleExpansion() {
    this.isNotExpanded = !this.isNotExpanded;
  }

}
