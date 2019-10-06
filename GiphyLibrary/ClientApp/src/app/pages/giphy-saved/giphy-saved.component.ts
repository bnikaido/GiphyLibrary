import { Component } from '@angular/core';
import { Giphy } from '../../models/giphy.model';

@Component({
  selector: 'app-giphy-saved',
  templateUrl: './giphy-saved.component.html',
})
export class GiphySavedComponent {
  giphies: Giphy[];

  updateGiphies(giphies: Giphy[]) {
    this.giphies = giphies;
  }
}
