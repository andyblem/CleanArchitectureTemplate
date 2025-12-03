import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ViewCostTypeComponent } from './view-cost-type.component';

describe('ViewCostTypeComponent', () => {
  let component: ViewCostTypeComponent;
  let fixture: ComponentFixture<ViewCostTypeComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [ViewCostTypeComponent]
    });
    fixture = TestBed.createComponent(ViewCostTypeComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
