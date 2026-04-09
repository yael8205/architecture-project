import { Component, OnInit, inject, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { DataViewModule } from 'primeng/dataview';
import { SelectButtonModule } from 'primeng/selectbutton';
import { TagModule } from 'primeng/tag';
import { ButtonModule } from 'primeng/button';
import { Video } from '../video/video';
import { PackageService } from '../../../services/package.service';
import { CommonModule } from '@angular/common'; 
import { SharedModule } from 'primeng/api';
  import { ChangeDetectorRef } from '@angular/core';
import { InputNumberModule } from 'primeng/inputnumber';
import { PackageInCartService } from '../../../services/package-in-cart.service';
import { concat } from 'rxjs';
import { ShoppingCartService } from '../../../services/shopping-cart.service';
import { computed } from '@angular/core';
import { OrgService } from '../../../services/org.service';
@Component({
  standalone: true,
  selector: 'app-package',
  imports: [InputNumberModule, Video, DataViewModule, SelectButtonModule, TagModule, ButtonModule, FormsModule, CommonModule, SharedModule],
  templateUrl: './package.html',
  styleUrl: './package.css',
})
export class Package implements OnInit {

private cdr = inject(ChangeDetectorRef);
public packageService = inject(PackageService);
public packageInCartService = inject(PackageInCartService);
public cartService = inject(ShoppingCartService);
public orgService = inject(OrgService); 
    packages = signal<any[]>([]);
    options = [
  { label: 'List', value: 'list', icon: 'pi pi-bars' },
  { label: 'Grid', value: 'grid', icon: 'pi pi-table' }
];

layout: 'list' | 'grid' = 'grid';
    ngOnInit() {
        this.loadPackages();
    }
    loadPackages() {
 this.packageService.getPackages().subscribe({
    next: (data) => {
      console.log('data packages:', data);
      const dataWithCount = data.map((pkg: any) => ({ ...pkg, ticketCount: 1 }));
        this.packages.set(dataWithCount);
          this.cdr.detectChanges();
      },
 
    error: (error) => {
        console.error('שגיאה בטעינת מוצרים:', error);
      }
    });
}
updateLocalCount(pkgId: number, delta: number) {
    this.packages.update(currentPackages => 
      currentPackages.map(p => {
        if (p.id === pkgId) {
          const newCount = (p.ticketCount || 1) + delta;
          return { ...p, ticketCount: newCount < 1 ? 1 : newCount };
        }
        return p;
      })
    );
  }


createMultiplePackages(packageId: number, quantity: number) {
  const requests = [];
  for (let i = 0; i < quantity; i++) {
    requests.push(this.packageInCartService.createMultiplePackages({ packageId }));
    this.cartService.refreshCartData(); 
  }

  concat(...requests).subscribe({
    complete: () => {
      console.log('כל החבילות נוספו אחת אחרי השנייה בבטחה');
      this.refreshCart(); 
      this.packages.update(currentPackages => 
        currentPackages.map(p => 
          p.id === packageId ? { ...p, ticketCount: 1 } : p
        )
      );}
  });
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
}}