import { HttpClient } from '@angular/common/http';
import { Component, Inject, OnInit, Output, EventEmitter } from '@angular/core';
import { Giphy } from '../../models/giphy.model';

@Component({
  selector: 'app-search-bar',
  templateUrl: './search-bar.component.html',
  styleUrls: ['./search-bar.component.css']
})
export class SearchBarComponent implements OnInit {
  inputText: string;
  baseUrl: string;
  http: HttpClient;

  @Output() searchCompleteEvent = new EventEmitter<Giphy[]>();

  constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    this.http = http;
    this.baseUrl = baseUrl;
  }

  ngOnInit() {
    // TODO: Use "trending" url instead
    this.http.get<Giphy[]>(`${this.baseUrl}Giphy/Search/cats`).subscribe((result: Giphy[]) => {
      console.log(result);
      this.updateGiphies(result);
    }, error => console.error(error));
  }

  onSubmit() {
    console.log(this.inputText);
    this.http.get<Giphy[]>(`${this.baseUrl}Giphy/Search/${this.inputText}`).subscribe(result => {
      console.log(result);
      this.updateGiphies(result);
    }, error => console.error(error));
  }

  updateGiphies(giphies: Giphy[]) {
    this.searchCompleteEvent.emit(giphies);
  }
}
