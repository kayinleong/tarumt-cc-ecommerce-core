import { ApiResponseBase } from "./api-response.response";

export interface ProductResponse extends ApiResponseBase {
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
