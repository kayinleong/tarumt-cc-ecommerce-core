namespace Tarumt.CC.Ecommerce.SharedLibrary.Infrastructure.Requests
{
    public class UserCardRequest
    {
        public required string CardHolderName { get; set; }

        public required string CardNumber { get; set; }

        public required string ExpiryDate { get; set; }

        public required string CVV { get; set; }
    }
}
