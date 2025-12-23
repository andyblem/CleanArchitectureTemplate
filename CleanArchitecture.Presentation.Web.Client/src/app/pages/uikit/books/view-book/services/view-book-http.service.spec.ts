import { TestBed } from '@angular/core/testing';

import { ViewBookHttpService } from './view-book-http.service';

describe('ViewBookHttpService', () => {
  let service: ViewBookHttpService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(ViewBookHttpService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
