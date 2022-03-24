using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using superShop_API.Database.Entities;
using superShop_API.Database.Entities.Auth;
using superShop_API.Database.Entities.Base;


namespace superShop_API.Database;
public class DatabaseContext : IdentityDbContext<User, Role, Guid>
{
    public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
    {
        Database.EnsureCreated();
    }

    public DbSet<Mall> Malls { get; set; }
    public DbSet<Branch> Branches { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<ProductOrder> ProductOrders { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<Mall>().Property(m => m.Coordinates).HasConversion(new CoordinatesCorverter());
        builder.Entity<Mall>().Navigation(m => m.Branches).AutoInclude();

        builder.Entity<Branch>().HasOne(b => b.Mall).WithMany(m => m.Branches).HasForeignKey(b => b.MallId).OnDelete(DeleteBehavior.Restrict);
        builder.Entity<Branch>().HasOne(b => b.Category).WithMany(c => c.Branches).HasForeignKey(b => b.CategoryId).OnDelete(DeleteBehavior.Restrict);
        builder.Entity<Branch>().Navigation(b => b.Category).AutoInclude();
        builder.Entity<Branch>().Navigation(b => b.Mall).AutoInclude();

        builder.Entity<Order>().HasOne(o => o.Branch).WithMany(b => b.Orders).HasForeignKey(o => o.BranchId).OnDelete(DeleteBehavior.Restrict);
        builder.Entity<Order>().HasOne(o => o.User).WithMany(u => u.Orders).HasForeignKey(o => o.UserId).OnDelete(DeleteBehavior.Restrict);
        builder.Entity<Order>().Navigation(b => b.Branch).AutoInclude();
        builder.Entity<Order>().Navigation(b => b.ProductOrders).AutoInclude();

        builder.Entity<Product>().HasOne(p => p.Branch).WithMany(b => b.Products).HasForeignKey(p => p.BranchId).OnDelete(DeleteBehavior.Restrict);

        builder.Entity<ProductOrder>().HasOne<Order>(po => po.Order).WithMany(o => o.ProductOrders).HasForeignKey(po => po.OrderId).OnDelete(DeleteBehavior.ClientCascade);
        builder.Entity<ProductOrder>().HasOne<Product>(po => po.Product).WithMany(p => p.ProductOrders).HasForeignKey(po => po.ProductId).OnDelete(DeleteBehavior.Restrict);
        base.OnModelCreating(builder);
    }

    public override int SaveChanges(bool acceptAllChangesOnSuccess)
    {
        OnBeforeSaving();
        return base.SaveChanges(acceptAllChangesOnSuccess);
    }

    public override async Task<int> SaveChangesAsync(
       bool acceptAllChangesOnSuccess,
       CancellationToken cancellationToken = default(CancellationToken)
    )
    {
        OnBeforeSaving();
        return (await base.SaveChangesAsync(acceptAllChangesOnSuccess,
                      cancellationToken));
    }

    private void OnBeforeSaving()
    {
        var entries = ChangeTracker.Entries();
        var utcNow = DateTime.UtcNow;

        foreach (var entry in entries)
        {
            // for entities that inherit from BaseEntity,
            // set UpdatedOn / CreatedOn appropriately
            if (entry.Entity is BaseEntity<Guid> trackable)
            {
                switch (entry.State)
                {
                    case EntityState.Modified:
                        // set the updated date to "now"
                        trackable.UpdatedAt = utcNow;

                        // mark property as "don't touch"
                        // we don't want to update on a Modify operation
                        entry.Property("CreatedAt").IsModified = false;
                        break;

                    case EntityState.Added:
                        // set both updated and created date to "now"
                        trackable.CreatedAt = utcNow;
                        trackable.UpdatedAt = utcNow;
                        break;
                }
            }
        }
    }
}