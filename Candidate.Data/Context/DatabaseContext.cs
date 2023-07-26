using Candidate.Data.Interfaces;
using Candidate.Data.Models;
using IdentityServer4.EntityFramework.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Candidate.Data.Context;

public class DatabaseContext : DbContext
{
    public DatabaseContext(DbContextOptions options) : base(options)
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        //==================================== For Lazy Loading
        optionsBuilder.UseLazyLoadingProxies();


    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // ====================================================================== Configuration

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(Person).Assembly);

        // ====================================================================== For Filtering of IsDeleted From Data in Lazy Loading

        foreach (var type in modelBuilder.Model.GetEntityTypes())
        {
            if (typeof(IEntity).IsAssignableFrom(type.ClrType))
                modelBuilder.SetSoftDeleteFilter(type.ClrType);
        }


    }
    public DbSet<Person> People { get; set; }
}