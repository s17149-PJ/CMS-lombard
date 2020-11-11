import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { LombardComponent } from './lombard.component';

describe('LombardComponent', () => {
  let component: LombardComponent;
  let fixture: ComponentFixture<LombardComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ LombardComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(LombardComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
