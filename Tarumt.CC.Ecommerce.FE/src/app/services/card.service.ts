import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../environments/env';
import { UserCardResponse } from '../types/responses/user-card.response';
import { UserCardRequest } from '../types/requests/user-card.request';

@Injectable({
  providedIn: 'root',
})
export class CardService {
  constructor(private http: HttpClient) {}

  public get(): Observable<UserCardResponse> {
    return this.http.get<UserCardResponse>(
      `${environment.api.baseUrl}/user_card/`
    );
  }

  public create(userCardRequest: UserCardRequest) {
    return this.http.post(
      `${environment.api.baseUrl}/user_card/`,
      userCardRequest
    );
  }

  public update(userCardRequest: UserCardRequest) {
    return this.http.put(
      `${environment.api.baseUrl}/user_card/`,
      userCardRequest
    );
  }

  public delete() {
    return this.http.delete(`${environment.api.baseUrl}/user_card/`);
  }

  public verify(userCardRequest: UserCardRequest) {
    return this.http.post(
      `${environment.api.baseUrl}/user_card/verify/`,
      userCardRequest
    );
  }
}
