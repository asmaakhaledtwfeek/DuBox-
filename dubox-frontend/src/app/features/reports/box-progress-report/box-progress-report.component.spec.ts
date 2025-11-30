import { ComponentFixture, TestBed } from '@angular/core/testing';

import { BoxProgressReportComponent } from './box-progress-report.component';

describe('BoxProgressReportComponent', () => {
  let component: BoxProgressReportComponent;
  let fixture: ComponentFixture<BoxProgressReportComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [BoxProgressReportComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(BoxProgressReportComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
