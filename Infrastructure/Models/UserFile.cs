using Ky.Web.CMS.SharedLibarary.Infrastructure.Responses;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Tarumt.CC.Ecommerce.Infrastructure.Models
{
    [Index(nameof(FileName), IsUnique = true)]
    public class UserFile : ModelBase
    {
        [Required]
        public required string FileName { get; set; }

        [Required]
        public required string Path { get; set; }

        public static implicit operator UserFileResponse(UserFile userFile)
        {
            return new()
            {
                Id = userFile.Id,
                Path = userFile.Path,
                UpdatedAt = userFile.UpdatedAt,
                CreatedAt = userFile.CreatedAt,
            };
        }
    }
}
