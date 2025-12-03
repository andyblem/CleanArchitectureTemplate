import { ComponentFixture, TestBed } from '@angular/core/testing';

import { IndexInvestmentComponent } from './index-investment.component';

describe('IndexInvestmentComponent', () => {
  let component: IndexInvestmentComponent;
  let fixture: ComponentFixture<IndexInvestmentComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [IndexInvestmentComponent]
    });
    fixture = TestBed.createComponent(IndexInvestmentComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
