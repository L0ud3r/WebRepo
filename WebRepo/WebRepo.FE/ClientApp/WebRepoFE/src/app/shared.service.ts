import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';


@Injectable({
  providedIn: 'root'
})
export class SharedService {
  readonly APIUrl="https://localhost:7058"
  token = ""

  constructor(private http:HttpClient) { }

  login(account : any):Observable<any>{
    return this.http.post<any>(this.APIUrl+'/User/login', account)
  }

  getUserbyEmail():Observable<any[]>{
    return this.http.get<any>(this.APIUrl+'/User/email', { headers: { Authorization: this.token } })
  }

  getUserPhoto():Observable<any>{
    return this.http.get<any>(this.APIUrl+'/User/email', { headers: { Authorization: this.token } })
  }

  updateUserPhoto(file : any):Observable<any>{
    return this.http.patch<any>(this.APIUrl+'/User/changephoto', file,{ headers: { Authorization: this.token } })
  }

  getDeletedFiles():Observable<any[]>{
    return this.http.get<any>(this.APIUrl+'/File/deleted', { headers: { Authorization: this.token } })
  }

  paginateFavouriteFiles(searchModel : any):Observable<any>{
    return this.http.post<any>(this.APIUrl+'/File/paginatefavourites', searchModel, { headers: { Authorization: this.token } })
  }

  paginateFiles(searchModel : any):Observable<any>{
    return this.http.post<any>(this.APIUrl+'/File/paginate', searchModel, { headers: { Authorization: this.token } })
  }

  paginateFolders(searchModel : any):Observable<any>{
    return this.http.post<any>(this.APIUrl+'/VirtualDirectory/paginate', searchModel, { headers: { Authorization: this.token } })
  }

  editUser(user : any):Observable<any>{
    return this.http.patch<any>(this.APIUrl+'/User', user, { headers: { Authorization: this.token } })
  }

  filesByUser(idCurrentFolder : number):Observable<any[]>{
    return this.http.get<any>(this.APIUrl+'/File/list?idCurrentFolder=' + idCurrentFolder,  { headers: { Authorization: this.token } })
  }

  patchFile(file : any):Observable<any>{
    return this.http.patch<any>(this.APIUrl+'/File', file, { headers: { Authorization: this.token } });
  }

  deleteRecoverFile(file : any):Observable<any>{
    return this.http.patch<any>(this.APIUrl+'/File/removerecover', file, { headers: { Authorization: this.token } });
  }

  foldersByUser(idCurrentFolder : number):Observable<any[]>{
    return this.http.get<any>(this.APIUrl+'/VirtualDirectory/folders?idCurrentFolder=' + idCurrentFolder,  { headers: { Authorization: this.token } })
  }

  getParentFolder(idCurrentFolder : number):Observable<any>{
    return this.http.get<any>(this.APIUrl+'/VirtualDirectory/getparent?idCurrentFolder=' + idCurrentFolder,  { headers: { Authorization: this.token } })
  }

  addFolder(folder : any):Observable<any>{
    return this.http.post<any>(this.APIUrl+'/VirtualDirectory', folder, { headers: { Authorization: this.token } });
  }

  patchFolder(folder : any):Observable<any>{
    return this.http.patch<any>(this.APIUrl+'/VirtualDirectory', folder, { headers: { Authorization: this.token } });
  }

  favouriteFilesByUser():Observable<any[]>{
    return this.http.get<any[]>(this.APIUrl+'/File/favourites',  { headers: {Authorization: this.token} })
  }

  uploadFile(file : any, idCurrentFolder : number):Observable<any>{
    return this.http.post<any>(this.APIUrl+'/File/uploadfile/' + idCurrentFolder, file, { headers: {Authorization: this.token} })
  }

  downloadFile(filename: string): Observable<Blob> {
    return this.http.get(this.APIUrl + '/File/downloadfile?filename=' + filename, {
      headers: { Authorization: this.token },
      responseType: 'blob'
    });
  }

  addRemoveFavourites(id : number): Observable<any>{
    return this.http.patch<any>(this.APIUrl + '/File/addremovefavourites', id, { headers: { Authorization: this.token }
    });
  }
}
