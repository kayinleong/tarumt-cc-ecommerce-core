import { ApiResponseBase } from './api-response.response';

export interface UserCardResponse extends ApiResponseBase {
  card_holder_name: string;
  card_number: string;
  expiry_date: string;
}
