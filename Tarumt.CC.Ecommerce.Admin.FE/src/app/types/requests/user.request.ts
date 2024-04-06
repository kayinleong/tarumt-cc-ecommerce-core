export interface UserCreateRequest {
  username: string;
  password: string;
  first_name: string;
  last_name: string;
  gender: number;
  date_of_birth: string;
  email: string;
  address: string;
  is_email_verified: boolean;
  phone_number: string;
  is_phone_number_verified: boolean;
  culture: string;
  is_admin: boolean;
  is_suspended: boolean;
}

export interface UserUpdateRequest {
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
  is_admin: boolean;
  is_suspended: boolean;
}
