import { Component, OnInit, inject, ViewChild } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { ButtonModule } from 'primeng/button';
import { ConfirmDialogModule } from 'primeng/confirmdialog';
import { DialogModule } from 'primeng/dialog';
import { SelectModule } from 'primeng/select'; // בגרסה 20 זה Select
import { FileUploadModule } from 'primeng/fileupload';
import { IconFieldModule } from 'primeng/iconfield';
import { InputIconModule } from 'primeng/inputicon';
import { InputNumberModule } from 'primeng/inputnumber';
import { RadioButtonModule } from 'primeng/radiobutton';
import { RatingModule } from 'primeng/rating';
import { TableModule, Table } from 'primeng/table';
import { TagModule } from 'primeng/tag';
import { ToastModule } from 'primeng/toast';
import { ToolbarModule } from 'primeng/toolbar';
import { InputTextModule } from 'primeng/inputtext';
import { TextareaModule } from 'primeng/textarea'; // הוספתי ייבוא חסר
import { MessageService, ConfirmationService } from 'primeng/api';
import { GiftService } from '../../../services/gift.service';
import { signal } from '@angular/core';
import { OrgService } from '../../../services/org.service';
import { ActivatedRoute } from '@angular/router';
import { forkJoin } from 'rxjs';
import { ChipModule } from 'primeng/chip';
import { ImageModule } from 'primeng/image';
import { BadgeModule } from 'primeng/badge';
import { MeterGroupModule } from 'primeng/metergroup';
import { TooltipModule } from 'primeng/tooltip';
 // בהנחה שיש לך Enum תואם ל-CardPriceEnum ב-C#
export enum CardPriceEnum {
    Classic = 0,
    Special = 1,
    Primum = 2 
}

export interface GiftDto {
  id: number;
  name: string;
  description?: string;
  categoryId: number;
  categoryName: string;
  prizeQuantity: number;
  cardPrice: CardPriceEnum;
  pictureUrl?: string;
  donorId: number;
  donorName: string;
  gifPurchased: GiftPurchaserDto[];
  purchasersCount: number;
  ticketCount?: number; 
}

export interface GiftCreateDto {
  name: string;
  description?: string;
  categoryId: number;
  prizeQuantity: number;
  cardPrice: CardPriceEnum; 
  pictureUrl?: string;
  donorId: number;
}

export interface GiftUpdateDto {
  name?: string;
  description?: string;
  categoryId?: number;
  prizeQuantity?: number;
  cardPrice?: CardPriceEnum;
  pictureUrl?: string;
  donorId?: number;
}


export interface GiftPurchaserDto {
  id: number;
  participantName: string;   //
  participantPhone: string;  //
  participantEmail: string;  //
  isWinner: boolean;         //
}

