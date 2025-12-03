import { TestBed } from '@angular/core/testing';

import { ViewIncomeTypeHttpService } from './view-income-type-http.service';

describe('ViewIncomeTypeHttpService', () => {
  let service: ViewIncomeTypeHttpService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(ViewIncomeTypeHttpService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
