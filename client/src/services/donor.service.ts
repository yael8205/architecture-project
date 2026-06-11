import { Injectable, inject } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { DonorDto, DonorCreateDto, DonorUpdateDto } from '../models/donor.model';

@Injectable({
  providedIn: 'root',
})
export class DonorService {
  private http = inject(HttpClient);
  private readonly apiUrl = 'https://localhost:7211/api/Donor';

  getDonors(): Observable<DonorDto[]> {
    return this.http.get<DonorDto[]>(this.apiUrl);
  }

  getDonorById(id: number): Observable<DonorDto> {
    return this.http.get<DonorDto>(`${this.apiUrl}/${id}`);
  }

  createDonor(donor: DonorCreateDto): Observable<DonorDto> {
    return this.http.post<DonorDto>(this.apiUrl, donor);
  }

  updateDonor(id: number, donor: DonorUpdateDto): Observable<void> {
    const body = {
      name: donor.name,
      email: donor.email,
      phone: donor.phone
    };
    return this.http.put<void>(`${this.apiUrl}/${id}`, body);
  }

  deleteDonor(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }

  getFilteredDonors(name?: string, email?: string, giftName?: string): Observable<DonorDto[]> {
    let params = new HttpParams();

    if (name) params = params.set('name', name);
    if (email) params = params.set('email', email);
    if (giftName) params = params.set('giftName', giftName);

    return this.http.get<DonorDto[]>(`${this.apiUrl}/filter`, { params });
  }
}
