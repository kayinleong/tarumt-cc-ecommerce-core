using System.ComponentModel.DataAnnotations;
using Tarumt.CC.Ecommerce.SharedLibrary.Infrastructure.Requests.Admin;

namespace Tarumt.CC.Ecommerce.Core.Infrastructure.Models
{
    public class UserPortalServerSetting : ModelBase
    {
        [Required]
        public bool EnableLoginPage { get; set; } = true;

        [Required]
        public bool EnableRegisterPage { get; set; } = true;

        [Required]
        public bool EnablePasswordForget { get; set; } = true;

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
