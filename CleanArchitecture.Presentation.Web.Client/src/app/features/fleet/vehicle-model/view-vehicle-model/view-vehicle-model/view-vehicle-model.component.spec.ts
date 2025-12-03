import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ViewVehicleModelComponent } from './view-vehicle-model.component';

describe('ViewVehicleModelComponent', () => {
  let component: ViewVehicleModelComponent;
  let fixture: ComponentFixture<ViewVehicleModelComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [ViewVehicleModelComponent]
    });
    fixture = TestBed.createComponent(ViewVehicleModelComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
