using superShop_API.Database.Entities.Base;
using superShop_API.Database.Repositories.Base;
using superShop_API.Database.Repositories.Constructor;
using superShop_API.Database.Seeders;
using superShop_API.Shared;

namespace superShop_API.Database.Services.Base;

public interface IBaseService<Tentity> where Tentity : class, IBaseEntity, ISeeder<Tentity>
{
    Task<Result<Object>> ValidateOnCreateAsync(Tentity entity);
    Task<Result<Object>> ValidateOnUpdateAsync(Tentity entity);
    Task<Result<Object>> ValidateOnDeleteAsync(Tentity entity);

    Task<List<Tentity>> GetAllAsync();
    Task<Tentity> GetByIDAsync(object id);
    Task<Result<Object>> UpdateAsync(Tentity entity);
    Task<Result<Object>> UpdateRangeAsync(IEnumerable<Tentity> entities);
    Task<Result<Object>> CreateAsync(Tentity entity);
    Task<Result<Object>> CreateRangeAsync(IEnumerable<Tentity> entities);
    Task<Result<Object>> DeleteAsync(object id);
    Task<Result<Object>> DeleteAsync(Tentity entity);
    Task<Result<Object>> DeleteRangeAsync(IEnumerable<Tentity> entities);

    Task<Result<TResult>> AdvanceQueryAsync<TResult>(TResult result, Func<IRepositoryConstructor, Task<Result<TResult>>> operation) where TResult : class;

    Task<Result<Object>> GenerateSeeders(int amount, object? referenceId);
}

public abstract class BaseService<Tentity> : IBaseService<Tentity> where Tentity : class, IBaseEntity, ISeeder<Tentity>
{
    protected readonly IRepositoryConstructor Constructor;
    protected IBaseRepository<Tentity> Repository { get => Constructor.GetRepository<Tentity>(); }

    public BaseService(IRepositoryConstructor constructor)
    {
        Constructor = constructor;
    }

    public virtual async Task<Result<TResult>> AdvanceQueryAsync<TResult>(TResult result, Func<IRepositoryConstructor, Task<Result<TResult>>> operation) where TResult : class => await operation(Constructor);

    public virtual async Task<Tentity> GetByIDAsync(object id)
    {
        return await Repository.GetByIDAsync(id);
    }

    public virtual async Task<List<Tentity>> GetAllAsync()
    {
        return (await Repository.GetAllAsync()).ToList();
    }

    public virtual async Task<Result<Object>> CreateAsync(Tentity entity)
    {
        try
        {
            var result = await ValidateOnCreateAsync(entity);
            if (result.IsSuccess)
            {
                await Repository.InsertAsync(entity);
                await Repository.CommitChangesAsync();
                return Result.Instance().Success($"{typeof(Tentity).Name} inserted successfully !");
            }
            else
            {
                return Result.Instance().Fail($"Fail on creating the new {typeof(Tentity).Name}", result);
            }
        }
        catch (Exception ex)
        {
            return Result.Instance().Fail($"These occurred an error while creating the new {typeof(Tentity).Name}", ex);
        }
    }

    public virtual async Task<Result<Object>> CreateRangeAsync(IEnumerable<Tentity> entities)
    {
        var errorList = new List<Result<Object>>();
        try
        {
            foreach (var model in entities)
            {
                var result = await ValidateOnCreateAsync(model);
                if (!result.IsSuccess)
                {
                    errorList.Add(result);
                }
            }

            if (errorList.Count > 0)
            {
                return Result.Instance().Fail($"Error in data for insert: {errorList.Count}", errorList);
            }
            else
            {
                await Repository.InsertRangeAsync(entities);
                await Repository.CommitChangesAsync();
                return Result.Instance().Success("models save successfully");
            }
        }
        catch (Exception ex)
        {
            return Result.Instance().Fail("Error creating entities in DB", ex);
        }
    }

    public virtual async Task<Result<Object>> DeleteAsync(object id)
    {
        try
        {
            var r = Result.Instance();
            var entity = await Repository.GetByIDAsync(id);
            if (entity != null)
            {
                var result = await ValidateOnDeleteAsync(entity);
                if (result.IsSuccess)
                {
                    await Repository.DeleteAsync(id);
                    await Repository.CommitChangesAsync();
                    r = Result.Instance().Success("entity delete successfully");
                }
                else
                {
                    r = Result.Instance().Fail("", result);

                }
            }
            return r;
        }
        catch (Exception ex)
        {
            return Result.Instance().Fail("Error deleting this entity from DB", ex);
        }
    }

