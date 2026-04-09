import { GiftInCartDto } from "./giftInCart.model";
export interface PackageInCartDto {
    id: number;
    packageId: number;
    packageName: string;
    packagePrice: number;
    
qtyClassicCards: number; 
    qtySpecialCards: number;
    qtyPrimumCards: number;
    giftsInPackage: GiftInCartDto[];
}

export interface PackageInCartCreateDto {
    packageId: number;
}