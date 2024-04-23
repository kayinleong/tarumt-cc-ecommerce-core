namespace Tarumt.CC.Ecommerce.SharedLibrary.Infrastructure.Responses
{
    public class UserCartItemResponse : Response
    {
        public required string ProductId { get; set; }

        public required int Count { get; set; }

        public required decimal Price { get; set; }

        public required decimal DiscountRate { get; set; }
    }
}
