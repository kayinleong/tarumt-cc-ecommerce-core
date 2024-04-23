namespace Tarumt.CC.Ecommerce.SharedLibrary.Infrastructure.Requests.Admin
{
    public class ProductAdminRequest
    {
        public required string Name { get; set; }

        public required string ShortName { get; set; }

        public required int Count { get; set; }

        public required decimal Price { get; set; }

        public required decimal DiscountRate { get; set; }

        public required string Description { get; set; }

        public required string ImageUrl { get; set; }

        public string[]? CategoriesId { get; set; }

        public string? StartAt { get; set; }

        public string? ExpiredAt { get; set; }
    }
}
