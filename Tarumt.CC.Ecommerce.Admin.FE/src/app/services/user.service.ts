import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../environments/env';
import { UserResponse } from '../types/responses/user-response.response';
import {
  UserCreateRequest,
  UserUpdateRequest,
} from '../types/requests/user.request';
import { PaginatedResponse } from '../types/responses/paginated-response.response';

@Injectable({
  providedIn: 'root',
})
export class UserService {
  constructor(private http: HttpClient) {}

  public getAll(
    pageNumber: number,
    pageSize: number,
    keyword: string,
    isDeleted: boolean,
    isSuspended: boolean
  ): Observable<PaginatedResponse<UserResponse[]>> {
    return this.http.get<PaginatedResponse<UserResponse[]>>(
      `${environment.api.baseUrl}/admin/user/?pageNumber=${pageNumber}&pageSize=${pageSize}&keyword=${keyword}&isDeleted=${isDeleted}&isSuspended=${isSuspended}`
    );
  }

  public getById(
    id: string,
    is_deleted: boolean = false,
    is_suspended: boolean = false
  ): Observable<UserResponse> {
    return this.http.get<UserResponse>(
      `${environment.api.baseUrl}/admin/user/${id}?isDeleted=${is_deleted}&isSuspended=${is_suspended}`
    );
  }

  public create(user: UserCreateRequest): Observable<void> {
    return this.http.post<void>(`${environment.api.baseUrl}/admin/user/`, user);
  }

  public updateById(id: string, user: UserUpdateRequest): Observable<void> {
    return this.http.put<void>(
      `${environment.api.baseUrl}/admin/user/${id}/`,
      user
    );
  }

  public deleteById(id: string): Observable<void> {
    return this.http.delete<void>(
      `${environment.api.baseUrl}/admin/user/${id}/`
    );
  }
}
