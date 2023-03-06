import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { RouterModule } from '@angular/router';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { NgbPaginationModule, NgbAlertModule } from '@ng-bootstrap/ng-bootstrap';

import { AppComponent } from './app.component';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { UserComponent } from './user/user.component';
import { LoginComponent } from './user/login/login.component';
import { FileComponent } from './file/file.component';
import { FavouriteComponent } from './file/favourite/favourite.component';
import { DeletedComponent } from './file/deleted/deleted.component';

@NgModule({
  declarations: [
    AppComponent,
    UserComponent,
    LoginComponent,
    FileComponent,
    FavouriteComponent,
    DeletedComponent
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    NgbModule,
    NgbAlertModule,
    FormsModule,
    HttpClientModule,
    NgbPaginationModule,
    FontAwesomeModule,
    RouterModule.forRoot([
      { path: '', component: LoginComponent, pathMatch: 'full' },
    ])
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
