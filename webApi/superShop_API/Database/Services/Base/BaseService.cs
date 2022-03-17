using superShop_API.Database.Entities.Base;
using superShop_API.Database.Repositories.Base;
using superShop_API.Database.Repositories.Constructor;
using superShop_API.Database.Seeders;
using superShop_API.Shared;

namespace superShop_API.Database.Services.Base;

public interface IBaseService<TEntity, TKey> where TEntity : class, IBaseEntity<TKey>, ISeeder<TEntity, TKey> where TKey : IEquatable<TKey>
{
    Task<Result<Object>> ValidateOnCreateAsync(TEntity entity);
    Task<Result<Object>> ValidateOnUpdateAsync(TEntity entity);
    Task<Result<Object>> ValidateOnDeleteAsync(TEntity entity);

    Task<List<TEntity>> GetAllAsync();
    Task<TEntity> GetByIDAsync(TKey id);
    Task<Result<Object>> UpdateAsync(TEntity entity);
    Task<Result<Object>> UpdateRangeAsync(IEnumerable<TEntity> entities);
    Task<Result<Object>> CreateAsync(TEntity entity);
    Task<Result<Object>> CreateRangeAsync(IEnumerable<TEntity> entities);
    Task<Result<Object>> DeleteAsync(TKey id);
    Task<Result<Object>> DeleteAsync(TEntity entity);
    Task<Result<Object>> DeleteRangeAsync(IEnumerable<TEntity> entities);

    Task<Result<TResult>> AdvanceQueryAsync<TResult>(TResult result, Func<IRepositoryConstructor, Task<Result<TResult>>> operation) where TResult : class;

    Task<Result<Object>> GenerateSeeders(int amount, object? referenceId);
}

public abstract class BaseService<TEntity, TKey> : IBaseService<TEntity, TKey> where TEntity : class, IBaseEntity<TKey>, ISeeder<TEntity, TKey> where TKey : IEquatable<TKey>
{
    protected readonly IRepositoryConstructor Constructor;
    protected IBaseRepository<TEntity, TKey> Repository { get => Constructor.GetRepository<TEntity, TKey>(); }

    public BaseService(IRepositoryConstructor constructor)
    {
        Constructor = constructor;
    }

    public virtual async Task<Result<TResult>> AdvanceQueryAsync<TResult>(TResult result, Func<IRepositoryConstructor, Task<Result<TResult>>> operation) where TResult : class => await operation(Constructor);

    public virtual async Task<TEntity> GetByIDAsync(TKey id)
    {
        return await Repository.GetByIDAsync(id);
    }

    public virtual async Task<List<TEntity>> GetAllAsync()
    {
        return (await Repository.GetAllAsync()).ToList();
    }

    public virtual async Task<Result<Object>> CreateAsync(TEntity entity)
    {
        try
        {
            var result = await ValidateOnCreateAsync(entity);
            if (result.IsSuccess)
            {
                await Repository.InsertAsync(entity);
                await Repository.CommitChangesAsync();
                return Result.Instance().Success($"{typeof(TEntity).Name} inserted successfully !");
            }
            else
            {
                return Result.Instance().Fail($"Fail on creating the new {typeof(TEntity).Name}", result);
            }
        }
        catch (Exception ex)
        {
            return Result.Instance().Fail($"These occurred an error while creating the new {typeof(TEntity).Name}", ex);
        }
    }

    public virtual async Task<Result<Object>> CreateRangeAsync(IEnumerable<TEntity> entities)
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

