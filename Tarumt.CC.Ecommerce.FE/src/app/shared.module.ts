import { NgModule } from '@angular/core';
import { LoaderComponent } from './components/loader/loader.component';
import { DateService } from './services/date.service';
import { ProductService } from './services/product.service';
import { ProductCategoryService } from './services/productcategory.service';
import { CardService } from './services/card.service';
import { CartService } from './services/cart.service';
import { OrderService } from './services/order.service';

@NgModule({
  declarations: [LoaderComponent],
  providers: [
    DateService,
    ProductService,
    ProductCategoryService,
    OrderService,
    CardService,
    CartService,
  ],
  exports: [LoaderComponent],
})
export class SharedModule {}
