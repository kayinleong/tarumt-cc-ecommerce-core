namespace Tarumt.CC.Ecommerce.SharedLibrary.Infrastructure.Requests.Admin
{
    public class UserUpdateAdminRequest
    {
        public required string Username { get; set; }

        public required string FirstName { get; set; }

        public required string LastName { get; set; }

        public required int Gender { get; set; }

        public required string DateOfBirth { get; set; }

        public required string Email { get; set; }

        public required bool IsEmailVerified { get; set; }

        public required string PhoneNumber { get; set; }

        public required bool IsPhoneNumberVerified { get; set; }

        public required string Address { get; set; }

        public required string Culture { get; set; } = "en-GB";

        public required bool IsAdmin { get; set; }

        public required bool IsSuspended { get; set; }
    }
}
