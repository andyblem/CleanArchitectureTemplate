import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ViewInvestmentTypeComponent } from './view-investment-type.component';

describe('ViewInvestmentTypeComponent', () => {
  let component: ViewInvestmentTypeComponent;
  let fixture: ComponentFixture<ViewInvestmentTypeComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [ViewInvestmentTypeComponent]
    });
    fixture = TestBed.createComponent(ViewInvestmentTypeComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
