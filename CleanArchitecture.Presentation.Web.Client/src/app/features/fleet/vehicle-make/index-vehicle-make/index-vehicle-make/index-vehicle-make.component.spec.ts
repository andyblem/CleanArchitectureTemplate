import { ComponentFixture, TestBed } from '@angular/core/testing';

import { IndexVehicleMakeComponent } from './index-vehicle-make.component';

describe('IndexVehicleMakeComponent', () => {
  let component: IndexVehicleMakeComponent;
  let fixture: ComponentFixture<IndexVehicleMakeComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [IndexVehicleMakeComponent]
    });
    fixture = TestBed.createComponent(IndexVehicleMakeComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
