import { TestBed } from '@angular/core/testing';

import { IndexInvestmentTypeHttpService } from './index-investment-type-http.service';

describe('IndexInvestmentTypeHttpService', () => {
  let service: IndexInvestmentTypeHttpService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(IndexInvestmentTypeHttpService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
