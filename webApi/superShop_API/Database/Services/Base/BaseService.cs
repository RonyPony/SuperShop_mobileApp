using System.Data.Entity;
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
            var r = await ValidateOnCreateAsync(entity);
            if (r.IsSuccess)
            {
                await Repository.InsertAsync(entity);
                var commitResult = (EntityState)Enum.ToObject(typeof(EntityState), (await Repository.CommitChangesAsync()));
                if (commitResult == EntityState.Detached)
                {
                    r = Result.Instance().Success($"{typeof(TEntity).Name} inserted successfully !");
                }
                else
                {
                    r = Result.Instance().Fail($"The requested entitty '{typeof(TEntity).Name}' cannot be created !");
                }
            }
            else
            {
                r = Result.Instance().Fail($"Fail on creating the new {typeof(TEntity).Name}", r);
            }
            return r;
        }
        catch (Exception ex)
        {
            return Result.Instance().Fail($"These occurred an error while creating the new {typeof(TEntity).Name}", ex);
        }
    }

    public virtual async Task<Result<Object>> CreateRangeAsync(IEnumerable<TEntity> entities)
    {
        var r = Result.Instance();
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
                var commitResult = (EntityState)Enum.ToObject(typeof(EntityState), (await Repository.CommitChangesAsync()));
                if (commitResult == EntityState.Detached)
                {
                    r = Result.Instance().Success($"{typeof(TEntity).Name}s models inserted successfully !");
                }
                else
                {
                    r = Result.Instance().Fail($"The requested entitty list '{typeof(TEntity).Name}' cannot be created !");
                }
            }
            return r;
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
                    if (result.IsSuccess)
                    {
                        await Repository.DeleteAsync(id);
                        var commitResult = (EntityState)Enum.ToObject(typeof(EntityState), (await Repository.CommitChangesAsync()));
                        if (commitResult == EntityState.Unchanged)
                        {
                            r = Result.Instance().Success("entity delete successfully");
                        }
                        else
                        {
                            r = Result.Instance().Fail($"The requested entitty '{typeof(TEntity).Name}' cannot be deleted");
                        }

                    }
                    else
                    {
                        r = Result.Instance().Fail($"These an error while validating this entity ({typeof(TEntity).Name}) for delete !", result);

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
            var r = Result.Instance();
            var ent = await Repository.GetByIDAsync(entity.Id);
            if (ent != null)
            {
                r = await ValidateOnDeleteAsync(entity);
                if (r.IsSuccess)
                {
                    await Repository.DeleteAsync(entity);
                    var commitResult = (EntityState)Enum.ToObject(typeof(EntityState), (await Repository.CommitChangesAsync()));
                    if (commitResult == EntityState.Unchanged)
                    {
                        r = Result.Instance().Success("entity delete successfully");
                    }
                    else
                    {
                        r = Result.Instance().Fail($"The requested entitty '{typeof(TEntity).Name}' cannot be deleted");
                    }
                }
                else
                {
                    r = Result.Instance().Fail($"These an error while validating this entity ({typeof(TEntity).Name}) for delete !", r);

                }
            }
            return r;
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
            var r = Result.Instance();
            foreach (var entity in entities)
            {
                r = await ValidateOnDeleteAsync(entity);
                if (!r.IsSuccess)
                {
                    break;
                }
            }
            if (r.IsSuccess)
            {
                await Repository.DeleteRangeAsync(entities);
                var commitResult = (EntityState)Enum.ToObject(typeof(EntityState), (await Repository.CommitChangesAsync()));
                if (commitResult == EntityState.Unchanged)
                {
                    r = Result.Instance().Success("entity delete successfully");
                }
                else
                {
                    r = Result.Instance().Fail($"The requested entitty '{typeof(TEntity).Name}' cannot be deleted");
                }
            }
            else
            {
                r = Result.Instance().Fail("This entity list cannot be deleted", r);
            }
            return r;
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
            r = await ValidateOnUpdateAsync(entity);
            if (r.IsSuccess)
            {
                await Repository.UpdateAsync(entity);
                var commitResult = (EntityState)Enum.ToObject(typeof(EntityState), (await Repository.CommitChangesAsync()));
                if (commitResult == EntityState.Modified)
                {
                    r = Result.Instance().Success("entity updated successfully");
                }
                else
                {
                    r = Result.Instance().Fail($"The requested entitty '{typeof(TEntity).Name}' cannot be updated !");
                }
            }
            else
            {
                r = Result.Instance().Fail($"There is an error while validate the requested entitty", r);
            }
            return r;
        }
        catch (Exception e)
        {
            return Result.Instance().Fail("These an error when trying to update the requested entity", e);
        }
    }

    public virtual async Task<Result<Object>> UpdateRangeAsync(IEnumerable<TEntity> entities)
    {
        try
        {
            var r = Result.Instance();
            foreach (var entity in entities)
            {
                r = await ValidateOnUpdateAsync(entity);
                if (!r.IsSuccess)
                {
                    break;
                }
            }
            if (!r.IsSuccess)
            {
                r = Result.Instance().Fail($"error when try to eliminate {typeof(TEntity).Name} entity list", r);
            }
            else
            {
                await Repository.UpdateRangeAsync(entities);
                var commitResult = (EntityState)Enum.ToObject(typeof(EntityState), (await Repository.CommitChangesAsync()));
                if (commitResult == EntityState.Modified)
                {
                    r = Result.Instance().Success("entity list updated successfully");
                }
                else
                {
                    r = Result.Instance().Fail($"The requested entitty list '{typeof(TEntity).Name}' cannot be updated !");
                }
            }
            return r;
        }
        catch (Exception e)
        {
            return Result.Instance().Fail($"error when try to update the list of {typeof(TEntity)} entities in database", e);
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
        var r = Result.Instance();
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
                var commitResult = (EntityState)Enum.ToObject(typeof(EntityState), (await Repository.CommitChangesAsync()));
                if (commitResult == EntityState.Detached)
                {
                    r = Result.Instance().Success($"{typeof(TEntity).Name}s models inserted successfully !");
                }
                else
                {
                    r = Result.Instance().Fail($"The requested entitty list '{typeof(TEntity).Name}' cannot be created !");
                }
            }
            return r;
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
                    if (result.IsSuccess)
                    {
                        await Repository.DeleteAsync(id);
                        var commitResult = (EntityState)Enum.ToObject(typeof(EntityState), (await Repository.CommitChangesAsync()));
                        if (commitResult == EntityState.Unchanged)
                        {
                            r = Result.Instance().Success("entity delete successfully");
                        }
                        else
                        {
                            r = Result.Instance().Fail($"The requested entitty '{typeof(TEntity).Name}' cannot be deleted");
                        }

                    }
                    else
                    {
                        r = Result.Instance().Fail($"These an error while validating this entity ({typeof(TEntity).Name}) for delete !", result);

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
            var r = Result.Instance();
            var ent = await Repository.GetByIDAsync(entity.Id);
            if (ent != null)
            {
                r = await ValidateOnDeleteAsync(entity);
                if (r.IsSuccess)
                {
                    await Repository.DeleteAsync(entity);
                    var commitResult = (EntityState)Enum.ToObject(typeof(EntityState), (await Repository.CommitChangesAsync()));
                    if (commitResult == EntityState.Unchanged)
                    {
                        r = Result.Instance().Success("entity delete successfully");
                    }
                    else
                    {
                        r = Result.Instance().Fail($"The requested entitty '{typeof(TEntity).Name}' cannot be deleted");
                    }
                }
                else
                {
                    r = Result.Instance().Fail($"These an error while validating this entity ({typeof(TEntity).Name}) for delete !", r);

                }
            }
            return r;
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
            var r = Result.Instance();
            foreach (var entity in entities)
            {
                r = await ValidateOnDeleteAsync(entity);
                if (!r.IsSuccess)
                {
                    break;
                }
            }
            if (r.IsSuccess)
            {
                await Repository.DeleteRangeAsync(entities);
                var commitResult = (EntityState)Enum.ToObject(typeof(EntityState), (await Repository.CommitChangesAsync()));
                if (commitResult == EntityState.Unchanged)
                {
                    r = Result.Instance().Success("entity delete successfully");
                }
                else
                {
                    r = Result.Instance().Fail($"The requested entitty '{typeof(TEntity).Name}' cannot be deleted");
                }
            }
            else
            {
                r = Result.Instance().Fail("This entity list cannot be deleted", r);
            }
            return r;
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
            r = await ValidateOnUpdateAsync(entity);
            if (r.IsSuccess)
            {
                await Repository.UpdateAsync(entity);
                var commitResult = (EntityState)Enum.ToObject(typeof(EntityState), (await Repository.CommitChangesAsync()));
                if (commitResult == EntityState.Modified)
                {
                    r = Result.Instance().Success("entity updated successfully");
                }
                else
                {
                    r = Result.Instance().Fail($"The requested entitty '{typeof(TEntity).Name}' cannot be updated !");
                }
            }
            else
            {
                r = Result.Instance().Fail($"There is an error while validate the requested entitty", r);
            }
            return r;
        }
        catch (Exception e)
        {
            return Result.Instance().Fail("These an error when trying to update the requested entity", e);
        }
    }

    public virtual async Task<Result<Object>> UpdateRangeAsync(IEnumerable<TEntity> entities)
    {
        try
        {
            var r = Result.Instance();
            foreach (var entity in entities)
            {
                r = await ValidateOnUpdateAsync(entity);
                if (!r.IsSuccess)
                {
                    break;
                }
            }
            if (!r.IsSuccess)
            {
                r = Result.Instance().Fail($"error when try to eliminate {typeof(TEntity).Name} entity list", r);
            }
            else
            {
                await Repository.UpdateRangeAsync(entities);
                var commitResult = (EntityState)Enum.ToObject(typeof(EntityState), (await Repository.CommitChangesAsync()));
                if (commitResult == EntityState.Modified)
                {
                    r = Result.Instance().Success("entity list updated successfully");
                }
                else
                {
                    r = Result.Instance().Fail($"The requested entitty list '{typeof(TEntity).Name}' cannot be updated !");
                }
            }
            return r;
        }
        catch (Exception e)
        {
            return Result.Instance().Fail($"error when try to update the list of {typeof(TEntity)} entities in database", e);
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
            var r = await ValidateOnCreateAsync(entity);
            if (r.IsSuccess)
            {
                await Repository.InsertAsync(entity);
                var commitResult = (EntityState)Enum.ToObject(typeof(EntityState), (await Repository.CommitChangesAsync()));
                if (commitResult == EntityState.Detached)
                {
                    r = Result.Instance().Success($"{typeof(TEntity).Name} inserted successfully !");
                }
                else
                {
                    r = Result.Instance().Fail($"The requested entitty '{typeof(TEntity).Name}' cannot be created !");
                }
            }
            else
            {
                r = Result.Instance().Fail($"Fail on creating the new {typeof(TEntity).Name}", r);
            }
            return r;
        }
        catch (Exception ex)
        {
            return Result.Instance().Fail($"These occurred an error while creating the new {typeof(TEntity).Name}", ex);
        }
    }

    public virtual async Task<Result<Object>> CreateRangeAsync(IEnumerable<TEntity> entities)
    {
        var r = Result.Instance();
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
                var commitResult = (EntityState)Enum.ToObject(typeof(EntityState), (await Repository.CommitChangesAsync()));
                if (commitResult == EntityState.Detached)
                {
                    r = Result.Instance().Success($"{typeof(TEntity).Name}s models inserted successfully !");
                }
                else
                {
                    r = Result.Instance().Fail($"The requested entitty list '{typeof(TEntity).Name}' cannot be created !");
                }
            }
            return r;
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
                    if (result.IsSuccess)
                    {
                        await Repository.DeleteAsync(id);
                        var commitResult = (EntityState)Enum.ToObject(typeof(EntityState), (await Repository.CommitChangesAsync()));
                        if (commitResult == EntityState.Unchanged)
                        {
                            r = Result.Instance().Success("entity delete successfully");
                        }
                        else
                        {
                            r = Result.Instance().Fail($"The requested entitty '{typeof(TEntity).Name}' cannot be deleted");
                        }

                    }
                    else
                    {
                        r = Result.Instance().Fail($"These an error while validating this entity ({typeof(TEntity).Name}) for delete !", result);

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
            var r = Result.Instance();
            var ent = await Repository.GetByIDAsync(entity.Id);
            if (ent != null)
            {
                r = await ValidateOnDeleteAsync(entity);
                if (r.IsSuccess)
                {
                    await Repository.DeleteAsync(entity);
                    var commitResult = (EntityState)Enum.ToObject(typeof(EntityState), (await Repository.CommitChangesAsync()));
                    if (commitResult == EntityState.Unchanged)
                    {
                        r = Result.Instance().Success("entity delete successfully");
                    }
                    else
                    {
                        r = Result.Instance().Fail($"The requested entitty '{typeof(TEntity).Name}' cannot be deleted");
                    }
                }
                else
                {
                    r = Result.Instance().Fail($"These an error while validating this entity ({typeof(TEntity).Name}) for delete !", r);

                }
            }
            return r;
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
            var r = Result.Instance();
            foreach (var entity in entities)
            {
                r = await ValidateOnDeleteAsync(entity);
                if (!r.IsSuccess)
                {
                    break;
                }
            }
            if (r.IsSuccess)
            {
                await Repository.DeleteRangeAsync(entities);
                var commitResult = (EntityState)Enum.ToObject(typeof(EntityState), (await Repository.CommitChangesAsync()));
                if (commitResult == EntityState.Unchanged)
                {
                    r = Result.Instance().Success("entity delete successfully");
                }
                else
                {
                    r = Result.Instance().Fail($"The requested entitty '{typeof(TEntity).Name}' cannot be deleted");
                }
            }
            else
            {
                r = Result.Instance().Fail("This entity list cannot be deleted", r);
            }
            return r;
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
            r = await ValidateOnUpdateAsync(entity);
            if (r.IsSuccess)
            {
                await Repository.UpdateAsync(entity);
                var commitResult = (EntityState)Enum.ToObject(typeof(EntityState), (await Repository.CommitChangesAsync()));
                if (commitResult == EntityState.Modified)
                {
                    r = Result.Instance().Success("entity updated successfully");
                }
                else
                {
                    r = Result.Instance().Fail($"The requested entitty '{typeof(TEntity).Name}' cannot be updated !");
                }
            }
            else
            {
                r = Result.Instance().Fail($"There is an error while validate the requested entitty", r);
            }
            return r;
        }
        catch (Exception e)
        {
            return Result.Instance().Fail("These an error when trying to update the requested entity", e);
        }
    }

    public virtual async Task<Result<Object>> UpdateRangeAsync(IEnumerable<TEntity> entities)
    {
        try
        {
            var r = Result.Instance();
            foreach (var entity in entities)
            {
                r = await ValidateOnUpdateAsync(entity);
                if (!r.IsSuccess)
                {
                    break;
                }
            }
            if (!r.IsSuccess)
            {
                r = Result.Instance().Fail($"error when try to eliminate {typeof(TEntity).Name} entity list", r);
            }
            else
            {
                await Repository.UpdateRangeAsync(entities);
                var commitResult = (EntityState)Enum.ToObject(typeof(EntityState), (await Repository.CommitChangesAsync()));
                if (commitResult == EntityState.Modified)
                {
                    r = Result.Instance().Success("entity list updated successfully");
                }
                else
                {
                    r = Result.Instance().Fail($"The requested entitty list '{typeof(TEntity).Name}' cannot be updated !");
                }
            }
            return r;
        }
        catch (Exception e)
        {
            return Result.Instance().Fail($"error when try to update the list of {typeof(TEntity)} entities in database", e);
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
            var r = await ValidateOnCreateAsync(entity);
            if (r.IsSuccess)
            {
                await Repository.InsertAsync(entity);
                var commitResult = (EntityState)Enum.ToObject(typeof(EntityState), (await Repository.CommitChangesAsync()));
                if (commitResult == EntityState.Detached)
                {
                    r = Result.Instance().Success($"{typeof(TEntity).Name} inserted successfully !");
                }
                else
                {
                    r = Result.Instance().Fail($"The requested entitty '{typeof(TEntity).Name}' cannot be created !");
                }
            }
            else
            {
                r = Result.Instance().Fail($"Fail on creating the new {typeof(TEntity).Name}", r);
            }
            return r;
        }
        catch (Exception ex)
        {
            return Result.Instance().Fail($"These occurred an error while creating the new {typeof(TEntity).Name}", ex);
        }
    }

    public virtual async Task<Result<Object>> CreateRangeAsync(IEnumerable<TEntity> entities)
    {
        var r = Result.Instance();
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
                var commitResult = (EntityState)Enum.ToObject(typeof(EntityState), (await Repository.CommitChangesAsync()));
                if (commitResult == EntityState.Detached)
                {
                    r = Result.Instance().Success($"{typeof(TEntity).Name}s models inserted successfully !");
                }
                else
                {
                    r = Result.Instance().Fail($"The requested entitty list '{typeof(TEntity).Name}' cannot be created !");
                }
            }
            return r;
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
                    if (result.IsSuccess)
                    {
                        await Repository.DeleteAsync(id);
                        var commitResult = (EntityState)Enum.ToObject(typeof(EntityState), (await Repository.CommitChangesAsync()));
                        if (commitResult == EntityState.Unchanged)
                        {
                            r = Result.Instance().Success("entity delete successfully");
                        }
                        else
                        {
                            r = Result.Instance().Fail($"The requested entitty '{typeof(TEntity).Name}' cannot be deleted");
                        }

                    }
                    else
                    {
                        r = Result.Instance().Fail($"These an error while validating this entity ({typeof(TEntity).Name}) for delete !", result);

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
            var r = Result.Instance();
            var ent = await Repository.GetByIDAsync(entity.Id);
            if (ent != null)
            {
                r = await ValidateOnDeleteAsync(entity);
                if (r.IsSuccess)
                {
                    await Repository.DeleteAsync(entity);
                    var commitResult = (EntityState)Enum.ToObject(typeof(EntityState), (await Repository.CommitChangesAsync()));
                    if (commitResult == EntityState.Unchanged)
                    {
                        r = Result.Instance().Success("entity delete successfully");
                    }
                    else
                    {
                        r = Result.Instance().Fail($"The requested entitty '{typeof(TEntity).Name}' cannot be deleted");
                    }
                }
                else
                {
                    r = Result.Instance().Fail($"These an error while validating this entity ({typeof(TEntity).Name}) for delete !", r);

                }
            }
            return r;
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
            var r = Result.Instance();
            foreach (var entity in entities)
            {
                r = await ValidateOnDeleteAsync(entity);
                if (!r.IsSuccess)
                {
                    break;
                }
            }
            if (r.IsSuccess)
            {
                await Repository.DeleteRangeAsync(entities);
                var commitResult = (EntityState)Enum.ToObject(typeof(EntityState), (await Repository.CommitChangesAsync()));
                if (commitResult == EntityState.Unchanged)
                {
                    r = Result.Instance().Success("entity delete successfully");
                }
                else
                {
                    r = Result.Instance().Fail($"The requested entitty '{typeof(TEntity).Name}' cannot be deleted");
                }
            }
            else
            {
                r = Result.Instance().Fail("This entity list cannot be deleted", r);
            }
            return r;
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
            r = await ValidateOnUpdateAsync(entity);
            if (r.IsSuccess)
            {
                await Repository.UpdateAsync(entity);
                var commitResult = (EntityState)Enum.ToObject(typeof(EntityState), (await Repository.CommitChangesAsync()));
                if (commitResult == EntityState.Modified)
                {
                    r = Result.Instance().Success("entity updated successfully");
                }
                else
                {
                    r = Result.Instance().Fail($"The requested entitty '{typeof(TEntity).Name}' cannot be updated !");
                }
            }
            else
            {
                r = Result.Instance().Fail($"There is an error while validate the requested entitty", r);
            }
            return r;
        }
        catch (Exception e)
        {
            return Result.Instance().Fail("These an error when trying to update the requested entity", e);
        }
    }

    public virtual async Task<Result<Object>> UpdateRangeAsync(IEnumerable<TEntity> entities)
    {
        try
        {
            var r = Result.Instance();
            foreach (var entity in entities)
            {
                r = await ValidateOnUpdateAsync(entity);
                if (!r.IsSuccess)
                {
                    break;
                }
            }
            if (!r.IsSuccess)
            {
                r = Result.Instance().Fail($"error when try to eliminate {typeof(TEntity).Name} entity list", r);
            }
            else
            {
                await Repository.UpdateRangeAsync(entities);
                var commitResult = (EntityState)Enum.ToObject(typeof(EntityState), (await Repository.CommitChangesAsync()));
                if (commitResult == EntityState.Modified)
                {
                    r = Result.Instance().Success("entity list updated successfully");
                }
                else
                {
                    r = Result.Instance().Fail($"The requested entitty list '{typeof(TEntity).Name}' cannot be updated !");
                }
            }
            return r;
        }
        catch (Exception e)
        {
            return Result.Instance().Fail($"error when try to update the list of {typeof(TEntity)} entities in database", e);
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
