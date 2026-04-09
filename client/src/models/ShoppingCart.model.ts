import { PackageInCartDto } from "./packageInCart.model";

export interface ShoppingCartCreate {
    participantId: number;
}


export interface ShoppingCartDto {
    id: number;
    participantId: number;
    participantName: string;
    packagesInShoppingCart: PackageInCartDto[]; 
    sumPrice: number;
}

