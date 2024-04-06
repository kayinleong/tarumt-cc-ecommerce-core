import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { authGuard } from './guards/auth.guard';
import { DefaultLayoutComponent } from './layouts/default-layout/default-layout.component';
import { LogoutComponent } from './pages/logout/logout.component';
import { UnauthorizedComponent } from './pages/unauthorized/unauthorized.component';

export const routes: Routes = [{
    path: '',
    component: DefaultLayoutComponent,
    // canActivate: [authGuard],
    children:[
      {
        path: 'logout',
        component: LogoutComponent,
      },
      {
        path: 'unauthorized',
        component: UnauthorizedComponent,
      }
    ]
}];

@NgModule({
  imports: [RouterModule.forRoot(routes, { anchorScrolling: 'enabled' })],
  exports: [RouterModule],
})
export class AppRoutingModule {}
