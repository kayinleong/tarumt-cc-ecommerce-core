using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using Tarumt.CC.Ecommerce.SharedLibrary.Infrastructure.Requests.Admin;
using Tarumt.CC.Ecommerce.SharedLibrary.Infrastructure.Responses;

namespace Tarumt.CC.Ecommerce.Core.Infrastructure.Models
{
    [Index(nameof(Name), IsUnique = true)]
    public class Product : ModelBase
    {
        [Required]
        public required string Name { get; set; }

        [Required]
        public required string ShortName { get; set; }

        [Required]
        public required int Count { get; set; } = 0;

        [Required]
        public required decimal Price { get; set; }

        [Required]
        public required decimal DiscountRate { get; set; } = 0.0M;

        [Required]
        public required string ImageUrl { get; set; }

        [Required]
        public required string Description { get; set; } = "";

        [Required]
        public required List<ProductCategory> Categories { get; set; }

        public DateTime? StartAt { get; set; }

        public DateTime? ExpiredAt { get; set; }

        public static implicit operator Product(ProductAdminRequest productAdminRequest)
        {
            return new()
            {
                Name = productAdminRequest.Name,
                ShortName = productAdminRequest.ShortName,
                Count = productAdminRequest.Count,
                Price = productAdminRequest.Price,
                DiscountRate = productAdminRequest.DiscountRate,
                Description = productAdminRequest.Description,
                ImageUrl = productAdminRequest.ImageUrl,
                Categories = new(),
                StartAt = productAdminRequest.StartAt != null
                    ? DateTime.ParseExact(
                        productAdminRequest.StartAt,
                        "M/d/yyyy",
                        CultureInfo.InvariantCulture)
                    : null,
                ExpiredAt = productAdminRequest.ExpiredAt != null
                    ? DateTime.ParseExact(
                        productAdminRequest.ExpiredAt,
                        "M/d/yyyy",
                        CultureInfo.InvariantCulture)
                    : null
            };
        }

        public static explicit operator ProductResponse(Product product)
        {
            return new()
            {
                Id = product.Id,
                Name = product.Name,
                ShortName = product.ShortName,
                Count = product.Count,
                Price = product.Price,
                DiscountRate = product.DiscountRate,
                Description = product.Description,
                ImageUrl = product.ImageUrl,
                CategoriesId = product.Categories
                    .Select(m => m.Name)
                    .ToArray(),
                StartAt = product.StartAt,
                ExpiredAt = product.ExpiredAt,
                UpdatedAt = product.UpdatedAt,
                CreatedAt = product.CreatedAt,
            };
        }
    }
}
