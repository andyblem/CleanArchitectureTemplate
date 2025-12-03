import { TestBed } from '@angular/core/testing';

import { IndexCostHttpService } from './index-cost-http.service';

describe('IndexCostHttpService', () => {
  let service: IndexCostHttpService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(IndexCostHttpService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
