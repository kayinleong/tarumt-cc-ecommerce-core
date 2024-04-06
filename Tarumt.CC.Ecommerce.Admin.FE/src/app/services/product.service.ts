import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { environment } from '../../environments/env';
import { PaginatedResponse } from "../types/responses/paginated-response.response";
import { Observable } from "rxjs";
import { ProductResponse } from "../types/responses/product-response";
import { ProductCreateRequest, ProductUpdateRequest } from "../types/requests/product.request";

@Injectable({
  providedIn: 'root',
})
export class ProductService {
  constructor(private http: HttpClient) {}

  public getAll(
    pageNumber: number,
    pageSize: number,
    keyword: string,
    isDeleted: boolean
  ): Observable<PaginatedResponse<ProductResponse[]>> {
    return this.http.get<PaginatedResponse<ProductResponse[]>>(
      `${environment.api.baseUrl}/admin/product/?pageNumber=${pageNumber}&pageSize=${pageSize}&keyword=${keyword}&isDeleted=${isDeleted}`
    );
  }

  public getById(
    id: string,
    is_deleted: boolean = false,
    is_suspended: boolean = false
  ): Observable<ProductResponse> {
    return this.http.get<ProductResponse>(
      `${environment.api.baseUrl}/admin/product/${id}?isDeleted=${is_deleted}&isSuspended=${is_suspended}`
    );
  }

  public create(product: ProductCreateRequest): Observable<void> {
    return this.http.post<void>(`${environment.api.baseUrl}/admin/product/`, product);
  }

  public updateById(id: string, product: ProductUpdateRequest): Observable<void> {
    return this.http.put<void>(
      `${environment.api.baseUrl}/admin/product/${id}/`,
      product
    );
  }

  public deleteById(id: string): Observable<void> {
    return this.http.delete<void>(
      `${environment.api.baseUrl}/admin/product/${id}/`
    );
  }
}
