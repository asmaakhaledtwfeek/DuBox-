import { ComponentFixture, TestBed } from '@angular/core/testing';

import { MissingMaterialsReportComponent } from './missing-materials-report.component';

describe('MissingMaterialsReportComponent', () => {
  let component: MissingMaterialsReportComponent;
  let fixture: ComponentFixture<MissingMaterialsReportComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [MissingMaterialsReportComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(MissingMaterialsReportComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
