import { Component, inject, OnInit, computed } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ShoppingCartService } from '../../../services/shopping-cart.service';
import { OrgService } from '../../../services/org.service';
import { ButtonModule } from 'primeng/button';
import { GiftInCartService } from '../../../services/gift-in-cart.service';
import { MessageService } from 'primeng/api';
import { Router } from '@angular/router'; 

@Component({
  selector: 'app-shopping-cart',
  standalone: true,
  imports: [CommonModule, ButtonModule],
  templateUrl: './shopping-cart.html',
  styleUrls: ['./shopping-cart.css']
})
export class ShoppingCart implements OnInit {
  public cartService = inject(ShoppingCartService);
  public orgService = inject(OrgService);
  private giftInCartService = inject(GiftInCartService);
  private messageService = inject(MessageService);
    private router = inject(Router); 
  totalPrice = computed(() => this.cartService.cart()?.sumPrice || 0);
isLoggedIn = computed(() => !!localStorage.getItem('token'));
  ngOnInit() {
    if (this.isLoggedIn()) {
    this.loadCart();}
  }

  loadCart() {
    if (!this.isLoggedIn()) return;
    this.cartService.getShoppingCart().subscribe(data => {
      this.cartService.cart.set(data);
    });
  }

  togglePackage(packageId: number) {
    const current = this.cartService.focusedPackageId();
    this.cartService.focusedPackageId.set(current === packageId ? null : packageId);
  }

  removeItem(packageId: number) {
    this.cartService.deletePackageFromCart(packageId).subscribe(() => this.loadCart());
  }

removeGiftFromPackage(giftId: number) {
  const focusedPackageId = this.cartService.focusedPackageId();
  const currentCart = this.cartService.cart();

  const targetPackage = currentCart?.packagesInShoppingCart.find(p => p.id === focusedPackageId);
  
  const giftInPackageRecord = targetPackage?.giftsInPackage.find(g => g.giftId === giftId);

  if (giftInPackageRecord) {
    this.giftInCartService.delete(giftInPackageRecord.id).subscribe({
      next: () => {
        this.messageService.add({ severity: 'success', summary: 'הוסר', detail: 'המתנה הוסרה' });
        this.refreshCart(); 
      },
      error: (err) => {
        this.messageService.add({ severity: 'error', summary: 'שגיאה', detail: 'המחיקה נכשלה' });
      }
    });
  } else {    
    this.messageService.add({ severity: 'warn', summary: 'שימי לב', detail: 'לא נמצאה מתנה כזו בחבילה הפעילה' });
  }
}
refreshCart() {
  this.cartService.getShoppingCart().subscribe({
    next: (cart) => {
      this.cartService.cart.set(cart);
      console.log('הסל רוענן בהצלחה:', cart);
    },
    error: (err) => {
      console.error('שגיאה בריענון הסל:', err);
    }
  });
}

checkout() {
  const currentCart = this.cartService.cart();
  const orgSlug = this.orgService.currentOrg()?.slug; 

  if (!currentCart || (currentCart.packagesInShoppingCart?.length ?? 0) === 0) {
    this.messageService.add({ 
      severity: 'warn', 
      summary: 'סל ריק', 
      detail: 'הוסיפו מתנות לסל לפני המעבר לתשלום' 
    });
    return;
  }

  if (orgSlug) {
    this.router.navigate(['/join', orgSlug, 'checkout']);
  } else {
    console.error('Org slug not found');
    this.messageService.add({ severity: 'error', summary: 'שגיאה', detail: 'לא ניתן למצוא את פרטי הארגון' });
  }
}
}