    public virtual async Task<Result<Object>> DeleteAsync(Tentity entity)
    {
        try
        {
            var result = await ValidateOnDeleteAsync(entity);
            if (result.IsSuccess)
            {
                await Repository.DeleteAsync(entity);
                await Repository.CommitChangesAsync();
                return Result.Instance().Success("entity delete successfully");
            }
            else
            {
                return Result.Instance().Fail("", result);
            }
        }
        catch (Exception ex)
        {
            return Result.Instance().Fail("Error deleting this entity from DB", ex);
        }
    }

    public virtual async Task<Result<Object>> DeleteRangeAsync(IEnumerable<Tentity> entities)
    {
        try
        {
            var result = Result.Instance();
            foreach (var entity in entities)
            {
                result = await ValidateOnDeleteAsync(entity);
                if (!result.IsSuccess)
                {
                    break;
                }
            }
            if (result.IsSuccess)
            {
                await Repository.DeleteRangeAsync(entities);
                await Repository.CommitChangesAsync();
                return Result.Instance().Success("Entittes deleted successfully !");
            }
            else
            {
                return Result.Instance().Fail("", result);
            }
        }
        catch (Exception ex)
        {
            return Result.Instance().Fail("Error on delete the list of entities", ex);
        }
    }

    public virtual async Task<Result<Object>> UpdateAsync(Tentity entity)
    {
        try
        {
            var r = Result.Instance();
            var result = await ValidateOnUpdateAsync(entity);
            if (result.IsSuccess)
            {
                await Repository.UpdateAsync(entity);
                await Repository.CommitChangesAsync();
                r = Result.Instance().Success($"{entity.GetType().Name} entity update successfully !");
            }
            else
            {
                r = Result.Instance().Fail($"", result);
            }
            return r;
        }
        catch (Exception ex)
        {
            return Result.Instance().Fail("", ex);
        }
    }

    public virtual async Task<Result<Object>> UpdateRangeAsync(IEnumerable<Tentity> entities)
    {
        try
        {
            var result = Result.Instance();
            foreach (var entity in entities)
            {
                result = await ValidateOnUpdateAsync(entity);
                if (!result.IsSuccess)
                {
                    break;
                }
            }
            if (!result.IsSuccess)
            {
                result = Result.Instance().Fail($"error when try to eliminate {typeof(Tentity).Name} entity list", result);
            }
            else
            {
                await Repository.UpdateRangeAsync(entities);
                await Repository.CommitChangesAsync();
                result = Result.Instance().Success($"list of {typeof(Tentity).Name} entities update succefully !");
            }
            return result;
        }
        catch (Exception ex)
        {
            return Result.Instance().Fail($"error when try to update the list of {typeof(Tentity)} entities in database", ex);
        }
    }

    public virtual async Task<Result<Object>> GenerateSeeders(int amount, object? referenceId)
    {
        var e = (ISeeder<Tentity>)Activator.CreateInstance<Tentity>();

        var generatedData = e.SeederDefinition(referenceId).Generate(amount);

        return await CreateRangeAsync(generatedData);
    }

    public virtual async Task<Result<Object>> ValidateOnDeleteAsync(Tentity entity)
    {
        var found = await GetByIDAsync(entity);
        if (found == null)
        {
            return Result.Instance().Fail($"This {entity.GetType().Name} cannot be found in database");
        }
        else
        {
            return Result.Instance().Success($"{entity.GetType().Name} founded !");
        }
    }

    public virtual async Task<Result<Object>> ValidateOnUpdateAsync(Tentity entity)
    {
        var found = await GetByIDAsync(entity);
        if (found == null)
        {
            return Result.Instance().Fail($"This {entity.GetType().Name} cannot be found in database");
        }
        else
        {
            return Result.Instance().Success($"{entity.GetType().Name} founded !");
        }
    }

    public abstract Task<Result<Object>> ValidateOnCreateAsync(Tentity entity);
}


