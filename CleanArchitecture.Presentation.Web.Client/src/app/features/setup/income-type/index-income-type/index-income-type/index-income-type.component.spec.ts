import { ComponentFixture, TestBed } from '@angular/core/testing';

import { IndexIncomeTypeComponent } from './index-income-type.component';

describe('IndexIncomeTypeComponent', () => {
  let component: IndexIncomeTypeComponent;
  let fixture: ComponentFixture<IndexIncomeTypeComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [IndexIncomeTypeComponent]
    });
    fixture = TestBed.createComponent(IndexIncomeTypeComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
