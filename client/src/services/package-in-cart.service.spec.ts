import { TestBed } from '@angular/core/testing';

import { PackageInCartService } from './package-in-cart.service';

describe('PackageInCartService', () => {
  let service: PackageInCartService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(PackageInCartService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
