import { Component } from '@angular/core';
import { MAT_DIALOG_DATA } from '@angular/material/dialog';
import { MatDialogRef } from '@angular/material/dialog';
import { Inject } from '@angular/core';

@Component({
  selector: 'app-details',
  templateUrl: './details.component.html',
  styleUrls: ['./details.component.css']
})

export class DetailsComponent {
  id;
  filename;
  contentType;
  size;
  addedDate;
  isFavourite;
  postDate;


  constructor(@Inject(MAT_DIALOG_DATA) public data : any, private dialogRef: MatDialogRef<DetailsComponent>) {
    this.id = data.id;
    this.filename = data.fileName;
    this.contentType = data.contentType;
    this.size = data.contentLength;
    this.isFavourite = data.isFavourite;
    this.addedDate = data.createdDate;
    this.postDate = data.postDate;
  }

  ngOnInit():void{
  }

  closeModal() {
    this.dialogRef.close();
  }
}
