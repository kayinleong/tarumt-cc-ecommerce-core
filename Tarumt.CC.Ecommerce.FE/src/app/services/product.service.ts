import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../environments/env';
import { Observable } from 'rxjs';
import { PaginatedResponse } from '../types/responses/paginated-response.response';
import { ProductResponse } from '../types/responses/product-response';

@Injectable({
  providedIn: 'root',
})
export class ProductService {
  constructor(private http: HttpClient) {}

  public getAll(
    pageNumber: number,
    pageSize: number,
    keyword: string
  ): Observable<PaginatedResponse<ProductResponse[]>> {
    return this.http.get<PaginatedResponse<ProductResponse[]>>(
      `${environment.api.baseUrl}/product/?pageNumber=${pageNumber}&pageSize=${pageSize}&keyword=${keyword}`
    );
  }

  public getAllByCategory(
    pageNumber: number,
    pageSize: number,
    category: string[],
    keyword: string
  ): Observable<PaginatedResponse<ProductResponse[]>> {
    return this.http.get<PaginatedResponse<ProductResponse[]>>(
      `${
        environment.api.baseUrl
      }/product/category/?pageNumber=${pageNumber}&pageSize=${pageSize}&category=${category.join(
        ','
      )}&keyword=${keyword}`
    );
  }

  public getById(id: string): Observable<ProductResponse> {
    return this.http.get<ProductResponse>(
      `${environment.api.baseUrl}/product/${id}`
    );
  }
}
