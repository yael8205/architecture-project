import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { inject } from '@angular/core';
import { CategoryDto } from '../models/category.model';
import { CategoryCreateDto } from '../models/category.model';
import { CategoryUpdateDto } from '../models/category.model';
@Injectable({
  providedIn: 'root',
})
export class CategoryService { // שיניתי את שם ה-Class ל-CategoryService
  private http = inject(HttpClient);
  private readonly apiUrl = 'https://localhost:7211/api/Category';

  // שליפת כל הקטגוריות
  getCategories(): Observable<CategoryDto[]> {
    return this.http.get<CategoryDto[]>(this.apiUrl);
  }

  private getHeaders() {
    const token = localStorage.getItem('token');
    return { 'Authorization': `Bearer ${token}` };
  }

  // שליפת קטגוריה לפי ID
  getCategoryById(id: number): Observable<CategoryDto> {
    return this.http.get<CategoryDto>(`${this.apiUrl}/${id}`);
  }

  // יצירת קטגוריה חדשה
  createCategory(category: CategoryCreateDto): Observable<CategoryDto> {
    return this.http.post<CategoryDto>(this.apiUrl, category, { headers: this.getHeaders() });
  }

  // עדכון קטגוריה קיימת
  updateCategory(id: number, category: CategoryUpdateDto): Observable<void> {
    return this.http.put<void>(`${this.apiUrl}/${id}`, category, { headers: this.getHeaders() });
  }

  // מחיקת קטגוריה
  deleteCategory(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`, { headers: this.getHeaders() });
  }
}