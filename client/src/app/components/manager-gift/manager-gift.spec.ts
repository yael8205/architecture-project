import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ManagerGift } from './manager-gift';

describe('ManagerGift', () => {
  let component: ManagerGift;
  let fixture: ComponentFixture<ManagerGift>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ManagerGift]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ManagerGift);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
