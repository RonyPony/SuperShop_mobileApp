using superShop_API.Database.Entities;
using superShop_API.Database.Repositories.Constructor;
using superShop_API.Database.Services.Base;
using superShop_API.Shared;

namespace superShop_API.Database.Services;

public interface IBranchesService : IBaseService<Branch, Guid, BranchSeedParams>
{
    Task<Branch> GetbyName(string name);
    Task<List<Branch>> GetAllByMallId(Guid mallId);

    Task<List<Branch>> GetAllByCategoryId(Guid categoryId);
}

public class BranchService : BaseService<Branch, Guid, BranchSeedParams>, IBranchesService
{
    public BranchService(IRepositoryConstructor constructor) : base(constructor)
    {
    }

    public async Task<Branch> GetbyName(string name) => (await this.Repository.GetAsync(b => b.Name == name)).FirstOrDefault();

    public async Task<List<Branch>> GetAllByMallId(Guid mallId) => (await this.Repository.GetAsync(b => b.MallId == mallId)).ToList();

    public async Task<List<Branch>> GetAllByCategoryId(Guid categoryId) => (await this.Repository.GetAsync(b => b.CategoryId == categoryId)).ToList();

    public async override Task<Result<Object>> ValidateOnCreateAsync(Branch entity)
    {
        try
        {
            var branchFound = (await this.Repository.GetAsync(m => m.Name == entity.Name)).FirstOrDefault();
            var categoryFound = (await this.Constructor.GetRepository<Category, Guid>().GetByIDAsync(entity.CategoryId));
            var mallFound = (await this.Constructor.GetRepository<Mall, Guid>().GetByIDAsync(entity.MallId));

            if (branchFound == null && categoryFound != null && mallFound != null)
            {
                return Result.Instance().Success("There noting branches in database");
            }
            else if (categoryFound == null)
            {
                return Result.Instance().Fail("The relational category doesn exists");
            }
            else if (mallFound == null)
            {
                return Result.Instance().Fail("The relational mall doesn exists");
            }

            return Result.Instance().Fail("The requested branch to create exists in database !");
        }
        catch (Exception e)
        {
            return Result.Instance().Fail("An error occurred while making the requested operation", e);
        }
    }
}