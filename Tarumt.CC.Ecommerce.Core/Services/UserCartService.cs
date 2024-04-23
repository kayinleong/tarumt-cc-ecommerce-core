using Microsoft.EntityFrameworkCore;
using Tarumt.CC.Ecommerce.Core.Infrastructure.Context;
using Tarumt.CC.Ecommerce.Core.Infrastructure.Models;
using Tarumt.CC.Ecommerce.SharedLibrary.Infrastructure.Requests;
using Tarumt.CC.Ecommerce.SharedLibrary.Infrastructure.Responses;

namespace Tarumt.CC.Ecommerce.Core.Services
{
    public class UserCartService(ILogger<UserCartService> logger, CoreContext context)
    {
        public PagedList<UserCart> GetAll(int pageNumber, int pageSize, string keyword, bool isDeleted)
        {
            logger.LogInformation($"[USERCART GET ALL] Page Number: {pageNumber}; Page Size: {pageSize}; Keyword: {keyword}");

            if (string.IsNullOrEmpty(keyword))
            {
                return PagedList<UserCart>.ToPagedList(
                    context.Set<UserCart>()
                        .Where(m => m.IsDeleted == isDeleted)
                        .Include(m => m.User)
                        .Include(m => m.UserCartItems)
                        .ThenInclude(m => m.Product)
                        .OrderBy(m => m.CreatedAt),
                    pageNumber, pageSize);
            }
            else
            {
                return PagedList<UserCart>.ToPagedList(
                    context.Set<UserCart>()
                        .Where(m => m.IsDeleted == isDeleted)
                        .Where(m => m.User.Username.Contains(keyword))
                        .Include(m => m.User)
                        .Include(m => m.UserCartItems)
                        .ThenInclude(m => m.Product)
                        .OrderBy(m => m.CreatedAt),
                    pageNumber, pageSize);
            }
        }

        public PagedList<UserCartResponse> GetAllResponse(int pageNumber, int pageSize, string keyword, bool isDeleted)
        {
            logger.LogInformation($"[USERCART GET ALL] Page Number: {pageNumber}; Page Size: {pageSize}; Keyword: {keyword}");

            if (string.IsNullOrEmpty(keyword))
            {
                return PagedList<UserCartResponse>.ToPagedList(
                    context.Set<UserCart>()
                        .Where(m => m.IsDeleted == isDeleted)
                        .Include(m => m.User)
                        .Include(m => m.UserCartItems)
                        .ThenInclude(m => m.Product)
                        .OrderBy(m => m.CreatedAt)
                        .Select(m => (UserCartResponse)m),
                    pageNumber, pageSize);
            }
            else
            {
                return PagedList<UserCartResponse>.ToPagedList(
                    context.Set<UserCart>()
                        .Where(m => m.IsDeleted == isDeleted)
                        .Where(m => m.User.Username.Contains(keyword))
                        .Include(m => m.User)
                        .Include(m => m.UserCartItems)
                        .ThenInclude(m => m.Product)
                        .OrderBy(m => m.CreatedAt)
                        .Select(m => (UserCartResponse)m),
                    pageNumber, pageSize);
            }
        }

        public async Task<UserCart> GetByIdAsync(string id, bool isDeleted)
        {
            logger.LogInformation($"[USERCART BY ID] ID: {id}");

            return await context.UserCarts
                .Where(m => m.IsDeleted == isDeleted)
                .Include(m => m.User)
                .Include(m => m.UserCartItems)
                .ThenInclude(m => m.Product)
                .FirstOrDefaultAsync(m => m.Id == id) ?? throw new InvalidOperationException("UserCart not found");
        }

        public async Task<UserCart> GetByUserIdAsync(string userId, bool isDeleted)
        {
            logger.LogInformation($"[USERCART BY USER ID] USER ID: {userId}");

            return await context.UserCarts
                .Where(m => m.IsDeleted == isDeleted)
                .Include(m => m.User)
                .Include(m => m.UserCartItems)
                .FirstOrDefaultAsync(m => m.User.Id == userId) ?? throw new InvalidOperationException("UserCart not found");
        }