public abstract class BaseCustonService<TRepository, Tentity> : IBaseService<Tentity> where TRepository : IBaseRepository<Tentity> where Tentity : class, IBaseEntity, ISeeder<Tentity>
{
    protected readonly IRepositoryConstructor Constructor;

    protected BaseCustonService(IRepositoryConstructor _constructor)
    {
        Constructor = _constructor;
    }

    protected TRepository Repository { get => Constructor.GetRepositoryImplementation<TRepository, Tentity>(); }

    public virtual async Task<Result<TResult>> AdvanceQueryAsync<TResult>(TResult result, Func<IRepositoryConstructor, Task<Result<TResult>>> operation) where TResult : class => await operation(Constructor);

    public virtual async Task<Tentity> GetByIDAsync(object id)
    {
        return await Repository.GetByIDAsync(id);
    }

    public virtual async Task<List<Tentity>> GetAllAsync()
    {
        return (await Repository.GetAllAsync()).ToList();
    }

    public virtual async Task<Result<Object>> CreateAsync(Tentity entity)
    {
        try
        {
            if ((await ValidateOnCreateAsync(entity)).IsSuccess)
            {
                await Repository.InsertAsync(entity);
                await Repository.CommitChangesAsync();
                return Result.Instance().Success($"{typeof(Tentity).Name} inserted successfully !");
            }
            else
            {
                return Result.Instance().Fail($"Fail on creating the requested {entity.GetType().Name}");
            }
        }
        catch (Exception ex)
        {
            return Result.Instance().Fail("These an error occurred while creating this entity", exception: ex);
        }
    }

    public virtual async Task<Result<Object>> CreateRangeAsync(IEnumerable<Tentity> entities)
    {
        var errorFounds = 0;
        try
        {
            foreach (var model in entities)
            {
                if ((await ValidateOnCreateAsync(model)).IsSuccess)
                {
                    errorFounds++;
                }
            }

            if (errorFounds > 0)
            {
                return Result.Instance().Fail($"Error in data for insert: {errorFounds}");
            }
            else
            {
                await Repository.InsertRangeAsync(entities);
                await Repository.CommitChangesAsync();
                return Result.Instance().Success("models save successfully");
            }
        }
        catch (Exception ex)
        {
            return Result.Instance().Fail("Error creating entities in DB", exception: ex);
        }
    }

    public virtual async Task<Result<Object>> DeleteAsync(object id)
    {
        try
        {
            var r = Result.Instance();
            var entity = await Repository.GetByIDAsync(id);
            if (entity != null)
            {
                var result = await ValidateOnDeleteAsync(entity);
                if (result.IsSuccess)
                {
                    await Repository.DeleteAsync(id);
                    await Repository.CommitChangesAsync();
                    r = Result.Instance().Success("entity delete successfully");
                }
                else
                {
                    r = Result.Instance().Fail("", result);

                }
            }
            return r;
        }
        catch (Exception ex)
        {
            return Result.Instance().Fail("Error deleting this entity from DB", ex);
        }
    }

    public virtual async Task<Result<Object>> DeleteAsync(Tentity entity)
    {
        try
        {
            var result = await ValidateOnDeleteAsync(entity);
            if (result.IsSuccess)
            {
                await Repository.DeleteAsync(entity);
                await Repository.CommitChangesAsync();
                return Result.Instance().Success("entity delete successfully");
            }
            else
            {
                return Result.Instance().Fail("", result);
            }
        }
        catch (Exception ex)
        {
            return Result.Instance().Fail("Error deleting this entity from DB", ex);
        }
    }

    public virtual async Task<Result<Object>> DeleteRangeAsync(IEnumerable<Tentity> entities)
    {
        try
        {
            var result = Result.Instance();
            foreach (var entity in entities)
            {
                result = await ValidateOnDeleteAsync(entity);
                if (!result.IsSuccess)
                {
                    break;
                }
            }
            if (result.IsSuccess)
            {
                await Repository.DeleteRangeAsync(entities);
                await Repository.CommitChangesAsync();
                return Result.Instance().Success("Entittes deleted successfully !");
            }
            else
            {
                return Result.Instance().Fail("", result);
            }
        }
        catch (Exception ex)
        {
            return Result.Instance().Fail("Error on delete the list of entities", ex);
        }
    }

