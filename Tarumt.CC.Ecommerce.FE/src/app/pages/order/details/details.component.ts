import { Component, OnInit } from '@angular/core';
import { Title } from '@angular/platform-browser';
import { environment } from '../../../../environments/env';
import { OrderService } from '../../../services/order.service';
import { CartService } from '../../../services/cart.service';
import { ActivatedRoute } from '@angular/router';
import { UserOrderDetailView } from '../../../types/responses/user-order.response';
import { ProductService } from '../../../services/product.service';

@Component({
  selector: 'app-details',
  templateUrl: './details.component.html',
  styleUrl: './details.component.css',
})
export class DetailsComponent implements OnInit {
  fileUrl = environment.fileUrl;
  id: string = '';
  userOrderView: UserOrderDetailView | undefined;

  constructor(
    private title: Title,
    private route: ActivatedRoute,
    private orderService: OrderService,
    private cartService: CartService,
    private productService: ProductService
  ) {}

  ngOnInit(): void {
    this.title.setTitle('Loading order....  - Tarumt.CC.Ecommerce');

    this.route.paramMap.subscribe((params) => {
      this.id = params.get('id')!;

      this.orderService.getById(this.id).subscribe((userOrderResponse) => {
        this.title.setTitle(
          `Order ${userOrderResponse.id} - Tarumt.CC.Ecommerce`
        );

        this.userOrderView = {
          total_price: 0,
          user_cart_view: [],
          user_order: userOrderResponse,
        };

        this.userOrderView.user_order.user_cart_items.forEach(
          (userCartItemId) => {
            this.cartService
              .getCartItemById(userCartItemId)
              .subscribe((cartItemResponse) => {
                this.productService
                  .getById(cartItemResponse.product_id)
                  .subscribe({
                    next: (productResponse) => {
                      this.userOrderView!.user_cart_view.push({
                        user_cart_items: cartItemResponse,
                        product: productResponse,
                      });

                      let discountPrice =
                        (cartItemResponse.price *
                          cartItemResponse.discount_rate) /
                        100;

                      let discountedPrice =
                        (cartItemResponse.price - discountPrice) *
                        cartItemResponse.count;

                      this.userOrderView!.total_price += discountedPrice;
                    },
                  });
              });
          }
        );
      });
    });
  }
}
