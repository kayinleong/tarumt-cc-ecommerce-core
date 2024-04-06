using Ky.Web.CMS.SharedLibarary.Infrastructure.Requests.Admin;
using System.ComponentModel.DataAnnotations;

namespace Tarumt.CC.Ecommerce.Infrastructure.Models
{
    public class UserServerSetting : ModelBase
    {
        [Required]
        public required bool RequiredUserAddress { get; set; } = false;

        [Required]
        public required bool RequiredStrongPasswordValidation { get; set; } = true;

        public static implicit operator UserServerSetting(UserServerSettingAdminRequest userServerSettingAdminRequest)
        {
            return new()
            {
                RequiredUserAddress = userServerSettingAdminRequest.RequiredUserAddress,
                RequiredStrongPasswordValidation = userServerSettingAdminRequest.RequiredStrongPasswordValidation
            };
        }
    }
}
