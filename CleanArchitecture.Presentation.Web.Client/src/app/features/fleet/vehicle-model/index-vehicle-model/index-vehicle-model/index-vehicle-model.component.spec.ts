import { ComponentFixture, TestBed } from '@angular/core/testing';

import { IndexVehicleModelComponent } from './index-vehicle-model.component';

describe('IndexVehicleModelComponent', () => {
  let component: IndexVehicleModelComponent;
  let fixture: ComponentFixture<IndexVehicleModelComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [IndexVehicleModelComponent]
    });
    fixture = TestBed.createComponent(IndexVehicleModelComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
