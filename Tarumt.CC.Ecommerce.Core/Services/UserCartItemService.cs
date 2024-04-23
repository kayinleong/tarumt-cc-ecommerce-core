using Microsoft.EntityFrameworkCore;
using Tarumt.CC.Ecommerce.Core.Infrastructure.Context;
using Tarumt.CC.Ecommerce.Core.Infrastructure.Models;

namespace Tarumt.CC.Ecommerce.Core.Services
{
    public class UserCartItemService(ILogger<UserCartItemService> logger, CoreContext context)
    {
        public async Task<UserCartItem> GetById(string id, bool isDeleted)
        {
            logger.LogInformation($"[USERCART BY ID] ID: {id}");

            return await context.UserCartItems
                .Where(m => m.IsDeleted == isDeleted)
                .Include(m => m.Product)
                .FirstOrDefaultAsync(m => m.Id == id) ?? throw new InvalidOperationException("UserCart not found");
        }
    }
}
