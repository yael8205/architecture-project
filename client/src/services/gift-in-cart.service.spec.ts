import { TestBed } from '@angular/core/testing';

import { GiftInCartService } from './gift-in-cart.service';

describe('GiftInCartService', () => {
  let service: GiftInCartService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(GiftInCartService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
