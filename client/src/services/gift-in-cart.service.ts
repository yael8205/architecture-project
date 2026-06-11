import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class GiftInCartService {
  private http = inject(HttpClient);
  private readonly apiUrl = 'https://localhost:7211/api/GiftInCart';

  createOrUpdate(giftInCart: any): Observable<any> {
    return this.http.post<any>(this.apiUrl, giftInCart);
  }

  delete(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }
}
