import { ComponentFixture, TestBed } from '@angular/core/testing';

import { IndexBooksComponent } from './index-books.component';

describe('IndexBooksComponent', () => {
  let component: IndexBooksComponent;
  let fixture: ComponentFixture<IndexBooksComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [IndexBooksComponent]
    });
    fixture = TestBed.createComponent(IndexBooksComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
