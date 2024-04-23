using System.ComponentModel.DataAnnotations;

namespace Tarumt.CC.Ecommerce.Core.Infrastructure.Models
{
    public class ServerSetting : ModelBase
    {
        [Required]
        public required UserServerSetting UserServerSettings { get; set; }

        [Required]
        public required UserPortalServerSetting UserPortalServerSettings { get; set; }
    }
}
