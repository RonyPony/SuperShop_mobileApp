using System.Linq.Expressions;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using superShop_API.Database.Entities.Base;

namespace superShop_API.Database.Repositories.Base;

public interface IBaseRepository<TEntity, TKey> where TEntity : class, IBaseEntity<TKey> where TKey : IEquatable<TKey>
{
    Task<int> CommitChangesAsync();
    Task<TEntity> GetByIDAsync(TKey id);
    Task<IQueryable<TEntity>> GetAsync(Expression<Func<TEntity, bool>> where, string includeProperties = "");
    Task<IQueryable<TEntity>> GetAsync(Expression<Func<TEntity, bool>> where, params Expression<Func<TEntity, object>>[] include);
    Task<IQueryable<TEntity>> GetAsync(params Expression<Func<TEntity, object>>[] include);
    Task<IQueryable<TEntity>> GetAllAsync();
    Task<int> CountAsync();
    Task<TEntity> InsertAsync(TEntity entity);
    Task<TEntity> UpdateAsync(TEntity entity);
    Task<TEntity> UpdateAsync(TEntity entity, TKey id);
    Task UpdatePropertyAsync<Type>(Expression<Func<TEntity, Type>> property, TEntity entity);
    Task DeleteAsync(TEntity Entity);
    Task DeleteAsync(TKey id);
    Task DeleteAsync(Expression<Func<TEntity, bool>> primaryKeys);
    Task<NpgsqlDataReader> RunAsync(string query);
    Task DeleteRangeAsync(IEnumerable<TEntity> entities);
    Task<IEnumerable<TEntity>> InsertRangeAsync(IEnumerable<TEntity> entities);
    Task<IEnumerable<TEntity>> UpdateRangeAsync(IEnumerable<TEntity> entities);
}
public class BaseRepository<TEntity, TKey> : IBaseRepository<TEntity, TKey> where TEntity : class, IBaseEntity<TKey> where TKey : IEquatable<TKey>
{
    protected readonly DatabaseContext Context;

    public BaseRepository(DatabaseContext context)
    {
        Context = context;
    }

    public async Task<int> CommitChangesAsync()
    {
        return await Context.SaveChangesAsync();
    }

    public virtual async Task<TEntity> GetByIDAsync(TKey id)
    {
        return await Context.Set<TEntity>().FindAsync(id);
    }

    public virtual async Task<IQueryable<TEntity>> GetAsync(Expression<Func<TEntity, bool>> where, string includeProperties = "")
    {
        return await Task.Run(() =>
        {
            var query = Context.Set<TEntity>().AsQueryable();

            foreach (var includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            if (where != null)
                query = query.AsExpandable().Where(where);

            return query;
        });
    }

    public virtual async Task<IQueryable<TEntity>> GetAsync(Expression<Func<TEntity, bool>> @where, params Expression<Func<TEntity, object>>[] include)
    {
        return await Task.Run(() =>
        {
            var query = Context.Set<TEntity>().AsQueryable();

            foreach (var includeProperty in include)
            {
                query = query.Include(includeProperty);
            }

            if (where != null)
                query = query.AsExpandable().Where(where);

            return query;
        });
    }

    public virtual async Task<IQueryable<TEntity>> GetAsync(params Expression<Func<TEntity, object>>[] include)
    {
        return await Task.Run(() =>
        {
            var query = Context.Set<TEntity>().AsQueryable().AsExpandable();

            foreach (var includeProperty in include)
            {
                query = query.Include(includeProperty);
            }

            return query;
        });
    }

    public virtual async Task<IQueryable<TEntity>> GetAllAsync()
    {
        return await Task.Run(() => Context.Set<TEntity>().AsQueryable());
    }

    public virtual async Task<int> CountAsync()
    {
        return await Task.Run(() => Context.Set<TEntity>().Count());
    }

    public virtual async Task<TEntity> InsertAsync(TEntity entity)
    {
        await Context.Set<TEntity>().AddAsync(entity);
        return entity;
    }

    public virtual async Task<IEnumerable<TEntity>> InsertRangeAsync(IEnumerable<TEntity> entity)
    {
        await Context.Set<TEntity>().AddRangeAsync(entity);
        return entity;
    }

    public virtual async Task<TEntity> UpdateAsync(TEntity entity)
    {
        return await Task.Run(() =>
        {
            Context.Set<TEntity>().Attach(entity);
            Context.Entry(entity).State = EntityState.Modified;

            return entity;
        });
    }

    public virtual async Task<TEntity> UpdateAsync(TEntity entity, TKey id)
    {
        var entry = Context.Entry(entity);

        if (entry.State == EntityState.Detached)
        {
            var attachedEntity = await GetByIDAsync(id);

            if (attachedEntity != null)
            {
                var attachedEntry = Context.Entry(attachedEntity);
                attachedEntry.CurrentValues.SetValues(entity);
            }
            else
            {
                entry.State = EntityState.Modified;
            }
        }
        return entity;
    }

    public virtual async Task<IEnumerable<TEntity>> UpdateRangeAsync(IEnumerable<TEntity> entities)
    {
        return await Task.Run(() =>
        {
            var result = new List<TEntity>();
            foreach (var entity in entities)
            {
                Context.Set<TEntity>().Attach(entity);
                Context.Entry(entity).State = EntityState.Modified;
                result.Add(entity);
            }
            return result;
        });
    }

    public virtual async Task UpdatePropertyAsync<Type>(Expression<Func<TEntity, Type>> property, TEntity entity)
    {
        Context.Set<TEntity>().Attach(entity);
        Context.Entry(entity).Property(property).IsModified = true;
        await Context.SaveChangesAsync();
    }

    public virtual async Task DeleteAsync(TEntity entity)
    {
        await Task.Run(() => Context.Set<TEntity>().Remove(entity));
    }

    public virtual async Task DeleteRangeAsync(IEnumerable<TEntity> entity)
    {
        await Task.Run(() => Context.Set<TEntity>().RemoveRange(entity));
    }


    public virtual async Task DeleteAsync(TKey id)
    {
        var entity = await GetByIDAsync(id);
        await DeleteAsync(entity);
    }

    public virtual async Task DeleteAsync(Expression<Func<TEntity, bool>> primaryKeys)
    {
        var entity = (await GetAsync(primaryKeys)).FirstOrDefault();
        await DeleteAsync(entity);
    }

    public virtual async Task<NpgsqlDataReader> RunAsync(string query)
    {
        var connection = Context.Database.GetDbConnection();

        var conn = new NpgsqlConnection(connection.ConnectionString);

        using var command = new NpgsqlCommand(query, conn);
        await conn.OpenAsync();

        return await command.ExecuteReaderAsync();
    }
}