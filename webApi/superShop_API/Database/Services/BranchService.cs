using superShop_API.Database.Entities;
using superShop_API.Database.Repositories.Constructor;
using superShop_API.Database.Services.Base;
using superShop_API.Shared;

namespace superShop_API.Database.Services;

public interface IBranchesService : IBaseService<Branch>
{
    Task<Branch> GetByLocalCode(string localCode);
    Task<Branch> GetbyName(string name);
}

public class BranchService : BaseService<Branch>, IBranchesService
{
    public BranchService(IRepositoryConstructor constructor) : base(constructor)
    {
    }

    public async Task<Branch> GetByLocalCode(string localCode) => (await this.Repository.GetAsync(b => b.LocalCode == localCode)).FirstOrDefault();

    public async Task<Branch> GetbyName(string name) => (await this.Repository.GetAsync(b => b.Name == name)).FirstOrDefault();

    public async override Task<Result> ValidateOnCreateAsync(Branch entity)
    {
        try
        {
            var mallFound = await this.Repository.GetAsync(m => m.Name == entity.Name);
            if (mallFound == null)
            {
                return Result.Instance().Success("There noting branches in database");
            }
            return Result.Instance().Fail("The requested branch to create exists in database !");
        }
        catch (Exception ex)
        {
            return Result.Instance().Fail("", ex);
        }
    }
}