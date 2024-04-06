using Ky.Web.CMS.SharedLibarary.Infrastructure.Responses;
using System.ComponentModel.DataAnnotations;
using Tarumt.CC.Ecommerce.Infrastructure.Models;
using Tarumt.CC.Ecommerce.SharedLibarary.Infrastructure.Requests;
using Tarumt.CC.Ecommerce.SharedLibarary.Infrastructure.Responses;

namespace Tarumt.CC.Ecommerce.Core.Infrastructure.Models
{
    public class ProductCart : ModelBase
    {
        [Required]
        public required List<Product> Products {  get; set; }

        [Required]
        public required User User { get; set; }

        public static implicit operator ProductCart(ProductCartRequest productCartRequest)
        {
            return new()
            {
                Products = new(),
                User = null
            };
        }

        public static explicit operator ProductCartResponse(ProductCart productCart)
        {
            return new()
            {
                Id = productCart.Id,
                Products = productCart.Products
                    .Select(m => ((ProductResponse)m))
                    .ToList(),
                UserId = productCart.User.Id,
                UpdatedAt = productCart.UpdatedAt,
                CreatedAt = productCart.CreatedAt,
            };
        }
    }
}
