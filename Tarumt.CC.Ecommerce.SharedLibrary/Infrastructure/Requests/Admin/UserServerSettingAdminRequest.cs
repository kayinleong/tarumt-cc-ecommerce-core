namespace Tarumt.CC.Ecommerce.SharedLibrary.Infrastructure.Requests.Admin
{
    public class UserServerSettingAdminRequest
    {
        public required bool RequiredUserAddress { get; set; }

        public required bool RequiredStrongPasswordValidation { get; set; }
    }
}
