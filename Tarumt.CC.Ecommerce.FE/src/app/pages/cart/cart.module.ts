import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { SharedModule } from '../../shared.module';
import { CartRoutingModule } from './cart.routes';
import { CartComponent } from './cart.component';
import { CheckoutComponent } from './checkout/checkout.component';
import { ConfirmationComponent } from './confirmation/confirmation.component';

@NgModule({
  declarations: [CartComponent, CheckoutComponent, ConfirmationComponent],
  imports: [CommonModule, FormsModule, SharedModule, CartRoutingModule],
})
export class CartModule {}
