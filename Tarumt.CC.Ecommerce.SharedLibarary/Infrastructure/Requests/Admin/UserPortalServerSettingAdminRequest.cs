namespace Ky.Web.CMS.SharedLibarary.Infrastructure.Requests.Admin
{
    public class UserPortalServerSettingAdminRequest
    {
        public required bool EnableLoginPage { get; set; }

        public required bool EnableRegisterPage { get; set; }

        public required bool EnablePasswordForget { get; set; }
    }
}
