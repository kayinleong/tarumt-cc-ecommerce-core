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
  dark = false;

  constructor(private oidc: OidcSecurityService) {}

  ngOnInit(): void {
    this.configureTheme();

    this.oidc.getUserData().subscribe((data) => {
      this.user = {
        sub: data.sub,
        profile: {
          username: data.profile.username,
          first_name: data.profile.first_name,
          last_name: data.profile.last_name,
          gender: data.profile.gender == 0 ? 0 : 1,
          culture: data.profile.culture,
          is_admin: data.profile.is_admin,
          date_of_birth: data.profile.date_of_birth,
        },
        email: data.email,
        email_verified: data.email_verified,
        phone_number: data.phone_number,
        phone_number_verified: data.phone_number_verified,
        address: data.address,
      };
    });
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
