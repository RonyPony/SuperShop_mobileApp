using superShop_API.Database.Entities;
using superShop_API.Database.Repositories.Constructor;
using superShop_API.Database.Services.Base;
using superShop_API.Shared;

namespace superShop_API.Database.Services;

public interface ICategoryService : IBaseService<Category, Guid>
{
    Task<Category?> GetByName(string name);
}

public class CategoryService : BaseService<Category, Guid>, ICategoryService
{
    public CategoryService(IRepositoryConstructor constructor) : base(constructor)
    {
    }

    public async Task<Category?> GetByName(string name) => (await this.Repository.GetAsync(c => c.Name == name)).FirstOrDefault();

    public async override Task<Result<Object>> ValidateOnCreateAsync(Category entity)
    {
        try
        {
            var categoryFound = (await this.Repository.GetAsync(c => c.Name == entity.Name)).FirstOrDefault();

            if (categoryFound != null)
            {
                return Result.Instance().Fail($"The name of category ('{entity.Name}') is already exists");
            }
            else
            {
                return Result.Instance().Success("Theres not found any category whit this name");
            }
        }
        catch (Exception e)
        {
            return Result.Instance().Fail("An error occurred while making the requested operation", e);
        }
    }
}