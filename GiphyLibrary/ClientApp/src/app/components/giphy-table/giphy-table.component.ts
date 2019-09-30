import { Component, Input, OnChanges, SimpleChanges } from '@angular/core';
import { Giphy } from '../../models/giphy.model';

@Component({
  selector: 'app-giphy-table',
  templateUrl: './giphy-table.component.html',
  styleUrls: ['./giphy-table.component.css']
})
export class GiphyTableComponent implements OnChanges {
  @Input() giphies: Giphy[];

  constructor() {
  }

  ngOnChanges(changes: SimpleChanges) {
    console.log(this.giphies);
  }

  saveGiphy() {
    console.log("save works!");
  }

  tagGiphy() {
    console.log("tag works!");
  }
}
