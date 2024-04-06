import { ApiResponseBase } from './api-response.response';

export interface OrganiserResponse extends ApiResponseBase {
  name: string;
  logo_url: string;
  description: string;
  is_active: boolean;
}