    public virtual async Task<Result<Object>> DeleteAsync(TKey id)
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
                    r = Result.Instance().Fail("This entity cannot be deleted", result);

                }
            }
            return r;
        }
        catch (Exception ex)
        {
            return Result.Instance().Fail("Error deleting this entity from DB", ex);
        }
    }

    public virtual async Task<Result<Object>> DeleteAsync(TEntity entity)
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
                return Result.Instance().Fail("This entity cannot be deleted", result);
            }
        }
        catch (Exception ex)
        {
            return Result.Instance().Fail("Error deleting this entity from DB", ex);
        }
    }

    public virtual async Task<Result<Object>> DeleteRangeAsync(IEnumerable<TEntity> entities)
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
                return Result.Instance().Fail("This entity list cannot be deleted", result);
            }
        }
        catch (Exception ex)
        {
            return Result.Instance().Fail("Error on delete the list of entities", ex);
        }
    }

    public virtual async Task<Result<Object>> UpdateAsync(TEntity entity)
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

    public virtual async Task<Result<Object>> UpdateRangeAsync(IEnumerable<TEntity> entities)
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
                result = Result.Instance().Fail($"error when try to eliminate {typeof(TEntity).Name} entity list", result);
            }
            else
            {
                await Repository.UpdateRangeAsync(entities);
                await Repository.CommitChangesAsync();
                result = Result.Instance().Success($"list of {typeof(TEntity).Name} entities update succefully !");
            }
            return result;
        }
        catch (Exception ex)
        {
            return Result.Instance().Fail($"error when try to update the list of {typeof(TEntity)} entities in database", ex);
        }
    }

    public virtual async Task<Result<Object>> GenerateSeeders(int amount, object? referenceId)
    {
        var e = (ISeeder<TEntity, TKey>)Activator.CreateInstance<TEntity>();

        var generatedData = e.SeederDefinition(referenceId).Generate(amount);

        return await CreateRangeAsync(generatedData);
    }

    public virtual async Task<Result<Object>> ValidateOnDeleteAsync(TEntity entity)
    {
        var found = await GetByIDAsync(entity.Id);
        if (found == null)
        {
            return Result.Instance().Fail($"This {entity.GetType().Name} cannot be found in database");
        }
        else
        {
            return Result.Instance().Success($"{entity.GetType().Name} founded !");
        }
    }

    public virtual async Task<Result<Object>> ValidateOnUpdateAsync(TEntity entity)
    {
        var found = await GetByIDAsync(entity.Id);
        if (found == null)
        {
            return Result.Instance().Fail($"This {entity.GetType().Name} cannot be found in database");
        }
        else
        {
            return Result.Instance().Success($"{entity.GetType().Name} founded !");
        }
    }

    public abstract Task<Result<Object>> ValidateOnCreateAsync(TEntity entity);
}


public abstract class BaseCustonService<TRepository, TEntity, TKey> : IBaseService<TEntity, TKey> where TRepository : IBaseRepository<TEntity, TKey> where TEntity : class, IBaseEntity<TKey>, ISeeder<TEntity, TKey> where TKey : IEquatable<TKey>
{
    protected readonly IRepositoryConstructor Constructor;

    protected BaseCustonService(IRepositoryConstructor _constructor)
    {
        Constructor = _constructor;
    }

    protected TRepository Repository { get => Constructor.GetRepositoryImplementation<TRepository, TEntity, TKey>(); }

    public virtual async Task<Result<TResult>> AdvanceQueryAsync<TResult>(TResult result, Func<IRepositoryConstructor, Task<Result<TResult>>> operation) where TResult : class => await operation(Constructor);

    public virtual async Task<TEntity> GetByIDAsync(TKey id)
    {
        return await Repository.GetByIDAsync(id);
    }

    public virtual async Task<List<TEntity>> GetAllAsync()
    {
        return (await Repository.GetAllAsync()).ToList();
    }

