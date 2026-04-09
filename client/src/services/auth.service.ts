import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs/internal/Observable';
import { LoginRequestDto, UserCreateDto, } from '../models/auth.model';
@Injectable({
  providedIn: 'root',
})
export class AuthService {
  private http = inject(HttpClient);
  private readonly apiUrl ='https://localhost:7211/api/Auth';


  register(userData: UserCreateDto): Observable<any> {
  
  return this.http.post(`${this.apiUrl}/register`, userData);
}
  login(userData: LoginRequestDto): Observable<any> {
  
  return this.http.post(`${this.apiUrl}/login`, userData);
}

}
