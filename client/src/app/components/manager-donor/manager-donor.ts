import { Component, OnInit, inject, ViewChild, signal, computed } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { forkJoin, catchError, of } from 'rxjs';

import { Table, TableModule } from 'primeng/table';
import { ButtonModule } from 'primeng/button';
import { InputTextModule } from 'primeng/inputtext';
import { DialogModule } from 'primeng/dialog';
import { ToastModule } from 'primeng/toast';
import { ConfirmDialogModule } from 'primeng/confirmdialog';
import { MessageService, ConfirmationService } from 'primeng/api';

import { DonorService } from '../../../services/donor.service';
import { DonorDto, DonorCreateDto, DonorUpdateDto } from '../../../models/donor.model';

@Component({
  selector: 'app-manager-donor',
  standalone: true,
  imports: [
    CommonModule, FormsModule, TableModule, ButtonModule, 
    InputTextModule, DialogModule, ToastModule, ConfirmDialogModule
  ],
  providers: [MessageService, ConfirmationService],
  templateUrl: './manager-donor.html'
})
export class ManagerDonor implements OnInit {
  @ViewChild('dt') dt: Table | undefined;
  
  private donorService = inject(DonorService);
  private messageService = inject(MessageService);
  private confirmationService = inject(ConfirmationService);

  donors = signal<DonorDto[]>([]);
  selectedDonors = signal<DonorDto[]>([]); 
  donorDialog = signal<boolean>(false);
  activeDonor = signal<Partial<DonorDto>>({});
  
  giftDialogVisible = false;
  selectedDonorName = signal<string>('');
  currentGifts = signal<string[]>([]);
  giftFilterText = signal<string>(''); 

  searchName = signal<string>('');
  searchEmail = signal<string>('');
  searchGift = signal<string>('');

  cols = [
    { field: 'name', header: 'שם תורם' },
    { field: 'phone', header: 'טלפון' },
    { field: 'email', header: 'אימייל' }
  ];

  filteredGifts = computed(() => {
    const filter = this.giftFilterText().toLowerCase();
    return this.currentGifts().filter(g => g.toLowerCase().includes(filter));
  });

  ngOnInit() { 
    this.loadDonors(); 
  }

  loadDonors() {
    this.donorService.getDonors().subscribe(data => this.donors.set(data || []));
  }

  filterDonors() {
    this.donorService.getFilteredDonors(this.searchName(), this.searchEmail(), this.searchGift())
      .subscribe(data => this.donors.set(data || []));
  }

  showGifts(donor: DonorDto) {
    this.selectedDonorName.set(donor.name);
    this.currentGifts.set(donor.giftNames || []);
    this.giftFilterText.set(''); 
    this.giftDialogVisible = true;
  }

  exportCSV() { 
    if (this.dt) {
      this.dt.exportFilename = 'donors';
      this.dt.exportCSV(); 
    }
  }

  openNew() {
    this.activeDonor.set({ name: '', phone: '', email: '' });
    this.donorDialog.set(true);
  }

  editDonor(donor: DonorDto) {
    this.activeDonor.set({ ...donor });
    this.donorDialog.set(true);
  }

  saveDonor() {
    const donor = this.activeDonor();
    if (donor.id) {
      const updateData: DonorUpdateDto = { name: donor.name!, email: donor.email!, phone: donor.phone! };
      this.donorService.updateDonor(donor.id, updateData).subscribe(() => this.handleSuccess());
    } else {
      this.donorService.createDonor(donor as DonorCreateDto).subscribe(() => this.handleSuccess());
    }
  }

  deleteDonor(donor: DonorDto) {
    this.confirmationService.confirm({
      message: `האם למחוק את ${donor.name}?`,
      header: 'אישור מחיקה',
      accept: () => {
        this.donorService.deleteDonor(donor.id).subscribe(() => {
          this.messageService.add({ severity: 'success', summary: 'נמחק' });
          this.loadDonors();
        });
      }
    });
  }

  deleteSelectedDonors() {
    const selected = this.selectedDonors();
    if (!selected || selected.length === 0) return;

    this.confirmationService.confirm({
      message: `האם למחוק את ${selected.length} התורמים שנבחרו?`,
      header: 'אישור מחיקה מרובה',
      accept: () => {
        const deleteRequests = selected.map(donor => 
          this.donorService.deleteDonor(donor.id).pipe(catchError(err => of(null)))
        );

        forkJoin(deleteRequests).subscribe(() => {
          this.messageService.add({ severity: 'success', summary: 'בוצע', detail: 'כל התורמים נמחקו' });
          this.selectedDonors.set([]); 
          this.loadDonors(); 
        });
      }
    });
  }

  private handleSuccess() {
    this.loadDonors();
    this.donorDialog.set(false);
    this.messageService.add({ severity: 'success', summary: 'בוצע' });
  }
}