    public virtual async Task<Result<Object>> CreateAsync(TEntity entity)
    {
        try
        {
            if ((await ValidateOnCreateAsync(entity)).IsSuccess)
            {
                await Repository.InsertAsync(entity);
                await Repository.CommitChangesAsync();
                return Result.Instance().Success($"{typeof(TEntity).Name} inserted successfully !");
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

    public virtual async Task<Result<Object>> CreateRangeAsync(IEnumerable<TEntity> entities)
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

    public virtual async Task<Result<Object>> DeleteAsync(TKey id)
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

    public virtual async Task<Result<Object>> DeleteAsync(TEntity entity)
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

    public virtual async Task<Result<Object>> DeleteRangeAsync(IEnumerable<TEntity> entities)
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

    public virtual async Task<Result<Object>> UpdateAsync(TEntity entity)
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

    public virtual async Task<Result<Object>> UpdateRangeAsync(IEnumerable<TEntity> entities)
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
                result = Result.Instance().Fail($"error when try to eliminate {typeof(TEntity).Name} entity list", result);
            }
            else
            {
                await Repository.UpdateRangeAsync(entities);
                await Repository.CommitChangesAsync();
                result = Result.Instance().Success($"list of {typeof(TEntity).Name} entities update succefully !");
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

        var generatedData = Activator.CreateInstance<TEntity>().SeederDefinition(referenceId).Generate(amount);

        return await CreateRangeAsync(generatedData);
    }

    public virtual async Task<Result<Object>> ValidateOnDeleteAsync(TEntity entity)
    {
        var found = await GetByIDAsync(entity.Id);
        if (found == null)
        {
            return Result.Instance().Fail($"This {entity.GetType().Name} cannot be found in database");
        }
        else
        {
            return Result.Instance().Success($"{entity.GetType().Name} founded !");
        }
    }

    public virtual async Task<Result<Object>> ValidateOnUpdateAsync(TEntity entity)
    {
        var found = await GetByIDAsync(entity.Id);
        if (found == null)
        {
            return Result.Instance().Fail($"This {entity.GetType().Name} cannot be found in database");
        }
        else
        {
            return Result.Instance().Success($"{entity.GetType().Name} founded !");
        }
    }

    public abstract Task<Result<Object>> ValidateOnCreateAsync(TEntity entity);
}

public interface IBaseService<TEntity, TKey, T> where TEntity : class, IBaseEntity<TKey>, ISeeder<TEntity, TKey, T> where TKey : IEquatable<TKey>
{
    Task<Result<Object>> ValidateOnCreateAsync(TEntity entity);
    Task<Result<Object>> ValidateOnUpdateAsync(TEntity entity);
    Task<Result<Object>> ValidateOnDeleteAsync(TEntity entity);

    Task<List<TEntity>> GetAllAsync();
    Task<TEntity> GetByIDAsync(TKey id);
    Task<Result<Object>> UpdateAsync(TEntity entity);
    Task<Result<Object>> UpdateRangeAsync(IEnumerable<TEntity> entities);
    Task<Result<Object>> CreateAsync(TEntity entity);
    Task<Result<Object>> CreateRangeAsync(IEnumerable<TEntity> entities);
    Task<Result<Object>> DeleteAsync(TKey id);
    Task<Result<Object>> DeleteAsync(TEntity entity);
    Task<Result<Object>> DeleteRangeAsync(IEnumerable<TEntity> entities);

    Task<Result<TResult>> AdvanceQueryAsync<TResult>(TResult result, Func<IRepositoryConstructor, Task<Result<TResult>>> operation) where TResult : class;

    Task<Result<Object>> GenerateSeeders(int amount, T data);
}

public abstract class BaseService<TEntity, TKey, T> : IBaseService<TEntity, TKey, T> where TEntity : class, IBaseEntity<TKey>, ISeeder<TEntity, TKey, T> where TKey : IEquatable<TKey>
{
    protected readonly IRepositoryConstructor Constructor;
    protected IBaseRepository<TEntity, TKey> Repository { get => Constructor.GetRepository<TEntity, TKey>(); }

    public BaseService(IRepositoryConstructor constructor)
    {
        Constructor = constructor;
    }

    public virtual async Task<Result<TResult>> AdvanceQueryAsync<TResult>(TResult result, Func<IRepositoryConstructor, Task<Result<TResult>>> operation) where TResult : class => await operation(Constructor);

    public virtual async Task<TEntity> GetByIDAsync(TKey id)
    {
        return await Repository.GetByIDAsync(id);
    }

    public virtual async Task<List<TEntity>> GetAllAsync()
    {
        return (await Repository.GetAllAsync()).ToList();
    }

    public virtual async Task<Result<Object>> CreateAsync(TEntity entity)
    {
        try
        {
            if ((await ValidateOnCreateAsync(entity)).IsSuccess)
            {
                await Repository.InsertAsync(entity);
                await Repository.CommitChangesAsync();
                return Result.Instance().Success($"{typeof(TEntity).Name} inserted successfully !");
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

    public virtual async Task<Result<Object>> CreateRangeAsync(IEnumerable<TEntity> entities)
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

    public virtual async Task<Result<Object>> DeleteAsync(TKey id)
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

    public virtual async Task<Result<Object>> DeleteAsync(TEntity entity)
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

    public virtual async Task<Result<Object>> DeleteRangeAsync(IEnumerable<TEntity> entities)
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

    public virtual async Task<Result<Object>> UpdateAsync(TEntity entity)
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

    public virtual async Task<Result<Object>> UpdateRangeAsync(IEnumerable<TEntity> entities)
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
                result = Result.Instance().Fail($"error when try to eliminate {typeof(TEntity).Name} entity list", result);
            }
            else
            {
                await Repository.UpdateRangeAsync(entities);
                await Repository.CommitChangesAsync();
                result = Result.Instance().Success($"list of {typeof(TEntity).Name} entities update succefully !");
            }
            return result;
        }
        catch (Exception ex)
        {
            return Result.Instance().Fail($"error when try to update the list of {typeof(TEntity)} entities in database", ex);
        }
    }

    public virtual async Task<Result<Object>> GenerateSeeders(int amount, T data)
    {
        var e = (ISeeder<TEntity, TKey, T>)Activator.CreateInstance<TEntity>();

        var generatedData = e.SeederDefinition(data).Generate(amount);

        return await CreateRangeAsync(generatedData);
    }

    public virtual async Task<Result<Object>> ValidateOnDeleteAsync(TEntity entity)
    {
        var found = await GetByIDAsync(entity.Id);
        if (found == null)
        {
            return Result.Instance().Fail($"This {entity.GetType().Name} cannot be found in database");
        }
        else
        {
            return Result.Instance().Success($"{entity.GetType().Name} founded !");
        }
    }

    public virtual async Task<Result<Object>> ValidateOnUpdateAsync(TEntity entity)
    {
        var found = await GetByIDAsync(entity.Id);
        if (found == null)
        {
            return Result.Instance().Fail($"This {entity.GetType().Name} cannot be found in database");
        }
        else
        {
            return Result.Instance().Success($"{entity.GetType().Name} founded !");
        }
    }

    public abstract Task<Result<Object>> ValidateOnCreateAsync(TEntity entity);
}

public abstract class BaseCustonService<TRepository, TEntity, TKey, T> : IBaseService<TEntity, TKey, T> where TRepository : IBaseRepository<TEntity, TKey> where TEntity : class, IBaseEntity<TKey>, ISeeder<TEntity, TKey, T> where TKey : IEquatable<TKey>
{
    protected readonly IRepositoryConstructor Constructor;

    protected BaseCustonService(IRepositoryConstructor _constructor)
    {
        Constructor = _constructor;
    }

    protected TRepository Repository { get => Constructor.GetRepositoryImplementation<TRepository, TEntity, TKey>(); }

    public virtual async Task<Result<TResult>> AdvanceQueryAsync<TResult>(TResult result, Func<IRepositoryConstructor, Task<Result<TResult>>> operation) where TResult : class => await operation(Constructor);

    public virtual async Task<TEntity> GetByIDAsync(TKey id)
    {
        return await Repository.GetByIDAsync(id);
    }

    public virtual async Task<List<TEntity>> GetAllAsync()
    {
        return (await Repository.GetAllAsync()).ToList();
    }

    public virtual async Task<Result<Object>> CreateAsync(TEntity entity)
    {
        try
        {
            if ((await ValidateOnCreateAsync(entity)).IsSuccess)
            {
                await Repository.InsertAsync(entity);
                await Repository.CommitChangesAsync();
                return Result.Instance().Success($"{typeof(TEntity).Name} inserted successfully !");
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

    public virtual async Task<Result<Object>> CreateRangeAsync(IEnumerable<TEntity> entities)
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

    public virtual async Task<Result<Object>> DeleteAsync(TKey id)
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

    public virtual async Task<Result<Object>> DeleteAsync(TEntity entity)
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

    public virtual async Task<Result<Object>> DeleteRangeAsync(IEnumerable<TEntity> entities)
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

    public virtual async Task<Result<Object>> UpdateAsync(TEntity entity)
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

    public virtual async Task<Result<Object>> UpdateRangeAsync(IEnumerable<TEntity> entities)
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
                result = Result.Instance().Fail($"error when try to eliminate {typeof(TEntity).Name} entity list", result);
            }
            else
            {
                await Repository.UpdateRangeAsync(entities);
                await Repository.CommitChangesAsync();
                result = Result.Instance().Success($"list of {typeof(TEntity).Name} entities update succefully !");
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

        var generatedData = Activator.CreateInstance<TEntity>().SeederDefinition(data).Generate(amount);

        return await CreateRangeAsync(generatedData);
    }

    public virtual async Task<Result<Object>> ValidateOnDeleteAsync(TEntity entity)
    {
        var found = await GetByIDAsync(entity.Id);
        if (found == null)
        {
            return Result.Instance().Fail($"This {entity.GetType().Name} cannot be found in database");
        }
        else
        {
            return Result.Instance().Success($"{entity.GetType().Name} founded !");
        }
    }

    public virtual async Task<Result<Object>> ValidateOnUpdateAsync(TEntity entity)
    {
        var found = await GetByIDAsync(entity.Id);
        if (found == null)
        {
            return Result.Instance().Fail($"This {entity.GetType().Name} cannot be found in database");
        }
        else
        {
            return Result.Instance().Success($"{entity.GetType().Name} founded !");
        }
    }

    public abstract Task<Result<Object>> ValidateOnCreateAsync(TEntity entity);
}
