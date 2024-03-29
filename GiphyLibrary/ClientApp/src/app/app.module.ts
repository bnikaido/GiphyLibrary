import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { MatPaginatorModule, MatTableModule, MatDialogModule, MatDialogRef, MAT_DIALOG_DATA, MatOptionModule, MatSelectModule } from '@angular/material';
import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { RouterModule } from '@angular/router';
import { ApiAuthorizationModule } from 'src/api-authorization/api-authorization.module';
import { AuthorizeGuard } from 'src/api-authorization/authorize.guard';
import { AuthorizeInterceptor } from 'src/api-authorization/authorize.interceptor';

import { AppComponent } from './app.component';
import { SearchGiphyTableComponent } from './components/search-giphy-table/search-giphy-table.component';
import { NavMenuComponent } from './components/nav-menu/nav-menu.component';
import { SearchBarComponent } from './components/search-bar/search-bar.component';
import { HomeComponent } from './pages/home/home.component';
import { TagGiphyDialogComponent } from './components/tag-giphy-dialog/tag-giphy-dialog.component';
import { GiphySearchComponent } from './pages/giphy-search/giphy-search.component';
import { GiphySavedComponent } from './pages/giphy-saved/giphy-saved.component';
import { FilterBarComponent } from './components/filter-bar/filter-bar.component';
import { SavedGiphyTableComponent } from './components/saved-giphy-table/saved-giphy-table.component';


@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    SearchGiphyTableComponent,
    SearchBarComponent,
    HomeComponent,
    TagGiphyDialogComponent,
    GiphySearchComponent,
    GiphySavedComponent,
    FilterBarComponent,
    SavedGiphyTableComponent
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    FormsModule,
    ApiAuthorizationModule,
    RouterModule.forRoot([
      { path: '', component: HomeComponent, pathMatch: 'full' },
      { path: 'giphy-search', component: GiphySearchComponent, canActivate: [AuthorizeGuard] },
      { path: 'giphy-saved', component: GiphySavedComponent, canActivate: [AuthorizeGuard] },
    ]),
    BrowserAnimationsModule,
    MatPaginatorModule,
    MatTableModule,
    MatDialogModule,
    MatOptionModule,
    MatSelectModule
  ],
  providers: [
    { provide: HTTP_INTERCEPTORS, useClass: AuthorizeInterceptor, multi: true },
    { provide: MatDialogRef, useValue: {} },
    { provide: MAT_DIALOG_DATA, useValue: [] }
  ],
  bootstrap: [AppComponent],
  entryComponents: [TagGiphyDialogComponent]
})
export class AppModule { }
