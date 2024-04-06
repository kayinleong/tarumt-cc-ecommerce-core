namespace Ky.Web.CMS.SharedLibarary.Infrastructure.Requests
{
    public class UserCreateRequest
    {
        public required string Username { get; set; }

        public required string FirstName { get; set; }

        public required string LastName { get; set; }

        public required int Gender { get; set; }

        public required string DateOfBirth { get; set; }

        public required string Email { get; set; }

        public required string PhoneNumber { get; set; }

        public required string Culture { get; set; } = "en-GB";

        public required string Password { get; set; }
    }
}
