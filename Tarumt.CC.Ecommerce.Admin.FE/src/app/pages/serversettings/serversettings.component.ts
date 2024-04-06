import { Component, OnInit } from '@angular/core';
import { Title } from '@angular/platform-browser';
import { ServerSettingsService } from '../../services/server-settings.service';
import { ServerSettingsResponse } from '../../types/responses/server-settings.response';

@Component({
  selector: 'app-serversettings',
  templateUrl: './serversettings.component.html',
  styleUrl: './serversettings.component.css',
})
export class ServersettingsComponent implements OnInit {
  serverSettings: ServerSettingsResponse | undefined;

  constructor(
    private title: Title,
    private serverSettingsService: ServerSettingsService
  ) {}

  ngOnInit(): void {
    this.title.setTitle('Server Settings - Tarumt.CC.Ecommerce Dashboard');

    this.serverSettingsService.getAll().subscribe((response) => {
      this.serverSettings = response;
    });
  }

  formSubmit(): void {
    if (this.serverSettings) {
      this.serverSettingsService
        .update({
          user_server_settings: {
            required_user_address:
              this.serverSettings.user_server_settings.required_user_address,
            required_strong_password_validation:
              this.serverSettings.user_server_settings
                .required_strong_password_validation,
          },
          user_portal_server_settings: {
            enable_login_page:
              this.serverSettings.user_portal_server_settings.enable_login_page,
            enable_register_page:
              this.serverSettings.user_portal_server_settings
                .enable_register_page,
            enable_password_forget:
              this.serverSettings.user_portal_server_settings
                .enable_password_forget,
          },
        })
        .subscribe();
    }
  }
}
