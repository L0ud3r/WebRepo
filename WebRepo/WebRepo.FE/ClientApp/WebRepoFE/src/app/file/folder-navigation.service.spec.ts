import { TestBed } from '@angular/core/testing';

import { FolderNavigationService } from './folder-navigation.service';

describe('FolderNavigationService', () => {
  let service: FolderNavigationService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(FolderNavigationService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
