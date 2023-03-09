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

  getUserbyToken():Observable<any[]>{
    return this.http.get<any>(this.APIUrl+'/User/token', { headers: { Authorization: this.token } })
  }

  filesByUser(idCurrentFolder : number):Observable<any[]>{
    return this.http.get<any>(this.APIUrl+'/File/list?idCurrentFolder=' + idCurrentFolder,  { headers: { Authorization: this.token } })
  }

  foldersByUser(idCurrentFolder : number):Observable<any[]>{
    return this.http.get<any>(this.APIUrl+'/VirtualDirectory/folders?idCurrentFolder=' + idCurrentFolder,  { headers: { Authorization: this.token } })
  }

  getParentFolder(idCurrentFolder : number):Observable<any>{
    return this.http.get<any>(this.APIUrl+'/VirtualDirectory/getparent?idCurrentFolder=' + idCurrentFolder,  { headers: { Authorization: this.token } })
  }

  favouriteFilesByUser():Observable<any[]>{
    return this.http.get<any[]>(this.APIUrl+'/File/favourites',  { headers: {Authorization: this.token} })
  }

  uploadFile(file : any):Observable<any>{
    return this.http.post<any>(this.APIUrl+'/File/uploadfile', file, { headers: {Authorization: this.token} })
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
