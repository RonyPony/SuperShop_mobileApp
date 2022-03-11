using superShop_API.Database.Entities;
using superShop_API.Database.Repositories.Constructor;
using superShop_API.Database.Services.Base;
using superShop_API.Shared;

namespace superShop_API.Database.Services;

public interface IProductService : IBaseService<Product>
{
    Task<Product> GetByCode(string code);
    Task<List<Product>> GetAllByBranchId(Guid branchId);
}

public class ProductService : BaseService<Product>, IProductService
{
    public ProductService(IRepositoryConstructor constructor) : base(constructor)
    {
    }

    public async Task<List<Product>> GetAllByBranchId(Guid branchId) => (await this.Repository.GetAsync(m => m.BranchId == branchId)).ToList();

    public async Task<Product> GetByCode(string code) => (await this.Repository.GetAsync(p => p.Code == code)).FirstOrDefault();

    public async override Task<Result<Object>> ValidateOnCreateAsync(Product entity)
    {
        try
        {
            var mallFound = (await this.Repository.GetAsync(m => m.Name == entity.Name)).FirstOrDefault();
            if (mallFound == null)
            {
                return Result.Instance().Success("There noting products in database");
            }
            return Result.Instance().Fail("The requested product to create exists in database !");
        }
        catch (Exception e)
        {
            return Result.Instance().Fail("An error occurred while making the requested operation", e);
        }
    }
}