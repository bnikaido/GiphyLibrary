import { Component } from '@angular/core';
import { Giphy } from '../../models/giphy.model';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
})
export class HomeComponent {
  giphies: Giphy[];

  updateGiphies(giphies: Giphy[]) {
    this.giphies = giphies;
  }
}
