import { NgModule } from '@angular/core';
import { HomeComponent } from './home.component';
import { SharedModule } from '../../shared.module';
import { ProductService } from '../../services/product.service';
import { FormsModule } from '@angular/forms';
import { HomeRoutingModule } from './home.routes';
import { CommonModule } from '@angular/common';

@NgModule({
  declarations: [HomeComponent],
  providers: [ProductService],
  imports: [SharedModule, CommonModule, FormsModule, HomeRoutingModule],
})
export class HomeModule {}
