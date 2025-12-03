import { TestBed } from '@angular/core/testing';

import { IndexVehicleModelHttpService } from './index-vehicle-model-http.service';

describe('IndexVehicleModelHttpService', () => {
  let service: IndexVehicleModelHttpService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(IndexVehicleModelHttpService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
