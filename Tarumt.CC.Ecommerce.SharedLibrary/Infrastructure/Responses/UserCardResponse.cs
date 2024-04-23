namespace Tarumt.CC.Ecommerce.SharedLibrary.Infrastructure.Responses
{
    public class UserCardResponse : Response
    {
        public required string CardHolderName { get; set; }

        public required string CardNumber { get; set; }

        public required string ExpiryDate { get; set; }
    }
}
