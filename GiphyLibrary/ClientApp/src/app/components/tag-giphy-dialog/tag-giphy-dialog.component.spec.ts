import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TagGiphyDialogComponent } from './tag-giphy-dialog.component';

describe('TagGiphyDialogComponent', () => {
  let component: TagGiphyDialogComponent;
  let fixture: ComponentFixture<TagGiphyDialogComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TagGiphyDialogComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TagGiphyDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
