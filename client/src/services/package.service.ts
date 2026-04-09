import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { PackageDto, PackageCreateDto, PackageUpdateDto } from '../models/package.model';

@Injectable({
  providedIn: 'root',
})
export class PackageService {
  private http = inject(HttpClient);
  private readonly apiUrl = 'https://localhost:7211/api/Package';

  // שליפת כל החבילות
  getPackages(): Observable<PackageDto[]> {
    return this.http.get<PackageDto[]>(this.apiUrl);
  }

  // פונקציית עזר להוספת טוקן (במידה ואין לך Interceptor)
  private getHeaders() {
    const token = localStorage.getItem('token');
    return { 'Authorization': `Bearer ${token}` };
  }

  // שליפת חבילה לפי ID
  getPackageById(id: number): Observable<PackageDto> {
    return this.http.get<PackageDto>(`${this.apiUrl}/${id}`);
  }

  // יצירת חבילה חדשה
  createPackage(packageData: PackageCreateDto): Observable<PackageDto> {
    return this.http.post<PackageDto>(this.apiUrl, packageData, { headers: this.getHeaders() });
  }

  // עדכון חבילה קיימת
  updatePackage(id: number, packageData: PackageUpdateDto): Observable<void> {
    return this.http.put<void>(`${this.apiUrl}/${id}`, packageData, { headers: this.getHeaders() });
  }

  // מחיקת חבילה
  deletePackage(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`, { headers: this.getHeaders() });
  }
}