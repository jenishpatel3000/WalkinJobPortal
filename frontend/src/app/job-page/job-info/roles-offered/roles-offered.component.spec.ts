import { ComponentFixture, TestBed } from '@angular/core/testing';

import { RolesOfferedComponent } from './roles-offered.component';

describe('RolesOfferedComponent', () => {
  let component: RolesOfferedComponent;
  let fixture: ComponentFixture<RolesOfferedComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ RolesOfferedComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(RolesOfferedComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
