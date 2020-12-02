import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { LombardDetailsComponent } from './lombard-details.component';

describe('LombardDetailsComponent', () => {
  let component: LombardDetailsComponent;
  let fixture: ComponentFixture<LombardDetailsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ LombardDetailsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(LombardDetailsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
