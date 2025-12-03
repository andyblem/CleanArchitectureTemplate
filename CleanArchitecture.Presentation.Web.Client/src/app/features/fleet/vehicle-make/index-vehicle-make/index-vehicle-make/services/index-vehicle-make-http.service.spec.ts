import { TestBed } from '@angular/core/testing';

import { IndexVehicleMakeHttpService } from './index-vehicle-make-http.service';

describe('IndexVehicleMakeHttpService', () => {
  let service: IndexVehicleMakeHttpService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(IndexVehicleMakeHttpService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
