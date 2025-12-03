import { TestBed } from '@angular/core/testing';

import { ViewVehicleHttpService } from './view-vehiclehttp.service';

describe('ViewVehicleHttpService', () => {
  let service: ViewVehicleHttpService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(ViewVehicleHttpService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
