using System.ComponentModel.DataAnnotations;

namespace Tarumt.CC.Ecommerce.SharedLibrary.ViewModels
{
    public class UserLoginViewModel
    {
        [Required(ErrorMessage = "Username is required")]
        public string? Username { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string? Password { get; set; }

        public string? ReturnUrl { get; set; }
    }
}
