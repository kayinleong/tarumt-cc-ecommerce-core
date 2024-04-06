export interface PaginatedResponse<T> {
  responses: T;
  current_page: number;
  total_pages: number;
  total_count: number;
  has_previous: boolean;
  has_next: boolean;
}
