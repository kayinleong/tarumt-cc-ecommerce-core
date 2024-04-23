import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../environments/env';
import { UserCartResponse } from '../types/responses/user-cart.response';
import { UserCartItemRequest } from '../types/requests/user-cart-item.request';
import { UserCartItemResponse } from '../types/responses/user-cart-item.response';

@Injectable({
  providedIn: 'root',
})
export class CartService {
  constructor(private http: HttpClient) {}

  public get(): Observable<UserCartResponse> {
    return this.http.get<UserCartResponse>(
      `${environment.api.baseUrl}/user_cart/`
    );
  }

  public getCartItemById(id: string): Observable<UserCartItemResponse> {
    return this.http.get<UserCartItemResponse>(
      `${environment.api.baseUrl}/user_cart/item/${id}`
    );
  }

  public updateCartItem(UserCartItemRequest: UserCartItemRequest) {
    return this.http.put(`${environment.api.baseUrl}/user_cart/item/`, {
      user_cart_item: [
        {
          product_Id: UserCartItemRequest.productId,
          count: UserCartItemRequest.count,
          price: UserCartItemRequest.price,
          discount_rate: UserCartItemRequest.discount_rate,
        },
      ],
    });
  }

  public checkoutCart() {
    return this.http.post(`${environment.api.baseUrl}/user_cart/checkout/`, {});
  }
}
