import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../environments/env';
import { PaginatedResponse } from '../types/responses/paginated-response.response';
import { OrganiserResponse } from '../types/responses/organiser.response';
import { OrganiserRequest } from '../types/requests/organiser.request';

@Injectable({
  providedIn: 'root',
})
export class OrganiserService {
  constructor(private http: HttpClient) {}

  public getAll(
    pageNumber: number,
    pageSize: number,
    keyword: string,
    isDeleted: boolean
  ): Observable<PaginatedResponse<OrganiserResponse[]>> {
    return this.http.get<PaginatedResponse<OrganiserResponse[]>>(
      `${environment.api.baseUrl}/admin/organiser?pageNumber=${pageNumber}&pageSize=${pageSize}&keyword=${keyword}&isDeleted=${isDeleted}`
    );
  }

  public getById(
    id: string,
    is_active: boolean = false,
    is_deleted: boolean = false
  ): Observable<OrganiserResponse> {
    return this.http.get<OrganiserResponse>(
      `${environment.api.baseUrl}/admin/organiser/${id}?isActive=${is_active}&isDeleted=${is_deleted}`
    );
  }

  public create(request: OrganiserRequest): Observable<void> {
    return this.http.post<void>(
      `${environment.api.baseUrl}/admin/organiser`,
      request
    );
  }

  public updateById(id: string, request: OrganiserRequest): Observable<void> {
    return this.http.put<void>(
      `${environment.api.baseUrl}/admin/organiser/${id}`,
      request
    );
  }

  public deleteById(id: string): Observable<void> {
    return this.http.delete<void>(
      `${environment.api.baseUrl}/admin/organiser/${id}`
    );
  }
}
