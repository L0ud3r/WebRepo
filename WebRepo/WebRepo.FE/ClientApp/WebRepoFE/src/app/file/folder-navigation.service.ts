import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class FolderNavigationService {

  currentFolder = 0

  constructor() { }
}
