import { ComponentFixture, TestBed } from '@angular/core/testing';

import { IndexVehicleComponent } from './index-vehicle.component';

describe('IndexVehicleComponent', () => {
  let component: IndexVehicleComponent;
  let fixture: ComponentFixture<IndexVehicleComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [IndexVehicleComponent]
    });
    fixture = TestBed.createComponent(IndexVehicleComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
