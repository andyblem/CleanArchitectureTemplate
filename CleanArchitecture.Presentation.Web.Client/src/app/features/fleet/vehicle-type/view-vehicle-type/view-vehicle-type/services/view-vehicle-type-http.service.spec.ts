import { TestBed } from '@angular/core/testing';

import { ViewVehicleTypeHttpService } from './view-vehicle-type-http.service';

describe('ViewVehicleTypeHttpService', () => {
  let service: ViewVehicleTypeHttpService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(ViewVehicleTypeHttpService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
