import { ComponentFixture, TestBed } from '@angular/core/testing';

import { IndexInvestmentTypeComponent } from './index-investment-type.component';

describe('IndexInvestmentTypeComponent', () => {
  let component: IndexInvestmentTypeComponent;
  let fixture: ComponentFixture<IndexInvestmentTypeComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [IndexInvestmentTypeComponent]
    });
    fixture = TestBed.createComponent(IndexInvestmentTypeComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
