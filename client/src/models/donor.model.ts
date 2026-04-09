export interface DonorDto {
  id: number;
  name: string;
  phone: string;
  email: string;
  giftNames: string[];
}

export interface DonorCreateDto {
  name: string;
  phone: string;
  email: string;
}

export interface DonorUpdateDto {
  name?: string;
  phone?: string;
  email?: string;
}