import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { LombardNewComponent } from './lombard-new.component';

describe('LombardNewComponent', () => {
  let component: LombardNewComponent;
  let fixture: ComponentFixture<LombardNewComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ LombardNewComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(LombardNewComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
