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
  isAuthenticated: boolean = false;
  dark = false;

  constructor(private oidc: OidcSecurityService) {}

  ngOnInit(): void {
    this.configureTheme();

    this.oidc.isAuthenticated$.subscribe((isAuthenticated) => {
      this.isAuthenticated = isAuthenticated.isAuthenticated;

      if (this.isAuthenticated) {
        this.oidc.getUserData().subscribe((userData) => {
          this.user = {
            sub: userData.sub,
            profile: {
              username: userData.profile.username,
              first_name: userData.profile.first_name,
              last_name: userData.profile.last_name,
              gender: userData.profile.gender == 0 ? 0 : 1,
              culture: userData.profile.culture,
              is_admin: userData.profile.is_admin,
              date_of_birth: userData.profile.date_of_birth,
            },
            email: userData.email,
            email_verified: userData.email_verified,
            phone_number: userData.phone_number,
            phone_number_verified: userData.phone_number_verified,
            address: userData.address,
          };
        })
      }
    });
  }

  login() {
    this.oidc.authorize();
  }

  logout() {
    this.oidc.logoff().subscribe();
  }

  configureTheme() {
    if (localStorage.getItem('color-theme') == 'dark') {
      this.dark = true;
    } else {
      this.dark = false;
    }
  }

  toggleTheme() {
    if (this.dark) {
      document.documentElement.classList.remove('dark');
      localStorage.setItem('color-theme', 'light');
      this.dark = false;
    } else {
      document.documentElement.classList.add('dark');
      localStorage.setItem('color-theme', 'dark');
      this.dark = true;
    }
  }
}
