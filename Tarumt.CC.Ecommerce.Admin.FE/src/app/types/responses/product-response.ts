import { ApiResponseBase } from "./api-response.response";
import { ProductCategoryResponse } from "./product-category-response";

export interface ProductResponse extends ApiResponseBase {
  name: string;
  short_name: string;
  count: number;
  price: number;
  discount_rate: number;
  description: string;
  image_url: string;
  categories: ProductCategoryResponse[];
  start_at: string;
  expired_at: string;
}
