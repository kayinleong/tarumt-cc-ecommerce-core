using Microsoft.EntityFrameworkCore;
using Polly;
using Tarumt.CC.Ecommerce.Core.Infrastructure.Context;
using Tarumt.CC.Ecommerce.Core.Infrastructure.Models;
using Tarumt.CC.Ecommerce.SharedLibrary.Infrastructure.Responses;

namespace Tarumt.CC.Ecommerce.Core.Services
{
    public class UserOrderService(ILogger<UserOrderService> logger, CoreContext context)
    {
        public PagedList<UserOrder> GetAll(int pageNumber, int pageSize, bool isDeleted)
        {
            logger.LogInformation($"[USERORDER GET ALL] Page Number: {pageNumber}; Page Size: {pageSize}");

            return PagedList<UserOrder>.ToPagedList(
                   context.Set<UserOrder>()
                       .Where(m => m.IsDeleted == isDeleted)
                       .Include(m => m.User)
                       .Include(m => m.UserCartItems)
                       .OrderBy(m => m.CreatedAt),
                   pageNumber, pageSize);
        }

        public PagedList<UserOrderResponse> GetAllResponseByUserId(string userId, int pageNumber, int pageSize, bool isDeleted)
        {
            logger.LogInformation($"[USERORDER GET ALL] Page Number: {pageNumber}; Page Size: {pageSize}");

            return PagedList<UserOrderResponse>.ToPagedList(
                   context.Set<UserOrder>()
                       .Where(m => m.IsDeleted == isDeleted)
                       .Include(m => m.User)
                       .Include(m => m.UserCartItems)
                       .Where(m => m.User.Id == userId)
                       .OrderBy(m => m.CreatedAt)
                       .Select(m => (UserOrderResponse)m),
                   pageNumber, pageSize);
        }

        public async Task<UserOrder> GetById(string id, bool isDeleted)
        {
            logger.LogInformation("[USERORDER BY ID] ID: {id}");

            return await context.UserOrders
                .Where(m => m.IsDeleted == isDeleted)
                .Include(m => m.User)
                .FirstOrDefaultAsync(m => m.Id == id) ?? throw new InvalidOperationException("UserOrder not found");
        }

        public async Task<UserOrder> GetByUserId(string userId, bool isDeleted)
        {
            logger.LogInformation("[USERORDER BY ID] USER ID: {id}");

            return await context.UserOrders
                .Where(m => m.IsDeleted == isDeleted)
                .Include(m => m.User)
                .Include(m => m.UserCartItems)
                .FirstOrDefaultAsync(m => m.User.Id == userId) ?? throw new InvalidOperationException("UserOrder not found");
        }
    }
}
