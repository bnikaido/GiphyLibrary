import { HttpClient, HttpHeaders, HttpResponse } from '@angular/common/http';
import { Component, Inject, Input, OnChanges, SimpleChanges } from '@angular/core';
import { MatDialog } from '@angular/material';
import { of } from 'rxjs';
import { switchMap } from 'rxjs/operators';
import { Giphy } from '../../models/giphy.model';
import { TagGiphyDialogComponent } from '../tag-giphy-dialog/tag-giphy-dialog.component';

@Component({
  selector: 'app-saved-giphy-table',
  templateUrl: './saved-giphy-table.component.html',
  styleUrls: ['./saved-giphy-table.component.css']
})
export class SavedGiphyTableComponent implements OnChanges {
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

  removeGiphy(id: string) {
    console.log(`saved ${id}!`);
    this.http.delete<string>(`${this.baseUrl}Account/SavedGiphies/${id}`, {
      observe: 'response'
    }).subscribe((response: any) => {
      if (response instanceof HttpResponse) {
        this.handleResponse(response);
        // Note: Finds the removed giphy and removes it from the displayed array (avoids htpp call)
        for (var i = 0; i < this.giphies.length; i++) {
          if (this.giphies[i].id === id) {
            this.giphies.splice(i, 1);
          }
        }
        console.log(`Remove giphy response: ${response.status} ${response.statusText}`);
      }
    });
  }

  openTagGiphyDialog(id: string) {
    console.log(`tagged ${id}`);
    const dialogRef = this.dialog.open(TagGiphyDialogComponent, {
      // Todo: change input and get user data
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
          this.handleResponse(response);
          console.log(`Tag giphy response: ${response.status} ${response.statusText}`);
        }
      });
  }

  tagGiphy(id: string, tag: string) {
    console.log(`tag giphy with id ${id} with tag ${tag}`);
    return this.http.post<string>(`${this.baseUrl}Account/TagGiphy/${id}`, JSON.stringify(tag), {
      observe: 'response',
      headers: new HttpHeaders()
        .set('Content-Type', 'application/json')
    });
  }

  handleResponse(response: HttpResponse<any>) {
    if (response.ok) {
      // TODO: Indicate success in UI
    }
    else if (response.status >= 300 && response.status < 400) {
      // TODO: Handle redirect
    }
    else {
      // TODO: Indicate failure
    }
  }
}
