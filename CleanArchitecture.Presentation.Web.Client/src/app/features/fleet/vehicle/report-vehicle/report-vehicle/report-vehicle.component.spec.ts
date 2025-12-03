import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ReportVehicleComponent } from './report-vehicle.component';

describe('ReportVehicleComponent', () => {
  let component: ReportVehicleComponent;
  let fixture: ComponentFixture<ReportVehicleComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [ReportVehicleComponent]
    });
    fixture = TestBed.createComponent(ReportVehicleComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
