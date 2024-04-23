import {
  Component,
  ElementRef,
  OnInit,
  QueryList,
  ViewChildren,
} from '@angular/core';
import { Title } from '@angular/platform-browser';
import { Router } from '@angular/router';
import { environment } from '../../../../environments/env';
import { CartService } from '../../../services/cart.service';
import { CardService } from '../../../services/card.service';
import { ProductService } from '../../../services/product.service';
import { UserCardResponse } from '../../../types/responses/user-card.response';
import {
  UserCartResponse,
  UserCartView,
} from '../../../types/responses/user-cart.response';

@Component({
  selector: 'app-confirmation',
  templateUrl: './confirmation.component.html',
  styleUrl: './confirmation.component.css',
})
export class ConfirmationComponent implements OnInit {
  fileUrl = environment.fileUrl;

  @ViewChildren('cardNumber1,cardNumber2,cardNumber3,cardNumber4')
  cardNumbers: QueryList<ElementRef> | undefined;
  cardNumbersIndex = 1;

  userCard: UserCardResponse | undefined;
  userCart: UserCartResponse | undefined;
  userCartView: UserCartView[] = [];
  cartTotalPrice: number = 0;

  constructor(
    private title: Title,
    private router: Router,
    private cardService: CardService,
    private cartService: CartService,
    private productService: ProductService
  ) {}

  ngOnInit(): void {
    this.title.setTitle('Confirmation - Tarumt.CC.Ecommerce');

    this.cardService.get().subscribe({
      next: (response) => {
        this.userCard = response;

        setTimeout(() => {
          for (let i = 0; i < 4; i++) {
            const start = i * 4;
            const end = start + 4;
            const part = this.userCard!.card_number.substring(start, end);
            this.cardNumbers!.toArray()[i].nativeElement.value = part;
          }
        }, 1000);
      },
    });

    this.cartService.get().subscribe({
      next: (response) => {
        this.userCart = response;

        response.user_cart_items.forEach((item) => {
          this.cartService.getCartItemById(item).subscribe({
            next: (cartItemResponse) => {
              this.productService
                .getById(cartItemResponse.product_id)
                .subscribe({
                  next: (productResponse) => {
                    this.userCartView.push({
                      user_cart_items: cartItemResponse,
                      product: productResponse,
                    });

                    let discountPrice =
                      (productResponse.price * productResponse.discount_rate) /
                      100;

                    let discountedPrice =
                      (productResponse.price - discountPrice) *
                      cartItemResponse.count;

                    this.cartTotalPrice += discountedPrice;
                  },
                });
            },
          });
        });
      },
    });
  }

  onClickPay() {
    this.cartService.checkoutCart().subscribe({
      next: () => {
        this.router.navigate(['/']);
      },
    });
  }
}
