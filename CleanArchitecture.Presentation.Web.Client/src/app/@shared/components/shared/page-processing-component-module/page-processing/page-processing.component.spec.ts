import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PageProcessingComponent } from './page-processing.component';

describe('PageProcessingComponent', () => {
  let component: PageProcessingComponent;
  let fixture: ComponentFixture<PageProcessingComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ PageProcessingComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(PageProcessingComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
