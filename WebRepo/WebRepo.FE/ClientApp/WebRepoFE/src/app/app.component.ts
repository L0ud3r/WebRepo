import { Component, HostListener } from '@angular/core';
import { Router } from '@angular/router';
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
  contextMenuX : number | null;
  contextMenuY : number | null;
  showContextMenu : boolean;
  account : any = {}

  constructor(private router: Router, private service : SharedService,
     private folderService : FolderNavigationService,
     @Inject(FileComponent) public fileComponent : FileComponent,
     private http: HttpClient, private sanitizer: DomSanitizer) {
    this.contextMenuX = 0;
    this.contextMenuY = 0;
    this.showContextMenu = false;
   }

  ngOnInit(){
    this.checkLayout();
    document.addEventListener('click', this.hideContextMenu.bind(this));
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

  onContextMenu(event: MouseEvent) {
    event.preventDefault();

    this.contextMenuX = event.clientX;
    this.contextMenuY = event.clientY;
    this.showContextMenu = true;
  }

  hideContextMenu() {
    this.contextMenuX = 0;
    this.contextMenuY = 0;
    this.showContextMenu = false;
  }

  checkLayout(){
    if(this.router.url === '/login'){
      this.isLoginLayout = true;
    }
  }

  @HostListener('click', ['$event'])
  onClickInsideContextMenu(event: MouseEvent) {
    event.stopPropagation();
    this.showContextMenu = false;
  }

  addFolder() : void{
    this.fileComponent.addFolder(this.folderService.currentFolder);
  }
}
