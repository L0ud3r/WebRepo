import { Component } from '@angular/core';
import { SharedService } from 'src/app/shared.service';
import { FolderNavigationService } from '../folder-navigation.service';
import { ElementRef, Renderer2 } from '@angular/core';

@Component({
  selector: 'app-favourite',
  templateUrl: './favourite.component.html',
  styleUrls: ['./favourite.component.css']
})
export class FavouriteComponent {
  userFiles : any = []
  userFilesPretty : any = []
  selectedFile: File | null
  selectedFileName: string = ""
  fileOn : boolean = false
  modelFiles = {
    offset: 0,
    limit: 0,
    userEmail: "pedro@gmail.com",
    searchParameter: [
      {
        fieldName: "Filename",
        fieldValue: ""
      },
      {
        fieldName: "Filetype",
        fieldValue: ""
      }
    ]
  }
  types : any = ["image/jpeg", "Folder"]

  constructor(private service : SharedService, private folderService : FolderNavigationService,
    private elementRef: ElementRef, private renderer: Renderer2) { this.selectedFile = null; }

  ngOnInit(): void {
    this.favouriteFilesByUser();
  }

  onFileSelected(event : any) {
    this.selectedFile = <File>event.target.files[0]
    this.selectedFileName = this.selectedFile.name
    this.fileOn = true
  }

  paginateFavouritesFiles() : void {
    if(this.modelFiles.searchParameter[1].fieldValue == 'All Types')
      this.modelFiles.searchParameter[1].fieldValue = ''

    if(this.modelFiles.searchParameter[1].fieldValue == '' && this.modelFiles.searchParameter[0].fieldValue == ''){
      this.favouriteFilesByUser();
    }
    else{
      this.service.paginateFavouriteFiles(this.modelFiles).subscribe(
        data => {
          console.log(data)
          this.userFiles = data.rows;
          this.userFilesPretty = data.rows;

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
          this.userFilesPretty = null
      })
    }
  }

  favouriteFilesByUser() : void {
    this.service.favouriteFilesByUser().subscribe(
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

  uploadFile() : void {
    const formData = new FormData();
    formData.append('file', this.selectedFile!, this.selectedFile!.name);

    this.service.uploadFile(formData, this.folderService.currentFolder).subscribe(
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
