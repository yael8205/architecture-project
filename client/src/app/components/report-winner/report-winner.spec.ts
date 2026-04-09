import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ReportWinner } from './report-winner';

describe('ReportWinner', () => {
  let component: ReportWinner;
  let fixture: ComponentFixture<ReportWinner>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ReportWinner]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ReportWinner);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
