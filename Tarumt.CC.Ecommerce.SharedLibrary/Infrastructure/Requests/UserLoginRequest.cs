namespace Tarumt.CC.Ecommerce.SharedLibrary.Infrastructure.Requests
{
    public class UserLoginRequest
    {
        public required string Username { get; set; }

        public required string Password { get; set; }
    }
}
