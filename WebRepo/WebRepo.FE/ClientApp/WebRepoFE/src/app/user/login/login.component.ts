import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { CookieService } from 'ngx-cookie-service';
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

  constructor(private service: SharedService, private router : Router, private cookieService: CookieService) { }

  login(): void {
    this.service.login(this.conta).subscribe(
      data => {
        alert('Login successfully.')
        //console.log(data);

        /*console.log(name[0]);
        console.log(name[1]);
        console.log(formattedDate);
        console.log(path[1]);
        console.log(secure);
        console.log(samesite[1]);*/

        //this.cookieService.set(name[0], name[1], 1, path[1], "localhost" , secure, samesite[1]);

        this.service.token = data.token;

        this.router.navigateByUrl('content').then(() =>{
          this.router.navigate([decodeURI('content')]);
          location.reload();
        });
        console.log(this.cookieService.get('.AspNetCore.Application.Id'))
      },
      error => {
        alert('Login failed. Please check your credentials and try again.')
      }
    );
   }
}
