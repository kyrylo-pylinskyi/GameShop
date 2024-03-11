import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../environments/environment.development';

interface User{
  email: string;
  password: string;
}

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  constructor(private http: HttpClient) { }

  login( user: User ) {
    return this.http.post(`${environment.apiUrl}${environment.loginPostRequestUrl}`, user);
  }
}
