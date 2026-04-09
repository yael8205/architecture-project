import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class GiftInCartService {
  private http = inject(HttpClient);
  private readonly apiUrl = 'https://localhost:7211/api/GiftInCart';

  private getHeaders() {
    const token = localStorage.getItem('token');
    return { 'Authorization': `Bearer ${token}` };
  }

  // הוספת מתנה לחבילה או עדכון כמות (CreateOrUpdate כפי שכתבת ב-C#)
  createOrUpdate(giftInCart: any): Observable<any> {
    return this.http.post<any>(this.apiUrl, giftInCart, { headers: this.getHeaders() });
  }

  // מחיקת מתנה מהסל (הפונקציה שהייתה חסרה)
  delete(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`, { headers: this.getHeaders() });
  }
}