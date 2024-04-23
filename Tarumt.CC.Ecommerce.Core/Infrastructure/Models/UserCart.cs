using System.ComponentModel.DataAnnotations;
using Tarumt.CC.Ecommerce.SharedLibrary.Infrastructure.Requests;
using Tarumt.CC.Ecommerce.SharedLibrary.Infrastructure.Responses;

namespace Tarumt.CC.Ecommerce.Core.Infrastructure.Models
{
    public class UserCart : ModelBase
    {
        [Required]
        public required List<UserCartItem> UserCartItems { get; set; }

        [Required]
        public required User User { get; set; }

        public static implicit operator UserCart(UserCartRequest userCartRequest)
        {
            return new()
            {
                UserCartItems = new(),
                User = null
            };
        }

        public static explicit operator UserCartResponse(UserCart userCart)
        {
            return new()
            {
                Id = userCart.Id,
                UserCartItems = userCart.UserCartItems
                    .Select(m => m.Id)
                    .ToArray(),
                UserId = userCart.User.Id,
                UpdatedAt = userCart.UpdatedAt,
                CreatedAt = userCart.CreatedAt,
            };
        }
    }
}
