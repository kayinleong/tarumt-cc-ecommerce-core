import { NgModule } from '@angular/core';
import { AuthModule, LogLevel } from 'angular-auth-oidc-client';

import { environment } from '../environments/env';

@NgModule({
  imports: [
    AuthModule.forRoot({
      config: {
        authority: environment.api.oidcUrl,
        redirectUrl: window.location.origin,
        postLogoutRedirectUri: window.location.origin,
        clientId: 'tarumt_web_ecommerce_frontend_fe',
        scope: 'openid profile email address phone roles offline_access',
        responseType: 'code',
        triggerAuthorizationResultEvent: true,
        historyCleanupOff: true,
        silentRenew: true,
        useRefreshToken: true,
        logLevel: LogLevel.Debug,
      },
    }),
  ],
  exports: [AuthModule],
})
export class AuthConfigModule {}
