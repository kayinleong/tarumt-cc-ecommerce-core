import { ApiResponseBase } from './api-response.response';
import { ProductResponse } from './product-response';
import { UserCartItemResponse } from './user-cart-item.response';

export interface UserCartResponse extends ApiResponseBase {
  user_cart_items: string[];
  user_id: string;
}

export interface UserCartView {
  user_cart_items: UserCartItemResponse;
  product: ProductResponse;
}
