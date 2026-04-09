import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { DonorDto, DonorCreateDto, DonorUpdateDto } from '../models/donor.model';
import { HttpParams } from '@angular/common/http';

@Injectable({
  providedIn: 'root',
})
export class DonorService {
  private http = inject(HttpClient);
  private readonly apiUrl = 'https://localhost:7211/api/Donor';

  // פונקציה פרטית לקבלת ה-Headers עם ה-Token
  private getHeaders() {
    const token = localStorage.getItem('token');
    return { 'Authorization': `Bearer ${token}` };
  }

  // שליפת כל התורמים
  getDonors(): Observable<DonorDto[]> {
    return this.http.get<DonorDto[]>(this.apiUrl, { headers: this.getHeaders() });
  }

  // שליפת תורם לפי ID
  getDonorById(id: number): Observable<DonorDto> {
    return this.http.get<DonorDto>(`${this.apiUrl}/${id}`, { headers: this.getHeaders() });
  }

  // יצירת תורם חדש (Name, Phone, Email)
  createDonor(donor: DonorCreateDto): Observable<DonorDto> {
    return this.http.post<DonorDto>(this.apiUrl, donor, { headers: this.getHeaders() });
  }

  // עדכון תורם קיים
 updateDonor(id: number, donor: DonorUpdateDto): Observable<void> {
  // אנחנו מוודאים שה-ID עובר בנתיב, ורק הנתונים הרלוונטיים עוברים ב-Body
  const body = {
    name: donor.name,
    email: donor.email,
    phone: donor.phone
  };
  return this.http.put<void>(`${this.apiUrl}/${id}`, body, { headers: this.getHeaders() });
}

  // מחיקת תורם
  deleteDonor(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`, { headers: this.getHeaders() });
  }


  getFilteredDonors(name?: string, email?: string, giftName?: string): Observable<DonorDto[]> {
    let params = new HttpParams();

    if (name) params = params.set('name', name);
    if (email) params = params.set('email', email);
    if (giftName) params = params.set('giftName', giftName);

    return this.http.get<DonorDto[]>(`${this.apiUrl}/filter`, { params, headers: this.getHeaders() });
  }

}
