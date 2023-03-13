import { Component, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { SharedService } from 'src/app/shared.service';

@Component({
  selector: 'app-change-photo',
  templateUrl: './change-photo.component.html',
  styleUrls: ['./change-photo.component.css']
})
export class ChangePhotoComponent {

  file: File | null = null;
  photoUrl: string | null = null;

  constructor(@Inject(MAT_DIALOG_DATA) public data : any,
  private dialogRef: MatDialogRef<ChangePhotoComponent>,
  private service: SharedService) { }

  onFileSelected(event: Event): void {
    const inputElement = event.target as HTMLInputElement;
    const files = inputElement.files;

    if (files && files.length) {
      this.file = files[0];

      const reader = new FileReader();
      reader.readAsDataURL(this.file);
      reader.onload = (e) => {
        this.photoUrl = reader.result as string;
      };
    }
  }

  changePicture(): void {
    console.log(this.file)
    const formData = new FormData();
    formData.append('file', this.file!, this.file!.name);

    this.service.updateUserPhoto(formData).subscribe(
      data => {
        alert("Profile pictured updated")
        this.closeModal()
        location.reload()
      },
      error => {
        alert("Error on changing profile picture")
        console.log(error)
        this.closeModal()
      })
  }

  closeModal() {
    this.dialogRef.close();
  }
}
