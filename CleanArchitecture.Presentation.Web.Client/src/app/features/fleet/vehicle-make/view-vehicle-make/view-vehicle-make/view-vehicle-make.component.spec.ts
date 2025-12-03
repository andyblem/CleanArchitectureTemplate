import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ViewVehicleMakeComponent } from './view-vehicle-make.component';

describe('ViewVehicleMakeComponent', () => {
  let component: ViewVehicleMakeComponent;
  let fixture: ComponentFixture<ViewVehicleMakeComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [ViewVehicleMakeComponent]
    });
    fixture = TestBed.createComponent(ViewVehicleMakeComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
