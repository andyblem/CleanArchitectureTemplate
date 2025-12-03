import { TestBed } from '@angular/core/testing';

import { AuthenticateHttpService } from './authenticate-http.service';

describe('AuthenticateService', () => {
    let service: AuthenticateHttpService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
      service = TestBed.inject(AuthenticateHttpService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
