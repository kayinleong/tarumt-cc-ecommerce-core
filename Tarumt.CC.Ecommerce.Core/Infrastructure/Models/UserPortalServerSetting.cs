using Ky.Web.CMS.SharedLibarary.Infrastructure.Requests.Admin;
using System.ComponentModel.DataAnnotations;

namespace Tarumt.CC.Ecommerce.Infrastructure.Models
{
    public class UserPortalServerSetting : ModelBase
    {
        [Required]
        public required bool EnableLoginPage { get; set; } = true;

        [Required]
        public required bool EnableRegisterPage { get; set; } = true;

        [Required]
        public required bool EnablePasswordForget { get; set; } = true;

        public static implicit operator UserPortalServerSetting(UserPortalServerSettingAdminRequest userPortalServerSettingAdminRequest)
        {
            return new()
            {
                EnableLoginPage = userPortalServerSettingAdminRequest.EnableLoginPage,
                EnableRegisterPage = userPortalServerSettingAdminRequest.EnableRegisterPage,
                EnablePasswordForget = userPortalServerSettingAdminRequest.EnablePasswordForget
            };
        }
    }
}
