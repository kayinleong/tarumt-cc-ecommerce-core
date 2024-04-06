import { ApiResponseBase } from './api-response.response';

export interface ServerSettingsResponse extends ApiResponseBase {
  user_server_settings: UserServerSettingsResponse;
  user_portal_server_settings: UserPortalServerSettingsResponse;
}

export interface UserServerSettingsResponse extends ApiResponseBase {
  required_user_address: boolean;
  required_strong_password_validation: boolean;
}

export interface UserPortalServerSettingsResponse extends ApiResponseBase {
  enable_login_page: boolean;
  enable_register_page: boolean;
  enable_password_forget: boolean;
}
