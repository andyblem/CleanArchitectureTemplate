import { TestBed } from '@angular/core/testing';

import { IndexIncomeHttpService } from './index-income-http.service';

describe('IndexIncomeHttpService', () => {
  let service: IndexIncomeHttpService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(IndexIncomeHttpService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
