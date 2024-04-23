using Microsoft.EntityFrameworkCore;
using Tarumt.CC.Ecommerce.Core.Infrastructure.Context;
using Tarumt.CC.Ecommerce.Core.Infrastructure.Models;
using Tarumt.CC.Ecommerce.SharedLibrary.Infrastructure.Requests;

namespace Tarumt.CC.Ecommerce.Core.Services
{
    public class UserCardService(ILogger<UserCardService> logger, CoreContext context)
    {
        public async Task<UserCard> GetByIdAsync(string id, bool isDeleted)
        {
            logger.LogInformation($"[USERCARD BY ID] ID: {id}");

            return await context.UserCards
                .Where(m => m.IsDeleted == isDeleted)
                .Include(m => m.User)
                .FirstOrDefaultAsync(m => m.Id == id) ?? throw new InvalidOperationException("UserCard not found");
        }

        public async Task<UserCard> GetByUserIdAsync(string userId, bool isDeleted)
        {
            logger.LogInformation($"[USERCARD BY USER ID] USER ID: {userId}");

            return await context.UserCards
                .Where(m => m.IsDeleted == isDeleted)
                .Include(m => m.User)
                .FirstOrDefaultAsync(m => m.User.Id == userId) ?? throw new InvalidOperationException("UserCard not found");
        }

        public async Task<bool> AddAsync(UserCardRequest userCardRequest, User user)
        {
            logger.LogInformation($"[USERCARD CREATE]");

            var isUserCardExists = await context.UserCards
                .Where(m => !m.IsDeleted)
                .Include(m => m.User)
                .AnyAsync(m => m.User.Id == user.Id);

            if (isUserCardExists)
            {
                throw new InvalidOperationException("User already have a UserCard");
            }

            if (userCardRequest.CardNumber.Length < 16 || userCardRequest.CardNumber.Length != 16)
            {
                throw new InvalidOperationException("Card number is invalid");
            }

            if (userCardRequest.ExpiryDate.Length < 5 || 
                userCardRequest.ExpiryDate.Length != 5 || 
                userCardRequest.ExpiryDate[2] != '/')
            {
                throw new InvalidOperationException("Card expiry date is invalid");
            }

            if (userCardRequest.CVV.Length < 3 || userCardRequest.CVV.Length != 3)
            {
                throw new InvalidOperationException("Card CVV is invalid");
            }

            UserCard userCard = userCardRequest;
            userCard.User = user;

            await context.UserCards.AddAsync(userCard);
            return await context.SaveChangesAsync() != 0;
        }

        public async Task<bool> UpdateByIdAsync(UserCardRequest userCardRequest, string id)
        {
            logger.LogInformation($"[USERCARD UPDATE BY ID] ID: {id}");

            if (userCardRequest.CardNumber.Length < 16 || userCardRequest.CardNumber.Length != 16)
            {
                throw new InvalidOperationException("Card number is invalid");
            }

            if (userCardRequest.ExpiryDate.Length < 5 ||
                userCardRequest.ExpiryDate.Length != 5 ||
                userCardRequest.ExpiryDate[2] != '/')
            {
                throw new InvalidOperationException("Card expiry date is invalid");
            }

            if (userCardRequest.CVV.Length < 3 || userCardRequest.CVV.Length != 3)
            {
                throw new InvalidOperationException("Card CVV is invalid");
            }

            var currentUserCard = await GetByIdAsync(id, false);
            currentUserCard.CardHolderName = userCardRequest.CardHolderName;
            currentUserCard.CardNumber = userCardRequest.CardNumber;
            currentUserCard.ExpiryDate = userCardRequest.ExpiryDate;
            currentUserCard.CVV = userCardRequest.CVV;

            context.UserCards.Update(currentUserCard);
            return await context.SaveChangesAsync() != 0;
        }

        public async Task<bool> DeleteByIdAsync(string id)
        {
            logger.LogInformation($"[USERCARD DELETE BY ID] ID: {id}");

            var currentUserCard = await GetByIdAsync(id, false);
            currentUserCard.IsDeleted = true;

            context.UserCards.Update(currentUserCard);
            return await context.SaveChangesAsync() != 0;
        }

        public async Task<bool> VerifyCardAsync(UserCardRequest userCardRequest, string id)
        {
            logger.LogInformation($"[USERCARD VerifyCVV] ID: {id}");

            var currentUserCard = await GetByIdAsync(id, false);
            if (currentUserCard.CardHolderName != userCardRequest.CardHolderName)
            {
                return false;
            }

            if (currentUserCard.CardNumber != userCardRequest.CardNumber)
            {
                return false;
            }

            if (currentUserCard.ExpiryDate != userCardRequest.ExpiryDate)
            {
                return false;
            }

            if (currentUserCard.CVV != userCardRequest.CVV)
            {
                return false;
            }

            return true;
        }
    }
}
