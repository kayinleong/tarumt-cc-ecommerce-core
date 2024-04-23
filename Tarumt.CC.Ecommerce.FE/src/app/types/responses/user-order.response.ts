import { ApiResponseBase } from './api-response.response';
import { UserCartItemResponse } from './user-cart-item.response';
import { UserCartView } from './user-cart.response';

export interface UserOrderResponse extends ApiResponseBase {
  user_id: string;
  user_cart_items: string[];
}

export interface UserOrderView {
  total_price: number;
  user_order: UserOrderResponse;
  user_cart_items: UserCartItemResponse[];
}

export interface UserOrderDetailView {
  total_price: number;
  user_order: UserOrderResponse;
  user_cart_view: UserCartView[];
}
