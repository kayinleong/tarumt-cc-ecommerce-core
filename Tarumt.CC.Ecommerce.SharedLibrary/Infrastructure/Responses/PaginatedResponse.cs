namespace Tarumt.CC.Ecommerce.SharedLibrary.Infrastructure.Responses
{
    public class PaginatedResponse<T>
    {
        public required T Responses { get; set; }

        public required int CurrentPage { get; set; }

        public required int TotalPages { get; set; }

        public required int TotalCount { get; set; }

        public required bool HasPrevious { get; set; }

        public required bool HasNext { get; set; }
    }
}
