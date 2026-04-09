export interface GiftInCartDto {
    id: number;
    giftId: number;
    giftName: string;
    giftPictureUrl: string; 
    giftCardPrice: string;  
    qty: number;
}

export interface GiftInCartCreateDto {
    packageInCartId: number; 
    giftId: number;
    qty: number;
}

export interface GiftInCartUpdateDto {
    qty: number;
}