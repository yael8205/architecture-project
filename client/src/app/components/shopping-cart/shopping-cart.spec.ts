import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ShoppingCartService } from '../../../services/shopping-cart.service';


describe('ShoppingCartService', () => {
  let component: ShoppingCartService;
  let fixture: ComponentFixture<ShoppingCartService>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ShoppingCartService]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ShoppingCartService);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
