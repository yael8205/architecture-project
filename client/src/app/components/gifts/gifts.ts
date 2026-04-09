
import { Component, OnInit, inject, signal, ViewEncapsulation } from '@angular/core';
import { CommonModule } from '@angular/common'; 
import { FormsModule } from '@angular/forms';
import { MessageService } from 'primeng/api';
import { ToastModule } from 'primeng/toast'; 
import { ButtonModule } from 'primeng/button';
import { SelectModule } from 'primeng/select';

import { GiftService } from '../../../services/gift.service';
import { ShoppingCartService } from '../../../services/shopping-cart.service';
import { GiftInCartService } from '../../../services/gift-in-cart.service';
import { OrgService } from '../../../services/org.service';
import { CategoryService } from '../../../services/category.service';

import { GiftDto } from '../../../models/gift.model';

@Component({
  selector: 'app-gifts',
  standalone: true,
  imports: [CommonModule, FormsModule, ToastModule, ButtonModule, SelectModule],
  providers: [MessageService],
  templateUrl: './gifts.html',
  styleUrl: './gifts.css',
  encapsulation: ViewEncapsulation.None
})
export class Gifts implements OnInit {
  private giftService = inject(GiftService);
  private giftInCartService = inject(GiftInCartService);
  public cartService = inject(ShoppingCartService);
  public orgService = inject(OrgService);
  private categoryService = inject(CategoryService);
  private messageService = inject(MessageService);

  selectedCategory = signal<number | null>(null);
  selectedPriceType = signal<number | null>(null);
  
  gifts = signal<GiftDto[]>([]);
  categories = signal<{id: number | null, name: string}[]>([]);

  priceOptions = [
    { name: 'הכל', value: null },
    { name: 'Classic', value: 0 },
    { name: 'Special', value: 1 },
    { name: 'Premium', value: 2 }
  ];

  ngOnInit() {
    this.loadCategories();
    this.loadGifts();
    this.refreshCart();
  }

  loadCategories() {
    this.categoryService.getCategories().subscribe({
      next: (data) => {
        this.categories.set([{ id: null, name: 'כל הקטגוריות' }, ...data]);
      }
    });
  }

  loadGifts() {
    this.giftService.getFilteredGifts(this.selectedCategory(), this.selectedPriceType())
      .subscribe({
        next: (data) => {
          this.gifts.set(data.map(g => ({ ...g, ticketCount: 1 })));
        },
        error: (err) => {
          this.messageService.add({ 
            severity: 'error', 
            summary: 'שגיאה', 
            detail: 'לא ניתן היה לטעון מתנות' 
          });
        }
      });
  }

  refreshCart() {
    this.cartService.getShoppingCart().subscribe(cart => {
      this.cartService.cart.set(cart);
    });
  }

  updateLocalCount(giftId: number, delta: number) {
    this.gifts.update(list => list.map(g => 
      g.id === giftId ? { ...g, ticketCount: Math.max(1, (g.ticketCount || 1) + delta) } : g
    ));
  }

  canAddGift(gift: GiftDto): boolean {
    if (this.isGiftDrawn(gift)) return false;
    const focusedId = this.cartService.focusedPackageId();
    const currentCart = this.cartService.cart();
    const targetPackage = currentCart?.packagesInShoppingCart.find(p => p.id === focusedId);
    
    if (!targetPackage) return false;

    const currentCount = targetPackage.giftsInPackage
      ?.filter(g => g.giftCardPrice?.toLowerCase() === gift.cardPrice?.toLowerCase())
      .reduce((sum, g) => sum + g.qty, 0) || 0;

    const type = gift.cardPrice?.toLowerCase();
    let maxAllowed = 0;
    const p = targetPackage as any; 
    
    if (type === 'classic') maxAllowed = p.qtyClassicCards ?? p.QtyClassicCards ?? 0;
    else if (type === 'special') maxAllowed = p.qtySpecialCards ?? p.QtySpecialCards ?? 0;
    else if (type === 'premium' || type === 'primum') maxAllowed = p.qtyPrimumCards ?? p.QtyPrimumCards ?? p.qtyPremiumCards ?? p.QtyPremiumCards ?? 0;

    return (currentCount + (gift.ticketCount || 1)) <= maxAllowed;
  }

  handleAddToPackage(gift: GiftDto) {
    const focusedId = this.cartService.focusedPackageId();
    if (!focusedId) {
      this.messageService.add({ severity: 'warn', summary: 'שימי לב', detail: 'יש לבחור חבילה קודם' });
      return;
    }

    const payload = { 
      giftId: gift.id, 
      packageInCartId: focusedId, 
      qty: gift.ticketCount || 1 
    };

    this.giftInCartService.createOrUpdate(payload).subscribe({
      next: () => {
        this.messageService.add({ severity: 'success', summary: 'עודכן', detail: 'המתנה נוספה בהצלחה' });
        this.refreshCart();
      },
      error: (err) => {
        this.messageService.add({ severity: 'error', summary: 'חסימה', detail: err.error?.message || 'אין מקום בחבילה' });
      }
    });
  }

isGiftDrawn(gift: GiftDto): boolean {
  if (gift.isDrawn) return true;
  
  return !!gift.gifPurchased && gift.gifPurchased.some(p => p.isWinner);
}
}