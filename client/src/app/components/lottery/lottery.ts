import { Component, OnInit, inject, signal } from '@angular/core';
import { GiftService } from '../../../services/gift.service'; 
import { UserDto } from '../../../models/auth.model';
import{ OrgService } from '../../../services/org.service';
import { GiftDto } from '../../../models/gift.model';
import { ShoppingCartService } from '../../../services/shopping-cart.service';
@Component({
  selector: 'app-lottery',
  templateUrl: './lottery.html',
  styleUrls: ['./lottery.css']
})
export class LotteryComponent implements OnInit {
  private giftService = inject(GiftService);
  public orgService = inject(OrgService);
  public cartService = inject(ShoppingCartService); 
  
  gifts = signal<GiftDto[]>([]);
  selectedWinners = signal<UserDto[]>([]);
  isDrawing = signal<boolean>(false);

  ngOnInit() {
    this.loadGifts();
  }

loadGifts() {
  this.giftService.getGifts().subscribe((data: GiftDto[]) => {
    this.gifts.set(data); 
  });
}

  onRunLottery(giftId: number) {
    this.isDrawing.set(true);
    this.selectedWinners.set([]); 

    this.giftService.RunLottery(giftId).subscribe({
      next: (winners) => {
        this.selectedWinners.set(winners); 
        this.isDrawing.set(false);
        this.loadGifts(); 
        console.log(this.selectedWinners);
        
      },
      error: (err) => {
        console.error(err);
        this.isDrawing.set(false);
      }
    });
  }

 
  hasWinners(gift: GiftDto): boolean {
  return gift.gifPurchased && gift.gifPurchased.some(p => p.isWinner === true);
}
}