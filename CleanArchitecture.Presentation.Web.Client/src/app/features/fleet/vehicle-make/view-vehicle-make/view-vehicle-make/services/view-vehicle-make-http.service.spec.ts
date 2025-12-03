import { TestBed } from '@angular/core/testing';

import { ViewVehicleMakeHttpService } from './view-vehicle-make-http.service';

describe('ViewVehicleMakeHttpService', () => {
  let service: ViewVehicleMakeHttpService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(ViewVehicleMakeHttpService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
