import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ViewIncomeTypeComponent } from './view-income-type.component';

describe('ViewIncomeTypeComponent', () => {
  let component: ViewIncomeTypeComponent;
  let fixture: ComponentFixture<ViewIncomeTypeComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [ViewIncomeTypeComponent]
    });
    fixture = TestBed.createComponent(ViewIncomeTypeComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
