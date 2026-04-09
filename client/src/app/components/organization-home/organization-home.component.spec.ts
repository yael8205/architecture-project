import { ComponentFixture, TestBed } from '@angular/core/testing';
import { OrganizationHomeComponent } from './organization-home.component';

describe('OrganizationHomeComponent', () => {
  let component: OrganizationHomeComponent;
  let fixture: ComponentFixture<OrganizationHomeComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      // בגלל שזו קומפוננטה מסוג Standalone, היא נכנסת ב-imports
      imports: [OrganizationHomeComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(OrganizationHomeComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});