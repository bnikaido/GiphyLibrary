import { Component } from '@angular/core';
import { Giphy } from '../../models/giphy.model';

@Component({
  selector: 'app-giphy-search',
  templateUrl: './giphy-search.component.html',
})
export class GiphySearchComponent {
  giphies: Giphy[];

  updateGiphies(giphies: Giphy[]) {
    this.giphies = giphies;
  }
}
