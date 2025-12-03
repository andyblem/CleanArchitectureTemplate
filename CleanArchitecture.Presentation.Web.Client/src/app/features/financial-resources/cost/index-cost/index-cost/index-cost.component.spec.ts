import { ComponentFixture, TestBed } from '@angular/core/testing';

import { IndexCostComponent } from './index-cost.component';

describe('IndexCostComponent', () => {
  let component: IndexCostComponent;
  let fixture: ComponentFixture<IndexCostComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [IndexCostComponent]
    });
    fixture = TestBed.createComponent(IndexCostComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
