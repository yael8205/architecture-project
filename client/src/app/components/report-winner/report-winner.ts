import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { TableModule } from 'primeng/table';
import { TagModule } from 'primeng/tag'; 
import { ReportService, WinnersReportDto } from '../../../services/report.service';
import { OrgService } from '../../../services/org.service';
import { inject } from '@angular/core';

@Component({
  selector: 'app-report-winner',
  standalone: true,
  imports: [CommonModule, TableModule, TagModule], 
  templateUrl: './report-winner.html',
  styleUrl: './report-winner.css'
})
export class ReportWinner implements OnInit {
  winners: WinnersReportDto[] = [];
  loading: boolean = true;
  

  public orgService = inject(OrgService);
  currentOrg = this.orgService.currentOrg; 

  constructor(private reportService: ReportService) {}

  ngOnInit() {
    this.loadWinners();
  }

  loadWinners() {
    this.reportService.getWinners().subscribe({
      next: (data) => {
        console.log(`[API] Total winners found: ${data.length}`);
        data.forEach(item => {
          console.log(`[API] Gift: ${item.giftName}, Winner: ${item.winnerName}`);
        });
        this.winners = data;
        this.loading = false;
      },
      error: (err) => {
        console.error('שגיאה בטעינת הזוכים', err);
        this.loading = false;
      }
    });
  }
}