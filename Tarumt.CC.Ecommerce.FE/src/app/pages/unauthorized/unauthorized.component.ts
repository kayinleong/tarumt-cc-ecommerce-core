import { Component, OnInit } from '@angular/core';
import { Title } from '@angular/platform-browser';
import { OidcSecurityService } from 'angular-auth-oidc-client';

@Component({
  selector: 'app-unauthorized',
  templateUrl: './unauthorized.component.html',
  styleUrl: './unauthorized.component.css',
})
export class UnauthorizedComponent implements OnInit {
  constructor(private title: Title, private oidc: OidcSecurityService) {}

  ngOnInit(): void {
    this.title.setTitle('Unauthorized - Ky.Web.CMS.Admin Dashboard');

    this.oidc.checkAuth().subscribe(({ isAuthenticated }) => {
      if (!isAuthenticated) this.oidc.authorize();
    });
  }
}
