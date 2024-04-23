using System.ComponentModel.DataAnnotations;
using Tarumt.CC.Ecommerce.SharedLibrary.Infrastructure.Requests;
using Tarumt.CC.Ecommerce.SharedLibrary.Infrastructure.Responses;

namespace Tarumt.CC.Ecommerce.Core.Infrastructure.Models
{
    public class UserCartItem : ModelBase
    {
        [Required]
        public required Product Product { get; set; }

        [Required]
        public required int Count { get; set; }

        [Required]
        public required decimal Price { get; set; }

        [Required]
        public required decimal DiscountRate { get; set; }

        public static implicit operator UserCartItem(UserCartItemRequest userCartItemRequest)
        {
            return new()
            {
                Product = null,
                Count = userCartItemRequest.Count,
                Price = userCartItemRequest.Price,
                DiscountRate = userCartItemRequest.DiscountRate,
            };
        }

        public static explicit operator UserCartItemResponse(UserCartItem userCartItem)
        {
            return new()
            {
                Id = userCartItem.Id,
                ProductId = userCartItem.Product.Id,
                Count = userCartItem.Count,
                Price = userCartItem.Price,
                DiscountRate = userCartItem.DiscountRate,
                UpdatedAt = userCartItem.UpdatedAt,
                CreatedAt = userCartItem.CreatedAt,
            };
        }
    }
}