@Component({
    selector: 'app-manager-gift',
    standalone: true,
    imports: [
        CommonModule, ButtonModule, ConfirmDialogModule, DialogModule, 
        SelectModule, FileUploadModule, IconFieldModule, InputIconModule, 
        InputNumberModule, RadioButtonModule, RatingModule, TableModule, BadgeModule,
        TagModule, ToastModule, ToolbarModule, InputTextModule, FormsModule, TextareaModule,SelectModule, ChipModule, ImageModule, MeterGroupModule, TooltipModule
    ],
    providers: [MessageService, ConfirmationService],
    templateUrl: './manager-gift.html',
    styleUrl: './manager-gift.css',
})
export class ManagerGift implements OnInit {
    @ViewChild('dt') dt: Table | undefined; // לגישה לפונקציית Export
    private route = inject(ActivatedRoute);
    private messageService = inject(MessageService);
    private confirmationService = inject(ConfirmationService);
    private giftService = inject(GiftService);
public orgService = inject(OrgService);
giftDialog = signal<boolean>(false);
    gifts = signal<GiftDto[]>([]); // שימוש ב-Interface הנכון
activeGift = signal<Partial<GiftDto>>({}); // שימוש ב-Partial כדי לאפשר אובייקט ריק בהתחלה  
selectedGifts = signal<GiftDto[] | null>(null);
  loading = signal<boolean>(false);
categories = signal<any[]>([]);
donors = signal<any[]>([]);
  submitted = signal<boolean>(false);
minPurchasers: number | null = null;
  cardOptions = [
    { label: 'Classic', value: CardPriceEnum.Classic },
    { label: 'Special', value: CardPriceEnum.Special },
    { label: 'Premium', value: CardPriceEnum.Primum }
];
filter = {
    giftName: '',
    donorName: '',
    minPurchasers: null
  };
  // פונקציית החיפוש המחוברת לשרת
// הוספי/עדכני את הפונקציות הבאות בתוך ה-Class:
selectedSort: string = '';

// פונקציית המיון החדשה
onSortChange(sortBy: string) {
    this.selectedSort = sortBy;
    this.loading.set(true);

    // קריאה לשירות עם פרמטר המיון (price או popular)
    this.giftService.getSortedGiftsByOrg(sortBy).subscribe({
        next: (data) => {
            const mappedGifts = data.map(gift => ({
                ...gift,
                cardPrice: Number(gift.cardPrice) as CardPriceEnum
            }));
            this.gifts.set(mappedGifts);
            this.loading.set(false);
        },
        error: (err) => {
            console.error('מיון נכשל:', err);
            this.loading.set(false);
            this.messageService.add({ severity: 'error', summary: 'שגיאה', detail: 'נכשל במיון הנתונים' });
        }
    });
}
onSearch() {
    this.loading.set(true);
    // קריאה לשירות עם הפרמטרים מהפילטר (מותאם ל-C# SearchGiftsAsync)
    this.giftService.searchGiftsAdmin(
        this.filter.giftName || undefined, 
        this.filter.donorName || undefined, 
        this.filter.minPurchasers ?? undefined
    ).subscribe({
        next: (data) => {
            const mappedGifts = data.map(gift => ({
                ...gift,
                cardPrice: Number(gift.cardPrice) as CardPriceEnum
            }));
            this.gifts.set(mappedGifts);
            this.loading.set(false);
        },
        error: (err) => {
            console.error('חיפוש נכשל:', err);
            this.loading.set(false);
        }
    });
}

onFilterChange() {
    // עדכון הערך בתוך אובייקט הפילטר וביצוע חיפוש
    this.filter.minPurchasers = this.minPurchasers as any;
    this.onSearch();
}
    ngOnInit() {
  this.loadGifts();
    this.loadCategories();
    this.loadDonors();
    }
    loadCategories() {
    // קריאה לשירות הקטגוריות שלך
    this.giftService.getCategories().subscribe(data => this.categories.set(data));
}

loadDonors() {
    // קריאה לשירות התורמים שלך
    this.giftService.getDonors().subscribe(data => this.donors.set(data));
}
 loadGifts() {
 this.giftService.getGifts().subscribe({
    next: (data) => {
      console.log('data gifts:', data);
      const mappedGifts = data.map(gift => ({
                ...gift,
                cardPrice: Number(gift.cardPrice) as CardPriceEnum
            }));
        this.gifts.set(mappedGifts);
      },
         error: (error) => {
        console.error('שגיאה בטעינת מתנות:', error);
      }
    });
}
    openNew() {
        this.activeGift.set({});
        this.submitted.set(false);
        this.giftDialog.set(true);
    }

    hideDialog() {
        this.giftDialog.set(false);
        this.submitted.set(false);
    }

    editGift(gift: GiftDto) {
this.activeGift.set({ 
        ...gift, 
cardPrice: Number(gift.cardPrice)as unknown as CardPriceEnum    });
        this.giftDialog.set(true);
    }

