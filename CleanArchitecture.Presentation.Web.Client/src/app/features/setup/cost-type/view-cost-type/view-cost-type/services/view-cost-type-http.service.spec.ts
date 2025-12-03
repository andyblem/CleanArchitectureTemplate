import { TestBed } from '@angular/core/testing';

import { ViewCostTypeHttpService } from './view-cost-type-http.service';

describe('ViewCostTypeHttpService', () => {
  let service: ViewCostTypeHttpService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(ViewCostTypeHttpService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
