import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ManagerDonor } from './manager-donor';

describe('ManagerDonor', () => {
  let component: ManagerDonor;
  let fixture: ComponentFixture<ManagerDonor>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ManagerDonor]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ManagerDonor);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
