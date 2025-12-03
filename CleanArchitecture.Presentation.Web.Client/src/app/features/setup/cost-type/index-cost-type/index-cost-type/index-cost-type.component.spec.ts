import { ComponentFixture, TestBed } from '@angular/core/testing';

import { IndexCostTypeComponent } from './index-cost-type.component';

describe('IndexCostTypeComponent', () => {
  let component: IndexCostTypeComponent;
  let fixture: ComponentFixture<IndexCostTypeComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [IndexCostTypeComponent]
    });
    fixture = TestBed.createComponent(IndexCostTypeComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
