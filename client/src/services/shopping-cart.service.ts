import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs/internal/Observable';
import { ShoppingCartCreate, ShoppingCartDto } from '../models/ShoppingCart.model';
import { tap } from 'rxjs/operators';
import { signal } from '@angular/core';
import { OrgService } from './org.service';
// import { GiftDto } from '../app/components/manager-gift/manager-gift';
import { GiftDto } from '../models/gift.model';
import { PackageInCartDto } from '../models/packageInCart.model';
import { CardPriceEnum } from '../models/gift.model';
@Injectable({
  providedIn: 'root',
})
export class ShoppingCartService {
  private http = inject(HttpClient);
  private orgService = inject(OrgService);
   private readonly apiUrl ='https://localhost:7211/api/ShoppingCart';
      private readonly apiUrl2 ='https://localhost:7211/api/PackageInCart';
      private readonly apiUrl3= 'https://localhost:7211/api/GiftInCart';
            private readonly apiUrl4= 'https://localhost:7211/api/Orsers';

public cart = signal<ShoppingCartDto | null>(null);
public focusedPackageId = signal<number | null>(null);

  private getHeaders() {
    const token = localStorage.getItem('token');
    return { 'Authorization': `Bearer ${token}` };
  }
createCart(userData:ShoppingCartCreate ): Observable<any> {
  
  return this.http.post(`${this.apiUrl}`, userData, { headers: this.getHeaders() });
}
getGiftImagePath(pictureUrl: string | null | undefined): string {
  const slug = this.orgService.currentOrg()?.slug || 'ezer-mizion';
  return `/${slug}/images/gifts/${pictureUrl}`;
}
    getShoppingCart() :Observable<ShoppingCartDto> {
 const userJson = localStorage.getItem('user');
const userObj = userJson ? JSON.parse(userJson) : null;
const id = userObj ? userObj.id : 0;
    return this.http.get<ShoppingCartDto>(`${this.apiUrl}/user/${id}`, { headers: this.getHeaders()});

  }
   deletePackageFromCart(packageId: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl2}/${packageId}`, { headers: this.getHeaders() });
  }
  // פונקציה חדשה להוספת מתנה לחבילה
 addGiftToPackage(giftId: number, packageInCartId: number, qty: number): Observable<any> {
  // בניית האובייקט עם שמות שדות שתואמים ל-DB ול-API
  const payload = { 
    giftId: giftId, 
    packageInCartId: packageInCartId, 
    qty: qty 
  };
  


  return this.http.post(`${this.apiUrl3}`, payload, { headers: this.getHeaders() })
    .pipe(
      // רק אחרי שה-POST מצליח, אנחנו מבצעים רענון
      tap((response) => {
        console.log('שמירה הצליחה בשרת:', response);
        // עדכון ה-Signal של הסל ישירות אם השרת מחזיר את הסל המעודכן
        // או קריאה לפונקציית רענון מסודרת
        this.refreshCartData(); 
      })
    );
}
// shopping-cart.service.ts

canAddGiftToPackage(gift: GiftDto, packageInCart: PackageInCartDto): boolean {
  // 1. נבדוק כמה כרטיסים מאותו סוג כבר יש בחבילה
  // אנחנו משתמשים ב- == כדי לוודא השוואה נכונה גם אם אחד מהם הוא מחרוזת והשני מספר
  const currentCount = packageInCart.giftsInPackage
    .filter(g => g.giftCardPrice.toString() === gift.cardPrice.toString())
    .reduce((sum, item) => sum + item.qty, 0);

  // 2. נבדוק מה המכסה המקסימלית של החבילה לסוג הזה
  let maxCapacity = 0;
  
  // המרת המחרוזת מה-DTO לערך מספרי של ה-Enum לצורך ה-Switch
  const priceType = gift.cardPrice as unknown as CardPriceEnum;

  switch (priceType) {
    case CardPriceEnum.Classic: 
      maxCapacity = packageInCart.qtyClassicCards; 
      break;
    case CardPriceEnum.Special: 
      maxCapacity = packageInCart.qtySpecialCards; 
      break;
    case CardPriceEnum.Primum: 
      maxCapacity = packageInCart.qtyPrimumCards; 
      break;
  }

  // 3. החזרת התוצאה
  return currentCount < maxCapacity;
}
// פונקציית עזר להוספת מתנה בקלות
addGiftToFocusedPackage(giftId: number) {
  const pkgId = this.focusedPackageId();
  if (!pkgId) return;

  // הוספת מתנה אחת (qty: 1)
  this.addGiftToPackage(giftId, pkgId, 1).subscribe();
}
public refreshCartData() {
  this.getShoppingCart().subscribe(cart => {
    this.cart.set(cart);
  });
}
 public createOrders(): Observable<any> {
    const currentCart = this.cart();
    if (!currentCart) {
      console.error('אין סל קניות פעיל לביצוע תשלום');
      return new Observable(observer => observer.error('אין סל קניות פעיל'));
    }
    return this.http.post(`${this.apiUrl4}`, currentCart, { headers: this.getHeaders() });
  }


}



