import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { SharedModule } from '../../shared.module';
import { OrderRoutingModule } from './order.routes';
import { OrderComponent } from './order.component';
import { DetailsComponent } from './details/details.component';

@NgModule({
  declarations: [OrderComponent, DetailsComponent],
  imports: [CommonModule, FormsModule, SharedModule, OrderRoutingModule],
})
export class OrderModule {}
