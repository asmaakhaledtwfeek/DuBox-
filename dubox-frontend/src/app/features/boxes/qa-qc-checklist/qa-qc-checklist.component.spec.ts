import { ComponentFixture, TestBed } from '@angular/core/testing';

import { QaQcChecklistComponent } from './qa-qc-checklist.component';

describe('QaQcChecklistComponent', () => {
  let component: QaQcChecklistComponent;
  let fixture: ComponentFixture<QaQcChecklistComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [QaQcChecklistComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(QaQcChecklistComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
