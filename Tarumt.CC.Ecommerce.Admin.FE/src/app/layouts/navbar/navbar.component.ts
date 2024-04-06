import { Component, OnInit } from '@angular/core';
import { OidcSecurityService } from 'angular-auth-oidc-client';
import { UserOidcResponse } from '../../types/responses/user-oidc-response.response';

@Component({
  selector: 'layout-navbar',
  templateUrl: './navbar.component.html',
  styleUrl: './navbar.component.css',
})
export class NavbarComponent implements OnInit {
  user: UserOidcResponse | undefined;

  constructor(private oidc: OidcSecurityService) {}

  ngOnInit(): void {
    this.oidc.userData$.subscribe((data) => {
      this.user = data.userData;
    });
  }
}
