
import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { OrgService } from '../../../services/org.service';
import { ShoppingCartService } from '../../../services/shopping-cart.service';
import { InputTextModule } from 'primeng/inputtext';
import { ButtonModule } from 'primeng/button';
import { MessageService } from 'primeng/api';

@Component({
  selector: 'app-checkout',
  standalone: true,
  imports: [CommonModule, FormsModule, InputTextModule, ButtonModule],
  templateUrl: './check-out.html',
  styleUrls: ['./check-out.css']
})
export class Checkout {
  public cartService = inject(ShoppingCartService);
  public orgService = inject(OrgService);
  private router = inject(Router);
  private messageService = inject(MessageService);

  paymentData = { name: '', cardNum: '', expiry: '', cvv: '' };


isFormValid(): boolean {
    const data = this.paymentData;
    return (
      data.name.trim().length > 0 && 
      data.cardNum.length === 16 && 
      data.cvv.length === 3 && 
      data.expiry.length  ===5 
    );
  }

  onlyNumbers(event: KeyboardEvent) {
    const pattern = /[0-9]/;
    if (!pattern.test(event.key)) {
      event.preventDefault();
    }
  }
  formatExpiry() {
    let value = this.paymentData.expiry.replace(/\D/g, '');
    if (value.length >= 2) {
      value = value.substring(0, 2) + '/' + value.substring(2, 4);
    }
    this.paymentData.expiry = value;
  }

  processPayment() {
    this.messageService.add({ 
      severity: 'info', 
      summary: 'מעבד תשלום', 
      detail: 'מבצע אימות מול חברת האשראי...' 
    });
    
    this.cartService.createOrders().subscribe({
      next: () => {
        setTimeout(() => {
          this.messageService.add({ 
            severity: 'success', 
            summary: 'הושלם', 
            detail: 'תודה על תרומתך! ההזמנה התקבלה' 
          });
          
          this.cartService.cart.set(null); 
          setTimeout(() => this.router.navigate(['/']), 1500);
        }, 1500);
      },
      error: () => {
        this.messageService.add({ 
          severity: 'error', 
          summary: 'שגיאה', 
          detail: 'לא ניתן היה להשלים את הפעולה' 
        });
      }
    });
  }
}