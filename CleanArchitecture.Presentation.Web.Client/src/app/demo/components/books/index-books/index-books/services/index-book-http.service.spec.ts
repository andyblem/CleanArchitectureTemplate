import { TestBed } from '@angular/core/testing';

import { IndexBookHttpService } from './index-book-http.service';

describe('IndexBookHttpService', () => {
  let service: IndexBookHttpService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(IndexBookHttpService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
