using Candidate.Data.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Candidate.Data.Context;

public static class EFFilterExtension
{
    public static void SetSoftDeleteFilter(this ModelBuilder modelBuilder, Type entityType)
    {
        SetSoftDeleteFilterMethod.MakeGenericMethod(entityType)
            .Invoke(null, new object[] { modelBuilder });
    }

    static readonly MethodInfo SetSoftDeleteFilterMethod = typeof(EFFilterExtension)
        .GetMethods(BindingFlags.Public | BindingFlags.Static)
        .Single(t => t.IsGenericMethod && t.Name == "SetSoftDeleteFilter");

    public static void SetSoftDeleteFilter<TEntity>(this ModelBuilder modelBuilder)
        where TEntity : class, IEntity
    {
        modelBuilder.Entity<TEntity>().HasQueryFilter(x => !x.IsDeleted);
    }
}