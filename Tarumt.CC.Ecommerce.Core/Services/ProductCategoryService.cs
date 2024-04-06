using Ky.Web.CMS.SharedLibarary.Infrastructure.Requests.Admin;
using Ky.Web.CMS.SharedLibarary.Infrastructure.Responses;
using Microsoft.EntityFrameworkCore;
using Tarumt.CC.Ecommerce.Infrastructure.Context;
using Tarumt.CC.Ecommerce.Infrastructure.Models;

namespace Tarumt.CC.Ecommerce.Services
{
    public class ProductCategoryService
    {
        private readonly ILogger<ProductCategoryService> _logger;
        private readonly CoreContext _context;

        public ProductCategoryService(ILogger<ProductCategoryService> logger, CoreContext context)
        {
            _logger = logger;
            _context = context;
        }

        public PagedList<ProductCategory> GetAll(int pageNumber, int pageSize, string keyword, bool isDeleted)
        {
            _logger.LogInformation($"[PRODUCTCATEGORY GET ALL] Page Number: {pageNumber}; Page Size: {pageSize}; Keyword: {keyword}");

            if (string.IsNullOrEmpty(keyword))
            {
                return PagedList<ProductCategory>.ToPagedList(
                    _context.Set<ProductCategory>()
                        .Where(m => m.IsDeleted == isDeleted)
                        .OrderBy(m => m.CreatedAt),
                    pageNumber, pageSize);
            }
            else
            {
                return PagedList<ProductCategory>.ToPagedList(
                    _context.Set<ProductCategory>()
                        .Where(m => m.IsDeleted == isDeleted)
                        .Where(m => m.Name.Contains(keyword))
                        .OrderBy(m => m.CreatedAt),
                    pageNumber, pageSize);
            }
        }

        public PagedList<ProductCategoryResponse> GetAllResponse(int pageNumber, int pageSize, string keyword, bool isDeleted)
        {
            _logger.LogInformation($"[PRODUCTCATEGORY GET ALL] Page Number: {pageNumber}; Page Size: {pageSize}; Keyword: {keyword}");

            if (string.IsNullOrEmpty(keyword))
            {
                return PagedList<ProductCategoryResponse>.ToPagedList(
                    _context.Set<ProductCategory>()
                        .Where(m => m.IsDeleted == isDeleted)
                        .OrderBy(m => m.CreatedAt)
                        .Select(m => (ProductCategoryResponse)m),
                    pageNumber, pageSize);
            }
            else
            {
                return PagedList<ProductCategoryResponse>.ToPagedList(
                    _context.Set<ProductCategory>()
                        .Where(m => m.IsDeleted == isDeleted)
                        .Where(m => m.Name.Contains(keyword))
                        .OrderBy(m => m.CreatedAt)
                        .Select(m => (ProductCategoryResponse)m),
                    pageNumber, pageSize);
            }
        }

        public async Task<ProductCategory> GetByIdAsync(string id, bool isDeleted)
        {
            _logger.LogInformation($"[PRODUCTCATEGORY GET BY ID] ID: {id}");

            return await _context.ProductCategories
                .Where(m => m.IsDeleted == isDeleted)
                .SingleAsync(m => m.Id == id) ?? throw new InvalidOperationException("Product Category not found");
        }

        public async Task<bool> CreateAsync(ProductCategoryAdminRequest productCategoryAdminRequest)
        {
            _logger.LogInformation($"[PRODUCTCATEGORY CREATE]");

            await _context.ProductCategories.AddAsync((ProductCategory)productCategoryAdminRequest);
            return await _context.SaveChangesAsync() != 0;
        }

        public async Task<bool> UpdateAsync(ProductCategoryAdminRequest productCategoryAdminRequest, string id, bool isDeleted)
        {
            _logger.LogInformation($"[PRODUCTCATEGORY UPDATE] ID: {id}");

            ProductCategory currentProductCategory = await GetByIdAsync(id, isDeleted);
            currentProductCategory.Name = productCategoryAdminRequest.Name;

            _context.ProductCategories.Update(currentProductCategory);
            return await _context.SaveChangesAsync() != 0;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            _logger.LogInformation($"[PRODUCTCATEGORY DELETE] ID: {id}");

            ProductCategory currentProductCategory = await GetByIdAsync(id, false);
            currentProductCategory.IsDeleted = true;

            _context.ProductCategories.Update(currentProductCategory);
            return await _context.SaveChangesAsync() != 0;
        }
    }
}
