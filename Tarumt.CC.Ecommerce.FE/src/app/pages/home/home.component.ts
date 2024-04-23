import { Component, OnInit } from '@angular/core';
import { ProductResponse } from '../../types/responses/product-response';
import { ProductService } from '../../services/product.service';
import { Title } from '@angular/platform-browser';
import { environment } from '../../../environments/env';
import { PaginatedResponse } from '../../types/responses/paginated-response.response';
import { ProductCategoryResponse } from '../../types/responses/product-category-response';
import { ProductCategoryService } from '../../services/productcategory.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrl: './home.component.css',
})
export class HomeComponent implements OnInit {
  fileUrl = environment.fileUrl;

  // Pagination
  pageNumber = 1;
  pageSize = 5;
  keyword = '';

  // Data
  products: PaginatedResponse<ProductResponse[]> | undefined;
  categories: ProductCategoryResponse[] | undefined;

  // Forms
  selectedCategory: string[] = [];

  constructor(
    private title: Title,
    private productService: ProductService,
    private productCategoryService: ProductCategoryService
  ) {}

  ngOnInit(): void {
    this.title.setTitle('Home - Tarumt.CC.Ecommerce');
    this.onSearch();

    this.productCategoryService.getAll(1, 99999).subscribe({
      next: (response) => {
        this.categories = response;
      },
    });
  }

  onChangeCategory(event: any) {
    if (event.target.checked) {
      this.selectedCategory.push(event.target.name);
    } else {
      let index = this.selectedCategory.findIndex(
        (x) => x === event.target.name
      );
      this.selectedCategory.splice(index, 1);
    }
  }

  onClickSearch() {
    this.pageNumber = 1;
    this.pageSize = 5;
    this.onSearch();
  }

  onSearch(): void {
    if (this.selectedCategory.length === 0) {
      this.productService
        .getAll(this.pageNumber, this.pageSize, this.keyword)
        .subscribe({
          next: (response) => {
            this.products = response;
          },
        });
    } else {
      this.productService
        .getAllByCategory(
          this.pageNumber,
          this.pageSize,
          this.selectedCategory,
          this.keyword
        )
        .subscribe({
          next: (response) => {
            this.products = response;
          },
        });
    }
  }

  onNextPage(): void {
    if (!this.products?.has_next) return;

    this.pageNumber++;
    this.onSearch();
  }

  onPreviousPage(): void {
    if (!this.products?.has_previous) return;

    this.pageNumber--;
    this.onSearch();
  }
}
