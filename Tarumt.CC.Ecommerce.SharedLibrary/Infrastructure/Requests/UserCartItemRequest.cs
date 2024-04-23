namespace Tarumt.CC.Ecommerce.SharedLibrary.Infrastructure.Requests
{
    public class UserCartItemRequest
    {
        public required string ProductId { get; set; }

        public required int Count { get; set; }

        public required decimal Price { get; set; }

        public required decimal DiscountRate { get; set; }
    }
}