    deleteGift(gift: GiftDto) {
        this.confirmationService.confirm({
            message: `Are you sure you want to delete ${gift.name}?`,
            header: 'Confirm',
            icon: 'pi pi-exclamation-triangle',
            accept: () => {
                this.giftService.deleteGift(gift.id).subscribe({
                next: () => {
                    this.messageService.add({ severity: 'success', summary: 'בוצע', detail: 'המתנה נמחקה בהצלחה' });
                    this.loadGifts(); // טעינה מחדש של הרשימה המעודכנת מהשרת
                },
                error: (err) => {
                    console.error('שגיאה במחיקה:', err);
                    this.messageService.add({ severity: 'error', summary: 'שגיאה', detail: 'לא ניתן למחוק את המתנה' });
                }
            });
        }});
    }
deleteSelectedGifts() {
    const selected = this.selectedGifts();
    if (!selected || selected.length === 0) return;

    this.confirmationService.confirm({
        message: `האם למחוק את ${selected.length} המתנות שנבחרו?`,
        header: 'אישור מחיקה מרובה',
        icon: 'pi pi-exclamation-triangle',
        accept: () => {
            // יצירת מערך של קריאות למחיקה
            const deleteRequests = selected.map(g => this.giftService.deleteGift(g.id));
            
            // שימוש ב-forkJoin (צריך לייבא מ-'rxjs') כדי להמתין שכולם יסתיימו
            import('rxjs').then(({ forkJoin }) => {
                forkJoin(deleteRequests).subscribe({
                    next: () => {
                        this.handleSuccess('כל המתנות הנבחרות נמחקו בהצלחה');
                        this.selectedGifts.set(null); // איפוס הבחירה בטבלה
                    },
                    error: (err) => {
                        console.error(err);
                        this.messageService.add({ 
                            severity: 'error', 
                            summary: 'שגיאה', 
                            detail: 'חלק מהמחיקות נכשלו' 
                        });
                    }
                });
            });
        }
    });
}

saveGift() {
    this.submitted.set(true);
    const gift = this.activeGift();

    // בדיקת תקינות לפי ה-CreateDto (שדות חובה)
    const isValid = gift.name?.trim() && 
                    gift.categoryId && 
                    gift.prizeQuantity != null && 
                    gift.cardPrice != null && 
                    gift.donorId;

    if (isValid) {
        if (gift.id) {
            // עדכון - משתמש ב-GiftUpdateDto
            this.giftService.updateGift(gift.id, gift as GiftUpdateDto).subscribe({
                next: () => this.handleSuccess('המתנה עודכנה'),
                error: (err) => console.error(err)
            });
        } else {
            // יצירה - משתמש ב-GiftCreateDto
            // TypeScript עשוי להתלונן על Partial, לכן נשתמש ב-Casting כפול במידת הצורך
            this.giftService.createGift(gift as unknown as GiftCreateDto).subscribe({
                next: () => this.handleSuccess('מתנה חדשה נוספה'),
                error: (err) => console.error(err)
            });
        }
    } else {
        this.messageService.add({ 
            severity: 'error', 
            summary: 'שגיאה', 
            detail: 'נא למלא את כל שדות החובה' 
        });
    }
}

// פונקציית עזר לקיצור הקוד
private handleSuccess(message: string) {
    this.messageService.add({ severity: 'success', summary: 'בוצע', detail: message });
    this.loadGifts();
    this.giftDialog.set(false);
    this.activeGift.set({});
    this.submitted.set(false);
}

// פונקציית עזר לסגירת הדיאלוג וניקוי
closeDialog() {
    this.giftDialog.set(false);
    this.submitted.set(false);
    this.activeGift.set({});
}

    exportCSV(event: any) {
        this.dt?.exportCSV();
    }
getGiftImageUrl(pictureUrl: string | undefined): string {
const slug = this.route.snapshot.paramMap.get('orgSlug');
    console.log('Slug check:', slug);
    if (!slug || !pictureUrl) {
         
        return ''; // או נתיב לתמונה שקיימת ב-public, למשל: '/logo.png'
      
    }
console.log(`/${slug}/images/gifts/${pictureUrl}`);

    // בניית הנתיב: ודאי שהתיקיות ב-public בנויות בדיוק כך: public/ezer-mizion/images/gifts/
    return `/${slug}/images/gifts/${pictureUrl}`;
}

  // פונקציה לטיפול בבחירת תמונה חדשה מהמחשב
  onImageSelect(event: any) {
    const file = event.files[0];
    if (file) {
      this.activeGift.update(current => ({
        ...current,
        pictureUrl: file.name 
      }));
    }}

getCardPriceLabel(value: any): string {
    if (!value && value !== 0) return 'Classic';
    
    if (typeof value === 'string' && isNaN(Number(value))) {
        return value; 
    }

    const options: any = {
        0: 'Classic',
        1: 'Special',
        2: 'Premium'
    };
    return options[Number(value)] || 'Classic';
}
}