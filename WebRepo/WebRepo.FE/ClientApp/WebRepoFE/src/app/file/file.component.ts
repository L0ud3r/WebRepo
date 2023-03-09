import { Component } from '@angular/core';
import { SharedService } from '../shared.service';

@Component({
  selector: 'app-file',
  templateUrl: './file.component.html',
  styleUrls: ['./file.component.css']
})
export class FileComponent {

  userFiles : any = []
  userFolders : any = []
  userFilesPretty : any = []
  selectedFile: File | null
  selectedFileName: string = ""
  fileOn : boolean = false
  currentFolder : number = 0;

  constructor(private service : SharedService) { this.selectedFile = null; }

  ngOnInit(): void {
    this.getUserFolders(0);
    this.getUserFiles(0);
  }

  onFileSelected(event : any) {
    this.selectedFile = <File>event.target.files[0]
    this.selectedFileName = this.selectedFile.name
    this.fileOn = true
  }

  goToParentFolder(idFolder : number) : void {
    this.service.getParentFolder(idFolder).subscribe(
      data => {
        this.getUserFolders(data);
        this.getUserFiles(data);
        this.currentFolder = data;
      },
      error => {
        alert("Something went wrong");
      }
    )

  }

  changeFolder(idFolder : number) : void{
    this.getUserFolders(idFolder);
    this.getUserFiles(idFolder);
    this.currentFolder = idFolder;
  }

  getUserFiles(idFolder : number) : void {
    this.service.filesByUser(idFolder).subscribe(
      data =>{
        this.userFiles = data;
        this.userFilesPretty = data;
        //console.log(data)

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
        }
    },
      error => {
        this.userFilesPretty = null;
    })
  }

  getUserFolders(idFolder : number) : void {
    this.service.foldersByUser(idFolder).subscribe(
      data =>{
        this.userFolders = data
    },
      error => {
        this.userFolders = null;
    })
  }

  uploadFile() : void {
    const formData = new FormData();
    formData.append('file', this.selectedFile!, this.selectedFile!.name);

    this.service.uploadFile(formData).subscribe(
      data => {
        alert("Success!")
        location.reload();
      },
      error => {
        alert("Error!")
      })
  }

  cancelUpload() : void {
    this.fileOn = false;
  }

  downloadFile(filename : string, fileIdentifier : string) : void {
    this.service.downloadFile(fileIdentifier).subscribe(
      (data) => {
        const blob = new Blob([data], { type: 'application/octet-stream' });
        const url = window.URL.createObjectURL(blob);
        const link = document.createElement('a');
        link.href = url;
        link.download = filename;
        document.body.appendChild(link);
        link.click();
        document.body.removeChild(link);
      },
      error => {
        alert("Error!")
    })
  }

  addRemoveFavourites(id : number) : void {
    alert(id)
    this.service.addRemoveFavourites(id).subscribe(
      data => {
        alert("Success!")
      },
      error => {
        alert("Error!")
      })
  }

  message(message : string) : void{
    alert(message)
  }
}
