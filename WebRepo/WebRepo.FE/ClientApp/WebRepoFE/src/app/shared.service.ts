import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, ObservedValueOf } from 'rxjs';


@Injectable({
  providedIn: 'root'
})
export class SharedService {
  //readonly APIUrl="https://localhost:7058"
  readonly APIUrl = "https://6d0d-149-90-178-195.eu.ngrok.io"
  token = ""

  constructor(private http:HttpClient) { }

  login(account : any):Observable<any>{
    return this.http.post<any>(this.APIUrl+'/User/login', account, { headers: { "Access-Control-Allow-Origin": "*" } })
  }

  test():Observable<any>{
    return this.http.get<any>("https://postman-echo.com/get?foo1=bar1&foo2=bar2", { headers: { "Access-Control-Allow-Origin": "*"} , "withCredentials": true })
  }
  //this.APIUrl+'/File', { headers: { 'Access-Control-Allow-Origin': '*' }}
  register(account : any):Observable<any>{
    return this.http.post<any>(this.APIUrl+'/User', account)
  }

  getUserbyEmail():Observable<any[]>{
    return this.http.get<any>(this.APIUrl+'/User/email', { headers: { Authorization: this.token,  "Access-Control-Allow-Origin": "*"  } })
  }

  getUserPhoto():Observable<any>{
    return this.http.get<any>(this.APIUrl+'/User/email', { headers: { Authorization: this.token,  "Access-Control-Allow-Origin": "*"  } })
  }

  updateUserPhoto(file : any):Observable<any>{
    return this.http.patch<any>(this.APIUrl+'/User/changephoto', file,{ headers: { Authorization: this.token,  "Access-Control-Allow-Origin": "*"  } })
  }

  getDeletedFiles():Observable<any[]>{
    return this.http.get<any>(this.APIUrl+'/File/deleted', { headers: { Authorization: this.token,  "Access-Control-Allow-Origin": "*"  } })
  }

  paginateFavouriteFiles(searchModel : any):Observable<any>{
    return this.http.post<any>(this.APIUrl+'/File/paginatefavourites', searchModel, { headers: { Authorization: this.token,  "Access-Control-Allow-Origin": "*"  } })
  }

  paginateFiles(searchModel : any):Observable<any>{
    return this.http.post<any>(this.APIUrl+'/File/paginate', searchModel, { headers: { Authorization: this.token,  "Access-Control-Allow-Origin": "*"  } })
  }

  paginateFolders(searchModel : any):Observable<any>{
    return this.http.post<any>(this.APIUrl+'/VirtualDirectory/paginate', searchModel, { headers: { Authorization: this.token,  "Access-Control-Allow-Origin": "*"  } })
  }

  editUser(user : any):Observable<any>{
    return this.http.patch<any>(this.APIUrl+'/User', user, { headers: { Authorization: this.token,  "Access-Control-Allow-Origin": "*"  } })
  }

  filesByUser(idCurrentFolder : number):Observable<any[]>{
    return this.http.get<any>(this.APIUrl+'/File/list?idCurrentFolder=' + idCurrentFolder,  { headers: { Authorization: this.token,  "Access-Control-Allow-Origin": "*"  } })
  }

  patchFile(file : any):Observable<any>{
    return this.http.patch<any>(this.APIUrl+'/File', file, { headers: { Authorization: this.token,  "Access-Control-Allow-Origin": "*"  } });
  }

  deleteRecoverFile(file : any):Observable<any>{
    return this.http.patch<any>(this.APIUrl+'/File/removerecover', file, { headers: { Authorization: this.token,  "Access-Control-Allow-Origin": "*"  } });
  }

  foldersByUser(idCurrentFolder : number):Observable<any[]>{
    return this.http.get<any>(this.APIUrl+'/VirtualDirectory/folders?idCurrentFolder=' + idCurrentFolder,  { headers: { Authorization: this.token,
      "Access-Control-Allow-Origin": "*", 'Access-Control-Allow-Headers': 'X-Requested-With,content-type',
      'Access-Control-Allow-Methods': 'GET, POST, OPTIONS, PUT, PATCH, DELETE'  } })
  }

  getParentFolder(idCurrentFolder : number):Observable<any>{
    return this.http.get<any>(this.APIUrl+'/VirtualDirectory/getparent?idCurrentFolder=' + idCurrentFolder,  { headers: { Authorization: this.token,  "Access-Control-Allow-Origin": "*"  } })
  }

  addFolder(folder : any):Observable<any>{
    return this.http.post<any>(this.APIUrl+'/VirtualDirectory', folder, { headers: { Authorization: this.token,  "Access-Control-Allow-Origin": "*"  } });
  }

  patchFolder(folder : any):Observable<any>{
    return this.http.patch<any>(this.APIUrl+'/VirtualDirectory', folder, { headers: { Authorization: this.token,  "Access-Control-Allow-Origin": "*"  } });
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
    return this.http.patch<any>(this.APIUrl + '/File/addremovefavourites', id, { headers: { Authorization: this.token,  "Access-Control-Allow-Origin": "*"  }
    });
  }
}
