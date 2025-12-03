import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ViewVehicleTypeComponent } from './view-vehicle-type.component';

describe('ViewVehicleTypeComponent', () => {
  let component: ViewVehicleTypeComponent;
  let fixture: ComponentFixture<ViewVehicleTypeComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [ViewVehicleTypeComponent]
    });
    fixture = TestBed.createComponent(ViewVehicleTypeComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
