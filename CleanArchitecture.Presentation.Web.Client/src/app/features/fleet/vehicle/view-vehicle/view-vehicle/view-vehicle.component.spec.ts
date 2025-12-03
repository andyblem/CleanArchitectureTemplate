import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ViewVehicleComponent } from './view-vehicle.component';

describe('ViewVehicleComponent', () => {
  let component: ViewVehicleComponent;
  let fixture: ComponentFixture<ViewVehicleComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [ViewVehicleComponent]
    });
    fixture = TestBed.createComponent(ViewVehicleComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
