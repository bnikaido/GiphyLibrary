import { HttpClient, HttpResponse, HttpHeaders } from '@angular/common/http';
import { Component, Input, OnChanges, SimpleChanges, Inject } from '@angular/core';
import { MatDialog } from '@angular/material';
import { of, observable } from 'rxjs';
import { switchMap } from 'rxjs/operators';
import { Giphy } from '../../models/giphy.model';
import { TagGiphyDialogComponent } from '../tag-giphy-dialog/tag-giphy-dialog.component';

@Component({
  selector: 'app-giphy-table',
  templateUrl: './giphy-table.component.html',
  styleUrls: ['./giphy-table.component.css']
})
export class GiphyTableComponent implements OnChanges {
  @Input() giphies: Giphy[];
  http: HttpClient;
  baseUrl: string;

  constructor(
    public dialog: MatDialog,
    http: HttpClient,
    @Inject('BASE_URL') baseUrl: string) {
    this.http = http;
    this.baseUrl = baseUrl;
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
          // TODO: Fix login redirect
          if (+response.status >= 200 && +response.status < 300) {
            // TODO: Indicate success in UI
          }
          console.log(`Tag giphy response: ${response.status} ${response.statusText}`);
        }
      });
  }

  tagGiphy(id: string, tag: string): any {
    console.log(`tag giphy with id ${id} with tag ${tag}`);

    return this.http.post<string>(`${this.baseUrl}Account/SavedGiphies/${id}`, tag, {
      observe: 'response',
      headers: new HttpHeaders()
        .set('Content-Type', 'application/json')
        .set('Accept', 'application/json')
    });
  }
}
