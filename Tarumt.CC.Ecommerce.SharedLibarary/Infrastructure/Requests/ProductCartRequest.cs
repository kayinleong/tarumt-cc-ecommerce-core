namespace Tarumt.CC.Ecommerce.SharedLibarary.Infrastructure.Requests
{
    public class ProductCartRequest
    {
        public required string[] ProductIds { get; set; }

        public required string UserId { get; set; }
    }
}
