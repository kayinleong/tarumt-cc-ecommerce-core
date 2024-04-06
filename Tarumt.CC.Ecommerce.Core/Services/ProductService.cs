using Ky.Web.CMS.SharedLibarary.Infrastructure.Requests.Admin;
using Ky.Web.CMS.SharedLibarary.Infrastructure.Responses;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using Tarumt.CC.Ecommerce.Infrastructure.Context;
using Tarumt.CC.Ecommerce.Infrastructure.Models;

namespace Tarumt.CC.Ecommerce.Services
{
    public class ProductService
    {
        private readonly ILogger<ProductService> _logger;
        private readonly CoreContext _context;

        public ProductService(ILogger<ProductService> logger, CoreContext context)
        {
            _logger = logger;
            _context = context;
        }

        public PagedList<Product> GetAll(int pageNumber, int pageSize, string keyword, bool isDeleted)
        {
            _logger.LogInformation($"[PRODUCT GET ALL] Page Number: {pageNumber}; Page Size: {pageSize}; Keyword: {keyword}");

            if (string.IsNullOrEmpty(keyword))
            {
                return PagedList<Product>.ToPagedList(
                    _context.Set<Product>()
                        .Where(m => m.IsDeleted == isDeleted)
                        .Include(m => m.Categories)
                        .OrderBy(m => m.CreatedAt),
                    pageNumber, pageSize);
            }
            else
            {
                return PagedList<Product>.ToPagedList(
                    _context.Set<Product>()
                        .Where(m => m.IsDeleted == isDeleted)
                        .Where(m => m.Name.Contains(keyword))
                        .Include(m => m.Categories)
                        .OrderBy(m => m.CreatedAt),
                    pageNumber, pageSize);
            }
        }

        public PagedList<ProductResponse> GetAllResponse(int pageNumber, int pageSize, string keyword, bool isDeleted)
        {
            _logger.LogInformation($"[PRODUCT GET ALL] Page Number: {pageNumber}; Page Size: {pageSize}; Keyword: {keyword}");

            if (string.IsNullOrEmpty(keyword))
            {
                return PagedList<ProductResponse>.ToPagedList(
                    _context.Set<Product>()
                        .Where(m => m.IsDeleted == isDeleted)
                        .Include(m => m.Categories)
                        .OrderBy(m => m.CreatedAt)
                        .Select(m => (ProductResponse)m),
                    pageNumber, pageSize);
            }
            else
            {
                return PagedList<ProductResponse>.ToPagedList(
                    _context.Set<Product>()
                        .Where(m => m.IsDeleted == isDeleted)
                        .Where(m => m.Name.Contains(keyword))
                        .Include(m => m.Categories)
                        .OrderBy(m => m.CreatedAt)
                        .Select(m => (ProductResponse)m),
                    pageNumber, pageSize);
            }
        }

        public PagedList<Product> GetAllByIsNotExpired(int pageNumber, int pageSize, string keyword, bool isDeleted)
        {
            _logger.LogInformation($"[PRODUCT GET ALL BY IS NOT EXPIRED] Page Number: {pageNumber}; Page Size: {pageSize}; Keyword: {keyword}");

            if (string.IsNullOrEmpty(keyword))
            {
                return PagedList<Product>.ToPagedList(
                    _context.Set<Product>()
                        .Where(m => m.IsDeleted == isDeleted)
                        .Where(m => m.ExpiredAt < DateTime.Now)
                        .Include(m => m.Categories)
                        .OrderBy(m => m.CreatedAt),
                    pageNumber, pageSize);
            }
            else
            {
                return PagedList<Product>.ToPagedList(
                    _context.Set<Product>()
                        .Where(m => m.IsDeleted == isDeleted)
                        .Where(m => m.ExpiredAt < DateTime.Now)
                        .Where(m => m.Name.Contains(keyword))
                        .Include(m => m.Categories)
                        .OrderBy(m => m.CreatedAt),
                    pageNumber, pageSize);
            }
        }

        public async Task<Product> GetByIdAsync(string id, bool isDeleted)
        {
            _logger.LogInformation($"[PRODUCT BY ID] ID: {id}");

            return await _context.Products
                .Where(m => m.IsDeleted == isDeleted)
                .Include(m => m.Categories)
                .SingleAsync(m => m.Id == id) ?? throw new InvalidOperationException("Product not found");
        }

        public async Task<bool> CreateAsync(ProductAdminRequest productAdminRequest)
        {
            _logger.LogInformation($"[PRODUCT CREATE]");

            Product product = productAdminRequest;
            if (productAdminRequest.CategoriesId?.Length > 0)
            {
                foreach (string categoryId in productAdminRequest.CategoriesId)
                {
                    ProductCategory category = await _context.ProductCategories
                        .Where(m => !m.IsDeleted)
                        .SingleAsync(m => m.Id == categoryId) ?? throw new InvalidOperationException("ProductCategory not found");

                    product.Categories.Add(category);
                }
            }
            else
            {
                product.Categories.AddRange(productAdminRequest.Categories!.Select(m => (ProductCategory)m));
            }

            await _context.Products.AddAsync(product);
            return await _context.SaveChangesAsync() != 0;
        }

        public async Task<bool> UpdateAsync(ProductAdminRequest productAdminRequest, string id, bool isDeleted)
        {
            _logger.LogInformation($"[PRODUCT UPDATE] ID: {id}");

            Product currentProduct = await GetByIdAsync(id, isDeleted);

            currentProduct.Name = productAdminRequest.Name;
            currentProduct.ShortName = productAdminRequest.ShortName;
            currentProduct.Count = productAdminRequest.Count;
            currentProduct.Price = productAdminRequest.Price;
            currentProduct.DiscountRate = productAdminRequest.DiscountRate;
            currentProduct.Description = productAdminRequest.Description;
            currentProduct.ImageUrl = productAdminRequest.ImageUrl;
            currentProduct.StartAt = productAdminRequest.StartAt != null
                ? DateTime.ParseExact(
                    productAdminRequest.StartAt,
                    "M/d/yyyy",
                    CultureInfo.InvariantCulture)
                : null;
            currentProduct.ExpiredAt = productAdminRequest.ExpiredAt != null
                ? DateTime.ParseExact(
                    productAdminRequest.ExpiredAt,
                    "M/d/yyyy",
                    CultureInfo.InvariantCulture)
                : null;

            _context.Products.Update(currentProduct);
            return await _context.SaveChangesAsync() != 0;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            _logger.LogInformation($"[PRODUCT DELETE] ID: {id}");

            Product currentProduct = await GetByIdAsync(id, false);
            currentProduct.IsDeleted = true;

            _context.Products.Update(currentProduct);
            return await _context.SaveChangesAsync() != 0;
        }
    }
}
