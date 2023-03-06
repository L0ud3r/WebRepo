import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class SharedService {
  readonly APIUrl="https://localhost:7058"

  constructor(private http:HttpClient) { }

  login(account:any):Observable<any[]>{
    return this.http.post<any>(this.APIUrl+'/User/login', account)
  }
}
