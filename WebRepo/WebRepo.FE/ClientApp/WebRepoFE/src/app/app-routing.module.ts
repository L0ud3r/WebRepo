import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { UserComponent } from './user/user.component';
import { LoginComponent } from './user/login/login.component';
import { FileComponent } from './file/file.component';
import { DeletedComponent } from './file/deleted/deleted.component';
import { FavouriteComponent } from './file/favourite/favourite.component';
import { EditComponent } from './user/edit/edit.component';

const routes: Routes = [
  { path: 'account', component: UserComponent },
  { path: 'login', component: LoginComponent },
  { path: 'content', component: FileComponent },
  { path: 'deleted', component: DeletedComponent },
  { path: 'favourite', component: FavouriteComponent },
  { path: 'account/edit', component: EditComponent }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
