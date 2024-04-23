import { Component, OnInit } from '@angular/core';
import { Title } from '@angular/platform-browser';
import { environment } from '../../../environments/env';
import { CartService } from '../../services/cart.service';
import {
  UserCartResponse,
  UserCartView,
} from '../../types/responses/user-cart.response';
import { ProductService } from '../../services/product.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-cart',
  templateUrl: './cart.component.html',
  styleUrl: './cart.component.css',
})
export class CartComponent implements OnInit {
  fileUrl = environment.fileUrl;
  cart: UserCartResponse | undefined;
  cartItems: UserCartView[] = [];
  cartTotalPrice: number = 0;

  constructor(
    private title: Title,
    private router: Router,
    private cartService: CartService,
    private productService: ProductService
  ) {}

  ngOnInit(): void {
    this.title.setTitle('Cart - Tarumt.CC.Ecommerce');

    this.cartService.get().subscribe({
      next: (response) => {
        this.cart = response;

        this.cart.user_cart_items.forEach((cartItemId) => {
          this.cartService.getCartItemById(cartItemId).subscribe({
            next: (cartResponse) => {
              this.productService.getById(cartResponse.product_id).subscribe({
                next: (productResponse) => {
                  this.cartItems.push({
                    user_cart_items: cartResponse,
                    product: productResponse,
                  });

                  let discountPrice =
                    (productResponse.price * productResponse.discount_rate) /
                    100;

                  let discountedPrice =
                    (productResponse.price - discountPrice) *
                    cartResponse.count;

                  this.cartTotalPrice += discountedPrice;
                },
                error: (error) => {},
              });
            },
            error: (error) => {},
          });
        });
      },
    });
  }

  onClickCheckout() {
    if (this.cartItems.length > 0) {
      this.router.navigate(['/cart/checkout']);
    }
  }
}
