using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using Tarumt.CC.Ecommerce.SharedLibrary.Infrastructure.Requests;
using Tarumt.CC.Ecommerce.SharedLibrary.Infrastructure.Responses;

namespace Tarumt.CC.Ecommerce.Core.Infrastructure.Models
{
    public class UserCard : ModelBase
    {
        [Required]
        public required string CardHolderName { get; set; }

        [Required]
        public required string CardNumber { get; set; }

        [Required]
        public required string ExpiryDate {  get; set; }

        [Required]
        public required string CVV { get; set; }

        [Required]
        public required User User { get; set; }

        public static implicit operator UserCard(UserCardRequest userCardRequest)
        {
            return new()
            {
                CardHolderName = userCardRequest.CardHolderName,
                CardNumber = userCardRequest.CardNumber,
                ExpiryDate = userCardRequest.ExpiryDate,
                CVV = userCardRequest.CVV,
                User = null,
            };
        }

        public static explicit operator UserCardResponse(UserCard userCard)
        {
            return new()
            {
                Id = userCard.Id,
                CardHolderName = userCard.CardHolderName,
                CardNumber = userCard.CardNumber,
                ExpiryDate = userCard.ExpiryDate,
                UpdatedAt = userCard.UpdatedAt,
                CreatedAt = userCard.CreatedAt,
            };
        }
    }
}
