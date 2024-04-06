export interface ProductCreateRequest {
  name: string;
  short_name: string;
  count: number;
  price: number;
  discount_rate: number;
  description: string;
  image_url: string;
  categories_id: string[];
  start_at: string;
  expired_at: string;
}

export interface ProductUpdateRequest {
  name: string;
  short_name: string;
  count: number;
  price: number;
  discount_rate: number;
  description: string;
  image_url: string;
  categories_id: string[];
  start_at: string;
  expired_at: string;
}
