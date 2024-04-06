import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../environments/env';
import { Observable } from 'rxjs';
import { UserFileResponse } from '../types/responses/user-file-response.response';

@Injectable({
  providedIn: 'root',
})
export class UserFileService {
  constructor(private http: HttpClient) {}

  public upload(file: File): Observable<UserFileResponse> {
    const formData = new FormData();
    formData.append('file', file);

    return this.http.post<UserFileResponse>(
      `${environment.api.baseUrl}/admin/user_file/`,
      formData
    );
  }
}
