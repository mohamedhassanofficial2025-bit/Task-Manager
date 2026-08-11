using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using TaskManager.Core.Entities;
using TaskManager.Infrastructure.Data;

namespace TaskManager.Infrastructure.Configurations;

public class ProjectConfiguration : IEntityTypeConfiguration<Project>
{
    public void Configure(EntityTypeBuilder<Project> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Name)
               .IsRequired()
               .HasMaxLength(100);

        builder.Property(x => x.Description)
               .HasMaxLength(500);

        builder.Property(x => x.CreatedAt)
               .IsRequired();

        builder.Property(x => x.UserId)
               .IsRequired();

        // Ownership relationship: Project belongs to a User
        builder.HasOne<AppUser>()
               .WithMany(u => u.Projects)
               .HasForeignKey(x => x.UserId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(x => x.Tasks)
               .WithOne(x => x.Project)
               .HasForeignKey(x => x.ProjectId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}

