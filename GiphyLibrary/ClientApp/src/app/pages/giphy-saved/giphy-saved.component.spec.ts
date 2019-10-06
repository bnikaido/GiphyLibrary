import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { GiphySavedComponent } from './giphy-saved.component';

describe('GiphySavedComponent', () => {
  let component: GiphySavedComponent;
  let fixture: ComponentFixture<GiphySavedComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ GiphySavedComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(GiphySavedComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
