import { inject } from '@angular/core';
import {
  CanActivateFn,
  ActivatedRouteSnapshot,
  RouterStateSnapshot,
  Router,
} from '@angular/router';
import { OidcSecurityService } from 'angular-auth-oidc-client';
import { map } from 'rxjs';
import { UserOidcResponse } from '../types/responses/user-oidc-response.response';

export const authGuard: CanActivateFn = (
  route: ActivatedRouteSnapshot,
  state: RouterStateSnapshot
) => {
  let router = inject(Router);
  let oidcSecurityService = inject(OidcSecurityService);
  let userData: UserOidcResponse | null = null;

  oidcSecurityService.getUserData().subscribe((data) => {
    if (data != null) {
      userData = data;
    }
  });

  return oidcSecurityService.isAuthenticated$.pipe(
    map(({ isAuthenticated }) => {
      if (isAuthenticated && userData?.profile.is_admin) {
        return true;
      }
      return router.parseUrl('/unauthorized');
    })
  );
};
