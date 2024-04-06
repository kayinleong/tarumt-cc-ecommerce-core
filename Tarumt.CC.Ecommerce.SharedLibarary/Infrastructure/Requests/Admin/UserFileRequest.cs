using Microsoft.AspNetCore.Http;

namespace Ky.Web.CMS.SharedLibarary.Infrastructure.Requests.Admin
{
    public class UserFileRequest
    {
        public required IFormFile file { get; set; }
    }
}
