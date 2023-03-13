import { Component, Inject } from '@angular/core';
import { MAT_DIALOG_DATA } from '@angular/material/dialog';
import { MatDialogRef } from '@angular/material/dialog';
import { SharedService } from 'src/app/shared.service';

@Component({
  selector: 'app-edit-file-name',
  templateUrl: './edit-file-name.component.html',
  styleUrls: ['./edit-file-name.component.css']
})
export class EditFileNameComponent {

  edittedFile : any = {}

  constructor(@Inject(MAT_DIALOG_DATA) public data : any,
  private dialogRef: MatDialogRef<EditFileNameComponent>,
  private service: SharedService){
    this.edittedFile.id = data
  }

  editFile(){
    this.service.patchFile(this.edittedFile).subscribe(
      data => {
        alert("File renamed!")
        location.reload()
        this.closeModal()
    },
      error=>{
        alert("Error on changing filename")
        console.log(error)
        this.closeModal()
    })
  }

  closeModal() {
    this.dialogRef.close();
  }
}
