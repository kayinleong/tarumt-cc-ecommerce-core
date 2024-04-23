import { Component, OnInit } from '@angular/core';
import { Title } from '@angular/platform-browser';
import { OrderService } from '../../services/order.service';
import { UserOrderView } from '../../types/responses/user-order.response';
import { PaginatedResponse } from '../../types/responses/paginated-response.response';
import { CartService } from '../../services/cart.service';

@Component({
  selector: 'app-order',
  templateUrl: './order.component.html',
  styleUrl: './order.component.css',
})
export class OrderComponent implements OnInit {
  pageNumber: number = 1;
  pageSize: number = 5;
  userOrderView: PaginatedResponse<UserOrderView[]> | undefined;

  constructor(
    private title: Title,
    private orderService: OrderService,
    private cartService: CartService
  ) {}

  ngOnInit(): void {
    this.title.setTitle('Orders - Tarumt.CC.Ecommerce');
    this.onSearch();
  }

  onSearch(): void {
    this.orderService.getAll(this.pageNumber, this.pageSize).subscribe({
      next: (response) => {
        this.userOrderView = {
          ...response,
          responses: response.responses.map((orderResponse) => {
            orderResponse.user_cart_items.forEach((cartItemId) => {
              this.cartService.getCartItemById(cartItemId).subscribe({
                next: (cartItemResponse) => {
                  let order = this.userOrderView!.responses.filter(
                    (m) => m.user_order.id === orderResponse.id
                  )[0];

                  let discountPrice =
                    (cartItemResponse.price * cartItemResponse.discount_rate) /
                    100;

                  let discountedPrice =
                    (cartItemResponse.price - discountPrice) *
                    cartItemResponse.count;

                  order.user_cart_items.push(cartItemResponse);
                  order.total_price += discountedPrice;
                },
              });
            });

            return {
              total_price: 0,
              user_order: orderResponse,
              user_cart_items: [],
            };
          }),
        };
      },
    });
  }

  onNextPage(): void {
    if (!this.userOrderView?.has_next) return;

    this.pageNumber++;
    this.onSearch();
  }

  onPreviousPage(): void {
    if (!this.userOrderView?.has_previous) return;

    this.pageNumber--;
    this.onSearch();
  }
}
