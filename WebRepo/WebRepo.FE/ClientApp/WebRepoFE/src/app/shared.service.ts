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

  login(account:any):Observable<any>{
    return this.http.post<any>(this.APIUrl+'/User/login', account)
  }

  filesByUser():Observable<any[]>{
    return this.http.get<any>(this.APIUrl+'/File/list',  { headers: {Authorization: this.token} })
  }

  /*getCookie(name: string): string {
    const value = `; ${document.cookie}`;
    const parts = value.split(`; ${name}=`);
    if (parts.length === 2) {
      return parts.pop()!.split(';').shift()!;
    }
    return "";
  }*/
}
