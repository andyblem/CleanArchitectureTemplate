import { TestBed } from '@angular/core/testing';

import { IndexVehicleTypeHttpService } from './index-vehicle-type-http.service';

describe('IndexVehicleTypeHttpService', () => {
  let service: IndexVehicleTypeHttpService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(IndexVehicleTypeHttpService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
