import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../environments/env';
import { PaginatedResponse } from '../types/responses/paginated-response.response';
import { Observable } from 'rxjs';
import { ProductCategoryResponse } from '../types/responses/product-category-response';
import {
  ProductCategoryCreateRequest,
  ProductCategoryUpdateRequest,
} from '../types/requests/product-category.request';

@Injectable({
  providedIn: 'root',
})
export class ProductCategoryService {
  constructor(private http: HttpClient) {}

  public getAll(
    pageNumber: number,
    pageSize: number,
    keyword: string,
    isDeleted: boolean
  ): Observable<PaginatedResponse<ProductCategoryResponse[]>> {
    return this.http.get<PaginatedResponse<ProductCategoryResponse[]>>(
      `${environment.api.baseUrl}/admin/product_category/?pageNumber=${pageNumber}&pageSize=${pageSize}&keyword=${keyword}&isDeleted=${isDeleted}`
    );
  }

  public getById(
    id: string,
    is_deleted: boolean = false
  ): Observable<ProductCategoryResponse> {
    return this.http.get<ProductCategoryResponse>(
      `${environment.api.baseUrl}/admin/product_category/${id}?isDeleted=${is_deleted}`
    );
  }

  public getByName(
    name: string,
    is_deleted: boolean = false
  ): Observable<ProductCategoryResponse> {
    return this.http.get<ProductCategoryResponse>(
      `${environment.api.baseUrl}/admin/product_category/name/${name}?isDeleted=${is_deleted}`
    );
  }

  public create(product: ProductCategoryCreateRequest): Observable<void> {
    return this.http.post<void>(
      `${environment.api.baseUrl}/admin/product_category/`,
      product
    );
  }

  public updateById(
    id: string,
    product: ProductCategoryUpdateRequest
  ): Observable<void> {
    return this.http.put<void>(
      `${environment.api.baseUrl}/admin/product_category/${id}/`,
      product
    );
  }

  public deleteById(id: string): Observable<void> {
    return this.http.delete<void>(
      `${environment.api.baseUrl}/admin/product_category/${id}/`
    );
  }
}
