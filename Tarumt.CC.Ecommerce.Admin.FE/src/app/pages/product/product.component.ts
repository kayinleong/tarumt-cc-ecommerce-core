import { Component, OnInit } from '@angular/core';
import { PaginatedResponse } from '../../types/responses/paginated-response.response';
import { ProductService } from '../../services/product.service';
import { ProductResponse } from '../../types/responses/product-response';
import { Title } from '@angular/platform-browser';
import { DateService } from '../../services/date.service';
import { ProductCreateRequest } from '../../types/requests/product.request';
import { Flowbite } from '../../decorators/flowbite-decorator.decorator';
import { ProductCategoryResponse } from '../../types/responses/product-category-response';
import { ProductCategoryCreateRequest } from '../../types/requests/product-category.request';
import { ProductCategoryService } from '../../services/product-category.service';
import { UserFileService } from '../../services/user-file.service';

@Flowbite()
@Component({
  selector: 'app-product',
  templateUrl: './product.component.html',
  styleUrl: './product.component.css',
})
export class ProductComponent implements OnInit {
  pageNumber = 1;
  pageSize = 10;
  keyword = '';
  isDeleted = false;
  errors: string[] = [];
  product_categories_errors: string[] = [];
  file: File | undefined;
  product: ProductCreateRequest = {
    name: '',
    short_name: '',
    count: 0,
    price: 0.0,
    discount_rate: 0.0,
    description: '',
    image_url: '',
    categories_id: [],
    start_at: '',
    expired_at: '',
  };
  productCategory: ProductCategoryCreateRequest = {
    name: '',
  };
  productCategories: PaginatedResponse<ProductCategoryResponse[]> | undefined;
  products: PaginatedResponse<ProductResponse[]> | undefined;

  constructor(
    private title: Title,
    private dateService: DateService,
    private userFileService: UserFileService,
    private productService: ProductService,
    private productCategoryService: ProductCategoryService
  ) {}

  ngOnInit(): void {
    this.title.setTitle('Products - Tarumt.CC.Ecommerce Dashboard');

    this.productService
      .getAll(this.pageNumber, this.pageSize, this.keyword, this.isDeleted)
      .subscribe({
        next: (response) => {
          this.products = response;
        },
      });

    this.productCategoryService.getAll(1, 10000, '', false).subscribe({
      next: (response) => {
        this.productCategories = response;
      },
    });
  }

  onSearch(): void {
    this.productService
      .getAll(this.pageNumber, this.pageSize, this.keyword, this.isDeleted)
      .subscribe({
        next: (response) => {
          this.products = response;
        },
      });
  }

  onCreate(): void {
    this.errors = [];

    if (this.product.name === undefined || this.product.name === '')
      this.errors.push('Name is required.');

    if (this.product.short_name === undefined || this.product.short_name === '')
      this.errors.push('Short name is required.');

    if (this.product.count === undefined || this.product.count === 0)
      this.errors.push('Count is required.');

    if (this.product.price === undefined || this.product.price < 0.0)
      this.errors.push('Price is required.');

    if (
      this.product.discount_rate === undefined ||
      this.product.discount_rate < 0.0
    )
      this.errors.push('Discount rate is required.');

    if (
      this.product.description === undefined ||
      this.product.description === ''
    )
      this.errors.push('Description is required.');

    if (this.file === undefined) this.errors.push('Image is required.');

    if (
      this.product.categories_id === undefined ||
      this.product.categories_id.length <= 0
    )
      this.errors.push('Categories is required.');

    if (this.product.start_at === undefined || this.product.start_at === '')
      this.errors.push('Start at is required.');

    if (this.product.expired_at === undefined || this.product.expired_at === '')
      this.errors.push('Expired at is required.');

    if (this.errors.length > 0) return;
    this.errors = [];

    this.product.start_at = this.dateService.convertToServerDateFormat(
      this.product.start_at
    );
    this.product.expired_at = this.dateService.convertToServerDateFormat(
      this.product.expired_at
    );

    this.userFileService.upload(this.file!).subscribe({
      next: (response) => {
        this.product.image_url = response.path;
        this.productService.create(this.product).subscribe({
          next: () => {
            this.onSearch();
            this.product = {
              name: '',
              short_name: '',
              count: 0,
              price: 0.0,
              discount_rate: 0.0,
              description: '',
              image_url: '',
              categories_id: [],
              start_at: '',
              expired_at: '',
            };
          },
        });
      },
    });
  }

  onFileChange(event: Event) {
    const element = event.currentTarget as HTMLInputElement;

    if (element != null && element.files != null) {
      this.file = element.files[0];
    }
  }

  onProductCategoryCreate(): void {
    this.product_categories_errors = [];

    if (
      this.productCategory.name === undefined ||
      this.productCategory.name === ''
    )
      this.product_categories_errors.push('Name is required.');

    if (this.product_categories_errors.length > 0) return;
    this.product_categories_errors = [];

    this.productCategoryService.create(this.productCategory).subscribe({
      next: () => {
        this.productCategory = {
          name: '',
        };

        this.productCategoryService.getAll(1, 10000, '', false).subscribe({
          next: (response) => {
            this.productCategories = response;
          },
        });
      },
    });
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
