import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { SharedService } from 'src/app/shared.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent {

  conta:any= {
    Email: "",
    Password: ""
  };

  constructor(private service: SharedService, private router : Router) { }

  ngOnInit(): void {
  }

  login(): void {
    this.service.login(this.conta).subscribe(
      data => {
        alert('Login succeded.')
        console.log(data)
      },
      error => {
        alert('Login failed. Please check your credentials and try again.')
      }
    );
   }
}
