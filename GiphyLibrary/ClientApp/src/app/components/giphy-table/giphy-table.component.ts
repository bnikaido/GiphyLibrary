import { Component, Input, OnChanges, SimpleChanges } from '@angular/core';
import { Giphy } from '../../models/giphy.model';
import { MatDialog } from '@angular/material';
import { TagGiphyDialogComponent } from '../tag-giphy-dialog/tag-giphy-dialog.component';
import { switchMap } from 'rxjs/operators';
import { of } from 'rxjs';
import { HttpResponse } from '@angular/common/http';

@Component({
  selector: 'app-giphy-table',
  templateUrl: './giphy-table.component.html',
  styleUrls: ['./giphy-table.component.css']
})
export class GiphyTableComponent implements OnChanges {
  @Input() giphies: Giphy[];

  constructor(public dialog: MatDialog) {
  }

  ngOnChanges(changes: SimpleChanges) {
    console.log(this.giphies);
  }

  saveGiphy(id: string) {
    console.log(`saved ${id}!`);
  }

  openTagGiphyDialog(id: string): any {
    console.log(`tagged ${id}`);
    const dialogRef = this.dialog.open(TagGiphyDialogComponent, {
      data: ["Cats", "Favorites", "Funny"]
    });
    dialogRef
      .afterClosed()
      .pipe(
        switchMap((tag) => {
          if (tag) {
            console.log(`${tag} declared.`);
            return this.tagGiphy(id, tag);
          }
          return of({});
        }))
      .subscribe((response) => {
        if (response instanceof HttpResponse) {
          if (+response.status >= 200 && +response.status < 300) {
            // TODO: Indicate success in UI
          }
          console.log(`Tag giphy response: ${response.status} ${response.statusText}`);
        }
      });
  }

  tagGiphy(id: string, tag: string): any {
    console.log(`tag giphy with id ${id} with tag ${tag}`);
    // TODO: Make http response to save giphy and tag to profile
  }
}
