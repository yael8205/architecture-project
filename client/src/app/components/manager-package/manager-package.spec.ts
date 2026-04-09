import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ManagerPackage } from './manager-package';

describe('ManagerPackage', () => {
  let component: ManagerPackage;
  let fixture: ComponentFixture<ManagerPackage>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ManagerPackage]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ManagerPackage);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
