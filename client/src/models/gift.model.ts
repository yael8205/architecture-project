export enum CardPriceEnum {
    Classic = 0,
    Special = 1,
    Primum = 2 
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
  participantName: string;   
  participantPhone: string;  
  participantEmail: string;  
  isWinner: boolean;         
}

export interface GiftDto {
  id: number;
  name: string;
  description?: string;
  categoryId: number;
  categoryName: string;
  prizeQuantity: number;      
  cardPrice: string;        
  pictureUrl?: string;
  donorId: number;
  donorName: string;
  gifPurchased: GiftPurchaserDto[]; 
  purchasersCount: number;    
  ticketCount?: number;
  isDrawn: boolean;
}

