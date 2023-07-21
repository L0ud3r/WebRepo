import { Component } from '@angular/core';
import { SharedService } from 'src/app/shared.service';
import { MatDialog } from '@angular/material/dialog';
import { DetailsComponent } from '../details/details.component';
import { EditFileNameComponent } from '../edit-file-name/edit-file-name.component';

@Component({
  selector: 'app-deleted',
  templateUrl: './deleted.component.html',
  styleUrls: ['./deleted.component.css']
})
export class DeletedComponent {
  userFiles : any = []
  userFilesPretty : any = []
  file : any = {
    id: 0,
    fileName: ""
  }

  constructor(private service : SharedService, private dialogReference : MatDialog) { }

  ngOnInit(): void {
    this.getDeletedFiles();
    this.setActive();
  }

  openModal(file:any){
    for(let i = 0; i < this.userFiles.length; i++){
      if(this.userFiles[i].id == file.id){
        this.dialogReference.open(DetailsComponent, {data : this.userFiles[i]})
      }
    }
  }

  recoverFile(file : any) : void {

    this.file.id = file.id
    this.file.fileName = file.fileName

    this.service.deleteRecoverFile(this.file).subscribe(
      data => {
        alert("File recovered")
        this.getDeletedFiles();
    },
      error => {
        alert("Error on recovering file")
        this.getDeletedFiles();
        console.log(error)
    })
  }

  changeFilename(file : any) : void {
    this.dialogReference.open(EditFileNameComponent, {data : file.id})
  }

  getDeletedFiles() : void {
    this.service.getDeletedFiles().subscribe(
      data =>{
        this.userFiles = data;
        this.userFilesPretty = data;

        for(let i = 0; i < this.userFilesPretty.length; i++){
          if (this.userFilesPretty[i].contentType == "text/plain")
            this.userFilesPretty[i].contentType = "Text";
          else if (this.userFilesPretty[i].contentType == "text/html")
            this.userFilesPretty[i].contentType = "HTML";
          else if (this.userFilesPretty[i].contentType == "image/jpg" || this.userFilesPretty[i].contentType == "image/jpeg")
            this.userFilesPretty[i].contentType = "JPEG";
          else if (this.userFilesPretty[i].contentType == "image/png")
            this.userFilesPretty[i].contentType = "PNG";
          else if (this.userFilesPretty[i].contentType == "image/gif")
            this.userFilesPretty[i].contentType = "GIF";
          else if (this.userFilesPretty[i].contentType == "image/bmp")
            this.userFilesPretty[i].contentType = "BMP";
          else if (this.userFilesPretty[i].contentType == "image/svg+xml")
            this.userFilesPretty[i].contentType = "SVG";
          else if (this.userFilesPretty[i].contentType == "audio/wav" || this.userFilesPretty[i].contentType == "audio/x-wav")
            this.userFilesPretty[i].contentType = "WAV";
          else if (this.userFilesPretty[i].contentType == "audio/mpeg")
            this.userFilesPretty[i].contentType = "MP3";
          else if (this.userFilesPretty[i].contentType == "audio/x-ms-wma")
            this.userFilesPretty[i].contentType = "WMA";
          else if (this.userFilesPretty[i].contentType == "application/json")
            this.userFilesPretty[i].contentType = "JSON";
          else if (this.userFilesPretty[i].contentType == "application/xml")
            this.userFilesPretty[i].contentType = "XML";
          else if (this.userFilesPretty[i].contentType == "application/pdf")
            this.userFilesPretty[i].contentType = "PDF";
          else if (this.userFilesPretty[i].contentType == "application/vnd.openxmlformats-officedocument.wordprocessingml.document")
            this.userFilesPretty[i].contentType = "Word";
          else if (this.userFilesPretty[i].contentType == "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
            this.userFilesPretty[i].contentType = "Open XML";
          else if (this.userFilesPretty[i].contentType == "application/vnd.ms-powerpoint"
          || this.userFilesPretty[i].contentType == "application/vnd.openxmlformats-officedocument.presentationml.presentation")
            this.userFilesPretty[i].contentType = "PowerPoint"
          else if (this.userFilesPretty[i].contentType == "video/mp4")
            this.userFilesPretty[i].contentType = "MP4";
          else if (this.userFilesPretty[i].contentType == "video/x-msvideo")
            this.userFilesPretty[i].contentType = "AVI";
          else if (this.userFilesPretty[i].contentType == "video/quicktime")
            this.userFilesPretty[i].contentType = "MOV";
          else if (this.userFilesPretty[i].contentType == "video/x-ms-wmv")
            this.userFilesPretty[i].contentType = "WMV";
          else if (this.userFilesPretty[i].contentType == "video/x-matroska")
            this.userFilesPretty[i].contentType = "MKV";
          else if (this.userFilesPretty[i].contentType == "video/x-flv")
            this.userFilesPretty[i].contentType = "FLV";

          this.userFilesPretty[i].contentLength = parseInt(Math.round(this.userFilesPretty[i].contentLength / 1000).toString())

          /*
          // Assuming your backend returns the created date as a string
          const createdDateString = this.userFilesPretty[i].createdDate;
          const createdDate = new Date(createdDateString);
          const currentDate = new Date();
          const diffMilliseconds = currentDate.getTime() - createdDate.getTime();

          // Convert milliseconds to the desired time unit
          const diffSeconds = Math.floor(diffMilliseconds / 1000);
          const diffMinutes = Math.floor(diffSeconds / 60);
          const diffHours = Math.floor(diffMinutes / 60);
          const diffDays = Math.floor(diffHours / 24);
          const diffMonths = Math.floor(diffDays / 30);
          const diffYears = Math.floor(diffDays / 365);

          if (diffYears > 0) {
            this.userFilesPretty[i].createdDate = `${diffYears} year${diffYears > 1 ? 's' : ''} ago`;
          } else if (diffMonths > 0) {
            this.userFilesPretty[i].createdDate = `${diffMonths} month${diffMonths > 1 ? 's' : ''} ago`;
          } else if (diffDays > 0) {
            this.userFilesPretty[i].createdDate = `${diffDays} day${diffDays > 1 ? 's' : ''} ago`;
          } else if (diffHours > 0) {
            this.userFilesPretty[i].createdDate = `${diffHours} hour${diffHours > 1 ? 's' : ''} ago`;
          } else if (diffMinutes > 0) {
            this.userFilesPretty[i].createdDate = `${diffMinutes} minute${diffMinutes > 1 ? 's' : ''} ago`;
          } else {
            this.userFilesPretty[i].createdDate = `${diffSeconds} second${diffSeconds > 1 ? 's' : ''} ago`;
          }*/
        }
    },
      error => {
        console.log(error)
        this.userFilesPretty = null;
    })
  }

  deleteFile(file : any) : void {
    this.file.id = file.id
    this.file.fileName = file.fileName

    this.service.deleteRecoverFile(this.file).subscribe(
      data => {
        alert("File deleted")
        this.getDeletedFiles();
    },
      error => {
        alert("Error on deleting file")
        this.getDeletedFiles();
        console.log(error)
    })
  }

  setActive(): void {
    const menuItems = document.getElementsByClassName('nav-link');

    // Remove 'active' class from all menu items
    for (let i = 0; i < menuItems.length; i++) {
      menuItems[i].classList.remove('active');
    }

    // Add 'active' class to the clicked menu item
    const clickedItem = document.getElementById("side-del");
    if (clickedItem) {
      clickedItem.classList.add('active');
    }
  }
}
