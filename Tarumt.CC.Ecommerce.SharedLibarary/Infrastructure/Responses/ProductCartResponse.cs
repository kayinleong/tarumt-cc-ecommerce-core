using Ky.Web.CMS.SharedLibarary.Infrastructure.Responses;

namespace Tarumt.CC.Ecommerce.SharedLibarary.Infrastructure.Responses
{
    public class ProductCartResponse : Response
    {
        public required List<ProductResponse> Products { get; set; }

        public required string UserId { get; set; }
    }
}