    public virtual async Task<Result<Object>> UpdateAsync(Tentity entity)
    {
        try
        {
            var r = Result.Instance();
            var result = await ValidateOnUpdateAsync(entity);
            if (result.IsSuccess)
            {
                await Repository.UpdateAsync(entity);
                await Repository.CommitChangesAsync();
                r = Result.Instance().Success($"{entity.GetType().Name} entity update successfully !");
            }
            else
            {
                r = Result.Instance().Fail($"", result);
            }
            return r;
        }
        catch (Exception ex)
        {
            return Result.Instance().Fail("", ex);
        }
    }

    public virtual async Task<Result<Object>> UpdateRangeAsync(IEnumerable<Tentity> entities)
    {
        try
        {
            var result = Result.Instance();
            foreach (var entity in entities)
            {
                result = await ValidateOnUpdateAsync(entity);
                if (!result.IsSuccess)
                {
                    break;
                }
            }
            if (!result.IsSuccess)
            {
                result = Result.Instance().Fail($"error when try to eliminate {typeof(Tentity).Name} entity list", result);
            }
            else
            {
                await Repository.UpdateRangeAsync(entities);
                await Repository.CommitChangesAsync();
                result = Result.Instance().Success($"list of {typeof(Tentity).Name} entities update succefully !");
            }
            return result;
        }
        catch (Exception ex)
        {
            return Result.Instance().Fail("There's an error when trying to update the requested range", ex);
        }

    }
    public async Task<Result<Object>> GenerateSeeders(int amount, object? referenceId)
    {

        var generatedData = Activator.CreateInstance<Tentity>().SeederDefinition(referenceId).Generate(amount);

        return await CreateRangeAsync(generatedData);
    }

    public virtual async Task<Result<Object>> ValidateOnDeleteAsync(Tentity entity)
    {
        var found = await GetByIDAsync(entity);
        if (found == null)
        {
            return Result.Instance().Fail($"This {entity.GetType().Name} cannot be found in database");
        }
        else
        {
            return Result.Instance().Success($"{entity.GetType().Name} founded !");
        }
    }

    public virtual async Task<Result<Object>> ValidateOnUpdateAsync(Tentity entity)
    {
        var found = await GetByIDAsync(entity);
        if (found == null)
        {
            return Result.Instance().Fail($"This {entity.GetType().Name} cannot be found in database");
        }
        else
        {
            return Result.Instance().Success($"{entity.GetType().Name} founded !");
        }
    }

    public abstract Task<Result<Object>> ValidateOnCreateAsync(Tentity entity);
}

public interface IBaseService<Tentity, T> where Tentity : class, IBaseEntity, ISeeder<Tentity, T>
{
    Task<Result<Object>> ValidateOnCreateAsync(Tentity entity);
    Task<Result<Object>> ValidateOnUpdateAsync(Tentity entity);
    Task<Result<Object>> ValidateOnDeleteAsync(Tentity entity);

    Task<List<Tentity>> GetAllAsync();
    Task<Tentity> GetByIDAsync(object id);
    Task<Result<Object>> UpdateAsync(Tentity entity);
    Task<Result<Object>> UpdateRangeAsync(IEnumerable<Tentity> entities);
    Task<Result<Object>> CreateAsync(Tentity entity);
    Task<Result<Object>> CreateRangeAsync(IEnumerable<Tentity> entities);
    Task<Result<Object>> DeleteAsync(object id);
    Task<Result<Object>> DeleteAsync(Tentity entity);
    Task<Result<Object>> DeleteRangeAsync(IEnumerable<Tentity> entities);

    Task<Result<TResult>> AdvanceQueryAsync<TResult>(TResult result, Func<IRepositoryConstructor, Task<Result<TResult>>> operation) where TResult : class;

    Task<Result<Object>> GenerateSeeders(int amount, T data);
}

public abstract class BaseService<Tentity, T> : IBaseService<Tentity, T> where Tentity : class, IBaseEntity, ISeeder<Tentity, T>
{
    protected readonly IRepositoryConstructor Constructor;
    protected IBaseRepository<Tentity> Repository { get => Constructor.GetRepository<Tentity>(); }

    public BaseService(IRepositoryConstructor constructor)
    {
        Constructor = constructor;
    }

    public virtual async Task<Result<TResult>> AdvanceQueryAsync<TResult>(TResult result, Func<IRepositoryConstructor, Task<Result<TResult>>> operation) where TResult : class => await operation(Constructor);

