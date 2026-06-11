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

  getPackages(): Observable<PackageDto[]> {
    return this.http.get<PackageDto[]>(this.apiUrl);
  }

  getPackageById(id: number): Observable<PackageDto> {
    return this.http.get<PackageDto>(`${this.apiUrl}/${id}`);
  }

  createPackage(packageData: PackageCreateDto): Observable<PackageDto> {
    return this.http.post<PackageDto>(this.apiUrl, packageData);
  }

  updatePackage(id: number, packageData: PackageUpdateDto): Observable<void> {
    return this.http.put<void>(`${this.apiUrl}/${id}`, packageData);
  }

  deletePackage(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }
}
