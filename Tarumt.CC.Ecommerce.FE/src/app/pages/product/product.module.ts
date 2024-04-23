import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SharedModule } from '../../shared.module';
import { ProductRoutingModule } from './product.routes';
import { FormsModule } from '@angular/forms';
import { DetailsComponent } from './details/details.component';

@NgModule({
  declarations: [DetailsComponent],
  imports: [CommonModule, SharedModule, FormsModule, ProductRoutingModule],
})
export class ProductModule {}