    public virtual async Task<Tentity> GetByIDAsync(object id)
    {
        return await Repository.GetByIDAsync(id);
    }

    public virtual async Task<List<Tentity>> GetAllAsync()
    {
        return (await Repository.GetAllAsync()).ToList();
    }

    public virtual async Task<Result<Object>> CreateAsync(Tentity entity)
    {
        try
        {
            if ((await ValidateOnCreateAsync(entity)).IsSuccess)
            {
                await Repository.InsertAsync(entity);
                await Repository.CommitChangesAsync();
                return Result.Instance().Success($"{typeof(Tentity).Name} inserted successfully !");
            }
            else
            {
                return Result.Instance().Fail("Fail ");
            }
        }
        catch (Exception ex)
        {
            return Result.Instance().Fail("", ex);
        }
    }

    public virtual async Task<Result<Object>> CreateRangeAsync(IEnumerable<Tentity> entities)
    {
        var errorFounds = 0;
        try
        {
            foreach (var model in entities)
            {
                if ((await ValidateOnCreateAsync(model)).IsSuccess)
                {
                    errorFounds++;
                }
            }

            if (errorFounds > 0)
            {
                return Result.Instance().Fail($"Error in data for insert: {errorFounds}");
            }
            else
            {
                await Repository.InsertRangeAsync(entities);
                await Repository.CommitChangesAsync();
                return Result.Instance().Success("models save successfully");
            }
        }
        catch (Exception ex)
        {
            return Result.Instance().Fail("Error creating entities in DB", ex);
        }
    }

    public virtual async Task<Result<Object>> DeleteAsync(object id)
    {
        try
        {
            var r = Result.Instance();
            var entity = await Repository.GetByIDAsync(id);
            if (entity != null)
            {
                var result = await ValidateOnDeleteAsync(entity);
                if (result.IsSuccess)
                {
                    await Repository.DeleteAsync(id);
                    await Repository.CommitChangesAsync();
                    r = Result.Instance().Success("entity delete successfully");
                }
                else
                {
                    r = Result.Instance().Fail("", result);

                }
            }
            return r;
        }
        catch (Exception ex)
        {
            return Result.Instance().Fail("Error deleting this entity from DB", ex);
        }
    }

    public virtual async Task<Result<Object>> DeleteAsync(Tentity entity)
    {
        try
        {
            var result = await ValidateOnDeleteAsync(entity);
            if (result.IsSuccess)
            {
                await Repository.DeleteAsync(entity);
                await Repository.CommitChangesAsync();
                return Result.Instance().Success("entity delete successfully");
            }
            else
            {
                return Result.Instance().Fail("", result);
            }
        }
        catch (Exception ex)
        {
            return Result.Instance().Fail("Error deleting this entity from DB", ex);
        }
    }

    public virtual async Task<Result<Object>> DeleteRangeAsync(IEnumerable<Tentity> entities)
    {
        try
        {
            var result = Result.Instance();
            foreach (var entity in entities)
            {
                result = await ValidateOnDeleteAsync(entity);
                if (!result.IsSuccess)
                {
                    break;
                }
            }
            if (result.IsSuccess)
            {
                await Repository.DeleteRangeAsync(entities);
                await Repository.CommitChangesAsync();
                return Result.Instance().Success("Entittes deleted successfully !");
            }
            else
            {
                return Result.Instance().Fail("", result);
            }
        }
        catch (Exception ex)
        {
            return Result.Instance().Fail("Error on delete the list of entities", ex);
        }
    }

    public virtual async Task<Result<Object>> UpdateAsync(Tentity entity)
    {
        try
        {
            var r = Result.Instance();
            var result = await ValidateOnUpdateAsync(entity);
            if (result.IsSuccess)
            {
                await Repository.UpdateAsync(entity);
                await Repository.CommitChangesAsync();
                r = Result.Instance().Success($"{entity.GetType().Name} entity update successfully !");
            }
            else
            {
                r = Result.Instance().Fail($"", result);
            }
            return r;
        }
        catch (Exception ex)
        {
            return Result.Instance().Fail("", ex);
        }
    }

