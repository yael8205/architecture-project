import { Component, OnInit, inject, ViewChild, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { ButtonModule } from 'primeng/button';
import { ConfirmDialogModule } from 'primeng/confirmdialog';
import { DialogModule } from 'primeng/dialog';
import { TableModule, Table } from 'primeng/table';
import { ToastModule } from 'primeng/toast';
import { ToolbarModule } from 'primeng/toolbar';
import { InputTextModule } from 'primeng/inputtext';
import { MessageService, ConfirmationService } from 'primeng/api';
import { CategoryService } from '../../../services/category.service'; 
import { CategoryDto, CategoryCreateDto, CategoryUpdateDto } from '../../../models/category.model';
import { forkJoin, Observable } from 'rxjs'; 

@Component({
  selector: 'app-manager-category',
  standalone: true,
  imports: [
    CommonModule, ButtonModule, ConfirmDialogModule, DialogModule, 
    TableModule, ToastModule, ToolbarModule, InputTextModule, FormsModule
  ],
  providers: [MessageService, ConfirmationService],
  templateUrl: './manager-category.html',
})
export class ManagerCategory implements OnInit {
  @ViewChild('dt') dt: Table | undefined;
  
  private categoryService = inject(CategoryService);
  private messageService = inject(MessageService);
  private confirmationService = inject(ConfirmationService);

  categories = signal<CategoryDto[]>([]);
  selectedCategories = signal<CategoryDto[] | null>(null);
  categoryDialog = signal<boolean>(false);
  activeCategory = signal<Partial<CategoryDto>>({});
  submitted = signal<boolean>(false);
  cols = [
    { field: 'id', header: 'מזהה' },
    { field: 'name', header: 'שם קטגוריה' }
  ];

  ngOnInit() {
    this.loadCategories();
  }

  loadCategories() {
    this.categoryService.getCategories().subscribe({
      next: (data) => this.categories.set(data),
      error: (err) => console.error('Error loading categories:', err)
    });
  }

  exportCSV() {
    this.dt?.exportCSV();
  }

  saveCategory() {
    this.submitted.set(true);
    const cat = this.activeCategory();
    
    if (cat.name?.trim()) {
      const action$: Observable<any> = cat.id 
        ? this.categoryService.updateCategory(cat.id, cat as CategoryUpdateDto)
        : this.categoryService.createCategory(cat as CategoryCreateDto);

      action$.subscribe({
        next: () => {
          this.messageService.add({ severity: 'success', summary: 'בוצע', detail: 'השינויים נשמרו' });
          this.loadCategories();
          this.categoryDialog.set(false);
        }
      });
    }
  }

  deleteSelectedCategories() {
    const selected = this.selectedCategories();
    if (!selected?.length) return;

    this.confirmationService.confirm({
      message: `למחוק ${selected.length} קטגוריות?`,
      header: 'אישור מחיקה',
      accept: () => {
        const requests = selected.map(cat => this.categoryService.deleteCategory(cat.id));
        forkJoin(requests).subscribe(() => {
          this.selectedCategories.set(null);
          this.loadCategories();
        });
      }
    });
  }

  openNew() {
    this.activeCategory.set({});
    this.submitted.set(false);
    this.categoryDialog.set(true);
  }

  editCategory(category: CategoryDto) {
    this.activeCategory.set({ ...category });
    this.categoryDialog.set(true);
  }

  deleteCategory(category: CategoryDto) {
    this.confirmationService.confirm({
      message: `למחוק את "${category.name}"?`,
      accept: () => {
        this.categoryService.deleteCategory(category.id).subscribe(() => this.loadCategories());
      }
    });
  }
}