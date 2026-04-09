import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';


export interface WinnersReportDto {
  giftName: string;
  winnerName: string;
}
@Injectable({
  providedIn: 'root',
})
export class ReportService {
  private apiUrl = 'https://localhost:7211/api/Report'; // ודאי שהפורט נכון

  constructor(private http: HttpClient) { }

  // קבלת דוח זוכים
  getWinners(): Observable<WinnersReportDto[]> {
    return this.http.get<WinnersReportDto[]>(`${this.apiUrl}/winners`);
  }

 getTotalRevenue(): Observable<number> {
    return this.http.get<number>(`${this.apiUrl}/total-revenue`);
  }
}
