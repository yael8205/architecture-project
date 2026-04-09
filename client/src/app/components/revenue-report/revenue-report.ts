import { Component, OnInit, inject, signal, computed } from '@angular/core';
import { ReportService } from '../../../services/report.service';
import { OrgService } from '../../../services/org.service'; 
import { CommonModule } from '@angular/common';
import { CardModule } from 'primeng/card'; 
import { MeterGroupModule } from 'primeng/metergroup';

@Component({
  selector: 'app-revenue-report',
  standalone: true,
  imports: [CommonModule, CardModule, MeterGroupModule],
  templateUrl: './revenue-report.html',
  styleUrl: './revenue-report.css',
})
export class RevenueReport implements OnInit {
  private reportService = inject(ReportService);
  private orgService = inject(OrgService);

  totalRevenue = signal<number>(0);
  
  currentOrg = this.orgService.currentOrg;

  meterValue = computed(() => [
    { 
      label: 'הכנסות', 
      value: (this.totalRevenue() / 1000000) * 100, 
      color: 'var(--org-primary)',
      icon: 'pi pi-money-bill' 
    }
  ]);

  ngOnInit(): void {
    this.loadRevenue();
  }

  loadRevenue() {
    this.reportService.getTotalRevenue().subscribe({
      next: (data) => this.totalRevenue.set(data),
      error: (err) => console.error('שגיאה בטעינת הכנסות', err)
    });
  }
}