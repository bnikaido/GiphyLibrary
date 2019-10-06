import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Component, EventEmitter, Inject, OnInit, Output } from '@angular/core';
import { Giphy } from '../../models/giphy.model';


@Component({
  selector: 'app-filter-bar',
  templateUrl: './filter-bar.component.html',
  styleUrls: ['../search-bar.css']
})
export class FilterBarComponent implements OnInit {
  inputText: string;
  baseUrl: string;
  http: HttpClient;
  savedGiphies: Giphy[];

  @Output() updateCompleteEvent = new EventEmitter<Giphy[]>();

  constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    this.http = http;
    this.http.options(baseUrl, {
      observe: 'response',
      headers: new HttpHeaders()
        .set('Accept', 'application/json')
    });
    this.baseUrl = baseUrl;
  }

  ngOnInit() {
    this.http.get<Giphy[]>(`${this.baseUrl}Account/SavedGiphies`).subscribe((result: Giphy[]) => {
      console.log(result);
      this.savedGiphies = result;
      this.updateGiphies(result);
    }, error => console.error(error));
  }

  onSubmit() {
    if (!this.inputText || this.inputText.length == 0 || !this.inputText.trim()) {
      this.updateGiphies(this.savedGiphies);
    }
    else {
      var tags = this.inputText.toLowerCase().split(',');
      // Note: filters for giphies that include any of the listed tags (case insensitive)
      var filteredGiphies = this.savedGiphies.filter(function (giphy) {
        return giphy.tags.some(function (tag) {
          return tags.includes(tag.toLowerCase());
        });
      });
      this.updateGiphies(filteredGiphies);
    }
  }

  updateGiphies(giphies: Giphy[]) {
    this.updateCompleteEvent.emit(giphies);
  }
}
