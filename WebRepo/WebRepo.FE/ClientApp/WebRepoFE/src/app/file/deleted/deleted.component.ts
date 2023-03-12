import { Component } from '@angular/core';
import { SharedService } from 'src/app/shared.service';
import { DetailsComponent } from '../details/details.component';
import { MatDialog } from '@angular/material/dialog';

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
    this.getDeletedFiles()
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
    file.fileName = 'PowerPoint Eddited'

    this.file.id = file.id
    this.file.fileName = file.fileName

    this.service.patchFile(this.file).subscribe(
      data => {
        alert("File renamed!")
        this.getDeletedFiles();
    },
      error=>{
        alert("Error on changing filename")
        this.getDeletedFiles();
        console.log(error)
    })
  }

  getDeletedFiles() : void {
    this.service.getDeletedFiles().subscribe(
      data =>{
        this.userFiles = data;
        this.userFilesPretty = data;

        for(let i = 0; i < this.userFilesPretty.length; i++){
          if(this.userFilesPretty[i].contentType == "application/vnd.openxmlformats-officedocument.presentationml.presentation")
            this.userFilesPretty[i].contentType = "PowerPoint"
            else if(this.userFilesPretty[i].contentType == "image/jpg")
            this.userFilesPretty[i].contentType = "JPG"
          else if(this.userFilesPretty[i].contentType == "image/jpeg")
            this.userFilesPretty[i].contentType = "JPEG"
          else if(this.userFilesPretty[i].contentType == "image/png")
            this.userFilesPretty[i].contentType = "PNG"
          else if(this.userFilesPretty[i].contentType == "application/pdf")
            this.userFilesPretty[i].contentType = "PDF"
          else if(this.userFilesPretty[i].contentType == "plain/text")
            this.userFilesPretty[i].contentType = "Text"

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
}
