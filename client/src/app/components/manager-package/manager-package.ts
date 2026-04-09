import { Component, OnInit, inject, ViewChild, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Observable, forkJoin, catchError, of } from 'rxjs';

import { Table, TableModule } from 'primeng/table';
import { ButtonModule } from 'primeng/button';
import { InputTextModule } from 'primeng/inputtext';
import { DialogModule } from 'primeng/dialog';
import { ToastModule } from 'primeng/toast';
import { ToolbarModule } from 'primeng/toolbar';
import { ConfirmDialogModule } from 'primeng/confirmdialog';
import { MessageService, ConfirmationService } from 'primeng/api';

import { PackageService } from '../../../services/package.service';
import { PackageDto, PackageCreateDto, PackageUpdateDto } from '../../../models/package.model';

@Component({
  selector: 'app-manager-package',
  standalone: true,
  imports: [
    CommonModule, FormsModule, TableModule, ButtonModule, 
    InputTextModule, DialogModule, ToastModule, ToolbarModule, ConfirmDialogModule
  ],
  providers: [MessageService, ConfirmationService],
  templateUrl: './manager-package.html'
})
export class ManagerPackage implements OnInit {
  @ViewChild('dt') dt: Table | undefined;
  
  private packageService = inject(PackageService);
  private messageService = inject(MessageService);
  private confirmationService = inject(ConfirmationService);

  packages = signal<PackageDto[]>([]);
  selectedPackages = signal<PackageDto[] | null>(null); 
  packageDialog = signal<boolean>(false);
  activePackage = signal<Partial<PackageDto>>({});
  submitted = signal<boolean>(false);

  cols = [
    { field: 'name', header: 'שם חבילה' },
    { field: 'price', header: 'מחיר' },
    { field: 'qtyClassicCards', header: 'כמות קלאסי' },
    { field: 'qtySpecialCards', header: 'כמות מיוחד' },
    { field: 'qtyPrimumCards', header: 'כמות פרימיום' }
  ];

  ngOnInit() { this.loadPackages(); }

  loadPackages() {
    this.packageService.getPackages().subscribe(data => this.packages.set(data));
  }

  exportCSV() {
    this.dt?.exportCSV();
  }

  openNew() {
    this.activePackage.set({ name: '', price: 0, qtyClassicCards: 0, qtySpecialCards: 0, qtyPrimumCards: 0 });
    this.submitted.set(false);
    this.packageDialog.set(true);
  }

  editPackage(pkg: PackageDto) {
    this.activePackage.set({ ...pkg });
    this.packageDialog.set(true);
  }

  savePackage() {
    this.submitted.set(true);
    const pkg = this.activePackage();
    if (!pkg.name?.trim()) return;

    const action$: Observable<any> = pkg.id 
      ? this.packageService.updatePackage(pkg.id, pkg as PackageUpdateDto)
      : this.packageService.createPackage(pkg as PackageCreateDto);

    action$.subscribe({
      next: () => {
        this.messageService.add({ severity: 'success', summary: 'בוצע', detail: 'החבילה נשמרה' });
        this.loadPackages();
        this.packageDialog.set(false);
      }
    });
  }

  deletePackage(pkg: PackageDto) {
    this.confirmationService.confirm({
      message: `למחוק את "${pkg.name}"?`,
      accept: () => {
        this.packageService.deletePackage(pkg.id).subscribe(() => this.loadPackages());
      }
    });
  }

  deleteSelectedPackages() {
    const selected = this.selectedPackages();
    if (!selected?.length) return;
    this.confirmationService.confirm({
      message: `למחוק ${selected.length} חבילות?`,
      accept: () => {
        const requests = selected.map(p => this.packageService.deletePackage(p.id).pipe(catchError(e => of(e))));
        forkJoin(requests).subscribe(() => {
          this.selectedPackages.set(null); 
          this.loadPackages();
        });
      }
    });
  }
}