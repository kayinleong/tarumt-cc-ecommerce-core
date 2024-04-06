export interface ServerSettingsRequest {
  user_server_settings: UserServerSettingRequest;
  user_portal_server_settings: UserPortalServerSettingRequest;
}

export interface UserServerSettingRequest {
  required_user_address: boolean;
  required_strong_password_validation: boolean;
}

export interface UserPortalServerSettingRequest {
  enable_login_page: boolean;
  enable_register_page: boolean;
  enable_password_forget: boolean;
}