    public virtual async Task<Result<Object>> UpdateRangeAsync(IEnumerable<Tentity> entities)
    {
        try
        {
            var result = Result.Instance();
            foreach (var entity in entities)
            {
                result = await ValidateOnUpdateAsync(entity);
                if (!result.IsSuccess)
                {
                    break;
                }
            }
            if (!result.IsSuccess)
            {
                result = Result.Instance().Fail($"error when try to eliminate {typeof(Tentity).Name} entity list", result);
            }
            else
            {
                await Repository.UpdateRangeAsync(entities);
                await Repository.CommitChangesAsync();
                result = Result.Instance().Success($"list of {typeof(Tentity).Name} entities update succefully !");
            }
            return result;
        }
        catch (Exception ex)
        {
            return Result.Instance().Fail($"error when try to update the list of {typeof(Tentity)} entities in database", ex);
        }
    }

    public virtual async Task<Result<Object>> GenerateSeeders(int amount, T data)
    {
        var e = (ISeeder<Tentity, T>)Activator.CreateInstance<Tentity>();

        var generatedData = e.SeederDefinition(data).Generate(amount);

        return await CreateRangeAsync(generatedData);
    }

    public virtual async Task<Result<Object>> ValidateOnDeleteAsync(Tentity entity)
    {
        var found = await GetByIDAsync(entity);
        if (found == null)
        {
            return Result.Instance().Fail($"This {entity.GetType().Name} cannot be found in database");
        }
        else
        {
            return Result.Instance().Success($"{entity.GetType().Name} founded !");
        }
    }

    public virtual async Task<Result<Object>> ValidateOnUpdateAsync(Tentity entity)
    {
        var found = await GetByIDAsync(entity);
        if (found == null)
        {
            return Result.Instance().Fail($"This {entity.GetType().Name} cannot be found in database");
        }
        else
        {
            return Result.Instance().Success($"{entity.GetType().Name} founded !");
        }
    }

    public abstract Task<Result<Object>> ValidateOnCreateAsync(Tentity entity);
}

public abstract class BaseCustonService<TRepository, Tentity, T> : IBaseService<Tentity, T> where TRepository : IBaseRepository<Tentity> where Tentity : class, IBaseEntity, ISeeder<Tentity, T>
{
    protected readonly IRepositoryConstructor Constructor;

    protected BaseCustonService(IRepositoryConstructor _constructor)
    {
        Constructor = _constructor;
    }

    protected TRepository Repository { get => Constructor.GetRepositoryImplementation<TRepository, Tentity>(); }

    public virtual async Task<Result<TResult>> AdvanceQueryAsync<TResult>(TResult result, Func<IRepositoryConstructor, Task<Result<TResult>>> operation) where TResult : class => await operation(Constructor);

    public virtual async Task<Tentity> GetByIDAsync(object id)
    {
        return await Repository.GetByIDAsync(id);
    }

    public virtual async Task<List<Tentity>> GetAllAsync()
    {
        return (await Repository.GetAllAsync()).ToList();
    }

    public virtual async Task<Result<Object>> CreateAsync(Tentity entity)
    {
        try
        {
            if ((await ValidateOnCreateAsync(entity)).IsSuccess)
            {
                await Repository.InsertAsync(entity);
                await Repository.CommitChangesAsync();
                return Result.Instance().Success($"{typeof(Tentity).Name} inserted successfully !");
            }
            else
            {
                return Result.Instance().Fail("Fail ");
            }
        }
        catch (Exception ex)
        {
            return Result.Instance().Fail("", ex);
        }
    }

    public virtual async Task<Result<Object>> CreateRangeAsync(IEnumerable<Tentity> entities)
    {
        var errorFounds = 0;
        try
        {
            foreach (var model in entities)
            {
                if ((await ValidateOnCreateAsync(model)).IsSuccess)
                {
                    errorFounds++;
                }
            }

            if (errorFounds > 0)
            {
                return Result.Instance().Fail($"Error in data for insert: {errorFounds}");
            }
            else
            {
                await Repository.InsertRangeAsync(entities);
                await Repository.CommitChangesAsync();
                return Result.Instance().Success("models save successfully");
            }
        }
        catch (Exception ex)
        {
            return Result.Instance().Fail("Error creating entities in DB", ex);
        }
    }

