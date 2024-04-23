using Microsoft.AspNetCore.Http;

namespace Tarumt.CC.Ecommerce.SharedLibrary.Infrastructure.Requests.Admin
{
    public class UserFileRequest
    {
        public required IFormFile file { get; set; }
    }
}
