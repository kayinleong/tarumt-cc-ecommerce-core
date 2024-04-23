namespace Tarumt.CC.Ecommerce.SharedLibrary.Infrastructure.Responses
{
    public class UserOrderResponse : Response
    {
        public required string[] UserCartItems { get; set; }

        public required string UserId { get; set; }
    }
}
