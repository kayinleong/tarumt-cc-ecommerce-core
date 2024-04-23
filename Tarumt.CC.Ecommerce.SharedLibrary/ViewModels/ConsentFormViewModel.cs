using System.ComponentModel.DataAnnotations;

namespace Tarumt.CC.Ecommerce.SharedLibrary.ViewModels
{
    public class ConsentFormViewModel
    {
        [Display(Name = "Application")]
        public required string ApplicationName { get; set; }

        [Display(Name = "Scope")]
        public required string Scope { get; set; }
    }
}
