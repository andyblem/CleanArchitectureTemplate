import { TestBed } from '@angular/core/testing';

import { ViewVehicleModelHttpService } from './view-vehicle-model-http.service';

describe('ViewVehicleModelHttpService', () => {
  let service: ViewVehicleModelHttpService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(ViewVehicleModelHttpService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
