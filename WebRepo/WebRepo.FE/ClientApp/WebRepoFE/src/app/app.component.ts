import { Component, HostListener } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { SharedService } from './shared.service';
import { DomSanitizer, SafeUrl } from '@angular/platform-browser';
import { FileComponent } from './file/file.component';
import { HttpClient } from '@angular/common/http';
import { Inject } from '@angular/core';
import { FolderNavigationService } from './file/folder-navigation.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  title = 'WebRepoFE';
  isLoginLayout : boolean = false;
  account : any = {}

  constructor(private router: Router, private service : SharedService,
     private folderService : FolderNavigationService,
     @Inject(FileComponent) public fileComponent : FileComponent,
     private http: HttpClient, private sanitizer: DomSanitizer,
     private activatedRoute: ActivatedRoute) {

   }

  ngOnInit(){
    this.checkLayout();
    if(!this.isLoginLayout)
      this.userInfo();
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

  checkLayout(){
    if(window.location.href === 'http://localhost:4200/register' || window.location.href === 'http://localhost:4200/login' ||
     window.location.href === 'http://localhost:4200/' || window.location.href === 'http://localhost:4200')
      this.isLoginLayout = true
    else
      this.isLoginLayout = false
  }


  addFolder() : void{
    this.fileComponent.addFolder(this.folderService.currentFolder);
  }
}
