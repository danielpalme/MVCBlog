using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MVCBlog.Data.Validation;

namespace MVCBlog.Data;

public class EFUnitOfWork : IdentityDbContext<User>
{
    public EFUnitOfWork(DbContextOptions<EFUnitOfWork> options)
        : base(options)
    {
    }

    public DbSet<BlogEntry> BlogEntries { get; set; } = null!;

    public DbSet<BlogEntryComment> BlogEntryComments { get; set; } = null!;

    public DbSet<BlogEntryFile> BlogEntryFiles { get; set; } = null!;

    public DbSet<Image> Images { get; set; } = null!;

    public DbSet<Tag> Tags { get; set; } = null!;

    public DbSet<BlogEntryTag> BlogEntryTags { get; set; } = null!;

    public override int SaveChanges()
    {
        this.ValidateEntitíes();

        return base.SaveChanges();
    }

    public override int SaveChanges(bool acceptAllChangesOnSuccess)
    {
        this.ValidateEntitíes();

        return base.SaveChanges(acceptAllChangesOnSuccess);
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken))
    {
        this.ValidateEntitíes();

        return base.SaveChangesAsync(cancellationToken);
    }

    public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default(CancellationToken))
    {
        this.ValidateEntitíes();

        return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
        {
            relationship.DeleteBehavior = DeleteBehavior.Cascade;
        }

        modelBuilder.Entity<BlogEntryTag>()
            .HasKey(m => new { m.BlogEntryId, m.TagId });

        modelBuilder.Entity<BlogEntry>()
            .HasIndex(m => new { m.Permalink })
            .IsUnique(true);

        modelBuilder.Entity<Tag>()
            .HasIndex(m => new { m.Name })
            .IsUnique(true);

        base.OnModelCreating(modelBuilder);
    }

    private void ValidateEntitíes()
    {
        var addedOrModifiedEntities = this.ChangeTracker.Entries()
            .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified);

        var errors = new List<EntityValidationResult>();
        var validationResults = new List<ValidationResult>();
        foreach (var entity in addedOrModifiedEntities)
        {
            if (!Validator.TryValidateObject(entity.Entity, new ValidationContext(entity.Entity), validationResults))
            {
                errors.Add(new EntityValidationResult(entity.Entity, validationResults));
                validationResults = new List<ValidationResult>();
            }
        }

        if (errors.Count > 0)
        {
            throw new Validation.ValidationException(errors);
        }
    }
}