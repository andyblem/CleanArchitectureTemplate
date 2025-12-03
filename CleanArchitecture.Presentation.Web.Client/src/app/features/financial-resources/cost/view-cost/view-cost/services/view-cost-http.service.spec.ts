import { TestBed } from '@angular/core/testing';

import { ViewCostHttpService } from './view-cost-http.service';

describe('ViewCostHttpService', () => {
  let service: ViewCostHttpService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(ViewCostHttpService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
