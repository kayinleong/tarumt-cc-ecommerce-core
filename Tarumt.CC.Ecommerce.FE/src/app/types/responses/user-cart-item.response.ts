import { ApiResponseBase } from './api-response.response';

export interface UserCartItemResponse extends ApiResponseBase {
  product_id: string;
  count: number;
  price: number;
  discount_rate: number;
}
