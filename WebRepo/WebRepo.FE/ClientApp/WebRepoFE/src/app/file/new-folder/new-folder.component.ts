import { Component, Inject } from '@angular/core';
import { MAT_DIALOG_DATA } from '@angular/material/dialog';
import { MatDialogRef } from '@angular/material/dialog';
import { SharedService } from 'src/app/shared.service';

@Component({
  selector: 'app-new-folder',
  templateUrl: './new-folder.component.html',
  styleUrls: ['./new-folder.component.css']
})
export class NewFolderComponent {

  newFolder : any = {}
  currentDirectory

  constructor(@Inject(MAT_DIALOG_DATA) public data : any,
  private dialogRef: MatDialogRef<NewFolderComponent>,
  private service : SharedService){
    this.currentDirectory = data
  }

  ngOnInit(): void {

  }

  createFolder(){
    this.newFolder.IdCurrentDirectory = this.currentDirectory

    this.service.addFolder(this.newFolder).subscribe(
      data => {
        alert("Folder created")
        this.closeModal()
        location.reload()
      },
      error => {
        alert("Error on creating folder")
        this.closeModal()
    })
  }

  closeModal() {
    this.dialogRef.close();
  }
}
