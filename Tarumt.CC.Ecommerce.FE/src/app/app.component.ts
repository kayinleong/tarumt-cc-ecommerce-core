import { Component, OnInit } from '@angular/core';
import { OidcSecurityService } from 'angular-auth-oidc-client';
import { initFlowbite } from 'flowbite';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent implements OnInit {
  constructor(private oidc: OidcSecurityService) {}

  ngOnInit(): void {
    initFlowbite();

    this.oidc.checkAuth().subscribe((isAuthenticated) => {})
  }
}
