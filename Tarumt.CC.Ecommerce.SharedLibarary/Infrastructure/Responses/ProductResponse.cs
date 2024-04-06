using Tarumt.CC.Ecommerce.SharedLibarary.Infrastructure.Responses;

namespace Ky.Web.CMS.SharedLibarary.Infrastructure.Responses
{
    public class ProductResponse : Response
    {
        public required string Name { get; set; }

        public required string ShortName { get; set; }

        public required int Count { get; set; }

        public required decimal Price { get; set; }

        public required decimal DiscountRate { get; set; }

        public required string Description { get; set; }

        public required string ImageUrl { get; set; }

        public required ProductCategoryResponse[] Categories { get; set; }

        public DateTime? StartAt { get; set; }

        public DateTime? ExpiredAt { get; set; }
    }
}
