import {
  HttpErrorResponse,
  HttpEvent,
  HttpHandler,
  HttpHeaders,
  HttpInterceptor,
  HttpRequest,
} from '@angular/common/http';
import { Injectable } from '@angular/core';
import { OidcSecurityService } from 'angular-auth-oidc-client';
import { Observable, catchError, throwError } from 'rxjs';
import { environment } from '../../environments/env';

@Injectable({
  providedIn: 'root',
})
export class AuthInterceptor implements HttpInterceptor {
  private token!: string;

  constructor(private oidcService: OidcSecurityService) {
    this.oidcService.getAccessToken().subscribe((token) => {
      this.token = token;
    });
  }

  intercept(
    req: HttpRequest<any>,
    next: HttpHandler
  ): Observable<HttpEvent<any>> {
    if (req.url.startsWith(environment.api.oidcUrl + '/connect'))
      return next.handle(req);

    const reqClone = req.clone({
      headers: new HttpHeaders({
        Authorization: `Bearer ${this.token}`,
      }),
    });

    return next.handle(reqClone).pipe(
      catchError((error: HttpErrorResponse): Observable<any> => {
        if (error.status === 401) window.location.replace('/unauthorized');

        return throwError(() => new Error(error.message));
      })
    );
  }
}
