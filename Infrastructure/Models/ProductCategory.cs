using Ky.Web.CMS.SharedLibarary.Infrastructure.Requests.Admin;
using Ky.Web.CMS.SharedLibarary.Infrastructure.Responses;
using Microsoft.EntityFrameworkCore;

namespace Tarumt.CC.Ecommerce.Infrastructure.Models
{
    [Index(nameof(Name), IsUnique = true)]
    public class ProductCategory : ModelBase
    {
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
