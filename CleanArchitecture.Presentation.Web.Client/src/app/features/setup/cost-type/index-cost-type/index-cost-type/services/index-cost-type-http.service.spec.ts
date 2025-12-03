import { TestBed } from '@angular/core/testing';

import { IndexCostTypeHttpService } from './index-cost-type-http.service';

describe('IndexCostTypeHttpService', () => {
  let service: IndexCostTypeHttpService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(IndexCostTypeHttpService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
