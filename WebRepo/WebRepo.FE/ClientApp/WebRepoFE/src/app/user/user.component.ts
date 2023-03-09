import { Component } from '@angular/core';
import { SharedService } from '../shared.service';
import { Router } from '@angular/router';
import { faPen } from '@fortawesome/free-solid-svg-icons';

@Component({
  selector: 'app-user',
  templateUrl: './user.component.html',
  styleUrls: ['./user.component.css']
})
export class UserComponent {

  faPen = faPen

  account:any = {}

  constructor(private service: SharedService, private router: Router) { }

  ngOnInit(): void {
    this.userInfo()
  }

  userInfo(): void {
    this.service.getUserbyEmail().subscribe(
      data => {
        this.account = data;
        console.log(this.account)
      },
      error => {
        alert("Error on loading user's data");
      }
    );
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
