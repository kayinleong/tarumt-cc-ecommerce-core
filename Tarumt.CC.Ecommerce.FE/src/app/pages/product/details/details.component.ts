import { Component, OnInit } from '@angular/core';
import { Title } from '@angular/platform-browser';
import { environment } from '../../../../environments/env';
import { ProductService } from '../../../services/product.service';
import { ProductResponse } from '../../../types/responses/product-response';
import { ActivatedRoute, Router } from '@angular/router';
import { CartService } from '../../../services/cart.service';
import { UserCartItemRequest } from '../../../types/requests/user-cart-item.request';

@Component({
  selector: 'app-details',
  templateUrl: './details.component.html',
  styleUrl: './details.component.css',
})
export class DetailsComponent implements OnInit {
  fileUrl = environment.fileUrl;
  id: string = '';
  product: ProductResponse | undefined;
  cartItem: UserCartItemRequest = {
    productId: '',
    count: 0,
    price: 0,
    discount_rate: 0,
  };

  constructor(
    private title: Title,
    private route: ActivatedRoute,
    private router: Router,
    private productService: ProductService,
    private cartService: CartService
  ) {}

  ngOnInit(): void {
    this.title.setTitle('Loading product... - Tarumt.CC.Ecommerce');

    this.route.paramMap.subscribe((params) => {
      this.id = params.get('id')!;

      this.productService.getById(this.id).subscribe({
        next: (response) => {
          this.product = response;
          this.cartItem.productId = this.id;
          this.cartItem.price = this.product.price;
          this.cartItem.discount_rate = this.product.discount_rate;
          this.title.setTitle(`${this.product.name} - Tarumt.CC.Ecommerce`);
        },
      });
    });
  }

  onSubmit() {
    if (this.cartItem.count > 0 && this.cartItem.count <= this.product!.count) {
      this.cartService.updateCartItem(this.cartItem).subscribe({
        next: () => {
          this.router.navigate(['/']);
        },
      });
    }
  }

  onIncrementCount() {
    this.cartItem.count++;
  }

  onDecrementCount() {
    if (this.cartItem.count > 0) {
      this.cartItem.count--;
    }
  }
}