    public virtual async Task<Result<Object>> DeleteAsync(object id)
    {
        try
        {
            var r = Result.Instance();
            var entity = await Repository.GetByIDAsync(id);
            if (entity != null)
            {
                var result = await ValidateOnDeleteAsync(entity);
                if (result.IsSuccess)
                {
                    await Repository.DeleteAsync(id);
                    await Repository.CommitChangesAsync();
                    r = Result.Instance().Success("entity delete successfully");
                }
                else
                {
                    r = Result.Instance().Fail("", result);

                }
            }
            return r;
        }
        catch (Exception ex)
        {
            return Result.Instance().Fail("Error deleting this entity from DB", ex);
        }
    }

    public virtual async Task<Result<Object>> DeleteAsync(Tentity entity)
    {
        try
        {
            var result = await ValidateOnDeleteAsync(entity);
            if (result.IsSuccess)
            {
                await Repository.DeleteAsync(entity);
                await Repository.CommitChangesAsync();
                return Result.Instance().Success("entity delete successfully");
            }
            else
            {
                return Result.Instance().Fail("", result);
            }
        }
        catch (Exception ex)
        {
            return Result.Instance().Fail("Error deleting this entity from DB", ex);
        }
    }

    public virtual async Task<Result<Object>> DeleteRangeAsync(IEnumerable<Tentity> entities)
    {
        try
        {
            var result = Result.Instance();
            foreach (var entity in entities)
            {
                result = await ValidateOnDeleteAsync(entity);
                if (!result.IsSuccess)
                {
                    break;
                }
            }
            if (result.IsSuccess)
            {
                await Repository.DeleteRangeAsync(entities);
                await Repository.CommitChangesAsync();
                return Result.Instance().Success("Entittes deleted successfully !");
            }
            else
            {
                return Result.Instance().Fail("", result);
            }
        }
        catch (Exception ex)
        {
            return Result.Instance().Fail("Error on delete the list of entities", ex);
        }
    }

    public virtual async Task<Result<Object>> UpdateAsync(Tentity entity)
    {
        try
        {
            var r = Result.Instance();
            var result = await ValidateOnUpdateAsync(entity);
            if (result.IsSuccess)
            {
                await Repository.UpdateAsync(entity);
                await Repository.CommitChangesAsync();
                r = Result.Instance().Success($"{entity.GetType().Name} entity update successfully !");
            }
            else
            {
                r = Result.Instance().Fail($"", result);
            }
            return r;
        }
        catch (Exception ex)
        {
            return Result.Instance().Fail("", ex);
        }
    }

    public virtual async Task<Result<Object>> UpdateRangeAsync(IEnumerable<Tentity> entities)
    {
        try
        {
            var result = Result.Instance();
            foreach (var entity in entities)
            {
                result = await ValidateOnUpdateAsync(entity);
                if (!result.IsSuccess)
                {
                    break;
                }
            }
            if (!result.IsSuccess)
            {
                result = Result.Instance().Fail($"error when try to eliminate {typeof(Tentity).Name} entity list", result);
            }
            else
            {
                await Repository.UpdateRangeAsync(entities);
                await Repository.CommitChangesAsync();
                result = Result.Instance().Success($"list of {typeof(Tentity).Name} entities update succefully !");
            }
            return result;
        }
        catch (Exception ex)
        {
            return Result.Instance().Fail("There's an error when trying to update the requested range", ex);
        }

    }
    public async Task<Result<Object>> GenerateSeeders(int amount, T data)
    {

        var generatedData = Activator.CreateInstance<Tentity>().SeederDefinition(data).Generate(amount);

        return await CreateRangeAsync(generatedData);
    }

    public virtual async Task<Result<Object>> ValidateOnDeleteAsync(Tentity entity)
    {
        var found = await GetByIDAsync(entity);
        if (found == null)
        {
            return Result.Instance().Fail($"This {entity.GetType().Name} cannot be found in database");
        }
        else
        {
            return Result.Instance().Success($"{entity.GetType().Name} founded !");
        }
    }

    public virtual async Task<Result<Object>> ValidateOnUpdateAsync(Tentity entity)
    {
        var found = await GetByIDAsync(entity);
        if (found == null)
        {
            return Result.Instance().Fail($"This {entity.GetType().Name} cannot be found in database");
        }
        else
        {
            return Result.Instance().Success($"{entity.GetType().Name} founded !");
        }
    }

    public abstract Task<Result<Object>> ValidateOnCreateAsync(Tentity entity);
}
