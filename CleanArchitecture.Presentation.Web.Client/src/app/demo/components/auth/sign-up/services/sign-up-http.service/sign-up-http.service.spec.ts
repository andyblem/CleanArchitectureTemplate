import { TestBed } from '@angular/core/testing';

import { SignUpHttpService } from './sign-up-http.service';

describe('SignUpHttpService', () => {
  let service: SignUpHttpService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(SignUpHttpService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
