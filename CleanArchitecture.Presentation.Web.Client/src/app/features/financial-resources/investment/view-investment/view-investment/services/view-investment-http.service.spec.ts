import { TestBed } from '@angular/core/testing';

import { ViewInvestmentHttpService } from './view-investment-http.service';

describe('ViewInvestmentHttpService', () => {
  let service: ViewInvestmentHttpService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(ViewInvestmentHttpService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
