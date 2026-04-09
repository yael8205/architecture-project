import { Injectable } from '@angular/core';
import { catchError, Observable, tap, throwError } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { inject } from '@angular/core';
import { OrderDto } from '../models/order.model';
import { signal } from '@angular/core';
import { OrgService } from './org.service';
import { ShoppingCartDto } from '../models/ShoppingCart.model';
@Injectable({
  providedIn: 'root',
})
export class OrdersService {
  private http = inject(HttpClient);
  private apiUrl = 'https://localhost:7211/api/Orsers'; // ודאי שהפורט נכון לפי image_7dc7fb.png
private orgService = inject(OrgService);
  // שימוש ב-Signal לניהול הנתונים
  orders = signal<OrderDto[]>([]);
  loading = signal<boolean>(false);
private getHeaders() {
    const token = localStorage.getItem('token');
    return { 'Authorization': `Bearer ${token}` };
  }
  getGiftImagePath(pictureUrl: string | null | undefined): string {
  const slug = this.orgService.currentOrg()?.slug || 'united-hatzalah';
  console.log(`🎬 טוען תמונת מתנה מהנתיב: /${slug}/images/gifts/${pictureUrl}`);
  return `/${slug}/images/gifts/${pictureUrl}`;
}
 getMyOrders(): void {
  this.loading.set(true);
  this.http.get<OrderDto[]>(`${this.apiUrl}/ByParticipant`, { headers: this.getHeaders() }).subscribe({
    next: (data) => {
      // שינוי חשוב: מיון כך שההזמנה החדשה (ID גבוה) תהיה ראשונה
      const sortedData = data.sort((a, b) => b.id - a.id);
      this.orders.set(sortedData);
      this.loading.set(false);
    },
    error: (err) => {
      console.error('Error fetching orders', err);
      this.loading.set(false);
    }
  });
}
createOrder(cart: ShoppingCartDto): Observable<OrderDto> {
    this.loading.set(true);
    return this.http.post<OrderDto>(this.apiUrl, cart, { headers: this.getHeaders() }).pipe(
      tap((newOrder) => {
        // רענון רשימת ההזמנות לאחר הצלחה
        this.getMyOrders();
        this.loading.set(false);
      }),
      catchError((err) => {
        this.loading.set(false);
        
        // כאן אנחנו תופסים את ה-Exception שזרקנו ב-C# (למשל: "המתנה כבר הוגרלה")
        const errorMessage = err.error?.message || err.error || 'שגיאה בביצוע ההזמנה';
        alert(errorMessage); // או שימוש ב-ToastService אם יש לך
        return throwError(() => err);
      })
    );
  }
  
}
