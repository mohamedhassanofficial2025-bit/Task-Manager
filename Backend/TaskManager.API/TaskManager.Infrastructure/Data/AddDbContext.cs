using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using TaskManager.Core.Entities;
using System.Text;

namespace TaskManager.Infrastructure.Data;

public class AddDbContext : IdentityDbContext<AppUser, AppRole, string>
{
    public AddDbContext(DbContextOptions<AddDbContext> options) : base(options)
    {
        
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Configure Identity tables first
        base.OnModelCreating(modelBuilder);
        // Apply custom entity configurations
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AddDbContext).Assembly);
    }

    public DbSet<Project> Projects { get; set; } = null!;
    public DbSet<TaskItem> TaskItems { get; set; } = null!;
}

