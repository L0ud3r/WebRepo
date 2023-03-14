import { Component, HostListener } from '@angular/core';
import { SharedService } from '../shared.service';
import { MatDialog } from '@angular/material/dialog';
import { NgxFileDropEntry, FileSystemFileEntry, FileSystemDirectoryEntry } from 'ngx-file-drop';
import { DetailsComponent } from './details/details.component';
import { FolderNavigationService } from './folder-navigation.service';
import { NewFolderComponent } from './new-folder/new-folder.component';
import { EditFolderNameComponent } from './edit-folder-name/edit-folder-name.component';
import { EditFileNameComponent } from './edit-file-name/edit-file-name.component';

@Component({
  selector: 'app-file',
  templateUrl: './file.component.html',
  styleUrls: ['./file.component.css']
})
export class FileComponent {

  public files: NgxFileDropEntry[] = [];
  userFiles : any = []
  userFolders : any = []
  userFilesPretty : any = []
  selectedFile: File | null
  selectedFileName: string = ""
  fileOn : boolean = false
  currentFolder : number = 0;
  newFolder : any = {
    Name: "",
    IdCurrentDirectory: 0
  }
  edditedFile : any = {
    id: 0,
    fileName: ""
  }

  edditedFolder : any = {
    name: "",
    idCurrentDirectory: 0
  }

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

  contextMenuX : number | null;
  contextMenuY : number | null;
  showContextMenu : boolean;

  constructor(private service : SharedService, private folderService : FolderNavigationService, private dialogReference : MatDialog)
  {
    this.selectedFile = null;
    this.contextMenuX = 0;
    this.contextMenuY = 0;
    this.showContextMenu = false;
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

  openModal(file:any){
    for(let i = 0; i < this.userFiles.length; i++){
      if(this.userFiles[i].id == file.id){
        this.dialogReference.open(DetailsComponent, {data : this.userFiles[i]})
      }
    }
  }

  public dropped(event: any) {
    let files: NgxFileDropEntry[] = event.files;

    for (const droppedFile of files) {

      // Is it a file?
      if (droppedFile.fileEntry.isFile) {
        const fileEntry = droppedFile.fileEntry as FileSystemFileEntry;
        fileEntry.file((file: File) => {
          this.selectedFile = file;
        });
      } else {
        // It was a directory (empty directories are added, otherwise only files)
        const fileEntry = droppedFile.fileEntry as FileSystemDirectoryEntry;
        console.log(droppedFile.relativePath, fileEntry);
      }
    }
  }

  public fileOver(event:any){
    console.log(event);
  }

  public fileLeave(event:any){
    console.log(event);
  }

  ngOnInit(): void {
    this.getUserFolders(this.currentFolder);
    this.getUserFiles(this.currentFolder);
    document.addEventListener('click', this.hideContextMenu.bind(this));
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
        this.folderService.currentFolder = data;
      },
      error => {
        alert("Something went wrong");
      }
    )
  }

  @HostListener('click', ['$event'])
  onClickInsideContextMenu(event: MouseEvent) {
    event.stopPropagation();
    this.showContextMenu = false;
  }

  changeFolder(idFolder : number) : void{
    this.getUserFolders(idFolder);
    this.getUserFiles(idFolder);
    this.currentFolder = idFolder;
    this.folderService.currentFolder = idFolder;
  }

  getUserFiles(idFolder : number) : void {
    this.service.filesByUser(idFolder).subscribe(
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
        this.userFilesPretty = null;
    })
  }

  paginateFiles() : void {
    if(this.modelFiles.searchParameter[1].fieldValue == 'All Types')
      this.modelFiles.searchParameter[1].fieldValue = ''

    if(this.modelFiles.searchParameter[1].fieldValue == '' && this.modelFiles.searchParameter[0].fieldValue == ''){
      this.getUserFiles(0);
      this.getUserFolders(0);
    }
    else{
      this.service.paginateFiles(this.modelFiles).subscribe(
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

  paginateFolders() : void {
    if(this.modelFiles.searchParameter[1].fieldValue == 'All Types')
      this.modelFiles.searchParameter[1].fieldValue = ''

    if(this.modelFiles.searchParameter[1].fieldValue == '' && this.modelFiles.searchParameter[0].fieldValue == ''){
      this.getUserFiles(0);
      this.getUserFolders(0);
    }
    else{
      this.service.paginateFolders(this.modelFiles).subscribe(
        data => {
          this.userFolders = data.rows
      },
        error => {
          this.userFolders = null
      })
    }
  }

  paginate() : void {
    this.paginateFiles()
    this.paginateFolders()
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

    this.service.uploadFile(formData, this.folderService.currentFolder).subscribe(
      data => {
        alert("File upload successfully")
        this.getUserFolders(this.currentFolder);
        this.getUserFiles(this.currentFolder);
        this.fileOn = false;
      },
      error => {
        alert("Error on uploading file")
        console.log(error)
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

  getCurrentFolder(): number {
    return this.currentFolder;
  }


  addFolder(currentFolder : number) : void{
    this.dialogReference.open(NewFolderComponent, {data : currentFolder})
  }

  changeFilename(file : any) : void {
    this.dialogReference.open(EditFileNameComponent, {data : file.id})
  }

  changeFoldername(folder : any) : void {
    this.dialogReference.open(EditFolderNameComponent, {data : folder.id})
  }

  deleteFile(file : any) : void {

    this.edditedFile.id = file.id
    this.edditedFile.fileName = file.fileName

    this.service.deleteRecoverFile(this.edditedFile).subscribe(
      data => {
        alert("File deleted")
        this.getUserFolders(this.currentFolder);
        this.getUserFiles(this.currentFolder);
    },
      error => {
        alert("Error on deleting file")
        this.getUserFolders(this.currentFolder);
        this.getUserFiles(this.currentFolder);
        console.log(error)
    })
  }

  message(message : string) : void{
    alert(message)
  }
}
