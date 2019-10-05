import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SavedGiphyTableComponent } from './saved-giphy-table.component';

describe('SavedGiphyTableComponent', () => {
  let component: SavedGiphyTableComponent;
  let fixture: ComponentFixture<SavedGiphyTableComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ SavedGiphyTableComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SavedGiphyTableComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
