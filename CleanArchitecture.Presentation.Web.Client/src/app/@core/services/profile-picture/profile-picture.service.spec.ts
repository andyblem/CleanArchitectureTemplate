import { TestBed } from '@angular/core/testing';

import { ProfilePictureService } from './profile-picture.service';

describe('ProfilePictureService', () => {
  let service: ProfilePictureService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(ProfilePictureService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
