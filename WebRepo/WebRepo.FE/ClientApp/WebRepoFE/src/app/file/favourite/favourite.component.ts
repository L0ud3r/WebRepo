import { Component } from '@angular/core';
import { SharedService } from 'src/app/shared.service';
import { FolderNavigationService } from '../folder-navigation.service';
import { ElementRef, Renderer2 } from '@angular/core';
<<<<<<< Updated upstream
=======
import { MatDialog } from '@angular/material/dialog';
import { DetailsComponent } from '../details/details.component';
import { EditFileNameComponent } from '../edit-file-name/edit-file-name.component';

>>>>>>> Stashed changes

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

<<<<<<< Updated upstream
  constructor(private service : SharedService, private folderService : FolderNavigationService,
    private elementRef: ElementRef, private renderer: Renderer2) { this.selectedFile = null; }
=======
  contextMenuX : number | null;
  contextMenuY : number | null;
  showContextMenu : boolean;

  constructor(private service : SharedService, private folderService : FolderNavigationService,
    private elementRef: ElementRef, private renderer: Renderer2, private dialogReference : MatDialog)
    {
      this.selectedFile = null;
      this.contextMenuX = 0;
      this.contextMenuY = 0;
      this.showContextMenu = false;
     }
>>>>>>> Stashed changes

  ngOnInit(): void {
    this.favouriteFilesByUser();
    this.setActive();
  }

  openModal(file:any){
    for(let i = 0; i < this.userFiles.length; i++){
      if(this.userFiles[i].id == file.id){
        let detailedFile = {
          id : file.id,
          fileName : file.fileName,
          contentType : file.contentType,
          contentLength : file.contentLength,
          isFavourite : file.isFavourite,
          createdDate : file.createdDate,
        }

        this.dialogReference.open(DetailsComponent, {data : detailedFile})
      }
    }
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
        console.log(data)

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

  changeFilename(file : any) : void {
    this.dialogReference.open(EditFileNameComponent, {data : file.id})
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

        this.favouriteFilesByUser();

        const menuItems = document.getElementsByClassName('dropdown-menu show');

        // Remove 'active' class from all menu items
        for (let i = 0; i < menuItems.length; i++) {
          menuItems[i].classList.remove('show');
        }
      },
      error => {
        alert("Error!")
      })
  }

  setActive(): void {
    const menuItems = document.getElementsByClassName('nav-link');

    // Remove 'active' class from all menu items
    for (let i = 0; i < menuItems.length; i++) {
      menuItems[i].classList.remove('active');
    }

    // Add 'active' class to the clicked menu item
    const clickedItem = document.getElementById("side-fav");
    if (clickedItem) {
      clickedItem.classList.add('active');
    }
  }

  message(message : string) : void{
    alert(message)
  }
}
