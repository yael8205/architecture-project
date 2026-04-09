import { GiftInOrderDto } from "./giftInOrder.model";

export interface PackageInOrderDto {
    id: number;               
    packageId: number;      
    priceAtPurchase: number; 
    packageName: string;     
    giftsInPackage: GiftInOrderDto[];
}