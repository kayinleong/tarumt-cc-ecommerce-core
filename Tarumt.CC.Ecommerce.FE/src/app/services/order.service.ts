import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../environments/env';
import { Observable } from 'rxjs';
import { UserOrderResponse } from '../types/responses/user-order.response';
import { PaginatedResponse } from '../types/responses/paginated-response.response';

@Injectable({
  providedIn: 'root',
})
export class OrderService {
  constructor(private http: HttpClient) {}

  public getAll(
    pageNumber: number,
    pageSize: number
  ): Observable<PaginatedResponse<UserOrderResponse[]>> {
    return this.http.get<PaginatedResponse<UserOrderResponse[]>>(
      `${environment.api.baseUrl}/user_order/?pageNumber=${pageNumber}&pageSize=${pageSize}`
    );
  }

  public getById(id: string): Observable<UserOrderResponse> {
    return this.http.get<UserOrderResponse>(
      `${environment.api.baseUrl}/user_order/${id}`
    );
  }
}
