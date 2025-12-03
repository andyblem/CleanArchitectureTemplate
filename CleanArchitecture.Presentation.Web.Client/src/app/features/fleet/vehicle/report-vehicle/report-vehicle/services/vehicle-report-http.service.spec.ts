import { TestBed } from '@angular/core/testing';

import { VehicleReportHttpService } from './vehicle-report-http.service';

describe('VehicleReportHttpService', () => {
  let service: VehicleReportHttpService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(VehicleReportHttpService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
