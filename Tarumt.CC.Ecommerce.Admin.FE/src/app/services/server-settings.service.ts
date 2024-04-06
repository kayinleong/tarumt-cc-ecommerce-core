import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../environments/env';
import { ServerSettingsResponse } from '../types/responses/server-settings.response';
import { ServerSettingsRequest } from '../types/requests/server-settings.request';

@Injectable({
  providedIn: 'root',
})
export class ServerSettingsService {
  constructor(private http: HttpClient) {}

  public getAll(): Observable<ServerSettingsResponse> {
    return this.http.get<ServerSettingsResponse>(
      `${environment.api.baseUrl}/admin/server_settings/`
    );
  }

  public update(serverSettings: ServerSettingsRequest): Observable<void> {
    return this.http.put<void>(
      `${environment.api.baseUrl}/admin/server_settings/`,
      serverSettings
    );
  }
}
