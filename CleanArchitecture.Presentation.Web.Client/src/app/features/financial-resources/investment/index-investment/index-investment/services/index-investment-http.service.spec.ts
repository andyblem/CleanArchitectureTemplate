import { TestBed } from '@angular/core/testing';

import { IndexInvestmentHttpService } from './index-investment-http.service';

describe('IndexInvestmentHttpService', () => {
  let service: IndexInvestmentHttpService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(IndexInvestmentHttpService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
