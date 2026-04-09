import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { PackageInCartCreateDto } from '../models/packageInCart.model';
@Injectable({
  providedIn: 'root',
})
export class PackageInCartService {
   private http = inject(HttpClient);
  private readonly apiUrl ='https://localhost:7211/api/PackageInCart';
  private getHeaders() {
    const token = localStorage.getItem('token');
    return { 'Authorization': `Bearer ${token}` };
  }
  
  createMultiplePackages(packageData: PackageInCartCreateDto): Observable<any> {
    
    return this.http.post(this.apiUrl,  packageData,{ headers: this.getHeaders() });
  }
 
}