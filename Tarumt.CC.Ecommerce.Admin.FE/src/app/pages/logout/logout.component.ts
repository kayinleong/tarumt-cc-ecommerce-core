import { Component, OnInit } from '@angular/core';
import { Title } from '@angular/platform-browser';
import { Router } from '@angular/router';
import { OidcSecurityService } from 'angular-auth-oidc-client';

@Component({
  selector: 'app-logout',
  templateUrl: './logout.component.html',
  styleUrl: './logout.component.css',
})
export class LogoutComponent implements OnInit {
  constructor(
    private title: Title,
    private router: Router,
    private oidc: OidcSecurityService
  ) {}

  ngOnInit(): void {
    console.log(123);

    this.title.setTitle('Logout - Tarumt.CC.Ecommerce Dashboard');
    this.oidc.logoff().subscribe();
    this.router.navigate(['/']);
  }
}
