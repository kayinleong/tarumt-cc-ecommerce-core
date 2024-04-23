namespace Tarumt.CC.Ecommerce.SharedLibrary.Infrastructure.Requests
{
    public class UserCartRequest
    {
        public required UserCartItemRequest[] UserCartItem { get; set; }
    }
}
