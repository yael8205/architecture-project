import{ PackageInOrderDto } from "./packageInOrder.model";

export interface OrderDto {
    id: number;                            
    participantId: number;                
    participantName: string;                  
    packagesInOrder: PackageInOrderDto[];      
    sumPrice: number;                         
    date: string;                             
}

export interface OrderCreateDto {
    participantId: number;                     
    packagesInOrder: PackageInOrderDto[];    
    sumPrice: number;                         
    date: string;                             
}