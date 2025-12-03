import { ComponentFixture, TestBed } from '@angular/core/testing';

import { IndexVehicleTypeComponent } from './index-vehicle-type.component';

describe('IndexVehicleTypeComponent', () => {
  let component: IndexVehicleTypeComponent;
  let fixture: ComponentFixture<IndexVehicleTypeComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [IndexVehicleTypeComponent]
    });
    fixture = TestBed.createComponent(IndexVehicleTypeComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
