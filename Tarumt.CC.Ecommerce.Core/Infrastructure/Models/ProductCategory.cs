using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using Tarumt.CC.Ecommerce.SharedLibrary.Infrastructure.Requests.Admin;
using Tarumt.CC.Ecommerce.SharedLibrary.Infrastructure.Responses;

namespace Tarumt.CC.Ecommerce.Core.Infrastructure.Models
{
    [Index(nameof(Name), IsUnique = true)]
    public class ProductCategory : ModelBase
    {
        [Required]
        public required string Name { get; set; }

        public List<Product>? Products { get; set; }

        public static implicit operator ProductCategory(ProductCategoryAdminRequest productCategoryAdminRequest)
        {
            return new()
            {
                Name = productCategoryAdminRequest.Name
            };
        }

        public static explicit operator ProductCategoryResponse(ProductCategory productCategory)
        {
            return new()
            {
                Id = productCategory.Id,
                Name = productCategory.Name,
                UpdatedAt = productCategory.UpdatedAt,
                CreatedAt = productCategory.CreatedAt,
            };
        }
    }
}
