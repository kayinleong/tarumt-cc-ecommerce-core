namespace Tarumt.CC.Ecommerce.Infrastructure.Models
{
    public class ServerSetting : ModelBase
    {
        public required UserServerSetting UserServerSettings { get; set; }
        public required UserPortalServerSetting UserPortalServerSettings { get; set; }
    }
}
