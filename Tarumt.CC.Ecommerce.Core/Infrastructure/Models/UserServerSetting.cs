using System.ComponentModel.DataAnnotations;
using Tarumt.CC.Ecommerce.SharedLibrary.Infrastructure.Requests.Admin;

namespace Tarumt.CC.Ecommerce.Core.Infrastructure.Models
{
    public class UserServerSetting : ModelBase
    {
        [Required]
        public bool RequiredUserAddress { get; set; } = false;

        [Required]
        public bool RequiredStrongPasswordValidation { get; set; } = true;

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
