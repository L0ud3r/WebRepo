import { Component, Inject } from '@angular/core';
import { MAT_DIALOG_DATA } from '@angular/material/dialog';
import { MatDialogRef } from '@angular/material/dialog';
import { SharedService } from 'src/app/shared.service';

@Component({
  selector: 'app-edit-folder-name',
  templateUrl: './edit-folder-name.component.html',
  styleUrls: ['./edit-folder-name.component.css']
})
export class EditFolderNameComponent {

  edittedFolder : any = {}

  constructor(@Inject(MAT_DIALOG_DATA) public data : any,
  private dialogRef: MatDialogRef<EditFolderNameComponent>,
  private service : SharedService){
    this.edittedFolder.idCurrentDirectory = data
  }

  editFolder(){
    this.service.patchFolder(this.edittedFolder).subscribe(
      data => {
        alert("Folder renamed!")
        location.reload()
        this.closeModal()
    },
      error=>{
        alert("Error on changing folder name")
        console.log(error)
        this.closeModal()
    })
  }

  closeModal() {
    this.dialogRef.close();
  }
}