        public async Task<bool> CreateAsync(UserCartRequest userCartRequest, User user)
        {
            logger.LogInformation($"[USERCART CREATE]");

            UserCart userCart = userCartRequest;
            userCart.User = user;

            if (userCartRequest.UserCartItem?.Length > 0)
            {
                foreach (UserCartItemRequest userCartItem in userCartRequest.UserCartItem)
                {
                    Product product = await context.Products
                        .Where(m => !m.IsDeleted)
                        .SingleAsync(m => m.Id == userCartItem.ProductId) ?? throw new InvalidOperationException("Product not found");

                    if (userCartItem.Count > product.Count)
                    {
                        throw new InvalidOperationException($"{product.Name} insufficient stock");
                    }

                    userCart.UserCartItems.Add(new()
                    {
                        Product = product,
                        Count = userCartItem.Count,
                        Price = userCartItem.Price,
                        DiscountRate = userCartItem.DiscountRate,
                    });
                }
            }

            await context.UserCarts.AddAsync(userCart);
            return await context.SaveChangesAsync() != 0;
        }

        public async Task<bool> UpdateAsync(UserCartRequest userCartRequest, string id, bool isDeleted)
        {
            logger.LogInformation($"[USERCART UPDATE] ID: {id}");

            UserCart currentUserCart = await GetByIdAsync(id, isDeleted);

            currentUserCart.UserCartItems = [];
            if (userCartRequest.UserCartItem?.Length > 0)
            {
                foreach (UserCartItemRequest userCartItem in userCartRequest.UserCartItem)
                {
                    Product product = await context.Products
                        .Where(m => !m.IsDeleted)
                        .SingleAsync(m => m.Id == userCartItem.ProductId) ?? throw new InvalidOperationException("Product not found");

                    if (userCartItem.Count > product.Count)
                    {
                        throw new InvalidOperationException($"{product.Name} insufficient stock");
                    }

                    currentUserCart.UserCartItems.Add(new()
                    {
                        Product = product,
                        Count = userCartItem.Count,
                        Price = userCartItem.Price,
                        DiscountRate = userCartItem.DiscountRate,
                    });
                }
            }

            context.UserCarts.Update(currentUserCart);
            return await context.SaveChangesAsync() != 0;
        }

        public async Task<bool> UpdateCartItemAsync(UserCartRequest userCartRequest, string cartId, bool isDeleted)
        {
            logger.LogInformation($"[USERCART UPDATE CART ITEM] ID: {cartId}");

            UserCart currentUserCart = await GetByIdAsync(cartId, isDeleted);
            if (userCartRequest.UserCartItem?.Length > 0)
            {
                foreach (UserCartItemRequest userCartItem in userCartRequest.UserCartItem)
                {
                    Product product = await context.Products
                        .Where(m => !m.IsDeleted)
                        .SingleAsync(m => m.Id == userCartItem.ProductId) ?? throw new InvalidOperationException("Product not found");

                    UserCartItem? containProduct = currentUserCart.UserCartItems.Find(m => m.Product == product);
                    if (containProduct != null)
                    {
                        containProduct.Count = userCartItem.Count;
                    }
                    else
                    {
                        currentUserCart.UserCartItems.Add(new()
                        {
                            Product = product,
                            Count = userCartItem.Count,
                            Price = userCartItem.Price,
                            DiscountRate = userCartItem.DiscountRate,
                        });
                    }
                }
            }

            context.UserCarts.Update(currentUserCart);
            return await context.SaveChangesAsync() != 0;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            logger.LogInformation($"[USERCART DELETE] ID: {id}");

            UserCart currentUserCart = await GetByIdAsync(id, false);
            currentUserCart.IsDeleted = true;

            context.UserCarts.Update(currentUserCart);
            return await context.SaveChangesAsync() != 0;
        }

        public async Task<bool> CheckoutAsync(string id, User user)
        {
            logger.LogInformation($"[USERCART CHECKOUT] ID: {id}");

            UserCart currentUserCart = await GetByIdAsync(id, false);
            currentUserCart.IsDeleted = true;

            UserOrder newUserOrder = new() 
            {
                UserCartItems = [],
                User = user
            };

            newUserOrder.UserCartItems.AddRange(currentUserCart.UserCartItems);
            
            List<Product> products = [];
            foreach (var userCartItems in currentUserCart.UserCartItems)
            {
                var product = await context.Products
                    .Where(m => !m.IsDeleted)
                    .FirstOrDefaultAsync(m => m.Id == userCartItems.Product.Id) ?? throw new InvalidOperationException("Product not found");

                if (userCartItems.Count > product.Count)
                {
                    throw new InvalidOperationException($"{product.Name} insufficient stock");
                }

                product.Count -= userCartItems.Count;
                products.Add(product);
            }

            currentUserCart.UserCartItems = [];

            context.UserCarts.Update(currentUserCart);
            context.Products.UpdateRange(products);
            await context.UserOrders.AddAsync(newUserOrder);
            return await context.SaveChangesAsync() != 0;
        }
    }
}
