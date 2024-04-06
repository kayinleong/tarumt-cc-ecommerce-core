export interface UserOidcResponse {
  sub: string;
  profile: UserOidcProfileResponse;
  email: string;
  email_verified: boolean;
  phone_number: string;
  phone_number_verified: boolean;
  address: string;
}

export interface UserOidcProfileResponse {
  username: string;
  first_name: string;
  last_name: string;
  gender: number;
  culture: string;
  is_admin: boolean;
  date_of_birth: Date;
}
