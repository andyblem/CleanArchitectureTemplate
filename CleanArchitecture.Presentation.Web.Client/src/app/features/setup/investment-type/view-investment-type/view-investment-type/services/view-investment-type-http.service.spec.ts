import { TestBed } from '@angular/core/testing';

import { ViewInvestmentTypeHttpService } from './view-investment-type-http.service';

describe('ViewInvestmentTypeHttpService', () => {
  let service: ViewInvestmentTypeHttpService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(ViewInvestmentTypeHttpService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
