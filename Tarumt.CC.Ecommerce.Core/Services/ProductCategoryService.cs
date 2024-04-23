using Microsoft.EntityFrameworkCore;
using Tarumt.CC.Ecommerce.Core.Infrastructure.Context;
using Tarumt.CC.Ecommerce.Core.Infrastructure.Models;
using Tarumt.CC.Ecommerce.SharedLibrary.Infrastructure.Requests.Admin;
using Tarumt.CC.Ecommerce.SharedLibrary.Infrastructure.Responses;

namespace Tarumt.CC.Ecommerce.Core.Services
{
    public class ProductCategoryService(ILogger<ProductCategoryService> logger, CoreContext context)
    {
        public PagedList<ProductCategory> GetAll(int pageNumber, int pageSize, string keyword, bool isDeleted)
        {
            logger.LogInformation($"[PRODUCTCATEGORY GET ALL] Page Number: {pageNumber}; Page Size: {pageSize}; Keyword: {keyword}");

            if (string.IsNullOrEmpty(keyword))
            {
                return PagedList<ProductCategory>.ToPagedList(
                    context.Set<ProductCategory>()
                        .Where(m => m.IsDeleted == isDeleted)
                        .OrderBy(m => m.CreatedAt),
                    pageNumber, pageSize);
            }
            else
            {
                return PagedList<ProductCategory>.ToPagedList(
                    context.Set<ProductCategory>()
                        .Where(m => m.IsDeleted == isDeleted)
                        .Where(m => m.Name.Contains(keyword))
                        .OrderBy(m => m.CreatedAt),
                    pageNumber, pageSize);
            }
        }

        public PagedList<ProductCategoryResponse> GetAllResponse(int pageNumber, int pageSize, string keyword, bool isDeleted)
        {
            logger.LogInformation($"[PRODUCTCATEGORY GET ALL] Page Number: {pageNumber}; Page Size: {pageSize}; Keyword: {keyword}");

            if (string.IsNullOrEmpty(keyword))
            {
                return PagedList<ProductCategoryResponse>.ToPagedList(
                    context.Set<ProductCategory>()
                        .Where(m => m.IsDeleted == isDeleted)
                        .OrderBy(m => m.CreatedAt)
                        .Select(m => (ProductCategoryResponse)m),
                    pageNumber, pageSize);
            }
            else
            {
                return PagedList<ProductCategoryResponse>.ToPagedList(
                    context.Set<ProductCategory>()
                        .Where(m => m.IsDeleted == isDeleted)
                        .Where(m => m.Name.Contains(keyword))
                        .OrderBy(m => m.CreatedAt)
                        .Select(m => (ProductCategoryResponse)m),
                    pageNumber, pageSize);
            }
        }

        public async Task<ProductCategory> GetByIdAsync(string id, bool isDeleted)
        {
            logger.LogInformation($"[PRODUCTCATEGORY GET BY ID] ID: {id}");

            return await context.ProductCategories
                .Where(m => m.IsDeleted == isDeleted)
                .SingleAsync(m => m.Id == id) ?? throw new InvalidOperationException("Product Category not found");
        }

        public async Task<ProductCategory> GetByNameAsync(string name, bool isDeleted)
        {
            logger.LogInformation($"[PRODUCTCATEGORY GET BY Name] ID: {name}");

            return await context.ProductCategories
                .Where(m => m.IsDeleted == isDeleted)
                .SingleAsync(m => m.Name == name) ?? throw new InvalidOperationException("Product Category not found");
        }

        public async Task<bool> CreateAsync(ProductCategoryAdminRequest productCategoryAdminRequest)
        {
            logger.LogInformation($"[PRODUCTCATEGORY CREATE]");

            await context.ProductCategories.AddAsync((ProductCategory)productCategoryAdminRequest);
            return await context.SaveChangesAsync() != 0;
        }

        public async Task<bool> UpdateAsync(ProductCategoryAdminRequest productCategoryAdminRequest, string id, bool isDeleted)
        {
            logger.LogInformation($"[PRODUCTCATEGORY UPDATE] ID: {id}");

            ProductCategory currentProductCategory = await GetByIdAsync(id, isDeleted);
            currentProductCategory.Name = productCategoryAdminRequest.Name;

            context.ProductCategories.Update(currentProductCategory);
            return await context.SaveChangesAsync() != 0;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            logger.LogInformation($"[PRODUCTCATEGORY DELETE] ID: {id}");

            ProductCategory currentProductCategory = await GetByIdAsync(id, false);
            currentProductCategory.IsDeleted = true;

            context.ProductCategories.Update(currentProductCategory);
            return await context.SaveChangesAsync() != 0;
        }
    }
}
