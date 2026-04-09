import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { DataViewModule } from 'primeng/dataview';
import { ButtonModule } from 'primeng/button';
import { TagModule } from 'primeng/tag';
import { SkeletonModule } from 'primeng/skeleton';
import { OrdersService } from '../../../services/orders.service';

@Component({
  selector: 'app-orders',
  standalone: true,
  imports: [CommonModule, DataViewModule, ButtonModule, TagModule, SkeletonModule],
  templateUrl: './orders.html',
  styleUrl: './orders.css'
})
export class orders implements OnInit {
  ordersService = inject(OrdersService);

  ngOnInit() {
    this.ordersService.getMyOrders();
  }
}