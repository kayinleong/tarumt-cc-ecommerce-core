using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Tarumt.CC.Ecommerce.Core.Constants;

namespace Tarumt.CC.Ecommerce.Core.Infrastructure.Models
{
    public class UserMfa : ModelBase
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required]
        public string SecretKey { get; set; } = Guid.NewGuid().ToString();

        [Required]
        public bool IsMfaEnable { get; set; } = false;

        [Required]
        public MfaTypes MfaTypes { get; set; } = MfaTypes.NONE;
    }
}
