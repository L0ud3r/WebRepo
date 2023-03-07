import { Component } from '@angular/core';
import { SharedService } from '../shared.service';

@Component({
  selector: 'app-file',
  templateUrl: './file.component.html',
  styleUrls: ['./file.component.css']
})
export class FileComponent {

  userFiles : any = []

  constructor(private service : SharedService) { }

  ngOnInit(): void {
    this.getUserFiles();
  }

  getUserFiles() : void {
    this.service.filesByUser().subscribe(
      data =>{
        this.userFiles = data;
    },
      error => {
        alert("Erro");
    })
  }

}
