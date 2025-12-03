import { TestBed } from '@angular/core/testing';

import { IndexIncomeTypeHttpService } from './index-income-type-http.service';

describe('IndexIncomeTypeHttpService', () => {
  let service: IndexIncomeTypeHttpService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(IndexIncomeTypeHttpService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
