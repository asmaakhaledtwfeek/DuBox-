import { ComponentFixture, TestBed } from '@angular/core/testing';
import { BoxesSummaryReportComponent } from './boxes-summary-report.component';

describe('BoxesSummaryReportComponent', () => {
  let component: BoxesSummaryReportComponent;
  let fixture: ComponentFixture<BoxesSummaryReportComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [BoxesSummaryReportComponent]
    }).compileComponents();

    fixture = TestBed.createComponent(BoxesSummaryReportComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

