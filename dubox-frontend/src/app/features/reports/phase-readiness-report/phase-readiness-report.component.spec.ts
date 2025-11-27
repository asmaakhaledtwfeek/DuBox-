import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PhaseReadinessReportComponent } from './phase-readiness-report.component';

describe('PhaseReadinessReportComponent', () => {
  let component: PhaseReadinessReportComponent;
  let fixture: ComponentFixture<PhaseReadinessReportComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [PhaseReadinessReportComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(PhaseReadinessReportComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
