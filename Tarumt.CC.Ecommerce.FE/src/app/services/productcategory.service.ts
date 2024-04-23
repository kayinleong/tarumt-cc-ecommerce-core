import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../environments/env';
import { Observable } from 'rxjs';
import { ProductCategoryResponse } from '../types/responses/product-category-response';

@Injectable({
  providedIn: 'root',
})
export class ProductCategoryService {
  constructor(private http: HttpClient) {}

  public getAll(
    pageNumber: number,
    pageSize: number
  ): Observable<ProductCategoryResponse[]> {
    return this.http.get<ProductCategoryResponse[]>(
      `${environment.api.baseUrl}/product_category/?pageNumber=${pageNumber}&pageSize=${pageSize}`
    );
  }

  public getById(id: string): Observable<ProductCategoryResponse> {
    return this.http.get<ProductCategoryResponse>(
      `${environment.api.baseUrl}/product_category/${id}`
    );
  }
}
