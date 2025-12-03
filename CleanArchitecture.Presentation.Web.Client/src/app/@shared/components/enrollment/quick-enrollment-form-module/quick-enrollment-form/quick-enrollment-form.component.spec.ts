import { ComponentFixture, TestBed } from '@angular/core/testing';

import { QuickEnrollmentFormComponent } from './quick-enrollment-form.component';

describe('QuickEnrollmentFormComponent', () => {
  let component: QuickEnrollmentFormComponent;
  let fixture: ComponentFixture<QuickEnrollmentFormComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ QuickEnrollmentFormComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(QuickEnrollmentFormComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
