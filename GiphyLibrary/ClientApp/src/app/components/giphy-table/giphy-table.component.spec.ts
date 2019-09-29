import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { GiphyTableComponent } from './giphy-table.component';

describe('GiphyTableComponent', () => {
  let component: GiphyTableComponent;
  let fixture: ComponentFixture<GiphyTableComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ GiphyTableComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(GiphyTableComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
