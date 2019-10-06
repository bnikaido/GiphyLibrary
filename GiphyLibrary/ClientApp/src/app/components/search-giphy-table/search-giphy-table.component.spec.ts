import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SearchGiphyTableComponent } from './search-giphy-table.component';

describe('GiphyTableComponent', () => {
  let component: SearchGiphyTableComponent;
  let fixture: ComponentFixture<SearchGiphyTableComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ SearchGiphyTableComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SearchGiphyTableComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
