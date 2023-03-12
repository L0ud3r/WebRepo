import { Component, HostListener } from '@angular/core';
import { Router } from '@angular/router';
import { SharedService } from './shared.service';
import { FileComponent } from './file/file.component';
import { Inject } from '@angular/core';

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

  constructor(private router: Router, private service : SharedService, @Inject(FileComponent) public fileComponent : FileComponent) {
    this.contextMenuX = 0;
    this.contextMenuY = 0;
    this.showContextMenu = false;
   }

  ngOnInit(){
    this.checkLayout();
    document.addEventListener('click', this.hideContextMenu.bind(this));
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
    this.fileComponent.addFolder(this.service.currentFolder);
  }
}
