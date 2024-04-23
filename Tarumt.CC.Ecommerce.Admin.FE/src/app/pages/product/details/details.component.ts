import { Component } from '@angular/core';
import { ProductResponse } from '../../../types/responses/product-response';
import { Title } from '@angular/platform-browser';
import { ActivatedRoute, Router } from '@angular/router';
import { DateService } from '../../../services/date.service';
import { ProductService } from '../../../services/product.service';
import { Flowbite } from '../../../decorators/flowbite-decorator.decorator';
import { PaginatedResponse } from '../../../types/responses/paginated-response.response';
import { ProductCategoryResponse } from '../../../types/responses/product-category-response';
import { ProductCategoryService } from '../../../services/product-category.service';
import { UserFileService } from '../../../services/user-file.service';
import { environment } from '../../../../environments/env';

@Flowbite()
@Component({
  selector: 'app-details',
  templateUrl: './details.component.html',
  styleUrl: './details.component.css',
})
export class DetailsComponent {
  id: string | undefined;
  errors: string[] = [];
  file: File | undefined;
  productCategories: PaginatedResponse<ProductCategoryResponse[]> | undefined;
  product: ProductResponse | undefined;
  baseUrl = environment.baseUrl;

  constructor(
    private title: Title,
    private route: ActivatedRoute,
    private router: Router,
    private dateService: DateService,
    private userFileService: UserFileService,
    private productService: ProductService,
    private productCategoryService: ProductCategoryService
  ) {}

  ngOnInit(): void {
    this.title.setTitle('Product Details - Tarumt.CC.Ecommerce Dashboard');

    this.route.paramMap.subscribe((params) => {
      this.id = params.get('id')!;

      this.route.queryParamMap.subscribe((queryParams) => {
        let is_deleted = /^true$/i.test(queryParams.get('is_deleted')!);

        this.productService
          .getById(this.id!, is_deleted)
          .subscribe((product) => {
            this.product = product;

            let tempCategory = this.product.categories_id;
            this.product.categories_id = [];
            tempCategory.forEach((category) => {
              this.productCategoryService.getByName(category).subscribe({
                next: (productCategory) => {
                  this.product!.categories_id.push(productCategory.id);
                },
              });
            });

            this.product.start_at = this.dateService.convertToHtmlDateFormat(
              new Date(product.start_at).toLocaleDateString('en-US')
            );
            this.product.expired_at = this.dateService.convertToHtmlDateFormat(
              new Date(product.expired_at).toLocaleDateString('en-US')
            );
          });

        this.productCategoryService
          .getAll(1, 100, '', false)
          .subscribe((productCategories) => {
            this.productCategories = productCategories;
          });
      });
    });
  }

  onSubmit() {
    this.errors = [];

    if (this.product!.name === undefined || this.product!.name === '')
      this.errors.push('Name is required.');

    if (
      this.product!.short_name === undefined ||
      this.product!.short_name === ''
    )
      this.errors.push('Short name is required.');

    if (this.product!.count === undefined || this.product!.count === 0)
      this.errors.push('Count is required.');

    if (this.product!.price === undefined || this.product!.price < 0.0)
      this.errors.push('Price is required.');

    if (
      this.product!.discount_rate === undefined ||
      this.product!.discount_rate < 0.0
    )
      this.errors.push('Discount rate is required.');

    if (
      this.product!.description === undefined ||
      this.product!.description === ''
    )
      this.errors.push('Description is required.');

    if (
      this.product!.categories_id === undefined ||
      this.product!.categories_id.length <= 0
    )
      this.errors.push('Categories is required.');

    if (this.product!.start_at === undefined || this.product!.start_at === '')
      this.errors.push('Start at is required.');

    if (
      this.product!.expired_at === undefined ||
      this.product!.expired_at === ''
    )
      this.errors.push('Expired at is required.');

    if (this.errors.length > 0) return;
    this.errors = [];

    this.product!.start_at = this.dateService.convertToServerDateFormat(
      this.product!.start_at
    );
    this.product!.expired_at = this.dateService.convertToServerDateFormat(
      this.product!.expired_at
    );

    if (this.file !== undefined) {
      this.userFileService.upload(this.file!).subscribe((file) => {
        this.productService
          .updateById(this.id!, {
            ...this.product!,
            categories_id: this.product!.categories_id!,
            image_url: file.path,
          })
          .subscribe({
            next: () => this.router.navigate(['/dashboard/product']),
          });
      });
    } else {
      this.productService
        .updateById(this.id!, {
          ...this.product!,
          categories_id: this.product!.categories_id!,
        })
        .subscribe({
          next: () => this.router.navigate(['/dashboard/product']),
        });
    }
  }

  onFileChange(event: Event) {
    const element = event.currentTarget as HTMLInputElement;

    if (element != null && element.files != null) {
      this.file = element.files[0];
    }
  }

  onDelete() {
    this.productService.deleteById(this.id!).subscribe({
      next: () => this.router.navigate(['/dashboard/product']),
      error: (err) => {},
    });
  }
}
