import { Component } from '@angular/core';
import { SharedService } from '../shared.service';
import { Router } from '@angular/router';
import { faPen } from '@fortawesome/free-solid-svg-icons';
import { ChangePhotoComponent } from './change-photo/change-photo.component';
import { MatDialog } from '@angular/material/dialog';

@Component({
  selector: 'app-user',
  templateUrl: './user.component.html',
  styleUrls: ['./user.component.css']
})
export class UserComponent {

  faPen = faPen

  account:any = {}

  constructor(private service: SharedService, private router: Router, private dialogReference : MatDialog) { }

  ngOnInit(): void {
    this.userInfo()
  }

  userInfo(): void {
    this.service.getUserbyEmail().subscribe(
      data => {
        this.account = data;
        this.account.photoURL = `https://localhost:7058/User/${this.account.id}/profile-photo`;
      },
      error => {
        alert("Error on loading user's data");
      }
    );
  }

  changeUserPhoto():void{
    this.dialogReference.open(ChangePhotoComponent)
  }

  /*deleteAccount():void{
    this.service.removeUser(this.account.id).subscribe(
      data =>{
        alert("Account removed successfully!")
        this.router.navigateByUrl('/login').then(() =>{
          location.reload();
          this.router.navigate([decodeURI('/login')]);
        });
      },
      error => {
        alert("Error on removing your account...")
      }
    )
  }*/
}
