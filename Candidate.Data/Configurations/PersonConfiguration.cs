using Candidate.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Candidate.Data.Configurations;

public class PersonConfiguration : IEntityTypeConfiguration<Person>
{
    public void Configure(EntityTypeBuilder<Person> builder)
    {
        builder.ToTable(name: "People");

        builder.HasKey(x => x.ID);
        builder.Property(x => x.FirstName).IsRequired().HasMaxLength(50);
        builder.Property(x => x.LastName).IsRequired().HasMaxLength(50);
        builder.Property(x => x.PersonCode).IsRequired().HasMaxLength(50);
        builder.Property(x => x.Date).IsRequired().HasMaxLength(10);

        

        builder.Property(x => x.IsActive).IsRequired();
        builder.Property(x => x.IsDeleted).IsRequired();
        builder.Property(x => x.CreationDateTime).IsRequired().HasColumnType("DateTime");
        builder.Property(x => x.ModificationDateTime).HasColumnType("DateTime");

    }
}