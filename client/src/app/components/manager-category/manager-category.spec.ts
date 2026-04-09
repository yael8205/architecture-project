import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ManagerCategory } from './manager-category';

describe('ManagerCategory', () => {
  let component: ManagerCategory;
  let fixture: ComponentFixture<ManagerCategory>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ManagerCategory]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ManagerCategory);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
