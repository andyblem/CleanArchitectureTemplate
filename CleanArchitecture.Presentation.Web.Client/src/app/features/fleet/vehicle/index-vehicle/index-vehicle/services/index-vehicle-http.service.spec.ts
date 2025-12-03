import { TestBed } from '@angular/core/testing';

import { IndexVehicleHttpService } from './index-vehiclehttp.service';

describe('IndexVehicleHttpService', () => {
  let service: IndexVehicleHttpService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(IndexVehicleHttpService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
