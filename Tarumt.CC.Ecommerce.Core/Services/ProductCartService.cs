using Ky.Web.CMS.SharedLibarary.Infrastructure.Responses;
using Microsoft.EntityFrameworkCore;
using Tarumt.CC.Ecommerce.Core.Infrastructure.Models;
using Tarumt.CC.Ecommerce.Infrastructure.Context;
using Tarumt.CC.Ecommerce.Infrastructure.Models;
using Tarumt.CC.Ecommerce.SharedLibarary.Infrastructure.Requests;
using Tarumt.CC.Ecommerce.SharedLibarary.Infrastructure.Responses;

namespace Tarumt.CC.Ecommerce.Core.Services
{
    public class ProductCartService
    {
        private readonly ILogger<ProductCartService> _logger;
        private readonly CoreContext _context;

        public ProductCartService(ILogger<ProductCartService> logger, CoreContext context)
        {
            _logger = logger;
            _context = context;
        }

        public PagedList<ProductCart> GetAll(int pageNumber, int pageSize, string keyword, bool isDeleted)
        {
            _logger.LogInformation($"[PRODUCTCART GET ALL] Page Number: {pageNumber}; Page Size: {pageSize}; Keyword: {keyword}");

            if (string.IsNullOrEmpty(keyword))
            {
                return PagedList<ProductCart>.ToPagedList(
                    _context.Set<ProductCart>()
                        .Where(m => m.IsDeleted == isDeleted)
                        .Include(m => m.User)
                        .OrderBy(m => m.CreatedAt),
                    pageNumber, pageSize);
            }
            else
            {
                return PagedList<ProductCart>.ToPagedList(
                    _context.Set<ProductCart>()
                        .Where(m => m.IsDeleted == isDeleted)
                        .Where(m => m.User.Username.Contains(keyword))
                        .Include(m => m.User)
                        .OrderBy(m => m.CreatedAt),
                    pageNumber, pageSize);
            }
        }

        public PagedList<ProductCartResponse> GetAllResponse(int pageNumber, int pageSize, string keyword, bool isDeleted)
        {
            _logger.LogInformation($"[PRODUCT GET ALL] Page Number: {pageNumber}; Page Size: {pageSize}; Keyword: {keyword}");

            if (string.IsNullOrEmpty(keyword))
            {
                return PagedList<ProductCartResponse>.ToPagedList(
                    _context.Set<ProductCart>()
                        .Where(m => m.IsDeleted == isDeleted)
                        .Include(m => m.User)
                        .OrderBy(m => m.CreatedAt)
                        .Select(m => (ProductCartResponse)m),
                    pageNumber, pageSize);
            }
            else
            {
                return PagedList<ProductCartResponse>.ToPagedList(
                    _context.Set<ProductCart>()
                        .Where(m => m.IsDeleted == isDeleted)
                        .Where(m => m.User.Username.Contains(keyword))
                        .Include(m => m.User)
                        .OrderBy(m => m.CreatedAt)
                        .Select(m => (ProductCartResponse)m),
                    pageNumber, pageSize);
            }
        }

        public async Task<ProductCart> GetByIdAsync(string id, bool isDeleted)
        {
            _logger.LogInformation($"[PRODUCTCART BY ID] ID: {id}");

            return await _context.ProductCarts
                .Where(m => m.IsDeleted == isDeleted)
                .Include(m => m.User)
                .SingleAsync(m => m.Id == id) ?? throw new InvalidOperationException("ProductCart not found");
        }

        public async Task<ProductCart> GetByUserIdAsync(string userId, bool isDeleted)
        {
            _logger.LogInformation($"[PRODUCTCART BY USER ID] USER ID: {userId}");

            return await _context.ProductCarts
                .Where(m => m.IsDeleted == isDeleted)
                .Include(m => m.User)
                .SingleAsync(m => m.User.Id == userId) ?? throw new InvalidOperationException("ProductCart not found");
        }

        public async Task<bool> CreateAsync(ProductCartRequest productCartRequest)
        {
            _logger.LogInformation($"[PRODUCTCART CREATE]");

            ProductCart productCart = productCartRequest;
            if (productCartRequest.ProductIds?.Length > 0)
            {
                foreach (string productId in productCartRequest.ProductIds)
                {
                    Product product = await _context.Products
                        .Where(m => !m.IsDeleted)
                        .SingleAsync(m => m.Id == productId) ?? throw new InvalidOperationException("Product not found");

                    productCart.Products.Add(product);
                }
            }

            await _context.ProductCarts.AddAsync(productCart);
            return await _context.SaveChangesAsync() != 0;
        }

        public async Task<bool> UpdateAsync(ProductCartRequest productCartRequest, string id, bool isDeleted)
        {
            _logger.LogInformation($"[PRODUCTCART UPDATE] ID: {id}");

            ProductCart currentProductCart = await GetByIdAsync(id, isDeleted);

            currentProductCart.Products = new();
            if (productCartRequest.ProductIds?.Length > 0)
            {
                foreach (string productId in productCartRequest.ProductIds)
                {
                    Product product = await _context.Products
                        .Where(m => !m.IsDeleted)
                        .SingleAsync(m => m.Id == productId) ?? throw new InvalidOperationException("Product not found");

                    currentProductCart.Products.Add(product);
                }
            }

            _context.ProductCarts.Update(currentProductCart);
            return await _context.SaveChangesAsync() != 0;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            _logger.LogInformation($"[PRODUCTCART DELETE] ID: {id}");

            ProductCart currentProductCart = await GetByIdAsync(id, false);
            currentProductCart.IsDeleted = true;

            _context.ProductCarts.Update(currentProductCart);
            return await _context.SaveChangesAsync() != 0;
        }
    }
}
