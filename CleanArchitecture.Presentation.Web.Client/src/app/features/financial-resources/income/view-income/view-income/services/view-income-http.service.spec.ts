import { TestBed } from '@angular/core/testing';

import { ViewIncomeHttpService } from './view-income-http.service';

describe('ViewIncomeHttpService', () => {
  let service: ViewIncomeHttpService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(ViewIncomeHttpService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
