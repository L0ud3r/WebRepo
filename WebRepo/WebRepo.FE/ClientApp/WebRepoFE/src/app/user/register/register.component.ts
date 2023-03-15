import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { SharedService } from 'src/app/shared.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent {
  account : any = {}

  constructor(private service: SharedService, private router: Router){}

  register(){
    this.service.register(this.account).subscribe(
      data =>{
      alert("Account created successfully")
      this.router.navigateByUrl('content').then(() =>{
        this.router.navigate([decodeURI('content')]);
        location.reload();
      });
    },
    error => {
      alert("Error on creating account")
      console.log(error)
    })
  }
}
