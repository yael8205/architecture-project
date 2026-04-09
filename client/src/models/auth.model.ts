export enum UserRole {
    Manager = 0,
    Participant = 1
}

export interface UserDto {
    id: number;
    name: string;
    email: string;
    phone: string;
    address: string;
    role: UserRole;
    orders: any[]; 
}

export interface UserCreateDto {
    name: string;
    password: string;
    email: string;
    phone: string;
    address: string;
}
export interface UserUpdateDto {
    name?: string;
    password?: string;
    email?: string;
    phone?: string;
    address?: string;
}
export interface LoginRequestDto {
    email: string;
    password: string;
}

export interface LoginResponseDto {
    token: string;
    tokenType: string;
    expiresIn: number;
    user: UserDto;
}

export interface GiftPurchaserDto {
    id: number;
    participantName: string;
    participantPhone: string;
    participantEmail: string;
    isWinner: boolean;
}