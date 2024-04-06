import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ProductComponent } from './product.component';
import { DetailsComponent } from './details/details.component';
import { ProductRoutingModule } from './product.routes';
import { ProductService } from '../../services/product.service';
import { SharedModule } from '../../shared.module';
import { FormsModule } from '@angular/forms';
import { ProductCategoryService } from '../../services/product-category.service';
import { UserFileService } from '../../services/user-file.service';

@NgModule({
  declarations: [ProductComponent, DetailsComponent],
  providers: [UserFileService, ProductService, ProductCategoryService],
  imports: [CommonModule, ProductRoutingModule, SharedModule, FormsModule],
})
export class ProductModule {}
