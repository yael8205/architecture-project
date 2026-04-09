export interface PackageDto {
    id: number;
    name: string;
    price: number;
    qtyClassicCards: number;
    qtySpecialCards: number;
    qtyPrimumCards: number;
}

export interface PackageCreateDto {
    name: string;
    price: number;
    qtyClassicCards: number;
    qtySpecialCards: number;
    qtyPrimumCards: number;
}

export interface PackageUpdateDto {
    name?: string;
    price?: number;
    qtyClassicCards?: number;
    qtySpecialCards?: number;
    qtyPrimumCards?: number;
}