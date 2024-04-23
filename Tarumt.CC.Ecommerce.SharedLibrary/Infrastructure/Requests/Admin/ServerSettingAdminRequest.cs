namespace Tarumt.CC.Ecommerce.SharedLibrary.Infrastructure.Requests.Admin
{
    public class ServerSettingAdminRequest
    {
        public required UserServerSettingAdminRequest UserServerSettings { get; set; }

        public required UserPortalServerSettingAdminRequest UserPortalServerSettings { get; set; }
    }
}
