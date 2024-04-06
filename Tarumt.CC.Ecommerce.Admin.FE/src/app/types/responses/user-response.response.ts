import { ApiResponseBase } from './api-response.response';

export interface UserResponse extends ApiResponseBase {
  username: string;
  first_name: string;
  last_name: string;
  gender: number;
  date_of_birth: string;
  email: string;
  is_email_verified: boolean;
  phone_number: string;
  is_phone_number_verified: boolean;
  address: string;
  culture: string;
  user_mfa: UserMfaResponse;
  is_admin: boolean;
  is_suspended: boolean;
}

export interface UserMfaResponse extends ApiResponseBase {
  secret_key: string;
  is_mfa_enable: boolean;
  mfa_types: number;
}
