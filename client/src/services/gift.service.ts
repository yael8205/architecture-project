import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { inject } from '@angular/core';
import { GiftCreateDto, GiftDto } from '../models/gift.model';
import {} from '../models/gift.model';
import {GiftUpdateDto} from '../models/gift.model';
import { OrgService } from './org.service';
import { HttpClient, HttpParams } from '@angular/common/http';@Injectable({
  providedIn: 'root',
})
export class GiftService {
    private http = inject(HttpClient);
private orgService = inject(OrgService);
  currentOrg = this.orgService.currentOrg;
  private readonly apiUrl ='https://localhost:7211/api/Gift';
  private readonly categoryUrl = 'https://localhost:7211/api/Category'; 
  private readonly donorUrl = 'https://localhost:7211/api/Donor';
    getGifts() :Observable<GiftDto[]> {
    return this.http.get<GiftDto[]>(this.apiUrl);
  }

  getGiftById(id: number): Observable<GiftDto> {
    return this.http.get<GiftDto>(`${this.apiUrl}/${id}`);
  }

 createGift(gift: GiftCreateDto): Observable<GiftDto> {
    return this.http.post<GiftDto>(this.apiUrl, gift);
  }
  updateGift(id: number, gift: GiftUpdateDto): Observable<void> {
    return this.http.put<void>(`${this.apiUrl}/${id}`, gift);
  }

  // 5. מחיקת מתנה (DELETE)
  deleteGift(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }
  RunLottery(giftId: number): Observable<any> {
    return this.http.post(`${this.apiUrl}/run/${giftId}`, { giftId});
  }
getFilteredGifts(categoryId: number | null, priceType: number | null): Observable<GiftDto[]> {
  let params = new HttpParams();

  // שליחת קטגוריה רק אם נבחרה
  if (categoryId !== null && categoryId !== undefined) {
    params = params.set('categoryId', categoryId.toString());
  }

  // שליחת סוג מחיר רק אם נבחר (0 הוא ערך תקין לכן בודקים null)
  if (priceType !== null && priceType !== undefined) {
    params = params.set('priceType', priceType.toString());
  }

  return this.http.get<GiftDto[]>(`${this.apiUrl}/filter`, { params });
}
  searchGiftsAdmin(giftName?: string, donorName?: string, minPurchasers?: number) {
    let params = new HttpParams();
    if (giftName) params = params.set('giftName', giftName);
    if (donorName) params = params.set('donorName', donorName);
    if (minPurchasers) params = params.set('minPurchasers', minPurchasers.toString());

    return this.http.get<any[]>(`${this.apiUrl}/search`, { params });
  }
  getCategories(): Observable<any[]> {
    return this.http.get<any[]>(this.categoryUrl);
  }

  getDonors(): Observable<any[]> {
    return this.http.get<any[]>(this.donorUrl);
  }
  getSortedGiftsByOrg( sortBy: string): Observable<GiftDto[]> {
    const params = new HttpParams().set('sortBy', sortBy);
    return this.http.get<GiftDto[]>(`${this.apiUrl}/sort`, { params });
  }

}

